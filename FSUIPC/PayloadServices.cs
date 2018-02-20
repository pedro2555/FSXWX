// Decompiled with JetBrains decompiler
// Type: FSUIPC.PayloadServices
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System.Collections.Generic;

namespace FSUIPC
{
  public class PayloadServices
  {
    private string payloadGroup = "~~PayloadServices~~";
    private List<FsPayloadStation> payloadStations = new List<FsPayloadStation>();
    private List<FsFuelTank> fuelTanks = new List<FsFuelTank>();
    private Dictionary<FSFuelTanks, FsFuelTank> fuelTankLookup = new Dictionary<FSFuelTanks, FsFuelTank>();
    private double totalWeightLbs;
    private byte classInstance;

    public List<FsPayloadStation> PayloadStations
    {
      get
      {
        return this.payloadStations;
      }
    }

    public List<FsFuelTank> FuelTanks
    {
      get
      {
        return this.fuelTanks;
      }
    }

    public FsFuelTank GetFuelTank(FSFuelTanks Tank)
    {
      if (this.fuelTankLookup.ContainsKey(Tank))
        return this.fuelTankLookup[Tank];
      return (FsFuelTank) null;
    }

    internal PayloadServices(byte ClassInstance)
    {
      this.classInstance = ClassInstance;
      this.payloadGroup += ClassInstance.ToString();
    }

    public double PayloadWeightLbs
    {
      get
      {
        double num = 0.0;
        foreach (FsPayloadStation payloadStation in this.payloadStations)
          num += payloadStation.WeightLbs;
        return num;
      }
    }

    public double PayloadWeightKgs
    {
      get
      {
        return this.PayloadWeightLbs * 0.45359237;
      }
    }

    public double PayloadWeightSlugs
    {
      get
      {
        return this.PayloadWeightLbs * 0.0310809502;
      }
    }

    public double PayloadWeightNewtons
    {
      get
      {
        return this.PayloadWeightKgs * 9.80665;
      }
    }

    public double AircraftWeightLbs
    {
      get
      {
        return this.totalWeightLbs;
      }
    }

    public double AircraftWeightKgs
    {
      get
      {
        return this.totalWeightLbs * 0.45359237;
      }
    }

    public double AircraftWeightSlugs
    {
      get
      {
        return this.totalWeightLbs * 0.0310809502;
      }
    }

    public double AircraftWeightNewtons
    {
      get
      {
        return this.AircraftWeightKgs * 9.80665;
      }
    }

    public double AircraftZeroFuelWeightLbs
    {
      get
      {
        return this.totalWeightLbs - this.FuelWeightLbs;
      }
    }

    public double AircraftZeroFuelWeightKgs
    {
      get
      {
        return this.AircraftZeroFuelWeightLbs * 0.45359237;
      }
    }

    public double AircraftZeroFuelWeightSlugs
    {
      get
      {
        return this.AircraftZeroFuelWeightLbs * 0.0310809502;
      }
    }

    public double AircraftZeroFuelWeightNewtons
    {
      get
      {
        return this.AircraftZeroFuelWeightKgs * 9.80665;
      }
    }

    public double FuelWeightLbs
    {
      get
      {
        double num = 0.0;
        foreach (FsFuelTank fuelTank in this.FuelTanks)
          num += fuelTank.WeightLbs;
        return num;
      }
    }

    public double FuelWeightKgs
    {
      get
      {
        return this.FuelWeightLbs * 0.45359237;
      }
    }

    public double FuelWeightSlugs
    {
      get
      {
        return this.FuelWeightLbs * 0.0310809502;
      }
    }

    public double FuelWeightNewtons
    {
      get
      {
        return this.FuelWeightKgs * 9.80665;
      }
    }

    public double FuelCapacityUSGallons
    {
      get
      {
        double num = 0.0;
        foreach (FsFuelTank fuelTank in this.FuelTanks)
          num += fuelTank.CapacityUSGallons;
        return num;
      }
    }

    public double FuelCapacityLitres
    {
      get
      {
        return this.FuelCapacityUSGallons * 3.78541178;
      }
    }

    public double FuelLevelUSGallons
    {
      get
      {
        double num = 0.0;
        foreach (FsFuelTank fuelTank in this.FuelTanks)
          num += fuelTank.LevelUSGallons;
        return num;
      }
    }

    public double FuelLevelLitres
    {
      get
      {
        return this.FuelLevelUSGallons * 3.78541178;
      }
    }

    public double FuelPercentage
    {
      get
      {
        return this.FuelLevelUSGallons / this.FuelCapacityUSGallons * 100.0;
      }
    }

    public void RefreshData()
    {
      Offset<int> offset1 = new Offset<int>(this.payloadGroup, 5116);
      Offset<double> offset2 = new Offset<double>(this.payloadGroup, 12480);
      Offset<int> offset3 = new Offset<int>(this.payloadGroup, 2932);
      Offset<int> offset4 = new Offset<int>(this.payloadGroup, 2936);
      Offset<int> offset5 = new Offset<int>(this.payloadGroup, 2940);
      Offset<int> offset6 = new Offset<int>(this.payloadGroup, 2944);
      Offset<int> offset7 = new Offset<int>(this.payloadGroup, 2948);
      Offset<int> offset8 = new Offset<int>(this.payloadGroup, 2952);
      Offset<int> offset9 = new Offset<int>(this.payloadGroup, 2956);
      Offset<int> offset10 = new Offset<int>(this.payloadGroup, 2960);
      Offset<int> offset11 = new Offset<int>(this.payloadGroup, 2964);
      Offset<int> offset12 = new Offset<int>(this.payloadGroup, 2968);
      Offset<int> offset13 = new Offset<int>(this.payloadGroup, 2972);
      Offset<int> offset14 = new Offset<int>(this.payloadGroup, 2976);
      Offset<int> offset15 = new Offset<int>(this.payloadGroup, 2980);
      Offset<int> offset16 = new Offset<int>(this.payloadGroup, 2984);
      Offset<int> offset17 = new Offset<int>(this.payloadGroup, 4676);
      Offset<int> offset18 = new Offset<int>(this.payloadGroup, 4680);
      Offset<int> offset19 = new Offset<int>(this.payloadGroup, 4684);
      Offset<int> offset20 = new Offset<int>(this.payloadGroup, 4688);
      Offset<int> offset21 = new Offset<int>(this.payloadGroup, 4692);
      Offset<int> offset22 = new Offset<int>(this.payloadGroup, 4696);
      Offset<int> offset23 = new Offset<int>(this.payloadGroup, 4700);
      Offset<int> offset24 = new Offset<int>(this.payloadGroup, 4704);
      Offset<short> offset25 = new Offset<short>(this.payloadGroup, 2804);
      FSUIPCConnection.Process(this.classInstance, this.payloadGroup);
      FSUIPCConnection.DeleteGroup(this.payloadGroup);
      this.totalWeightLbs = offset2.Value;
      Offset<double>[] offsetArray1 = new Offset<double>[offset1.Value];
      Offset<double>[] offsetArray2 = new Offset<double>[offset1.Value];
      Offset<double>[] offsetArray3 = new Offset<double>[offset1.Value];
      Offset<double>[] offsetArray4 = new Offset<double>[offset1.Value];
      Offset<string>[] offsetArray5 = new Offset<string>[offset1.Value];
      for (int index = 0; index < offset1.Value; ++index)
      {
        offsetArray1[index] = new Offset<double>(this.payloadGroup, 5120 + 48 * index);
        offsetArray2[index] = new Offset<double>(this.payloadGroup, 5128 + 48 * index);
        offsetArray3[index] = new Offset<double>(this.payloadGroup, 5136 + 48 * index);
        offsetArray4[index] = new Offset<double>(this.payloadGroup, 5144 + 48 * index);
        offsetArray5[index] = new Offset<string>(this.payloadGroup, 5152 + 48 * index, 16);
      }
      if (offset1.Value > 0)
      {
        FSUIPCConnection.Process(this.classInstance, this.payloadGroup);
        FSUIPCConnection.DeleteGroup(this.payloadGroup);
      }
      this.payloadStations.Clear();
      for (int Index = 0; Index < offset1.Value; ++Index)
        this.payloadStations.Add(new FsPayloadStation(Index, offsetArray1[Index].Value, offsetArray2[Index].Value, offsetArray3[Index].Value, offsetArray4[Index].Value, offsetArray5[Index].Value));
      double PoundsPerGallon = (double) offset25.Value / 256.0;
      this.fuelTanks.Clear();
      this.fuelTankLookup.Clear();
      double Level1 = (double) offset3.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank1 = new FsFuelTank(FSFuelTanks.Centre_Main, (double) offset4.Value, Level1, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank1);
      this.fuelTankLookup.Add(fsFuelTank1.Tank, fsFuelTank1);
      double Level2 = (double) offset17.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank2 = new FsFuelTank(FSFuelTanks.Centre_2, (double) offset18.Value, Level2, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank2);
      this.fuelTankLookup.Add(fsFuelTank2.Tank, fsFuelTank2);
      double Level3 = (double) offset19.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank3 = new FsFuelTank(FSFuelTanks.Centre_3, (double) offset20.Value, Level3, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank3);
      this.fuelTankLookup.Add(fsFuelTank3.Tank, fsFuelTank3);
      double Level4 = (double) offset21.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank4 = new FsFuelTank(FSFuelTanks.External_1, (double) offset22.Value, Level4, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank4);
      this.fuelTankLookup.Add(fsFuelTank4.Tank, fsFuelTank4);
      double Level5 = (double) offset23.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank5 = new FsFuelTank(FSFuelTanks.External_2, (double) offset24.Value, Level5, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank5);
      this.fuelTankLookup.Add(fsFuelTank5.Tank, fsFuelTank5);
      double Level6 = (double) offset7.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank6 = new FsFuelTank(FSFuelTanks.Left_Aux, (double) offset8.Value, Level6, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank6);
      this.fuelTankLookup.Add(fsFuelTank6.Tank, fsFuelTank6);
      double Level7 = (double) offset5.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank7 = new FsFuelTank(FSFuelTanks.Left_Main, (double) offset6.Value, Level7, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank7);
      this.fuelTankLookup.Add(fsFuelTank7.Tank, fsFuelTank7);
      double Level8 = (double) offset9.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank8 = new FsFuelTank(FSFuelTanks.Left_Tip, (double) offset10.Value, Level8, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank8);
      this.fuelTankLookup.Add(fsFuelTank8.Tank, fsFuelTank8);
      double Level9 = (double) offset13.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank9 = new FsFuelTank(FSFuelTanks.Right_Aux, (double) offset14.Value, Level9, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank9);
      this.fuelTankLookup.Add(fsFuelTank9.Tank, fsFuelTank9);
      double Level10 = (double) offset11.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank10 = new FsFuelTank(FSFuelTanks.Right_Main, (double) offset12.Value, Level10, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank10);
      this.fuelTankLookup.Add(fsFuelTank10.Tank, fsFuelTank10);
      double Level11 = (double) offset15.Value / 128.0 / 65536.0;
      FsFuelTank fsFuelTank11 = new FsFuelTank(FSFuelTanks.Right_Tip, (double) offset16.Value, Level11, PoundsPerGallon);
      this.fuelTanks.Add(fsFuelTank11);
      this.fuelTankLookup.Add(fsFuelTank11.Tank, fsFuelTank11);
    }

    public void WriteChanges()
    {
      Offset<int> offset1 = new Offset<int>(this.payloadGroup, 5116);
      foreach (FsFuelTank fuelTank in this.fuelTanks)
      {
        if (fuelTank.valueChanged)
        {
          Offset<int> offset2 = (Offset<int>) null;
          switch (fuelTank.Tank)
          {
            case FSFuelTanks.Centre_Main:
              offset2 = new Offset<int>(this.payloadGroup, 2932, true);
              break;
            case FSFuelTanks.Left_Main:
              offset2 = new Offset<int>(this.payloadGroup, 2940, true);
              break;
            case FSFuelTanks.Right_Main:
              offset2 = new Offset<int>(this.payloadGroup, 2964, true);
              break;
            case FSFuelTanks.Left_Aux:
              offset2 = new Offset<int>(this.payloadGroup, 2948, true);
              break;
            case FSFuelTanks.Right_Aux:
              offset2 = new Offset<int>(this.payloadGroup, 2972, true);
              break;
            case FSFuelTanks.Left_Tip:
              offset2 = new Offset<int>(this.payloadGroup, 2956, true);
              break;
            case FSFuelTanks.Right_Tip:
              offset2 = new Offset<int>(this.payloadGroup, 2980, true);
              break;
            case FSFuelTanks.Centre_2:
              offset2 = new Offset<int>(this.payloadGroup, 4676, true);
              break;
            case FSFuelTanks.Centre_3:
              offset2 = new Offset<int>(this.payloadGroup, 4684, true);
              break;
            case FSFuelTanks.External_1:
              offset2 = new Offset<int>(this.payloadGroup, 4692, true);
              break;
            case FSFuelTanks.External_2:
              offset2 = new Offset<int>(this.payloadGroup, 4700, true);
              break;
          }
          if (offset2 != null)
            offset2.Value = (int) (fuelTank.LevelPercentage / 100.0 * 128.0 * 65536.0);
        }
      }
      foreach (FsPayloadStation payloadStation in this.payloadStations)
      {
        if (payloadStation.valueChanged)
          new Offset<double>(this.payloadGroup, 5120 + 48 * payloadStation.Index, true).Value = payloadStation.WeightLbs;
      }
      FSUIPCConnection.Process(this.classInstance, this.payloadGroup);
      FSUIPCConnection.DeleteGroup(this.payloadGroup);
    }
  }
}
