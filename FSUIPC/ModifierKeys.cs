// Decompiled with JetBrains decompiler
// Type: FSUIPC.ModifierKeys
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Flags]
  public enum ModifierKeys : byte
  {
    None = 0,
    Shift = 1,
    Ctrl = 2,
    Alt = 4,
    ExpectAnotherKey = 8,
    Tab = 16, // 0x10
  }
}
