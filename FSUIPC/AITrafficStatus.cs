// Decompiled with JetBrains decompiler
// Type: FSUIPC.AITrafficStatus
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

namespace FSUIPC
{
  public enum AITrafficStatus : byte
  {
    StatusNotAvilable = 0,
    Initialising = 128, // 0x80
    Sleeping = 129, // 0x81
    FilingFlightPlan = 130, // 0x82
    ObtainingClearance = 131, // 0x83
    PushingBack = 132, // 0x84
    PushingBackTurn = 133, // 0x85
    StartingUp = 134, // 0x86
    ReadyForTaxi = 135, // 0x87
    TaxiingOut = 136, // 0x88
    ReadyForTakeOff = 137, // 0x89
    TakingOff = 138, // 0x8A
    Departing = 139, // 0x8B
    Enroute = 140, // 0x8C
    InThePattern = 141, // 0x8D
    Landing = 142, // 0x8E
    RollingOut = 143, // 0x8F
    GoingAround = 144, // 0x90
    TaxiingIn = 145, // 0x91
    ShuttingDown = 146, // 0x92
  }
}
