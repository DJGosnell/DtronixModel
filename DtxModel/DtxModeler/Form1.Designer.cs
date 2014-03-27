namespace DtxModeler {
	partial class Form1 {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.tableItem1 = new DtxModeler.Forms.Controls.TableItem();
			this.SuspendLayout();
			// 
			// tableItem1
			// 
			this.tableItem1.Location = new System.Drawing.Point(270, 97);
			this.tableItem1.Name = "tableItem1";
			this.tableItem1.Size = new System.Drawing.Size(252, 304);
			this.tableItem1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(706, 551);
			this.Controls.Add(this.tableItem1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private Forms.Controls.TableItem tableItem1;

	}
}

