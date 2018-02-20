// Decompiled with JetBrains decompiler
// Type: FSXWX.GeoClass
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSXWX
{
  internal static class GeoClass
  {
    public static double Dist(double lat1, double lon1, double lat2, double lon2)
    {
      int num1 = 6371;
      double rad1 = GeoClass.ToRad(lat1);
      double rad2 = GeoClass.ToRad(lat2);
      double num2 = rad2 - rad1;
      double rad3 = GeoClass.ToRad(lon2 - lon1);
      double num3 = 2.0;
      double d = Math.Pow(Math.Sin(num2 / num3), 2.0) + Math.Pow(Math.Sin(rad3 / 2.0), 2.0) * Math.Cos(rad1) * Math.Cos(rad2);
      double num4 = 2.0 * Math.Atan2(Math.Sqrt(d), Math.Sqrt(1.0 - d));
      return (double) num1 * num4;
    }

    public static double ToRad(double degrees)
    {
      return degrees * (Math.PI / 180.0);
    }

    public static double ToDeg(double radians)
    {
      return radians * 180.0 / Math.PI;
    }

    public static double ToBearing(double radians)
    {
      return (GeoClass.ToDeg(radians) + 360.0) % 360.0;
    }

    public static double DegreeBearing(double lat1, double lon1, double lat2, double lon2)
    {
      double y = GeoClass.ToRad(lon2 - lon1);
      double x = Math.Log(Math.Tan(GeoClass.ToRad(lat2) / 2.0 + Math.PI / 4.0) / Math.Tan(GeoClass.ToRad(lat1) / 2.0 + Math.PI / 4.0));
      if (Math.Abs(y) > Math.PI)
        y = y > 0.0 ? -(2.0 * Math.PI - y) : 2.0 * Math.PI + y;
      return GeoClass.ToBearing(Math.Atan2(y, x));
    }

    public static GeoClass.Coord PointAtDistance(GeoClass.Coord startPoint, double initialBearingRadians, double distanceKilometres)
    {
      double num1 = distanceKilometres / 6371.01;
      double num2 = Math.Sin(num1);
      double num3 = Math.Cos(num1);
      double rad1 = GeoClass.ToRad(startPoint.lat);
      double rad2 = GeoClass.ToRad(startPoint.lon);
      double num4 = Math.Cos(rad1);
      double num5 = Math.Sin(rad1);
      double num6 = Math.Asin(num5 * num3 + num4 * num2 * Math.Cos(initialBearingRadians));
      double radians = rad2 + Math.Atan2(Math.Sin(initialBearingRadians) * num2 * num4, num3 - num5 * Math.Sin(num6));
      return new GeoClass.Coord()
      {
        lat = GeoClass.ToDeg(num6),
        lon = GeoClass.ToDeg(radians)
      };
    }

    public static int BearingDiff(int bear2, int bear1)
    {
      int num = bear2 - bear1;
      if (num < 0)
        num = 360 + num;
      return num;
    }

    public static double BearingDiff(double bear2, double bear1)
    {
      double num = bear2 - bear1;
      if (num < 0.0)
        num = 360.0 + num;
      return num;
    }

    public static int BearingDiffAbs(int bear2, int bear1)
    {
      int num = bear2 >= bear1 ? bear2 - bear1 : bear1 - bear2;
      if (num > 180)
        num = 360 - num;
      return num;
    }

    public static double BearingDiffAbs(double bear2, double bear1)
    {
      double num = bear2 >= bear1 ? bear2 - bear1 : bear1 - bear2;
      if (num > 180.0)
        num = 360.0 - num;
      return num;
    }

    public static int BearingAdd(int bear, int bearAdd)
    {
      int num = bear + bearAdd;
      if (num >= 360)
        num -= 360;
      else if (num < 0)
        num = 360 + num;
      return num;
    }

    public static double BearingAdd(double bear, double bearAdd)
    {
      double num = bear + bearAdd;
      if (num >= 360.0)
        num -= 360.0;
      else if (num < 0.0)
        num = 360.0 + num;
      return num;
    }

    public static int BearingSub(int bear, int bearSub)
    {
      int num = bear - bearSub;
      if (num < 0)
        num = 360 + num;
      else if (num >= 360)
        num -= 360;
      return num;
    }

    public static double BearingSub(double bear, double bearSub)
    {
      double num = bear - bearSub;
      if (num < 0.0)
        num = 360.0 + num;
      else if (num >= 360.0)
        num -= 360.0;
      return num;
    }

    public struct Coord
    {
      public double lat { get; set; }

      public double lon { get; set; }
    }
  }
}
