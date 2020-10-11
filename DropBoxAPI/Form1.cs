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
using CefSharp.WinForms;
using System.IO;

namespace DropBoxAPI
{
    public partial class Form1 : Form
    {
        string oauth2State;
        string RedirectUri = "http://127.0.0.1:52475/authorize";
        DropboxClient client;
        ListFolderResult content;
        string currentPath = "";
        string lastPath = "";
        KeyValuePair<string, string> buffer;
        bool copy;
        

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
            w.Load(DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, "hzw1k2rtk9p5qcj", RedirectUri, state: oauth2State).ToString());
            w.LoadError += W_LoadError;
        }

        private void W_LoadError(object sender, CefSharp.LoadErrorEventArgs e)
        {
            if (e.FailedUrl.StartsWith(RedirectUri, StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.FailedUrl));
                    if (result.State == oauth2State)
                    {
                        w.Invoke((MethodInvoker)(() => w.Hide()));
                        Properties.Settings.Default.AccessToken = result.AccessToken;
                        Properties.Settings.Default.Save();
                        Invoke((MethodInvoker)(() => InitClient(result.AccessToken)));
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
            ShowContent();
        }

        private void ShowContent(string path = "")
        {
            label_loading.Show();
            listView1.Clear();
            currentPath = path;
            menuTextBox.Text = currentPath;
            content = client.Files.ListFolderAsync(path).Result;
            foreach (var item in content.Entries)
            {
                // If folder - item.SubItems[1].Text (size) == ""
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.AsFile?.Size.ToString() }, item.IsFolder ? 0 : 1));
            }
            statusStrip1.Items["itemsCount"].Text = listView1.Items.Count.ToString();
            label_loading.Hide();
        }

        private void ShowContent(IList<SearchMatchV2> content)
        {
            label_loading.Show();
            listView1.Clear();
            foreach (var item in content)
            {
                var meta = item.Metadata.AsMetadata.Value;
                listView1.Items.Add(new ListViewItem(new string[] { meta.Name, meta.AsFile?.Size.ToString() }, meta.IsFolder ? 0 : 1));
            }
            statusStrip1.Items["itemsCount"].Text = listView1.Items.Count.ToString();
            label_loading.Hide();
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

        private void menuButton_l_Click(object sender, EventArgs e)
        {
            lastPath = currentPath;
            ShowContent(string.Concat(currentPath.Take(currentPath.LastIndexOf('/'))));
        }

        private void menuButton_r_Click(object sender, EventArgs e)
        {
            ShowContent(lastPath);
        }

        private void menuButton_s_Click(object sender, EventArgs e)
        {
            if(menuTextBox.Text != "")
            {
                var searchRes = client.Files.SearchV2Async(menuTextBox.Text).Result;
                ShowContent(searchRes.Matches);
            }
        }

        private void menuButton_refresh_Click(object sender, EventArgs e)
        {
            ShowContent(currentPath);
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip.Items.Clear();
            switch (listView1.SelectedItems.Count)
            {
                case 0:
                    {
                        if(buffer.Key != null)
                        {
                            var paste = new ToolStripButton("Вставить");
                            paste.Click += Paste_Click;
                            contextMenuStrip.Items.Add(paste);
                        }
                        var createFolder = new ToolStripButton("Создать папку");
                        createFolder.Click += CreateFolder_Click;
                        var uploadFile = new ToolStripButton("Загрузить файл");
                        uploadFile.Click += UploadFile_Click;
                        contextMenuStrip.Items.AddRange(new[] { createFolder, uploadFile });
                    } break;
                case 1:
                    {
                        var cut = new ToolStripButton("Вырезать");
                        cut.Click += Cut_Click;
                        var copy = new ToolStripButton("Копировать");
                        copy.Click += Copy_Click;
                        var delete = new ToolStripButton("Удалить");
                        delete.Click += Delete_Click;
                        var info = new ToolStripButton("Свойства");
                        info.Click += Info_Click;
                        var share = new ToolStripButton("Поделиться");
                        share.Click += Share_Click;
                        var rename = new ToolStripButton("Переименовать");
                        rename.Click += Rename_Click;
                        var download = new ToolStripButton("Скачать");
                        download.Click += Download_Click;
                        contextMenuStrip.Items.AddRange(new[] { share, download, cut, copy, delete, rename, info });
                    } break;
            }
            contextMenuStrip.Width++; // small width fix
        }

        private void Rename_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].BeginEdit();
        }

        private void Share_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Info_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            buffer = new KeyValuePair<string, string>(currentPath, listView1.SelectedItems[0].Text);
            copy = true;
        }

        private void Download_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];
            var saveDialog = new SaveFileDialog();
            saveDialog.FileName = item.Text;
            if (!item.Text.Contains('.')) saveDialog.FileName += ".zip";
            if(saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (item.SubItems[1].Text == "")
                {
                    var res = client.Files.DownloadZipAsync(currentPath + "/" + item.Text).Result;
                    File.WriteAllBytes(saveDialog.FileName, res.GetContentAsByteArrayAsync().Result);
                }
                else
                {
                    var res = client.Files.DownloadAsync(currentPath + "/" + item.Text).Result;
                    File.WriteAllBytes(saveDialog.FileName, res.GetContentAsByteArrayAsync().Result);
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];
            client.Files.DeleteV2Async(currentPath + "/" + item.Text);
            listView1.Items.Remove(item);
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];
            buffer = new KeyValuePair<string, string>(currentPath, item.Text);
            copy = false;
            item.ImageIndex += 2;
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (copy) client.Files.CopyV2Async(buffer.Key + "/" + buffer.Value, currentPath + "/" + buffer.Value);
            else client.Files.MoveV2Async(buffer.Key + "/" + buffer.Value, currentPath + "/" + buffer.Value);
            buffer = new KeyValuePair<string, string>();
        }

        private void UploadFile_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            if(openDialog.ShowDialog() == DialogResult.OK)
            {
                client.Files.UploadAsync(currentPath + "/" + openDialog.SafeFileName, WriteMode.Overwrite.Instance, body: File.OpenRead(openDialog.FileName));
            }
        }

        private void CreateFolder_Click(object sender, EventArgs e)
        {
            var res = client.Files.CreateFolderV2Async(currentPath + "/New folder", true).Result;
            var item = listView1.Items.Add(new ListViewItem(new string[] { res.Metadata.Name, "" }, 0));
            item.BeginEdit();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0];

                // folder
                if(item.SubItems[1].Text == "")
                {
                    ShowContent(currentPath + "/" + item.Text);
                }
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                client.Files.MoveV2Async(currentPath + "/" + listView1.Items[e.Item].Text, currentPath + "/" + e.Label);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                e.CancelEdit = true;
            }
        }
    }
}
