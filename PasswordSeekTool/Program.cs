using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace PasswordSeekTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string str = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDO3zzIlPw/Zhpbnj8FrLXihT3pgXMmiV4z61EIVtgaRWAaVL3zZxnmjvYo+QLflGrfPGdo43JjpoTVeeERBJyyOx9jqlHQRQCC2kAqGOje8GYxHI+dQzZk6Ky3OYlJvP/VGAKpDHCQXAFnT4xnBNIbskr0QtIJ4vj9YgdBEZoOKQIDAQAB";

            var data = Convert.FromBase64String(str);
            string hex = HexUtil.Byte2Hex(data);
            hex = "8f930e1559ba09143edcc01f542628f1ace3230f85936010b0e4f227db3534d71a83f779ab656e3d0f357913690593bc6e1e37078700503a92a64ac16b4e71cff11ace708f76fe49d8b24e13e8402d775f0d6b0f615c9e4f0d055d284e0b220802c499f7751994a1c22f360c61835c3ceef4cbad650d8a61744e78e463581acd";
            var data2 = RSAHelper.Encrypt_PEM_Hex(Encoding.UTF8.GetBytes("ede"), hex);
            string str2 = Encoding.UTF8.GetString(data2);

            System.Net.ServicePointManager.DefaultConnectionLimit = 1024;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
    }
}