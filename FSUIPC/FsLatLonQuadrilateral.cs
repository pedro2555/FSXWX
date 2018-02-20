// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsLatLonQuadrilateral
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Collections.Generic;

namespace FSUIPC
{
  public struct FsLatLonQuadrilateral
  {
    private FsLatLonPoint ne;
    private FsLatLonPoint se;
    private FsLatLonPoint sw;
    private FsLatLonPoint nw;

    public FsLatLonPoint NE
    {
      get
      {
        return this.ne;
      }
    }

    public FsLatLonPoint SE
    {
      get
      {
        return this.se;
      }
    }

    public FsLatLonPoint SW
    {
      get
      {
        return this.sw;
      }
    }

    public FsLatLonPoint NW
    {
      get
      {
        return this.nw;
      }
    }

    public static FsLatLonQuadrilateral ForRunway(FsLatLonPoint ThresholdCentre, double HeadingTrue, double WidthInFeet, double LengthInFeet)
    {
      double num = Math.PI * HeadingTrue / 180.0;
      double decimalDegrees1 = FsLatitudeSpan.FromFeet(Math.Sin(num) * WidthInFeet / 2.0).DecimalDegrees;
      FsLatitude fsLatitude1 = new FsLatitude(ThresholdCentre.Latitude.DecimalDegrees - decimalDegrees1);
      double Feet = Math.Cos(num) * WidthInFeet / 2.0;
      double decimalDegrees2 = FsLongitudeSpan.FromFeet(Feet, fsLatitude1).DecimalDegrees;
      FsLongitude Longitude1 = new FsLongitude(ThresholdCentre.Longitude.DecimalDegrees + decimalDegrees2);
      FsLatLonPoint P1 = new FsLatLonPoint(fsLatitude1, Longitude1);
      fsLatitude1 = new FsLatitude(ThresholdCentre.Latitude.DecimalDegrees + decimalDegrees1);
      double decimalDegrees3 = FsLongitudeSpan.FromFeet(Feet, fsLatitude1).DecimalDegrees;
      Longitude1 = new FsLongitude(ThresholdCentre.Longitude.DecimalDegrees - decimalDegrees3);
      FsLatLonPoint P0 = new FsLatLonPoint(fsLatitude1, Longitude1);
      double decimalDegrees4 = FsLatitudeSpan.FromFeet(Math.Cos(num) * LengthInFeet).DecimalDegrees;
      FsLatitude fsLatitude2 = new FsLatitude(ThresholdCentre.Latitude.DecimalDegrees + decimalDegrees4);
      double decimalDegrees5 = FsLongitudeSpan.FromFeet(Math.Sin(num) * LengthInFeet, fsLatitude2).DecimalDegrees;
      FsLongitude Longitude2 = new FsLongitude(ThresholdCentre.Longitude.DecimalDegrees + decimalDegrees5);
      FsLatLonPoint fsLatLonPoint = new FsLatLonPoint(fsLatitude2, Longitude2);
      fsLatitude1 = new FsLatitude(fsLatLonPoint.Latitude.DecimalDegrees - decimalDegrees1);
      double decimalDegrees6 = FsLongitudeSpan.FromFeet(Feet, fsLatitude1).DecimalDegrees;
      Longitude1 = new FsLongitude(fsLatLonPoint.Longitude.DecimalDegrees + decimalDegrees6);
      FsLatLonPoint P3 = new FsLatLonPoint(fsLatitude1, Longitude1);
      fsLatitude1 = new FsLatitude(fsLatLonPoint.Latitude.DecimalDegrees + decimalDegrees1);
      double decimalDegrees7 = FsLongitudeSpan.FromFeet(Feet, fsLatitude1).DecimalDegrees;
      Longitude1 = new FsLongitude(fsLatLonPoint.Longitude.DecimalDegrees - decimalDegrees7);
      FsLatLonPoint P2 = new FsLatLonPoint(fsLatitude1, Longitude1);
      return new FsLatLonQuadrilateral(P0, P1, P2, P3);
    }

    public FsLatLonQuadrilateral(FsLatLonPoint P0, FsLatLonPoint P1, FsLatLonPoint P2, FsLatLonPoint P3)
    {
      List<FsLatLonPoint> fsLatLonPointList1 = new List<FsLatLonPoint>();
      List<FsLatLonPoint> fsLatLonPointList2 = new List<FsLatLonPoint>();
      fsLatLonPointList1.Add(P0);
      fsLatLonPointList1.Add(P1);
      fsLatLonPointList1.Add(P2);
      fsLatLonPointList1.Add(P3);
      for (int index1 = 0; index1 < 3; ++index1)
      {
        double num = 0.0;
        int index2 = 0;
        for (int index3 = 0; index3 < fsLatLonPointList1.Count; ++index3)
        {
          if (fsLatLonPointList1[index3].Latitude.UDegrees > num)
          {
            num = fsLatLonPointList1[index3].Latitude.UDegrees;
            index2 = index3;
          }
        }
        fsLatLonPointList2.Add(fsLatLonPointList1[index2]);
        fsLatLonPointList1.RemoveAt(index2);
      }
      fsLatLonPointList2.Add(fsLatLonPointList1[0]);
      if (fsLatLonPointList2[1].Longitude.UDegrees > fsLatLonPointList2[0].Longitude.UDegrees)
      {
        this.ne = fsLatLonPointList2[1];
        this.nw = fsLatLonPointList2[0];
      }
      else
      {
        this.ne = fsLatLonPointList2[0];
        this.nw = fsLatLonPointList2[1];
      }
      if (fsLatLonPointList2[3].Longitude.UDegrees > fsLatLonPointList2[2].Longitude.UDegrees)
      {
        this.se = fsLatLonPointList2[3];
        this.sw = fsLatLonPointList2[2];
      }
      else
      {
        this.se = fsLatLonPointList2[2];
        this.sw = fsLatLonPointList2[3];
      }
    }

    public bool ContainsPoint(FsLatLonPoint point)
    {
      bool flag = false;
      double udegrees1 = this.nw.Latitude.UDegrees;
      double udegrees2 = this.sw.Latitude.UDegrees;
      double udegrees3 = this.se.Latitude.UDegrees;
      double udegrees4 = this.ne.Latitude.UDegrees;
      double udegrees5 = this.nw.Longitude.UDegrees;
      double udegrees6 = this.sw.Longitude.UDegrees;
      double udegrees7 = this.se.Longitude.UDegrees;
      double udegrees8 = this.ne.Longitude.UDegrees;
      double udegrees9 = point.Longitude.UDegrees;
      double udegrees10 = point.Latitude.UDegrees;
      if (udegrees9 > udegrees5 + (udegrees6 - udegrees5) / (udegrees1 - udegrees2) * (udegrees1 - udegrees10) && udegrees9 < udegrees8 + (udegrees7 - udegrees8) / (udegrees4 - udegrees3) * (udegrees4 - udegrees10) && udegrees10 > udegrees2 + (udegrees3 - udegrees2) / (udegrees7 - udegrees6) * (udegrees9 - udegrees6))
        flag = udegrees10 < udegrees1 + (udegrees4 - udegrees1) / (udegrees8 - udegrees5) * (udegrees9 - udegrees5);
      return flag;
    }

    public override string ToString()
    {
      return this.ToString(true, "m", (short) 2);
    }

    public string ToString(bool HemisphereAsText, string DetailLevel, short DecimalPlaces)
    {
      return this.NE.ToString(HemisphereAsText, DetailLevel, DecimalPlaces) + "\n" + this.NW.ToString(HemisphereAsText, DetailLevel, DecimalPlaces) + "\n" + this.SW.ToString(HemisphereAsText, DetailLevel, DecimalPlaces) + "\n" + this.SE.ToString(HemisphereAsText, DetailLevel, DecimalPlaces);
    }
  }
}
