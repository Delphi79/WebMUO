<?php
// Database configuration
$dbHost = 'localhost';
$dbPort = 3306;//Database Port
$dbName = 'webmuo';//Database Name
$dbUser = 'webmuo_user';//Database Username
$dbPass = 'webmuo_pass';//Database Password
// Admin and upload passwords
$adminPassword = 'admin123';//change this
$uploadPassword = 'ModernUO!';//change this to match 'UploadPassword' field in MUODataExporter.cs
// Upload target paths
$targetSqlPath = __DIR__ . '/sharddata.sql';
$targetGumpMapPath = __DIR__ . '/gump_mappings.json';
$targetStylesPath = __DIR__ . '/styles.css';
$targetJsonPath = __DIR__ . '/sharddata.json';
$targetIndexPath = __DIR__ . '/index.php';
$targetPlayersPath = __DIR__ . '/players.php';
$targetGuildsPath = __DIR__ . '/guilds.php';
$targetSocialPath = __DIR__ . '/social.php';
$targetUOToolsPath = __DIR__ . '/uotools.php';
$targetClientPath = __DIR__ . '/client.php';
$targetContactPath = __DIR__ . '/contact.php';
$targetPaperdollPath = __DIR__ . '/paperdoll.php';
$targetAdminPath = __DIR__ . '/admin.php';
// Log helper
function logMessage($message) {
    $logFile = __DIR__ . '/upload.log';
    $timestamp = date('Y-m-d H:i:s');
    file_put_contents($logFile, "[$timestamp] $message\n", FILE_APPEND);
}
// Only run if config.php is accessed directly
if (basename(__FILE__) === basename($_SERVER['SCRIPT_FILENAME'])) {
    if ($_SERVER['REQUEST_METHOD'] === 'POST') {
        logMessage('Received POST request');
        if (!isset($_POST['key']) || $_POST['key'] !== $uploadPassword) {
            logMessage('Invalid or missing key');
            http_response_code(403);
            die('Invalid or missing key');
        }
        $requiredFiles = ['sqlfile', 'gumpmapfile', 'stylesfile'
        , 'jsonfile', 'indexfile', 'playersfile', 'guildsfile', 'socialfile', 'uotoolsfile', 'clientfile', 'contactfile', 'paperdollfile', 'adminfile'
        ];
        $missingFiles = [];
        foreach ($requiredFiles as $field) {
            if (!isset($_FILES[$field]) || $_FILES[$field]['error'] === UPLOAD_ERR_NO_FILE) {
                $missingFiles[] = $field;
            }
        }
        if (!empty($missingFiles)) {
            logMessage('Missing files: ' . implode(', ', $missingFiles));
            http_response_code(403);
            die('Missing files: ' . implode(', ', $missingFiles));
        }
        $uploads = [
            'sqlfile' => $targetSqlPath,
            'gumpmapfile' => $targetGumpMapPath,
            'stylesfile' => $targetStylesPath,
            'jsonfile' => $targetJsonPath,
            'indexfile' => $targetIndexPath,
            'playersfile' => $targetPlayersPath,
            'guildsfile' => $targetGuildsPath,
            'socialfile' => $targetSocialPath,
            'uotoolsfile' => $targetUOToolsPath,
            'clientfile' => $targetClientPath,
            'contactfile' => $targetContactPath,
            'paperdollfile' => $targetPaperdollPath,
            'adminfile' => $targetAdminPath,
        ];
        foreach ($requiredFiles as $field) {
            $source = $_FILES[$field]['tmp_name'];
            $dest = $uploads[$field];
            if (!move_uploaded_file($source, $dest)) {
                logMessage('Failed to move file to ' . $dest);
                http_response_code(500);
                die('Failed to move file to ' . basename($dest));
            }
            logMessage('Moved file to ' . $dest);
        }
        $conn = mysqli_connect($dbHost, $dbUser, $dbPass, $dbName, $dbPort);
        if (!$conn) {
            logMessage('Database connection failed: ' . mysqli_connect_error());
            http_response_code(500);
            die('Database connection failed');
        }
        $sql = file_get_contents($targetSqlPath);
        if (!$conn->multi_query($sql)) {
            $error = $conn->error;
            mysqli_close($conn);
            logMessage('SQL execution failed: ' . $error);
            http_response_code(500);
            die('SQL execution failed: ' . $error);
        }
        while ($conn->more_results() && $conn->next_result()) {}
        mysqli_close($conn);
        logMessage('Upload and SQL processing complete');
        echo 'Success';
    } else {
        logMessage('Non-POST request received');
    }
}
?>
