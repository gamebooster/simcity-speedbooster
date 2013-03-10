using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GW2FoVBooster {
  class ProcessHelper {
    struct BYTES {
      public byte BaseMid;
      public byte Flags1;
      public byte Flags2;
      public byte BaseHi;
    }

    struct BITS {
      int Value;
      /// <summary>
      /// Max set value is 255 (11111111b)
      /// </summary>
      public int BaseMid {
        get {
          return (Value & 0xFF);
        }
        set {
          Value = (Value & unchecked((int)0xFFFFFF00)) | (value & 0xFF);
        }
      }
      /// <summary>
      /// Max set value is 31 (11111b)
      /// </summary>
      public int Type {
        get {
          return (Value & 0x1F00) >> 8;
        }
        set {
          Value = (Value & unchecked((int)0xFFFFE0FF)) | ((value & 0x1F) << 8);
        }
      }
      /// <summary>
      /// Max set value is 3 (11b)
      /// </summary>
      public int Dpl {
        get {
          return (Value & 0x6000) >> 13;
        }
        set {
          Value = (Value & unchecked((int)0xFFFF9FFF)) | ((value & 0x3) << 13);
        }
      }
      /// <summary>
      /// Max set value is 1 (1b)
      /// </summary>
      public int Pres {
        get {
          return (Value & 0x4000) >> 15;
        }
        set {
          Value = (Value & unchecked((int)0xFFFFBFFF)) | ((value & 0x1) << 15);
        }
      }
      /// <summary>
      /// Max set value is 15 (1111b)
      /// </summary>
      public int LimitHi {
        get {
          return (Value & 0xF0000) >> 16;
        }
        set {
          Value = (Value & unchecked((int)0xFFF0FFFF)) | ((value & 0xF) << 16);
        }
      }
      /// <summary>
      /// Max set value is 1 (1b)
      /// </summary>
      public int Sys {
        get {
          return (Value & 0x100000) >> 20;
        }
        set {
          Value = (Value & unchecked((int)0xFFEFFFFF)) | ((value & 0x1) << 20);
        }
      }
      /// <summary>
      /// Max set value is 1 (1b)
      /// </summary>
      public int Reserved_0 {
        get {
          return (Value & 0x200000) >> 21;
        }
        set {
          Value = (Value & unchecked((int)0xFFDFFFFF)) | ((value & 0x1) << 21);
        }
      }
      /// <summary>
      /// Max set value is 1 (1b)
      /// </summary>
      public int Default_Big {
        get {
          return (Value & 0x400000) >> 22;
        }
        set {
          Value = (Value & unchecked((int)0xFFBFFFFF)) | ((value & 0x1) << 22);
        }
      }
      /// <summary>
      /// Max set value is 1 (1b)
      /// </summary>
      public int Granularity {
        get {
          return (Value & 0x800000) >> 23;
        }
        set {
          Value = (Value & unchecked((int)0xFF7FFFFF)) | ((value & 0x1) << 23);
        }
      }
      /// <summary>
      /// Max set value is 255 (11111111b)
      /// </summary>
      public int BaseHi {
        get {
          return (Value & unchecked((int)0xFF000000)) >> 24;
        }
        set {
          Value = (Value & unchecked((int)0xFFFFFF)) | ((value & 0xFF) << 24);
        }
      }
    }

    [StructLayout(LayoutKind.Explicit)]
    struct Highword {
      [FieldOffset(0)]
      public BYTES Bytes;
      [FieldOffset(0)]
      public BITS Bits;
    }

    struct LdtEntry {
      public ushort LimitLow;
      public ushort BaseLow;
      public Highword HighWord;
    }
    [Flags]
    public enum ThreadAccess {
      Terminate = (0x0001),
      SuspendResume = (0x0002),
      GetContext = (0x0008),
      SetContext = (0x0010),
      SetInformation = (0x0020),
      QueryInformation = (0x0040),
      SetThreadToken = (0x0080),
      Impersonate = (0x0100),
      DirectImpersonation = (0x0200)
    }
    public enum ContextFlags : uint {

      ContextI386 = 0x10000,
      ContextI486 = 0x10000,   //  same as i386
      ContextControl = ContextI386 | 0x01, // SS:SP, CS:IP, FLAGS, BP
      ContextInteger = ContextI386 | 0x02, // AX, BX, CX, DX, SI, DI
      ContextSegments = ContextI386 | 0x04, // DS, ES, FS, GS
      ContextFloatingPoint = ContextI386 | 0x08, // 387 state
      ContextDebugRegisters = ContextI386 | 0x10, // DB 0-3,6,7
      ContextExtendedRegisters = ContextI386 | 0x20, // cpu specific extensions
      ContextFull = ContextControl | ContextInteger | ContextSegments,
      ContextAll = ContextControl | ContextInteger | ContextSegments | ContextFloatingPoint | ContextDebugRegisters | ContextExtendedRegisters
    }

    [StructLayout(LayoutKind.Sequential)]

    public struct FloatingSaveArea {

      public uint ControlWord;
      public uint StatusWord;
      public uint TagWord;
      public uint ErrorOffset;
      public uint ErrorSelector;
      public uint DataOffset;
      public uint DataSelector;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
      public byte[] RegisterArea;
      public uint Cr0NpxState;
    }

    [StructLayout(LayoutKind.Sequential)]

    public struct Context {

      public uint ContextFlags; //set this to an appropriate value 
      // Retrieved by CONTEXT_DEBUG_REGISTERS 
      public uint Dr0;
      public uint Dr1;
      public uint Dr2;
      public uint Dr3;
      public uint Dr6;
      public uint Dr7;
      // Retrieved by CONTEXT_FLOATING_POINT 
      public FloatingSaveArea FloatSave;
      // Retrieved by CONTEXT_SEGMENTS 
      public uint SegGs;
      public uint SegFs;
      public uint SegEs;
      public uint SegDs;
      // Retrieved by CONTEXT_INTEGER 
      public uint Edi;
      public uint Esi;
      public uint Ebx;
      public uint Edx;
      public uint Ecx;
      public uint Eax;
      // Retrieved by CONTEXT_CONTROL 
      public uint Ebp;
      public uint Eip;
      public uint SegCs;
      public uint EFlags;
      public uint Esp;
      public uint SegSs;
      // Retrieved by CONTEXT_EXTENDED_REGISTERS 
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
      public byte[] ExtendedRegisters;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle,
    uint dwThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetThreadContext(IntPtr hThread, ref Context lpContext);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetThreadSelectorEntry(
        IntPtr hThread,
        uint dwSelector,
        out LdtEntry lpSelectorEntry
        );

    static public IntPtr GetThreadInformationBlock(uint threadId) {
      var context = new Context { ContextFlags = (uint)ContextFlags.ContextSegments };

      var hThread = OpenThread((uint)(ThreadAccess.GetContext | ThreadAccess.QueryInformation), false, threadId);
      if (!GetThreadContext(hThread, ref context)) throw new ApplicationException("GetThreadContext");

      LdtEntry ldtEntry;
      if (!GetThreadSelectorEntry(hThread, context.SegFs, out ldtEntry)) throw new ApplicationException("GetThreadSelectorEntry");

      var threadInformationBlock = (ldtEntry.HighWord.Bits.BaseHi << 24) | (ldtEntry.HighWord.Bits.BaseMid << 16) | (ldtEntry.BaseLow);
      return (IntPtr) threadInformationBlock;
    }
  }
}
