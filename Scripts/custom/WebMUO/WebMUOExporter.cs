using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Server.Mobiles;
using Server.Guilds;
using Server.Network;
using Server.Accounting;
using Server.Misc;
using Server.MUOTemplates;
using System.Linq;
using Server.Items;

namespace Server.Services
{
    public class MUODataExporter : GenericPersistence
    {
        private static readonly TimeSpan ExportInterval = TimeSpan.FromMinutes(0.5);
        private static readonly string ExportPath = Path.Combine(Core.BaseDirectory, "wwwroot");
        private static readonly string LogPath = Path.Combine(Core.BaseDirectory, "Logs", "muo_data_export.log");
        private static readonly string ConfigPath = Path.Combine(Core.BaseDirectory, "Configuration", "modernuo.json"); // Updated path
        private static DateTime ServerStartTime;
        private static readonly string[] UploadUrls = new[]
        {
            "https://mydomain.com/config.php"
        };
        private static readonly string UploadPassword = "ModernUO!";
        private static int MaxOnline;
        private static string Port = "2593"; // Default port, overridden by modernuo.json
        private const int Version = 0;

        public MUODataExporter() : base("MUOData", 100)
        {
            Log("[MUODataExporter] Instance created.");
        }

        [CallPriority(100)]
        public static void Configure()
        {
            EventSink.ServerStarted += () =>
            {
                ServerStartTime = DateTime.UtcNow;
                Log($"[MUODataExporter] Core.BaseDirectory: {Core.BaseDirectory}");
                // Initialize Port from modernuo.json
                Port = GetServerPortFromConfig();
                Log($"[MUODataExporter] ServerName set to: {ServerList.ServerName}");
                Log($"[MUODataExporter] Loaded MaxOnline: {MaxOnline}");
                Log($"[MUODataExporter] Loaded Port: {Port}");
                _ = ExportAndUploadData();
            };

            Timer.DelayCall(ExportInterval, ExportInterval, () => _ = ExportAndUploadData());
            Log("[MUODataExporter] Configured.");
        }

        public override void Serialize(IGenericWriter writer)
        {
            writer.Write(Version);
            writer.Write(MaxOnline);
            writer.Write(Port);
            Log($"[MUODataExporter] Serializing MaxOnline: {MaxOnline}, Port: {Port} (Version: {Version})");
        }

        public override void Deserialize(IGenericReader reader)
        {
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    MaxOnline = reader.ReadInt();
                    string deserializedPort = reader.ReadString();
                    Port = deserializedPort ?? "2593"; // Handle null explicitly
                    Log($"[MUODataExporter] Deserialized MaxOnline: {MaxOnline}, Port: {Port} (Version: {version})");
                    break;
                default:
                    Log($"[MUODataExporter] Unknown version {version}, skipping deserialization");
                    break;
            }
        }

        private static string GetServerPortFromConfig()
        {
            try
            {
                Log($"[MUODataExporter] Attempting to read modernuo.json from: {ConfigPath}");
                if (!File.Exists(ConfigPath))
                {
                    Log($"[MUODataExporter] Config file not found at {ConfigPath}, using default port 2593.");
                    return "2593";
                }

                string json = File.ReadAllText(ConfigPath);
                Log($"[MUODataExporter] Successfully read modernuo.json, length: {json.Length} characters.");
                Log($"[MUODataExporter] modernuo.json content: {json.Substring(0, Math.Min(json.Length, 500))}..."); // Log first 500 chars

                using var document = JsonDocument.Parse(json);
                if (!document.RootElement.TryGetProperty("listeners", out var listeners))
                {
                    Log("[MUODataExporter] No 'listeners' property found in modernuo.json, using default port 2593.");
                    return "2593";
                }

                Log($"[MUODataExporter] Found listeners array with {listeners.GetArrayLength()} elements.");
                if (listeners.GetArrayLength() > 0)
                {
                    string listener = listeners[0].GetString();
                    Log($"[MUODataExporter] First listener entry: {listener ?? "null"}");
                    if (!string.IsNullOrEmpty(listener))
                    {
                        // Extract port from "0.0.0.0:2597"
                        string[] parts = listener.Split(':');
                        Log($"[MUODataExporter] Split listener into {parts.Length} parts: {string.Join(", ", parts)}");
                        if (parts.Length == 2 && int.TryParse(parts[1], out int portNum) && portNum >= 1 && portNum <= 65535)
                        {
                            Log($"[MUODataExporter] Valid port extracted: {parts[1]}");
                            return parts[1];
                        }
                        Log($"[MUODataExporter] Invalid port format in listener: {listener}, using default port 2593.");
                    }
                    else
                    {
                        Log("[MUODataExporter] Listener entry is null or empty, using default port 2593.");
                    }
                }
                else
                {
                    Log("[MUODataExporter] Listeners array is empty, using default port 2593.");
                }
                return "2593";
            }
            catch (JsonException ex)
            {
                Log($"[MUODataExporter] JSON parsing error in modernuo.json: {ex.Message}, using default port 2593.");
                return "2593";
            }
            catch (IOException ex)
            {
                Log($"[MUODataExporter] IO error reading modernuo.json: {ex.Message}, using default port 2593.");
                return "2593";
            }
            catch (Exception ex)
            {
                Log($"[MUODataExporter] Unexpected error reading port from modernuo.json: {ex.Message}, StackTrace: {ex.StackTrace}, using default port 2593.");
                return "2593";
            }
        }

        private static async Task ExportAndUploadData()
        {
            try
            {
                Directory.CreateDirectory(ExportPath);
                //Directory.CreateDirectory(Path.GetDirectoryName(LogPath));
                string serverName = ServerList.ServerName;
                int currentOnline = NetState.Instances.Count;

                // Update MaxOnline to be the maximum of the current online count and the stored value
                MaxOnline = Math.Max(currentOnline, MaxOnline);
                Log($"[MUODataExporter] Current Online: {currentOnline}, Updated MaxOnline: {MaxOnline}");
                Log($"[MUODataExporter] Using Port for export: {Port}");

                var players = World.Mobiles.Values
                    .OfType<PlayerMobile>()
                    .Where(p => !p.Deleted)
                    .Select(p => new PlayerInfo
                    {
                        Serial = (int)p.Serial.Value,
                        Name = p.Name,
                        Title = Titles.ComputeTitle(p, p).Replace(p.Name, "").Trim(',', ' ', ':'),
                        Guild = p.Guild?.Name ?? "",
                        GuildAbbr = p.Guild?.Abbreviation ?? "",
                        Kills = p.Kills,
                        Fame = p.Fame,
                        Karma = p.Karma,
                        LastLogin = (p.Account as Account)?.LastLogin ?? DateTime.MinValue,
                        RaceGender = GetRaceGender(p),
                        IsOnline = NetState.Instances.Any(n => n?.Mobile == p),
                        ServerName = serverName,
                        Equipment = GetEquippedItems(p),
                        HighestSkill = GetHighestSkill(p),
                        BodyID = p.Body.BodyID,
                        Hue = p.Hue,
                        Str = p.Str,
                        Dex = p.Dex,
                        Int = p.Int,
                        Stam = p.Stam,
                        Mana = p.Mana,
                        Hits = p.Hits,
                        StatCap = p.StatCap,
                        SkillsTotal = p.SkillsTotal
                    }).ToList();

                Log($"[MUODataExporter] Processing {players.Count} players:");
                foreach (var p in players)
                {
                    Log($"[MUODataExporter] Player: {p.Name}, Serial: {p.Serial}, Str: {p.Str}, Dex: {p.Dex}, Int: {p.Int}, Hits: {p.Hits}, Mana: {p.Mana}, Stam: {p.Stam}, StatCap: {p.StatCap}, SkillsTotal: {p.SkillsTotal}, AccessLevel: {World.Mobiles.Values.OfType<PlayerMobile>().FirstOrDefault(m => m.Name == p.Name)?.AccessLevel}");
                }

                var guilds = World.Mobiles.Values
                    .OfType<PlayerMobile>()
                    .Where(p => p.Guild is Guild)
                    .Select(p => (Guild)p.Guild)
                    .Distinct()
                    .Where(g => !g.Disbanded)
                    .Select(g => new GuildInfo
                    {
                        Name = g.Name,
                        Abbreviation = g.Abbreviation,
                        Type = g.Type.ToString(),
                        Members = g.Members.Count,
                        Kills = g.AcceptedWars.Sum(w => w.Kills),
                        Wars = g.Enemies.Count
                    }).ToList();

                var shardStatus = new ShardStatusInfo
                {
                    Online = true,
                    ServerName = serverName,
                    UptimeMinutes = (int)(DateTime.UtcNow - ServerStartTime).TotalMinutes,
                    PlayerCount = currentOnline,
                    Time = DateTime.UtcNow.ToString("u"),
                    Port = Port, // Use persisted port from config
                    MaxOnline = MaxOnline
                };

                var export = new ExportWrapper
                {
                    Shard = shardStatus,
                    Players = players,
                    Guilds = guilds
                };

                string jsonPath = Path.Combine(ExportPath, "sharddata.json");
                string sqlPath = Path.Combine(ExportPath, "sharddata.sql");
                string json = JsonSerializer.Serialize(export, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, json);
                Log($"[MUODataExporter] Generated sharddata.json at {jsonPath} with Port: {Port}");

                string gumpMappingsPath = Path.Combine(ExportPath, "gump_mappings.json");
                File.WriteAllText(gumpMappingsPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.GumpMappings));
                Log($"[MUODataExporter] Exported gump_mappings.json to {gumpMappingsPath}");

                // Generate styles.css
                string stylesPath = Path.Combine(ExportPath, "styles.css");
                Directory.CreateDirectory(Path.GetDirectoryName(stylesPath));
                File.WriteAllText(stylesPath, StylesCssTemplate.Generate());
                if (!File.Exists(stylesPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy styles.css to {stylesPath}");
                    throw new Exception("Failed to deploy styles.css");
                }
                Log($"[MUODataExporter] Successfully deployed styles.css to {stylesPath}");

                StringBuilder sql = new StringBuilder();

                // Drop specific tables to ensure a clean slate (except maxonline and settings)
                sql.AppendLine("SET FOREIGN_KEY_CHECKS = 0;");
                sql.AppendLine("DROP TABLE IF EXISTS player_equipment;");
                sql.AppendLine("DROP TABLE IF EXISTS players;");
                sql.AppendLine("DROP TABLE IF EXISTS guilds;");
                sql.AppendLine("SET FOREIGN_KEY_CHECKS = 1;");

                // Create players table
                sql.AppendLine("CREATE TABLE players (serial INT, name VARCHAR(50), title VARCHAR(50), guild VARCHAR(50), abbr VARCHAR(10), kills INT, fame INT, karma INT, lastlogin DATETIME, racegender VARCHAR(20), isonline TINYINT(1), servername VARCHAR(50), bodyid INT, hue INT, str INT, dex INT, intel INT, stam INT, mana INT, hits INT, statcap INT, skillstotal INT, PRIMARY KEY (serial));");
                foreach (var p in players)
                {
                    string lastLoginValue = p.LastLogin == DateTime.MinValue ? "NULL" : $"'{p.LastLogin:yyyy-MM-dd HH:mm:ss}'";
                    sql.AppendLine($"INSERT INTO players VALUES ({p.Serial}, '{Escape(p.Name)}', '{Escape(p.Title)}', '{Escape(p.Guild)}', '{Escape(p.GuildAbbr)}', {p.Kills}, {p.Fame}, {p.Karma}, {lastLoginValue}, '{Escape(p.RaceGender)}', {(p.IsOnline ? 1 : 0)}, '{Escape(p.ServerName)}', {p.BodyID}, {p.Hue}, {p.Str}, {p.Dex}, {p.Int}, {p.Stam}, {p.Mana}, {p.Hits}, {p.StatCap}, {p.SkillsTotal});");
                }

                // Create player_equipment table
                sql.AppendLine("CREATE TABLE player_equipment (player_serial INT, layer VARCHAR(20), item_name VARCHAR(50), item_id INT, hue INT, weight FLOAT, physical_resist INT, fire_resist INT, cold_resist INT, poison_resist INT, energy_resist INT, strength_requirement INT, durability_current INT, durability_max INT, properties TEXT, PRIMARY KEY (player_serial, layer), FOREIGN KEY (player_serial) REFERENCES players(serial));");
                foreach (var p in players)
                {
                    foreach (var item in p.Equipment.Items)
                    {
                        string itemName = item.Name != null ? $"'{Escape(item.Name)}'" : "NULL";
                        string properties = item.Properties != null ? $"'{Escape(item.Properties)}'" : "NULL";
                        string weight = item.Weight.HasValue ? item.Weight.Value.ToString() : "NULL";
                        string physicalResist = item.PhysicalResist.HasValue ? item.PhysicalResist.Value.ToString() : "NULL";
                        string fireResist = item.FireResist.HasValue ? item.FireResist.Value.ToString() : "NULL";
                        string coldResist = item.ColdResist.HasValue ? item.ColdResist.Value.ToString() : "NULL";
                        string poisonResist = item.PoisonResist.HasValue ? item.PoisonResist.Value.ToString() : "NULL";
                        string energyResist = item.EnergyResist.HasValue ? item.EnergyResist.Value.ToString() : "NULL";
                        string strengthRequirement = item.StrengthRequirement.HasValue ? item.StrengthRequirement.Value.ToString() : "NULL";
                        string durabilityCurrent = item.DurabilityCurrent.HasValue ? item.DurabilityCurrent.Value.ToString() : "NULL";
                        string durabilityMax = item.DurabilityMax.HasValue ? item.DurabilityMax.Value.ToString() : "NULL";

                        sql.AppendLine($"INSERT INTO player_equipment VALUES ({p.Serial}, '{Escape(item.Layer.ToString())}', {itemName}, {item.ItemID}, {item.Hue}, {weight}, {physicalResist}, {fireResist}, {coldResist}, {poisonResist}, {energyResist}, {strengthRequirement}, {durabilityCurrent}, {durabilityMax}, {properties});");
                    }
                }

                // Create guilds table
                sql.AppendLine("CREATE TABLE guilds (name VARCHAR(50), abbr VARCHAR(10), type VARCHAR(20), members INT, kills INT, wars INT, PRIMARY KEY (name));");
                foreach (var g in guilds)
                {
                    sql.AppendLine($"INSERT INTO guilds VALUES ('{Escape(g.Name)}', '{Escape(g.Abbreviation)}', '{Escape(g.Type)}', {g.Members}, {g.Kills}, {g.Wars});");
                }

                // Create or update maxonline table without dropping it
                sql.AppendLine("CREATE TABLE IF NOT EXISTS maxonline (id INT PRIMARY KEY, value INT);");
                sql.AppendLine($"INSERT INTO maxonline (id, value) VALUES (1, {MaxOnline}) ON DUPLICATE KEY UPDATE value = GREATEST(value, {MaxOnline});");

                // Create settings table without dropping it (to preserve admin settings)
                sql.AppendLine("CREATE TABLE IF NOT EXISTS settings (id INT AUTO_INCREMENT PRIMARY KEY, setting_key VARCHAR(255) NOT NULL UNIQUE, setting_value TEXT NOT NULL);");
                // Insert default settings, excluding wiki
                sql.AppendLine("INSERT INTO settings (setting_key, setting_value) VALUES ('theme', 'theme1') ON DUPLICATE KEY UPDATE setting_value = setting_value;"); // Default to theme1 (Black/Red/Pink)
                sql.AppendLine("INSERT INTO settings (setting_key, setting_value) VALUES ('menu_visibility', '{\"index\": true, \"players\": true, \"guilds\": true, \"social\": true, \"uotools\": true, \"client\": true, \"contact\": true}') ON DUPLICATE KEY UPDATE setting_value = setting_value;");
                sql.AppendLine("INSERT INTO settings (setting_key, setting_value) VALUES ('social_visibility', '{\"discord\": true, \"youtube\": true, \"tiktok\": true, \"twitter\": true, \"facebook\": true}') ON DUPLICATE KEY UPDATE setting_value = setting_value;");

                File.WriteAllText(sqlPath, sql.ToString());
                Log($"[MUODataExporter] Generated sharddata.sql at {sqlPath}");

                var templateConfig = new MUOTemplateWriter.TemplateConfig
                {
                    ServerName = "My MUO Server",
                    Port = Port, // Use persisted port from config
                    UploadPassword = UploadPassword,
                    IncludeOptionalUploads = true,
                    AdminPassword = "admin123"
                };

                // Define file paths and generate PHP files
                string configPhpPath = Path.Combine(ExportPath, "config.php");
                File.WriteAllText(configPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Config, templateConfig));
                if (!File.Exists(configPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy config.php to {configPhpPath}");
                    throw new Exception("Failed to deploy config.php");
                }
                Log($"[MUODataExporter] Successfully deployed config.php to {configPhpPath}");

                string indexPhpPath = Path.Combine(ExportPath, "index.php");
                File.WriteAllText(indexPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Index, templateConfig));
                if (!File.Exists(indexPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy index.php to {indexPhpPath}");
                    throw new Exception("Failed to deploy index.php");
                }
                Log($"[MUODataExporter] Successfully deployed index.php to {indexPhpPath}");

                string playersPhpPath = Path.Combine(ExportPath, "players.php");
                File.WriteAllText(playersPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Players, templateConfig));
                if (!File.Exists(playersPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy players.php to {playersPhpPath}");
                    throw new Exception("Failed to deploy players.php");
                }
                Log($"[MUODataExporter] Successfully deployed players.php to {playersPhpPath}");

                string guildsPhpPath = Path.Combine(ExportPath, "guilds.php");
                File.WriteAllText(guildsPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Guilds, templateConfig));
                if (!File.Exists(guildsPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy guilds.php to {guildsPhpPath}");
                    throw new Exception("Failed to deploy guilds.php");
                }
                Log($"[MUODataExporter] Successfully deployed guilds.php to {guildsPhpPath}");

                string socialPhpPath = Path.Combine(ExportPath, "social.php");
                File.WriteAllText(socialPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Updates, templateConfig));
                if (!File.Exists(socialPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy social.php to {socialPhpPath}");
                    throw new Exception("Failed to deploy social.php");
                }
                Log($"[MUODataExporter] Successfully deployed social.php to {socialPhpPath}");

                string uotoolsPhpPath = Path.Combine(ExportPath, "uotools.php");
                File.WriteAllText(uotoolsPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.UOTools, templateConfig));
                if (!File.Exists(uotoolsPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy uotools.php to {uotoolsPhpPath}");
                    throw new Exception("Failed to deploy uotools.php");
                }
                Log($"[MUODataExporter] Successfully deployed uotools.php to {uotoolsPhpPath}");

                string clientPhpPath = Path.Combine(ExportPath, "client.php");
                File.WriteAllText(clientPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Client, templateConfig));
                if (!File.Exists(clientPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy client.php to {clientPhpPath}");
                    throw new Exception("Failed to deploy client.php");
                }
                Log($"[MUODataExporter] Successfully deployed client.php to {clientPhpPath}");

                string contactPhpPath = Path.Combine(ExportPath, "contact.php");
                File.WriteAllText(contactPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Contact, templateConfig));
                if (!File.Exists(contactPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy contact.php to {contactPhpPath}");
                    throw new Exception("Failed to deploy contact.php");
                }
                Log($"[MUODataExporter] Successfully deployed contact.php to {contactPhpPath}");

                string paperdollPath = Path.Combine(ExportPath, "paperdoll.php");
                File.WriteAllText(paperdollPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Paperdoll, templateConfig));
                if (!File.Exists(paperdollPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy paperdoll.php to {paperdollPath}");
                    throw new Exception("Failed to deploy paperdoll.php");
                }
                Log($"[MUODataExporter] Successfully deployed paperdoll.php to {paperdollPath}");

                string adminPhpPath = Path.Combine(ExportPath, "admin.php");
                File.WriteAllText(adminPhpPath, MUOTemplateWriter.GetTemplate(MUOTemplateWriter.TemplateType.Admin, templateConfig));
                if (!File.Exists(adminPhpPath))
                {
                    Log($"[MUODataExporter] Error: Failed to deploy admin.php to {adminPhpPath}");
                    throw new Exception("Failed to deploy admin.php");
                }
                Log($"[MUODataExporter] Successfully deployed admin.php to {adminPhpPath}");

                Log($"[MUODataExporter] Export complete: {players.Count} players, {guilds.Count} guilds.");
                await UploadFiles(sqlPath, jsonPath, gumpMappingsPath, stylesPath, indexPhpPath, playersPhpPath, guildsPhpPath, socialPhpPath, uotoolsPhpPath, clientPhpPath, contactPhpPath, paperdollPath, adminPhpPath);

                Console.WriteLine("[MUODataExporter] Export successful. Details in muo_data_export.log");
            }
            catch (Exception ex)
            {
                Log($"[MUODataExporter] Export error: {ex.Message}, StackTrace: {ex.StackTrace}");
                Console.WriteLine($"[MUODataExporter] Export failed. See {LogPath} for details.");
            }
        }

        private static async Task UploadFiles(string sqlPath, string jsonPath, string gumpMappingsPath, string stylesPath, string indexPhpPath, string playersPhpPath, string guildsPhpPath, string socialPhpPath, string uotoolsPhpPath, string clientPhpPath, string contactPhpPath, string paperdollPath, string adminPhpPath)
        {
            Log($"[MUODataExporter] Starting upload to {string.Join(", ", UploadUrls)}");
            foreach (var url in UploadUrls)
            {
                Log($"[MUODataExporter] Preparing upload to {url}");
                try
                {
                    using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                    using var form = new MultipartFormDataContent();

                    var files = new[]
                    {
                        (sqlPath, "sqlfile", "sharddata.sql", "application/octet-stream"),
                        (jsonPath, "jsonfile", "sharddata.json", "application/octet-stream"),
                        (gumpMappingsPath, "gumpmapfile", "gump_mappings.json", "application/octet-stream"),
                        (stylesPath, "stylesfile", "styles.css", "application/octet-stream"),
                        (indexPhpPath, "indexfile", "index.php", "application/octet-stream"),
                        (playersPhpPath, "playersfile", "players.php", "application/octet-stream"),
                        (guildsPhpPath, "guildsfile", "guilds.php", "application/octet-stream"),
                        (socialPhpPath, "socialfile", "social.php", "application/octet-stream"),
                        (uotoolsPhpPath, "uotoolsfile", "uotools.php", "application/octet-stream"),
                        (clientPhpPath, "clientfile", "client.php", "application/octet-stream"),
                        (contactPhpPath, "contactfile", "contact.php", "application/octet-stream"),
                        (paperdollPath, "paperdollfile", "paperdoll.php", "application/octet-stream"),
                        (adminPhpPath, "adminfile", "admin.php", "application/octet-stream")
                    };

                    foreach (var (path, field, name, mime) in files)
                    {
                        if (!File.Exists(path))
                        {
                            Log($"[MUODataExporter] File missing: {path}");
                            continue;
                        }
                        byte[] bytes = await File.ReadAllBytesAsync(path);
                        var content = new ByteArrayContent(bytes);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mime);
                        content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = field,
                            FileName = $"\"{name}\""
                        };
                        form.Add(content);
                        Log($"[MUODataExporter] Added file: {name}, Field: {field}, Size: {bytes.Length} bytes");
                    }

                    form.Add(new StringContent(UploadPassword), "key");
                    Log($"[MUODataExporter] Form fields: {string.Join(", ", form.Where(f => f.Headers?.ContentDisposition?.Name != null).Select(f => f.Headers.ContentDisposition.Name))}");

                    Log($"[MUODataExporter] Sending POST to {url}");
                    var response = await client.PostAsync(url, form);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Log($"[MUODataExporter] Response status: {response.StatusCode}, Content: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        Log($"[MUODataExporter] Upload to {url} succeeded: {responseContent}");
                    }
                    else
                    {
                        Log($"[MUODataExporter] Upload to {url} failed: HTTP {response.StatusCode}, Response: {responseContent}");
                    }
                }
                catch (HttpRequestException hre)
                {
                    Log($"[MUODataExporter] HTTP error uploading to {url}: {hre.Message}, InnerException: {hre.InnerException?.Message}");
                }
                catch (IOException ioe)
                {
                    Log($"[MUODataExporter] File IO error uploading to {url}: {ioe.Message}");
                }
                catch (Exception ex)
                {
                    Log($"[MUODataExporter] Unexpected error uploading to {url}: {ex.Message}, StackTrace: {ex.StackTrace}");
                }
            }
            Log("[MUODataExporter] UploadFiles completed");
        }

        private static string Escape(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return input.Replace("'", "''").Replace("\\", "\\\\");
        }

        private static string GetRaceGender(PlayerMobile p)
        {
            if (p.Body.IsGargoyle) return p.Body.IsFemale ? "garg_fem" : "garg_male";
            if (p.Body.BodyID == 605 || p.Body.BodyID == 606) return p.Body.IsFemale ? "elf_fem" : "elf_male";
            return p.Body.IsFemale ? "hum_fem" : "hum_male";
        }

        private static EquipmentList GetEquippedItems(PlayerMobile p)
        {
            var equipment = new EquipmentList();
            var items = p.Items
                .Where(i => i != null && !i.Deleted)
                .Select(i =>
                {
                    var itemProps = new ItemProperties();
                    string properties = "";
                    if (i is BaseArmor armor)
                    {
                        itemProps.Weight = armor.Weight;
                        itemProps.PhysicalResist = armor.PhysicalResistance;
                        itemProps.FireResist = armor.FireResistance;
                        itemProps.ColdResist = armor.ColdResistance;
                        itemProps.PoisonResist = armor.PoisonResistance;
                        itemProps.EnergyResist = armor.EnergyResistance;
                        itemProps.DurabilityCurrent = armor.HitPoints;
                        itemProps.DurabilityMax = armor.MaxHitPoints;

                        var sb = new StringBuilder();
                        if (armor.Weight > 0) sb.Append($"Weight: {armor.Weight} Stones");
                        if (armor.PhysicalResistance > 0) sb.Append($"Physical Resist {armor.PhysicalResistance}%");
                        if (armor.FireResistance > 0) sb.Append($"Fire Resist {armor.FireResistance}%");
                        if (armor.ColdResistance > 0) sb.Append($"Cold Resist {armor.ColdResistance}%");
                        if (armor.PoisonResistance > 0) sb.Append($"Poison Resist {armor.PoisonResistance}%");
                        if (armor.EnergyResistance > 0) sb.Append($"Energy Resist {armor.EnergyResistance}%");
                        if (armor.MaxHitPoints > 0) sb.Append($"Durability {armor.HitPoints} / {armor.MaxHitPoints}");
                        properties = sb.ToString();
                    }
                    else if (i is BaseWeapon weapon)
                    {
                        itemProps.Weight = weapon.Weight;
                        itemProps.DurabilityCurrent = weapon.HitPoints;
                        itemProps.DurabilityMax = weapon.MaxHitPoints;

                        var sb = new StringBuilder();
                        if (weapon.Weight > 0) sb.Append($"Weight: {weapon.Weight} Stones");
                        if (weapon.MaxHitPoints > 0) sb.Append($"Durability {weapon.HitPoints} / {weapon.MaxHitPoints}");
                        properties = sb.ToString();
                    }
                    else
                    {
                        itemProps.Weight = i.Weight;
                        var sb = new StringBuilder();
                        if (i.Weight > 0) sb.Append($"Weight: {i.Weight} Stones");
                        properties = sb.ToString();
                    }

                    return new Item
                    {
                        Name = i.Name ?? i.ItemData.Name ?? i.GetType().Name,
                        ItemID = i.ItemID,
                        Hue = i.Hue,
                        Layer = i.Layer,
                        Properties = properties,
                        Weight = itemProps.Weight,
                        PhysicalResist = itemProps.PhysicalResist,
                        FireResist = itemProps.FireResist,
                        ColdResist = itemProps.ColdResist,
                        PoisonResist = itemProps.PoisonResist,
                        EnergyResist = itemProps.EnergyResist,
                        StrengthRequirement = itemProps.StrengthRequirement,
                        DurabilityCurrent = itemProps.DurabilityCurrent,
                        DurabilityMax = itemProps.DurabilityMax
                    };
                }).ToList();

            if (p.HairItemID > 0)
            {
                items.Add(new Item
                {
                    Name = "Hair",
                    ItemID = p.HairItemID,
                    Hue = p.HairHue,
                    Layer = Layer.Hair,
                    Properties = "Hair"
                });
                Log($"[GetEquippedItems] Added hair for {p.Name}: ItemID={p.HairItemID}, Hue={p.HairHue}");
            }

            if (p.FacialHairItemID > 0)
            {
                items.Add(new Item
                {
                    Name = "Facial Hair",
                    ItemID = p.FacialHairItemID,
                    Hue = p.FacialHairHue,
                    Layer = Layer.FacialHair,
                    Properties = "Facial Hair"
                });
                Log($"[GetEquippedItems] Added facial hair for {p.Name}: ItemID={p.FacialHairItemID}, Hue={p.FacialHairHue}");
            }

            equipment.Items = items;
            return equipment;
        }

        private static Skill GetHighestSkill(PlayerMobile p)
        {
            if (p.Skills == null || p.Skills.Total == 0) return null;
            Server.Skill highest = null;
            double maxBase = -1;
            foreach (var skill in p.Skills)
            {
                if (skill != null && skill.Base > maxBase)
                {
                    highest = skill;
                    maxBase = skill.Base;
                }
            }
            return highest == null ? null : new Skill
            {
                Name = highest.SkillName.ToString(),
                Level = (int)highest.Base / 10,
                Proficiency = highest.Base
            };
        }

        private static void Log(string message)
        {
            try
            {
                File.AppendAllText(LogPath, $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} {message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MUODataExporter] Failed to write to log: {ex.Message}");
            }
        }

        public class PlayerInfo
        {
            public int Serial { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Guild { get; set; }
            public string GuildAbbr { get; set; }
            public int Kills { get; set; }
            public int Fame { get; set; }
            public int Karma { get; set; }
            public DateTime LastLogin { get; set; }
            public string RaceGender { get; set; }
            public bool IsOnline { get; set; }
            public string ServerName { get; set; }
            public EquipmentList Equipment { get; set; } = new EquipmentList();
            public Skill HighestSkill { get; set; }
            public int BodyID { get; set; }
            public int Hue { get; set; }
            public int Str { get; set; }
            public int Dex { get; set; }
            public int Int { get; set; }
            public int Stam { get; set; }
            public int Mana { get; set; }
            public int Hits { get; set; }
            public int StatCap { get; set; }
            public int SkillsTotal { get; set; }
        }

        public class EquipmentList
        {
            public List<Item> Items { get; set; } = new List<Item>();
        }

        public class Item
        {
            public string Name { get; set; }
            public int ItemID { get; set; }
            public int Hue { get; set; }
            public Layer Layer { get; set; }
            public string Properties { get; set; }
            public double? Weight { get; set; }
            public int? PhysicalResist { get; set; }
            public int? FireResist { get; set; }
            public int? ColdResist { get; set; }
            public int? PoisonResist { get; set; }
            public int? EnergyResist { get; set; }
            public int? StrengthRequirement { get; set; }
            public int? DurabilityCurrent { get; set; }
            public int? DurabilityMax { get; set; }
        }

        public class ItemProperties
        {
            public string Properties { get; set; }
            public double? Weight { get; set; }
            public int? PhysicalResist { get; set; }
            public int? FireResist { get; set; }
            public int? ColdResist { get; set; }
            public int? PoisonResist { get; set; }
            public int? EnergyResist { get; set; }
            public int? StrengthRequirement { get; set; }
            public int? DurabilityCurrent { get; set; }
            public int? DurabilityMax { get; set; }
        }

        public class Skill
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public double Proficiency { get; set; }
        }

        public class GuildInfo
        {
            public string Name { get; set; }
            public string Abbreviation { get; set; }
            public string Type { get; set; }
            public int Members { get; set; }
            public int Kills { get; set; }
            public int Wars { get; set; }
        }

        public class ShardStatusInfo
        {
            public bool Online { get; set; }
            public string ServerName { get; set; }
            public int UptimeMinutes { get; set; }
            public int PlayerCount { get; set; }
            public string Time { get; set; }
            public string Port { get; set; }
            public int MaxOnline { get; set; }
        }

        public class ExportWrapper
        {
            public ShardStatusInfo Shard { get; set; }
            public List<PlayerInfo> Players { get; set; }
            public List<GuildInfo> Guilds { get; set; }
        }
    }
}
