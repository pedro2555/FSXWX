// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsLongitudeSpan
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  public struct FsLongitudeSpan
  {
    private double span;

    public FsLongitudeSpan(double DecimalDegrees)
    {
      this.span = DecimalDegrees;
    }

    public FsLongitudeSpan(int Degrees, double DecimalMinutes)
    {
      this = new FsLongitudeSpan((double) Degrees + DecimalMinutes / 60.0);
    }

    public FsLongitudeSpan(int Degrees, int Minutes, double DecimalSeconds)
    {
      this = new FsLongitudeSpan((double) Degrees + (double) Minutes / 60.0 + DecimalSeconds / 3600.0);
    }

    public static FsLongitudeSpan FromFeet(double Feet, FsLatitude AtLatitude)
    {
      double num = Math.Cos(Math.PI * AtLatitude.DecimalDegrees / 180.0) * 131479672.3 / 360.0;
      return new FsLongitudeSpan(Feet / num);
    }

    public static FsLongitudeSpan FromNauticalMiles(double NauticalMiles, FsLatitude AtLatitude)
    {
      return FsLongitudeSpan.FromFeet(NauticalMiles * 6076.1155, AtLatitude);
    }

    public static FsLongitudeSpan FromMetres(double Metres, FsLatitude AtLatitude)
    {
      return FsLongitudeSpan.FromFeet(Metres * 3.2808, AtLatitude);
    }

    public static FsLongitudeSpan BetweenTwoLongitudes(FsLongitude Lon1, FsLongitude Lon2)
    {
      if ((Lon2.UDegrees - Lon1.UDegrees) % 360.0 < (Lon1.UDegrees - Lon2.UDegrees) % 360.0)
        return new FsLongitudeSpan((Lon2.UDegrees - Lon1.UDegrees) % 360.0);
      return new FsLongitudeSpan((Lon1.UDegrees - Lon2.UDegrees) % 360.0);
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

    public double ToFeet(FsLatitude AtLatitude)
    {
      return Math.Cos(Math.PI * AtLatitude.DecimalDegrees / 180.0) * 131479672.3 / 360.0 * this.span;
    }

    public double ToNauticalMiles(FsLatitude AtLatitude)
    {
      return this.ToFeet(AtLatitude) / 6076.1155;
    }

    public double ToMetres(FsLatitude AtLatitude)
    {
      return this.ToFeet(AtLatitude) / 3.2808;
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
