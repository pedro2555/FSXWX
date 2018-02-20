// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsPayloadStation
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

namespace FSUIPC
{
  public class FsPayloadStation
  {
    private double weightLbs;
    private double lateralPositionFt;
    private double verticalPositionFt;
    private double longitudinalPositionFt;
    private string name;
    private int index;
    internal bool valueChanged;

    internal FsPayloadStation(int Index, double Weight, double Lateral, double Vertical, double Longitudinal, string Name)
    {
      this.weightLbs = Weight;
      this.lateralPositionFt = Lateral;
      this.verticalPositionFt = Vertical;
      this.longitudinalPositionFt = Longitudinal;
      this.name = Name;
      this.index = Index;
    }

    public int Index
    {
      get
      {
        return this.index;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public double WeightLbs
    {
      get
      {
        return this.weightLbs;
      }
      set
      {
        this.weightLbs = value;
        this.valueChanged = true;
      }
    }

    public double WeightSlugs
    {
      get
      {
        return this.WeightLbs * 0.0310809502;
      }
      set
      {
        this.WeightLbs = value / 0.0310809502;
      }
    }

    public double WeightKgs
    {
      get
      {
        return this.weightLbs * 0.45359237;
      }
      set
      {
        this.WeightLbs = value / 0.45359237;
      }
    }

    public double WeightNewtons
    {
      get
      {
        return this.WeightKgs * 9.80665;
      }
      set
      {
        this.WeightKgs = value / 9.80665;
      }
    }

    public double PositionLateralFeet
    {
      get
      {
        return this.lateralPositionFt;
      }
    }

    public double PositionLongitudinalFeet
    {
      get
      {
        return this.longitudinalPositionFt;
      }
    }

    public double PositionVerticalFeet
    {
      get
      {
        return this.verticalPositionFt;
      }
    }

    public double PositionLateralMetres
    {
      get
      {
        return this.lateralPositionFt * 0.3048;
      }
    }

    public double PositionLongitudinalMetres
    {
      get
      {
        return this.longitudinalPositionFt * 0.3048;
      }
    }

    public double PositionVerticalMetres
    {
      get
      {
        return this.verticalPositionFt * 0.3048;
      }
    }
  }
}
