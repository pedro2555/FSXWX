// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsLatitudeSpan
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  public struct FsLatitudeSpan
  {
    private double span;

    public FsLatitudeSpan(double DecimalDegrees)
    {
      this.span = DecimalDegrees;
    }

    public FsLatitudeSpan(int Degrees, double DecimalMinutes)
    {
      this = new FsLatitudeSpan((double) Degrees + DecimalMinutes / 60.0);
    }

    public FsLatitudeSpan(int Degrees, int Minutes, double DecimalSeconds)
    {
      this = new FsLatitudeSpan((double) Degrees + (double) Minutes / 60.0 + DecimalSeconds / 3600.0);
    }

    public static FsLatitudeSpan FromFeet(double Feet)
    {
      return new FsLatitudeSpan(Feet / 364601.4567);
    }

    public static FsLatitudeSpan FromNauticalMiles(double NauticalMiles)
    {
      return FsLatitudeSpan.FromFeet(NauticalMiles * 6076.1155);
    }

    public static FsLatitudeSpan FromMetres(double Metres)
    {
      return FsLatitudeSpan.FromFeet(Metres * 3.2808);
    }

    public static FsLatitudeSpan BetweenTwoLatitides(FsLatitude Lat1, FsLatitude Lat2)
    {
      return new FsLatitudeSpan(Math.Abs(Lat2.UDegrees - Lat1.UDegrees));
    }

    public int Degrees
    {
      get
      {
        return (int) Math.Truncate(this.DecimalDegrees);
      }
    }

    public int Minutes
    {
      get
      {
        return (int) Math.Truncate(this.DecimalMinutes);
      }
    }

    public int Seconds
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
        return this.span;
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

    public double TotalMinutes
    {
      get
      {
        return this.DecimalDegrees * 60.0;
      }
    }

    public double TotalSeconds
    {
      get
      {
        return this.DecimalDegrees * 3600.0;
      }
    }

    public double ToFeet()
    {
      return 364601.4567 * this.span;
    }

    public double ToNauticalMiles()
    {
      return this.ToFeet() / 6076.1155;
    }

    public double ToMetres()
    {
      return this.ToFeet() / 3.2808;
    }

    public override string ToString()
    {
      return this.ToString("m", 4);
    }

    public string ToString(string DetailLevel, int DecimalPlaces)
    {
      string str1 = "";
      string str2;
      switch (DetailLevel)
      {
        case "m":
          str2 = str1 + Math.Abs(this.Degrees).ToString("000") + "* " + Math.Abs(this.DecimalMinutes).ToString("00" + (DecimalPlaces > 0 ? "." + new string('0', DecimalPlaces) : "")) + "'";
          break;
        case "s":
          str2 = str1 + Math.Abs(this.Degrees).ToString("000") + "* " + Math.Abs(this.Minutes).ToString("00") + "' " + Math.Abs(this.DecimalSeconds).ToString("00" + (DecimalPlaces > 0 ? "." + new string('0', DecimalPlaces) : "")) + "\"";
          break;
        default:
          str2 = str1 + Math.Abs(this.DecimalDegrees).ToString("000" + (DecimalPlaces > 0 ? "." + new string('0', DecimalPlaces) : "")) + "*";
          break;
      }
      return str2;
    }
  }
}
