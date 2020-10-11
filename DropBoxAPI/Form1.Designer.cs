namespace DropBoxAPI
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.itemsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.selected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuButton_l = new System.Windows.Forms.ToolStripButton();
            this.menuButton_r = new System.Windows.Forms.ToolStripButton();
            this.menuButton_s = new System.Windows.Forms.ToolStripButton();
            this.menuTextBox = new ToolStripSpringTextBox();
            this.menuButton_refresh = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label_loading = new System.Windows.Forms.Label();
            this.w = new CefSharp.WinForms.ChromiumWebBrowser();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.itemsCount,
            this.selected});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 590);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(942, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabel1.Text = "Элементов:";
            // 
            // itemsCount
            // 
            this.itemsCount.Name = "itemsCount";
            this.itemsCount.Size = new System.Drawing.Size(0, 17);
            // 
            // selected
            // 
            this.selected.Name = "selected";
            this.selected.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuButton_l,
            this.menuButton_r,
            this.menuButton_s,
            this.menuTextBox,
            this.menuButton_refresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(942, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuButton_l
            // 
            this.menuButton_l.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuButton_l.Image = global::DropBoxAPI.Properties.Resources.left;
            this.menuButton_l.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuButton_l.Name = "menuButton_l";
            this.menuButton_l.Size = new System.Drawing.Size(23, 22);
            this.menuButton_l.Text = "toolStripButton1";
            this.menuButton_l.Click += new System.EventHandler(this.menuButton_l_Click);
            // 
            // menuButton_r
            // 
            this.menuButton_r.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuButton_r.Image = global::DropBoxAPI.Properties.Resources.right;
            this.menuButton_r.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuButton_r.Name = "menuButton_r";
            this.menuButton_r.Size = new System.Drawing.Size(23, 22);
            this.menuButton_r.Text = "toolStripButton2";
            this.menuButton_r.Click += new System.EventHandler(this.menuButton_r_Click);
            // 
            // menuButton_s
            // 
            this.menuButton_s.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuButton_s.Image = global::DropBoxAPI.Properties.Resources.search;
            this.menuButton_s.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuButton_s.Name = "menuButton_s";
            this.menuButton_s.Size = new System.Drawing.Size(23, 22);
            this.menuButton_s.Text = "toolStripButton3";
            this.menuButton_s.Click += new System.EventHandler(this.menuButton_s_Click);
            // 
            // menuTextBox
            // 
            this.menuTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuTextBox.Name = "menuTextBox";
            this.menuTextBox.Size = new System.Drawing.Size(816, 25);
            // 
            // menuButton_refresh
            // 
            this.menuButton_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuButton_refresh.Image = global::DropBoxAPI.Properties.Resources.refresh;
            this.menuButton_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuButton_refresh.Name = "menuButton_refresh";
            this.menuButton_refresh.Size = new System.Drawing.Size(23, 22);
            this.menuButton_refresh.Text = "toolStripButton4";
            this.menuButton_refresh.Click += new System.EventHandler(this.menuButton_refresh_Click);
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.LabelEdit = true;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(942, 565);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView1_AfterLabelEdit);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.ShowImageMargin = false;
            this.contextMenuStrip.ShowItemToolTips = false;
            this.contextMenuStrip.Size = new System.Drawing.Size(36, 4);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "notepad.png");
            this.imageList1.Images.SetKeyName(2, "folder_cut.png");
            this.imageList1.Images.SetKeyName(3, "notepad_cut.png");
            // 
            // label_loading
            // 
            this.label_loading.AutoSize = true;
            this.label_loading.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_loading.Location = new System.Drawing.Point(354, 279);
            this.label_loading.Name = "label_loading";
            this.label_loading.Size = new System.Drawing.Size(235, 55);
            this.label_loading.TabIndex = 5;
            this.label_loading.Text = "Loading...";
            this.label_loading.Visible = false;
            // 
            // w
            // 
            this.w.ActivateBrowserOnCreation = false;
            this.w.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w.Location = new System.Drawing.Point(0, 25);
            this.w.Name = "w";
            this.w.Size = new System.Drawing.Size(942, 565);
            this.w.TabIndex = 6;
            this.w.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 612);
            this.Controls.Add(this.w);
            this.Controls.Add(this.label_loading);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Dropbox";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel itemsCount;
        private System.Windows.Forms.ToolStripStatusLabel selected;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton menuButton_l;
        private System.Windows.Forms.ToolStripButton menuButton_r;
        private System.Windows.Forms.ToolStripButton menuButton_s;
        private ToolStripSpringTextBox menuTextBox;
        private System.Windows.Forms.ToolStripButton menuButton_refresh;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label_loading;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private CefSharp.WinForms.ChromiumWebBrowser w;
    }
}

