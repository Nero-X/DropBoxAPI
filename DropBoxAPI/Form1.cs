using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace DropBoxAPI
{
    public partial class Form1 : Form
    {
        string oauth2State;
        string RedirectUri = "http://127.0.0.1:52475/authorize";
        DropboxClient client;
        ListFolderResult content;
        string Path = "";

        enum Units { байт, КБ, МБ, ГБ}

        public Form1()
        {
            InitializeComponent();
            try
            {
                InitClient(Properties.Settings.Default.AccessToken);
            }
            catch
            {
                GetAccessToken();
            }
        }

        void GetAccessToken()
        {
            oauth2State = Guid.NewGuid().ToString("N");
            w.Show();
            w.Navigate(DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, "hzw1k2rtk9p5qcj", RedirectUri, state: oauth2State));
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().StartsWith(RedirectUri, StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(e.Url);
                    if (result.State == oauth2State)
                    {
                        w.Hide();
                        Properties.Settings.Default.AccessToken = result.AccessToken;
                        InitClient(result.AccessToken);
                    }
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("There was an error");
                }
            }
        }

        void InitClient(string accessToken)
        {
            client = new DropboxClient(accessToken);
            var info = client.Users.GetCurrentAccountAsync().Result;
            label_loading.Show();

            var spaceUsage = new ToolStripProgressBar();
            var max = client.Users.GetSpaceUsageAsync().Result.Allocation.AsIndividual.Value.Allocated;
            var used = client.Users.GetSpaceUsageAsync().Result.Used;
            statusStrip1.Items.Add("Использовано " + SizeToStr(used) + " / " + SizeToStr(max));
            while (max > 10 && used > 10)
            {
                max /= 10;
                used /= 10;
            }
            spaceUsage.Maximum = (int)Math.Max(max, used);
            spaceUsage.Value = (int)used;
            statusStrip1.Items.Add(spaceUsage);
            statusStrip1.Items.Add(new ToolStripSeparator());
            statusStrip1.Items.Add(info.Name.DisplayName); // user name
            using (WebClient web = new WebClient())
                statusStrip1.Items.Add(new Bitmap(web.OpenRead(info.ProfilePhotoUrl))); // user icon
            statusStrip1.Items.Add(new ToolStripSeparator());
            for (int i = 3; i < 9; i++)
            {
                statusStrip1.Items[i].Alignment = ToolStripItemAlignment.Right;
            }
            statusStrip1.Items[6].Click += ShowInfo;
            statusStrip1.Items[7].Click += ShowInfo;

            // Show root
            ShowFolderContent(Path);

            label_loading.Hide();
        }

        private void ShowFolderContent(string path)
        {
            listView1.Clear();
            Path = path;
            menuTextBox.Text = Path;
            content = client.Files.ListFolderAsync(path).Result;
            foreach (var item in content.Entries)
            {
                // If folder - item.SubItems[1].Text (size) == ""
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.AsFile?.Size.ToString() }, item.IsFolder ? 0 : 1));
            }
            statusStrip1.Items["itemsCount"].Text = listView1.Items.Count.ToString();
        }

        void ShowInfo(object sender, EventArgs e)
        {
            var full = client.Users.GetCurrentAccountAsync().Result;
            StringBuilder str = new StringBuilder();
            str.AppendFormat("Account id    : {0}\n", full.AccountId);
            str.AppendFormat("Country       : {0}\n", full.Country);
            str.AppendFormat("Email         : {0}\n", full.Email);
            str.AppendFormat("Is paired     : {0}\n", full.IsPaired ? "Yes" : "No");
            str.AppendFormat("Locale        : {0}\n", full.Locale);
            str.AppendFormat("Display Name  : {0}\n", full.Name.DisplayName);
            str.AppendFormat("Referral link : {0}\n", full.ReferralLink);
            MessageBox.Show(str.ToString(), "Информация о пользователе");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listView1.SelectedItems.Count)
            {
                case 0:
                    statusStrip1.Items["selected"].Text = ""; break;
                case 1:
                    {
                        ulong size = Convert.ToUInt64("0" + listView1.SelectedItems[0].SubItems[1].Text);
                        statusStrip1.Items["selected"].Text = "Выбран 1 элемент" + (size > 0 ? ": " + SizeToStr(size) : ""); break;
                    }
                default:
                    {
                        ulong size = 0;
                        foreach(ListViewItem item in listView1.SelectedItems)
                        {
                            if (item.SubItems[1].Text == "")
                            {
                                size = 0; 
                                break;
                            }
                            size += Convert.ToUInt64(item.SubItems[1].Text);
                        }
                        statusStrip1.Items["selected"].Text = "Выбрано " + listView1.SelectedItems.Count + " элем." + (size > 0 ? ": " + SizeToStr(size) : ""); break;
                    }
            }
            
        }

        string SizeToStr(ulong sizeInBytes)
        {
            Units i = 0;
            while(sizeInBytes > 1000)
            {
                sizeInBytes /= 1000;
                i++;
            }
            return sizeInBytes.ToString() + " " + i;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void menuButton_l_Click(object sender, EventArgs e)
        {

        }

        private void menuButton_r_Click(object sender, EventArgs e)
        {

        }

        private void menuButton_s_Click(object sender, EventArgs e)
        {

        }

        private void menuButton_refresh_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            switch (listView1.SelectedItems.Count)
            {
                case 0:
                    {
                        var createFolder = new ToolStripButton("Создать папку");
                        createFolder.Click += CreateFolder_Click;
                        var uploadFile = new ToolStripButton("Загрузить файл");
                        uploadFile.Click += UploadFile_Click;
                        contextMenuStrip.Items.AddRange(new[] { createFolder, uploadFile });
                    } break;
                case 1:
                    {
                        /*var createFolder = new ToolStripButton("Создать папку");
                        createFolder.Click += CreateFolder_Click;
                        var uploadFile = new ToolStripButton("Загрузить файл");
                        uploadFile.Click += UploadFile_Click;
                        contextMenuStrip.Items.AddRange(new[] { createFolder, uploadFile });*/
                    } break;
                default:
                    break;
            }
        }

        private void UploadFile_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreateFolder_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0];

                // folder
                if(item.SubItems[1].Text == "")
                {
                    ShowFolderContent(Path + "/" + item.Text);
                }
            }
        }
    }
}
