// Decompiled with JetBrains decompiler
// Type: FSUIPC.PageProtection
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Flags]
  internal enum PageProtection : uint
  {
    NoAccess = 1,
    Readonly = 2,
    ReadWrite = 4,
    WriteCopy = 8,
    Execute = 16, // 0x00000010
    ExecuteRead = 32, // 0x00000020
    ExecuteReadWrite = 64, // 0x00000040
    ExecuteWriteCopy = 128, // 0x00000080
    Guard = 256, // 0x00000100
    NoCache = 512, // 0x00000200
    WriteCombine = 1024, // 0x00000400
  }
}
