using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace GW2FoVBooster {
  internal class ProcessMemory {
    private Process _process;

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
      if (ReadProcessMemory(_process.Handle, address, buffer, size, out bytesRead) == false) throw new ApplicationException("ReadProcessMemory failed: " + address);
      return buffer;
    }

    public string ReadString(IntPtr address, int size) {
      return System.Text.Encoding.UTF8.GetString(ReadBytes(address, size), 0, size);
    }

    public string ReadWideString(IntPtr address, int size) {
      return System.Text.Encoding.Unicode.GetString(ReadBytes(address, size), 0, size);
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
      return !btPattern.Where((t, x) => strMask[x] != '?' && ((strMask[x] == 'x') && (t != data[x + offset]))).Any();
    }

    public IntPtr FindPattern(byte[] bytePattern, string stringMask) {
      if (bytePattern.Length < 1) throw new ArgumentException("bytePattern < 0");
      if (stringMask.Length != bytePattern.Length) throw new ArgumentException("strMask.Length != btPattern.Length");

      const int minAddress = 0x400000;
      const int maxAddress = 0x800000;

      var data = ReadBytes((IntPtr) minAddress, maxAddress - minAddress);

      for (var x = 0; x < maxAddress; x++) {
        if (MaskCheck(data, x, bytePattern, stringMask)) {
          return new IntPtr(minAddress + (x));
        }
      }

      return IntPtr.Zero;
    }
  }
}
