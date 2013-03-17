using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SimCitySpeedBooster {
  internal class ProcessMemory {
    [Flags]
    public enum AllocationType {
      Commit = 0x1000,
      Reserve = 0x2000,
      Decommit = 0x4000,
      Release = 0x8000,
      Reset = 0x80000,
      Physical = 0x400000,
      TopDown = 0x100000,
      WriteWatch = 0x200000,
      LargePages = 0x20000000
    }

    [Flags]
    public enum FreeType {
      Decommit = 0x4000,
      Release = 0x8000,
    }

    [Flags]
    public enum MemoryProtection {
      Execute = 0x10,
      ExecuteRead = 0x20,
      ExecuteReadWrite = 0x40,
      ExecuteWriteCopy = 0x80,
      NoAccess = 0x01,
      ReadOnly = 0x02,
      ReadWrite = 0x04,
      WriteCopy = 0x08,
      GuardModifierflag = 0x100,
      NoCacheModifierflag = 0x200,
      WriteCombineModifierflag = 0x400
    }

    public enum Protection {
      PageNoaccess = 0x01,
      PageReadonly = 0x02,
      PageReadwrite = 0x04,
      PageWritecopy = 0x08,
      PageExecute = 0x10,
      PageExecuteRead = 0x20,
      PageExecuteReadwrite = 0x40,
      PageExecuteWritecopy = 0x80,
      PageGuard = 0x100,
      PageNocache = 0x200,
      PageWritecombine = 0x400
    }

    private Process _process;

    [DllImport("kernel32.dll")]
    private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, Protection flNewProtect,
                                                out Protection lpflOldProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
                                                  out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer,
                                                 int dwSize, out int lpNumberOfBytesRead);

    ~ProcessMemory() {
      _process = null;
    }

    public void OpenProcess(Process process) {
      _process = process;
    }

    public byte[] ReadBytes(IntPtr address, int size) {
      var buffer = new byte[size];
      int bytesRead;
      if (ReadProcessMemory(_process.Handle, address, buffer, size, out bytesRead) == false)
        throw new ApplicationException("ReadProcessMemory failed: " + address);
      return buffer;
    }

    public void Write(IntPtr address, byte[] data) {
      var size = (uint) data.Length;

      int written;
      Protection protection;
      if (VirtualProtectEx(_process.Handle, address, size, Protection.PageExecuteReadwrite, out protection) == false) {
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }
      if (WriteProcessMemory(_process.Handle, address, data, size, out written) == false) {
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }
      if (VirtualProtectEx(_process.Handle, address, size, protection, out protection) == false) {
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }
    }

    public string ReadString(IntPtr address, int size) {
      return Encoding.UTF8.GetString(ReadBytes(address, size), 0, size);
    }

    public string ReadWideString(IntPtr address, int size) {
      return Encoding.Unicode.GetString(ReadBytes(address, size), 0, size);
    }

    public bool ReadBoolean(IntPtr address) {
      return BitConverter.ToBoolean(ReadBytes(address, 1), 0);
    }

    public char ReadChar(IntPtr address) {
      return BitConverter.ToChar(ReadBytes(address, 1), 0);
    }

    public bool WriteFloat(IntPtr address, float value) {
      int written;
      return WriteProcessMemory(_process.Handle, address, BitConverter.GetBytes(value), 4, out written);
    }

    public float ReadFloat(IntPtr address) {
      return BitConverter.ToSingle(ReadBytes(address, 4), 0);
    }

    public double ReadDouble(IntPtr address) {
      return BitConverter.ToDouble(ReadBytes(address, 8), 0);
    }

    public Int16 ReadInt16(IntPtr address) {
      return BitConverter.ToInt16(ReadBytes(address, 2), 0);
    }

    public Int32 ReadInt32(IntPtr address) {
      return BitConverter.ToInt32(ReadBytes(address, 4), 0);
    }

    public Int64 ReadInt64(IntPtr address) {
      return BitConverter.ToInt64(ReadBytes(address, 8), 0);
    }

    public UInt16 ReadUInt16(IntPtr address) {
      return BitConverter.ToUInt16(ReadBytes(address, 2), 0);
    }

    public UInt32 ReadUInt32(IntPtr address) {
      return BitConverter.ToUInt32(ReadBytes(address, 4), 0);
    }

    public UInt64 ReadUInt64(IntPtr address) {
      return BitConverter.ToUInt64(ReadBytes(address, 8), 0);
    }

    public UIntPtr ReadUInt32Ptr(IntPtr address) {
      return new UIntPtr(BitConverter.ToUInt32(ReadBytes(address, 4), 0));
    }

    public UIntPtr ReadUInt64Ptr(IntPtr address) {
      return new UIntPtr(BitConverter.ToUInt64(ReadBytes(address, 8), 0));
    }

    public IntPtr ReadInt32Ptr(IntPtr address) {
      return new IntPtr(BitConverter.ToInt32(ReadBytes(address, 4), 0));
    }

    public IntPtr ReadInt64Ptr(IntPtr address) {
      return new IntPtr(BitConverter.ToInt64(ReadBytes(address, 8), 0));
    }

    private bool MaskCheck(IList<byte> data, int offset, IEnumerable<byte> btPattern, string strMask) {
      return
        !btPattern.Where(
          (t, x) => strMask[x] != '?' && ((strMask[x] == 'x') && (data.Count > x + offset && t != data[x + offset])))
                  .Any();
    }

    public IntPtr FindPattern(byte[] bytePattern, string stringMask) {
      if (bytePattern.Length < 1) throw new ArgumentException("bytePattern < 0");
      if (stringMask.Length != bytePattern.Length) throw new ArgumentException("strMask.Length != btPattern.Length");

      const int minAddress = 0x400000;
      const int maxAddress = 0x800000;

      byte[] data = ReadBytes((IntPtr) minAddress, maxAddress - minAddress);

      for (int x = 0; x < maxAddress; x++) {
        if (MaskCheck(data, x, bytePattern, stringMask)) {
          return new IntPtr(minAddress + (x));
        }
      }

      return IntPtr.Zero;
    }
  }
}