namespace PipeServer
{
    partial class MainFormServer
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
            this._consoleRichTextBox = new System.Windows.Forms.RichTextBox();
            this._startBtn = new System.Windows.Forms.Button();
            this._stopBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _consoleRichTextBox
            // 
            this._consoleRichTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this._consoleRichTextBox.Location = new System.Drawing.Point(22, 27);
            this._consoleRichTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._consoleRichTextBox.Name = "_consoleRichTextBox";
            this._consoleRichTextBox.Size = new System.Drawing.Size(323, 317);
            this._consoleRichTextBox.TabIndex = 0;
            this._consoleRichTextBox.Text = "";
            // 
            // _startBtn
            // 
            this._startBtn.Location = new System.Drawing.Point(96, 362);
            this._startBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._startBtn.Name = "_startBtn";
            this._startBtn.Size = new System.Drawing.Size(56, 19);
            this._startBtn.TabIndex = 1;
            this._startBtn.Text = "Start";
            this._startBtn.UseVisualStyleBackColor = true;
            this._startBtn.Click += new System.EventHandler(this._startBtn_Click);
            // 
            // _stopBtn
            // 
            this._stopBtn.Location = new System.Drawing.Point(182, 362);
            this._stopBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._stopBtn.Name = "_stopBtn";
            this._stopBtn.Size = new System.Drawing.Size(56, 19);
            this._stopBtn.TabIndex = 2;
            this._stopBtn.Text = "Stop";
            this._stopBtn.UseVisualStyleBackColor = true;
            this._stopBtn.Click += new System.EventHandler(this._stopBtn_Click);
            // 
            // MainFormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 407);
            this.Controls.Add(this._stopBtn);
            this.Controls.Add(this._startBtn);
            this.Controls.Add(this._consoleRichTextBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainFormServer";
            this.Text = "MainFormServer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _consoleRichTextBox;
        private System.Windows.Forms.Button _startBtn;
        private System.Windows.Forms.Button _stopBtn;
    }
}