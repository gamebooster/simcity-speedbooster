using System;
using System.Diagnostics;
using System.Windows.Forms;
using GW2FoVBooster;
using SimCitySpeedBooster.Properties;

namespace SimCitySpeedBooster {
  public partial class Main : Form {
    private readonly ProcessMemory _processMemory = new ProcessMemory();
    private IntPtr _gameUI;
    private IntPtr _speedAddress;

    private readonly Hotkey _increaseHotkey;
    private readonly Hotkey _decreaseHotkey;
    private IntPtr _ifAddress;

    public Main() {
      InitializeComponent();
      speedNumeric.Hide();

      searchTimer.Start();

      _increaseHotkey = new Hotkey(Settings.Default.increaseHotkey);
      _increaseHotkey.Pressed += (sender, args) => {
        if (speedNumeric.Value + 1 <= speedNumeric.Maximum) speedNumeric.Value += 1;
      };
      _increaseHotkey.Register(this);

      _decreaseHotkey = new Hotkey(Settings.Default.decreaseHotkey);
      _decreaseHotkey.Pressed += (sender, args) => {
        if (speedNumeric.Value - 1 >= speedNumeric.Minimum) speedNumeric.Value -= 1;
      };
      _decreaseHotkey.Register(this);
    }

    private void SpeedNumericValueChanged(object sender, EventArgs e) {
      if (_speedAddress == IntPtr.Zero) return;

      _processMemory.WriteFloat(_speedAddress, (float) speedNumeric.Value);
    }

    private void SearchTimerTick(object sender, EventArgs e) {
      speedNumeric.Hide();
      statusLabel.Show();
      statusLabel.Text = Resources.searching_SimCity;
      var processes = Process.GetProcessesByName("SimCity");
      if (processes.Length == 0) return;

      statusLabel.Text = Resources.checking_player_status___;

      _processMemory.OpenProcess(processes[0]);
      _ifAddress = _processMemory.FindPattern(new byte[] {
        0x8D, 0x4C, 0x24, 0x1C, 0x77, 0x02
      }, "xxxxxx");

      _ifAddress += 4;

      if (_processMemory.ReadBytes(_ifAddress, 2)[0] != 0x77)
        throw new ApplicationException("!ifAdress");

      _processMemory.Write(_ifAddress, new byte[] { 0x90, 0x90 });

      var address = _processMemory.FindPattern(
        new byte[]
          {
            0xE8 , 0xFF ,0xFF, 0xFF, 0xFF, 0x8B, 0xFB, 0x83, 0xEC, 0x10
          }, "x????xxxxx");

      int getFunc = _processMemory.ReadInt32(address + 0x1);
      _gameUI = _processMemory.ReadInt32Ptr(address + getFunc + 0x6);
      if (_gameUI == IntPtr.Zero) throw new ApplicationException("!_gameUI");

      searchTimer.Stop();
      checkTimer.Start();
    }

    private void CheckTimerTick(object sender, EventArgs e) {
      IntPtr gameUI;
      try {
        gameUI = _processMemory.ReadInt32Ptr(_gameUI);
      } catch (ApplicationException) {
        checkTimer.Stop();
        searchTimer.Start();
        return;
      }

      if (gameUI == IntPtr.Zero) {
        statusLabel.Text = Resources.Waiting_for_game_world;
        return;
      }

      statusLabel.Hide();
      speedNumeric.Show();

      _speedAddress = gameUI + 0x264;

      speedNumeric.Value = (decimal) _processMemory.ReadFloat(_speedAddress);
    }

    private void OptionsButtonClick(object sender, EventArgs e) {
      var optionsForm = new OptionsForm(_increaseHotkey.KeyCode, _decreaseHotkey.KeyCode);
      optionsForm.ShowDialog();

      _increaseHotkey.KeyCode = optionsForm.IncreaseHotkey;
      _decreaseHotkey.KeyCode = optionsForm.DecreaseHotkey;

      Settings.Default.decreaseHotkey = optionsForm.DecreaseHotkey;
      Settings.Default.increaseHotkey = optionsForm.IncreaseHotkey;
      Settings.Default.Save();
    }

    private void LinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Process.Start("https://github.com/gamebooster/simcity-speedbooster");
    }

    private void Main_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (_ifAddress != IntPtr.Zero) _processMemory.Write(_ifAddress, new byte[] { 0x77, 0x02 });
    }
  }
}
