using System.Windows.Forms;

namespace GW2FoVBooster {
  public partial class OptionsForm : Form {
    public Keys IncreaseHotkey { get; private set; }
    public Keys DecreaseHotkey { get; private set; }

    public OptionsForm(Keys increaseHotkey, Keys decreaseHotkey) {
      InitializeComponent();
      increaseHotkeyBox.Text = increaseHotkey.ToString();
      IncreaseHotkey = increaseHotkey;
      decreaseHotkeyBox.Text = decreaseHotkey.ToString();
      DecreaseHotkey = decreaseHotkey;
    }

    private void IncreaseHotkeyBoxKeyDown(object sender, KeyEventArgs e) {
      increaseHotkeyBox.Text = e.KeyData.ToString();
      IncreaseHotkey = e.KeyData;
    }

    private void DecreaseHotkeyBoxKeyDown(object sender, KeyEventArgs e) {
      decreaseHotkeyBox.Text = e.KeyData.ToString();
      DecreaseHotkey = e.KeyData;
    }
  }
}
