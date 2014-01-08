using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QC2DXLuaScriptPacker
{
    public partial class Form1 : Form
    {
        private string quickRoot = "";//QUICK_COCOS2DX_ROOT系统变量路径
        public Form1()
        { 
                InitializeComponent(); 
                ReadData();//从config读取配置
                quickRoot = Environment.GetEnvironmentVariable("QUICK_COCOS2DX_ROOT");
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //获得lua文件列表
            FindFile(textBox1.Text, "*.lua"); 
        }

        private string tempPath = @"script";
        private SortedList<string, string> paramList = new SortedList<string, string>();
        private void ReadData()
        {

            paramList.Clear();
            if (File.Exists("config.cfg"))
            {
                FileStream fs;

                fs = new FileStream("config.cfg", FileMode.Open, FileAccess.ReadWrite);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);


                ArrayList list = new ArrayList();
                ArrayList pList = new ArrayList();
                pList.Add("scriptDirectory");//脚本目录
                pList.Add("packageOutputDir");//包目录
                pList.Add("outputPackageName");//报名
                pList.Add("key");//密钥
                pList.Add("sign");//签名
                pList.Add("isPakage");//是否打包
                pList.Add("customArgs");//自定义参数

                string temp = sr.ReadLine();
                while (temp != null)
                {
                    list.Add(temp);
                    temp = sr.ReadLine();
                }

                foreach (string str in list)
                    foreach (string s in pList)
                        parseParams(str, s);

                try//将各个配置装在起来
                {

                    textBox1.Text = paramList["scriptDirectory"] as string;
                    textBox2.Text = paramList["packageOutputDir"] as string;
                    textBox4.Text = paramList["outputPackageName"] as string;
                    textBox5.Text = paramList["key"] as string;
                    textBox6.Text = paramList["sign"] as string;
                    if (paramList["isPakage"] as string == "True")
                        checkBox1.Checked = true;
                    textBox3.Text = paramList["customArgs"] as string;
                }
                catch (Exception ex)
                {

                }



                sr.Close();
                fs.Close();
            }




        }

        private void parseParams(string src, string param)//读取单个配置
        {
            if (src.Contains(param))
            {
                string s = src.Replace(param, "");
                s = s.Replace("=", "");
                paramList.Add(param, s);
            }
        }

        private void FindFile(string FoldPath, string filter)//要查找的文件夹和文件类型
        {
            if (!Directory.Exists(FoldPath))//判断是否存在
            {
                MessageBox.Show("目录" + FoldPath + "不存在!");
                return;
            }

            DirectoryInfo thefolder = new DirectoryInfo(FoldPath);
            foreach (DirectoryInfo nextfolder in thefolder.GetDirectories())
            {
                FindFile(nextfolder.FullName, filter);
            }
            foreach (FileInfo nextfile in thefolder.GetFiles(filter))
            {

                ListViewItem item = this.listView1.Items.Add(nextfile.Name);
                item.SubItems.Add("" + nextfile.LastWriteTime);
                item.SubItems.Add("" + nextfile.FullName); 
 
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = getPath(); 
        }

        private string getPath()//获得路径
        {
            FolderBrowserDialog fld = new FolderBrowserDialog();
            fld.ShowDialog();
            string aa = fld.SelectedPath;
            if (aa != "")
                return aa;
            else return "";
                
        }
        private void button3_Click(object sender, EventArgs e)
        { 
                textBox2.Text = getPath(); 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public static string GetCMDOutString(string path, string arguments)//运行命令行
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = path;
            psi.UseShellExecute = false;
            psi.Arguments = arguments;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true; 
            String s = "";
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            while (p.WaitForExit(0) == false)
            {
                s += p.StandardOutput.ReadLine() + "\r\n";
            }

            return s;

        }

        private ListViewColumnSorter lvwColumnSorter;
        private bool sort = false;
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)//列表排序  有待完善
        {
            
            lvwColumnSorter = new ListViewColumnSorter();


            if (sort == false)
            {
                sort = true;
                lvwColumnSorter.Order = SortOrder.Descending;
            }
            else
            {
                sort = false;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            lvwColumnSorter.SortColumn = e.Column;
            this.listView1.ListViewItemSorter = lvwColumnSorter;
 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
         
        private void writeSonfigToFile()//保存配置文件
        {
            paramList.Clear();
            paramList.Add("scriptDirectory", textBox1.Text);
            paramList.Add("packageOutputDir", textBox2.Text);
            paramList.Add("outputPackageName", textBox4.Text);
            paramList.Add("key", textBox5.Text);
            paramList.Add("sign", textBox6.Text);
            paramList.Add("isPakage", checkBox1.Checked.ToString());
            paramList.Add("customArgs", textBox3.Text);

            FileStream fs;

            fs = new FileStream("config.cfg", FileMode.Create, FileAccess.ReadWrite);//直接覆盖以前的.
            StreamWriter sr = new StreamWriter(fs);

            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            foreach (KeyValuePair<string, string> item in paramList)
            {

                sr.WriteLine(item.Key + "=" + item.Value);
            }
            sr.Close();
            fs.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeSonfigToFile();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            

            
           //如果有选中的内容
            if (listView1.FocusedItem != null && listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                writeSonfigToFile();
                ReadData();
                copySelectedFileToTempDir();

                string path = Application.StartupPath + @"\" + tempPath;//临时目录
                string arg = " -i \"" + path + "\" -o \""
                        + paramList["packageOutputDir"]
                        + @"\" + paramList["outputPackageName"] + "\"";//compile_scripts 要用到的参数.

                //还有问题 暂未实现
               /* if (paramList["isPakage"] != null && paramList["isPakage"].Length > 0 && paramList["key"] == "True")
                {
                    arg += " -m files";
                    arg = arg.Replace(@"\" + paramList["outputPackageName"], "");
                }*/

                if (paramList["key"] != null && paramList["key"].Length > 0)//密钥 若没有则不使用xxtea_zip加密
                {
                    arg += " -e xxtea_zip -ek " + paramList["key"];
                }

                if (paramList["sign"] != null && paramList["sign"].Length > 0)//加密签名 默认为XXTEA
                {
                    arg += " -es " + paramList["sign"];
                }

                if (paramList["customArgs"] != null && paramList["customArgs"].Length > 0)//自定义参数. 若有自定义参数,则覆盖.
                {
                    arg = paramList["customArgs"];
                }
                
                MessageBox.Show("arg:" + arg+"\n"+GetCMDOutString(quickRoot + @"\bin\compile_scripts.bat", arg));//开始执行并且返回结果
            }
            else
            {
                MessageBox.Show("请先选择文件.");
            }
     
           
        }

        //将选中的文件按原有的目录结构拷贝到临时目录
        private void copySelectedFileToTempDir()
        {
            Directory.CreateDirectory(tempPath);
            deleteTmpFiles(tempPath);//CLEAR EVERYTHING

        

            if (listView1.FocusedItem != null)
                if (listView1.SelectedItems != null)
                {
                    string output = "";
                    string scrDir = paramList["scriptDirectory"];
                    string curDir = scrDir.Substring(scrDir.LastIndexOf(@"\"));
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        string temp = item.SubItems[2].Text.Substring(scrDir.Length);
                        string temp2 = temp.Substring(0, temp.Length - item.SubItems[0].Text.Length);
                        Directory.CreateDirectory(tempPath + temp2);
                        string destPath = tempPath + temp2 + item.SubItems[0].Text;
                        output += destPath + "\n";
                        try
                        {
                            File.Copy(item.SubItems[2].Text, destPath, true);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "出错!");
                        }
                    }

                }


             
        }

        /// <summary>  
        /// 解决删除目录提示：System.IO.IOException: 目录不是空的。  
        /// 删除一个目录，先遍历删除其下所有文件和目录（递归）  
        /// </summary>  
        /// <param name="strPath">绝对路径</param>  
        /// <returns>是否已经删除</returns>  
        public static bool DeleteADirectory(string strPath)
        {
            string[] strTemp;
            try
            {
                //先删除该目录下的文件  
                strTemp = System.IO.Directory.GetFiles(strPath);
                foreach (string str in strTemp)
                {
                    System.IO.File.Delete(str);
                }
                //删除子目录，递归  
                strTemp = System.IO.Directory.GetDirectories(strPath);
                foreach (string str in strTemp)
                {
                    DeleteADirectory(str);
                }
                //删除该目录  
                System.IO.Directory.Delete(strPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } 
         
        
         
        //删除这个目录下的所有文件及文件夹
        private void deleteTmpFiles(string strPath)
        {
            //删除这个目录下的所有子目录
            if (Directory.GetDirectories(strPath).Length > 0)
            {
                foreach (string var in Directory.GetDirectories(strPath))
                {
                    //DeleteDirectory(var);
                    DeleteADirectory(var);
                    //DeleteDirectory(var);
                }
            }
            //删除这个目录下的所有文件
            if (Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string var in Directory.GetFiles(strPath))
                {
                    File.Delete(var);
                }
            }
        }
      
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Quick-cocos2d-x Lua脚本加密打包器 Ver 0.1\n作者:Omicron_NEGA\nQQ:1262576802\nThe software and source code \nis published under WTFPL.");
        }

    }
}
