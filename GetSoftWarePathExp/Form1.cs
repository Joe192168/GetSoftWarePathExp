using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace GetSoftWarePathExp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string softName = textBox1.Text.ToString();
                RegistryKey regKey = Registry.CurrentUser;
                RegistryKey regSubKey = regKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\UFH", false);

                if (regSubKey != null)
                {
                    //遍历子项名称的字符串数组
                    foreach (string keyName in regSubKey.GetSubKeyNames())
                    {
                        //遍历子项节点
                        using (RegistryKey key2 = regSubKey.OpenSubKey(keyName, false))
                        {
                            if (key2 != null)
                            {
                                int[] axm = new int[3];
                                string[] maxvalue = key2.GetValueNames();
                                //可能存在多个遗留的安装路径，获取最后的安装路径即可
                                Dictionary<int, string> dic = new Dictionary<int, string>();
                                foreach (string valuename in key2.GetValueNames())
                                {
                                    string[] softwareName = (string[])key2.GetValue(valuename, "");
                                    if (softwareName[1].Contains(softName))
                                    {
                                        dic.Add(int.Parse(valuename), softwareName[1]);
                                    }

                                }
                                this.textBox2.AppendText(dic.Values.Max() + "\r\n");
                            }
                        }
                    }
                }
            }
            catch
            {
                this.label3.Text = "朋友，获取程序路径失败！";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Clear();
        }
    }
}
