// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsLongitude
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Serializable]
  public struct FsLongitude
  {
    private double pos;

    public FsLongitude(long FSUnits)
    {
      this = new FsLongitude((double) FSUnits * 360.0 / 1.84467440737096E+19);
    }

    public FsLongitude(int FSUnits)
    {
      this = new FsLongitude((double) FSUnits * 360.0 / 4294967296.0);
    }

    public FsLongitude(double DecimalDegrees)
    {
      this.pos = DecimalDegrees;
      while (this.pos < -180.0 || this.pos > 180.0)
      {
        if (this.pos > 180.0)
          this.pos -= 360.0;
        if (this.pos < -180.0)
          this.pos += 360.0;
      }
    }

    public FsLongitude(int Degrees, double DecimalMinutes)
    {
      this = new FsLongitude((double) Degrees + DecimalMinutes / 60.0);
    }

    public FsLongitude(int Degrees, int Minutes, double DecimalSeconds)
    {
      this = new FsLongitude((double) Degrees + (double) Minutes / 60.0 + DecimalSeconds / 3600.0);
    }

    public long ToFSUnits8()
    {
      return (long) (this.pos * 1.84467440737096E+19 / 360.0);
    }

    public int ToFSUnits4()
    {
      return (int) (this.pos * 4294967296.0 / 360.0);
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
        return this.pos + 180.0;
      }
    }

    public override string ToString()
    {
      return this.ToString(true, "m", (short) 2);
    }

    public string ToString(bool HemisphereAsText, string DetailLevel, short DecimalPlaces)
    {
      string str1 = "";
      string str2 = !HemisphereAsText ? str1 + (this.pos < 0.0 ? "-" : "") : str1 + (this.pos < 0.0 ? "W" : "E");
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

    public FsLongitude Add(FsLongitudeSpan Distance)
    {
      return new FsLongitude(this.pos + Distance.DecimalDegrees);
    }

    public FsLongitude Subtract(FsLongitudeSpan Distance)
    {
      return new FsLongitude(this.pos - Distance.DecimalDegrees);
    }

    public FsLongitude AddDegrees(double Degrees)
    {
      return new FsLongitude(this.pos + Degrees);
    }

    public FsLongitude AddMinutes(double Minutes)
    {
      return new FsLongitude(this.pos + Minutes / 60.0);
    }

    public FsLongitude AddSeconds(double Seconds)
    {
      return new FsLongitude(this.pos + Seconds / 3600.0);
    }
  }
}
