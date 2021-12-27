using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using CodeTools;
using MaterialSkin.Controls;

namespace BatchIDE
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        public static string s1;
        public Form1()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Indigo500, MaterialSkin.Primary.Indigo700, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Indigo100, MaterialSkin.TextShade.WHITE);
        }


        private static Random random = new Random();
        public static String RandomString(int length) 
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string output;
        public string output2;

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch1.Checked == true)
            {
                materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            }
            else
            {
                materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string enc = "";
            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Please enter a file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // try
                    {
                        string code = textBox1.Text;
                        
                        if (checkBox2.Checked == true)
                        {
                            var input = code;
                            string bruh = input = string.Concat(input.Select(x => Char.IsLetterOrDigit(x) ? "%" + RandomString(6) + "%" + x : x.ToString())).TrimStart();
                            output = bruh;
                        }
                        else if (checkBox2.Checked == false)
                        {
                            output = code;
                        }
                        if (materialCheckbox1.Checked == true)
                        {
                            var p1 = output + "\nDel %~0";
                            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(p1);
                            var outy = System.Convert.ToBase64String(plainTextBytes);

                            s1 = "@echo off\nCERTUTIL -f -decode \"%~f0\" \"%Temp%/test.bat\" >nul 2>&1 \ncls\n\"%Temp%/test.bat\"\nExit\n-----BEGIN CERTIFICATE-----\n" + outy + "\n-----END CERTIFICATE-----";
                            File.WriteAllText(Environment.CurrentDirectory + "/" + textBox2.Text + ".bat", s1);
                            output = s1;
                            enc = s1;


                        }
                        if (materialCheckbox2.Checked == true)
                        {
                            var unicodeHeader = new byte[] { 0xFF, 0xFE, 0x0D, 0x0A }; //Code script from BatchProtector
                            var obfuscatedCodeByteArray = new byte[] { };
                            obfuscatedCodeByteArray = Encoding.ASCII.GetBytes("cls" + Environment.NewLine + enc);
                            var concatenatedByteArray = new byte[unicodeHeader.Length + obfuscatedCodeByteArray.Length + 1];
                            unicodeHeader.CopyTo(concatenatedByteArray, 0);
                            obfuscatedCodeByteArray.CopyTo(concatenatedByteArray, unicodeHeader.Length);

                            File.WriteAllBytes(Environment.CurrentDirectory + "/" + textBox2.Text + ".bat", concatenatedByteArray);

                        }
                        if (optionCompileCS.Checked == true) { CompileToCS(); MessageBox.Show("Successfully Generated C# File. Now Making Batch File.", "Success!" + MessageBoxIcon.Information);}
                        else if (optionCompileExe.Checked == true)
                        {
                            CompileToCS();
                            MessageBox.Show("If the executable compile process not work you may not have Dotnet Developer Tools installed.", "Disclaimer!!!" + MessageBoxIcon.Information);
                            ProcessStartInfo ps = new ProcessStartInfo();
                            ps.Arguments = Environment.CurrentDirectory + "/" + textBox2.Text + ".cs";
                            ps.FileName = Environment.CurrentDirectory + "/csc/csc.exe";
                            ps.WindowStyle = ProcessWindowStyle.Hidden;
                            Process.Start(ps);
                            System.Threading.Thread.Sleep(3000);
                            File.Delete(Environment.CurrentDirectory + $"/{textBox2.Text}.cs");
                        }

                    }
                    //catch { MessageBox.Show("Fatal Error Occured", "Error"); }
                }
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "An Error Occured While Obfuscaing" + MessageBoxIcon.Error); }
        }

        public  void CompileToCS()
        {
            {
                string outy = output + "\nDel %~0";
                string configData1 = File.ReadAllText(Environment.CurrentDirectory + "/config.txt");
                string configData2 = File.ReadAllText(Environment.CurrentDirectory + "/config2.txt");
                Obfuscator o = new Obfuscator();
                string out2 = o.Obfuscate(outy, "123");
                string config = $"{out2}";
                string conjoined = configData1 + Environment.NewLine + config + configData2;

                File.WriteAllText(Environment.CurrentDirectory + "/" + textBox2.Text + ".cs", conjoined);
                /*/
                Process pr = new Process();
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Environment.CurrentDirectory + "/csc.exe";
                startInfo.Arguments = textBox2.Text + ".cs";
                process.StartInfo = startInfo;
                process.Start();
                /*/
            }
            File.WriteAllText(Environment.CurrentDirectory + "/" + textBox2.Text + ".bat", output);
            MessageBox.Show("Obfuscated Successfully", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void materialRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
