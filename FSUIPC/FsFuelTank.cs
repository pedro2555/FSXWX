// Decompiled with JetBrains decompiler
// Type: FSUIPC.FsFuelTank
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

namespace FSUIPC
{
  public class FsFuelTank
  {
    private FSFuelTanks tank;
    private double capacityUSGallons;
    private double levelFraction;
    private double poundsPerGallon;
    internal bool valueChanged;

    internal FsFuelTank(FSFuelTanks Tank, double Capacity, double Level, double PoundsPerGallon)
    {
      this.tank = Tank;
      this.capacityUSGallons = Capacity;
      this.levelFraction = Level;
      this.poundsPerGallon = PoundsPerGallon;
    }

    public FSFuelTanks Tank
    {
      get
      {
        return this.tank;
      }
    }

    public double CapacityUSGallons
    {
      get
      {
        return this.capacityUSGallons;
      }
    }

    public double CapacityLitres
    {
      get
      {
        return this.capacityUSGallons * 3.78541178;
      }
    }

    public double LevelPercentage
    {
      get
      {
        return this.levelFraction * 100.0;
      }
      set
      {
        this.levelFraction = value / 100.0;
        if (this.levelFraction > 1.0)
          this.levelFraction = 1.0;
        this.valueChanged = true;
      }
    }

    public double LevelUSGallons
    {
      get
      {
        return this.capacityUSGallons * this.levelFraction;
      }
      set
      {
        this.LevelPercentage = value / this.capacityUSGallons * 100.0;
      }
    }

    public double LevelLitres
    {
      get
      {
        return this.LevelUSGallons * 3.78541178;
      }
      set
      {
        this.LevelUSGallons = value / 3.78541178;
      }
    }

    public double WeightLbs
    {
      get
      {
        return this.LevelUSGallons * this.poundsPerGallon;
      }
      set
      {
        this.LevelUSGallons = value / this.poundsPerGallon;
      }
    }

    public double WeightKgs
    {
      get
      {
        return this.WeightLbs * 0.45359237;
      }
      set
      {
        this.WeightLbs = value / 0.45359237;
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
  }
}
