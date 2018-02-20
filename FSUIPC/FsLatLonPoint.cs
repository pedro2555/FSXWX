// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsLatLonPoint
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Serializable]
  public struct FsLatLonPoint
  {
    private FsLongitude lon;
    private FsLatitude lat;

    public FsLatLonPoint(FsLatitude Latitude, FsLongitude Longitude)
    {
      this.lon = Longitude;
      this.lat = Latitude;
    }

    public FsLongitude Longitude
    {
      get
      {
        return this.lon;
      }
    }

    public FsLatitude Latitude
    {
      get
      {
        return this.lat;
      }
    }

    public double DistanceFromInFeet(FsLatLonPoint Point)
    {
      FsLongitudeSpan fsLongitudeSpan = FsLongitudeSpan.BetweenTwoLongitudes(Point.Longitude, this.lon);
      double num = (fsLongitudeSpan.ToFeet(this.lat) + fsLongitudeSpan.ToFeet(Point.lat)) / 2.0;
      double feet = new FsLatitudeSpan(Point.Latitude.DecimalDegrees - this.lat.DecimalDegrees).ToFeet();
      return Math.Sqrt(num * num + feet * feet);
    }

    public double DistanceFromInNauticalMiles(FsLatLonPoint Point)
    {
      return this.DistanceFromInFeet(Point) / 6076.1155;
    }

    public double DistanceFromInMetres(FsLatLonPoint Point)
    {
      return this.DistanceFromInFeet(Point) / 3.2808;
    }

    public double BearingTo(FsLatLonPoint Point)
    {
      double num1 = 0.0;
      double num2 = Math.Abs(new FsLatitudeSpan(this.lat.DecimalDegrees - Point.Latitude.DecimalDegrees).ToFeet());
      double num3 = Math.Abs((new FsLongitudeSpan(Point.Longitude.DecimalDegrees - this.lon.DecimalDegrees).ToFeet(this.lat) + new FsLongitudeSpan(Point.Longitude.DecimalDegrees - this.lon.DecimalDegrees).ToFeet(Point.lat)) / 2.0);
      if (num2 == 0.0)
        num1 = this.lon.DecimalDegrees > Point.Longitude.DecimalDegrees ? 270.0 : 90.0;
      else if (this.lat.DecimalDegrees < Point.Latitude.DecimalDegrees && this.lon.DecimalDegrees < Point.Longitude.DecimalDegrees)
        num1 = Math.Atan(num3 / num2) * 180.0 / Math.PI;
      else if (this.lat.DecimalDegrees > Point.Latitude.DecimalDegrees && this.lon.DecimalDegrees < Point.Longitude.DecimalDegrees)
        num1 = Math.Atan(num2 / num3) * 180.0 / Math.PI + 90.0;
      else if (this.lat.DecimalDegrees > Point.Latitude.DecimalDegrees && this.lon.DecimalDegrees > Point.Longitude.DecimalDegrees)
        num1 = Math.Atan(num3 / num2) * 180.0 / Math.PI + 180.0;
      else if (this.lat.DecimalDegrees < Point.Latitude.DecimalDegrees && this.lon.DecimalDegrees > Point.Longitude.DecimalDegrees)
        num1 = Math.Atan(num2 / num3) * 180.0 / Math.PI + 270.0;
      return num1;
    }

    public double BearingFrom(FsLatLonPoint Point)
    {
      double num = 180.0 + this.BearingTo(Point);
      if (num >= 360.0)
        num -= 360.0;
      if (num < 0.0)
        num += 360.0;
      return num;
    }

    public FsLatLonPoint OffsetByFeet(double Bearing, double Distance)
    {
      double Feet1 = Math.Sin(Math.PI * Bearing / 180.0) * Distance;
      double Feet2 = Math.Cos(Math.PI * Bearing / 180.0) * Distance;
      FsLatLonPoint fsLatLonPoint = new FsLatLonPoint();
      fsLatLonPoint.lat = this.lat.Add(FsLatitudeSpan.FromFeet(Feet2));
      fsLatLonPoint.lon = this.lon.Add(FsLongitudeSpan.FromFeet(Feet1, fsLatLonPoint.lat));
      return fsLatLonPoint;
    }

    public FsLatLonPoint OffsetByMetres(double Bearing, double Distance)
    {
      double Metres1 = Math.Sin(Math.PI * Bearing / 180.0) * Distance;
      double Metres2 = Math.Cos(Math.PI * Bearing / 180.0) * Distance;
      FsLatLonPoint fsLatLonPoint = new FsLatLonPoint();
      fsLatLonPoint.lat = this.lat.Add(FsLatitudeSpan.FromMetres(Metres2));
      fsLatLonPoint.lon = this.lon.Add(FsLongitudeSpan.FromMetres(Metres1, fsLatLonPoint.lat));
      return fsLatLonPoint;
    }

    public FsLatLonPoint OffsetByNauticalMiles(double Bearing, double Distance)
    {
      double NauticalMiles1 = Math.Sin(Math.PI * Bearing / 180.0) * Distance;
      double NauticalMiles2 = Math.Cos(Math.PI * Bearing / 180.0) * Distance;
      FsLatLonPoint fsLatLonPoint = new FsLatLonPoint();
      fsLatLonPoint.lat = this.lat.Add(FsLatitudeSpan.FromNauticalMiles(NauticalMiles2));
      fsLatLonPoint.lon = this.lon.Add(FsLongitudeSpan.FromNauticalMiles(NauticalMiles1, fsLatLonPoint.lat));
      return fsLatLonPoint;
    }

    public override string ToString()
    {
      return this.ToString(true, "m", (short) 2);
    }

    public string ToString(bool HemisphereAsText, string DetailLevel, short DecimalPlaces)
    {
      return this.lat.ToString(HemisphereAsText, DetailLevel, DecimalPlaces) + ", " + this.lon.ToString(HemisphereAsText, DetailLevel, DecimalPlaces);
    }
  }
}
