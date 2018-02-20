// Decompiled with JetBrains decompiler
// Type: FSUIPC.ConnectionInfo
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  internal class ConnectionInfo
  {
    internal IntPtr hWnd = IntPtr.Zero;
    internal IntPtr atomFileName = IntPtr.Zero;
    internal IntPtr hMap = IntPtr.Zero;
    internal IntPtr pView = IntPtr.Zero;
    internal uint messageID;
  }
}
