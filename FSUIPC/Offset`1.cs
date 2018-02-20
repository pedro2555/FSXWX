// Decompiled with JetBrains decompiler
// Type: FSUIPC.Offset`1
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Collections;

namespace FSUIPC
{
  public class Offset<DataType> : IOffset
  {
    internal Guid id;
    internal int address;
    internal int fileAddress;
    internal bool write;
    internal bool connected;
    internal string group;
    internal bool onceOnly;
    internal bool writeOnly;
    internal int dataLength;
    internal fsuipcDataType dataType;
    internal DataType dataValue;
    internal DataType oldValue;

    public Offset(int Address)
      : this("", Address, 0, false)
    {
    }

    public Offset(string DataGroupName, int Address)
      : this(DataGroupName, Address, 0, false)
    {
    }

    public Offset(int Address, int ArrayOrStringLength)
      : this("", Address, ArrayOrStringLength, false)
    {
    }

    public Offset(int Address, bool WriteOnly)
      : this("", Address, 0, WriteOnly)
    {
    }

    public Offset(string DataGroupName, int Address, bool WriteOnly)
      : this(DataGroupName, Address, 0, WriteOnly)
    {
    }

    public Offset(int Address, int ArrayOrStringLength, bool WriteOnly)
      : this("", Address, ArrayOrStringLength, WriteOnly)
    {
    }

    public Offset(string DataGroupName, int Address, int ArrayOrStringLength)
      : this(DataGroupName, Address, ArrayOrStringLength, false)
    {
    }

    public Offset(string DataGroupName, int Address, int ArrayOrStringLength, bool WriteOnly)
    {
      this.initDataInfo(DataGroupName, Address, ArrayOrStringLength, WriteOnly);
    }

    public int Address
    {
      get
      {
        return this.address;
      }
      set
      {
        this.address = value;
      }
    }

    public DataType Value
    {
      get
      {
        return this.dataValue;
      }
      set
      {
        this.write = true;
        this.dataValue = value;
      }
    }

    public bool IsConnected
    {
      get
      {
        return this.connected;
      }
    }

    public bool WriteOnly
    {
      set
      {
        this.writeOnly = value;
      }
      get
      {
        return this.writeOnly;
      }
    }

    public void Disconnect()
    {
      this.Disconnect(false);
    }

    public void Disconnect(bool AfterNextProcess)
    {
      if (AfterNextProcess)
      {
        this.onceOnly = true;
      }
      else
      {
        if (!this.IsConnected)
          return;
        FSUIPCConnection.RemoveOffset((IOffset) this);
      }
    }

    public void Reconnect()
    {
      this.Reconnect(false);
    }

    public void Reconnect(bool ForNextProcessOnly)
    {
      if (!this.IsConnected)
        FSUIPCConnection.AddOffset((IOffset) this);
      this.onceOnly = ForNextProcessOnly;
    }

    private void initDataInfo(string DataGroupName, int Address, int length, bool WriteOnly)
    {
      this.connected = true;
      this.address = Address;
      this.dataType = fsuipcDataType.TypeUnknown;
      this.writeOnly = WriteOnly;
      switch (typeof (DataType).Name)
      {
        case "Byte":
          this.dataType = fsuipcDataType.TypeByte;
          this.dataLength = 1;
          break;
        case "SByte":
          this.dataType = fsuipcDataType.TypeSByte;
          this.dataLength = 1;
          break;
        case "Int16":
          this.dataType = fsuipcDataType.TypeInt16;
          this.dataLength = 2;
          break;
        case "Int32":
          this.dataType = fsuipcDataType.TypeInt32;
          this.dataLength = 4;
          break;
        case "Int64":
          this.dataType = fsuipcDataType.TypeInt64;
          this.dataLength = 8;
          break;
        case "UInt16":
          this.dataType = fsuipcDataType.TypeUInt16;
          this.dataLength = 2;
          break;
        case "UInt32":
          this.dataType = fsuipcDataType.TypeUInt32;
          this.dataLength = 4;
          break;
        case "UInt64":
          this.dataType = fsuipcDataType.TypeUInt64;
          this.dataLength = 8;
          break;
        case "Double":
          this.dataType = fsuipcDataType.TypeDouble;
          this.dataLength = 8;
          break;
        case "Single":
          this.dataType = fsuipcDataType.TypeFloat;
          this.dataLength = 4;
          break;
        case "Byte[]":
          this.dataType = fsuipcDataType.TypeByteArray;
          this.dataValue = (DataType) new byte[length];
          this.oldValue = (DataType) new byte[length];
          this.dataLength = length;
          break;
        case "String":
          this.dataType = fsuipcDataType.TypeString;
          this.dataLength = length;
          break;
        case "BitArray":
          this.dataType = fsuipcDataType.TypeBitArray;
          this.dataLength = length;
          this.dataValue = (DataType) new BitArray(length * 8);
          this.oldValue = (DataType) new BitArray(length * 8);
          break;
        default:
          throw new Exception("Tried to create an Offset with an invalid type.");
      }
      if (this.dataType == fsuipcDataType.TypeByteArray || this.dataType == fsuipcDataType.TypeString || this.dataType == fsuipcDataType.TypeBitArray)
      {
        if (length <= 0)
          throw new Exception("Must set a size set for ArrayOrStringLength for Byte[], BitArray or String.");
      }
      else if (length > 0)
        throw new Exception("Cannot specify an ArrayOrStringLength for datatypes other than Byte[], BitArray and String.");
      this.group = DataGroupName;
      this.id = Guid.NewGuid();
      FSUIPCConnection.AddOffset((IOffset) this);
    }

    fsuipcDataType IOffset.DataType
    {
      get
      {
        return this.dataType;
      }
    }

    bool IOffset.Connected
    {
      get
      {
        return this.connected;
      }
      set
      {
        this.connected = value;
      }
    }

    bool IOffset.Write
    {
      get
      {
        return this.write;
      }
      set
      {
        this.write = value;
      }
    }

    bool IOffset.OnceOnly
    {
      get
      {
        return this.onceOnly;
      }
      set
      {
        this.onceOnly = value;
      }
    }

    int IOffset.FileAddress
    {
      get
      {
        return this.fileAddress;
      }
      set
      {
        this.fileAddress = value;
      }
    }

    int IOffset.DataLength
    {
      get
      {
        return this.dataLength;
      }
    }

    object IOffset.Value
    {
      set
      {
        this.dataValue = (DataType) value;
      }
      get
      {
        return (object) this.dataValue;
      }
    }

    object IOffset.OldValue
    {
      set
      {
        this.oldValue = (DataType) value;
      }
      get
      {
        return (object) this.oldValue;
      }
    }

    string IOffset.Group
    {
      get
      {
        return this.group;
      }
    }

    Guid IOffset.ID
    {
      get
      {
        return this.id;
      }
    }
  }
}
