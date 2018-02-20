// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsLatitude
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Serializable]
  public struct FsLatitude
  {
    private double pos;

    public FsLatitude(long FSUnits)
    {
      this = new FsLatitude((double) FSUnits * 90.0 / 4.2957189152768E+16);
    }

    public FsLatitude(int FSUnits)
    {
      this = new FsLatitude((double) FSUnits * 90.0 / 10001750.0);
    }

    public FsLatitude(double DecimalDegrees)
    {
      this.pos = DecimalDegrees;
      while (this.pos > 90.0 || this.pos < -90.0)
      {
        if (this.pos > 90.0)
          this.pos = 180.0 - this.pos;
        if (this.pos < -90.0)
          this.pos = -180.0 - this.pos;
      }
    }

    public FsLatitude(int Degrees, double DecimalMinutes)
    {
      this = new FsLatitude((double) Degrees + DecimalMinutes / 60.0);
    }

    public FsLatitude(int Degrees, int Minutes, double DecimalSeconds)
    {
      this = new FsLatitude((double) Degrees + (double) Minutes / 60.0 + DecimalSeconds / 3600.0);
    }

    public long ToFSUnits8()
    {
      return (long) (this.pos * 4.2957189152768E+16 / 90.0);
    }

    public int ToFSUnits4()
    {
      return (int) (this.pos * 10001750.0 / 90.0);
    }

    public int Degree
    {
      get
      {
        return (int) Math.Truncate(this.DecimalDegrees);
      }
    }

    public int Minute
    {
      get
      {
        return (int) Math.Truncate(this.DecimalMinutes);
      }
    }

    public int Second
    {
      get
      {
        return (int) Math.Truncate(this.DecimalSeconds);
      }
    }

    public double DecimalDegrees
    {
      get
      {
        return this.pos;
      }
    }

    public double DecimalMinutes
    {
      get
      {
        double decimalDegrees = this.DecimalDegrees;
        return (decimalDegrees - Math.Truncate(decimalDegrees)) * 60.0;
      }
    }

    public double DecimalSeconds
    {
      get
      {
        double decimalMinutes = this.DecimalMinutes;
        return (decimalMinutes - Math.Truncate(decimalMinutes)) * 60.0;
      }
    }

    internal double UDegrees
    {
      get
      {
        return this.pos + 90.0;
      }
    }

    public override string ToString()
    {
      return this.ToString(true, "m", (short) 2);
    }

    public string ToString(bool HemisphereAsText, string DetailLevel, short DecimalPlaces)
    {
      string str1 = "";
      string str2 = !HemisphereAsText ? str1 + (this.pos < 0.0 ? "-" : "") : str1 + (this.pos < 0.0 ? "S" : "N");
      string str3;
      switch (DetailLevel)
      {
        case "m":
          str3 = str2 + Math.Abs(this.Degree).ToString("000") + "° " + Math.Abs(this.DecimalMinutes).ToString("00" + ((int) DecimalPlaces > 0 ? "." + new string('0', (int) DecimalPlaces) : "")) + "'";
          break;
        case "s":
          str3 = str2 + Math.Abs(this.Degree).ToString("000") + "° " + Math.Abs(this.Minute).ToString("00") + "' " + Math.Abs(this.DecimalSeconds).ToString("00" + ((int) DecimalPlaces > 0 ? "." + new string('0', (int) DecimalPlaces) : "")) + "\"";
          break;
        default:
          str3 = str2 + Math.Abs(this.DecimalDegrees).ToString("000" + ((int) DecimalPlaces > 0 ? "." + new string('0', (int) DecimalPlaces) : "")) + "*";
          break;
      }
      return str3;
    }

    public FsLatitude Add(FsLatitudeSpan Distance)
    {
      return new FsLatitude(this.pos + Distance.DecimalDegrees);
    }

    public FsLatitude Subtract(FsLatitudeSpan Distance)
    {
      return new FsLatitude(this.pos - Distance.DecimalDegrees);
    }

    public FsLatitude AddDegrees(double Degrees)
    {
      return new FsLatitude(this.pos + Degrees);
    }

    public FsLatitude AddMinutes(double Minutes)
    {
      return new FsLatitude(this.pos + Minutes / 60.0);
    }

    public FsLatitude AddSeconds(double Seconds)
    {
      return new FsLatitude(this.pos + Seconds / 3600.0);
    }
  }
}
