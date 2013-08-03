namespace Game_V1._2
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
            this.button1 = new System.Windows.Forms.Button();
            this.serverIPtb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.serverPorttb = new System.Windows.Forms.TextBox();
            this.clientporttb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mapsizetb = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.clientIPtb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LimeGreen;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(55, 382);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(369, 71);
            this.button1.TabIndex = 0;
            this.button1.Text = "Join Game";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // serverIPtb
            // 
            this.serverIPtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverIPtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIPtb.Location = new System.Drawing.Point(166, 93);
            this.serverIPtb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.serverIPtb.Name = "serverIPtb";
            this.serverIPtb.Size = new System.Drawing.Size(258, 26);
            this.serverIPtb.TabIndex = 1;
            this.serverIPtb.Text = "169.254.129.189";
            this.serverIPtb.TextChanged += new System.EventHandler(this.serverIPtb_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 93);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server IP :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 206);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server Port :";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // serverPorttb
            // 
            this.serverPorttb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverPorttb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverPorttb.Location = new System.Drawing.Point(166, 204);
            this.serverPorttb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.serverPorttb.Name = "serverPorttb";
            this.serverPorttb.Size = new System.Drawing.Size(258, 26);
            this.serverPorttb.TabIndex = 3;
            this.serverPorttb.Text = "6000";
            this.serverPorttb.TextChanged += new System.EventHandler(this.serverPorttb_TextChanged);
            // 
            // clientporttb
            // 
            this.clientporttb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientporttb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientporttb.Location = new System.Drawing.Point(166, 256);
            this.clientporttb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.clientporttb.Name = "clientporttb";
            this.clientporttb.Size = new System.Drawing.Size(258, 26);
            this.clientporttb.TabIndex = 4;
            this.clientporttb.Text = "7000";
            this.clientporttb.TextChanged += new System.EventHandler(this.clientporttb_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(51, 258);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Client Port :";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // mapsizetb
            // 
            this.mapsizetb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapsizetb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapsizetb.Location = new System.Drawing.Point(166, 308);
            this.mapsizetb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mapsizetb.Name = "mapsizetb";
            this.mapsizetb.Size = new System.Drawing.Size(258, 26);
            this.mapsizetb.TabIndex = 5;
            this.mapsizetb.Text = "20";
            this.mapsizetb.TextChanged += new System.EventHandler(this.mapsizetb_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(51, 310);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Map Size : ";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // clientIPtb
            // 
            this.clientIPtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientIPtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientIPtb.Location = new System.Drawing.Point(166, 147);
            this.clientIPtb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.clientIPtb.Name = "clientIPtb";
            this.clientIPtb.Size = new System.Drawing.Size(258, 26);
            this.clientIPtb.TabIndex = 2;
            this.clientIPtb.Text = "169.254.13.169";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 152);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Client IP :";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(91, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(302, 55);
            this.label4.TabIndex = 16;
            this.label4.Text = "Battle Tanks";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(477, 466);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clientIPtb);
            this.Controls.Add(this.mapsizetb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.clientporttb);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.serverPorttb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverIPtb);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(489, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(489, 500);
            this.Name = "Form1";
            this.Text = "Battle Tanks";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox serverIPtb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPorttb;
        private System.Windows.Forms.TextBox clientporttb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mapsizetb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox clientIPtb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}