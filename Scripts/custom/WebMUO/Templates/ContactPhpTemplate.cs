using System.Text;

namespace Server.MUOTemplates
{
    public class ContactPhpTemplate : BasePhpTemplate
    {
        private readonly MUOTemplateWriter.TemplateConfig _config;

        public ContactPhpTemplate(string serverName, string port, MUOTemplateWriter.TemplateConfig config)
            : base(serverName, port)
        {
            _config = config;
        }

        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var template = new ContactPhpTemplate(config.ServerName, config.Port, config);
            return template.GenerateTemplate();
        }

        private string GenerateTemplate()
        {
            var sb = new StringBuilder();
            AppendPhpHeader(sb, "contact_errors.log", true, true, _config);

            // Load settings from sitesettings.json
            sb.AppendLine("<?php");
            sb.AppendLine("$activePage = 'contact';");
            sb.AppendLine("$settingsFile = __DIR__ . '/sitesettings.json';");
            sb.AppendLine("$settings = [");
            sb.AppendLine("  'menu_visibility' => [],");
            sb.AppendLine("  'social_visibility' => [],");
            sb.AppendLine("  'show_background_image' => true,");
            sb.AppendLine("  'show_powered_by' => true,");
            sb.AppendLine("  'footer_copyright_domain' => 'mydomain.com',");
            sb.AppendLine("  'background_color' => '#000000',");
            sb.AppendLine("  'text_color' => '#ffffff',");
            sb.AppendLine("  'theme' => 'dark',");
            sb.AppendLine("  'contact_emails' => ['support@yourserver.com'],"); // Default email
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
            sb.AppendLine("if (file_exists($settingsFile)) {");
            sb.AppendLine("  $json = file_get_contents($settingsFile);");
            sb.AppendLine("  $decoded = json_decode($json, true);");
            sb.AppendLine("  if (is_array($decoded)) {");
            sb.AppendLine("    $settings = array_merge($settings, $decoded);");
            sb.AppendLine("  }");
            sb.AppendLine("}");
            // Debug settings
            sb.AppendLine("echo '<!-- Debug: show_background_image = ' . ($settings['show_background_image'] ? 'true' : 'false') . ', show_powered_by = ' . ($settings['show_powered_by'] ? 'true' : 'false') . ', footer_copyright_domain = ' . htmlspecialchars($settings['footer_copyright_domain']) . ', background_color = ' . htmlspecialchars($settings['background_color']) . ' -->';");
            sb.AppendLine("echo '<!-- Debug: Applied background style = ' . ($settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center ' . htmlspecialchars($settings['background_color']) : htmlspecialchars($settings['background_color'])) . ' -->';");
            sb.AppendLine("?>");

            // HTML structure
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset='UTF-8' />");
            sb.AppendLine("  <meta name='viewport' content='width=device-width, initial-scale=1.0'/>");
            sb.AppendLine("  <title><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?> Contact</title>");
            sb.AppendLine("  <link rel=\"stylesheet\" href=\"styles.css\">");
            sb.AppendLine("  <style>");
            sb.AppendLine("    :root {");
            sb.AppendLine("        --text-color: <?php echo htmlspecialchars($settings['text_color']); ?>;");
            sb.AppendLine("    }");
            sb.AppendLine("    html, body { margin: 0; padding: 0; background: <?php echo $settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center ' . htmlspecialchars($settings['background_color']) : htmlspecialchars($settings['background_color']); ?>; background-size: cover; color: var(--text-color); }");
            sb.AppendLine("    .container, .main-content, .top-bar, .menu ul li a, .powered-by-header, .footer { color: var(--text-color); }");
            sb.AppendLine("    .content-section-contact h1, .content-section-contact p, .content-section-contact ul li, .content-section-contact ul li a, .content-header h1, .content-header p { color: var(--text-color) !important; }");
            sb.AppendLine("    .content-section-contact ul li a:hover { text-decoration: underline; }");
            sb.AppendLine("    .content-wrapper-contact { display: flex; flex: 1; overflow: hidden; position: relative; margin-top: 60px; margin-bottom: 60px; }");
            sb.AppendLine("    .main-section-contact { flex: 0 0 100%; display: flex; flex-direction: column; margin: 0; width: 100%; min-height: calc(100vh - 120px); padding: 0; }");
            sb.AppendLine("    .content-section-contact { display: flex; flex-direction: column; width: 100%; max-width: 90vw; margin: 0 auto; height: 100%; }");
            sb.AppendLine("    .content-header { position: sticky; top: 0; background: rgba(0, 0, 0, 0.7); padding: 10px 0; z-index: 10; text-align: center; width: 100%; max-width: 90vw; margin: 0 auto; box-sizing: border-box; }");
            sb.AppendLine("    .content-header h1 { margin: 0; font-size: 2.5em; }");
            sb.AppendLine("    .content-header p { margin: 5px 0 0; font-size: 1.2em; }");
            sb.AppendLine("    .contact-info-container { margin-top: 10px; width: 100%; max-width: 90vw; background: rgba(0, 0, 0, 0.7); padding: 15px; border: 1px solid #333; margin-left: auto; margin-right: auto; text-align: center; }");
            sb.AppendLine("    .contact-info { list-style: none; padding: 0; margin: 0; }");
            sb.AppendLine("    .contact-info li { margin: 10px 0; font-size: 1.1em; }");
            sb.AppendLine("    .contact-info li a { text-decoration: none; }");
            sb.AppendLine("    @media (max-width: 768px) {");
            sb.AppendLine("      .content-section-contact { max-width: 100%; padding: 10px; }");
            sb.AppendLine("      .content-header { max-width: 100%; padding: 10px; }");
            sb.AppendLine("      .content-header h1 { font-size: 2em; }");
            sb.AppendLine("      .content-header p { font-size: 1em; }");
            sb.AppendLine("      .contact-info-container { max-width: 100%; padding: 10px; }");
            sb.AppendLine("      .contact-info li { font-size: 1em; }");
            sb.AppendLine("    }");
            sb.AppendLine("  </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body class='theme-<?php echo htmlspecialchars($settings['theme']); ?>'>");
            sb.AppendLine("  <div class='container'>");
            sb.AppendLine("    <div class='main-content'>");

            // Top bar with logo and updated menu (with visibility checks and dynamic URLs)
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

            // Shared powered-by header with toggle
            sb.AppendLine("      <?php if (!empty($settings['show_powered_by'])) { ?>");
            sb.AppendLine("      <div class='powered-by-header'>");
            sb.AppendLine("        <span>Powered by ModernUO</span>");
            sb.AppendLine("        <img src='images/muologo.png' alt='ModernUO Logo' class='modernuo-logo'>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <?php } ?>");

            // Page-specific content with content-header
            sb.AppendLine("      <div class='content-wrapper-contact'>");
            sb.AppendLine("        <div class='main-section-contact'>");
            sb.AppendLine("          <div class='content-section-contact'>");
            sb.AppendLine("            <div class='content-header'>");
            sb.AppendLine("              <h1><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?> Contact</h1>");
            sb.AppendLine("              <p>Reach out to us for support or inquiries.</p>");
            sb.AppendLine("            </div>");
            sb.AppendLine("            <div class='contact-info-container'>");
            sb.AppendLine("              <ul class='contact-info'>");
            sb.AppendLine("                <?php");
            sb.AppendLine("                if (!empty($settings['contact_emails'])) {");
            sb.AppendLine("                  foreach ($settings['contact_emails'] as $email) {");
            sb.AppendLine("                    echo \"<li>Email: <a href='mailto:\" . htmlspecialchars($email) . \"'>\" . htmlspecialchars($email) . \"</a></li>\";");
            sb.AppendLine("                  }");
            sb.AppendLine("                } else {");
            sb.AppendLine("                  echo \"<li>No contact email available.</li>\";");
            sb.AppendLine("                }");
            sb.AppendLine("                ?>");
            sb.AppendLine("                <li>Discord: <a href='<?php echo htmlspecialchars($settings['social_urls']['discord']); ?>' target='_blank'>Join our Discord</a></li>");
            sb.AppendLine("              </ul>");
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
            sb.AppendLine("            if ($settings['social_visibility']['twitter']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['twitter']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/?size=48&id=phOKFKYpe00C&format=png&color=000000\" alt=\"Twitter\" class=\"social-icon-img\"></a>';");
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
