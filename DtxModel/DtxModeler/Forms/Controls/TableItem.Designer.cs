namespace DtxModeler.Forms.Controls {
	partial class TableItem {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1", -2, -2);
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("TableName", new System.Windows.Forms.TreeNode[] {
            treeNode1});
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableItem));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeView1.ImageIndex = 0;
			this.treeView1.ImageList = this.imageList1;
			this.treeView1.Indent = 12;
			this.treeView1.LabelEdit = true;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			treeNode1.ImageIndex = -2;
			treeNode1.Name = "Node1";
			treeNode1.SelectedImageIndex = -2;
			treeNode1.Text = "Node1";
			treeNode2.Name = "TableName";
			treeNode2.Text = "TableName";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
			this.treeView1.PathSeparator = "/";
			this.treeView1.Scrollable = false;
			this.treeView1.SelectedImageIndex = 0;
			this.treeView1.ShowLines = false;
			this.treeView1.Size = new System.Drawing.Size(252, 304);
			this.treeView1.TabIndex = 0;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "database_table.png");
			this.imageList1.Images.SetKeyName(1, "key.png");
			this.imageList1.Images.SetKeyName(2, "index-cards.png");
			// 
			// TableItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.treeView1);
			this.Name = "TableItem";
			this.Size = new System.Drawing.Size(252, 304);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ImageList imageList1;
	}
}
