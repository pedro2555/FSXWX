// Decompiled with JetBrains decompiler
// Type: FSUIPC.DesiredAccess
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Flags]
  internal enum DesiredAccess : uint
  {
    StandardRights = 983040, // 0x000F0000
    Query = 1,
    MapWrite = 2,
    MapRead = 4,
    MapExecute = 8,
    SectionExtendSize = 16, // 0x00000010
  }
}
