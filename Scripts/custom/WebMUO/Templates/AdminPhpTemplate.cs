using System.Text;

namespace Server.MUOTemplates
{
    public class AdminPhpTemplate : BasePhpTemplate
    {
        private readonly MUOTemplateWriter.TemplateConfig _config;

        public AdminPhpTemplate(string serverName, string port, MUOTemplateWriter.TemplateConfig config)
            : base(serverName, port)
        {
            _config = config;
        }

        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var template = new AdminPhpTemplate(config.ServerName, config.Port, config);
            return template.GenerateTemplate();
        }

        private string GenerateTemplate()
        {
            var sb = new StringBuilder();
            AppendPhpHeader(sb, "admin_errors.log", true, true, _config);

            sb.AppendLine("<?php");
            sb.AppendLine("ob_start();");
            sb.AppendLine("$activePage = 'admin';");
            sb.AppendLine("session_start();");
            sb.AppendLine("$settingsFile = __DIR__ . '/sitesettings.json';");
            sb.AppendLine("$updatesFile = __DIR__ . '/shard_updates.json';");
            sb.AppendLine("$toolsFile = __DIR__ . '/tools.json';");
            sb.AppendLine("$settings = [");
            sb.AppendLine("  'menu_visibility' => [");
            sb.AppendLine("    'index' => false,");
            sb.AppendLine("    'players' => false,");
            sb.AppendLine("    'guilds' => false,");
            sb.AppendLine("    'forum' => false,");
            sb.AppendLine("    'wiki' => false,");
            sb.AppendLine("    'uotools' => false,");
            sb.AppendLine("    'social' => false,");
            sb.AppendLine("    'client' => false,");
            sb.AppendLine("    'contact' => false");
            sb.AppendLine("  ],");
            sb.AppendLine("  'social_visibility' => [],");
            sb.AppendLine("  'show_background_image' => true,");
            sb.AppendLine("  'show_powered_by' => true,");
            sb.AppendLine("  'footer_copyright_domain' => 'mydomain.com',");
            sb.AppendLine("  'client_host' => 'localhost',");
            sb.AppendLine("  'client_port' => '2593',");
            sb.AppendLine("  'background_color' => '#000000',");
            sb.AppendLine("  'text_color' => '#ffffff',");
            sb.AppendLine("  'theme' => 'dark',");
            sb.AppendLine("  'homepage_text' => \"Welcome to WebMUO\\nThis is the official page for our Ultima Online shard. Explore the menu to view player stats, guild information, download our custom client, or get in touch!\",");
            sb.AppendLine("  'show_download_button' => true,");
            sb.AppendLine("  'download_button_color' => '#FFFF00',");
            sb.AppendLine("  'download_button_text' => 'DOWNLOAD CLIENT',");
            sb.AppendLine("  'client_download_button_color' => '#FFFF00',");
            sb.AppendLine("  'client_download_url' => 'https://example.com/download/client.zip',");
            sb.AppendLine("  'client_text_above_button' => 'Get our custom Ultima Online client to join the shard.',");
            sb.AppendLine("  'client_text_below_button' => 'Ensure you have the latest version for the best experience.',");
            sb.AppendLine("  'contact_emails' => ['support@yourserver.com'],");
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
            sb.AppendLine("$tools = [];");
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
            sb.AppendLine("if (file_exists($toolsFile)) {");
            sb.AppendLine("  $json = file_get_contents($toolsFile);");
            sb.AppendLine("  $tools = json_decode($json, true) ?: [];");
            sb.AppendLine("}");

            // Handle form submissions
            sb.AppendLine("if ($_SERVER['REQUEST_METHOD'] === 'POST') {");
            sb.AppendLine("  if (isset($_POST['login'])) {");
            sb.AppendLine("    if (isset($_POST['password']) && $_POST['password'] === $adminPassword) {");
            sb.AppendLine("      $_SESSION['admin_logged_in'] = true;");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } else { $error = 'Invalid password'; }");
            sb.AppendLine("  } elseif (isset($_POST['logout'])) {");
            sb.AppendLine("    session_destroy();");
            sb.AppendLine("    header('Location: admin.php'); exit;");
            sb.AppendLine("  } elseif (isset($_SESSION['admin_logged_in']) && $_SESSION['admin_logged_in']) {");
            sb.AppendLine("    $uploadDir = __DIR__ . '/images/';");
            sb.AppendLine("    $allowedTypes = ['image/png'];");
            sb.AppendLine("    if (!is_dir($uploadDir)) {");
            sb.AppendLine("      mkdir($uploadDir, 0755, true);");
            sb.AppendLine("    }");
            sb.AppendLine("    $uploadErrors = [];");
            // Logo upload
            sb.AppendLine("    if (!empty($_FILES['logo_image']['name'])) {");
            sb.AppendLine("      $logoFile = $_FILES['logo_image'];");
            sb.AppendLine("      if ($logoFile['error'] === UPLOAD_ERR_OK) {");
            sb.AppendLine("        if (in_array($logoFile['type'], $allowedTypes)) {");
            sb.AppendLine("          $logoPath = $uploadDir . 'logo.png';");
            sb.AppendLine("          if (move_uploaded_file($logoFile['tmp_name'], $logoPath)) {");
            sb.AppendLine("            // Logo uploaded successfully");
            sb.AppendLine("          } else {");
            sb.AppendLine("            $uploadErrors[] = 'Failed to upload logo image.';");
            sb.AppendLine("          }");
            sb.AppendLine("        } else {");
            sb.AppendLine("          $uploadErrors[] = 'Logo image must be a PNG file.';");
            sb.AppendLine("        }");
            sb.AppendLine("      } else {");
            sb.AppendLine("        $uploadErrors[] = 'Logo upload error: ' . $logoFile['error'];");
            sb.AppendLine("      }");
            sb.AppendLine("    }");
            // Background upload
            sb.AppendLine("    if (!empty($_FILES['background_image']['name'])) {");
            sb.AppendLine("      $bgFile = $_FILES['background_image'];");
            sb.AppendLine("      if ($bgFile['error'] === UPLOAD_ERR_OK) {");
            sb.AppendLine("        if (in_array($bgFile['type'], $allowedTypes)) {");
            sb.AppendLine("          $bgPath = $uploadDir . 'background.png';");
            sb.AppendLine("          if (move_uploaded_file($bgFile['tmp_name'], $bgPath)) {");
            sb.AppendLine("            // Background uploaded successfully");
            sb.AppendLine("          } else {");
            sb.AppendLine("            $uploadErrors[] = 'Failed to upload background image.';");
            sb.AppendLine("          }");
            sb.AppendLine("        } else {");
            sb.AppendLine("          $uploadErrors[] = 'Background image must be a PNG file.';");
            sb.AppendLine("        }");
            sb.AppendLine("      } else {");
            sb.AppendLine("        $uploadErrors[] = 'Background upload error: ' . $bgFile['error'];");
            sb.AppendLine("      }");
            sb.AppendLine("    }");
            sb.AppendLine("    if (!empty($uploadErrors)) {");
            sb.AppendLine("      $error = implode('<br>', array_map('htmlspecialchars', $uploadErrors));");
            sb.AppendLine("    }");
            sb.AppendLine("    if (isset($_POST['apply_settings'])) {");
            sb.AppendLine("      $settings['show_background_image'] = isset($_POST['show_background_image']);");
            sb.AppendLine("      $settings['show_powered_by'] = isset($_POST['show_powered_by']);");
            sb.AppendLine("      $settings['footer_copyright_domain'] = !empty($_POST['footer_copyright_domain']) ? htmlspecialchars(strip_tags($_POST['footer_copyright_domain'])) : 'mydomain.com';");
            sb.AppendLine("      $settings['client_host'] = !empty($_POST['client_host']) ? htmlspecialchars(strip_tags($_POST['client_host'])) : 'localhost';");
            sb.AppendLine("      $settings['client_port'] = !empty($_POST['client_port']) && is_numeric($_POST['client_port']) ? htmlspecialchars(strip_tags($_POST['client_port'])) : '2593';");
            sb.AppendLine("      $settings['background_color'] = preg_match('/^#[0-9A-Fa-f]{6}$/', $_POST['background_color'] ?? '') ? $_POST['background_color'] : '#000000';");
            sb.AppendLine("      $settings['text_color'] = preg_match('/^#[0-9A-Fa-f]{6}$/', $_POST['text_color'] ?? '') ? $_POST['text_color'] : '#ffffff';");
            sb.AppendLine("      $settings['homepage_text'] = !empty($_POST['homepage_text']) ? htmlspecialchars(strip_tags($_POST['homepage_text'])) : $settings['homepage_text'];");
            sb.AppendLine("      $settings['show_download_button'] = isset($_POST['show_download_button']);");
            sb.AppendLine("      $settings['download_button_color'] = preg_match('/^#[0-9A-Fa-f]{6}$/', $_POST['download_button_color'] ?? '') ? $_POST['download_button_color'] : '#FFFF00';");
            sb.AppendLine("      $settings['download_button_text'] = !empty($_POST['download_button_text']) ? htmlspecialchars(strip_tags($_POST['download_button_text'])) : 'DOWNLOAD CLIENT';");
            sb.AppendLine("      $settings['client_download_button_color'] = preg_match('/^#[0-9A-Fa-f]{6}$/', $_POST['client_download_button_color'] ?? '') ? $_POST['client_download_button_color'] : '#FFFF00';");
            sb.AppendLine("      $settings['client_download_url'] = !empty($_POST['client_download_url']) ? htmlspecialchars(strip_tags($_POST['client_download_url'])) : 'https://example.com/download/client.zip';");
            sb.AppendLine("      $settings['client_text_above_button'] = !empty($_POST['client_text_above_button']) ? htmlspecialchars(strip_tags($_POST['client_text_above_button'])) : $settings['client_text_above_button'];");
            sb.AppendLine("      $settings['client_text_below_button'] = !empty($_POST['client_text_below_button']) ? htmlspecialchars(strip_tags($_POST['client_text_below_button'])) : $settings['client_text_below_button'];");
            sb.AppendLine("      $settings['menu_visibility'] = [");
            sb.AppendLine("        'index' => isset($_POST['menu_index']),");
            sb.AppendLine("        'players' => isset($_POST['menu_players']),");
            sb.AppendLine("        'guilds' => isset($_POST['menu_guilds']),");
            sb.AppendLine("        'forum' => isset($_POST['menu_forum']),");
            sb.AppendLine("        'wiki' => isset($_POST['menu_wiki']),");
            sb.AppendLine("        'uotools' => isset($_POST['menu_uotools']),");
            sb.AppendLine("        'social' => isset($_POST['menu_social']),");
            sb.AppendLine("        'client' => isset($_POST['menu_client']),");
            sb.AppendLine("        'contact' => isset($_POST['menu_contact'])");
            sb.AppendLine("      ];");
            sb.AppendLine("      $settings['social_visibility'] = [");
            sb.AppendLine("        'discord' => isset($_POST['social_discord']),");
            sb.AppendLine("        'youtube' => isset($_POST['social_youtube']),");
            sb.AppendLine("        'tiktok' => isset($_POST['social_tiktok']),");
            sb.AppendLine("        'twitter' => isset($_POST['social_twitter']),");
            sb.AppendLine("        'facebook' => isset($_POST['social_facebook'])");
            sb.AppendLine("      ];");
            sb.AppendLine("      $settings['social_urls'] = [");
            sb.AppendLine("        'discord' => !empty($_POST['social_url_discord']) ? htmlspecialchars(strip_tags($_POST['social_url_discord'])) : $settings['social_urls']['discord'],");
            sb.AppendLine("        'youtube' => !empty($_POST['social_url_youtube']) ? htmlspecialchars(strip_tags($_POST['social_url_youtube'])) : $settings['social_urls']['youtube'],");
            sb.AppendLine("        'tiktok' => !empty($_POST['social_url_tiktok']) ? htmlspecialchars(strip_tags($_POST['social_url_tiktok'])) : $settings['social_urls']['tiktok'],");
            sb.AppendLine("        'twitter' => !empty($_POST['social_url_twitter']) ? htmlspecialchars(strip_tags($_POST['social_url_twitter'])) : $settings['social_urls']['twitter'],");
            sb.AppendLine("        'facebook' => !empty($_POST['social_url_facebook']) ? htmlspecialchars(strip_tags($_POST['social_url_facebook'])) : $settings['social_urls']['facebook']");
            sb.AppendLine("      ];");
            sb.AppendLine("      $settings['menu_urls'] = [");
            sb.AppendLine("        'wiki' => !empty($_POST['menu_url_wiki']) ? htmlspecialchars(strip_tags($_POST['menu_url_wiki'])) : $settings['menu_urls']['wiki'],");
            sb.AppendLine("        'forum' => !empty($_POST['menu_url_forum']) ? htmlspecialchars(strip_tags($_POST['menu_url_forum'])) : $settings['menu_urls']['forum']");
            sb.AppendLine("      ];");
            sb.AppendLine("      file_put_contents($settingsFile, json_encode($settings, JSON_PRETTY_PRINT));");
            sb.AppendLine("      if (!empty($_SERVER['HTTP_X_REQUESTED_WITH']) && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) === 'xmlhttprequest') {");
            sb.AppendLine("        header('Content-Type: application/json');");
            sb.AppendLine("        echo json_encode(['status' => 'success']);");
            sb.AppendLine("        exit;");
            sb.AppendLine("      } else {");
            sb.AppendLine("        header('Location: admin.php'); exit;");
            sb.AppendLine("      }");
            sb.AppendLine("    } elseif (isset($_POST['add_update']) && !empty($_POST['update_title']) && !empty($_POST['update_content'])) {");
            sb.AppendLine("      $newUpdate = [");
            sb.AppendLine("        'id' => uniqid(),");
            sb.AppendLine("        'title' => htmlspecialchars(strip_tags($_POST['update_title'])),");
            sb.AppendLine("        'content' => htmlspecialchars(strip_tags($_POST['update_content'])),");
            sb.AppendLine("        'date' => date('Y-m-d H:i:s')");
            sb.AppendLine("      ];");
            sb.AppendLine("      $updates[] = $newUpdate;");
            sb.AppendLine("      file_put_contents($updatesFile, json_encode($updates, JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['edit_update']) && !empty($_POST['update_id']) && !empty($_POST['update_title']) && !empty($_POST['update_content'])) {");
            sb.AppendLine("      foreach ($updates as &$update) {");
            sb.AppendLine("        if ($update['id'] === $_POST['update_id']) {");
            sb.AppendLine("          $update['title'] = htmlspecialchars(strip_tags($_POST['update_title']));");
            sb.AppendLine("          $update['content'] = htmlspecialchars(strip_tags($_POST['update_content']));");
            sb.AppendLine("          $update['date'] = !empty($_POST['update_date']) ? htmlspecialchars(strip_tags($_POST['update_date'])) : $update['date'];");
            sb.AppendLine("          break;");
            sb.AppendLine("        }");
            sb.AppendLine("      }");
            sb.AppendLine("      file_put_contents($updatesFile, json_encode($updates, JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['delete_update']) && !empty($_POST['update_id'])) {");
            sb.AppendLine("      $updates = array_filter($updates, fn($update) => $update['id'] !== $_POST['update_id']);");
            sb.AppendLine("      file_put_contents($updatesFile, json_encode(array_values($updates), JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['add_tool']) && !empty($_POST['tool_name']) && !empty($_POST['tool_url']) && !empty($_POST['tool_description'])) {");
            sb.AppendLine("      $newTool = [");
            sb.AppendLine("        'id' => uniqid(),");
            sb.AppendLine("        'name' => htmlspecialchars(strip_tags($_POST['tool_name'])),");
            sb.AppendLine("        'url' => htmlspecialchars(strip_tags($_POST['tool_url'])),");
            sb.AppendLine("        'description' => htmlspecialchars(strip_tags($_POST['tool_description']))");
            sb.AppendLine("      ];");
            sb.AppendLine("      $tools[] = $newTool;");
            sb.AppendLine("      file_put_contents($toolsFile, json_encode($tools, JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['edit_tool']) && !empty($_POST['tool_id']) && !empty($_POST['tool_name']) && !empty($_POST['tool_url']) && !empty($_POST['tool_description'])) {");
            sb.AppendLine("      foreach ($tools as &$tool) {");
            sb.AppendLine("        if ($tool['id'] === $_POST['tool_id']) {");
            sb.AppendLine("          $tool['name'] = htmlspecialchars(strip_tags($_POST['tool_name']));");
            sb.AppendLine("          $tool['url'] = htmlspecialchars(strip_tags($_POST['tool_url']));");
            sb.AppendLine("          $tool['description'] = htmlspecialchars(strip_tags($_POST['tool_description']));");
            sb.AppendLine("          break;");
            sb.AppendLine("        }");
            sb.AppendLine("      }");
            sb.AppendLine("      file_put_contents($toolsFile, json_encode($tools, JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['delete_tool']) && !empty($_POST['tool_id'])) {");
            sb.AppendLine("      $tools = array_filter($tools, fn($tool) => $tool['id'] !== $_POST['tool_id']);");
            sb.AppendLine("      file_put_contents($toolsFile, json_encode(array_values($tools), JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['add_email']) && !empty($_POST['email_address'])) {");
            sb.AppendLine("      $settings['contact_emails'][] = htmlspecialchars(strip_tags($_POST['email_address']));");
            sb.AppendLine("      file_put_contents($settingsFile, json_encode($settings, JSON_PRETTY_PRINT));");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['edit_email']) && isset($_POST['email_index']) && !empty($_POST['email_address'])) {");
            sb.AppendLine("      $index = (int)$_POST['email_index'];");
            sb.AppendLine("      if (isset($settings['contact_emails'][$index])) {");
            sb.AppendLine("        $settings['contact_emails'][$index] = htmlspecialchars(strip_tags($_POST['email_address']));");
            sb.AppendLine("        file_put_contents($settingsFile, json_encode($settings, JSON_PRETTY_PRINT));");
            sb.AppendLine("      }");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    } elseif (isset($_POST['delete_email']) && isset($_POST['email_index'])) {");
            sb.AppendLine("      $index = (int)$_POST['email_index'];");
            sb.AppendLine("      if (isset($settings['contact_emails'][$index])) {");
            sb.AppendLine("        array_splice($settings['contact_emails'], $index, 1);");
            sb.AppendLine("        file_put_contents($settingsFile, json_encode($settings, JSON_PRETTY_PRINT));");
            sb.AppendLine("      }");
            sb.AppendLine("      header('Location: admin.php'); exit;");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine("}");

            sb.AppendLine("echo '<!-- Debug: show_background_image = ' . ($settings['show_background_image'] ? 'true' : 'false') . ', show_powered_by = ' . ($settings['show_powered_by'] ? 'true' : 'false') . ', footer_copyright_domain = ' . htmlspecialchars($settings['footer_copyright_domain']) . ', client_host = ' . htmlspecialchars($settings['client_host']) . ', client_port = ' . htmlspecialchars($settings['client_port']) . ', background_color = ' . htmlspecialchars($settings['background_color']) . ' -->';");
            sb.AppendLine("echo '<!-- Debug: Applied background style = ' . ($settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center ' . htmlspecialchars($settings['background_color']) : htmlspecialchars($settings['background_color'])) . ' -->';");
            sb.AppendLine("ob_end_flush();");
            sb.AppendLine("?>");

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><title>Admin Dashboard</title>");
            sb.AppendLine("<link rel='stylesheet' href='styles.css'>");
            sb.AppendLine("<style>");
            sb.AppendLine(":root {");
            sb.AppendLine("    --text-color: <?php echo htmlspecialchars($settings['text_color']); ?>;");
            sb.AppendLine("    --primary-color: #2196F3;");
            sb.AppendLine("    --secondary-color: #1976D2;");
            sb.AppendLine("    --border-color: #333;");
            sb.AppendLine("    --input-bg: #333;");
            sb.AppendLine("    --input-border: #555;");
            sb.AppendLine("}");
            sb.AppendLine("html {");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    background: <?php echo $settings['show_background_image'] ? 'url(\"images/background.png\") no-repeat center center' : htmlspecialchars($settings['background_color']); ?> !important;");
            sb.AppendLine("    background-size: cover !important;");
            sb.AppendLine("    background-attachment: fixed !important;");
            sb.AppendLine("    background-color: <?php echo htmlspecialchars($settings['background_color']); ?> !important;");
            sb.AppendLine("}");
            sb.AppendLine("body {");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    padding: 20px;");
            sb.AppendLine("    min-height: 100%;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    background: transparent !important;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    font-family: Arial, sans-serif;");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("}");
            sb.AppendLine(".admin-container { min-height: 100vh; display: flex; flex-direction: column; width: 100%; max-width: 1200px; margin: 0 auto; }");
            sb.AppendLine("h1 { margin: 30px 0; font-size: 2.5em; text-align: center; color: var(--text-color); padding: 10px; background: rgba(0, 0, 0, 0.7); border-bottom: 2px solid var(--primary-color); border-radius: 5px; }");
            sb.AppendLine("h2 { font-size: 2em; text-align: center; margin: 40px 0 20px; color: var(--text-color); background: rgba(0, 0, 0, 0.7); padding: 10px; border-radius: 5px; }");
            sb.AppendLine("h3 { margin: 0 0 15px; font-size: 1.5em; color: var(--text-color); border-bottom: 1px solid var(--border-color); padding-bottom: 5px; }");
            sb.AppendLine("h4 { margin: 0 0 10px; font-size: 1.2em; color: var(--text-color); }");
            sb.AppendLine(".section { background: rgba(0, 0, 0, 0.7); padding: 20px; margin-bottom: 20px; border: 1px solid var(--border-color); border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); }");
            sb.AppendLine(".section-group { margin-bottom: 40px; }");
            sb.AppendLine(".toggle-container { margin-bottom: 15px; display: flex; align-items: center; }");
            sb.AppendLine(".toggle-switch { position: relative; display: inline-block; width: 50px; height: 24px; margin-right: 10px; }");
            sb.AppendLine(".toggle-switch input { opacity: 0; width: 0; height: 0; }");
            sb.AppendLine(".slider { position: absolute; top: 0; left: 0; right: 0; bottom: 0; background-color: #ccc; transition: 0.4s; border-radius: 24px; }");
            sb.AppendLine(".slider:before { content: ''; position: absolute; height: 20px; width: 20px; left: 2px; bottom: 2px; background-color: white; transition: 0.4s; border-radius: 50%; }");
            sb.AppendLine("input:checked + .slider { background-color: var(--primary-color); }");
            sb.AppendLine("input:checked + .slider:before { transform: translateX(26px); }");
            sb.AppendLine(".toggle-label { margin-left: 10px; font-size: 1em; color: var(--text-color); }");
            sb.AppendLine(".text-input-container { margin-bottom: 15px; }");
            sb.AppendLine(".text-input-container label { display: block; margin-bottom: 5px; font-weight: bold; color: var(--text-color); }");
            sb.AppendLine(".text-input-container input[type='text'], .text-input-container input[type='password'], .text-input-container input[type='email'], .text-input-container input[type='file'] { width: 100%; padding: 8px; background: var(--input-bg); color: var(--text-color); border: 1px solid var(--input-border); border-radius: 3px; box-sizing: border-box; transition: border-color 0.3s; }");
            sb.AppendLine(".text-input-container input[type='text']:focus, .text-input-container input[type='password']:focus, .text-input-container input[type='email']:focus, .text-input-container input[type='file']:focus { border-color: var(--primary-color); outline: none; }");
            sb.AppendLine(".text-input-container textarea { width: 100%; padding: 8px; background: var(--input-bg); color: var(--text-color); border: 1px solid var(--input-border); border-radius: 3px; box-sizing: border-box; height: 100px; resize: vertical; transition: border-color 0.3s; }");
            sb.AppendLine(".text-input-container textarea:focus { border-color: var(--primary-color); outline: none; }");
            // Reinforced wide-text-input rule with higher specificity
            sb.AppendLine(".text-input-container input.wide-text-input[type='text'] { min-width: 400px !important; }");
            sb.AppendLine(".text-input-container input[type='file'] { min-width: 300px; }");
            sb.AppendLine(".background-color-picker { margin-top: 10px; display: none; }");
            sb.AppendLine(".update-form, .tool-form, .email-form { margin-bottom: 20px; }");
            sb.AppendLine(".update-form input[type='text'], .update-form textarea, .tool-form input[type='text'], .tool-form textarea, .email-form input[type='text'], .email-form input[type='email'] { width: 100%; padding: 8px; margin-bottom: 10px; background: var(--input-bg); color: var(--text-color); border: 1px solid var(--input-border); border-radius: 3px; box-sizing: border-box; transition: border-color 0.3s; }");
            sb.AppendLine(".update-form input[type='text']:focus, .update-form textarea:focus, .tool-form input[type='text']:focus, .tool-form textarea:focus, .email-form input[type='text']:focus, .email-form input[type='email']:focus { border-color: var(--primary-color); outline: none; }");
            sb.AppendLine(".update-form textarea, .tool-form textarea { height: 100px; resize: vertical; }");
            sb.AppendLine(".update-form button, .tool-form button, .email-form button { padding: 8px 16px; background: var(--primary-color); color: #fff; border: none; border-radius: 3px; cursor: pointer; transition: background 0.3s; }");
            sb.AppendLine(".update-form button:hover, .tool-form button:hover, .email-form button:hover { background: var(--secondary-color); }");
            sb.AppendLine(".update-list, .tool-list, .email-list { padding-right: 10px; }");
            sb.AppendLine(".update-item, .tool-item, .email-item { background: rgba(0, 0, 0, 0.7); border: 1px solid var(--border-color); padding: 15px; margin-bottom: 15px; border-radius: 5px; }");
            sb.AppendLine(".update-item h4, .tool-item h4, .email-item h4 { margin: 0 0 5px; font-size: 1.2em; color: var(--text-color); }");
            sb.AppendLine(".update-item p, .tool-item p, .email-item p { margin: 0 0 10px; color: var(--text-color); }");
            sb.AppendLine(".update-item form, .tool-item form, .email-item form { display: inline; }");
            sb.AppendLine(".update-item button, .tool-item button, .email-item button { padding: 5px 10px; background: #d32f2f; color: #fff; border: none; border-radius: 3px; cursor: pointer; margin-right: 5px; transition: background 0.3s; }");
            sb.AppendLine(".update-item button.edit, .tool-item button.edit, .email-item button.edit { background: #4CAF50; }");
            sb.AppendLine(".update-item button.edit:hover, .tool-item button.edit:hover, .email-item button.edit:hover { background: #388E3C; }");
            sb.AppendLine(".update-item button:hover, .tool-item button:hover, .email-item button:hover { background: #b71c1c; }");
            sb.AppendLine(".edit-form { display: none; margin-top: 10px; }");
            sb.AppendLine(".edit-form.active { display: block; }");
            sb.AppendLine("button { padding: 8px 16px; background: var(--primary-color); color: #fff; border: none; border-radius: 3px; cursor: pointer; transition: background 0.3s; }");
            sb.AppendLine("button:hover { background: var(--secondary-color); }");
            sb.AppendLine("input[type='color'] { width: 50px !important; height: 30px !important; padding: 0 !important; border: 1px solid var(--input-border); border-radius: 3px; cursor: pointer; vertical-align: middle; background: var(--input-bg); transition: border-color 0.3s; }");
            sb.AppendLine("input[type='color']:focus { border-color: var(--primary-color); outline: none; }");
            sb.AppendLine(".toggle-url-container { margin-bottom: 15px; display: flex; align-items: center; justify-content: space-between; }");
            sb.AppendLine(".toggle-url-container .toggle-switch { margin-right: 10px; }");
            sb.AppendLine(".toggle-url-container .toggle-label { flex: 0 0 100px; }");
            sb.AppendLine(".toggle-url-container .url-input { flex: 1; margin-left: 10px; }");
            sb.AppendLine(".toggle-url-container .url-input input[type='text'] { width: 100%; padding: 8px; background: var(--input-bg); color: var(--text-color); border: 1px solid var(--input-border); border-radius: 3px; box-sizing: border-box; transition: border-color 0.3s; }");
            sb.AppendLine(".toggle-url-container .url-input input[type='text']:focus { border-color: var(--primary-color); outline: none; }");
            sb.AppendLine(".settings-grid { display: flex; flex-wrap: wrap; gap: 20px; margin-bottom: 40px; }");
            sb.AppendLine(".settings-grid .section { flex: 1 1 300px; min-width: 300px; max-width: 100%; }");
            sb.AppendLine("@media (max-width: 768px) {");
            sb.AppendLine("  body { padding: 10px; }");
            sb.AppendLine("  .admin-container { max-width: 100%; }");
            sb.AppendLine("  .section { padding: 15px; }");
            sb.AppendLine("  .toggle-container, .text-input-container { margin-bottom: 10px; }");
            sb.AppendLine("  .update-form input[type='text'], .update-form textarea, .tool-form input[type='text'], .tool-form textarea, .email-form input[type='text'], .email-form input[type='email'] { padding: 6px; }");
            sb.AppendLine("  .update-item, .tool-item, .email-item { padding: 10px; }");
            sb.AppendLine("  .toggle-url-container { flex-direction: column; align-items: flex-start; }");
            sb.AppendLine("  .toggle-url-container .toggle-label { margin-bottom: 5px; }");
            sb.AppendLine("  .toggle-url-container .url-input { margin-left: 0; width: 100%; }");
            sb.AppendLine("  .settings-grid { flex-direction: column; }");
            sb.AppendLine("  .settings-grid .section { flex: 1 1 100%; }");
            sb.AppendLine("  h1 { font-size: 28px; }");
            sb.AppendLine("  h2 { font-size: 1.8em; }");
            sb.AppendLine("}");
            sb.AppendLine("</style>");
            sb.AppendLine("<script>");
            sb.AppendLine("function autoApplySettings() {");
            sb.AppendLine("  const form = document.getElementById('settings-form');");
            sb.AppendLine("  const formData = new FormData(form);");
            sb.AppendLine("  fetch('admin.php', {");
            sb.AppendLine("    method: 'POST',");
            sb.AppendLine("    body: formData");
            sb.AppendLine("  }).then(response => response.json())");
            sb.AppendLine("    .then(data => {");
            sb.AppendLine("      if (data.status === 'success') {");
            sb.AppendLine("        console.log('Settings saved successfully');");
            sb.AppendLine("        location.reload();");
            sb.AppendLine("      }");
            sb.AppendLine("    }).catch(error => console.error('Error saving settings:', error));");
            sb.AppendLine("}");
            sb.AppendLine("document.addEventListener('DOMContentLoaded', function() {");
            sb.AppendLine("  const showBackgroundImageToggle = document.getElementById('show_background_image');");
            sb.AppendLine("  const backgroundColorPicker = document.getElementById('background-color-picker');");
            sb.AppendLine("  if (showBackgroundImageToggle && backgroundColorPicker) {");
            sb.AppendLine("    const toggleBackgroundColorPicker = () => {");
            sb.AppendLine("      backgroundColorPicker.style.display = showBackgroundImageToggle.checked ? 'none' : 'block';");
            sb.AppendLine("    };");
            sb.AppendLine("    toggleBackgroundColorPicker();");
            sb.AppendLine("    showBackgroundImageToggle.addEventListener('change', toggleBackgroundColorPicker);");
            sb.AppendLine("    showBackgroundImageToggle.addEventListener('change', autoApplySettings);");
            sb.AppendLine("  }");
            sb.AppendLine("  const textColorPicker = document.getElementById('text_color');");
            sb.AppendLine("  if (textColorPicker) {");
            sb.AppendLine("    textColorPicker.addEventListener('input', function() {");
            sb.AppendLine("      document.documentElement.style.setProperty('--text-color', this.value);");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine("  document.querySelectorAll('input[type=checkbox], input[type=text], input[type=color], textarea, input[type=email], input[type=file]').forEach(element => {");
            sb.AppendLine("    if (element.id !== 'show_background_image') {");
            sb.AppendLine("      element.addEventListener('change', autoApplySettings);");
            sb.AppendLine("    }");
            sb.AppendLine("  });");
            sb.AppendLine("  document.querySelectorAll('.edit-button').forEach(button => {");
            sb.AppendLine("    button.addEventListener('click', function() {");
            sb.AppendLine("      const updateId = this.getAttribute('data-id');");
            sb.AppendLine("      const editForm = document.getElementById('edit-form-' + updateId);");
            sb.AppendLine("      editForm.classList.toggle('active');");
            sb.AppendLine("      const isActive = editForm.classList.contains('active');");
            sb.AppendLine("      const inputs = editForm.querySelectorAll('input, textarea, button');");
            sb.AppendLine("      inputs.forEach(input => {");
            sb.AppendLine("        input.disabled = !isActive;");
            sb.AppendLine("      });");
            sb.AppendLine("      button.textContent = isActive ? 'Cancel' : 'Edit';");
            sb.AppendLine("    });");
            sb.AppendLine("  });");
            sb.AppendLine("  document.querySelectorAll('.edit-tool-button').forEach(button => {");
            sb.AppendLine("    button.addEventListener('click', function() {");
            sb.AppendLine("      const toolId = this.getAttribute('data-id');");
            sb.AppendLine("      const editForm = document.getElementById('edit-tool-form-' + toolId);");
            sb.AppendLine("      editForm.classList.toggle('active');");
            sb.AppendLine("      const isActive = editForm.classList.contains('active');");
            sb.AppendLine("      const inputs = editForm.querySelectorAll('input, textarea, button');");
            sb.AppendLine("      inputs.forEach(input => {");
            sb.AppendLine("        input.disabled = !isActive;");
            sb.AppendLine("      });");
            sb.AppendLine("      button.textContent = isActive ? 'Cancel' : 'Edit';");
            sb.AppendLine("    });");
            sb.AppendLine("  });");
            sb.AppendLine("  document.querySelectorAll('.edit-email-button').forEach(button => {");
            sb.AppendLine("    button.addEventListener('click', function() {");
            sb.AppendLine("      const emailIndex = this.getAttribute('data-index');");
            sb.AppendLine("      const editForm = document.getElementById('edit-email-form-' + emailIndex);");
            sb.AppendLine("      editForm.classList.toggle('active');");
            sb.AppendLine("      const isActive = editForm.classList.contains('active');");
            sb.AppendLine("      const inputs = editForm.querySelectorAll('input, button');");
            sb.AppendLine("      inputs.forEach(input => {");
            sb.AppendLine("        input.disabled = !isActive;");
            sb.AppendLine("      });");
            sb.AppendLine("      button.textContent = isActive ? 'Cancel' : 'Edit';");
            sb.AppendLine("    });");
            sb.AppendLine("  });");
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            sb.AppendLine("</head><body class=\"theme-<?php echo htmlspecialchars($settings['theme']); ?>\">");

            sb.AppendLine("<div class='admin-container'>");
            sb.AppendLine("<?php if (!isset($_SESSION['admin_logged_in']) || !$_SESSION['admin_logged_in']) { ?>");
            sb.AppendLine("  <div class='section'>");
            sb.AppendLine("    <h3>Admin Login</h3>");
            sb.AppendLine("    <form method='post'>");
            sb.AppendLine("      <?php if (isset($error)) echo \"<p style='color:red'>\" . htmlspecialchars($error) . \"</p>\"; ?>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='password'>Password:</label>");
            sb.AppendLine("        <input type='password' id='password' name='password' required>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <button name='login'>Login</button>");
            sb.AppendLine("    </form>");
            sb.AppendLine("  </div>");
            sb.AppendLine("<?php } else { ?>");

            // Logout Section
            sb.AppendLine("  <div class='section'>");
            sb.AppendLine("    <form method='post'>");
            sb.AppendLine("      <button name='logout'>Logout</button>");
            sb.AppendLine("    </form>");
            sb.AppendLine("  </div>");

            // Main Admin Dashboard Title with Logo
            sb.AppendLine("  <h1>Admin Dashboard</h1>");
            sb.AppendLine("  <div style='text-align: center; margin-bottom: 20px;'>");
            sb.AppendLine("    <img src='images/logo.png' alt='Site Logo' style='max-width: 200px; height: auto;'>");
            sb.AppendLine("  </div>");
            sb.AppendLine("  <?php if (isset($error)) echo \"<p style='color:red; text-align:center;'>\" . htmlspecialchars($error) . \"</p>\"; ?>");

            // Site-Wide Settings Form (single form for all settings)
            sb.AppendLine("  <form id='settings-form' method='post' enctype='multipart/form-data'>");

            // Appearance Settings Group
            sb.AppendLine("  <div class='section-group'>");
            sb.AppendLine("    <h2>Site Appearance</h2>");
            sb.AppendLine("    <div class='settings-grid'>");

            // Image Settings
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Image Settings</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='logo_image'>Upload Site Logo (PNG):</label>");
            sb.AppendLine("          <input type='file' id='logo_image' name='logo_image' accept='image/png'>");
            sb.AppendLine("          <p style='font-size: 0.9em; color: var(--text-color);'>Recommended dimensions: 200x50 pixels</p>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='background_image'>Upload Background Image (PNG):</label>");
            sb.AppendLine("          <input type='file' id='background_image' name='background_image' accept='image/png'>");
            sb.AppendLine("          <p style='font-size: 0.9em; color: var(--text-color);'>Recommended dimensions: 1920x1080 pixels</p>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='toggle-container'>");
            sb.AppendLine("          <label><span class='toggle-switch'><input type='checkbox' id='show_background_image' name='show_background_image' <?php echo $settings['show_background_image'] ? 'checked' : ''; ?>><span class='slider'></span></span>");
            sb.AppendLine("          <span class='toggle-label'>Show Background Image</span></label>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div id='background-color-picker' class='background-color-picker'>");
            sb.AppendLine("          <label for='background_color'>Background Color (when image hidden):</label>");
            sb.AppendLine("          <input type='color' id='background_color' name='background_color' value='<?php echo htmlspecialchars($settings['background_color'] ?? '#000000'); ?>'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            // Text Color
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Text Color</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='text_color'>Choose Text Hue:</label>");
            sb.AppendLine("          <input type='color' id='text_color' name='text_color' value='<?php echo htmlspecialchars($settings['text_color'] ?? '#ffffff'); ?>'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Homepage Settings Group
            sb.AppendLine("  <div class='section-group'>");
            sb.AppendLine("    <h2>Homepage Configuration</h2>");
            sb.AppendLine("    <div class='settings-grid'>");

            // Homepage Text
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Homepage Content</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='homepage_text'>Homepage Text:</label>");
            sb.AppendLine("          <textarea id='homepage_text' name='homepage_text'><?php echo htmlspecialchars($settings['homepage_text']); ?></textarea>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            // Download Button Settings
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Download Button</h3>");
            sb.AppendLine("        <div class='toggle-container'>");
            sb.AppendLine("          <label><span class='toggle-switch'><input type='checkbox' id='show_download_button' name='show_download_button' <?php echo $settings['show_download_button'] ? 'checked' : ''; ?>><span class='slider'></span></span>");
            sb.AppendLine("          <span class='toggle-label'>Show Download Button</span></label>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='download_button_text'>Download Button Text:</label>");
            sb.AppendLine("          <input type='text' id='download_button_text' name='download_button_text' value='<?php echo htmlspecialchars($settings['download_button_text']); ?>' placeholder='e.g., DOWNLOAD CLIENT'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='download_button_color'>Download Button Color:</label>");
            sb.AppendLine("          <input type='color' id='download_button_color' name='download_button_color' value='<?php echo htmlspecialchars($settings['download_button_color']); ?>'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Client Settings Group
            sb.AppendLine("  <div class='section-group'>");
            sb.AppendLine("    <h2>Client Configuration</h2>");
            sb.AppendLine("    <div class='settings-grid'>");

            // Client Page Text
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Client Page Text</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='client_text_above_button'>Text Above Download Button:</label>");
            sb.AppendLine("          <input type='text' id='client_text_above_button' name='client_text_above_button' value='<?php echo htmlspecialchars($settings['client_text_above_button']); ?>' placeholder='e.g., Get our custom Ultima Online client to join the shard.'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='client_text_below_button'>Text Below Download Button:</label>");
            sb.AppendLine("          <input type='text' id='client_text_below_button' name='client_text_below_button' value='<?php echo htmlspecialchars($settings['client_text_below_button']); ?>' placeholder='e.g., Ensure you have the latest version for the best experience.'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            // Client Connection Settings
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Client Connection</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='client_host'>Client Host:</label>");
            sb.AppendLine("          <input type='text' id='client_host' name='client_host' value='<?php echo htmlspecialchars($settings['client_host'] ?? 'localhost'); ?>' placeholder='e.g., server.yourdomain.com'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='client_port'>Client Port:</label>");
            sb.AppendLine("          <input type='text' id='client_port' name='client_port' value='<?php echo htmlspecialchars($settings['client_port'] ?? '2593'); ?>' placeholder='e.g., 2593'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            // Client Download Settings
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Client Download</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='client_download_url'>Client Download URL:</label>");
            sb.AppendLine("          <input type='text' id='client_download_url' name='client_download_url' value='<?php echo htmlspecialchars($settings['client_download_url']); ?>' placeholder='e.g., https://example.com/download/client.zip'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='client_download_button_color'>Client Download Button Color:</label>");
            sb.AppendLine("          <input type='color' id='client_download_button_color' name='client_download_button_color' value='<?php echo htmlspecialchars($settings['client_download_button_color']); ?>'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Navigation and Social Links Group
            sb.AppendLine("  <div class='section-group'>");
            sb.AppendLine("    <h2>Navigation & Social Links</h2>");
            sb.AppendLine("    <div class='settings-grid'>");

            // Menu Visibility
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Menu Visibility</h3>");
            sb.AppendLine("        <?php");
            sb.AppendLine("        $menuLabels = [");
            sb.AppendLine("          'index' => 'Home',");
            sb.AppendLine("          'players' => 'Players',");
            sb.AppendLine("          'guilds' => 'Guilds',");
            sb.AppendLine("          'forum' => 'Forum',");
            sb.AppendLine("          'wiki' => 'Wiki',");
            sb.AppendLine("          'uotools' => 'UO Tools',");
            sb.AppendLine("          'social' => 'Shard Updates',");
            sb.AppendLine("          'client' => 'Client',");
            sb.AppendLine("          'contact' => 'Contact'");
            sb.AppendLine("        ];");
            sb.AppendLine("        foreach ($menuLabels as $m => $label) {");
            sb.AppendLine("          $c = !empty($settings['menu_visibility'][$m]) ? 'checked' : '';");
            sb.AppendLine("          if ($m === 'wiki' || $m === 'forum') {");
            sb.AppendLine("            echo \"<div class='toggle-url-container'><label><span class='toggle-switch'><input type='checkbox' name='menu_$m' $c><span class='slider'></span></span><span class='toggle-label'>\" . htmlspecialchars($label) . \"</span></label>\";");
            sb.AppendLine("            echo \"<div class='url-input'><input type='text' name='menu_url_$m' value='\" . htmlspecialchars($settings['menu_urls'][$m]) . \"' placeholder='Enter URL for $label'></div></div>\";");
            sb.AppendLine("          } else {");
            sb.AppendLine("            echo \"<div class='toggle-container'><label><span class='toggle-switch'><input type='checkbox' name='menu_$m' $c><span class='slider'></span></span><span class='toggle-label'>\" . htmlspecialchars($label) . \"</span></label></div>\";");
            sb.AppendLine("          }");
            sb.AppendLine("        }");
            sb.AppendLine("        ?>");
            sb.AppendLine("      </div>");

            // Social Links Visibility
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Social Links</h3>");
            sb.AppendLine("        <?php");
            sb.AppendLine("        $socials = ['discord', 'youtube', 'tiktok', 'twitter', 'facebook'];");
            sb.AppendLine("        foreach ($socials as $s) {");
            sb.AppendLine("          $c = !empty($settings['social_visibility'][$s]) ? 'checked' : '';");
            sb.AppendLine("          echo \"<div class='toggle-url-container'><label><span class='toggle-switch'><input type='checkbox' name='social_$s' $c><span class='slider'></span></span><span class='toggle-label'>\" . htmlspecialchars(ucfirst($s)) . \"</span></label>\";");
            sb.AppendLine("          echo \"<div class='url-input'><input type='text' name='social_url_$s' value='\" . htmlspecialchars($settings['social_urls'][$s]) . \"' placeholder='Enter URL for \" . htmlspecialchars(ucfirst($s)) . \"'></div></div>\";");
            sb.AppendLine("        }");
            sb.AppendLine("        ?>");
            sb.AppendLine("      </div>");

            // Header Settings
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Header Settings</h3>");
            sb.AppendLine("        <div class='toggle-container'>");
            sb.AppendLine("          <label><span class='toggle-switch'><input type='checkbox' id='show_powered_by' name='show_powered_by' <?php echo !empty($settings['show_powered_by']) ? 'checked' : ''; ?>><span class='slider'></span></span>");
            sb.AppendLine("          <span class='toggle-label'>Show Powered by ModernUO</span></label>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            // Footer Settings
            sb.AppendLine("      <div class='section'>");
            sb.AppendLine("        <h3>Footer Settings</h3>");
            sb.AppendLine("        <div class='text-input-container'>");
            sb.AppendLine("          <label for='footer_copyright_domain'>Copyright Domain:</label>");
            sb.AppendLine("          <input type='text' id='footer_copyright_domain' name='footer_copyright_domain' value='<?php echo htmlspecialchars($settings['footer_copyright_domain'] ?? 'mydomain.com'); ?>' placeholder='e.g., yourdomain.com'>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Hidden input to trigger settings application
            sb.AppendLine("    <input type='hidden' name='apply_settings' value='1'>");
            sb.AppendLine("  </form>");

            // Contact Page Emails Section
            sb.AppendLine("  <h2>Manage Contact Page Emails</h2>");
            sb.AppendLine("  <div class='section'>");
            sb.AppendLine("    <h3>Add New Email</h3>");
            sb.AppendLine("    <form method='post' class='email-form'>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='email_address'>Email Address:</label>");
            sb.AppendLine("        <input type='email' id='email_address' name='email_address' placeholder='e.g., support@yourserver.com' required>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <button type='submit' name='add_email'>Add Email</button>");
            sb.AppendLine("    </form>");

            sb.AppendLine("    <div class='email-list'>");
            sb.AppendLine("      <?php");
            sb.AppendLine("      if (!empty($settings['contact_emails'])) {");
            sb.AppendLine("        foreach ($settings['contact_emails'] as $index => $email) {");
            sb.AppendLine("          echo \"<div class='email-item'>\";");
            sb.AppendLine("          echo \"<h4>Email #$index</h4>\";");
            sb.AppendLine("          echo \"<p>\" . htmlspecialchars($email) . \"</p>\";");
            sb.AppendLine("          echo \"<form method='post' style='display:inline;'><input type='hidden' name='email_index' value='\" . $index . \"'><button type='submit' name='delete_email'>Delete</button></form>\";");
            sb.AppendLine("          echo \"<button class='edit-email-button' data-index='\" . $index . \"'>Edit</button>\";");
            sb.AppendLine("          echo \"<form id='edit-email-form-\" . $index . \"' method='post' class='email-form edit-form'>\";");
            sb.AppendLine("          echo \"<input type='hidden' name='email_index' value='\" . $index . \"'>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_email_address_\" . $index . \"'>Email Address:</label>\";");
            sb.AppendLine("          echo \"<input type='email' id='edit_email_address_\" . $index . \"' name='email_address' value='\" . htmlspecialchars($email) . \"' placeholder='e.g., support@yourserver.com' required disabled></div>\";");
            sb.AppendLine("          echo \"<button type='submit' name='edit_email' disabled>Save Changes</button>\";");
            sb.AppendLine("          echo \"</form>\";");
            sb.AppendLine("          echo \"</div>\";");
            sb.AppendLine("        }");
            sb.AppendLine("      } else {");
            sb.AppendLine("        echo \"<p>No email addresses available.</p>\";");
            sb.AppendLine("      }");
            sb.AppendLine("      ?>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // UO Tools Section
            sb.AppendLine("  <h2>Manage UO Tools</h2>");
            sb.AppendLine("  <div class='section'>");
            sb.AppendLine("    <h3>Add New Tool</h3>");
            sb.AppendLine("    <form method='post' class='tool-form'>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='tool_name'>Tool Name:</label>");
            sb.AppendLine("        <input type='text' id='tool_name' name='tool_name' placeholder='Tool Name' required>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='tool_url'>Tool URL:</label>");
            sb.AppendLine("        <input type='text' id='tool_url' name='tool_url' placeholder='https://example.com' required>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='tool_description'>Description:</label>");
            sb.AppendLine("        <textarea id='tool_description' name='tool_description' placeholder='Tool Description' required></textarea>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <button type='submit' name='add_tool'>Add Tool</button>");
            sb.AppendLine("    </form>");

            sb.AppendLine("    <div class='tool-list'>");
            sb.AppendLine("      <?php");
            sb.AppendLine("      if (!empty($tools)) {");
            sb.AppendLine("        foreach ($tools as $tool) {");
            sb.AppendLine("          echo \"<div class='tool-item'>\";");
            sb.AppendLine("          echo \"<h4>\" . htmlspecialchars($tool['name']) . \"</h4>\";");
            sb.AppendLine("          echo \"<p><strong>URL:</strong> \" . htmlspecialchars($tool['url']) . \"</p>\";");
            sb.AppendLine("          echo \"<p><strong>Description:</strong> \" . htmlspecialchars($tool['description']) . \"</p>\";");
            sb.AppendLine("          echo \"<form method='post' style='display:inline;'><input type='hidden' name='tool_id' value='\" . htmlspecialchars($tool['id']) . \"'><button type='submit' name='delete_tool'>Delete</button></form>\";");
            sb.AppendLine("          echo \"<button class='edit-tool-button' data-id='\" . htmlspecialchars($tool['id']) . \"'>Edit</button>\";");
            sb.AppendLine("          echo \"<form id='edit-tool-form-\" . htmlspecialchars($tool['id']) . \"' method='post' class='tool-form edit-form'>\";");
            sb.AppendLine("          echo \"<input type='hidden' name='tool_id' value='\" . htmlspecialchars($tool['id']) . \"'>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_tool_name_\" . htmlspecialchars($tool['id']) . \"'>Tool Name:</label>\";");
            sb.AppendLine("          echo \"<input type='text' id='edit_tool_name_\" . htmlspecialchars($tool['id']) . \"' name='tool_name' value='\" . htmlspecialchars($tool['name']) . \"' placeholder='Tool Name' required disabled></div>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_tool_url_\" . htmlspecialchars($tool['id']) . \"'>Tool URL:</label>\";");
            sb.AppendLine("          echo \"<input type='text' id='edit_tool_url_\" . htmlspecialchars($tool['id']) . \"' name='tool_url' value='\" . htmlspecialchars($tool['url']) . \"' placeholder='https://example.com' required disabled></div>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_tool_description_\" . htmlspecialchars($tool['id']) . \"'>Description:</label>\";");
            sb.AppendLine("          echo \"<textarea id='edit_tool_description_\" . htmlspecialchars($tool['id']) . \"' name='tool_description' placeholder='Tool Description' required disabled>\" . htmlspecialchars($tool['description']) . \"</textarea></div>\";");
            sb.AppendLine("          echo \"<button type='submit' name='edit_tool' disabled>Save Changes</button>\";");
            sb.AppendLine("          echo \"</form>\";");
            sb.AppendLine("          echo \"</div>\";");
            sb.AppendLine("        }");
            sb.AppendLine("      } else {");
            sb.AppendLine("        echo \"<p>No tools available.</p>\";");
            sb.AppendLine("      }");
            sb.AppendLine("      ?>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Shard Updates Section (Moved to Last)
            sb.AppendLine("  <h2>Manage Shard Updates</h2>");
            sb.AppendLine("  <div class='section'>");
            sb.AppendLine("    <h3>Add New Update</h3>");
            sb.AppendLine("    <form method='post' class='update-form'>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='update_title'>Update Title:</label>");
            sb.AppendLine("        <input type='text' id='update_title' name='update_title' placeholder='Update Title' required>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class='text-input-container'>");
            sb.AppendLine("        <label for='update_content'>Update Content:</label>");
            sb.AppendLine("        <textarea id='update_content' name='update_content' placeholder='Update Content' required></textarea>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <button type='submit' name='add_update'>Add Update</button>");
            sb.AppendLine("    </form>");

            sb.AppendLine("    <div class='update-list'>");
            sb.AppendLine("      <?php");
            sb.AppendLine("      if (!empty($updates)) {");
            sb.AppendLine("        usort($updates, fn($a, $b) => strtotime($b['date']) - strtotime($a['date']));");
            sb.AppendLine("        foreach ($updates as $update) {");
            sb.AppendLine("          echo \"<div class='update-item'>\";");
            sb.AppendLine("          echo \"<h4>\" . htmlspecialchars($update['title']) . \"</h4>\";");
            sb.AppendLine("          echo \"<p><strong>Date:</strong> \" . htmlspecialchars($update['date']) . \"</p>\";");
            sb.AppendLine("          echo \"<p>\" . nl2br(htmlspecialchars($update['content'])) . \"</p>\";");
            sb.AppendLine("          echo \"<form method='post' style='display:inline;'><input type='hidden' name='update_id' value='\" . htmlspecialchars($update['id']) . \"'><button type='submit' name='delete_update'>Delete</button></form>\";");
            sb.AppendLine("          echo \"<button class='edit-button' data-id='\" . htmlspecialchars($update['id']) . \"'>Edit</button>\";");
            sb.AppendLine("          echo \"<form id='edit-form-\" . htmlspecialchars($update['id']) . \"' method='post' class='update-form edit-form'>\";");
            sb.AppendLine("          echo \"<input type='hidden' name='update_id' value='\" . htmlspecialchars($update['id']) . \"'>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_update_title_\" . htmlspecialchars($update['id']) . \"'>Update Title:</label>\";");
            sb.AppendLine("          echo \"<input type='text' id='edit_update_title_\" . htmlspecialchars($update['id']) . \"' name='update_title' value='\" . htmlspecialchars($update['title']) . \"' placeholder='Update Title' required disabled></div>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_update_content_\" . htmlspecialchars($update['id']) . \"'>Update Content:</label>\";");
            sb.AppendLine("          echo \"<textarea id='edit_update_content_\" . htmlspecialchars($update['id']) . \"' name='update_content' placeholder='Update Content' required disabled>\" . htmlspecialchars($update['content']) . \"</textarea></div>\";");
            sb.AppendLine("          echo \"<div class='text-input-container'><label for='edit_update_date_\" . htmlspecialchars($update['id']) . \"'>Date (YYYY-MM-DD HH:MM:SS):</label>\";");
            sb.AppendLine("          echo \"<input type='text' id='edit_update_date_\" . htmlspecialchars($update['id']) . \"' name='update_date' value='\" . htmlspecialchars($update['date']) . \"' placeholder='Date (YYYY-MM-DD HH:MM:SS)' disabled></div>\";");
            sb.AppendLine("          echo \"<button type='submit' name='edit_update' disabled>Save Changes</button>\";");
            sb.AppendLine("          echo \"</form>\";");
            sb.AppendLine("          echo \"</div>\";");
            sb.AppendLine("        }");
            sb.AppendLine("      } else {");
            sb.AppendLine("        echo \"<p>No updates available.</p>\";");
            sb.AppendLine("      }");
            sb.AppendLine("      ?>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Footer
            sb.AppendLine("  <div class=\"footer\">");
            sb.AppendLine("    <div class=\"footer-container\">");
            sb.AppendLine("      <div class=\"social-icons\">");
            sb.AppendLine("        <?php");
            sb.AppendLine("        if ($settings['social_visibility']['facebook']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['facebook']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/facebook.png\" alt=\"Facebook\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("        if ($settings['social_visibility']['youtube']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['youtube']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/youtube-play.png\" alt=\"YouTube\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("        if ($settings['social_visibility']['tiktok']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['tiktok']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/tiktok.png\" alt=\"TikTok\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("        if ($settings['social_visibility']['discord']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['discord']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/color/48/000000/discord.png\" alt=\"Discord\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("        if ($settings['social_visibility']['twitter']) echo '<a href=\"' . htmlspecialchars($settings['social_urls']['twitter']) . '\" class=\"social-icon\"><img src=\"https://img.icons8.com/?size=48&id=phOKFKYpe00C&format=png&color=000000\" alt=\"Twitter\" class=\"social-icon-img\"></a>';");
            sb.AppendLine("        ?>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <p>Copyright <?php echo htmlspecialchars($settings['footer_copyright_domain'] ?? 'mydomain.com'); ?></p>");
            sb.AppendLine("      <div class=\"powered-by\">");
            sb.AppendLine("        <span>Compatible</span>");
            sb.AppendLine("        <div class=\"powered-by-images\">");
            sb.AppendLine("          <img src=\"images/razor.png\" alt=\"Razor\">");
            sb.AppendLine("          <img src=\"images/Uosteamlogo.png\" alt=\"UOsteam\" class=\"uosteam-icon\">");
            sb.AppendLine("          <img src=\"images/classicUOLogo.png\" alt=\"ClassicUO\" class=\"classicuo-icon\">");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            sb.AppendLine("</div>");
            sb.AppendLine("<?php } ?>");

            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
}
