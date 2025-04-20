using System.Text;

namespace Server.MUOTemplates
{
    public class UpdatesPhpTemplate : BasePhpTemplate
    {
        private readonly MUOTemplateWriter.TemplateConfig _config;

        public UpdatesPhpTemplate(string serverName, string port, MUOTemplateWriter.TemplateConfig config)
            : base(serverName, port)
        {
            _config = config;
        }

        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var template = new UpdatesPhpTemplate(config.ServerName, config.Port, config);
            return template.GenerateTemplate();
        }

        private string GenerateTemplate()
        {
            var sb = new StringBuilder();
            AppendPhpHeader(sb, "social_errors.log", true, true, _config);

            sb.AppendLine("<?php");
            sb.AppendLine("$activePage = 'social';");
            sb.AppendLine("$settingsFile = __DIR__ . '/sitesettings.json';");
            sb.AppendLine("$updatesFile = __DIR__ . '/shard_updates.json';");
            sb.AppendLine("$settings = [");
            sb.AppendLine("  'menu_visibility' => [],");
            sb.AppendLine("  'social_visibility' => [],");
            sb.AppendLine("  'show_background_image' => true,");
            sb.AppendLine("  'show_powered_by' => true,");
            sb.AppendLine("  'footer_copyright_domain' => 'mydomain.com',");
            sb.AppendLine("  'client_host' => 'localhost',");
            sb.AppendLine("  'client_port' => '2593',");
            sb.AppendLine("  'background_color' => '#000000',");
            sb.AppendLine("  'text_color' => '#ffffff',");
            sb.AppendLine("  'theme' => 'dark',");
            // New settings for social media and wiki/forum URLs
            sb.AppendLine("  'social_urls' => [");
            sb.AppendLine("    'discord' => 'https://discord.com',");
            sb.AppendLine("    'youtube' => 'https://youtube.com',");
            sb.AppendLine("    'tiktok' => 'https://tiktok.com',");
            sb.AppendLine("    'twitter' => 'https://x.com',");
            sb.AppendLine("    'facebook' => 'https://facebook.com'");
            sb.AppendLine("  ],");
            sb.AppendLine("  'menu_urls' => [");
            sb.AppendLine("    'wiki' => 'https://wiki.example.com',");
            sb.AppendLine("    'forum' => 'https://forum.example.com'");
            sb.AppendLine("  ]");
            sb.AppendLine("];");
            sb.AppendLine("$updates = [];");
            sb.AppendLine("if (file_exists($settingsFile)) {");
            sb.AppendLine("  $json = file_get_contents($settingsFile);");
            sb.AppendLine("  $decoded = json_decode($json, true);");
            sb.AppendLine("  if (is_array($decoded)) {");
            sb.AppendLine("    $settings = array_merge($settings, $decoded);");
            sb.AppendLine("  }");
            sb.AppendLine("}");
            sb.AppendLine("if (file_exists($updatesFile)) {");
            sb.AppendLine("  $json = file_get_contents($updatesFile);");
            sb.AppendLine("  $updates = json_decode($json, true) ?: [];");
            sb.AppendLine("}");
            sb.AppendLine("function makeLinksClickable($text) {");
            sb.AppendLine("  $urlPattern = '/(https?:\\/\\/[^\\s<]+)/i';");
            sb.AppendLine("  $text = preg_replace_callback($urlPattern, function($matches) {");
            sb.AppendLine("    error_log('URL match: ' . print_r($matches, true));");
            sb.AppendLine("    $url = $matches[1];");
            sb.AppendLine("    $escapedUrl = htmlspecialchars($url, ENT_QUOTES, 'UTF-8');");
            sb.AppendLine("    return '<a href=\"' . $escapedUrl . '\" target=\"_blank\" rel=\"noopener noreferrer\">' . $escapedUrl . '</a>';");
            sb.AppendLine("  }, htmlspecialchars($text, ENT_QUOTES, 'UTF-8'));");
            sb.AppendLine("  return $text;");
            sb.AppendLine("}");
            sb.AppendLine("?>");

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset='UTF-8' />");
            sb.AppendLine("  <meta name='viewport' content='width=device-width, initial-scale=1.0'/>");
            sb.AppendLine("  <title><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?> Shard Updates</title>");
            sb.AppendLine("  <link rel=\"stylesheet\" href=\"styles.css\">");
            sb.AppendLine("  <style>");
            sb.AppendLine("    :root {");
            sb.AppendLine("        --text-color: <?php echo htmlspecialchars($settings['text_color']); ?>; /* Override the default --text-color */");
            sb.AppendLine("    }");
            sb.AppendLine("    html, body { margin: 0; padding: 0; background: <?php echo $settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center ' . htmlspecialchars($settings['background_color']) : htmlspecialchars($settings['background_color']); ?>; background-size: cover; color: var(--text-color); }");
            sb.AppendLine("    .container, .main-content, .top-bar, .menu ul li a, .powered-by-header, .footer { color: var(--text-color); }");
            sb.AppendLine("    .content-section-social h1, .content-section-social p, .content-header h1, .content-header p, .update-title h3, .update-item p, .update-item .date { color: var(--text-color) !important; }");
            sb.AppendLine("    .content-wrapper-social { display: flex; flex: 1; overflow: hidden; position: relative; margin-top: 60px; margin-bottom: 60px; }");
            sb.AppendLine("    .main-section-social { flex: 0 0 100%; display: flex; flex-direction: column; margin: 0; width: 100%; min-height: calc(100vh - 120px); padding: 0; }");
            sb.AppendLine("    .content-section-social { display: flex; flex-direction: column; width: 100%; max-width: 90vw; margin: 0 auto; height: 100%; }");
            sb.AppendLine("    .content-header { position: sticky; top: 0; background: rgba(0, 0, 0, 0.7); padding: 10px 0; z-index: 10; text-align: center; }");
            sb.AppendLine("    .content-header h1 { margin: 0; font-size: 2.5em; }");
            sb.AppendLine("    .content-header p { margin: 5px 0 0; font-size: 1.2em; }");
            sb.AppendLine("    .update-list { margin-top: 10px; width: 100%; background: rgba(0, 0, 0, 0.7); padding: 15px; border: 1px solid #333; overflow-y: auto; flex-grow: 1; max-height: calc(100vh - 220px); }");
            sb.AppendLine("    .update-item { border: 1px solid #333; padding: 15px; margin-bottom: 15px; background: #222; display: flex; flex-direction: column; width: 100%; }");
            sb.AppendLine("    .update-title { display: flex; align-items: center; margin-bottom: 5px; }");
            sb.AppendLine("    .update-title img { height: 24px; width: 24px; margin-right: 10px; }");
            sb.AppendLine("    .update-title h3 { margin: 0; font-size: 1.5em; flex-grow: 1; }");
            sb.AppendLine("    .update-item p { margin: 0 0 10px; }");
            sb.AppendLine("    .update-item a { color: #66ccff; text-decoration: underline; }");
            sb.AppendLine("    .update-item a:hover { color: #99eeff; }");
            sb.AppendLine("    .update-item .date { font-size: 0.9em; color: #aaa; }");
            sb.AppendLine("    @media (max-width: 768px) {");
            sb.AppendLine("      .content-section-social { padding: 10px; max-width: 100%; }");
            sb.AppendLine("      .content-header h1 { font-size: 2em; }");
            sb.AppendLine("      .content-header p { font-size: 1em; }");
            sb.AppendLine("      .update-list { padding: 10px; max-height: calc(100vh - 200px); }");
            sb.AppendLine("      .update-title img { height: 20px; width: 20px; }");
            sb.AppendLine("      .update-title h3 { font-size: 1.3em; }");
            sb.AppendLine("    }");
            sb.AppendLine("  </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body class='theme-<?php echo htmlspecialchars($settings['theme']); ?>'>");
            sb.AppendLine("  <div class='container'>");
            sb.AppendLine("    <div class='main-content'>");

            // Shared header (top bar with logo and updated menu with dynamic URLs)
            sb.AppendLine("      <div class='top-bar'>");
            sb.AppendLine("        <div class='logo'>");
            sb.AppendLine("          <img src='images/logo.png' alt='ModernUO Logo'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='menu'>");
            sb.AppendLine("          <ul>");
            sb.AppendLine("            <?php");
            sb.AppendLine("            if ($settings['menu_visibility']['index']) {");
            sb.AppendLine("              echo \"<li><a href='index.php' class='\" . ($activePage == 'index' ? 'active' : '') . \"'>Home</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['players']) {");
            sb.AppendLine("              echo \"<li><a href='players.php' class='\" . ($activePage == 'players' ? 'active' : '') . \"'>Players</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['guilds']) {");
            sb.AppendLine("              echo \"<li><a href='guilds.php' class='\" . ($activePage == 'guilds' ? 'active' : '') . \"'>Guilds</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['forum']) {");
            sb.AppendLine("              echo \"<li><a href='\" . htmlspecialchars($settings['menu_urls']['forum']) . \"' class='\" . ($activePage == 'forum' ? 'active' : '') . \"'>Forum</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['wiki']) {");
            sb.AppendLine("              echo \"<li><a href='\" . htmlspecialchars($settings['menu_urls']['wiki']) . \"' class='\" . ($activePage == 'wiki' ? 'active' : '') . \"'>Wiki</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['uotools']) {");
            sb.AppendLine("              echo \"<li><a href='uotools.php' class='\" . ($activePage == 'uotools' ? 'active' : '') . \"'>UO Tools</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['social']) {");
            sb.AppendLine("              echo \"<li><a href='social.php' class='\" . ($activePage == 'social' ? 'active' : '') . \"'>Updates</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['contact']) {");
            sb.AppendLine("              echo \"<li><a href='contact.php' class='\" . ($activePage == 'contact' ? 'active' : '') . \"'>Contact</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if ($settings['menu_visibility']['client']) {");
            sb.AppendLine("              echo \"<li><a href='client.php' class='\" . ($activePage == 'client' ? 'active' : '') . \"'>Client</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            if (isset($_SESSION['admin_logged_in']) && $_SESSION['admin_logged_in']) {");
            sb.AppendLine("              echo \"<li><a href='admin.php' class='\" . ($activePage == 'admin' ? 'active' : '') . \"'>Admin</a></li>\";");
            sb.AppendLine("            }");
            sb.AppendLine("            ?>");
            sb.AppendLine("          </ul>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            sb.AppendLine("      <?php if (!empty($settings['show_powered_by'])) { ?>");
            sb.AppendLine("      <div class='powered-by-header'>");
            sb.AppendLine("        <span>Powered by ModernUO</span>");
            sb.AppendLine("        <img src='images/muologo.png' alt='ModernUO Logo' class='modernuo-logo'>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <?php } ?>");

            sb.AppendLine("      <div class='content-wrapper-social'>");
            sb.AppendLine("        <div class='main-section-social'>");
            sb.AppendLine("          <div class='content-section-social'>");
            sb.AppendLine("            <div class='content-header'>");
            sb.AppendLine("              <h1><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?> Shard Updates</h1>");
            sb.AppendLine("              <p>Latest updates and announcements for the shard.</p>");
            sb.AppendLine("            </div>");
            sb.AppendLine("            <div class='update-list'>");
            sb.AppendLine("              <?php");
            sb.AppendLine("              if (!empty($updates)) {");
            sb.AppendLine("                usort($updates, fn($a, $b) => strtotime($b['date']) - strtotime($a['date']));");
            sb.AppendLine("                foreach ($updates as $update) {");
            sb.AppendLine("                  echo \"<div class='update-item'>\";");
            sb.AppendLine("                  echo \"<div class='update-title'>\";");
            sb.AppendLine("                  echo \"<img src='images/logo.png' alt='Update Logo'>\";");
            sb.AppendLine("                  echo \"<h3>\" . htmlspecialchars($update['title']) . \"</h3>\";");
            sb.AppendLine("                  echo \"</div>\";");
            sb.AppendLine("                  echo \"<p class='date'>\" . htmlspecialchars($update['date']) . \"</p>\";");
            sb.AppendLine("                  echo \"<!-- Debug: Content before processing: \" . htmlspecialchars($update['content']) . \" -->\";");
            sb.AppendLine("                  echo \"<!-- Debug: Content after processing: \" . makeLinksClickable($update['content']) . \" -->\";");
            sb.AppendLine("                  echo \"<p>\" . nl2br(makeLinksClickable($update['content'])) . \"</p>\";");
            sb.AppendLine("                  echo \"</div>\";");
            sb.AppendLine("                }");
            sb.AppendLine("              } else {");
            sb.AppendLine("                echo \"<p>No updates available.</p>\";");
            sb.AppendLine("              }");
            sb.AppendLine("              ?>");
            sb.AppendLine("            </div>");
            sb.AppendLine("          </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            // Shared footer with dynamic social media links
            sb.AppendLine("      <div class='footer'>");
            sb.AppendLine("        <div class='footer-container'>");
            sb.AppendLine("          <div class='social-icons'>");
            sb.AppendLine("            <?php");
            sb.AppendLine("            if ($settings['social_visibility']['facebook']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['facebook']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/facebook.png\" alt=\"Facebook\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("            if ($settings['social_visibility']['youtube']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['youtube']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/youtube-play.png\" alt=\"YouTube\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("            if ($settings['social_visibility']['tiktok']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['tiktok']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/tiktok.png\" alt=\"TikTok\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("            if ($settings['social_visibility']['discord']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['discord']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/discord.png\" alt=\"Discord\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("            if ($settings['social_visibility']['twitter']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['twitter']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/?size=48&id=phOKFKYpe00C&format=png&color=000000\" alt=\"X\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("            ?>");
            sb.AppendLine("          </div>");
            sb.AppendLine("          <p>Copyright <?php echo htmlspecialchars($settings['footer_copyright_domain'] ?? 'mydomain.com'); ?></p>");
            sb.AppendLine("          <div class='powered-by'>");
            sb.AppendLine("            <span>Compatible</span>");
            sb.AppendLine("            <div class='powered-by-images'>");
            sb.AppendLine("              <img src='images/razor.png' alt='Razor'>");
            sb.AppendLine("              <img src='images/Uosteamlogo.png' alt='UOsteam' class='uosteam-icon'>");
            sb.AppendLine("              <img src='images/classicUOLogo.png' alt='ClassicUO' class='classicuo-icon'>");
            sb.AppendLine("            </div>");
            sb.AppendLine("          </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
