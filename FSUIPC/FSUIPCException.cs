// Decompiled with JetBrains decompiler
// Type: FSUIPC.FSUIPCException
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  public class FSUIPCException : Exception
  {
    private FSUIPCError fsuipcErrorCode;

    public FSUIPCError FSUIPCErrorCode
    {
      get
      {
        return this.fsuipcErrorCode;
      }
    }

    public FSUIPCException(FSUIPCError FSUIPCErrorCode, string Message)
      : base("FSUIPC Error #" + ((int) FSUIPCErrorCode).ToString() + ": " + FSUIPCErrorCode.ToString() + ". " + Message)
    {
      this.fsuipcErrorCode = FSUIPCErrorCode;
    }
  }
}
