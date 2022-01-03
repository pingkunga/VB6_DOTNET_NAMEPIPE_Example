namespace PipeClient
{
    partial class MainSimpleClient
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
            this._requestText = new System.Windows.Forms.TextBox();
            this._sendBtn = new System.Windows.Forms.Button();
            this._responseText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _requestText
            // 
            this._requestText.Location = new System.Drawing.Point(165, 57);
            this._requestText.Name = "_requestText";
            this._requestText.Size = new System.Drawing.Size(307, 22);
            this._requestText.TabIndex = 0;
            // 
            // _sendBtn
            // 
            this._sendBtn.Location = new System.Drawing.Point(509, 57);
            this._sendBtn.Name = "_sendBtn";
            this._sendBtn.Size = new System.Drawing.Size(93, 22);
            this._sendBtn.TabIndex = 1;
            this._sendBtn.Text = "Send";
            this._sendBtn.UseVisualStyleBackColor = true;
            this._sendBtn.Click += new System.EventHandler(this._sendBtn_Click);
            // 
            // _responseText
            // 
            this._responseText.Location = new System.Drawing.Point(165, 105);
            this._responseText.Name = "_responseText";
            this._responseText.Size = new System.Drawing.Size(307, 22);
            this._responseText.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Response Message :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Request Message :";
            // 
            // MainFormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 210);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._responseText);
            this.Controls.Add(this._sendBtn);
            this.Controls.Add(this._requestText);
            this.Name = "MainFormClient";
            this.Text = "MainFormClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _requestText;
        private System.Windows.Forms.Button _sendBtn;
        private System.Windows.Forms.TextBox _responseText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}