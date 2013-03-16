namespace SimCitySpeedBooster {
  partial class Main {
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this.speedNumeric = new System.Windows.Forms.NumericUpDown();
      this.statusLabel = new System.Windows.Forms.Label();
      this.searchTimer = new System.Windows.Forms.Timer(this.components);
      this.checkTimer = new System.Windows.Forms.Timer(this.components);
      this.optionsButton = new System.Windows.Forms.Button();
      this.usageTooltip = new System.Windows.Forms.ToolTip(this.components);
      this.linkLabel = new System.Windows.Forms.LinkLabel();
      ((System.ComponentModel.ISupportInitialize)(this.speedNumeric)).BeginInit();
      this.SuspendLayout();
      // 
      // speedNumeric
      // 
      this.speedNumeric.DecimalPlaces = 4;
      this.speedNumeric.Location = new System.Drawing.Point(12, 6);
      this.speedNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.speedNumeric.Name = "speedNumeric";
      this.speedNumeric.Size = new System.Drawing.Size(120, 20);
      this.speedNumeric.TabIndex = 0;
      this.speedNumeric.ThousandsSeparator = true;
      this.usageTooltip.SetToolTip(this.speedNumeric, "Current Speed Value");
      this.speedNumeric.ValueChanged += new System.EventHandler(this.SpeedNumericValueChanged);
      // 
      // statusLabel
      // 
      this.statusLabel.AutoSize = true;
      this.statusLabel.Location = new System.Drawing.Point(10, 11);
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new System.Drawing.Size(0, 13);
      this.statusLabel.TabIndex = 2;
      // 
      // searchTimer
      // 
      this.searchTimer.Interval = 1000;
      this.searchTimer.Tick += new System.EventHandler(this.SearchTimerTick);
      // 
      // checkTimer
      // 
      this.checkTimer.Interval = 1000;
      this.checkTimer.Tick += new System.EventHandler(this.CheckTimerTick);
      // 
      // optionsButton
      // 
      this.optionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.optionsButton.Location = new System.Drawing.Point(141, 6);
      this.optionsButton.Name = "optionsButton";
      this.optionsButton.Size = new System.Drawing.Size(52, 20);
      this.optionsButton.TabIndex = 3;
      this.optionsButton.Text = "Options";
      this.optionsButton.UseVisualStyleBackColor = true;
      this.optionsButton.Click += new System.EventHandler(this.OptionsButtonClick);
      // 
      // linkLabel
      // 
      this.linkLabel.AutoSize = true;
      this.linkLabel.Location = new System.Drawing.Point(12, 29);
      this.linkLabel.Name = "linkLabel";
      this.linkLabel.Size = new System.Drawing.Size(171, 13);
      this.linkLabel.TabIndex = 5;
      this.linkLabel.TabStop = true;
      this.linkLabel.Text = "gamebooster/simcity-speedbooster";
      this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelLinkClicked);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(205, 46);
      this.Controls.Add(this.linkLabel);
      this.Controls.Add(this.optionsButton);
      this.Controls.Add(this.statusLabel);
      this.Controls.Add(this.speedNumeric);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(260, 80);
      this.MinimizeBox = false;
      this.Name = "Main";
      this.ShowIcon = false;
      this.Text = "SimCity SpeedBooster";
      this.usageTooltip.SetToolTip(this, "gamebooster.github.com");
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
      ((System.ComponentModel.ISupportInitialize)(this.speedNumeric)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NumericUpDown speedNumeric;
    private System.Windows.Forms.Label statusLabel;
    private System.Windows.Forms.Timer searchTimer;
    private System.Windows.Forms.Timer checkTimer;
    private System.Windows.Forms.Button optionsButton;
    private System.Windows.Forms.ToolTip usageTooltip;
    private System.Windows.Forms.LinkLabel linkLabel;
  }
}

