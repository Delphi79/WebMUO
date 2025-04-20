using System.Text;

namespace Server.MUOTemplates
{
    public static class UploadTemplate
    {
        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            var sb = new StringBuilder();

            // Base PHP header and configuration
            sb.AppendLine("<?php");
            sb.AppendLine("// upload.php");
            sb.AppendLine();
            sb.AppendLine("// Configuration");
            sb.AppendLine("$uploadPassword = '" + config.UploadPassword + "';");
            sb.AppendLine("$targetSqlPath = __DIR__ . 'sharddata.sql';");
            sb.AppendLine("$targetGumpMapPath = __DIR__ . 'gump_mappings.json';");
            sb.AppendLine("$targetStylesPath = __DIR__ . 'styles.css';");

            // Optional uploads block, including admin.php and styles.css
            string optionalUploadsDeclarations = "";
            string optionalUploadsArrayEntries = "";
            if (config.IncludeOptionalUploads)
            {
                optionalUploadsDeclarations = @"
    $targetJsonPath = __DIR__ . 'sharddata.json';
    $targetIndexPath = __DIR__ . 'index.php';
    $targetPlayersPath = __DIR__ . 'players.php';
    $targetGuildsPath = __DIR__ . 'guilds.php';
    $targetSocialPath = __DIR__ . 'social.php';
    $targetUOToolsPath = __DIR__ . 'uotools.php';
    $targetClientPath = __DIR__ . 'client.php';
    $targetContactPath = __DIR__ . 'contact.php';
    $targetPaperdollPath = __DIR__ . 'paperdoll.php';
    $targetTooltipPath = __DIR__ . 'tooltip.php';
    $targetAdminPath = __DIR__ . 'admin.php';";

                optionalUploadsArrayEntries = @"
    $_FILES['jsonfile']['tmp_name']      => $targetJsonPath,
    $_FILES['indexfile']['tmp_name']     => $targetIndexPath,
    $_FILES['playersfile']['tmp_name']   => $targetPlayersPath,
    $_FILES['guildsfile']['tmp_name']    => $targetGuildsPath,
    $_FILES['socialfile']['tmp_name']    => $targetSocialPath,
    $_FILES['uotoolsfile']['tmp_name']   => $targetUOToolsPath,
    $_FILES['clientfile']['tmp_name']    => $targetClientPath,
    $_FILES['contactfile']['tmp_name']   => $targetContactPath,
    $_FILES['paperdollfile']['tmp_name'] => $targetPaperdollPath,
    $_FILES['tooltipfile']['tmp_name']   => $targetTooltipPath,
    $_FILES['adminfile']['tmp_name']     => $targetAdminPath";
            }
            sb.AppendLine(optionalUploadsDeclarations);
            sb.AppendLine();
            sb.AppendLine("require_once __DIR__ . 'config.php';");
            sb.AppendLine();

            // Validation block
            sb.AppendLine("// Validate required upload fields and password");
            sb.AppendLine("if (");
            sb.AppendLine("    !isset($_POST['key']) || $_POST['key'] !== $uploadPassword || !isset($_FILES['sqlfile']) || !isset($_FILES['gumpmapfile']) || !isset($_FILES['stylesfile'])"); // Added stylesfile validation
            if (config.IncludeOptionalUploads)
            {
                sb.AppendLine("    || !isset($_FILES['jsonfile']) || !isset($_FILES['indexfile']) || !isset($_FILES['playersfile']) || !isset($_FILES['guildsfile'])");
                sb.AppendLine("    || !isset($_FILES['socialfile']) || !isset($_FILES['uotoolsfile']) || !isset($_FILES['clientfile']) || !isset($_FILES['contactfile'])");
                sb.AppendLine("    || !isset($_FILES['paperdollfile']) || !isset($_FILES['tooltipfile']) || !isset($_FILES['adminfile'])");
            }
            sb.AppendLine(") {");
            sb.AppendLine("    http_response_code(403);");
            sb.AppendLine("    die('Missing or invalid upload parameters.');");
            sb.AppendLine("}");
            sb.AppendLine();

            // Upload array and processing
            sb.AppendLine("// Upload and overwrite all provided files");
            sb.AppendLine("$uploads = [");
            sb.AppendLine("    $_FILES['sqlfile']['tmp_name']       => $targetSqlPath,");
            sb.AppendLine("    $_FILES['gumpmapfile']['tmp_name']   => $targetGumpMapPath,");
            sb.AppendLine("    $_FILES['stylesfile']['tmp_name']    => $targetStylesPath" + (config.IncludeOptionalUploads ? "," : "")); // Added styles.css
            if (config.IncludeOptionalUploads)
            {
                sb.AppendLine("");
                sb.AppendLine(optionalUploadsArrayEntries);
            }
            sb.AppendLine("];");
            sb.AppendLine();
            sb.AppendLine("foreach ($uploads as $source => $dest) {");
            sb.AppendLine("    if (!move_uploaded_file($source, $dest)) {");
            sb.AppendLine("        http_response_code(500);");
            sb.AppendLine("        die('Failed to move uploaded file to ' . basename($dest));");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            // SQL execution
            sb.AppendLine("// Execute SQL if present");
            sb.AppendLine("$conn = mysqli_connect($dbHost, $dbUser, $dbPass, $dbName, $dbPort);");
            sb.AppendLine("if (!$conn) {");
            sb.AppendLine("    http_response_code(500);");
            sb.AppendLine("    die('Database connection failed');");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("$sql = file_get_contents($targetSqlPath);");
            sb.AppendLine("if (!$conn->multi_query($sql)) {");
            sb.AppendLine("    mysqli_close($conn);");
            sb.AppendLine("    http_response_code(500);");
            sb.AppendLine("    die('SQL execution failed');");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("mysqli_close($conn);");
            sb.AppendLine("echo 'Success';");
            sb.AppendLine("?>");

            return sb.ToString();
        }
    }
}
