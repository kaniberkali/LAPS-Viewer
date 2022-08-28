using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAPS_Viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        List<LAPSList> directoryList = new List<LAPSList>();
        List<LAPSTxtList> filesList = new List<LAPSTxtList>();

        private void getAllDirectoriesAndFiles()
        {
            try
            {
                clearItems();
                clearGroups();
                directoryList.Clear();
                filesList.Clear();
                listBox1.Items.Clear();
                string[] directories = System.IO.Directory.GetDirectories(Application.StartupPath, "*", System.IO.SearchOption.AllDirectories);
                foreach (string directory in directories)
                {
                    try
                    {
                        string wifiName = directory.Split('\\').Last().Split(' ')[0];
                        string creationDate = directory.Split('\\').Last().Split('-').Last().Trim();
                        string[] files = Directory.GetFiles(directory, "*.txt");
                        List<string> ips = new List<string>();
                        foreach (string file in files)
                        {
                            List<int> ports = new List<int>();
                            LAPSTxtList itemTxt = new LAPSTxtList();
                            try
                            {
                                ips.Add(file.Split('\\').Last().Replace(".txt", ""));
                                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                                StreamReader sw = new StreamReader(fs);
                                string yazi = sw.ReadLine();
                                while (yazi != null)
                                {
                                    try
                                    {
                                        string query = yazi.Split(' ')[0];
                                        string result = yazi.Split(' ')[1];
                                        if (query == "Connection:")
                                            itemTxt.connection = Convert.ToBoolean(result);
                                        if (query == "Index:")
                                            itemTxt.index = Convert.ToBoolean(result);
                                        if (query == "PortsCount:")
                                            itemTxt.portsCount = Convert.ToInt32(result);
                                        if (query == "TimeToFindAddress:")
                                            itemTxt.timeToFindAddress = Convert.ToDouble(result);
                                        if (query == "TotalTime:")
                                            itemTxt.totalTime = Convert.ToDouble(result);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            int port = Convert.ToInt32(yazi);
                                            if (port >= 0 && port <= 65535)
                                                ports.Add(port);
                                        }
                                        catch { }
                                    }
                                    yazi = sw.ReadLine();
                                }
                                sw.Close();
                                fs.Close();
                            }
                            catch { }
                            itemTxt.ports = ports;
                            filesList.Add(itemTxt);
                        }
                        LAPSList item = new LAPSList();
                        item.ips = ips;
                        item.wifiName = wifiName;
                        item.creationDate = creationDate;
                        directoryList.Add(item);
                    }
                    catch
                    {

                    }
                }
                foreach (LAPSList item in directoryList)
                    listBox1.Items.Add(item.wifiName);
                listBox1.SelectedIndex = 0;
            }
            catch { }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("LAPS Viewer").Length > 1)
            {
                MessageBox.Show(new Form() { TopMost = true },"The laps viewer application can only be run once at a time.","@kodzamani.tk");
                Application.Exit();
            }
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                getAllDirectoriesAndFiles();
                args[1] = args[1].Replace('|', ' ');
                if (args.Length == 2)
                {
                    string wifiName = args[1].Split(' ')[0];
                    string creationDate = args[1].Split('-').Last().Trim();
                    int counter = 0;
                    foreach (LAPSList item in directoryList)
                    {
                        if (wifiName == item.wifiName && creationDate == item.creationDate)
                        {
                            listBox1.SelectedIndex = counter;
                            break;
                        }
                        counter++;
                    }
                }
            }
            catch { }
            if (listBox1.Items.Count <= 0)
            {
                try { Process.Start("Lan Address - Port Scanner.exe"); } catch { }
                MessageBox.Show(new Form() { TopMost = true },"Port and ip scan not found please do a scan before running this program", "@kodzamani.tk");
                Application.Exit();
            }
        }
        private void clearItems()
        {
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            richTextBox1.Text = "";
            label43.ForeColor = Color.Red;
            label42.ForeColor = Color.Red;
            label41.ForeColor = Color.Red;
            label40.ForeColor = Color.Red;
            label39.ForeColor = Color.Red;
            label38.ForeColor = Color.Red;
            label43.Text = "False";
            label42.Text = "False";
            label41.Text = "0";
            label40.Text = "0";
            label39.Text = "0";
            label38.Text = "00.00.0000 00.00.00";
            groupBox1.Text = "Informatin";
        }
        private void clearGroups()
        {
            listBox3.Items.Clear();
            richTextBox1.Text = "";
            label43.ForeColor = Color.Red;
            label42.ForeColor = Color.Red;
            label41.ForeColor = Color.Red;
            label40.ForeColor = Color.Red;
            label39.ForeColor = Color.Red;
            label38.ForeColor = Color.Red;
            label43.Text = "False";
            label42.Text = "False";
            label41.Text = "0";
            label40.Text = "0";
            label39.Text = "0";
            groupBox1.Text = "Informatin";
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearItems();
            try
            {
                string wifiName = directoryList[listBox1.SelectedIndex].wifiName;
                string creationDate = directoryList[listBox1.SelectedIndex].creationDate;
                List<string> ips = directoryList[listBox1.SelectedIndex].ips;
                groupBox1.Text = wifiName;
                label38.Text = creationDate;
                label38.ForeColor = Color.LightGreen;
                for (int i = 0; i < ips.Count; i++)
                    listBox2.Items.Add(ips[i].ToString());
                groupBox1.Text = wifiName;
                listBox2.SelectedIndex = 0;
            }
            catch { }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearGroups();
                int id = listBox2.SelectedIndex;
                label43.Text = filesList[id].connection.ToString();
                label42.Text = filesList[id].index.ToString();
                label41.Text = filesList[id].portsCount.ToString();
                label40.Text = filesList[id].timeToFindAddress.ToString();
                label39.Text = filesList[id].totalTime.ToString();
                groupBox1.Text = directoryList[listBox1.SelectedIndex].wifiName + " - " + directoryList[listBox1.SelectedIndex].ips[id];
                label43.ForeColor = Color.LightGreen;
                label42.ForeColor = Color.LightGreen;
                label41.ForeColor = Color.LightGreen;
                label40.ForeColor = Color.LightGreen;
                label39.ForeColor = Color.LightGreen;
                label38.ForeColor = Color.LightGreen;
                for (int i = 0; i < filesList[id].ports.Count; i++)
                {
                    listBox3.Items.Add(filesList[id].ports[i].ToString());
                    richTextBox1.Text += filesList[id].ports[i].ToString();
                    if (i < filesList[id].ports.Count-1)
                        richTextBox1.Text += ", ";
                }
            }
            catch
            {

            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { Process.Start("http://" + listBox2.Text + ":" + listBox3.Text); } catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getAllDirectoriesAndFiles();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

    }
}
