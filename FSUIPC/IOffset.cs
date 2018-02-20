// Decompiled with JetBrains decompiler
// Type: FSUIPC.IOffset
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  internal interface IOffset
  {
    fsuipcDataType DataType { get; }

    int Address { get; set; }

    int FileAddress { get; set; }

    bool Connected { get; set; }

    bool Write { get; set; }

    bool OnceOnly { get; set; }

    int DataLength { get; }

    object Value { set; get; }

    object OldValue { set; get; }

    bool WriteOnly { set; get; }

    string Group { get; }

    Guid ID { get; }
  }
}
