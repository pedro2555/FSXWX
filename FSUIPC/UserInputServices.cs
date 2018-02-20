// Decompiled with JetBrains decompiler
// Type: FSUIPC.UserInputServices
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FSUIPC
{
  public class UserInputServices
  {
    private readonly int keySlotsBase = 12816;
    private readonly int buttonSlotsBase = 10512;
    private string temporaryGroup = "~~UserInputTemp~~";
    private string pollingSlotsGroup = "~~UserInputSlotPolling~~";
    private string pollingSlotsResetGroup = "~~UserInputSlotPollingReset~~";
    private string keepAliveGroup = "~~UserInputKeepAlive~~";
    private Dictionary<string, Offset<byte>> keySlotIndicators = new Dictionary<string, Offset<byte>>();
    private Dictionary<string, Offset<byte>> buttonSlotIndicators = new Dictionary<string, Offset<byte>>();
    private Dictionary<string, Offset<byte>> menuSlotIndicators = new Dictionary<string, Offset<byte>>();
    private Dictionary<string, Offset<byte>> menuKeepAlives = new Dictionary<string, Offset<byte>>();
    private int keySlotCount;
    private int buttonSlotCount;
    private byte classinstance;

    internal UserInputServices(byte ClassInstance)
    {
      this.classinstance = ClassInstance;
      this.temporaryGroup += this.classinstance.ToString();
      this.pollingSlotsGroup += this.classinstance.ToString();
      this.pollingSlotsResetGroup += this.classinstance.ToString();
      this.keepAliveGroup += this.classinstance.ToString();
      Offset<int> offset1 = new Offset<int>(this.temporaryGroup, 10508);
      Offset<int> offset2 = new Offset<int>(this.temporaryGroup, 12812);
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      this.keySlotCount = offset2.Value;
      this.buttonSlotCount = offset1.Value;
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
    }

    public void AddKeyPresss(string ID, ModifierKeys Modifier, Keys Key, bool PassThroughToFS)
    {
      if (this.keySlotCount <= 0 || this.keySlotCount >= 9000)
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_VERSION, "This version of FSUIPC is too old to support hot key user input.");
      int freeSlot = this.findFreeSlot(this.keySlotsBase, this.keySlotCount);
      if (freeSlot < 0)
        throw new FSUIPCException(FSUIPCError.FSUIPC_KEY_SLOTS_FULL, "Cannot add keypress: " + ID);
      new Offset<int>(this.temporaryGroup, this.keySlotsBase + freeSlot * 4, true).Value = this.assembleInt((byte) Key, (byte) Modifier, PassThroughToFS ? (byte) 2 : (byte) 0);
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      Offset<byte> offset = new Offset<byte>(this.pollingSlotsGroup, this.keySlotsBase + freeSlot * 4 + 3);
      this.keySlotIndicators.Add(ID, offset);
    }

    public void AddJoystickButtonPress(string ID, byte JoystickNumber, byte ButtonNumber, StateChange StateChangeToDetect)
    {
      if (this.buttonSlotCount <= 0 || this.buttonSlotCount >= 9000)
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_VERSION, "This version of FSUIPC is too old to support joystick button user input.");
      int freeSlot = this.findFreeSlot(this.buttonSlotsBase, this.buttonSlotCount);
      if (freeSlot < 0)
        throw new FSUIPCException(FSUIPCError.FSUIPC_BUTTON_SLOTS_FULL, "Cannot add button press: " + ID);
      new Offset<int>(this.temporaryGroup, this.buttonSlotsBase + freeSlot * 4, true).Value = this.assembleInt((byte) ((uint) JoystickNumber + 128U), ButtonNumber, (byte) StateChangeToDetect);
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      Offset<byte> offset = new Offset<byte>(this.pollingSlotsGroup, this.buttonSlotsBase + freeSlot * 4 + 3);
      this.buttonSlotIndicators.Add(ID, offset);
    }

    public void AddMenuItem(string ID, string MenuText, bool PauseFSOnSelection)
    {
      if (this.keySlotCount <= 0 || this.keySlotCount >= 9000)
        throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_VERSION, "This version of FSUIPC is too old to support adding FS menu items.");
      int freeSlot = this.findFreeSlot(this.keySlotsBase, this.keySlotCount);
      if (freeSlot < 0)
        throw new FSUIPCException(FSUIPCError.FSUIPC_KEY_SLOTS_FULL, "Cannot add menu item: " + ID);
      new Offset<int>(this.temporaryGroup, this.keySlotsBase + freeSlot * 4, true).Value = this.assembleInt(byte.MaxValue, byte.MaxValue, PauseFSOnSelection ? (byte) 2 : (byte) 0);
      new Offset<string>(this.temporaryGroup, 12256, 31, true).Value = new string(Convert.ToChar(freeSlot), 1) + MenuText;
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      Offset<byte> offset1 = new Offset<byte>(this.pollingSlotsGroup, this.keySlotsBase + freeSlot * 4 + 3);
      this.menuSlotIndicators.Add(ID, offset1);
      Offset<byte> offset2 = new Offset<byte>(this.keepAliveGroup, this.keySlotsBase + freeSlot * 4 + 1, true);
      this.menuKeepAlives.Add(ID, offset2);
      Thread.Sleep(250);
    }

    private int findFreeSlot(int slotsBase, int slotCount)
    {
      int num = -1;
      Offset<int>[] offsetArray = new Offset<int>[slotCount];
      for (int index = 0; index < slotCount; ++index)
        offsetArray[index] = new Offset<int>(this.temporaryGroup, slotsBase + 4 * index);
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      for (int index = 0; index < slotCount && num < 0; ++index)
      {
        if (offsetArray[index].Value == 0)
          num = index;
      }
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      return num;
    }

    private int assembleInt(byte b0, byte b1, byte b2)
    {
      return (int) b0 + (int) b1 * 256 + (int) b2 * 65536;
    }

    public void CheckForInput()
    {
      if (this.keySlotIndicators.Count + this.buttonSlotIndicators.Count + this.menuSlotIndicators.Count <= 0)
        return;
      FSUIPCConnection.Process(this.classinstance, this.pollingSlotsGroup);
      List<UserInputKeyEventArgs> inputKeyEventArgsList = new List<UserInputKeyEventArgs>();
      List<UserInputButtonEventArgs> inputButtonEventArgsList = new List<UserInputButtonEventArgs>();
      List<UserInputMenuEventArgs> inputMenuEventArgsList = new List<UserInputMenuEventArgs>();
      IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) this.keySlotIndicators.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        Offset<byte> offset = (Offset<byte>) enumerator1.Value;
        if ((int) offset.Value > 0)
        {
          Keys keys = Keys.None;
          if ((int) offset.Value > 1)
            keys = (Keys) offset.Value;
          inputKeyEventArgsList.Add(new UserInputKeyEventArgs()
          {
            ID = enumerator1.Key.ToString(),
            SecondKeyPressed = keys
          });
          new Offset<byte>(this.pollingSlotsResetGroup, offset.address, true).Value = (byte) 0;
        }
      }
      IDictionaryEnumerator enumerator2 = (IDictionaryEnumerator) this.menuSlotIndicators.GetEnumerator();
      while (enumerator2.MoveNext())
      {
        Offset<byte> offset = (Offset<byte>) enumerator2.Value;
        if (((int) offset.Value & 1) == 1)
        {
          inputMenuEventArgsList.Add(new UserInputMenuEventArgs()
          {
            ID = enumerator2.Key.ToString()
          });
          new Offset<byte>(this.pollingSlotsResetGroup, offset.address, true).Value = (byte) 0;
        }
      }
      IDictionaryEnumerator enumerator3 = (IDictionaryEnumerator) this.buttonSlotIndicators.GetEnumerator();
      while (enumerator3.MoveNext())
      {
        Offset<byte> offset = (Offset<byte>) enumerator3.Value;
        if (((int) offset.Value & 1) == 1)
        {
          bool flag = ((int) offset.Value & 2) == 2;
          inputButtonEventArgsList.Add(new UserInputButtonEventArgs()
          {
            ID = enumerator3.Key.ToString(),
            ButtonState = flag
          });
          new Offset<byte>(this.pollingSlotsResetGroup, offset.address, true).Value = (byte) 0;
        }
      }
      if (inputKeyEventArgsList.Count + inputButtonEventArgsList.Count + inputMenuEventArgsList.Count <= 0)
        return;
      FSUIPCConnection.Process(this.classinstance, this.pollingSlotsResetGroup);
      FSUIPCConnection.DeleteGroup(this.pollingSlotsResetGroup);
      foreach (UserInputKeyEventArgs e in inputKeyEventArgsList)
      {
        if (this.KeyPressed != null)
          this.KeyPressed((object) this, e);
      }
      foreach (UserInputButtonEventArgs e in inputButtonEventArgsList)
      {
        if (this.ButtonPressed != null)
          this.ButtonPressed((object) this, e);
      }
      foreach (UserInputMenuEventArgs e in inputMenuEventArgsList)
      {
        if (this.MenuSelected != null)
          this.MenuSelected((object) this, e);
      }
    }

    public void KeepMenuItemsAlive()
    {
      if (this.menuKeepAlives.Count <= 0)
        return;
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this.menuKeepAlives.GetEnumerator();
      while (enumerator.MoveNext())
        ((Offset<byte>) enumerator.Value).Value = byte.MaxValue;
      FSUIPCConnection.Process(this.classinstance, this.keepAliveGroup);
    }

    public void RemoveKeyPress(string ID)
    {
      new Offset<int>(this.temporaryGroup, this.keySlotIndicators[ID].Address - 3, true).Value = 0;
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      this.keySlotIndicators[ID].Disconnect();
      this.keySlotIndicators.Remove(ID);
    }

    public void RemoveJoystickButtonPress(string ID)
    {
      new Offset<int>(this.temporaryGroup, this.buttonSlotIndicators[ID].Address - 3, true).Value = 0;
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      this.buttonSlotIndicators[ID].Disconnect();
      this.buttonSlotIndicators.Remove(ID);
    }

    public void RemoveMenuItem(string ID)
    {
      new Offset<int>(this.temporaryGroup, this.menuSlotIndicators[ID].Address - 3, true).Value = 0;
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      this.menuSlotIndicators[ID].Disconnect();
      this.menuSlotIndicators.Remove(ID);
      this.menuKeepAlives[ID].Disconnect();
      this.menuKeepAlives.Remove(ID);
    }

    public void RemoveAll()
    {
      if (this.keySlotIndicators.Count + this.buttonSlotIndicators.Count + this.menuSlotIndicators.Count <= 0)
        return;
      IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) this.keySlotIndicators.GetEnumerator();
      while (enumerator1.MoveNext())
        new Offset<int>(this.temporaryGroup, this.keySlotIndicators[enumerator1.Key.ToString()].Address - 3, true).Value = 0;
      IDictionaryEnumerator enumerator2 = (IDictionaryEnumerator) this.buttonSlotIndicators.GetEnumerator();
      while (enumerator2.MoveNext())
        new Offset<int>(this.temporaryGroup, this.buttonSlotIndicators[enumerator2.Key.ToString()].Address - 3, true).Value = 0;
      IDictionaryEnumerator enumerator3 = (IDictionaryEnumerator) this.menuSlotIndicators.GetEnumerator();
      while (enumerator3.MoveNext())
        new Offset<int>(this.temporaryGroup, this.menuSlotIndicators[enumerator3.Key.ToString()].Address - 3, true).Value = 0;
      FSUIPCConnection.Process(this.classinstance, this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.temporaryGroup);
      FSUIPCConnection.DeleteGroup(this.pollingSlotsGroup);
      this.menuSlotIndicators.Clear();
      this.buttonSlotIndicators.Clear();
      this.keySlotIndicators.Clear();
    }

    public event EventHandler<UserInputKeyEventArgs> KeyPressed;

    public event EventHandler<UserInputButtonEventArgs> ButtonPressed;

    public event EventHandler<UserInputMenuEventArgs> MenuSelected;
  }
}
