using System.Text;

namespace Server.MUOTemplates
{
    public class IndexPhpTemplate : BasePhpTemplate
    {
        private readonly MUOTemplateWriter.TemplateConfig _config;

        public IndexPhpTemplate(string serverName, string port, MUOTemplateWriter.TemplateConfig config)
            : base(serverName, port)
        {
            _config = config;
        }

        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var template = new IndexPhpTemplate(config.ServerName, config.Port, config);
            return template.GenerateTemplate();
        }

        private string GenerateTemplate()
{
    var sb = new StringBuilder();
    AppendPhpHeader(sb, "index_errors.log", true, true, _config);

    // Handle JSON request for server status updates
    sb.AppendLine("<?php");
    // Include config.php for database connection (if it exists)
    sb.AppendLine("if (file_exists('config.php')) {");
    sb.AppendLine("    require_once 'config.php';");
    sb.AppendLine("}");
    // Fetch Max Online from database for JSON endpoint
    sb.AppendLine("// Database connection for Max Online");
    sb.AppendLine("$max_online_from_db = 0; // Default fallback");
    sb.AppendLine("if (isset($dbHost, $dbUser, $dbPass, $dbName)) {");
    sb.AppendLine("    $mysqli = new mysqli($dbHost, $dbUser, $dbPass, $dbName, $dbPort ?? 3306);");
    sb.AppendLine("    if ($mysqli->connect_error) {");
    sb.AppendLine("        error_log('Database connection failed: ' . $mysqli->connect_error);");
    sb.AppendLine("        $max_online_from_db = 0; // Fallback value");
    sb.AppendLine("    } else {");
    sb.AppendLine("        $result = $mysqli->query('SELECT value FROM maxonline WHERE id = 1 LIMIT 1');");
    sb.AppendLine("        if ($result && $row = $result->fetch_assoc()) {");
    sb.AppendLine("            $max_online_from_db = $row['value'] ?? 0;");
    sb.AppendLine("        } else {");
    sb.AppendLine("            $max_online_from_db = 0; // Fallback if query fails");
    sb.AppendLine("        }");
    sb.AppendLine("        $result->free();");
    sb.AppendLine("        $mysqli->close();");
    sb.AppendLine("    }");
    sb.AppendLine("} else {");
    sb.AppendLine("    error_log('Database connection variables not defined in config.php');");
    sb.AppendLine("}");

    sb.AppendLine("if (isset($_GET['json'])) {");
    sb.AppendLine("    $shardDataFile = __DIR__ . '/sharddata.json';");
    sb.AppendLine("    $jsonData = [];");
    sb.AppendLine("    if (file_exists($shardDataFile)) {");
    sb.AppendLine("        $json = file_get_contents($shardDataFile);");
    sb.AppendLine("        $jsonData = json_decode($json, true) ?? [];");
    sb.AppendLine("    }");
    // Override MaxOnline with database value in JSON response
    sb.AppendLine("    if (isset($jsonData['Shard'])) {");
    sb.AppendLine("        $jsonData['Shard']['MaxOnline'] = $max_online_from_db;");
    sb.AppendLine("    } else {");
    sb.AppendLine("        $jsonData['Shard'] = ['MaxOnline' => $max_online_from_db];");
    sb.AppendLine("    }");
    sb.AppendLine("    header('Access-Control-Allow-Origin: *');");
    sb.AppendLine("    header('Content-Type: application/json');");
    sb.AppendLine("    echo json_encode($jsonData);");
    sb.AppendLine("    exit;");
    sb.AppendLine("}");

    // Load settings from sitesettings.json
    sb.AppendLine("$activePage = 'index';");
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
    sb.AppendLine("  'homepage_text' => \"Welcome to WebMUO\\nThis is the official page for our Ultima Online shard. Explore the menu to view player stats, guild information, download our custom client, or get in touch!\",");
    sb.AppendLine("  'show_download_button' => true,");
    sb.AppendLine("  'download_button_color' => '#FFFF00',");
    sb.AppendLine("  'download_button_text' => 'DOWNLOAD CLIENT',");
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

    // Load shard data for initial render
    sb.AppendLine("$shardDataFile = __DIR__ . '/sharddata.json';");
    sb.AppendLine("$jsonData = [];");
    sb.AppendLine("if (file_exists($shardDataFile)) {");
    sb.AppendLine("    $json = file_get_contents($shardDataFile);");
    sb.AppendLine("    $jsonData = json_decode($json, true) ?? [];");
    sb.AppendLine("}");
    sb.AppendLine("?>");

    // HTML structure
    sb.AppendLine("<!DOCTYPE html>");
    sb.AppendLine("<html lang=\"en\">");
    sb.AppendLine("<head>");
    sb.AppendLine("    <meta charset=\"UTF-8\" />");
    sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
    sb.AppendLine("    <title><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?></title>");
    sb.AppendLine("    <link rel=\"stylesheet\" href=\"styles.css\">");
    sb.AppendLine("    <style>");
    sb.AppendLine("        :root {");
    sb.AppendLine("            --text-color: <?php echo htmlspecialchars($settings['text_color']); ?>;");
    sb.AppendLine("        }");
    sb.AppendLine("        html, body { margin: 0; padding: 0; background: <?php echo $settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center ' . htmlspecialchars($settings['background_color']) : htmlspecialchars($settings['background_color']); ?>; background-size: cover; color: var(--text-color); }");
    sb.AppendLine("        .container, .main-content, .top-bar, .menu ul li a, .powered-by-header, .status-section-index, .footer { color: var(--text-color); }");
    sb.AppendLine("        .content-section-index h1, .content-section-index p, .content-section-index a, .content-header h1, .content-header p, .status-section-index h3, .status-section-index p, .status-section-index span { color: var(--text-color) !important; }");
    // Layout styles to match other pages
    sb.AppendLine("        .content-wrapper-index { display: flex; flex: 1; overflow: hidden; position: relative; margin-top: 60px; margin-bottom: 60px; width: 100%; max-width: 90vw; margin-left: auto; margin-right: auto; }");
    sb.AppendLine("        .status-section-index { flex: 0 0 250px; margin-right: 20px; background: rgba(0, 0, 0, 0.7); padding: 15px; border: 1px solid #333; border-radius: 5px; }");
    sb.AppendLine("        .main-section-index { flex: 1; display: flex; flex-direction: column; padding: 15px; border-radius: 5px; text-align: center; }");
    sb.AppendLine("        .right-spacer-index { flex: 0 0 250px; }");
    sb.AppendLine("        .content-section-index { flex: 1; display: flex; flex-direction: column; align-items: center; justify-content: center; background: rgba(0, 0, 0, 0.7); border-radius: 5px; padding: 15px; }");
    // Content-header styling to match other pages
    sb.AppendLine("        .content-header { padding: 10px 0; text-align: center; width: 100%; box-sizing: border-box; }");
    sb.AppendLine("        .content-header h1 { margin: 0; font-size: 2.5em; }");
    sb.AppendLine("        .content-header p { margin: 5px 0 0; font-size: 1.2em; }");
    // Other styles
    sb.AppendLine("        .status-section-index h3 { margin: 0 0 10px; font-size: 1.5em; text-align: center; }");
    sb.AppendLine("        .status-section-index p { margin: 5px 0; font-size: 1em; }");
    sb.AppendLine("        .download-button {");
    sb.AppendLine("            background-color: <?php echo htmlspecialchars($settings['download_button_color']); ?> !important;");
    sb.AppendLine("            padding: 10px 20px;");
    sb.AppendLine("            text-decoration: none;");
    sb.AppendLine("            border-radius: 5px;");
    sb.AppendLine("            display: inline-block;");
    sb.AppendLine("            font-weight: bold;");
    sb.AppendLine("            color: #000 !important;");
    sb.AppendLine("            margin-top: 10px;");
    sb.AppendLine("        }");
    sb.AppendLine("        .download-button:hover {");
    sb.AppendLine("            opacity: 0.8;");
    sb.AppendLine("        }");
    sb.AppendLine("        @media (max-width: 768px) {");
    sb.AppendLine("            .content-wrapper-index { flex-direction: column; max-width: 100%; padding: 0 10px; }");
    sb.AppendLine("            .status-section-index { flex: 0 0 auto; margin-right: 0; margin-bottom: 20px; width: 100%; }");
    sb.AppendLine("            .main-section-index { width: 100%; }");
    sb.AppendLine("            .right-spacer-index { display: none; }");
    sb.AppendLine("            .content-header { padding: 10px; }");
    sb.AppendLine("            .content-header h1 { font-size: 2em; }");
    sb.AppendLine("            .content-header p { font-size: 1em; }");
    sb.AppendLine("            .status-section-index h3 { font-size: 1.3em; }");
    sb.AppendLine("            .status-section-index p { font-size: 0.9em; }");
    sb.AppendLine("        }");
    sb.AppendLine("    </style>");
    sb.AppendLine("</head>");
    sb.AppendLine("<body class=\"theme-<?php echo htmlspecialchars($settings['theme']); ?>\">");
    sb.AppendLine("    <div class=\"container\">");
    sb.AppendLine("        <div class=\"main-content\">");

    // Top bar with logo and updated menu
    sb.AppendLine("            <div class=\"top-bar\">");
    sb.AppendLine("                <div class=\"logo\">");
    sb.AppendLine("                    <img src=\"images/logo.png\" alt=\"ModernUO Logo\">");
    sb.AppendLine("                </div>");
    sb.AppendLine("                <div class=\"menu\">");
    sb.AppendLine("                    <ul>");
    sb.AppendLine("                        <?php");
    // Add isset checks to avoid undefined key warnings
    sb.AppendLine("                        if (isset($settings['menu_visibility']['index']) && $settings['menu_visibility']['index']) {");
    sb.AppendLine("                            echo \"<li><a href='index.php' class='\" . ($activePage == 'index' ? 'active' : '') . \"'>Home</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['players']) && $settings['menu_visibility']['players']) {");
    sb.AppendLine("                            echo \"<li><a href='players.php' class='\" . ($activePage == 'players' ? 'active' : '') . \"'>Players</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['guilds']) && $settings['menu_visibility']['guilds']) {");
    sb.AppendLine("                            echo \"<li><a href='guilds.php' class='\" . ($activePage == 'guilds' ? 'active' : '') . \"'>Guilds</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['forum']) && $settings['menu_visibility']['forum']) {");
    sb.AppendLine("                            echo \"<li><a href='\" . htmlspecialchars($settings['menu_urls']['forum']) . \"' class='\" . ($activePage == 'forum' ? 'active' : '') . \"'>Forum</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['wiki']) && $settings['menu_visibility']['wiki']) {");
    sb.AppendLine("                            echo \"<li><a href='\" . htmlspecialchars($settings['menu_urls']['wiki']) . \"' class='\" . ($activePage == 'wiki' ? 'active' : '') . \"'>Wiki</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['uotools']) && $settings['menu_visibility']['uotools']) {");
    sb.AppendLine("                            echo \"<li><a href='uotools.php' class='\" . ($activePage == 'uotools' ? 'active' : '') . \"'>UO Tools</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['social']) && $settings['menu_visibility']['social']) {");
    sb.AppendLine("                            echo \"<li><a href='social.php' class='\" . ($activePage == 'social' ? 'active' : '') . \"'>Updates</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['contact']) && $settings['menu_visibility']['contact']) {");
    sb.AppendLine("                            echo \"<li><a href='contact.php' class='\" . ($activePage == 'contact' ? 'active' : '') . \"'>Contact</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($settings['menu_visibility']['client']) && $settings['menu_visibility']['client']) {");
    sb.AppendLine("                            echo \"<li><a href='client.php' class='\" . ($activePage == 'client' ? 'active' : '') . \"'>Client</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        if (isset($_SESSION['admin_logged_in']) && $_SESSION['admin_logged_in']) {");
    sb.AppendLine("                            echo \"<li><a href='admin.php' class='\" . ($activePage == 'admin' ? 'active' : '') . \"'>Admin</a></li>\";");
    sb.AppendLine("                        }");
    sb.AppendLine("                        ?>");
    sb.AppendLine("                    </ul>");
    sb.AppendLine("                </div>");
    sb.AppendLine("            </div>");

    // Powered-by header with toggle conditional and text
    sb.AppendLine("            <?php if (!empty($settings['show_powered_by'])) { ?>");
    sb.AppendLine("            <div class=\"powered-by-header\">");
    sb.AppendLine("                <span>Powered by ModernUO</span>");
    sb.AppendLine("                <img src=\"images/muologo.png\" alt=\"ModernUO Logo\" class=\"modernuo-logo\">");
    sb.AppendLine("            </div>");
    sb.AppendLine("            <?php } ?>");

    // Content wrapper with three-column layout
    sb.AppendLine("            <div class=\"content-wrapper-index\">");
    sb.AppendLine("                <div class=\"status-section-index\">");
    sb.AppendLine("                    <h3><b><u>Server Status</u></b></h3>");
    sb.AppendLine("                    <?php");
    sb.AppendLine("                    if ($jsonData) {");
    sb.AppendLine("                        $isOnline = (new DateTime($jsonData['Shard']['Time']))->getTimestamp() > (time() - 45);");
    sb.AppendLine("                        echo '<p>Name: ' . htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server') . '</p>';");
    sb.AppendLine("                        echo '<p>Status: <span class=\"' . ($isOnline ? 'status-online' : 'status-offline') . '\">' . ($isOnline ? 'Online' : 'Offline') . '</span></p>';");
    sb.AppendLine("                        echo '<p>Port: ' . htmlspecialchars($jsonData['Shard']['Port']) . '</p>';");
    sb.AppendLine("                        echo '<p>Uptime: ' . ($isOnline ? htmlspecialchars($jsonData['Shard']['UptimeMinutes']) : '0') . ' minutes</p>';");
    sb.AppendLine("                        echo '<p>Online Players: ' . ($isOnline ? count(array_filter($jsonData['Players'], fn($p) => $p['IsOnline'])) : '0') . '</p>';");
    sb.AppendLine("                        echo '<p>Total Players: ' . count($jsonData['Players']) . '</p>';");
    sb.AppendLine("                        echo '<p>Guilds: ' . count($jsonData['Guilds']) . '</p>';");
    sb.AppendLine("                        echo '<p>Max Online: ' . htmlspecialchars($max_online_from_db) . '</p>';");
    sb.AppendLine("                    } else {");
    sb.AppendLine("                        echo '<p>Name: Unknown Server</p><p>Status: <span class=\"status-offline\">Offline</span></p><p>Port: 2593</p><p>Uptime: 0 minutes</p>';");
    sb.AppendLine("                        echo '<p>Online Players: 0</p><p>Total Players: 0</p><p>Guilds: 0</p><p>Max Online: ' . htmlspecialchars($max_online_from_db) . '</p>';");
    sb.AppendLine("                    }");
    sb.AppendLine("                    ?>");
    sb.AppendLine("                </div>");
    sb.AppendLine("                <div class=\"main-section-index\">");
    sb.AppendLine("                    <div class=\"content-section-index\">");
    sb.AppendLine("                        <div class=\"content-header\">");
    sb.AppendLine("                            <?php");
    sb.AppendLine("                            $homepage_lines = explode(\"\\n\", htmlspecialchars($settings['homepage_text']));");
    sb.AppendLine("                            echo '<h1>' . trim($homepage_lines[0]) . '</h1>';");
    sb.AppendLine("                            if (isset($homepage_lines[1])) {");
    sb.AppendLine("                                echo '<p>' . trim($homepage_lines[1]) . '</p>';");
    sb.AppendLine("                            }");
    sb.AppendLine("                            ?>");
    sb.AppendLine("                        </div>");
    sb.AppendLine("                        <?php if ($settings['show_download_button']) { ?>");
    sb.AppendLine("                        <a href=\"client.php\" class=\"download-button\"><?php echo htmlspecialchars($settings['download_button_text']); ?></a>");
    sb.AppendLine("                        <?php } ?>");
    sb.AppendLine("                    </div>");
    sb.AppendLine("                </div>");
    sb.AppendLine("                <div class=\"right-spacer-index\"></div>");
    sb.AppendLine("            </div>");
    sb.AppendLine("        </div>");

    // Footer with dynamic social media links
    sb.AppendLine("        <div class=\"footer\">");
    sb.AppendLine("            <div class=\"footer-container\">");
    sb.AppendLine("                <div class=\"social-icons\">");
    sb.AppendLine("                    <?php");
    // Add isset checks to avoid undefined key warnings
    sb.AppendLine("                    if (isset($settings['social_visibility']['facebook']) && $settings['social_visibility']['facebook']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['facebook']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/facebook.png\" alt=\"Facebook\" class=\"social-icon-img\"></a>';");
    sb.AppendLine("                    if (isset($settings['social_visibility']['youtube']) && $settings['social_visibility']['youtube']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['youtube']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/youtube-play.png\" alt=\"YouTube\" class=\"social-icon-img\"></a>';");
    sb.AppendLine("                    if (isset($settings['social_visibility']['tiktok']) && $settings['social_visibility']['tiktok']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['tiktok']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/tiktok.png\" alt=\"TikTok\" class=\"social-icon-img\"></a>';");
    sb.AppendLine("                    if (isset($settings['social_visibility']['discord']) && $settings['social_visibility']['discord']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['discord']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/discord.png\" alt=\"Discord\" class=\"social-icon-img\"></a>';");
    sb.AppendLine("                    if (isset($settings['social_visibility']['twitter']) && $settings['social_visibility']['twitter']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['twitter']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/?size=43&id=phOKFKYpe00C&format=png&color=000000\" alt=\"Twitter\" class=\"social-icon-img\"></a>';");
    sb.AppendLine("                    ?>");
    sb.AppendLine("                </div>");
    sb.AppendLine("                <p>Copyright <?php echo htmlspecialchars($settings['footer_copyright_domain'] ?? 'mydomain.com'); ?></p>");
    sb.AppendLine("                <div class=\"powered-by\">");
    sb.AppendLine("                    <span>Compatible</span>");
    sb.AppendLine("                    <div class=\"powered-by-images\">");
    sb.AppendLine("                        <img src=\"images/razor.png\" alt=\"Razor\">");
    sb.AppendLine("                        <img src=\"images/Uosteamlogo.png\" alt=\"UOsteam\" class=\"uosteam-icon\">");
    sb.AppendLine("                        <img src=\"images/classicUOLogo.png\" alt=\"ClassicUO\" class=\"classicuo-icon\">");
    sb.AppendLine("                    </div>");
    sb.AppendLine("                </div>");
    sb.AppendLine("            </div>");
    sb.AppendLine("        </div>");
    sb.AppendLine("    </div>");

    // JavaScript for server status updates
    sb.AppendLine("    <script>");
    sb.AppendLine("        setInterval(() => fetch('index.php?json').then(r => r.json()).then(data => {");
    sb.AppendLine("            const isOnline = new Date(data.Shard.Time) > new Date() - 45000;");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(1)').innerText = 'Name: ' + (data.Shard.ServerName || 'Unknown Server');");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(2) span').innerText = isOnline ? 'Online' : 'Offline';");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(2) span').className = isOnline ? 'status-online' : 'status-offline';");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(3)').innerText = 'Port: ' + data.Shard.Port;");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(4)').innerText = 'Uptime: ' + (isOnline ? data.Shard.UptimeMinutes : 0) + ' minutes';");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(5)').innerText = 'Online Players: ' + (isOnline ? data.Players.filter(p => p.IsOnline).length : 0);");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(6)').innerText = 'Total Players: ' + data.Players.length;");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(7)').innerText = 'Guilds: ' + data.Guilds.length;");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(8)').innerText = 'Max Online: ' + data.Shard.MaxOnline;");
    sb.AppendLine("        }).catch(err => {");
    sb.AppendLine("            console.error('Failed to fetch server status:', err);");
    sb.AppendLine("            document.querySelector('.status-section-index p:nth-child(1)').innerText = 'Name: Unknown Server';");
    sb.AppendLine("        }), 30000);");
    sb.AppendLine("    </script>");

    sb.AppendLine("</body>");
    sb.AppendLine("</html>");

    return sb.ToString();
}
    }
}
