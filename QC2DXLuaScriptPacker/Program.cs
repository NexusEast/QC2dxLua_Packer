using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QC2DXLuaScriptPacker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.GetEnvironmentVariable("QUICK_COCOS2DX_ROOT") == "" || Environment.GetEnvironmentVariable("QUICK_COCOS2DX_ROOT") == null)
            {
                MessageBox.Show("QUICK_COCOS2DX_ROOT环境变量未设置!");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
