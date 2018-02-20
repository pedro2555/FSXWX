// Decompiled with JetBrains decompiler
// Type: FSUIPC.AITrafficServices
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FSUIPC
{
  public class AITrafficServices
  {
    private List<AIPlaneInfo> aiGround = new List<AIPlaneInfo>();
    private List<AIPlaneInfo> aiAirborne = new List<AIPlaneInfo>();
    private List<AIPlaneInfo> aiAll = new List<AIPlaneInfo>();
    private List<AIPlaneInfo> aiTCASTargets = new List<AIPlaneInfo>();
    private Dictionary<string, List<FSRunway>> aiArrivalRunwaysInUse = new Dictionary<string, List<FSRunway>>();
    private Dictionary<string, List<FSRunway>> aiDepartureRunwaysInUse = new Dictionary<string, List<FSRunway>>();
    private List<int> aiGroundIDs = new List<int>();
    private List<int> aiAirbornIDs = new List<int>();
    private List<int> aiAllIDs = new List<int>();
    private string AISystemGroup = "~AISystemGroup~";
    private string AIWriteOptionsGroupAirborne = "~AIWriteOptionsAir~";
    private string AIWriteOptionsGroupGround = "~AIWriteOptionsGround~";
    private string AISlotGroup = "~AISlots~";
    private string AITrafficIDStringGroupWrite = "~AITrafficIDsWrite~";
    private string AITrafficIDStringGroupRead = "~AITrafficIDsRead~";
    private string AITrafficIDStringGroupTest = "~AITrafficIDsTest~";
    private string AITCASWrite = "~AITCASWrite~";
    private Offset<short> slotSizeGround;
    private Offset<short> slotSizeAir;
    private Offset<short> slotsUsedGround;
    private Offset<short> slotsUsedAir;
    private Offset<short> slotChangesGround;
    private Offset<short> slotChangesAir;
    private Offset<byte[]> slotChangeArrayGround;
    private Offset<byte[]> slotChangeArrayAir;
    private Offset<long> playerLon;
    private Offset<long> playerLat;
    private Offset<long> playerAlt;
    private Offset<short> playerMagVar;
    private Offset<int> atcInfoCommand;
    private Offset<int> atcInfoTimeStampInit;
    private Offset<int> atcInfoAiID;
    private Offset<int> atcInfoSignature;
    private Offset<int> atcInfoTimeStamp;
    private Offset<byte[]> atcInfoString;
    private bool atcUpdateTailNumber;
    private bool atcUpdateAirlineAndFlightNumbe;
    private bool atcUpdateAircraftTypeAndModel;
    private bool atcUpdateAircraftTitle;
    private Offset<byte> AIOptGroundRangeInAir;
    private Offset<byte> AIOptGroundRangeOnGround;
    private Offset<byte> AIOptGroundIDString;
    private Offset<byte> AIOptGroundPreferActive;
    private Offset<byte> AIOptAirIDString;
    private Offset<byte> AIOptAirRange;
    private byte classInstance;

    internal AITrafficServices(byte ClassInstance)
    {
      this.classInstance = ClassInstance;
      this.AISystemGroup += this.classInstance.ToString();
      this.AIWriteOptionsGroupAirborne += this.classInstance.ToString();
      this.AIWriteOptionsGroupGround += this.classInstance.ToString();
      this.AISlotGroup += this.classInstance.ToString();
      this.AITrafficIDStringGroupWrite += this.classInstance.ToString();
      this.AITrafficIDStringGroupRead += this.classInstance.ToString();
      this.AITrafficIDStringGroupTest += this.classInstance.ToString();
      this.AITCASWrite += this.classInstance.ToString();
      this.slotSizeGround = new Offset<short>(this.AISystemGroup, 57344);
      this.slotSizeAir = new Offset<short>(this.AISystemGroup, 61440);
      this.slotsUsedGround = new Offset<short>(this.AISystemGroup, 57348);
      this.slotsUsedAir = new Offset<short>(this.AISystemGroup, 61444);
      this.slotChangesGround = new Offset<short>(this.AISystemGroup, 57350);
      this.slotChangesAir = new Offset<short>(this.AISystemGroup, 61446);
      this.slotChangeArrayGround = new Offset<byte[]>(this.AISystemGroup, 57352, 96);
      this.slotChangeArrayAir = new Offset<byte[]>(this.AISystemGroup, 61448, 96);
      this.playerAlt = new Offset<long>(this.AISystemGroup, 1392);
      this.playerLat = new Offset<long>(this.AISystemGroup, 1376);
      this.playerLon = new Offset<long>(this.AISystemGroup, 1384);
      this.playerMagVar = new Offset<short>(this.AISystemGroup, 672);
      this.AIOptGroundRangeInAir = new Offset<byte>(this.AIWriteOptionsGroupGround, 57448, true);
      this.AIOptGroundRangeOnGround = new Offset<byte>(this.AIWriteOptionsGroupGround, 57449, true);
      this.AIOptGroundIDString = new Offset<byte>(this.AIWriteOptionsGroupGround, 57450, true);
      this.AIOptGroundPreferActive = new Offset<byte>(this.AIWriteOptionsGroupGround, 57451, true);
      this.AIOptAirRange = new Offset<byte>(this.AIWriteOptionsGroupAirborne, 61544, true);
      this.AIOptAirIDString = new Offset<byte>(this.AIWriteOptionsGroupAirborne, 61546, true);
      this.atcInfoCommand = new Offset<int>(this.AITrafficIDStringGroupWrite, 53252, true);
      this.atcInfoTimeStampInit = new Offset<int>(this.AITrafficIDStringGroupWrite, 53256);
      this.atcInfoAiID = new Offset<int>(this.AITrafficIDStringGroupWrite, 53260, true);
      this.atcInfoSignature = new Offset<int>(this.AITrafficIDStringGroupWrite, 53248, true);
      this.atcInfoTimeStamp = new Offset<int>(this.AITrafficIDStringGroupTest, 53256);
      this.atcInfoString = new Offset<byte[]>(this.AITrafficIDStringGroupRead, 53264, 48);
    }

    public List<AIPlaneInfo> GroundTraffic
    {
      get
      {
        return this.aiGround;
      }
    }

    public List<AIPlaneInfo> AirbourneTraffic
    {
      get
      {
        return this.aiAirborne;
      }
    }

    public List<AIPlaneInfo> AllTraffic
    {
      get
      {
        return this.aiAll;
      }
    }

    public AIPlaneInfo GetPlaneInfoByID(int ID)
    {
      if (this.aiAllIDs.Contains(ID))
        return this.aiAll[this.aiAllIDs.IndexOf(ID)];
      return (AIPlaneInfo) null;
    }

    public void UpdateExtendedPlaneIndentifiers(bool TailNumber, bool AirlineAndFlightNumber, bool AircraftTypeAndModel, bool AircraftTitle)
    {
      this.atcUpdateAircraftTitle = AircraftTitle;
      this.atcUpdateAircraftTypeAndModel = AircraftTypeAndModel;
      this.atcUpdateAirlineAndFlightNumbe = AirlineAndFlightNumber;
      this.atcUpdateTailNumber = TailNumber;
    }

    public void RefreshAITrafficInformation()
    {
      this.RefreshAITrafficInformation(true, true);
    }

    public void RefreshAITrafficInformation(bool UpdateGroundTraffic, bool UpdateAirbourneTraffic)
    {
      FSUIPCConnection.Process(this.classInstance, this.AISystemGroup);
      List<int> processedPlaneIDs = new List<int>();
      this.aiArrivalRunwaysInUse.Clear();
      this.aiDepartureRunwaysInUse.Clear();
      if (UpdateGroundTraffic)
      {
        List<Offset<byte[]>> offsetList1 = new List<Offset<byte[]>>();
        List<Offset<byte[]>> offsetList2 = new List<Offset<byte[]>>();
        for (int index = 0; index < (int) this.slotsUsedGround.Value; ++index)
        {
          Offset<byte[]> offset1 = new Offset<byte[]>(this.AISlotGroup, 57472 + 40 * index, 40);
          Offset<byte[]> offset2 = new Offset<byte[]>(this.AISlotGroup, 53312 + 20 * index, 20);
          offsetList1.Add(offset1);
          offsetList2.Add(offset2);
        }
        if (offsetList1.Count > 0)
        {
          FSUIPCConnection.Process(this.classInstance, this.AISlotGroup);
          FSUIPCConnection.DeleteGroup(this.AISlotGroup);
          processedPlaneIDs = new List<int>();
          for (int index = 0; index < offsetList1.Count; ++index)
          {
            AIPlaneInfo planeInfo = this.updatePlaneInfo(offsetList1[index].Value, offsetList2[index].Value, this.aiGround, this.aiGroundIDs, processedPlaneIDs);
            if (planeInfo != null)
              this.addToRunwaysInUse(planeInfo);
          }
        }
        List<int> intList = new List<int>();
        foreach (int aiGroundId in this.aiGroundIDs)
        {
          if (!processedPlaneIDs.Contains(aiGroundId))
            intList.Add(this.aiGroundIDs.IndexOf(aiGroundId));
        }
        intList.Sort();
        intList.Reverse();
        foreach (int index in intList)
        {
          AIPlaneInfo aiPlaneInfo = this.aiGround[index];
          aiPlaneInfo.id = 0;
          this.aiGround.Remove(aiPlaneInfo);
        }
        this.aiGround.Sort(new Comparison<AIPlaneInfo>(this.comparePlaneDistance));
        this.aiGroundIDs.Clear();
        foreach (AIPlaneInfo aiPlaneInfo in this.aiGround)
          this.aiGroundIDs.Add(aiPlaneInfo.id);
      }
      if (UpdateAirbourneTraffic)
      {
        List<Offset<byte[]>> offsetList1 = new List<Offset<byte[]>>();
        List<Offset<byte[]>> offsetList2 = new List<Offset<byte[]>>();
        for (int index = 0; index < (int) this.slotsUsedAir.Value; ++index)
        {
          Offset<byte[]> offset1 = new Offset<byte[]>(this.AISlotGroup, 61568 + 40 * index, 40);
          Offset<byte[]> offset2 = new Offset<byte[]>(this.AISlotGroup, 55360 + 20 * index, 20);
          offsetList1.Add(offset1);
          offsetList2.Add(offset2);
        }
        if (offsetList1.Count > 0)
        {
          FSUIPCConnection.Process(this.classInstance, this.AISlotGroup);
          FSUIPCConnection.DeleteGroup(this.AISlotGroup);
          processedPlaneIDs = new List<int>();
          for (int index = 0; index < offsetList1.Count; ++index)
          {
            AIPlaneInfo planeInfo = this.updatePlaneInfo(offsetList1[index].Value, offsetList2[index].Value, this.aiAirborne, this.aiAirbornIDs, processedPlaneIDs);
            if (planeInfo != null)
              this.addToRunwaysInUse(planeInfo);
          }
        }
        List<int> intList = new List<int>();
        foreach (int aiAirbornId in this.aiAirbornIDs)
        {
          if (!processedPlaneIDs.Contains(aiAirbornId))
            intList.Add(this.aiAirbornIDs.IndexOf(aiAirbornId));
        }
        intList.Sort();
        intList.Reverse();
        foreach (int index in intList)
        {
          AIPlaneInfo aiPlaneInfo = this.aiAirborne[index];
          aiPlaneInfo.id = 0;
          this.aiAirborne.Remove(aiPlaneInfo);
        }
        this.aiAirborne.Sort(new Comparison<AIPlaneInfo>(this.comparePlaneDistance));
        this.aiAirbornIDs.Clear();
        foreach (AIPlaneInfo aiPlaneInfo in this.aiAirborne)
          this.aiAirbornIDs.Add(aiPlaneInfo.id);
      }
      this.aiAll.Clear();
      this.aiAll.AddRange((IEnumerable<AIPlaneInfo>) this.aiGround);
      this.aiAll.AddRange((IEnumerable<AIPlaneInfo>) this.aiAirborne);
      if (this.aiAll.Count <= 0)
        return;
      this.aiAll.Sort(new Comparison<AIPlaneInfo>(this.comparePlaneDistance));
      this.aiAllIDs.Clear();
      foreach (AIPlaneInfo aiPlaneInfo in this.aiAll)
        this.aiAllIDs.Add(aiPlaneInfo.id);
    }

    private void addToRunwaysInUse(AIPlaneInfo planeInfo)
    {
      switch (planeInfo.State)
      {
        case AITrafficStatus.Initialising:
        case AITrafficStatus.FilingFlightPlan:
        case AITrafficStatus.ObtainingClearance:
        case AITrafficStatus.PushingBack:
        case AITrafficStatus.PushingBackTurn:
        case AITrafficStatus.StartingUp:
        case AITrafficStatus.ReadyForTaxi:
        case AITrafficStatus.TaxiingOut:
        case AITrafficStatus.ReadyForTakeOff:
        case AITrafficStatus.TakingOff:
        case AITrafficStatus.Departing:
          if ((int) planeInfo.RunwayAssigned.Number <= 0)
            break;
          List<FSRunway> fsRunwayList1;
          if (this.aiDepartureRunwaysInUse.ContainsKey(planeInfo.departureICAO))
          {
            fsRunwayList1 = this.aiDepartureRunwaysInUse[planeInfo.departureICAO];
          }
          else
          {
            fsRunwayList1 = new List<FSRunway>();
            this.aiDepartureRunwaysInUse.Add(planeInfo.departureICAO, fsRunwayList1);
          }
          if (fsRunwayList1.Contains(planeInfo.RunwayAssigned))
            break;
          fsRunwayList1.Add(planeInfo.RunwayAssigned);
          break;
        case AITrafficStatus.Enroute:
        case AITrafficStatus.InThePattern:
        case AITrafficStatus.Landing:
        case AITrafficStatus.RollingOut:
        case AITrafficStatus.GoingAround:
        case AITrafficStatus.TaxiingIn:
        case AITrafficStatus.ShuttingDown:
          if ((int) planeInfo.RunwayAssigned.Number <= 0)
            break;
          List<FSRunway> fsRunwayList2;
          if (this.aiArrivalRunwaysInUse.ContainsKey(planeInfo.destinationICAO))
          {
            fsRunwayList2 = this.aiArrivalRunwaysInUse[planeInfo.destinationICAO];
          }
          else
          {
            fsRunwayList2 = new List<FSRunway>();
            this.aiArrivalRunwaysInUse.Add(planeInfo.destinationICAO, fsRunwayList2);
          }
          if (fsRunwayList2.Contains(planeInfo.RunwayAssigned))
            break;
          fsRunwayList2.Add(planeInfo.RunwayAssigned);
          break;
      }
    }

    public void ApplyFilter(bool FilterGroundTraffic, bool FilterAirbourneTraffic, double StartBearing, double EndBearing, double? MinAltitude, double? MaxAltitude, double? WithinDistance)
    {
      if (FilterGroundTraffic)
      {
        int index = 0;
        int count = this.aiGround.Count;
        while (index < count)
        {
          AIPlaneInfo planeInfo = this.aiGround[index];
          if (!this.PlaneIsWithin(planeInfo, StartBearing, EndBearing, MinAltitude, MaxAltitude, WithinDistance))
          {
            this.aiGroundIDs.Remove(planeInfo.id);
            this.aiGround.Remove(planeInfo);
            this.aiAllIDs.Remove(planeInfo.id);
            this.aiAll.Remove(planeInfo);
            --count;
          }
          else
            ++index;
        }
      }
      if (!FilterAirbourneTraffic)
        return;
      int index1 = 0;
      int count1 = this.aiAirborne.Count;
      while (index1 < count1)
      {
        AIPlaneInfo planeInfo = this.aiAirborne[index1];
        if (!this.PlaneIsWithin(planeInfo, StartBearing, EndBearing, MinAltitude, MaxAltitude, WithinDistance))
        {
          this.aiAirbornIDs.Remove(planeInfo.id);
          this.aiAirborne.Remove(planeInfo);
          this.aiAllIDs.Remove(planeInfo.id);
          this.aiAll.Remove(planeInfo);
          --count1;
        }
        else
          ++index1;
      }
    }

    private bool PlaneIsWithin(AIPlaneInfo planeInfo, double StartBearing, double EndBearing, double? MinAltitude, double? MaxAltitude, double? WithinDistance)
    {
      bool flag = false;
      if (WithinDistance.HasValue)
      {
        double distanceNm = planeInfo.DistanceNM;
        double? nullable = WithinDistance;
        flag = distanceNm > nullable.GetValueOrDefault() && nullable.HasValue;
      }
      if (!flag && MinAltitude.HasValue)
      {
        double altitudeFeet = planeInfo.AltitudeFeet;
        double? nullable = MinAltitude;
        flag = altitudeFeet < nullable.GetValueOrDefault() && nullable.HasValue;
      }
      if (!flag && MaxAltitude.HasValue)
      {
        double altitudeFeet = planeInfo.AltitudeFeet;
        double? nullable = MaxAltitude;
        flag = altitudeFeet > nullable.GetValueOrDefault() && nullable.HasValue;
      }
      if (!flag)
        flag = !this.bearingBetween(planeInfo.BearingTo, StartBearing, EndBearing);
      return !flag;
    }

    private int comparePlaneDistance(AIPlaneInfo plane1, AIPlaneInfo plane2)
    {
      if (plane1.DistanceFeet == plane2.DistanceFeet)
        return 0;
      return plane1.DistanceFeet < plane2.DistanceFeet ? -1 : 1;
    }

    private AIPlaneInfo updatePlaneInfo(byte[] slot, byte[] slotExtra, List<AIPlaneInfo> aiPlaneList, List<int> aiPlaneIDs, List<int> processedPlaneIDs)
    {
      AIPlaneInfo planeInfo = (AIPlaneInfo) null;
      int int32 = BitConverter.ToInt32(slot, 0);
      if (int32 != 0)
      {
        if (aiPlaneIDs.Contains(int32))
        {
          planeInfo = aiPlaneList[aiPlaneIDs.IndexOf(int32)];
        }
        else
        {
          planeInfo = new AIPlaneInfo();
          planeInfo.id = int32;
          aiPlaneList.Add(planeInfo);
          aiPlaneIDs.Add(int32);
        }
        planeInfo.updateFromByteArrays(slot, slotExtra, this.playerAlt.Value, this.playerLat.Value, this.playerLon.Value, this.playerMagVar.Value);
        this.getATCInfo(planeInfo, this.atcUpdateTailNumber, this.atcUpdateAirlineAndFlightNumbe, this.atcUpdateAircraftTypeAndModel, this.atcUpdateAircraftTitle);
        processedPlaneIDs.Add(int32);
      }
      return planeInfo;
    }

    internal void getATCInfo(AIPlaneInfo planeInfo, bool TailNumber, bool AirlineAndFlightNumber, bool AircraftTypeAndModel, bool AircraftTitle)
    {
      int id = planeInfo.id;
      if (TailNumber && planeInfo.tailNumber.Length == 0)
        planeInfo.tailNumber = this.getATCString(id, 1);
      if (AirlineAndFlightNumber)
      {
        bool flag = false;
        if (planeInfo.State == AITrafficStatus.Initialising || planeInfo.State == AITrafficStatus.Sleeping)
          planeInfo.flightNumber = "";
        else if (planeInfo.flightNumber.Length == 0)
          flag = true;
        if (planeInfo.airline.Length == 0)
          flag = true;
        if (flag)
        {
          string atcString = this.getATCString(id, 2);
          int length = atcString.LastIndexOf(' ');
          if (length > 0)
            planeInfo.airline = atcString.Substring(0, length);
          planeInfo.flightNumber = atcString.Substring(length + 1);
          if (planeInfo.airline.Length == 0)
            planeInfo.airline = "Private (GA)";
        }
      }
      if (AircraftTypeAndModel && planeInfo.aircraftModel.Length == 0)
      {
        string atcString = this.getATCString(id, 3);
        int length = atcString.IndexOf('~');
        planeInfo.aircraftType = atcString.Substring(0, length);
        planeInfo.aircraftModel = atcString.Substring(length + 1);
        if (planeInfo.aircraftType.Length == 0)
          planeInfo.aircraftType = "n/a";
        if (planeInfo.aircraftModel.Trim().Length == 0)
          planeInfo.aircraftModel = "n/a";
      }
      if (!AircraftTitle || planeInfo.aircraftTitle.Trim().Length != 0)
        return;
      planeInfo.aircraftTitle = this.getATCString(id, 4);
    }

    private bool bearingBetween(double TestBearing, double StartBearing, double EndBearing)
    {
      return StartBearing > EndBearing ? TestBearing >= StartBearing || TestBearing <= EndBearing : TestBearing >= StartBearing && TestBearing <= EndBearing;
    }

    internal void cleanup()
    {
      FSUIPCConnection.DeleteGroup(this.AISystemGroup);
      FSUIPCConnection.DeleteGroup(this.AIWriteOptionsGroupAirborne);
      FSUIPCConnection.DeleteGroup(this.AIWriteOptionsGroupGround);
    }

    public List<FSRunway> GetArrivalRunwaysInUse(string AirportICAOCode)
    {
      List<FSRunway> fsRunwayList = new List<FSRunway>();
      if (this.aiArrivalRunwaysInUse.ContainsKey(AirportICAOCode))
        fsRunwayList = this.aiArrivalRunwaysInUse[AirportICAOCode];
      return fsRunwayList;
    }

    public List<FSRunway> GetDepartureRunwaysInUse(string AirportICAOCode)
    {
      List<FSRunway> fsRunwayList = new List<FSRunway>();
      if (this.aiDepartureRunwaysInUse.ContainsKey(AirportICAOCode))
        fsRunwayList = this.aiDepartureRunwaysInUse[AirportICAOCode];
      return fsRunwayList;
    }

    private string getATCString(int ID, int Command)
    {
      int num = 4112;
      this.atcInfoCommand.Value = Command;
      this.atcInfoAiID.Value = ID;
      this.atcInfoSignature.Value = num;
      FSUIPCConnection.Process(this.classInstance, this.AITrafficIDStringGroupWrite);
      Thread.Sleep(10);
      FSUIPCConnection.Process(this.classInstance, this.AITrafficIDStringGroupTest);
      while (this.atcInfoTimeStampInit.Value == this.atcInfoTimeStamp.Value)
      {
        Thread.Sleep(10);
        FSUIPCConnection.Process(this.classInstance, this.AITrafficIDStringGroupTest);
      }
      FSUIPCConnection.Process(this.classInstance, this.AITrafficIDStringGroupRead);
      string str1 = Encoding.ASCII.GetString(this.atcInfoString.Value);
      string str2;
      if (Command == 3)
      {
        int length = str1.IndexOf(char.MinValue);
        str2 = str1.Substring(0, length) + "~" + str1.Substring(length + 1, str1.IndexOf(char.MinValue, length + 1) - length - 1);
      }
      else
        str2 = str1.Substring(0, str1.IndexOf(char.MinValue));
      return str2;
    }

    public void OverrideAirborneTrafficINISettings(ATCIdentifier? ATCId, byte? RangeInNM)
    {
      bool flag = false;
      if (ATCId.HasValue)
      {
        this.AIOptAirIDString.Value = (byte) ATCId.Value;
        flag = true;
      }
      byte? nullable = RangeInNM;
      if ((nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
      {
        this.AIOptAirRange.Value = RangeInNM.Value;
        flag = true;
      }
      if (!flag)
        return;
      FSUIPCConnection.Process(this.classInstance, this.AIWriteOptionsGroupAirborne);
    }

    public void OverrideGroundTrafficINISettings(ATCIdentifier? ATCId, bool? PreferActive, byte? RangeInAirInNM, byte? RangeOnGroundInNM)
    {
      bool flag = false;
      if (ATCId.HasValue)
      {
        this.AIOptGroundIDString.Value = (byte) ATCId.Value;
        flag = true;
      }
      if (PreferActive.HasValue)
      {
        this.AIOptGroundPreferActive.Value = PreferActive.Value ? (byte) 1 : (byte) 0;
        flag = true;
      }
      byte? nullable1 = RangeInAirInNM;
      if ((nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?()).HasValue)
      {
        this.AIOptGroundRangeInAir.Value = RangeInAirInNM.Value;
        flag = true;
      }
      byte? nullable2 = RangeOnGroundInNM;
      if ((nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?()).HasValue)
      {
        this.AIOptGroundRangeOnGround.Value = RangeOnGroundInNM.Value;
        flag = true;
      }
      if (!flag)
        return;
      FSUIPCConnection.Process(this.classInstance, this.AIWriteOptionsGroupGround);
    }

    public void AddTCASTarget(int ID, string ATCIdentifier, AITrafficStatus State, FsLatitude Latitude, FsLongitude Longitude, double AltitudeFeet, double HeadingDegreesTrue, short GroundSpeedKnots, short VerticalSpeedFeet, short Com1)
    {
      this.aiTCASTargets.Add(new AIPlaneInfo()
      {
        id = ID,
        idATC = ATCIdentifier,
        lat = (float) Latitude.DecimalDegrees,
        lon = (float) Longitude.DecimalDegrees,
        alt = (float) AltitudeFeet,
        hdg = (ushort) (HeadingDegreesTrue * 360.0),
        gs = GroundSpeedKnots,
        vs = VerticalSpeedFeet,
        com1 = Com1,
        state = (byte) State
      });
    }

    public void SendTCASTargets()
    {
      if (this.aiTCASTargets.Count <= 0)
        return;
      foreach (AIPlaneInfo aiTcasTarget in this.aiTCASTargets)
        new Offset<byte[]>(this.AITCASWrite, 8064, 40, true).Value = aiTcasTarget.getTCASByteArray();
      FSUIPCConnection.Process(this.classInstance, this.AITCASWrite);
      FSUIPCConnection.DeleteGroup(this.AITCASWrite);
      this.aiTCASTargets.Clear();
    }
  }
}
