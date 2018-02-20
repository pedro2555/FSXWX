// Decompiled with JetBrains decompiler
// Type: FSUIPC.SendMessageTimeoutFlags
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Flags]
  internal enum SendMessageTimeoutFlags : uint
  {
    SMTO_NORMAL = 0,
    SMTO_BLOCK = 1,
    SMTO_ABORTIFHUNG = 2,
    SMTO_NOTIMEOUTIFNOTHUNG = 8,
  }
}
