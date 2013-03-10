namespace GW2FoVBooster {
  partial class OptionsForm {
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
      this.increaseHotkeyBox = new System.Windows.Forms.TextBox();
      this.increaseHotkeyLabel = new System.Windows.Forms.Label();
      this.decreaseHotkeyBox = new System.Windows.Forms.TextBox();
      this.decreaseHotkeyLabel = new System.Windows.Forms.Label();
      this.usageTooltip = new System.Windows.Forms.ToolTip(this.components);
      this.SuspendLayout();
      // 
      // increaseHotkeyBox
      // 
      this.increaseHotkeyBox.Location = new System.Drawing.Point(91, 12);
      this.increaseHotkeyBox.Name = "increaseHotkeyBox";
      this.increaseHotkeyBox.Size = new System.Drawing.Size(121, 20);
      this.increaseHotkeyBox.TabIndex = 4;
      this.usageTooltip.SetToolTip(this.increaseHotkeyBox, "Type the keys you want to set the hotkey to");
      this.increaseHotkeyBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IncreaseHotkeyBoxKeyDown);
      // 
      // increaseHotkeyLabel
      // 
      this.increaseHotkeyLabel.AutoSize = true;
      this.increaseHotkeyLabel.Location = new System.Drawing.Point(3, 15);
      this.increaseHotkeyLabel.Name = "increaseHotkeyLabel";
      this.increaseHotkeyLabel.Size = new System.Drawing.Size(82, 13);
      this.increaseHotkeyLabel.TabIndex = 3;
      this.increaseHotkeyLabel.Text = "IncreaseHotkey";
      // 
      // decreaseHotkeyBox
      // 
      this.decreaseHotkeyBox.Location = new System.Drawing.Point(91, 38);
      this.decreaseHotkeyBox.Name = "decreaseHotkeyBox";
      this.decreaseHotkeyBox.Size = new System.Drawing.Size(121, 20);
      this.decreaseHotkeyBox.TabIndex = 6;
      this.usageTooltip.SetToolTip(this.decreaseHotkeyBox, "Type the keys you want to set the hotkey to");
      this.decreaseHotkeyBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DecreaseHotkeyBoxKeyDown);
      // 
      // decreaseHotkeyLabel
      // 
      this.decreaseHotkeyLabel.AutoSize = true;
      this.decreaseHotkeyLabel.Location = new System.Drawing.Point(3, 41);
      this.decreaseHotkeyLabel.Name = "decreaseHotkeyLabel";
      this.decreaseHotkeyLabel.Size = new System.Drawing.Size(87, 13);
      this.decreaseHotkeyLabel.TabIndex = 5;
      this.decreaseHotkeyLabel.Text = "DecreaseHotkey";
      // 
      // OptionsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(224, 66);
      this.Controls.Add(this.decreaseHotkeyBox);
      this.Controls.Add(this.decreaseHotkeyLabel);
      this.Controls.Add(this.increaseHotkeyBox);
      this.Controls.Add(this.increaseHotkeyLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "OptionsForm";
      this.Text = "Options";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox increaseHotkeyBox;
    private System.Windows.Forms.Label increaseHotkeyLabel;
    private System.Windows.Forms.TextBox decreaseHotkeyBox;
    private System.Windows.Forms.Label decreaseHotkeyLabel;
    private System.Windows.Forms.ToolTip usageTooltip;
  }
}