using System.Text;

namespace Server.MUOTemplates
{
    public abstract class BasePhpTemplate
    {
        protected readonly string ServerName;
        protected readonly string Port;

        protected BasePhpTemplate(string serverName, string port)
        {
            ServerName = string.IsNullOrEmpty(serverName) ? "My MUO Server" : serverName;
            Port = string.IsNullOrEmpty(port) ? "2593" : port;
        }

        protected string GenerateSidebar(string activePage)
        {
            var sb = new StringBuilder();
            sb.AppendLine("      <h3><b><u>Server Status</u></b></h3>");
            sb.AppendLine("      <?php");
            sb.AppendLine("      if ($jsonData) {");
            sb.AppendLine("          $isOnline = (new DateTime($jsonData['Shard']['Time']))->getTimestamp() > (time() - 45);");
            sb.AppendLine("          echo '<p>Name: ' . htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server') . '</p>';");
            sb.AppendLine("          echo '<p>Status: <span class=\"' . ($isOnline ? 'status-online' : 'status-offline') . '\">' . ($isOnline ? 'Online' : 'Offline') . '</span></p>';");
            sb.AppendLine("          echo '<p>Port: ' . htmlspecialchars($jsonData['Shard']['Port']) . '</p>';");
            sb.AppendLine("          echo '<p>Uptime: ' . ($isOnline ? htmlspecialchars($jsonData['Shard']['UptimeMinutes']) : '0') . ' minutes</p>';");
            sb.AppendLine("          echo '<p>Online Players: ' . ($isOnline ? count(array_filter($jsonData['Players'], fn($p) => $p['IsOnline'])) : '0') . '</p>';");
            sb.AppendLine("          echo '<p>Total Players: ' . count($jsonData['Players']) . '</p>';");
            sb.AppendLine("          echo '<p>Guilds: ' . count($jsonData['Guilds']) . '</p>';");
            sb.AppendLine("          echo '<p>Max Online: ' . htmlspecialchars($jsonData['Shard']['MaxOnline']) . '</p>';");
            sb.AppendLine("      } else {");
            sb.AppendLine("          echo '<p>Name: Unknown Server</p><p>Status: <span class=\\'status-offline\\'>Offline</span></p><p>Port: " + Port + "</p><p>Uptime: 0 minutes</p>';");
            sb.AppendLine("          echo '<p>Online Players: 0</p><p>Total Players: 0</p>';");
            sb.AppendLine("          echo '<p>Guilds: 0</p>';");
            sb.AppendLine("          echo '<p>Max Online: 0</p>';");
            sb.AppendLine("      }");
            sb.AppendLine("      ?>");
            sb.AppendLine("      <div class='menu'>");
            sb.AppendLine("        <ul>");
            sb.AppendLine("        <?php");
            sb.AppendLine("        if ($settings['menu_visibility']['index']) {");
            sb.AppendLine($"            echo \"<li><a href='index.php' class='\" . ($activePage == 'index' ? 'active' : '') . \"'>Home</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if ($settings['menu_visibility']['players']) {");
            sb.AppendLine($"            echo \"<li><a href='players.php' class='\" . ($activePage == 'players' ? 'active' : '') . \"'>Players</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if ($settings['menu_visibility']['guilds']) {");
            sb.AppendLine($"            echo \"<li><a href='guilds.php' class='\" . ($activePage == 'guilds' ? 'active' : '') . \"'>Guilds</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if ($settings['menu_visibility']['social']) {");
            sb.AppendLine($"            echo \"<li><a href='social.php' class='\" . ($activePage == 'social' ? 'active' : '') . \"'>Social</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if ($settings['menu_visibility']['uotools']) {");
            sb.AppendLine($"            echo \"<li><a href='uotools.php' class='\" . ($activePage == 'uotools' ? 'active' : '') . \"'>UO Tools</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if ($settings['menu_visibility']['client']) {");
            sb.AppendLine($"            echo \"<li><a href='client.php' class='\" . ($activePage == 'client' ? 'active' : '') . \"'>Client</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if ($settings['menu_visibility']['contact']) {");
            sb.AppendLine($"            echo \"<li><a href='contact.php' class='\" . ($activePage == 'contact' ? 'active' : '') . \"'>Contact</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        if (isset($_SESSION['admin_logged_in']) && $_SESSION['admin_logged_in']) {");
            sb.AppendLine($"            echo \"<li><a href='admin.php' class='\" . ($activePage == 'admin' ? 'active' : '') . \"'>Admin</a></li>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        ?>");
            sb.AppendLine("        </ul>");
            sb.AppendLine("      </div>");
            return sb.ToString();
        }

        protected void AppendPhpHeader(StringBuilder sb, string errorLogFile, bool fetchPlayers, bool fetchGuilds, MUOTemplateWriter.TemplateConfig config)
        {
            sb.AppendLine("<?php");
            sb.AppendLine("// Configuration");
            sb.AppendLine("ini_set('display_errors', 0);");
            sb.AppendLine("ini_set('log_errors', 1);");
            sb.AppendLine($"ini_set('error_log', __DIR__ . '/{errorLogFile}');");
            sb.AppendLine("error_reporting(E_ALL);");
            sb.AppendLine();
            sb.AppendLine("// Debug logging function");
            sb.AppendLine("function logDebug($message) {");
            sb.AppendLine($"    file_put_contents(__DIR__ . '/{errorLogFile.Replace("errors", "debug")}', date('Y-m-d H:i:s') . ' - ' . $message . \"\\n\", FILE_APPEND);");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("require_once __DIR__ . '/config.php';"); // Fixed path with separator

            // Load settings from the database
            sb.AppendLine();
            sb.AppendLine("// Load admin settings");
            sb.AppendLine("$link = mysqli_connect($dbHost, $dbUser, $dbPass, $dbName, $dbPort);");
            sb.AppendLine("if (!$link) {");
            sb.AppendLine("    logDebug('Settings: Database connection failed: ' . mysqli_connect_error());");
            sb.AppendLine("    $settings = ['theme' => 'dark', 'menu_visibility' => [], 'social_visibility' => []];");
            sb.AppendLine("} else {");
            sb.AppendLine("    $settings = [");
            sb.AppendLine("        'theme' => 'dark',");
            sb.AppendLine("        'menu_visibility' => json_decode('{\"index\": true, \"players\": true, \"guilds\": true, \"social\": true, \"uotools\": true, \"client\": true, \"contact\": true}', true),");
            sb.AppendLine("        'social_visibility' => json_decode('{\"discord\": true, \"youtube\": true, \"tiktok\": true, \"twitter\": true, \"facebook\": true}', true)");
            sb.AppendLine("    ];");
            sb.AppendLine("    $result = mysqli_query($link, 'SELECT setting_key, setting_value FROM settings');");
            sb.AppendLine("    if ($result) {");
            sb.AppendLine("        while ($row = mysqli_fetch_assoc($result)) {");
            sb.AppendLine("            if ($row['setting_key'] === 'theme') {");
            sb.AppendLine("                $settings['theme'] = $row['setting_value'];");
            sb.AppendLine("            } elseif ($row['setting_key'] === 'menu_visibility') {");
            sb.AppendLine("                $settings['menu_visibility'] = json_decode($row['setting_value'], true);");
            sb.AppendLine("            } elseif ($row['setting_key'] === 'social_visibility') {");
            sb.AppendLine("                $settings['social_visibility'] = json_decode($row['setting_value'], true);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            if (fetchPlayers || fetchGuilds)
            {
                sb.AppendLine();
                sb.AppendLine("// Serve JSON if requested");
                sb.AppendLine("if (isset($_GET['json'])) {");
                sb.AppendLine("    $maxOnline = (int)mysqli_fetch_assoc(mysqli_query($link, 'SELECT value FROM maxonline LIMIT 1'))['value'] ?? 0;");

                if (fetchPlayers)
                {
                    sb.AppendLine("    $players = [];");
                    sb.AppendLine("    $result = mysqli_query($link, 'SELECT serial, name, title, guild, abbr, kills, fame, karma, lastlogin, racegender, isonline, servername FROM players');");
                    sb.AppendLine("    if (!$result) {");
                    sb.AppendLine("        logDebug('JSON: Failed to query players: ' . mysqli_error($link));");
                    sb.AppendLine("        die(json_encode(['error' => 'Failed to query players']));");
                    sb.AppendLine("    }");
                    sb.AppendLine("    while ($row = mysqli_fetch_assoc($result)) {");
                    sb.AppendLine("        $serial = (int)$row['serial'];");
                    sb.AppendLine("        if ($serial <= 0) {");
                    sb.AppendLine("            logDebug('JSON: Skipping player with invalid serial - Name: ' . $row['name'] . ', Serial: ' . $serial);");
                    sb.AppendLine("            continue;");
                    sb.AppendLine("        }");
                    sb.AppendLine("        $players[] = [");
                    sb.AppendLine("            'Serial' => $serial,");
                    sb.AppendLine("            'Name' => $row['name'], 'Title' => $row['title'], 'Guild' => $row['guild'], 'GuildAbbr' => $row['abbr'],");
                    sb.AppendLine("            'Kills' => (int)$row['kills'], 'Fame' => (int)$row['fame'], 'Karma' => (int)$row['karma'],");
                    sb.AppendLine("            'LastLogin' => $row['lastlogin'], 'RaceGender' => $row['racegender'], 'IsOnline' => (bool)$row['isonline'],");
                    sb.AppendLine("            'ServerName' => $row['servername']");
                    sb.AppendLine("        ];");
                    sb.AppendLine("    }");
                }
                else
                {
                    sb.AppendLine("    $players = []; // Not fetching players for this endpoint");
                }

                if (fetchGuilds)
                {
                    sb.AppendLine("    $guilds = [];");
                    sb.AppendLine("    $result = mysqli_query($link, 'SELECT name, abbr, type, members, kills, wars FROM guilds');");
                    sb.AppendLine("    if (!$result) {");
                    sb.AppendLine("        logDebug('JSON: Failed to query guilds: ' . mysqli_error($link));");
                    sb.AppendLine("    } else {");
                    sb.AppendLine("        while ($row = mysqli_fetch_assoc($result)) {");
                    sb.AppendLine("            $guilds[] = [");
                    sb.AppendLine("                'Name' => $row['name'], 'Abbreviation' => $row['abbr'], 'Type' => $row['type'],");
                    sb.AppendLine("                'Members' => (int)$row['members'], 'Kills' => (int)$row['kills'], 'Wars' => (int)$row['wars']");
                    sb.AppendLine("            ];");
                    sb.AppendLine("        }");
                    sb.AppendLine("    }");
                }
                else
                {
                    sb.AppendLine("    $guilds = []; // Not fetching guilds for this endpoint");
                }

                sb.AppendLine("    $jsonData = [");
                sb.AppendLine("        'Shard' => [");
                sb.AppendLine("            'Online' => true, 'ServerName' => $jsonData['Shard']['ServerName'] ?? 'Unknown Server', 'UptimeMinutes' => 0,");
                sb.AppendLine("            'PlayerCount' => count(array_filter($players, fn($p) => $p['IsOnline'])), 'Time' => date('c'),");
                sb.AppendLine("            'Port' => '2593', 'MaxOnline' => $maxOnline");
                sb.AppendLine("        ],");
                sb.AppendLine("        'Players' => $players, 'Guilds' => $guilds");
                sb.AppendLine("    ];");
                sb.AppendLine("    header('Content-Type: application/json');");
                sb.AppendLine("    echo json_encode($jsonData);");
                sb.AppendLine("    exit;");
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("// Database connection for HTML rendering");
            sb.AppendLine("if (!isset($link) || !$link) {");
            sb.AppendLine("    $link = mysqli_connect($dbHost, $dbUser, $dbPass, $dbName, $dbPort);");
            sb.AppendLine("}");
            sb.AppendLine("if (!$link) {");
            sb.AppendLine("    logDebug('HTML: Database connection failed: ' . mysqli_connect_error());");
            sb.AppendLine("    $jsonData = null;");
            sb.AppendLine("} else {");
            sb.AppendLine("    $jsonData = fetchData($link);");
            sb.AppendLine("    mysqli_close($link);");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("function fetchData($link) {");
            sb.AppendLine("    $maxOnline = (int)mysqli_fetch_assoc(mysqli_query($link, 'SELECT value FROM maxonline LIMIT 1'))['value'] ?? 0;");

            if (fetchPlayers)
            {
                sb.AppendLine("    $players = [];");
                sb.AppendLine("    $result = mysqli_query($link, 'SELECT serial, name, title, guild, abbr, kills, fame, karma, lastlogin, racegender, isonline, servername FROM players');");
                sb.AppendLine("    if (!$result) {");
                sb.AppendLine("        logDebug('HTML: Failed to query players: ' . mysqli_error($link));");
                sb.AppendLine("        return null;");
                sb.AppendLine("    }");
                sb.AppendLine("    while ($row = mysqli_fetch_assoc($result)) {");
                sb.AppendLine("        $serial = (int)$row['serial'];");
                sb.AppendLine("        if ($serial <= 0) {");
                sb.AppendLine("            logDebug('HTML: Skipping player with invalid serial - Name: ' . $row['name'] . ', Serial: ' . $serial);");
                sb.AppendLine("            continue;");
                sb.AppendLine("        }");
                sb.AppendLine("        $players[] = [");
                sb.AppendLine("            'Serial' => $serial,");
                sb.AppendLine("            'Name' => $row['name'], 'Title' => $row['title'], 'Guild' => $row['guild'], 'GuildAbbr' => $row['abbr'],");
                sb.AppendLine("            'Kills' => (int)$row['kills'], 'Fame' => (int)$row['fame'], 'Karma' => (int)$row['karma'],");
                sb.AppendLine("            'LastLogin' => $row['lastlogin'], 'RaceGender' => $row['racegender'], 'IsOnline' => (bool)$row['isonline'],");
                sb.AppendLine("            'ServerName' => $row['servername']");
                sb.AppendLine("        ];");
                sb.AppendLine("    }");
            }
            else
            {
                sb.AppendLine("    $players = []; // Not fetching players for this page");
            }

            if (fetchGuilds)
            {
                sb.AppendLine("    $guilds = [];");
                sb.AppendLine("    $result = mysqli_query($link, 'SELECT name, abbr, type, members, kills, wars FROM guilds');");
                sb.AppendLine("    if (!$result) {");
                sb.AppendLine("        logDebug('HTML: Failed to query guilds: ' . mysqli_error($link));");
                sb.AppendLine("    } else {");
                sb.AppendLine("        while ($row = mysqli_fetch_assoc($result)) {");
                sb.AppendLine("            $guilds[] = [");
                sb.AppendLine("                'Name' => $row['name'], 'Abbreviation' => $row['abbr'], 'Type' => $row['type'],");
                sb.AppendLine("                'Members' => (int)$row['members'], 'Kills' => (int)$row['kills'], 'Wars' => (int)$row['wars']");
                sb.AppendLine("            ];");
                sb.AppendLine("        }");
                sb.AppendLine("    }");
            }
            else
            {
                sb.AppendLine("    $guilds = []; // Not fetching guilds for this page");
            }

            sb.AppendLine("    return [");
            sb.AppendLine("        'Shard' => [");
            sb.AppendLine("            'Online' => true, 'ServerName' => $players[0]['ServerName'] ?? 'Unknown Server', 'UptimeMinutes' => 0,");
            sb.AppendLine("            'PlayerCount' => count(array_filter($players, fn($p) => $p['IsOnline'])), 'Time' => date('c'),");
            sb.AppendLine("            'Port' => '2593', 'MaxOnline' => $maxOnline");
            sb.AppendLine("        ],");
            sb.AppendLine("        'Players' => $players, 'Guilds' => $guilds");
            sb.AppendLine("    ];");
            sb.AppendLine("}");
            sb.AppendLine("?>");
        }

        protected void AppendSidebarJavaScript(StringBuilder sb)
        {
            sb.AppendLine("  <script>");
            sb.AppendLine("    setInterval(() => fetch('index.php?json').then(r => r.json()).then(data => {");
            sb.AppendLine("      const isOnline = new Date(data.Shard.Time) > new Date() - 45000;");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(1)').innerText = 'Name: ' + (data.Shard.ServerName || 'Unknown Server');");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(2) span').innerText = isOnline ? 'Online' : 'Offline';");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(2) span').className = isOnline ? 'status-online' : 'status-offline';");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(3)').innerText = 'Port: ' + data.Shard.Port;");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(4)').innerText = 'Uptime: ' + (isOnline ? data.Shard.UptimeMinutes : 0) + ' minutes';");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(5)').innerText = 'Online Players: ' + (isOnline ? data.Shard.PlayerCount : 0);");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(6)').innerText = 'Total Players: ' + data.Players.length;");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(7)').innerText = 'Guilds: ' + data.Guilds.length;");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(8)').innerText = 'Max Online: ' + data.Shard.MaxOnline;");
            sb.AppendLine("    }).catch(err => {");
            sb.AppendLine("      console.error('Failed to fetch server status:', err);");
            sb.AppendLine("      document.querySelector('.sidebar p:nth-child(1)').innerText = 'Name: Unknown Server';");
            sb.AppendLine("    }), 30000);");
            sb.AppendLine("  </script>");
        }
    }
}
