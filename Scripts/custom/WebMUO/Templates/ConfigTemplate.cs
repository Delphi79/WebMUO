using System.Text;

namespace Server.MUOTemplates
{
    public static class ConfigAndUploadTemplate
    {
        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<?php");
            sb.AppendLine("// Database configuration");
            sb.AppendLine("$dbHost = 'localhost';");//Database Host
            sb.AppendLine("$dbPort = 3306;//Database Port");
            sb.AppendLine("$dbName = 'webmuo';//Database Name");
            sb.AppendLine("$dbUser = 'webmuo_user';//Database Username");
            sb.AppendLine("$dbPass = 'webmuo_pass';//Database Password");

            sb.AppendLine("// Admin and upload passwords");
            sb.AppendLine("$adminPassword = '" + config.AdminPassword + "';//change this");
            sb.AppendLine("$uploadPassword = '" + config.UploadPassword + "';//change this to match 'UploadPassword' field in MUODataExporter.cs");

            sb.AppendLine("// Upload target paths");
            sb.AppendLine("$targetSqlPath = __DIR__ . '/sharddata.sql';");
            sb.AppendLine("$targetGumpMapPath = __DIR__ . '/gump_mappings.json';");
            sb.AppendLine("$targetStylesPath = __DIR__ . '/styles.css';");

            if (config.IncludeOptionalUploads)
            {
                sb.AppendLine("$targetJsonPath = __DIR__ . '/sharddata.json';");
                sb.AppendLine("$targetIndexPath = __DIR__ . '/index.php';");
                sb.AppendLine("$targetPlayersPath = __DIR__ . '/players.php';");
                sb.AppendLine("$targetGuildsPath = __DIR__ . '/guilds.php';");
                sb.AppendLine("$targetSocialPath = __DIR__ . '/social.php';");
                sb.AppendLine("$targetUOToolsPath = __DIR__ . '/uotools.php';");
                sb.AppendLine("$targetClientPath = __DIR__ . '/client.php';");
                sb.AppendLine("$targetContactPath = __DIR__ . '/contact.php';");
                sb.AppendLine("$targetPaperdollPath = __DIR__ . '/paperdoll.php';");
                sb.AppendLine("$targetAdminPath = __DIR__ . '/admin.php';");
            }

            sb.AppendLine("// Log helper");
            sb.AppendLine("function logMessage($message) {");
            sb.AppendLine("    $logFile = __DIR__ . '/upload.log';");
            sb.AppendLine("    $timestamp = date('Y-m-d H:i:s');");
            sb.AppendLine("    file_put_contents($logFile, \"[$timestamp] $message\\n\", FILE_APPEND);");
            sb.AppendLine("}");

            sb.AppendLine("// Only run if config.php is accessed directly");
            sb.AppendLine("if (basename(__FILE__) === basename($_SERVER['SCRIPT_FILENAME'])) {");
            sb.AppendLine("    if ($_SERVER['REQUEST_METHOD'] === 'POST') {");
            sb.AppendLine("        logMessage('Received POST request');");
            sb.AppendLine("        if (!isset($_POST['key']) || $_POST['key'] !== $uploadPassword) {");
            sb.AppendLine("            logMessage('Invalid or missing key');");
            sb.AppendLine("            http_response_code(403);");
            sb.AppendLine("            die('Invalid or missing key');");
            sb.AppendLine("        }");

            sb.AppendLine("        $requiredFiles = ['sqlfile', 'gumpmapfile', 'stylesfile'");
            if (config.IncludeOptionalUploads)
            {
                sb.AppendLine("        , 'jsonfile', 'indexfile', 'playersfile', 'guildsfile', 'socialfile', 'uotoolsfile', 'clientfile', 'contactfile', 'paperdollfile', 'adminfile'");
            }
            sb.AppendLine("        ];");
            sb.AppendLine("        $missingFiles = [];");
            sb.AppendLine("        foreach ($requiredFiles as $field) {");
            sb.AppendLine("            if (!isset($_FILES[$field]) || $_FILES[$field]['error'] === UPLOAD_ERR_NO_FILE) {");
            sb.AppendLine("                $missingFiles[] = $field;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        if (!empty($missingFiles)) {");
            sb.AppendLine("            logMessage('Missing files: ' . implode(', ', $missingFiles));");
            sb.AppendLine("            http_response_code(403);");
            sb.AppendLine("            die('Missing files: ' . implode(', ', $missingFiles));");
            sb.AppendLine("        }");

            sb.AppendLine("        $uploads = [");
            sb.AppendLine("            'sqlfile' => $targetSqlPath,");
            sb.AppendLine("            'gumpmapfile' => $targetGumpMapPath,");
            sb.AppendLine("            'stylesfile' => $targetStylesPath,");
            if (config.IncludeOptionalUploads)
            {
                sb.AppendLine("            'jsonfile' => $targetJsonPath,");
                sb.AppendLine("            'indexfile' => $targetIndexPath,");
                sb.AppendLine("            'playersfile' => $targetPlayersPath,");
                sb.AppendLine("            'guildsfile' => $targetGuildsPath,");
                sb.AppendLine("            'socialfile' => $targetSocialPath,");
                sb.AppendLine("            'uotoolsfile' => $targetUOToolsPath,");
                sb.AppendLine("            'clientfile' => $targetClientPath,");
                sb.AppendLine("            'contactfile' => $targetContactPath,");
                sb.AppendLine("            'paperdollfile' => $targetPaperdollPath,");
                sb.AppendLine("            'adminfile' => $targetAdminPath,");
            }
            sb.AppendLine("        ];");

            sb.AppendLine("        foreach ($requiredFiles as $field) {");
            sb.AppendLine("            $source = $_FILES[$field]['tmp_name'];");
            sb.AppendLine("            $dest = $uploads[$field];");
            sb.AppendLine("            if (!move_uploaded_file($source, $dest)) {");
            sb.AppendLine("                logMessage('Failed to move file to ' . $dest);");
            sb.AppendLine("                http_response_code(500);");
            sb.AppendLine("                die('Failed to move file to ' . basename($dest));");
            sb.AppendLine("            }");
            sb.AppendLine("            logMessage('Moved file to ' . $dest);");
            sb.AppendLine("        }");

            sb.AppendLine("        $conn = mysqli_connect($dbHost, $dbUser, $dbPass, $dbName, $dbPort);");
            sb.AppendLine("        if (!$conn) {");
            sb.AppendLine("            logMessage('Database connection failed: ' . mysqli_connect_error());");
            sb.AppendLine("            http_response_code(500);");
            sb.AppendLine("            die('Database connection failed');");
            sb.AppendLine("        }");

            sb.AppendLine("        $sql = file_get_contents($targetSqlPath);");
            sb.AppendLine("        if (!$conn->multi_query($sql)) {");
            sb.AppendLine("            $error = $conn->error;");
            sb.AppendLine("            mysqli_close($conn);");
            sb.AppendLine("            logMessage('SQL execution failed: ' . $error);");
            sb.AppendLine("            http_response_code(500);");
            sb.AppendLine("            die('SQL execution failed: ' . $error);");
            sb.AppendLine("        }");

            sb.AppendLine("        while ($conn->more_results() && $conn->next_result()) {}");
            sb.AppendLine("        mysqli_close($conn);");
            sb.AppendLine("        logMessage('Upload and SQL processing complete');");
            sb.AppendLine("        echo 'Success';");
            sb.AppendLine("    } else {");
            sb.AppendLine("        logMessage('Non-POST request received');");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("?>");

            return sb.ToString();
        }
    }
}
