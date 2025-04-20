using System.Text;

namespace Server.MUOTemplates
{
    public static class StylesCssTemplate
    {
        public static string Generate()
        {
            var sb = new StringBuilder();

            sb.AppendLine("/* Global reset and box-sizing */");
            sb.AppendLine("* {");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(":root {");
            sb.AppendLine("    --text-color: #ffffff; /* Default text color, overridden by PHP in templates */");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("html {");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    font-family: Arial, sans-serif;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    overflow-x: hidden;"); // Prevent horizontal scroll
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("body {");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    background: none;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    min-height: 100vh;"); // Ensure full viewport height
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".container {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    min-height: 100vh;");
            sb.AppendLine("    width: 100vw;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    z-index: 1;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".main-content {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    overflow: hidden;");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    z-index: 1;");
            sb.AppendLine("    height: calc(100vh - 120px);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".top-bar {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    justify-content: space-between;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    background-color: #000;");
            sb.AppendLine("    padding: 10px 0;");
            sb.AppendLine("    border-bottom: 1px solid #333;");
            sb.AppendLine("    position: fixed;");
            sb.AppendLine("    top: 0;");
            sb.AppendLine("    left: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    z-index: 1000;");
            sb.AppendLine("    height: 60px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".logo img {");
            sb.AppendLine("    height: 40px;");
            sb.AppendLine("    width: auto;");
            sb.AppendLine("    margin-left: 10px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".menu ul {");
            sb.AppendLine("    list-style: none;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    gap: 15px;");
            sb.AppendLine("    margin-right: 10px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".menu li {");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".menu a {");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    text-decoration: none;");
            sb.AppendLine("    font-size: 14px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".menu a:hover {");
            sb.AppendLine("    color: #ffcc00;");
            sb.AppendLine("    text-decoration: underline;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".menu .active {");
            sb.AppendLine("    font-weight: bold;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by-header {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    justify-content: flex-end;");
            sb.AppendLine("    padding: 5px 10px;");
            sb.AppendLine("    background: transparent;");
            sb.AppendLine("    z-index: 2000;"); // From previous fix
            sb.AppendLine("    position: absolute;");
            sb.AppendLine("    top: 60px;");
            sb.AppendLine("    right: 90px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by-header span {");
            sb.AppendLine("    font-size: 14px;");
            sb.AppendLine("    margin-right: 5px;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by-header img {");
            sb.AppendLine("    height: 20px;");
            sb.AppendLine("    width: auto;");
            sb.AppendLine("    filter: drop-shadow(0 0 2px rgba(255, 255, 255, 0.5));");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-wrapper-index {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex: 1;");
            sb.AppendLine("    overflow: hidden;");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    margin-top: 60px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".status-section-index {");
            sb.AppendLine("    flex: 0 0 20%;");
            sb.AppendLine("    background-color: rgba(0, 0, 0, 0.7);");
            sb.AppendLine("    padding: 15px;");
            sb.AppendLine("    border-right: 1px solid #333;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    word-wrap: break-word;");
            sb.AppendLine("    overflow-wrap: break-word;");
            sb.AppendLine("    margin-left: 0;");
            sb.AppendLine("    padding-left: 15px;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    position: absolute;");
            sb.AppendLine("    top: 0;");
            sb.AppendLine("    left: 0;");
            sb.AppendLine("    height: 75%;");
            sb.AppendLine("    z-index: 2;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".status-section-index h3 {");
            sb.AppendLine("    margin-bottom: 10px;");
            sb.AppendLine("    font-size: 18px;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".status-section-index p {");
            sb.AppendLine("    margin: 5px 0;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".status-online {");
            sb.AppendLine("    color: green;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".status-offline {");
            sb.AppendLine("    color: red;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".main-section-index {");
            sb.AppendLine("    flex: 0 0 60%;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    margin-left: 20%;");
            sb.AppendLine("    margin-right: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".right-spacer-index {");
            sb.AppendLine("    flex: 0 0 20%;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-index {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    padding: 20px 0;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    justify-content: center;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: 600px;");
            sb.AppendLine("    margin: 0 auto;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-index h1 {");
            sb.AppendLine("    font-size: 2.5em;");
            sb.AppendLine("    margin-bottom: 10px;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-index p {");
            sb.AppendLine("    font-size: 1.2em;");
            sb.AppendLine("    margin-bottom: 20px;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".download-button {");
            sb.AppendLine("    display: inline-block;");
            sb.AppendLine("    padding: 15px 30px;");
            sb.AppendLine("    background-color: #ffcc00;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    text-decoration: none;");
            sb.AppendLine("    font-size: 1.2em;");
            sb.AppendLine("    border-radius: 5px;");
            sb.AppendLine("    font-weight: bold;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".download-button:hover {");
            sb.AppendLine("    background-color: #e6b800;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-wrapper-players {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex: 1;");
            sb.AppendLine("    overflow: hidden;");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    margin-top: 60px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".main-section-players {");
            sb.AppendLine("    flex: 0 0 100%;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    margin-left: 0;");
            sb.AppendLine("    margin-right: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    min-height: calc(100vh - 120px);");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-players {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    justify-content: flex-start;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: none;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-players h1 {");
            sb.AppendLine("    font-size: 2.5em;");
            sb.AppendLine("    margin: 0 0 10px 0;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    z-index: 10;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-section-players {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: none;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    min-height: 0;");
            sb.AppendLine("    max-height: calc(100vh - 160px);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-container-players {");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("    overflow-x: auto;");
            sb.AppendLine("    max-height: 100%;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-wrapper-guilds {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex: 1;");
            sb.AppendLine("    overflow: hidden;");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    margin-top: 60px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".main-section-guilds {");
            sb.AppendLine("    flex: 0 0 100%;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    margin-left: 0;");
            sb.AppendLine("    margin-right: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    min-height: calc(100vh - 120px);");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-guilds {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    justify-content: flex-start;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: none;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-guilds h1 {");
            sb.AppendLine("    font-size: 2.5em;");
            sb.AppendLine("    margin: 0 0 10px 0;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    z-index: 10;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-section-guilds {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: none;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    min-height: 0;");
            sb.AppendLine("    max-height: calc(100vh - 160px);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-container-guilds {");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("    overflow-x: auto;");
            sb.AppendLine("    max-height: 100%;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-wrapper-uotools {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex: 1;");
            sb.AppendLine("    overflow: hidden;");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    margin-top: 60px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".main-section-uotools {");
            sb.AppendLine("    flex: 0 0 100%;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    margin-left: 0;");
            sb.AppendLine("    margin-right: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    min-height: calc(100vh - 120px);");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-uotools {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    justify-content: flex-start;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: none;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".content-section-uotools h1 {");
            sb.AppendLine("    font-size: 2.5em;");
            sb.AppendLine("    margin: 0 0 10px 0;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    position: relative;");
            sb.AppendLine("    z-index: 10;");
            sb.AppendLine("    display: block;");
            sb.AppendLine("    visibility: visible;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-section-uotools {");
            sb.AppendLine("    flex-grow: 1;");
            sb.AppendLine("    overflow-y: auto;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: none;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    min-height: 0;");
            sb.AppendLine("    max-height: calc(100vh - 160px);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-container-uotools {");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("    overflow-x: auto;");
            sb.AppendLine("    max-height: 100%;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("table {");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    border-collapse: collapse;");
            sb.AppendLine("    table-layout: auto;");
            sb.AppendLine("    min-width: 800px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("th, td {");
            sb.AppendLine("    padding: 10px;");
            sb.AppendLine("    border: 1px solid #333;");
            sb.AppendLine("    text-align: left;");
            sb.AppendLine("    white-space: nowrap;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-container-players th:nth-child(1), .table-container-players td:nth-child(1) {");
            sb.AppendLine("    min-width: 40px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(2), .table-container-players td:nth-child(2) {");
            sb.AppendLine("    min-width: 120px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(3), .table-container-players td:nth-child(3) {");
            sb.AppendLine("    min-width: 200px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(4), .table-container-players td:nth-child(4) {");
            sb.AppendLine("    min-width: 120px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(5), .table-container-players td:nth-child(5) {");
            sb.AppendLine("    min-width: 40px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(6), .table-container-players td:nth-child(6) {");
            sb.AppendLine("    min-width: 40px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(7), .table-container-players td:nth-child(7) {");
            sb.AppendLine("    min-width: 60px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(8), .table-container-players td:nth-child(8) {");
            sb.AppendLine("    min-width: 60px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-players th:nth-child(9), .table-container-players td:nth-child(9) {");
            sb.AppendLine("    min-width: 80px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-container-guilds th:nth-child(1), .table-container-guilds td:nth-child(1) {");
            sb.AppendLine("    min-width: 200px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-guilds th:nth-child(2), .table-container-guilds td:nth-child(2) {");
            sb.AppendLine("    min-width: 40px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-guilds th:nth-child(3), .table-container-guilds td:nth-child(3) {");
            sb.AppendLine("    min-width: 100px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-guilds th:nth-child(4), .table-container-guilds td:nth-child(4) {");
            sb.AppendLine("    min-width: 60px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-guilds th:nth-child(5), .table-container-guilds td:nth-child(5) {");
            sb.AppendLine("    min-width: 40px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-guilds th:nth-child(6), .table-container-guilds td:nth-child(6) {");
            sb.AppendLine("    min-width: 40px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".table-container-uotools th:nth-child(1), .table-container-uotools td:nth-child(1) {");
            sb.AppendLine("    min-width: 150px;");
            sb.AppendLine("}");
            sb.AppendLine(".table-container-uotools th:nth-child(2), .table-container-uotools td:nth-child(2) {");
            sb.AppendLine("    min-width: 200px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("th {");
            sb.AppendLine("    background-color: #222;");
            sb.AppendLine("    position: sticky;");
            sb.AppendLine("    top: 0;");
            sb.AppendLine("    z-index: 10;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("tr:nth-child(even) {");
            sb.AppendLine("    background-color: #1a1a1a;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("tr:nth-child(odd) {");
            sb.AppendLine("    background-color: #121212;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".status-dot {");
            sb.AppendLine("    width: 10px;");
            sb.AppendLine("    height: 10px;");
            sb.AppendLine("    border-radius: 50%;");
            sb.AppendLine("    display: inline-block;");
            sb.AppendLine("    vertical-align: middle;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".online {");
            sb.AppendLine("    background-color: green;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".offline {");
            sb.AppendLine("    background-color: red;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".player-icon {");
            sb.AppendLine("    width: 32px;");
            sb.AppendLine("    height: 32px;");
            sb.AppendLine("    vertical-align: middle;");
            sb.AppendLine("    margin-right: 8px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".player-name {");
            sb.AppendLine("    white-space: nowrap;");
            sb.AppendLine("    overflow: hidden;");
            sb.AppendLine("    text-overflow: ellipsis;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("a {");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    text-decoration: none;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("a:hover {");
            sb.AppendLine("    text-decoration: underline;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".footer {");
            sb.AppendLine("    background-color: #222;");
            sb.AppendLine("    padding: 0;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    border-top: 1px solid #333;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    z-index: 1000;");
            sb.AppendLine("    position: fixed;");
            sb.AppendLine("    bottom: 0;");
            sb.AppendLine("    left: 0;");
            sb.AppendLine("    height: 60px;");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".footer-container {");
            sb.AppendLine("    padding: 10px 10px;");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    justify-content: space-between;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    height: 100%;");
            sb.AppendLine("    flex-shrink: 0;");
            sb.AppendLine("    z-index: 31;");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".social-icons {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    gap: 10px;");
            sb.AppendLine("    z-index: 32;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".social-icons a {");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("    display: inline-flex;");
            sb.AppendLine("    width: 24px;");
            sb.AppendLine("    height: 24px;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    justify-content: center;");
            sb.AppendLine("    text-decoration: none;");
            sb.AppendLine("    z-index: 32;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".social-icons img {");
            sb.AppendLine("    width: 24px;");
            sb.AppendLine("    height: 24px;");
            sb.AppendLine("    filter: drop-shadow(0 0 2px rgba(255, 255, 255, 0.5));");
            sb.AppendLine("    transition: opacity 0.3s;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".social-icons a:hover img {");
            sb.AppendLine("    opacity: 0.8;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    flex-direction: column;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    gap: 5px;");
            sb.AppendLine("    z-index: 32;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by-images {");
            sb.AppendLine("    display: flex;");
            sb.AppendLine("    align-items: center;");
            sb.AppendLine("    gap: 10px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by span {");
            sb.AppendLine("    font-size: 14px;");
            sb.AppendLine("    filter: drop-shadow(0 0 2px rgba(255, 255, 255, 0.5));");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".powered-by img {");
            sb.AppendLine("    max-height: 30px;");
            sb.AppendLine("    width: auto;");
            sb.AppendLine("    max-width: 60px;");
            sb.AppendLine("    filter: drop-shadow(0 0 2px rgba(255, 255, 255, 0.5));");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".uosteam-icon {");
            sb.AppendLine("    max-height: 25px;");
            sb.AppendLine("    max-width: 50px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".classicuo-icon {");
            sb.AppendLine("    max-height: 50px;");
            sb.AppendLine("    max-width: 80px;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".theme-dark {");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("}");
            sb.AppendLine();

            // New responsive styles for paperdoll and error pages
            sb.AppendLine("/* Paperdoll image responsiveness */");
            sb.AppendLine(".paperdoll-img {");
            sb.AppendLine("    max-width: 100%;"); // Scale to container width
            sb.AppendLine("    height: auto;"); // Preserve aspect ratio
            sb.AppendLine("    display: block;"); // Remove inline spacing
            sb.AppendLine("    margin: 0 auto;"); // Center horizontally
            sb.AppendLine("}");
            sb.AppendLine();

            // Style for paperdoll.php error pages
            sb.AppendLine(".theme-dark .content-section {");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    max-width: 800px;"); // Constrain width on large screens
            sb.AppendLine("    margin: 20px auto;"); // Center with padding
            sb.AppendLine("    padding: 20px;");
            sb.AppendLine("    background: rgba(0, 0, 0, 0.8);"); // Semi-transparent background
            sb.AppendLine("    border-radius: 5px;");
            sb.AppendLine("    text-align: center;");
            sb.AppendLine("    color: var(--text-color);");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine(".theme-dark .content-section p {");
            sb.AppendLine("    font-size: 1.2em;");
            sb.AppendLine("    margin: 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            // Media queries for responsiveness
            sb.AppendLine("@media (max-width: 768px) {");
            sb.AppendLine("    .paperdoll-img {");
            sb.AppendLine("        max-width: 80%;"); // Slightly smaller on tablets
            sb.AppendLine("    }");
            sb.AppendLine("    .theme-dark .content-section {");
            sb.AppendLine("        margin: 10px;"); // Less margin on smaller screens
            sb.AppendLine("        padding: 15px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .theme-dark .content-section p {");
            sb.AppendLine("        font-size: 1em;"); // Smaller text on mobile
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("@media (max-width: 480px) {");
            sb.AppendLine("    .paperdoll-img {");
            sb.AppendLine("        max-width: 100%;"); // Full width on phones
            sb.AppendLine("    }");
            sb.AppendLine("    .theme-dark .content-section {");
            sb.AppendLine("        margin: 5px;");
            sb.AppendLine("        padding: 10px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .theme-dark .content-section p {");
            sb.AppendLine("        font-size: 0.9em;");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            // Ensure parent page responsiveness
            sb.AppendLine("/* Enhance existing content sections for paperdoll embedding */");
            sb.AppendLine(".content-section-players, .content-section-index {");
            sb.AppendLine("    width: 100%;");
            sb.AppendLine("    box-sizing: border-box;");
            sb.AppendLine("}");
            sb.AppendLine("@media (max-width: 768px) {");
            sb.AppendLine("    .content-section-players, .content-section-index {");
            sb.AppendLine("        padding: 10px;");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            // Existing media queries
            sb.AppendLine("@media (max-width: 768px) {");
            sb.AppendLine("    .top-bar {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        padding: 10px 0;");
            sb.AppendLine("        height: auto;");
            sb.AppendLine("    }");
            sb.AppendLine("    .logo img {");
            sb.AppendLine("        margin-left: 0;");
            sb.AppendLine("        margin-bottom: 10px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .menu ul {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        gap: 5px;");
            sb.AppendLine("        margin-right: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-wrapper-index {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        margin-top: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-wrapper-players {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        margin-top: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-wrapper-guilds {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        margin-top: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-wrapper-uotools {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        margin-top: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .status-section-index {");
            sb.AppendLine("        flex: 0 0 100%;");
            sb.AppendLine("        padding: 10px;");
            sb.AppendLine("        margin-left: 0;");
            sb.AppendLine("        position: relative;");
            sb.AppendLine("        height: auto;");
            sb.AppendLine("        top: auto;");
            sb.AppendLine("        left: auto;");
            sb.AppendLine("    }");
            sb.AppendLine("    .main-section-index {");
            sb.AppendLine("        flex: 0 0 100%;");
            sb.AppendLine("        margin-left: 0;");
            sb.AppendLine("        margin-right: 0;");
            sb.AppendLine("        padding: 0 5px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .right-spacer-index {");
            sb.AppendLine("        display: none;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-index {");
            sb.AppendLine("        padding: 10px 0;");
            sb.AppendLine("        max-width: 100%;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-index h1 {");
            sb.AppendLine("        font-size: 2em;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-index p {");
            sb.AppendLine("        font-size: 1em;");
            sb.AppendLine("    }");
            sb.AppendLine("    .download-button {");
            sb.AppendLine("        padding: 10px 20px;");
            sb.AppendLine("        font-size: 1em;");
            sb.AppendLine("    }");
            sb.AppendLine("    .main-section-players {");
            sb.AppendLine("        flex: 0 0 100%;");
            sb.AppendLine("        margin-left: 0;");
            sb.AppendLine("        margin-right: 0;");
            sb.AppendLine("        padding: 0 5px;");
            sb.AppendLine("        min-height: calc(100vh - 120px);");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-players {");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-players h1 {");
            sb.AppendLine("        font-size: 2em;");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .table-section-players {");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("        max-height: calc(100vh - 150px);");
            sb.AppendLine("    }");
            sb.AppendLine("    .main-section-guilds {");
            sb.AppendLine("        flex: 0 0 100%;");
            sb.AppendLine("        margin-left: 0;");
            sb.AppendLine("        margin-right: 0;");
            sb.AppendLine("        padding: 0 5px;");
            sb.AppendLine("        min-height: calc(100vh - 120px);");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-guilds {");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-guilds h1 {");
            sb.AppendLine("        font-size: 2em;");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .table-section-guilds {");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("        max-height: calc(100vh - 150px);");
            sb.AppendLine("    }");
            sb.AppendLine("    .main-section-uotools {");
            sb.AppendLine("        flex: 0 0 100%;");
            sb.AppendLine("        margin-left: 0;");
            sb.AppendLine("        margin-right: 0;");
            sb.AppendLine("        padding: 0 5px;");
            sb.AppendLine("        min-height: calc(100vh - 120px);");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-uotools {");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .content-section-uotools h1 {");
            sb.AppendLine("        font-size: 2em;");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("    }");
            sb.AppendLine("    .table-section-uotools {");
            sb.AppendLine("        padding: 0;");
            sb.AppendLine("        max-height: calc(100vh - 150px);");
            sb.AppendLine("    }");
            sb.AppendLine("    .footer {");
            sb.AppendLine("        height: auto;");
            sb.AppendLine("    }");
            sb.AppendLine("    .footer-container {");
            sb.AppendLine("        flex-direction: column;");
            sb.AppendLine("        padding: 10px;");
            sb.AppendLine("        height: auto;");
            sb.AppendLine("    }");
            sb.AppendLine("    .social-icons {");
            sb.AppendLine("        margin-bottom: 10px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .powered-by {");
            sb.AppendLine("        gap: 3px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .powered-by-images {");
            sb.AppendLine("        gap: 5px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .powered-by img {");
            sb.AppendLine("        max-height: 25px;");
            sb.AppendLine("        max-width: 50px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .uosteam-icon {");
            sb.AppendLine("        max-height: 20px;");
            sb.AppendLine("        max-width: 40px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .classicuo-icon {");
            sb.AppendLine("        max-height: 35px;");
            sb.AppendLine("        max-width: 60px;");
            sb.AppendLine("    }");
            sb.AppendLine("    .powered-by-header {");
            sb.AppendLine("        justify-content: center;");
            sb.AppendLine("        padding: 5px;");
            sb.AppendLine("        top: 120px;");
            sb.AppendLine("        right: 0;");
            sb.AppendLine("        width: 100%;");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
