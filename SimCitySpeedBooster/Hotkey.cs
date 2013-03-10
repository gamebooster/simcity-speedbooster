using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GW2FoVBooster {
  public class Hotkey : IMessageFilter {
    #region Interop

    private const int WmHotkey = 0x312;

    private const int ModAlt = 0x1;
    private const int ModControl = 0x2;
    private const int ModShift = 0x4;
    private const int ModWin = 0x8;

    private const uint ErrorHotkeyAlreadyRegistered = 1409;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int UnregisterHotKey(IntPtr hWnd, int id);

    #endregion

    private const int MaximumID = 0xBFFF;
    private static int _currentID;

    [XmlIgnore] private int _id;
    private Keys _keyCode;
    [XmlIgnore] private bool _registered;
    [XmlIgnore] private Control _windowControl;

    public Hotkey(Keys keyCode) {
      KeyCode = keyCode;

      Application.AddMessageFilter(this);
    }

    public bool Registered {
      get { return _registered; }
    }

    public Keys KeyCode {
      get { return _keyCode; }
      set {
        // Save and reregister
        _keyCode = value;
        Reregister();
      }
    }

    #region IMessageFilter Members

    public bool PreFilterMessage(ref Message message) {
      // Only process WM_HOTKEY messages
      if (message.Msg != WmHotkey)
      {
        return false;
      }

      // Check that the ID is our key and we are registerd
      if (_registered && (message.WParam.ToInt32() == _id))
      {
        // Fire the event and pass on the event if our handlers didn't handle it
        return OnPressed();
      }
      return false;
    }

    #endregion

    public event HandledEventHandler Pressed;

    ~Hotkey() {
      // Unregister the hotkey if necessary
      if (Registered)
      {
        Unregister();
      }
    }

    public bool GetCanRegister(Control windowControl) {
      // Handle any exceptions: they mean "no, you can't register" :)
      try
      {
        // Attempt to register
        if (!Register(windowControl))
        {
          return false;
        }

        // Unregister and say we managed it
        Unregister();
        return true;
      }
      catch (Win32Exception)
      {
        return false;
      }
      catch (NotSupportedException)
      {
        return false;
      }
    }

    public bool Register(Control windowControl) {
      if (_registered) {
        throw new NotSupportedException("You cannot register a hotkey that is already registered");
      }

      if (KeyCode == Keys.None) {
        throw new NotSupportedException("You cannot register an empty hotkey");
      }

      // Get an ID for the hotkey and increase current ID
      _id = _currentID;
      _currentID = _currentID + 1%MaximumID;


      uint modifiers = 0;

      if ((KeyCode & Keys.Alt) == Keys.Alt)
        modifiers = modifiers | ModAlt;

      if ((KeyCode & Keys.Control) == Keys.Control)
        modifiers = modifiers | ModControl;

      if ((KeyCode & Keys.Shift) == Keys.Shift)
        modifiers = modifiers | ModShift;

      Keys k = KeyCode & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;   

      // Register the hotkey
      if (RegisterHotKey(windowControl.Handle, _id, modifiers, k) == 0)
      {
        // Is the error that the hotkey is registered?
        if (Marshal.GetLastWin32Error() == ErrorHotkeyAlreadyRegistered)
        {
          return false;
        }
        throw new Win32Exception();
      }

      // Save the control reference and register state
      _registered = true;
      _windowControl = windowControl;

      // We successfully registered
      return true;
    }

    public void Unregister() {
      // Check that we have registered
      if (!_registered)
      {
        throw new NotSupportedException("You cannot unregister a hotkey that is not registered");
      }

      // It's possible that the control itself has died: in that case, no need to unregister!
      if (!_windowControl.IsDisposed)
      {
        // Clean up after ourselves
        if (UnregisterHotKey(_windowControl.Handle, _id) == 0)
        {
          throw new Win32Exception();
        }
      }

      // Clear the control reference and register state
      _registered = false;
      _windowControl = null;
    }

    private void Reregister() {
      // Only do something if the key is already registered
      if (!_registered)
      {
        return;
      }

      // Save control reference
      Control windowControl = _windowControl;

      // Unregister and then reregister again
      Unregister();
      Register(windowControl);
    }

    private bool OnPressed() {
      // Fire the event if we can
      var handledEventArgs = new HandledEventArgs(false);
      if (Pressed != null)
      {
        Pressed(this, handledEventArgs);
      }

      // Return whether we handled the event or not
      return handledEventArgs.Handled;
    }
  }
}