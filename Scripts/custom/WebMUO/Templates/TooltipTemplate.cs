using System.Text;

namespace Server.MUOTemplates
{
    public class TooltipTemplate : BasePhpTemplate
    {
        private readonly MUOTemplateWriter.TemplateConfig _config;

        public TooltipTemplate(string serverName, string port, MUOTemplateWriter.TemplateConfig config)
            : base(serverName, port)
        {
            _config = config;
        }

        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var template = new TooltipTemplate(config.ServerName, config.Port, config);
            return template.GenerateTemplate();
        }

        private string GenerateTemplate()
        {
            var sb = new StringBuilder();
            AppendPhpHeader(sb, "tooltip_errors.log", true, false, _config);
            sb.AppendLine("<?php");
            sb.AppendLine("$activePage = 'tooltip';");
            sb.AppendLine("?>");
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset='UTF-8' />");
            sb.AppendLine("  <meta name='viewport' content='width=device-width, initial-scale=1.0'/>");
            sb.AppendLine("  <title>Tooltip - <?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?></title>");
            sb.AppendLine("  <link rel=\"stylesheet\" href=\"styles.css\">");
            sb.AppendLine("</head>");
            sb.AppendLine("<body class='theme-<?php echo htmlspecialchars($settings['theme']); ?>'>");
            sb.AppendLine("  <div class='container'>");
            sb.AppendLine("    <div class='sidebar'>");
            sb.Append(GenerateSidebar("tooltip"));
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class='main-content'>");
            sb.AppendLine("      <div class='header'><img src='images/banner.png' alt='Server Banner'></div>");
            sb.AppendLine("      <div class='content-section'>");
            sb.AppendLine("        <h2>Item Tooltip</h2>");
            sb.AppendLine("        <p>Render item tooltips here.</p>");
            sb.AppendLine("        <!-- Add tooltip rendering logic here -->");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            AppendSidebarJavaScript(sb);
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}
