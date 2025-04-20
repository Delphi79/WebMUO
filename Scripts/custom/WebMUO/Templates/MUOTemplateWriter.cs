using System;
using System.Collections.Generic;

namespace Server.MUOTemplates
{
    public static class MUOTemplateWriter
    {
        public enum TemplateType
        {
            Config,
            Index,
            Paperdoll,
            GumpMappings,
            Players,
            Guilds,
            Updates,
            UOTools,
            Client,
            Contact,
            Admin
        }

        public class TemplateConfig
        {
            public string DbHost { get; set; } = "localhost";
            public string DbName { get; set; } = "dbname_mymuo";
            public string DbUser { get; set; } = "dbuser_imanewb";
            public string DbPass { get; set; } = "db_pawword";
            public int DbPort { get; set; } = 3306;
            public string UploadPassword { get; set; } = "ModernUO!";
            public string ServerName { get; set; } = "ModernUO"; // Default, will be overridden
            public string Port { get; set; } = "2593";
            public bool IncludeOptionalUploads { get; set; } = false;
            public string AdminPassword { get; set; } = "your_secure_admin_password";
            public Dictionary<string, string> CustomReplacements { get; set; } = new();
        }

        public static string GetTemplate(TemplateType type, TemplateConfig config = null)
        {
            // Ensure config is never null and set default server name if empty
            config = config ?? new TemplateConfig();
            if (string.IsNullOrEmpty(config.ServerName))
            {
                config.ServerName = "My MUO Server"; // Ensure server name is set
            }

            // Log the server name for debugging
            //Console.WriteLine($"Generating template {type} with ServerName: {config.ServerName}");

            string template = type switch
            {
                TemplateType.Config       => ConfigAndUploadTemplate.Generate(config),
                TemplateType.Index        => IndexPhpTemplate.Generate(config),
                TemplateType.Paperdoll    => PaperdollTemplate.Generate(config),
                TemplateType.GumpMappings => GumpMappingsTemplate.Generate(config),
                TemplateType.Players      => PlayersPhpTemplate.Generate(config),
                TemplateType.Guilds       => GuildsPhpTemplate.Generate(config),
                TemplateType.Updates       => UpdatesPhpTemplate.Generate(config),
                TemplateType.UOTools      => UOToolsPhpTemplate.Generate(config),
                TemplateType.Client       => ClientPhpTemplate.Generate(config),
                TemplateType.Contact      => ContactPhpTemplate.Generate(config),
                TemplateType.Admin        => AdminPhpTemplate.Generate(config),
                _                         => throw new ArgumentException("Invalid template type")
            };

            // Apply custom replacements
            foreach (KeyValuePair<string, string> replacement in config.CustomReplacements)
            {
                template = template.Replace(replacement.Key, replacement.Value);
            }

            return template;
        }
    }
}
