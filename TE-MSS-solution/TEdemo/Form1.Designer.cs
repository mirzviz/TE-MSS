namespace TEdemo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axTE3DWindow1 = new AxTerraExplorerX.AxTE3DWindow();
            this.buttonAddToMap = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axTE3DWindow1)).BeginInit();
            this.SuspendLayout();
            // 
            // axTE3DWindow1
            // 
            this.axTE3DWindow1.Enabled = true;
            this.axTE3DWindow1.Location = new System.Drawing.Point(12, 12);
            this.axTE3DWindow1.Name = "axTE3DWindow1";
            this.axTE3DWindow1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTE3DWindow1.OcxState")));
            this.axTE3DWindow1.Size = new System.Drawing.Size(483, 426);
            this.axTE3DWindow1.TabIndex = 0;
            // 
            // buttonAddToMap
            // 
            this.buttonAddToMap.Location = new System.Drawing.Point(539, 29);
            this.buttonAddToMap.Name = "buttonAddToMap";
            this.buttonAddToMap.Size = new System.Drawing.Size(151, 33);
            this.buttonAddToMap.TabIndex = 1;
            this.buttonAddToMap.Text = "add to map";
            this.buttonAddToMap.UseVisualStyleBackColor = true;
            this.buttonAddToMap.Click += new System.EventHandler(this.buttonAddToMap_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonAddToMap);
            this.Controls.Add(this.axTE3DWindow1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axTE3DWindow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxTerraExplorerX.AxTE3DWindow axTE3DWindow1;
        private System.Windows.Forms.Button buttonAddToMap;
    }
}

