// Decompiled with JetBrains decompiler
// Type: FSUIPC.FSUIPCConnection
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FSUIPC
{
  public static class FSUIPCConnection
  {
    private static Dictionary<string, Dictionary<Guid, IOffset>> dataGroups = new Dictionary<string, Dictionary<Guid, IOffset>>();
    private static Dictionary<Guid, string> offsetGroupDictionary = new Dictionary<Guid, string>();
    private static readonly string FS6IPCMessageName = "FsasmLib:IPC";
    private static readonly string SystemOffsetsGroup = "~SystemOffsets~";
    private static readonly uint MaximumDataSize = 32512;
    private static readonly int ERROR_ALREADY_EXISTS = 387;
    private static Dictionary<byte, ConnectionInfo> connections = new Dictionary<byte, ConnectionInfo>();
    private static IntPtr pNext = IntPtr.Zero;
    private static int tryCount = 0;
    private static FlightSim fsVersionConnected = FlightSim.Any;
    private static Dictionary<byte, AITrafficServices> aiServices = new Dictionary<byte, AITrafficServices>();
    private static Dictionary<byte, UserInputServices> userInput = new Dictionary<byte, UserInputServices>();
    private static Dictionary<byte, PayloadServices> payloadServices = new Dictionary<byte, PayloadServices>();

    [Obsolete("This didn't provide much benefit (if any) with FSUIPC3 and certainly none with FSUIPC4.  This funcionality is now disabled.", true)]
    public static bool OptimiseIPCFile
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public static Version DLLVersion
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName(false).Version;
      }
    }

    public static AITrafficServices AITrafficServices
    {
      get
      {
        return FSUIPCConnection.aiServices[(byte) 0];
      }
    }

    public static AITrafficServices AITrafficServicesForClass(byte ClassInstance)
    {
      return FSUIPCConnection.aiServices[ClassInstance];
    }

    public static UserInputServices UserInputServices
    {
      get
      {
        return FSUIPCConnection.userInput[(byte) 0];
      }
    }

    public static UserInputServices UserInputServicesForClass(byte ClassInstance)
    {
      return FSUIPCConnection.userInput[ClassInstance];
    }

    public static PayloadServices PayloadServices
    {
      get
      {
        return FSUIPCConnection.payloadServices[(byte) 0];
      }
    }

    public static PayloadServices PayloadServicesForClass(byte ClassInstance)
    {
      return FSUIPCConnection.payloadServices[ClassInstance];
    }

    public static FlightSim FlightSimVersionConnected
    {
      get
      {
        return FSUIPCConnection.fsVersionConnected;
      }
    }

    internal static void AddOffset(IOffset Offset)
    {
      lock (FSUIPCConnection.dataGroups)
      {
        Dictionary<Guid, IOffset> dictionary;
        if (!FSUIPCConnection.dataGroups.ContainsKey(Offset.Group))
        {
          dictionary = new Dictionary<Guid, IOffset>();
          FSUIPCConnection.dataGroups.Add(Offset.Group, dictionary);
        }
        else
          dictionary = FSUIPCConnection.dataGroups[Offset.Group];
        dictionary.Add(Offset.ID, Offset);
        Offset.Connected = true;
        FSUIPCConnection.offsetGroupDictionary.Add(Offset.ID, Offset.Group);
      }
    }

    internal static void RemoveOffset(IOffset Offset)
    {
      lock (FSUIPCConnection.dataGroups)
      {
        if (!FSUIPCConnection.dataGroups.ContainsKey(Offset.Group))
          return;
        FSUIPCConnection.dataGroups[Offset.Group].Remove(Offset.ID);
        Offset.Connected = false;
        FSUIPCConnection.offsetGroupDictionary.Remove(Offset.ID);
      }
    }

    public static void Open()
    {
      FSUIPCConnection.Open((byte) 0, FlightSim.Any);
    }

    public static void Open(int RequiredFlightSimVersion)
    {
      FSUIPCConnection.Open((byte) 0, RequiredFlightSimVersion);
    }

    public static void Open(FlightSim RequiredFlightSimVersion)
    {
      FSUIPCConnection.Open((byte) 0, (int) RequiredFlightSimVersion);
    }

    public static void Open(byte ClassInstance, FlightSim RequiredFlightSimVersion)
    {
      FSUIPCConnection.Open(ClassInstance, (int) RequiredFlightSimVersion);
    }

    public static void Open(byte ClassInstance, int RequiredFlightSimVersion)
    {
      FSUIPCConnection.DeleteGroup(FSUIPCConnection.SystemOffsetsGroup);
      bool flag = false;
      if (FSUIPCConnection.connections.ContainsKey(ClassInstance))
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_OPEN, "The connection to FSUIPC is already open.");
      ConnectionInfo connectionInfo = new ConnectionInfo();
      FSUIPCConnection.connections.Add(ClassInstance, connectionInfo);
      if ((int) ClassInstance == 0)
      {
        connectionInfo.hWnd = Win32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "UIPCMAIN", IntPtr.Zero);
        if (connectionInfo.hWnd == IntPtr.Zero)
        {
          connectionInfo.hWnd = Win32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "FS98MAIN", IntPtr.Zero);
          flag = true;
        }
      }
      else
      {
        connectionInfo.hWnd = Win32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "FS98MAIN" + ClassInstance.ToString("D2"), IntPtr.Zero);
        flag = true;
      }
      if (connectionInfo.hWnd == IntPtr.Zero)
      {
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOFS, "Cannot find FSUIPC or WideFS running on this machine.");
      }
      connectionInfo.messageID = Win32.RegisterWindowMessage(FSUIPCConnection.FS6IPCMessageName);
      if ((int) connectionInfo.messageID == 0)
      {
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_REGMSG, "Could not register the IPC window message");
      }
      ++FSUIPCConnection.tryCount;
      string str = FSUIPCConnection.FS6IPCMessageName + ":" + Process.GetCurrentProcess().Id.ToString("X") + ":" + FSUIPCConnection.tryCount.ToString("X");
      connectionInfo.atomFileName = Win32.GlobalAddAtom(str);
      if (connectionInfo.atomFileName == IntPtr.Zero)
      {
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_ATOM, "Could not add the Global Atom for the file mapping path.");
      }
      connectionInfo.hMap = Win32.CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PageProtection.ReadWrite, 0U, FSUIPCConnection.MaximumDataSize + 256U, str);
      if (connectionInfo.hMap == IntPtr.Zero || Marshal.GetLastWin32Error() == FSUIPCConnection.ERROR_ALREADY_EXISTS)
      {
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_MAP, "Could not create file mapping.");
      }
      connectionInfo.pView = Win32.MapViewOfFile(connectionInfo.hMap, DesiredAccess.MapWrite, 0U, 0U, 0U);
      if (connectionInfo.pView == IntPtr.Zero)
      {
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_VIEW, "Could not Open file view.");
      }
      Offset<int> offset1 = new Offset<int>(FSUIPCConnection.SystemOffsetsGroup, 13060);
      Offset<int> offset2 = new Offset<int>(FSUIPCConnection.SystemOffsetsGroup, 13064);
      for (int index = 0; index < 5 && (offset1.dataValue == 0 || offset2.dataValue == 0); ++index)
      {
        FSUIPCConnection.Process(ClassInstance, FSUIPCConnection.SystemOffsetsGroup);
        Thread.Sleep(100);
      }
      if (offset1.dataValue < 429391877 || ((long) offset2.dataValue & 4294901760L) != 4208852992L)
      {
        if (flag)
        {
          FSUIPCConnection.Close(ClassInstance);
          throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_RUNNING, "FSUIPC is not running.");
        }
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_VERSION, "Incorrect version of FSUIPC.");
      }
      int num = offset2.dataValue & (int) ushort.MaxValue;
      FSUIPCConnection.fsVersionConnected = (FlightSim) num;
      if (RequiredFlightSimVersion != 0 && RequiredFlightSimVersion != num)
      {
        FSUIPCConnection.Close(ClassInstance);
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_WRONGFS, "Incorrect version of Flight Sim");
      }
      FSUIPCConnection.DeleteGroup(FSUIPCConnection.SystemOffsetsGroup);
      if (!FSUIPCConnection.aiServices.ContainsKey(ClassInstance))
        FSUIPCConnection.aiServices.Add(ClassInstance, new AITrafficServices(ClassInstance));
      if (!FSUIPCConnection.userInput.ContainsKey(ClassInstance))
        FSUIPCConnection.userInput.Add(ClassInstance, new UserInputServices(ClassInstance));
      if (FSUIPCConnection.payloadServices.ContainsKey(ClassInstance))
        return;
      FSUIPCConnection.payloadServices.Add(ClassInstance, new PayloadServices(ClassInstance));
    }

    [Obsolete("Use DeleteGroup() instead.  Does the same thing but is a better name.", true)]
    public static void DisconnectGroup(string groupName)
    {
      FSUIPCConnection.DeleteGroup(groupName);
    }

    public static void DeleteGroup(string groupName)
    {
      if (!FSUIPCConnection.dataGroups.ContainsKey(groupName))
        return;
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) FSUIPCConnection.dataGroups[groupName].GetEnumerator();
      while (enumerator.MoveNext())
        FSUIPCConnection.offsetGroupDictionary.Remove((Guid) enumerator.Key);
      FSUIPCConnection.dataGroups.Remove(groupName);
    }

    public static void Close()
    {
      byte[] array = new byte[FSUIPCConnection.connections.Count];
      FSUIPCConnection.connections.Keys.CopyTo(array, 0);
      foreach (byte ClassInstance in array)
        FSUIPCConnection.Close(ClassInstance);
    }

    public static void Close(byte ClassInstance)
    {
      if (!FSUIPCConnection.connections.ContainsKey(ClassInstance))
        return;
      ConnectionInfo connection = FSUIPCConnection.connections[ClassInstance];
      connection.hWnd = IntPtr.Zero;
      connection.messageID = 0U;
      if (connection.atomFileName != IntPtr.Zero)
      {
        int num = (int) Win32.GlobalDeleteAtom(connection.atomFileName);
        connection.atomFileName = IntPtr.Zero;
      }
      if (connection.pView != IntPtr.Zero)
      {
        Win32.UnmapViewOfFile(connection.pView);
        connection.pView = IntPtr.Zero;
      }
      if (connection.hMap != IntPtr.Zero)
      {
        Win32.CloseHandle(connection.hMap);
        connection.hMap = IntPtr.Zero;
      }
      FSUIPCConnection.connections.Remove(ClassInstance);
    }

    public static void Process()
    {
      FSUIPCConnection.Process((byte) 0, "");
    }

    public static void Process(string GroupName)
    {
      FSUIPCConnection.Process((byte) 0, GroupName);
    }

    public static void Process(IEnumerable<string> GroupNames)
    {
      FSUIPCConnection.Process((byte) 0, GroupNames);
    }

    public static void Process(byte ClassInstance)
    {
      FSUIPCConnection.Process(ClassInstance, "");
    }

    public static void Process(byte ClassInstance, string GroupName)
    {
      FSUIPCConnection.Process(ClassInstance, (IEnumerable<string>) new List<string>()
      {
        GroupName
      });
    }

    public static void Process(byte ClassInstance, IEnumerable<string> GroupNames)
    {
      lock (FSUIPCConnection.dataGroups)
      {
        if (FSUIPCConnection.connections.ContainsKey(ClassInstance))
        {
          ConnectionInfo connection = FSUIPCConnection.connections[ClassInstance];
          if (connection.pView == IntPtr.Zero)
            throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "The connection to FSUIPC is not open.");
          int num1 = 80;
          List<IOffset> offsetList1 = new List<IOffset>();
          int int32 = connection.pView.ToInt32();
          int num2 = int32;
          int num3 = 0;
          int num4 = 0;
          List<IOffset> offsetList2 = new List<IOffset>();
          foreach (string groupName in GroupNames)
          {
            if (!FSUIPCConnection.dataGroups.ContainsKey(groupName))
              throw new Exception("Group '" + groupName + "' does not exsist.");
            Dictionary<Guid, IOffset> dataGroup = FSUIPCConnection.dataGroups[groupName];
            offsetList2.AddRange((IEnumerable<IOffset>) dataGroup.Values);
          }
          int num5 = (num1 << 8) + 97;
          foreach (IOffset offset in offsetList2)
          {
            if (!offset.Write)
            {
              switch (offset.DataType)
              {
                case fsuipcDataType.TypeByteArray:
                  byte[] numArray = (byte[]) offset.Value;
                  if (numArray != null)
                  {
                    byte[] oldValue = (byte[]) offset.OldValue;
                    bool flag = true;
                    for (int index = 0; index < oldValue.Length; ++index)
                      flag &= (int) oldValue[index] == (int) numArray[index];
                    offset.Write = !flag && (offset.WriteOnly || offset.FileAddress != 0);
                    break;
                  }
                  break;
                case fsuipcDataType.TypeBitArray:
                  BitArray bitArray = (BitArray) offset.Value;
                  if (bitArray != null)
                  {
                    BitArray oldValue = (BitArray) offset.OldValue;
                    bool flag = true;
                    for (int index = 0; index < oldValue.Length; ++index)
                      flag &= oldValue[index] == bitArray[index];
                    offset.Write = !flag && (offset.WriteOnly || offset.FileAddress != 0);
                    break;
                  }
                  break;
              }
            }
            ++num4;
          }
          int num6 = 0;
          int num7 = (num5 << 16) + 30060;
          foreach (IOffset offset in offsetList2)
          {
            if (offset.OnceOnly)
              offsetList1.Add(offset);
            int[] source1 = (int[]) null;
            if (offset.Write)
            {
              source1 = new int[3]
              {
                2,
                offset.Address,
                offset.DataLength
              };
              num3 = source1.Length * 4;
            }
            else if (!offset.WriteOnly)
            {
              source1 = new int[4]
              {
                1,
                offset.Address,
                offset.DataLength,
                num7
              };
              num3 = source1.Length * 4;
              offset.FileAddress = num2 + num3;
            }
            if (source1 != null)
            {
              if ((long) (num2 - int32 + num3 + offset.DataLength + 4) > (long) FSUIPCConnection.MaximumDataSize)
                throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_SIZE, "The amount of data requested exceeded the maximum allowed in one Process().");
              byte[] source2 = new byte[source1[2]];
              if (offset.Write)
              {
                switch (offset.DataType)
                {
                  case fsuipcDataType.TypeByte:
                    source2[0] = (byte) offset.Value;
                    break;
                  case fsuipcDataType.TypeInt16:
                    source2 = BitConverter.GetBytes((short) offset.Value);
                    break;
                  case fsuipcDataType.TypeInt32:
                    source2 = BitConverter.GetBytes((int) offset.Value);
                    break;
                  case fsuipcDataType.TypeInt64:
                    source2 = BitConverter.GetBytes((long) offset.Value);
                    break;
                  case fsuipcDataType.TypeDouble:
                    source2 = BitConverter.GetBytes((double) offset.Value);
                    break;
                  case fsuipcDataType.TypeFloat:
                    source2 = BitConverter.GetBytes((float) offset.Value);
                    break;
                  case fsuipcDataType.TypeByteArray:
                    source2 = (byte[]) offset.Value;
                    if (source2.Length != offset.DataLength)
                      throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_WRITE_OVERFLOW, "Offset for Offset " + offset.Address.ToString("X") + " is a ByteArray with a declared length of " + offset.DataLength.ToString() + " Bytes. The array you are trying to write is different from this. (" + source2.Length.ToString() + " bytes)");
                    source2.CopyTo((Array) offset.OldValue, 0);
                    break;
                  case fsuipcDataType.TypeUInt16:
                    source2 = BitConverter.GetBytes((ushort) offset.Value);
                    break;
                  case fsuipcDataType.TypeUInt32:
                    source2 = BitConverter.GetBytes((uint) offset.Value);
                    break;
                  case fsuipcDataType.TypeUInt64:
                    source2 = BitConverter.GetBytes((ulong) offset.Value);
                    break;
                  case fsuipcDataType.TypeString:
                    string s = (string) offset.Value;
                    if (s.Length >= offset.DataLength)
                      s = s.Substring(0, offset.DataLength - 1);
                    source1[2] = s.Length + 1;
                    source2 = new byte[source1[2]];
                    Encoding.ASCII.GetBytes(s).CopyTo((Array) source2, 0);
                    break;
                  case fsuipcDataType.TypeBitArray:
                    BitArray bits = (BitArray) offset.Value;
                    if (bits.Length != offset.DataLength * 8)
                      throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_WRITE_OVERFLOW, "Offset for Offset " + offset.Address.ToString("X") + " is a BitArray with a declared length of " + offset.DataLength.ToString() + " Bytes. The BitArray you are trying to write is different from this. (" + bits.Length.ToString() + " bits)");
                    offset.OldValue = (object) new BitArray(bits);
                    bits.CopyTo((Array) source2, 0);
                    break;
                  case fsuipcDataType.TypeSByte:
                    source2[0] = (byte) offset.Value;
                    break;
                }
              }
              Marshal.Copy(source1, 0, new IntPtr(num2), source1.Length);
              num2 += num3;
              Marshal.Copy(source2, 0, new IntPtr(num2), source2.Length);
              num2 += source2.Length;
            }
            ++num6;
          }
          Marshal.WriteInt32(new IntPtr(num2), 0);
          IntPtr result = IntPtr.Zero;
          int num8 = 0;
          while (Win32.SendMessageTimeout(connection.hWnd, connection.messageID, connection.atomFileName, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_BLOCK, 2000U, out result) == IntPtr.Zero && num8 < 10)
          {
            ++num8;
            Thread.Sleep(100);
          }
          if (num8 >= 10)
          {
            if (Marshal.GetLastWin32Error() == 0)
              throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_TIMEOUT, "SendMessage timed-out.  Tried 10 times.");
            throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_SENDMSG, "Error sending message to FSUIPC.");
          }
          connection.pView.ToInt32();
          int num9 = 0;
          foreach (IOffset offset in offsetList2)
          {
            if (!offset.Write && !offset.WriteOnly)
            {
              byte[] numArray = new byte[offset.DataLength];
              Marshal.Copy(new IntPtr(offset.FileAddress), numArray, 0, numArray.Length);
              switch (offset.DataType)
              {
                case fsuipcDataType.TypeByte:
                  offset.Value = (object) numArray[0];
                  break;
                case fsuipcDataType.TypeInt16:
                  offset.Value = (object) BitConverter.ToInt16(numArray, 0);
                  break;
                case fsuipcDataType.TypeInt32:
                  offset.Value = (object) BitConverter.ToInt32(numArray, 0);
                  break;
                case fsuipcDataType.TypeInt64:
                  offset.Value = (object) BitConverter.ToInt64(numArray, 0);
                  break;
                case fsuipcDataType.TypeDouble:
                  offset.Value = (object) BitConverter.ToDouble(numArray, 0);
                  break;
                case fsuipcDataType.TypeFloat:
                  offset.Value = (object) BitConverter.ToSingle(numArray, 0);
                  break;
                case fsuipcDataType.TypeByteArray:
                  offset.Value = (object) numArray;
                  offset.OldValue = (object) numArray;
                  break;
                case fsuipcDataType.TypeUInt16:
                  offset.Value = (object) BitConverter.ToUInt16(numArray, 0);
                  break;
                case fsuipcDataType.TypeUInt32:
                  offset.Value = (object) BitConverter.ToUInt32(numArray, 0);
                  break;
                case fsuipcDataType.TypeUInt64:
                  offset.Value = (object) BitConverter.ToUInt64(numArray, 0);
                  break;
                case fsuipcDataType.TypeString:
                  string str = Encoding.ASCII.GetString(numArray);
                  int length = str.IndexOf(char.MinValue);
                  offset.Value = length <= 0 ? (length != -1 ? (object) "" : (object) str) : (object) str.Substring(0, length);
                  break;
                case fsuipcDataType.TypeBitArray:
                  offset.Value = (object) new BitArray(numArray);
                  offset.OldValue = (object) new BitArray(numArray);
                  break;
                case fsuipcDataType.TypeSByte:
                  offset.Value = (object) (sbyte) numArray[0];
                  break;
              }
            }
            offset.Write = false;
            ++num9;
          }
          foreach (IOffset Offset in offsetList1)
          {
            Offset.OnceOnly = false;
            FSUIPCConnection.RemoveOffset(Offset);
          }
        }
        else
        {
          if ((int) ClassInstance == 0)
            throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "The connection to FSUIPC is not open.");
          throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "The connection to class instance " + ClassInstance.ToString("D2") + " of WideClient.exe is not open.");
        }
      }
    }
  }
}
