// Decompiled with JetBrains decompiler
// Type: FSUIPC.AIPlaneInfo
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Text;

namespace FSUIPC
{
  public class AIPlaneInfo
  {
    internal FSRunway runway = new FSRunway();
    internal string tailNumber = "";
    internal string airline = "";
    internal string flightNumber = "";
    internal string aircraftType = "";
    internal string aircraftModel = "";
    internal string aircraftTitle = "";
    internal int id;
    internal float lat;
    internal float lon;
    internal float alt;
    internal ushort hdg;
    internal short gs;
    internal short vs;
    internal string idATC;
    internal byte state;
    internal short com1;
    internal int key;
    internal short file;
    internal short pitch;
    internal string departureICAO;
    internal string destinationICAO;
    internal short bank;
    internal double distance;
    internal double altDiff;
    internal double bearingTo;
    internal double magVar;
    internal byte gateName;
    internal short gateNumber;
    internal byte gateType;

    internal AIPlaneInfo()
    {
    }

    public string TailNumber
    {
      get
      {
        return this.tailNumber;
      }
    }

    public string Airline
    {
      get
      {
        return this.airline;
      }
    }

    public string FlightNumber
    {
      get
      {
        return this.flightNumber;
      }
    }

    public string AircraftType
    {
      get
      {
        return this.aircraftType;
      }
    }

    public string AircraftModel
    {
      get
      {
        return this.aircraftModel;
      }
    }

    public string AircraftTitle
    {
      get
      {
        return this.aircraftTitle;
      }
    }

    public double AltitudeDifferenceFeet
    {
      get
      {
        return this.altDiff;
      }
    }

    public double AltitudeDifferenceMetres
    {
      get
      {
        return this.altDiff / 3.2808;
      }
    }

    public double DistanceFeet
    {
      get
      {
        return this.distance;
      }
    }

    public double DistanceNM
    {
      get
      {
        return this.distance / 6076.1155;
      }
    }

    public double DistanceMetres
    {
      get
      {
        return this.distance / 3.2808;
      }
    }

    public double BearingToMagnetic
    {
      get
      {
        double num = this.bearingTo - this.magVar;
        if (num < 0.0)
          num += 360.0;
        return num;
      }
    }

    public double BearingTo
    {
      get
      {
        return this.bearingTo;
      }
    }

    public double BearingFrom
    {
      get
      {
        double num = 180.0 + this.bearingTo;
        if (num >= 360.0)
          num -= 360.0;
        if (num < 0.0)
          num += 360.0;
        return num;
      }
    }

    public double BearingFromMag
    {
      get
      {
        double num = this.BearingFrom - this.magVar;
        if (num < 0.0)
          num += 360.0;
        return num;
      }
    }

    public int ID
    {
      get
      {
        return this.id;
      }
    }

    public FsLatLonPoint Location
    {
      get
      {
        return new FsLatLonPoint(new FsLatitude((double) this.lat), new FsLongitude((double) this.lon));
      }
    }

    public double AltitudeFeet
    {
      get
      {
        return (double) this.alt;
      }
    }

    public double AltitudeMetres
    {
      get
      {
        return (double) this.alt / 3.2808;
      }
    }

    public ushort Heading
    {
      get
      {
        return this.hdg;
      }
    }

    public double HeadingDegrees
    {
      get
      {
        return (double) this.hdg / 65536.0 * 360.0;
      }
    }

    public double HeadingDegreesMag
    {
      get
      {
        double num = this.HeadingDegrees - this.magVar;
        if (num < 0.0)
          num += 360.0;
        return num;
      }
    }

    public short GroundSpeed
    {
      get
      {
        return this.gs;
      }
    }

    public short VirticalSpeedFeet
    {
      get
      {
        return this.vs;
      }
    }

    public short VirticalSpeedMetres
    {
      get
      {
        return (short) ((double) this.vs / 3.2808);
      }
    }

    public string ATCIdentifier
    {
      get
      {
        return this.idATC;
      }
    }

    public AITrafficStatus State
    {
      get
      {
        return (AITrafficStatus) this.state;
      }
    }

    public short Com1
    {
      get
      {
        return this.com1;
      }
    }

    public string Com1String
    {
      get
      {
        string str = this.com1.ToString("X");
        return "1" + str.Substring(0, 2) + "." + str.Substring(2, 2);
      }
    }

    public int Key
    {
      get
      {
        return this.key;
      }
    }

    public int File
    {
      get
      {
        return (int) this.file;
      }
    }

    public double PitchDegrees
    {
      get
      {
        return (double) this.pitch / 65536.0 * 360.0;
      }
    }

    public short Pitch
    {
      get
      {
        return this.pitch;
      }
    }

    public double BankDegrees
    {
      get
      {
        return (double) this.bank / 65536.0 * 360.0;
      }
    }

    public short Bank
    {
      get
      {
        return this.bank;
      }
    }

    public string DepartureICAO
    {
      get
      {
        return this.departureICAO;
      }
    }

    public string DestinationICAO
    {
      get
      {
        return this.destinationICAO;
      }
    }

    public FSRunway RunwayAssigned
    {
      get
      {
        return this.runway;
      }
    }

    public byte GateName
    {
      get
      {
        return this.gateName;
      }
    }

    public short GateNumber
    {
      get
      {
        return this.gateNumber;
      }
    }

    public byte GateType
    {
      get
      {
        return this.gateType;
      }
    }

    public void GetExtendedPlaneIndentifiers(bool TailNumber, bool AirlineAndFlightNumber, bool AircraftTypeAndModel, bool AircraftTitle)
    {
      FSUIPCConnection.AITrafficServices.getATCInfo(this, TailNumber, AirlineAndFlightNumber, AircraftTypeAndModel, AircraftTitle);
    }

    internal void updateFromByteArrays(byte[] slot, byte[] slotExtra, long playerAlt, long playerLat, long playerLon, short playerMagVar)
    {
      this.lat = BitConverter.ToSingle(slot, 4);
      this.lon = BitConverter.ToSingle(slot, 8);
      this.alt = BitConverter.ToSingle(slot, 12);
      this.hdg = BitConverter.ToUInt16(slot, 16);
      this.gs = BitConverter.ToInt16(slot, 18);
      this.vs = BitConverter.ToInt16(slot, 20);
      Encoding ascii = Encoding.ASCII;
      string str = ascii.GetString(slot, 22, 15);
      this.idATC = str.Substring(0, str.IndexOf(char.MinValue));
      this.state = slot[37];
      this.com1 = BitConverter.ToInt16(slot, 38);
      if (FSUIPCConnection.FlightSimVersionConnected == FlightSim.FSX)
      {
        this.gateName = slotExtra[0];
        this.gateType = slotExtra[1];
        this.gateNumber = BitConverter.ToInt16(slotExtra, 2);
      }
      else
      {
        this.key = BitConverter.ToInt32(slotExtra, 0);
        this.file = BitConverter.ToInt16(slotExtra, 4);
      }
      this.pitch = BitConverter.ToInt16(slotExtra, 6);
      this.departureICAO = ascii.GetString(slotExtra, 8, 4);
      this.destinationICAO = ascii.GetString(slotExtra, 12, 4);
      this.runway = new FSRunway()
      {
        Number = slotExtra[16],
        Designator = (FSRunwayDesignator) slotExtra[17]
      };
      this.bank = BitConverter.ToInt16(slotExtra, 18);
      this.altDiff = (double) this.alt - (double) playerAlt / 65536.0 / 65536.0 * 3.2808;
      FsLatLonPoint fsLatLonPoint = new FsLatLonPoint(new FsLatitude(playerLat), new FsLongitude(playerLon));
      FsLatLonPoint Point = new FsLatLonPoint(new FsLatitude((double) this.lat), new FsLongitude((double) this.lon));
      this.distance = fsLatLonPoint.DistanceFromInFeet(Point);
      this.bearingTo = fsLatLonPoint.BearingTo(Point);
      this.magVar = (double) playerMagVar * 360.0 / 65536.0;
    }

    internal byte[] getTCASByteArray()
    {
      byte[] numArray = new byte[40];
      BitConverter.GetBytes(this.id).CopyTo((Array) numArray, 0);
      BitConverter.GetBytes(this.lat).CopyTo((Array) numArray, 4);
      BitConverter.GetBytes(this.lon).CopyTo((Array) numArray, 8);
      BitConverter.GetBytes(this.alt).CopyTo((Array) numArray, 12);
      BitConverter.GetBytes(this.hdg).CopyTo((Array) numArray, 16);
      BitConverter.GetBytes(this.gs).CopyTo((Array) numArray, 18);
      BitConverter.GetBytes(this.vs).CopyTo((Array) numArray, 20);
      string s = this.ATCIdentifier;
      if (s.Length > 14)
        s = s.Substring(0, 14);
      Encoding.ASCII.GetBytes(s).CopyTo((Array) numArray, 22);
      BitConverter.GetBytes((short) this.state).CopyTo((Array) numArray, 37);
      BitConverter.GetBytes(this.com1).CopyTo((Array) numArray, 38);
      return numArray;
    }
  }
}
