using System.Text;

namespace Server.MUOTemplates
{
    public static class PaperdollTemplate
    {
        public static string Generate(MUOTemplateWriter.TemplateConfig config)
        {
            return $@"<?php
// Error logging configuration
ini_set('display_errors', 0);
ini_set('log_errors', 1);
ini_set('error_log', __DIR__ . '/webmuo_errors.log');
error_reporting(E_ALL);
ini_set('memory_limit', '512M');
set_time_limit(60);

// Debug logging
const LOG_FILE = __DIR__ . '/paperdoll_debug.log';
const MAX_ERROR_LOGS = 100;
$errorLogCount = 0;

function logDebug(string $message, bool $isError = false): void {{
    global $errorLogCount;
    if ($isError && $errorLogCount >= MAX_ERROR_LOGS) {{
        return;
    }}
    if (is_writable(dirname(LOG_FILE))) {{
        file_put_contents(
            LOG_FILE,
            sprintf(""%s - %s\n"", date('Y-m-d H:i:s'), $message),
            FILE_APPEND
        );
    }}
    if ($isError) {{
        $errorLogCount++;
    }}
}}

// Database configuration
require_once __DIR__ . '/config.php';

// Database connection
$link = mysqli_connect($dbHost, $dbUser, $dbPass, $dbName, $dbPort);
if (!$link) {{
    logDebug('Database connection error: ' . mysqli_connect_error(), true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Database connection failed.</p></div></body></html>';
    exit;
}}

// Static body mappings
const BODY_TO_GUMP_MAP = [
    400 => 12, // Human Male
    401 => 13, // Human Female
    605 => 14, // Elf Male
    606 => 15, // Elf Female
    666 => 666, // Garg Male
    667 => 665 // Garg Female
];

// Load item gump mappings
$gumpMappingsFile = __DIR__ . '/gump_mappings.json';
if (!file_exists($gumpMappingsFile)) {{
    logDebug(""Gump mappings file not found: $gumpMappingsFile"", true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Gump mappings file missing.</p></div></body></html>';
    exit;
}}

$gumpMappingsJson = file_get_contents($gumpMappingsFile);
if ($gumpMappingsJson === false) {{
    logDebug(""Failed to read gump mappings: $gumpMappingsFile"", true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Failed to read gump mappings.</p></div></body></html>';
    exit;
}}

$gumpMappings = json_decode($gumpMappingsJson, true);
if ($gumpMappings === null) {{
    logDebug('JSON parsing error: ' . json_last_error_msg(), true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Invalid gump mappings JSON.</p></div></body></html>';
    exit;
}}

$itemToGumpMap = [];
foreach ($gumpMappings as $item) {{
    $itemId = (int)($item['ItemID'] ?? 0);
    $gumpIdMale = (int)($item['GumpID_Male'] ?? 0);
    $gumpIdFemale = (int)($item['GumpID_Female'] ?? 0);
    if ($itemId > 0) {{
        $itemToGumpMap[$itemId] = ""$gumpIdMale/$gumpIdFemale"";
    }}
}}

// Process player data
if (empty($_GET['id'])) {{
    logDebug('No player ID provided', true);
    http_response_code(400);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Player ID required.</p></div></body></html>';
    exit;
}}

$serial = (int)trim($_GET['id']);

$query = ""SELECT serial, name, title, kills, karma, racegender, bodyid, hue, guild, abbr
          FROM players WHERE serial = ?"";
$stmt = mysqli_prepare($link, $query);
mysqli_stmt_bind_param($stmt, 'i', $serial);
mysqli_stmt_execute($stmt);
$result = mysqli_stmt_get_result($stmt);

if (!$result) {{
    logDebug('Player query error: ' . mysqli_error($link), true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Failed to query player data.</p></div></body></html>';
    exit;
}}

if (!($row = mysqli_fetch_assoc($result))) {{
    logDebug(""Player not found: serial $serial"", true);
    http_response_code(404);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Player not found: serial $serial.</p></div></body></html>';
    exit;
}}

$player = [
    'serial' => (int)$row['serial'],
    'name' => $row['name'] ?? '',
    'title' => $row['title'] ?? '',
    'kills' => (int)($row['kills'] ?? 0),
    'karma' => (int)($row['karma'] ?? 0),
    'racegender' => $row['racegender'] ?? '',
    'bodyid' => (int)($row['bodyid'] ?? 0),
    'hue' => (int)($row['hue'] ?? 0),
    'guild' => $row['guild'] ?? '',
    'abbr' => $row['abbr'] ?? ''
];
mysqli_stmt_close($stmt);

// Determine PK status
$isPker = $player['karma'] < 0 && $player['kills'] >= 5;

// Clean title and name
$skillTitles = ['Grandmaster Alchemist', 'Grandmaster', 'Elder', 'Journeyman', 'Apprentice', 'Novice'];
$cleanTitle = $player['title'];
$cleanName = $player['name'];
foreach ($skillTitles as $skill) {{
    $cleanTitle = str_replace($skill, '', $cleanTitle);
    $cleanName = str_replace($skill, '', $cleanName);
}}
$cleanTitle = trim($cleanTitle, "" ,\t\n\r\0\x0B"");
$cleanName = trim($cleanName, "" ,\t\n\r\0\x0B"");
if (strpos($cleanTitle, ',') !== false) {{
    $cleanTitle = trim(explode(',', $cleanTitle, 2)[0]);
}}

// Determine gender and body gump
$isFemale = stripos($player['racegender'], 'fem') !== false;
$bodyGumpId = BODY_TO_GUMP_MAP[$player['bodyid']] ?? $player['bodyid'];

// Fetch equipment
$query = ""SELECT layer, item_name, item_id, hue FROM player_equipment WHERE player_serial = ?"";
$stmt = mysqli_prepare($link, $query);
mysqli_stmt_bind_param($stmt, 'i', $serial);
mysqli_stmt_execute($stmt);
$result = mysqli_stmt_get_result($stmt);

$equipment = [];
if ($result) {{
    while ($row = mysqli_fetch_assoc($result)) {{
        $layer = $row['layer'];
        $itemId = (int)$row['item_id'];
        $hue = (int)$row['hue'];
        $gumpId = getGumpIdFromItemId($itemId, $layer, $isFemale);
        $equipment[$layer] = ['gumpId' => $gumpId, 'hue' => $hue, 'itemId' => $itemId];
    }}
}}
mysqli_stmt_close($stmt);
mysqli_close($link);

// Load MUL files
$mulPath = __DIR__ . '/MulFiles/';
foreach (['hues.mul' => 'hues', 'gumpart.mul' => 'gumpfile', 'gumpidx.mul' => 'gumpindex'] as $file => $var) {{
    $$var = @fopen(""$mulPath$file"", 'rb');
    if (!$$var) {{
        logDebug(""Failed to open $file"", true);
        http_response_code(500);
        echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Unable to open $file.</p></div></body></html>';
        exit;
    }}
}}

$gumpIndexSize = filesize($mulPath . 'gumpidx.mul');
$gumpFileSize = filesize($mulPath . 'gumpart.mul');

// Initialize image
$finalWidth = 1024;
$finalHeight = 1536;
$gumpRawData = initializeGump($finalWidth, $finalHeight);
if (!$gumpRawData) {{
    logDebug('Failed to initialize gump image', true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Failed to initialize image.</p></div></body></html>';
    exit;
}}

// Render character on temporary canvas
$tempWidth = 262;
$tempHeight = 324;
$tempCanvas = imagecreatetruecolor($tempWidth, $tempHeight);
imagealphablending($tempCanvas, false);
imagesavealpha($tempCanvas, true);
$transparent = imagecolorallocatealpha($tempCanvas, 0, 0, 0, 127);
imagefill($tempCanvas, 0, 0, $transparent);

// Render body
try {{
    if (!loadRawGump($gumpindex, $gumpfile, $bodyGumpId, $player['hue'], $hues, $tempCanvas, $gumpIndexSize, $gumpFileSize, 'Body')) {{
        logDebug(""Failed to render body: GumpID=$bodyGumpId"", true);
    }}
}} catch (Exception $e) {{
    logDebug(""Body render exception: {{$e->getMessage()}}"", true);
    http_response_code(500);
    echo '<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><link rel=""stylesheet"" href=""styles.css""></head><body class=""theme-dark""><div class=""content-section""><p>Body rendering failed.</p></div></body></html>';
    exit;
}}

// Layer order
const LAYER_ORDER = [
    'OneHanded' => 1,
    'FirstValid' => 1,
    'Pants' => 2,
    'Shoes' => 3,
    'Shirt' => 4,
    'Gloves' => 5,
    'Ring' => 6,
    'Talisman' => 7,
    'Neck' => 8,
    'Hair' => 9,
    'Helm' => 10,
    'Cloak' => 11,
    'InnerTorso' => 12,
    'Waist' => 13,
    'Earrings' => 14,
    'MiddleTorso' => 15,
    'OuterLegs' => 16,
    'Arms' => 17,
    'Bracelet' => 18,
    'Backpack' => 19,
    'FacialHair' => 20,
    'OuterTorso' => 21,
    'TwoHanded' => 22
];

// Sort equipment
uksort($equipment, fn($a, $b) => (LAYER_ORDER[$a] ?? 0) <=> (LAYER_ORDER[$b] ?? 0));

// Render equipment
$nonRenderLayers = ['Bank', 'ShopBuy', 'ShopResale', 'ShopSell'];
foreach ($equipment as $layer => $itemData) {{
    if (in_array($layer, $nonRenderLayers)) {{
        continue;
    }}

    $gumpId = $itemData['gumpId'];
    $hue = $itemData['hue'];
    $itemId = $itemData['itemId'];

    if ($gumpId <= 0) {{
        logDebug(""Invalid GumpID for layer $layer: $gumpId (ItemID=$itemId)"", true);
        continue;
    }}

    try {{
        $success = loadRawGump($gumpindex, $gumpfile, $gumpId, $hue, $hues, $tempCanvas, $gumpIndexSize, $gumpFileSize, $layer);
        if (!$success && $isFemale) {{
            // Fallback to male GumpID for all layers if female GumpID fails
            $gumpIdString = $itemToGumpMap[$itemId] ?? '';
            if (!empty($gumpIdString)) {{
                $gumpIds = explode('/', (string)$gumpIdString);
                if (count($gumpIds) === 2) {{
                    $maleGumpId = (int)$gumpIds[0];
                    if ($maleGumpId > 0 && $maleGumpId != $gumpId) {{
                        $success = loadRawGump($gumpindex, $gumpfile, $maleGumpId, $hue, $hues, $tempCanvas, $gumpIndexSize, $gumpFileSize, $layer);
                    }}
                }}
            }}
        }}
        if (!$success) {{
            logDebug(""Failed to render layer: $layer, GumpID=$gumpId (ItemID=$itemId)"", true);
        }}
    }} catch (Exception $e) {{
        logDebug(""Layer $layer render exception: {{$e->getMessage()}}"", true);
    }}
}}

// Scale image with adjusted alpha handling
$targetWidth = 900;
$targetHeight = 1350;
$scaledCanvas = scaleImage($tempCanvas, $tempWidth, $tempHeight, $targetWidth, $targetHeight);
imagedestroy($tempCanvas);

// Center and copy to final canvas
$xOffset = ($finalWidth - $targetWidth) / 2;
$yOffset = ($finalHeight - $targetHeight) / 2 - 10;
imagecopy($gumpRawData, $scaledCanvas, (int)$xOffset, (int)$yOffset, 0, 0, $targetWidth, $targetHeight);
imagedestroy($scaledCanvas);

// Add text
$nametitle = stripHtmlChars($cleanTitle . ' ' . $cleanName);
$guildTitle = $player['abbr'] ? ""[{{$player['abbr']}}]"" : '';
addText($gumpRawData, $nametitle, $guildTitle, $player['guild'], $isPker);

// Output image
header('Content-Type: image/png');
imagepng($gumpRawData);
imagedestroy($gumpRawData);

// Cleanup
fclose($hues);
fclose($gumpfile);
fclose($gumpindex);
exit;

function initializeGump(int $width, int $height): GdImage {{
    $img = @imagecreatefrompng(__DIR__ . '/images/char_stand.png');
    if (!$img) {{
        $img = imagecreatetruecolor($width, $height);
        imagefill($img, 0, 0, imagecolorallocate($img, 0, 0, 0));
    }}
    imagealphablending($img, true);
    imagesavealpha($img, true);
    return $img;
}}

function loadRawGump($gumpindex, $gumpfile, int $index, int $hue, $huesFile, GdImage &$canvas, int $indexSize, int $fileSize, string $layer): bool {{
    if ($index < 0 || $index > 0xFFFFF) {{
        return false;
    }}

    // Reset file pointers to ensure consistent access
    fseek($gumpindex, 0, SEEK_SET);
    fseek($gumpfile, 0, SEEK_SET);

    $indexPosition = $index * 12;
    if ($indexPosition + 12 > $indexSize) {{
        return false;
    }}

    fseek($gumpindex, $indexPosition, SEEK_SET);
    if (feof($gumpindex)) {{
        return false;
    }}

    $rawData = fread($gumpindex, 12);
    if (strlen($rawData) != 12) {{
        return false;
    }}

    $lookup = unpack('V', substr($rawData, 0, 4))[1];
    $gsize = unpack('V', substr($rawData, 4, 4))[1];
    $gextra = unpack('V', substr($rawData, 8, 4))[1];
    $gwidth = ($gextra >> 16) & 0xFFFF;
    $gheight = $gextra & 0xFFFF;

    if ($lookup == 0xFFFFFFFF || $lookup < 0 || $lookup >= $fileSize) {{
        return false; // Suppress intermediate failure log; will log only on total failure
    }}

    $gwidth = max(1, $gwidth);
    $gheight = max(1, $gheight);

    fseek($gumpfile, $lookup, SEEK_SET);
    $heightTable = array_fill(0, $gheight, 0);
    for ($y = 0; $y < $gheight; $y++) {{
        $heightTable[$y] = read_big_to_little_endian($gumpfile, 4);
        if ($heightTable[$y] == -1) {{
            return false;
        }}
    }}

    $hueId = $hue;
    $isSpecialHue = ($hueId > 0x8000);
    $color32 = null;
    if ($hueId > 0) {{
        if ($isSpecialHue) {{
            $hueId -= 0x8000;
        }}
        $hueIdForPalette = $hueId - 1; // Adjust for 0-based palette index
        $color32 = getHuePaletteFromHuesMul($huesFile, $hueIdForPalette);
        if ($color32 === false) {{
            $hueId = 0; // Fallback to original colors
        }}
    }}

    $send_data = '';
    $totalPixels = 0;
    for ($y = 0; $y < $gheight; $y++) {{
        fseek($gumpfile, $lookup + $heightTable[$y] * 4, SEEK_SET);
        $x = 0;
        $maxIterations = 10000;
        $iteration = 0;
        while ($x < $gwidth && !feof($gumpfile)) {{
            if ($iteration++ > $maxIterations) {{
                return false;
            }}
            $rle = read_big_to_little_endian($gumpfile, 4);
            if ($rle == -1) {{
                return false;
            }}
            $length = ($rle >> 16) & 0xFFFF;
            $color = $rle & 0xFFFF;
            $totalPixels += $length;
            $r = ($color >> 10) & 0x1F;
            $g = ($color >> 5) & 0x1F;
            $b = $color & 0x1F;

            if ($r == 0 && $g == 0 && $b == 0) {{
                $x += $length;
                continue;
            }}

            $newr = $r * 8;
            $newg = $g * 8;
            $newb = $b * 8;

            if ($hueId > 0 && $color32 !== null) {{
                if ($isSpecialHue) {{
                    if ($r == $g && $r == $b) {{
                        $intensity = $r;
                        $index = min(31, max(0, (int)($intensity)));
                        $hueColor = $color32[$index];
                        $newr = (($hueColor >> 10) & 0x1F) * 8;
                        $newg = (($hueColor >> 5) & 0x1F) * 8;
                        $newb = ($hueColor & 0x1F) * 8;
                    }}
                }} else {{
                    $intensity = $color & 0x7FFF;
                    $index = ($intensity >> 10) & 0x1F;
                    $hueColor = $color32[$index];
                    $newr = (($hueColor >> 10) & 0x1F) * 8;
                    $newg = (($hueColor >> 5) & 0x1F) * 8;
                    $newb = ($hueColor & 0x1F) * 8;
                }}
            }}

            if ($newr > 0 || $newg > 0 || $newb > 0) {{
                $send_data .= $x . ':' . $y . ':' . $newr . ':' . $newg . ':' . $newb . ':' . $length . ':0***';
            }}
            $x += $length;
        }}
        if ($x > $gwidth) {{
            return false;
        }}
    }}

    if (empty($send_data)) {{
        return false;
    }}

    addGump($send_data, $canvas, $layer);
    return true;
}}

function getHuePaletteFromHuesMul($huesFile, $hueId) {{
    if ($hueId < 0 || $hueId > 3000) {{
        return false;
    }}

    $group = floor($hueId / 8);
    $entry = $hueId % 8;
    $offset = $group * 708 + 4 + $entry * 88;

    $huesSize = filesize(__DIR__ . '/MulFiles/hues.mul');
    if ($offset + 64 > $huesSize) {{
        return false;
    }}

    fseek($huesFile, $offset, SEEK_SET);
    $palette = [];
    for ($i = 0; $i < 32; $i++) {{
        $hueColor = read_big_to_little_endian($huesFile, 2);
        if ($hueColor == -1) {{
            return false;
        }}
        $palette[$i] = $hueColor;
    }}
    return $palette;
}}

function addText(GdImage &$img, string $name, string $guildTitle, string $guildName, bool $isPker): void {{
    $fontPath = __DIR__ . '/fonts/FantasyMagist.otf';
    if (!file_exists($fontPath)) {{
        logDebug(""Font missing: $fontPath"", true);
        return;
    }}

    $boxWidth = 900;
    $boxHeight = 180;
    $imageWidth = 1024;
    $boxLeft = ($imageWidth - $boxWidth) / 2;
    $boxTop = 1250;

    $fontSize = 52;
    $bbox = imagettfbbox($fontSize, 0, $fontPath, $name);
    $textWidth = $bbox[2] - $bbox[0];
    $textHeight = $bbox[1] - $bbox[7];

    while ($textWidth > $boxWidth && $fontSize > 12) {{
        $fontSize--;
        $bbox = imagettfbbox($fontSize, 0, $fontPath, $name);
        $textWidth = $bbox[2] - $bbox[0];
        $textHeight = $bbox[1] - $bbox[7];
    }}

    $x = $boxLeft + ($boxWidth - $textWidth) / 2;
    $y = $boxTop + ($boxHeight - $textHeight) / 2;

    $shadow = imagecolorallocate($img, 0, 0, 0);
    $textColor = imagecolorallocate($img, 255, 255, 255);

    imagettftext($img, $fontSize, 0, (int)$x + 2, (int)$y + 2, $shadow, $fontPath, $name);
    imagettftext($img, $fontSize, 0, (int)$x, (int)$y, $textColor, $fontPath, $name);
}}

function read_big_to_little_endian($file, $length) {{
    if (feof($file)) {{
        return -1;
    }}
    $val = fread($file, $length);
    if ($val === false || strlen($val) < $length) {{
        return -1;
    }}
    switch ($length) {{
        case 1: return unpack('C', $val)[1];
        case 2: return unpack('v', $val)[1];
        case 4: return unpack('V', $val)[1];
        default:
            return -1;
    }}
}}

function addGump($read, GdImage &$img, $layer = 'Unknown') {{
    if (empty($read)) {{
        return;
    }}
    $newdata = explode('***', $read);

    $xOffset = in_array($layer, ['TwoHanded', 'FacialHair']) ? 40 : ($layer === 'Backpack' ? 60 : 40);
    $yOffset = 25;

    foreach ($newdata as $val) {{
        if (empty($val)) continue;
        $val = explode(':', $val);
        if (count($val) < 7) {{
            continue;
        }}
        $x = intval($val[0]) + $xOffset;
        $y = intval($val[1]) + $yOffset;
        $r = intval($val[2]);
        $g = intval($val[3]);
        $b = intval($val[4]);
        $length = intval($val[5]);
        $alpha = intval($val[6]);

        if ($r > 255) $r = 255; if ($g > 255) $g = 255; if ($b > 255) $b = 255;
        if ($r < 0) $r = 0; if ($g < 0) $g = 0; if ($b < 0) $b = 0;

        if ($alpha == 0 && ($r > 0 || $g > 0 || $b > 0)) {{
            $col = imagecolorallocatealpha($img, $r, $g, $b, 0);
            for ($i = 0; $i < $length; $i++) {{
                imagesetpixel($img, $x + $i, $y, $col);
            }}
        }}
    }}
}}

function getGumpIdFromItemId(int $itemId, string $layer, bool $isFemale): int {{
    global $itemToGumpMap;
    $gumpIdString = $itemToGumpMap[$itemId] ?? '';
    if (empty($gumpIdString)) {{
        return 0;
    }}

    $gumpIds = explode('/', (string)$gumpIdString);
    if (count($gumpIds) !== 2) {{
        return 0;
    }}

    $gumpId = $isFemale ? (int)$gumpIds[1] : (int)$gumpIds[0];
    if ($gumpId == 0) {{
        return 0;
    }}
    return $gumpId;
}}

function stripHtmlChars(string $str): string {{
    return str_replace(""'"", ""\\'"", $str);
}}

function scaleImage(GdImage $src, int $srcWidth, int $srcHeight, int $targetWidth, int $targetHeight): GdImage {{
    $srcRatio = $srcWidth / $srcHeight;
    $targetRatio = $targetWidth / $targetHeight;

    if ($srcRatio > $targetRatio) {{
        $scaledWidth = $targetWidth;
        $scaledHeight = (int)($targetWidth / $srcRatio);
    }} else {{
        $scaledHeight = $targetHeight;
        $scaledWidth = (int)($targetHeight * $srcRatio);
    }}

    $canvas = imagecreatetruecolor($targetWidth, $targetHeight);
    imagealphablending($canvas, false);
    imagesavealpha($canvas, true);
    $transparent = imagecolorallocatealpha($canvas, 0, 0, 0, 127);
    imagefill($canvas, 0, 0, $transparent);

    $xOffset = ($targetWidth - $scaledWidth) / 2;
    $yOffset = ($targetHeight - $scaledHeight) / 2;

    imagealphablending($src, false);
    imagesavealpha($src, true);

    // Create a temporary canvas to ensure binary transparency
    $tempCanvas = imagecreatetruecolor($srcWidth, $srcHeight);
    imagealphablending($tempCanvas, false);
    imagesavealpha($tempCanvas, true);
    $tempTransparent = imagecolorallocatealpha($tempCanvas, 0, 0, 0, 127);
    imagefill($tempCanvas, 0, 0, $tempTransparent);

    // Copy pixels with binary transparency
    for ($y = 0; $y < $srcHeight; $y++) {{
        for ($x = 0; $x < $srcWidth; $x++) {{
            $color = imagecolorat($src, $x, $y);
            $alpha = ($color >> 24) & 0x7F;
            if ($alpha == 0) {{ // Fully opaque
                imagesetpixel($tempCanvas, $x, $y, $color);
            }}
        }}
    }}

    imagecopyresampled(
        $canvas, $tempCanvas,
        (int)$xOffset, (int)$yOffset,
        0, 0,
        $scaledWidth, $scaledHeight,
        $srcWidth, $srcHeight
    );

    imagedestroy($tempCanvas);
    return $canvas;
}}
";
        }
    }
}
