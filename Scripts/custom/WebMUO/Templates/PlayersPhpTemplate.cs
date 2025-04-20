using System.Text;

namespace Server.MUOTemplates
{
    public class PlayersPhpTemplate : BasePhpTemplate
    {
        private readonly MUOTemplateWriter.TemplateConfig _config;

        public PlayersPhpTemplate(string serverName, string port, MUOTemplateWriter.TemplateConfig config)
            : base(serverName, port)
        {
            _config = config;
        }

        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var template = new PlayersPhpTemplate(config.ServerName, config.Port, config);
            return template.GenerateTemplate();
        }

        private string GenerateTemplate()
        {
            var sb = new StringBuilder();
            AppendPhpHeader(sb, "players_errors.log", true, true, _config);

            // Handle JSON request for player updates
            sb.AppendLine("<?php");
            sb.AppendLine("if (isset($_GET['json'])) {");
            sb.AppendLine("    $shardDataFile = __DIR__ . '/sharddata.json';");
            sb.AppendLine("    $jsonData = [];");
            sb.AppendLine("    if (file_exists($shardDataFile)) {");
            sb.AppendLine("        $json = file_get_contents($shardDataFile);");
            sb.AppendLine("        $jsonData = json_decode($json, true) ?? [];");
            sb.AppendLine("    }");
            sb.AppendLine("    header('Access-Control-Allow-Origin: *');");
            sb.AppendLine("    header('Content-Type: application/json');");
            sb.AppendLine("    echo json_encode($jsonData);");
            sb.AppendLine("    exit;");
            sb.AppendLine("}");

            // Load settings from sitesettings.json
            sb.AppendLine("$activePage = 'players';");
            sb.AppendLine("$settingsFile = __DIR__ . '/sitesettings.json';");
            sb.AppendLine("$settings = [");
            sb.AppendLine("    'menu_visibility' => [],");
            sb.AppendLine("    'social_visibility' => [],");
            sb.AppendLine("    'show_background_image' => true,");
            sb.AppendLine("    'show_powered_by' => true,");
            sb.AppendLine("    'footer_copyright_domain' => 'mydomain.com',");
            sb.AppendLine("    'background_color' => '#000000',");
            sb.AppendLine("    'text_color' => '#ffffff',");
            sb.AppendLine("    'theme' => 'dark',");
            sb.AppendLine("    'social_urls' => [");
            sb.AppendLine("        'discord' => 'https://discord.com',");
            sb.AppendLine("        'youtube' => 'https://youtube.com',");
            sb.AppendLine("        'tiktok' => 'https://tiktok.com',");
            sb.AppendLine("        'twitter' => 'https://x.com',");
            sb.AppendLine("        'facebook' => 'https://facebook.com'");
            sb.AppendLine("    ],");
            sb.AppendLine("    'menu_urls' => [");
            sb.AppendLine("        'wiki' => 'https://wiki.example.com',");
            sb.AppendLine("        'forum' => 'https://forum.example.com'");
            sb.AppendLine("    ]");
            sb.AppendLine("];");
            sb.AppendLine("if (file_exists($settingsFile)) {");
            sb.AppendLine("    $json = file_get_contents($settingsFile);");
            sb.AppendLine("    $decoded = json_decode($json, true);");
            sb.AppendLine("    if (is_array($decoded)) {");
            sb.AppendLine("        $settings = array_merge($settings, $decoded);");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            // Load shard data for initial render
            sb.AppendLine("$shardDataFile = __DIR__ . '/sharddata.json';");
            sb.AppendLine("$jsonData = [];");
            sb.AppendLine("if (file_exists($shardDataFile)) {");
            sb.AppendLine("    $json = file_get_contents($shardDataFile);");
            sb.AppendLine("    $jsonData = json_decode($json, true) ?? [];");
            sb.AppendLine("}");
            sb.AppendLine("$isServerOnline = isset($jsonData['Shard']['Time']) ? (new DateTime($jsonData['Shard']['Time']))->getTimestamp() > (time() - 45) : false;");
            sb.AppendLine("?>");

            // HTML structure
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\" />");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
            sb.AppendLine("    <title><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?> Players</title>");
            sb.AppendLine("    <link rel=\"stylesheet\" href=\"styles.css\">");
            sb.AppendLine("    <style>");
            sb.AppendLine("        :root {");
            sb.AppendLine("            --text-color: <?php echo htmlspecialchars($settings['text_color']); ?>;");
            sb.AppendLine("        }");
            sb.AppendLine("        html, body { margin: 0; padding: 0; background: <?php echo $settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center ' . htmlspecialchars($settings['background_color']) : htmlspecialchars($settings['background_color']); ?>; background-size: cover; color: var(--text-color); }");
            sb.AppendLine("        .container, .main-content, .top-bar, .menu ul li a, .powered-by-header, .footer { color: var(--text-color); }");
            sb.AppendLine("        .content-section-players h1, .content-section-players table th, .content-section-players table td, .content-header h1, .content-header p { color: var(--text-color) !important; }");
            // Layout styles to match Updates page
            sb.AppendLine("        .content-wrapper-players { display: flex; flex: 1; overflow: hidden; position: relative; margin-top: 60px; margin-bottom: 60px; }");
            sb.AppendLine("        .main-section-players { flex: 0 0 100%; display: flex; flex-direction: column; margin: 0; width: 100%; min-height: calc(100vh - 120px); padding: 0; }");
            sb.AppendLine("        .content-section-players { display: flex; flex-direction: column; width: 100%; max-width: 90vw; margin: 0 auto; height: 100%; }");
            // Content-header styling to match Updates page
            sb.AppendLine("        .content-header { position: sticky; top: 0; background: rgba(0, 0, 0, 0.7); padding: 10px 0; z-index: 10; text-align: center; width: 100%; max-width: 90vw; margin: 0 auto; box-sizing: border-box; }");
            sb.AppendLine("        .content-header h1 { margin: 0; font-size: 2.5em; }");
            sb.AppendLine("        .content-header p { margin: 5px 0 0; font-size: 1.2em; }");
            // Style the table container to align with the content-header
            sb.AppendLine("        .table-container-players { margin-top: 10px; width: 100%; max-width: 90vw; background: rgba(0, 0, 0, 0.7); padding: 15px; border: 1px solid #333; overflow-y: auto; flex-grow: 1; max-height: calc(100vh - 220px); box-sizing: border-box; margin-left: auto; margin-right: auto; }");
            sb.AppendLine("        .table-container-players table { width: 100%; border-collapse: collapse; table-layout: auto; }");
            sb.AppendLine("        .table-container-players th, .table-container-players td { padding: 10px; text-align: left; border-bottom: 1px solid #333; white-space: nowrap; }");
            sb.AppendLine("        .table-container-players th { background: #222; }");
            sb.AppendLine("        .table-container-players img { height: 24px; width: 24px; margin-right: 5px; vertical-align: middle; }");
            sb.AppendLine("        @media (max-width: 768px) {");
            sb.AppendLine("            .content-section-players { padding: 10px; max-width: 100%; }");
            sb.AppendLine("            .content-header { max-width: 100%; padding: 10px; }");
            sb.AppendLine("            .content-header h1 { font-size: 2em; }");
            sb.AppendLine("            .content-header p { font-size: 1em; }");
            sb.AppendLine("            .table-container-players { padding: 10px; max-width: 100%; max-height: calc(100vh - 200px); }");
            sb.AppendLine("            .table-container-players th, .table-container-players td { padding: 8px; font-size: 0.9em; }");
            sb.AppendLine("        }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body class=\"theme-<?php echo htmlspecialchars($settings['theme']); ?>\">");
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <div class=\"main-content\">");

            // Shared header (top bar with logo and updated menu with dynamic URLs)
            sb.AppendLine("            <div class=\"top-bar\">");
            sb.AppendLine("                <div class=\"logo\">");
            sb.AppendLine("                    <img src=\"images/logo.png\" alt=\"ModernUO Logo\">");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class=\"menu\">");
            sb.AppendLine("                    <ul>");
            sb.AppendLine("                        <?php");
            sb.AppendLine("                        if ($settings['menu_visibility']['index']) {");
            sb.AppendLine("                            echo \"<li><a href='index.php' class='\" . ($activePage == 'index' ? 'active' : '') . \"'>Home</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['players']) {");
            sb.AppendLine("                            echo \"<li><a href='players.php' class='\" . ($activePage == 'players' ? 'active' : '') . \"'>Players</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['guilds']) {");
            sb.AppendLine("                            echo \"<li><a href='guilds.php' class='\" . ($activePage == 'guilds' ? 'active' : '') . \"'>Guilds</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['forum']) {");
            sb.AppendLine("                            echo \"<li><a href='\" . htmlspecialchars($settings['menu_urls']['forum']) . \"' class='\" . ($activePage == 'forum' ? 'active' : '') . \"'>Forum</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['wiki']) {");
            sb.AppendLine("                            echo \"<li><a href='\" . htmlspecialchars($settings['menu_urls']['wiki']) . \"' class='\" . ($activePage == 'wiki' ? 'active' : '') . \"'>Wiki</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['uotools']) {");
            sb.AppendLine("                            echo \"<li><a href='uotools.php' class='\" . ($activePage == 'uotools' ? 'active' : '') . \"'>UO Tools</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['social']) {");
            sb.AppendLine("                            echo \"<li><a href='social.php' class='\" . ($activePage == 'social' ? 'active' : '') . \"'>Updates</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['contact']) {");
            sb.AppendLine("                            echo \"<li><a href='contact.php' class='\" . ($activePage == 'contact' ? 'active' : '') . \"'>Contact</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if ($settings['menu_visibility']['client']) {");
            sb.AppendLine("                            echo \"<li><a href='client.php' class='\" . ($activePage == 'client' ? 'active' : '') . \"'>Client</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        if (isset($_SESSION['admin_logged_in']) && $_SESSION['admin_logged_in']) {");
            sb.AppendLine("                            echo \"<li><a href='admin.php' class='\" . ($activePage == 'admin' ? 'active' : '') . \"'>Admin</a></li>\";");
            sb.AppendLine("                        }");
            sb.AppendLine("                        ?>");
            sb.AppendLine("                    </ul>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");

            // Shared powered-by header with toggle
            sb.AppendLine("            <?php if (!empty($settings['show_powered_by'])) { ?>");
            sb.AppendLine("            <div class=\"powered-by-header\">");
            sb.AppendLine("                <span>Powered by ModernUO</span>");
            sb.AppendLine("                <img src=\"images/muologo.png\" alt=\"ModernUO Logo\" class=\"modernuo-logo\">");
            sb.AppendLine("            </div>");
            sb.AppendLine("            <?php } ?>");

            // Unique content wrapper for players page
            sb.AppendLine("            <div class=\"content-wrapper-players\">");
            sb.AppendLine("                <div class=\"main-section-players\">");
            sb.AppendLine("                    <div class=\"content-section-players table-section-players\">");
            sb.AppendLine("                        <div class=\"content-header\">");
            sb.AppendLine("                            <h1><?php echo htmlspecialchars($jsonData['Shard']['ServerName'] ?? 'Unknown Server'); ?> Players</h1>");
            sb.AppendLine("                            <p>List of players on the shard.</p>");
            sb.AppendLine("                        </div>");
            sb.AppendLine("                        <div class=\"table-container-players\">");
            sb.AppendLine("                            <table>");
            sb.AppendLine("                                <thead><tr><th>Status</th><th>Name</th><th>Title</th><th>Guild</th><th>Abbr</th><th>Kills</th><th>Fame</th><th>Karma</th><th>Last Login</th></tr></thead>");
            sb.AppendLine("                                <tbody id=\"players-table-body\">");
            sb.AppendLine("                                    <?php");
            sb.AppendLine("                                    if ($jsonData && !empty($jsonData['Players'])) {");
            sb.AppendLine("                                        usort($jsonData['Players'], fn($a, $b) => $b['IsOnline'] <=> $a['IsOnline'] ?: strcmp($a['Name'], $b['Name']));");
            sb.AppendLine("                                        foreach ($jsonData['Players'] as $p) {");
            sb.AppendLine("                                            if (!isset($p['Serial']) || $p['Serial'] <= 0) {");
            sb.AppendLine("                                                logDebug('Invalid serial for player: ' . ($p['Name'] ?? 'unknown') . ', Serial: ' . ($p['Serial'] ?? 'null'));");
            sb.AppendLine("                                                continue;");
            sb.AppendLine("                                            }");
            sb.AppendLine("                                            $link = 'paperdoll.php?id=' . htmlspecialchars($p['Serial']);");
            sb.AppendLine("                                            $icon = 'images/icon_' . strtolower(htmlspecialchars($p['RaceGender'] ?? 'unknown')) . '.png';");
            sb.AppendLine("                                            $playerOnline = $isServerOnline && ($p['IsOnline'] ?? false);");
            sb.AppendLine("                                            $nameColor = ($p['Kills'] ?? 0) >= 5 ? 'red' : 'lightblue';");
            sb.AppendLine("                                            $lastLogin = '';");
            sb.AppendLine("                                            if (!empty($p['LastLogin'])) {");
            sb.AppendLine("                                                try {");
            sb.AppendLine("                                                    $date = new DateTime($p['LastLogin']);");
            sb.AppendLine("                                                    $lastLogin = $date->format('Y-m-d');");
            sb.AppendLine("                                                } catch (Exception $e) {");
            sb.AppendLine("                                                    $lastLogin = '';");
            sb.AppendLine("                                                }");
            sb.AppendLine("                                            }");
            sb.AppendLine("                                            echo '<tr data-serial=\"' . htmlspecialchars($p['Serial']) . '\">';");
            sb.AppendLine("                                            echo '<td><span class=\"status-dot ' . ($playerOnline ? 'online' : 'offline') . '\"></span></td>';");
            sb.AppendLine("                                            echo '<td><img class=\"player-icon\" src=\"' . $icon . '\" alt=\"' . htmlspecialchars($p['RaceGender'] ?? 'unknown') . '\" /><a href=\"' . $link . '\" style=\"color:' . $nameColor . '\" data-serial=\"' . htmlspecialchars($p['Serial']) . '\">' . htmlspecialchars($p['Name'] ?? 'Unknown') . '</a></td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($p['Title'] ?? '') . '</td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($p['Guild'] ?? '') . '</td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($p['GuildAbbr'] ?? '') . '</td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($p['Kills'] ?? 0) . '</td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($p['Fame'] ?? 0) . '</td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($p['Karma'] ?? 0) . '</td>';");
            sb.AppendLine("                                            echo '<td>' . htmlspecialchars($lastLogin) . '</td>';");
            sb.AppendLine("                                            echo '</tr>';");
            sb.AppendLine("                                        }");
            sb.AppendLine("                                    } else {");
            sb.AppendLine("                                        echo '<tr><td colspan=\"9\">No players found.</td></tr>';");
            sb.AppendLine("                                    }");
            sb.AppendLine("                                    ?>");
            sb.AppendLine("                                </tbody>");
            sb.AppendLine("                            </table>");
            sb.AppendLine("                        </div>");
            sb.AppendLine("                    </div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");

            // Shared footer with dynamic social media links
            sb.AppendLine("        <div class=\"footer\">");
            sb.AppendLine("            <div class=\"footer-container\">");
            sb.AppendLine("                <div class=\"social-icons\">");
            sb.AppendLine("                    <?php");
            sb.AppendLine("                    if ($settings['social_visibility']['facebook']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['facebook']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/facebook.png\" alt=\"Facebook\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("                    if ($settings['social_visibility']['youtube']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['youtube']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/youtube-play.png\" alt=\"YouTube\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("                    if ($settings['social_visibility']['tiktok']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['tiktok']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/tiktok.png\" alt=\"TikTok\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("                    if ($settings['social_visibility']['discord']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['discord']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/discord.png\" alt=\"Discord\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("                    if ($settings['social_visibility']['twitter']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['twitter']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/?size=48&id=phOKFKYpe00C&format=png&color=000000\" alt=\"Twitter\" class=\"social-icon-img\"></a>';");
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

            // JavaScript for player updates and link logging
            sb.AppendLine("    <script>");
            sb.AppendLine("        // Function to escape HTML to prevent XSS");
            sb.AppendLine("        function escapeHtml(unsafe) {");
            sb.AppendLine("            if (typeof unsafe !== 'string') return '';");
            sb.AppendLine("            return unsafe");
            sb.AppendLine("                .replace(/&/g, '&amp;')");
            sb.AppendLine("                .replace(/</g, '&lt;')");
            sb.AppendLine("                .replace(/>/g, '&gt;')");
            sb.AppendLine("                .replace(/\"/g, '&quot;')");
            sb.AppendLine("                .replace(/'/g, '&#39;');");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        // Player link logging");
            sb.AppendLine("        function attachLinkListeners() {");
            sb.AppendLine("            document.querySelectorAll('a[href^=\"paperdoll.php\"]').forEach(link => {");
            sb.AppendLine("                link.addEventListener('click', (e) => {");
            sb.AppendLine("                    const href = link.getAttribute('href');");
            sb.AppendLine("                    const serial = link.getAttribute('data-serial');");
            sb.AppendLine("                    console.log('Clicked player link - Serial: ' + serial + ', URL: ' + href);");
            sb.AppendLine("                });");
            sb.AppendLine("            });");
            sb.AppendLine("        }");
            sb.AppendLine("        attachLinkListeners();");
            sb.AppendLine("");
            sb.AppendLine("        // Dynamic player status updates");
            sb.AppendLine("        setInterval(() => {");
            sb.AppendLine("            fetch('players.php?json')");
            sb.AppendLine("                .then(r => r.json())");
            sb.AppendLine("                .then(data => {");
            sb.AppendLine("                    const isServerOnline = new Date(data.Shard.Time) > new Date() - 45000;");
            sb.AppendLine("                    const players = data.Players || [];");
            sb.AppendLine("                    players.sort((a, b) => (b.IsOnline ? 1 : 0) - (a.IsOnline ? 1 : 0) || a.Name.localeCompare(b.Name));");
            sb.AppendLine("                    const tbody = document.getElementById('players-table-body');");
            sb.AppendLine("                    tbody.innerHTML = '';");
            sb.AppendLine("                    if (players.length === 0) {");
            sb.AppendLine("                        const row = document.createElement('tr');");
            sb.AppendLine("                        row.innerHTML = '<td colspan=\"9\">No players found.</td>';");
            sb.AppendLine("                        tbody.appendChild(row);");
            sb.AppendLine("                        return;");
            sb.AppendLine("                    }");
            sb.AppendLine("                    players.forEach(p => {");
            sb.AppendLine("                        if (!p.Serial || p.Serial <= 0) return;");
            sb.AppendLine("                        const playerOnline = isServerOnline && (p.IsOnline || false);");
            sb.AppendLine("                        const nameColor = (p.Kills || 0) >= 5 ? 'red' : 'lightblue';");
            sb.AppendLine("                        let lastLogin = '';");
            sb.AppendLine("                        if (p.LastLogin) {");
            sb.AppendLine("                            try {");
            sb.AppendLine("                                const date = new Date(p.LastLogin);");
            sb.AppendLine("                                lastLogin = date.toISOString().split('T')[0];");
            sb.AppendLine("                            } catch (e) {");
            sb.AppendLine("                                console.error('Invalid LastLogin format:', p.LastLogin, e);");
            sb.AppendLine("                            }");
            sb.AppendLine("                        }");
            sb.AppendLine("                        const row = document.createElement('tr');");
            sb.AppendLine("                        row.setAttribute('data-serial', escapeHtml(p.Serial.toString()));");
            sb.AppendLine("                        row.innerHTML = [");
            sb.AppendLine("                            '<td><span class=\"status-dot ' + (playerOnline ? 'online' : 'offline') + '\"></span></td>',");
            sb.AppendLine("                            '<td><img class=\"player-icon\" src=\"images/icon_' + escapeHtml((p.RaceGender || 'unknown').toLowerCase()) + '.png\" alt=\"' + escapeHtml(p.RaceGender || 'unknown') + '\" /><a href=\"paperdoll.php?id=' + escapeHtml(p.Serial.toString()) + '\" style=\"color:' + nameColor + '\" data-serial=\"' + escapeHtml(p.Serial.toString()) + '\">' + escapeHtml(p.Name || 'Unknown') + '</a></td>',");
            sb.AppendLine("                            '<td>' + escapeHtml(p.Title || '') + '</td>',");
            sb.AppendLine("                            '<td>' + escapeHtml(p.Guild || '') + '</td>',");
            sb.AppendLine("                            '<td>' + escapeHtml(p.GuildAbbr || '') + '</td>',");
            sb.AppendLine("                            '<td>' + escapeHtml((p.Kills || 0).toString()) + '</td>',");
            sb.AppendLine("                            '<td>' + escapeHtml((p.Fame || 0).toString()) + '</td>',");
            sb.AppendLine("                            '<td>' + escapeHtml((p.Karma || 0).toString()) + '</td>',");
            sb.AppendLine("                            '<td>' + escapeHtml(lastLogin) + '</td>'");
            sb.AppendLine("                        ].join('');");
            sb.AppendLine("                        tbody.appendChild(row);");
            sb.AppendLine("                    });");
            sb.AppendLine("                    attachLinkListeners();");
            sb.AppendLine("                })");
            sb.AppendLine("                .catch(err => {");
            sb.AppendLine("                    console.error('Failed to fetch player data:', err);");
            sb.AppendLine("                    const tbody = document.getElementById('players-table-body');");
            sb.AppendLine("                    tbody.innerHTML = '<tr><td colspan=\"9\">Error loading players.</td></tr>';");
            sb.AppendLine("                });");
            sb.AppendLine("        }, 30000);");
            sb.AppendLine("    </script>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
