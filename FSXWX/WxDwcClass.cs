// Decompiled with JetBrains decompiler
// Type: FSXWX.WxDwcClass
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using FSUIPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FSXWX
{
  internal class WxDwcClass
  {
    private Timer WindDirWriteTimer = new Timer();
    private Timer WindSpdWriteTimer = new Timer();
    private Timer WindReadMagTimer = new Timer();
    private Timer TurbWriteTimer = new Timer();
    private Timer TurbWriteOnOffTimer = new Timer();
    private Timer VarianceTimer = new Timer();
    private Timer VarianceWriteTimer = new Timer();
    private Timer GustsSpdTimer = new Timer();
    private Timer GustsDirTimer = new Timer();
    private Timer GustsSpdWriteTimer = new Timer();
    private Timer GustsDirWriteTimer = new Timer();
    private Timer FSUIPCProcessTimer = new Timer();
    private Timer FSUIPCProcessMiscTimer = new Timer();
    private Timer FSUIPCReconnectTimer = new Timer();
    private Timer DebugTimer = new Timer();
    private Offset<double> fsuipcSpdWrite = new Offset<double>(11752);
    private Offset<double> fsuipcDirWrite = new Offset<double>(11744);
    private Offset<short> fsuipcCloudTurb = new Offset<short>(3720);
    private Offset<short> fsuipcWindTurb = new Offset<short>(3736);
    private Offset<short> fsuipcMagVar = new Offset<short>(672);
    private Offset<short> fsuipcGndAlt = new Offset<short>(2892);
    private Offset<double> fsuipcAlt = new Offset<double>(24608);
    private Offset<double> fsuipcLat = new Offset<double>(24592);
    private Offset<double> fsuipcLon = new Offset<double>(24600);
    private double[] sinCurve = new double[100];
    private double[] sinCurveHalf = new double[100];
    private double[] centerPeakCurve = new double[100]
    {
      0.0,
      0.0,
      0.002,
      0.005,
      0.009,
      0.012,
      0.017,
      0.021,
      0.026,
      0.032,
      0.038,
      0.043,
      0.05,
      0.057,
      0.064,
      0.071,
      0.079,
      0.087,
      0.094,
      0.103,
      0.113,
      0.122,
      0.132,
      0.142,
      0.153,
      0.164,
      0.177,
      0.19,
      0.207,
      0.225,
      0.246,
      0.267,
      0.287,
      0.308,
      0.337,
      0.368,
      0.406,
      0.444,
      0.498,
      0.543,
      0.59,
      0.666,
      0.76,
      0.831,
      0.89,
      0.924,
      0.949,
      0.967,
      0.98,
      0.991,
      1.0,
      0.991,
      0.98,
      0.967,
      0.949,
      0.924,
      0.89,
      0.831,
      0.76,
      0.666,
      0.59,
      0.543,
      0.498,
      0.444,
      0.406,
      0.368,
      0.337,
      0.308,
      0.287,
      0.267,
      0.246,
      0.225,
      0.207,
      0.19,
      0.177,
      0.164,
      0.153,
      0.142,
      0.132,
      0.122,
      0.113,
      0.103,
      0.094,
      0.087,
      0.079,
      0.071,
      0.064,
      0.057,
      0.05,
      0.043,
      0.038,
      0.032,
      0.026,
      0.021,
      0.017,
      0.012,
      0.009,
      0.005,
      0.002,
      0.0
    };
    private double[] frontPeakCurve = new double[100]
    {
      0.0,
      0.085,
      0.183,
      0.288,
      0.384,
      0.454,
      0.506,
      0.55,
      0.592,
      0.632,
      0.667,
      0.704,
      0.738,
      0.77,
      0.801,
      0.831,
      0.857,
      0.882,
      0.902,
      0.922,
      0.938,
      0.952,
      0.964,
      0.974,
      0.983,
      0.99,
      0.995,
      0.998,
      0.1,
      0.999,
      0.998,
      0.997,
      0.996,
      0.993,
      0.99,
      0.986,
      0.981,
      0.974,
      0.967,
      0.958,
      0.95,
      0.938,
      0.929,
      0.917,
      0.904,
      0.89,
      0.873,
      0.858,
      0.838,
      0.816,
      0.794,
      0.772,
      0.746,
      0.717,
      0.684,
      0.652,
      0.62,
      0.593,
      0.561,
      0.53,
      0.5,
      0.473,
      0.444,
      0.418,
      0.393,
      0.37,
      0.348,
      0.325,
      0.303,
      0.281,
      0.262,
      0.242,
      0.223,
      0.207,
      0.192,
      0.178,
      0.165,
      0.152,
      0.14,
      0.128,
      0.118,
      0.108,
      0.099,
      0.091,
      0.083,
      0.076,
      0.069,
      0.063,
      0.057,
      0.051,
      0.045,
      0.04,
      0.035,
      0.031,
      0.026,
      0.021,
      0.017,
      0.012,
      0.007,
      0.003
    };
    private double _dwcTurbScale;
    private double _dwcGustsSpdScale;
    private double _dwcGustsDirScale;
    private double _dwcVarianceScale;
    private double _dwcRandomScale;
    private WxDwcClass.wxAcPos _acPos;
    private WxDwcClass.wxWindInfo _windinfo;
    private WxDwcClass.wxWindTurb _windturb;
    private WxDwcClass.wxWind _wind;
    private WxDwcClass.wxWindGusts _windgusts;
    private WxDwcClass.wxWindVar _windvar;
    private double[] backPeakCurve;

    public double dwcTurbScale
    {
      get
      {
        return this._dwcTurbScale;
      }
      set
      {
        this._dwcTurbScale = value;
      }
    }

    public double dwcGustsSpdScale
    {
      get
      {
        return this._dwcGustsSpdScale;
      }
      set
      {
        this._dwcGustsSpdScale = value;
      }
    }

    public double dwcGustsDirScale
    {
      get
      {
        return this._dwcGustsDirScale;
      }
      set
      {
        this._dwcGustsDirScale = value;
      }
    }

    public double dwcVarianceScale
    {
      get
      {
        return this._dwcVarianceScale;
      }
      set
      {
        this._dwcVarianceScale = value;
      }
    }

    public double dwcRandomScale
    {
      get
      {
        return this._dwcRandomScale;
      }
      set
      {
        this._dwcRandomScale = value;
      }
    }

    public WxDwcClass()
    {
      for (int index = 0; index < 100; ++index)
      {
        this.sinCurve[index] = (Math.Sin((double) (index * 2) * Math.PI / 100.0 - Math.PI / 2.0) + 1.0) / 2.0;
        this.sinCurveHalf[index] = (Math.Sin((double) index * Math.PI / 100.0 - Math.PI / 2.0) + 1.0) / 2.0;
      }
      this.backPeakCurve = (double[]) this.frontPeakCurve.Clone();
      Array.Reverse((Array) this.backPeakCurve);
      this._wind.spdLast = 0.0;
      this._wind.dirLast = 0.0;
      this.WindSpdWriteTimer.Interval = 400;
      this.WindSpdWriteTimer.Tick += new EventHandler(this.WindSpdWriteTimer_Tick);
      this.WindDirWriteTimer.Interval = 400;
      this.WindDirWriteTimer.Tick += new EventHandler(this.WindDirWriteTimer_Tick);
      this.WindReadMagTimer.Interval = 10000;
      this.WindReadMagTimer.Tick += new EventHandler(this.WindReadMagTimer_Tick);
      this._windvar.dir = 0.0;
      this._windvar.maxHeight = 24000.0;
      this._windvar.dirTickList = new List<double>();
      this.VarianceTimer.Interval = 1111;
      this.VarianceTimer.Tick += new EventHandler(this.VarianceTimer_Tick);
      this.VarianceWriteTimer.Interval = 400;
      this.VarianceWriteTimer.Tick += new EventHandler(this.VarianceWriteTimer_Tick);
      this._windgusts.maxHeightSpd = 24000.0;
      this._windgusts.maxHeightDir = 16000.0;
      this._windgusts.spdTickList = new List<double>();
      this._windgusts.dirTickList = new List<double>();
      this.GustsSpdTimer.Interval = 1111;
      this.GustsSpdTimer.Tick += new EventHandler(this.GustsSpdTimer_Tick);
      this.GustsSpdWriteTimer.Interval = 400;
      this.GustsSpdWriteTimer.Tick += new EventHandler(this.GustsSpdWriteTimer_Tick);
      this.GustsDirTimer.Interval = 1111;
      this.GustsDirTimer.Tick += new EventHandler(this.GustsDirTimer_Tick);
      this.GustsDirWriteTimer.Interval = 400;
      this.GustsDirWriteTimer.Tick += new EventHandler(this.GustsDirWriteTimer_Tick);
      this._windturb.maxHeight = 26000.0;
      this.TurbWriteTimer.Interval = 800;
      this.TurbWriteTimer.Tick += new EventHandler(this.TurbWriteTimer_Tick);
      this.TurbWriteOnOffTimer.Interval = 2200;
      this.TurbWriteOnOffTimer.Tick += new EventHandler(this.TurbWriteOnOffTimer_Tick);
      this.FSUIPCReconnectTimer.Interval = 9977;
      this.FSUIPCReconnectTimer.Tick += new EventHandler(this.FSUIPCReconnectTimer_Tick);
      this.FSUIPCProcessTimer.Interval = 300;
      this.FSUIPCProcessTimer.Tick += new EventHandler(this.FSUIPCProcessTimer_Tick);
      this.DebugTimer.Interval = 1000;
      this.DebugTimer.Tick += new EventHandler(this.DebugTimer_Tick);
    }

    public bool DwcClose()
    {
      this.FSUIPCProcessTimer.Stop();
      this.WindDirWriteTimer.Stop();
      this.WindSpdWriteTimer.Stop();
      this.WindReadMagTimer.Stop();
      this.GustsSpdTimer.Stop();
      this.GustsDirTimer.Stop();
      this.TurbWriteTimer.Stop();
      this.TurbWriteOnOffTimer.Stop();
      this.VarianceTimer.Stop();
      this.DebugTimer.Stop();
      try
      {
        FSUIPCConnection.Close();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public bool DwcOpen()
    {
      try
      {
        FSUIPCConnection.Close();
        FSUIPCConnection.Open();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void StartTimer()
    {
      this.VarianceTimer.Start();
      this.GustsSpdTimer.Start();
      this.GustsDirTimer.Start();
      this.TurbWriteTimer.Start();
      this.TurbWriteOnOffTimer.Start();
      this.WindReadMagTimer.Start();
      this.WindDirWriteTimer.Start();
      this.WindSpdWriteTimer.Start();
      this.FSUIPCProcessTimer.Start();
    }

    private void DebugTimer_Tick(object sender, EventArgs e)
    {
      LogClass.LogWrite("FSXWXlogDwcDir.txt", "dir: " + (object) this._wind.dirWrite + " | gusts: " + (object) this._windgusts.dir + " | var: " + (object) this._windvar.dir + " | magVar: " + (object) this._wind.magVar + " | fsuipc => " + this.fsuipcDirWrite.Value.ToString());
      LogClass.LogWrite("FSXWXlogDwcSpd.txt", "spd: " + (object) this._wind.spdWrite + " | gusts: " + (object) this._windgusts.spd + " | turb: " + (object) this._windturb.spd + " | fsuipc => " + this.fsuipcSpdWrite.Value.ToString());
    }

    private void FSUIPCReconnectTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        FSUIPCConnection.Close();
        FSUIPCConnection.Open();
      }
      catch
      {
      }
    }

    private void FSUIPCProcessTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        FSUIPCConnection.Process();
      }
      catch
      {
        this.DwcOpen();
      }
    }

    public void CollectWindData(WxGridClass wxGridObj)
    {
      try
      {
        Random random = new Random();
        List<WxGridClass.wxStruct> wxStructList = new List<WxGridClass.wxStruct>();
        List<WxGridClass.wxStruct> wxData = wxGridObj.FetchWxDataNearest(wxGridObj.wxGrid, this._acPos.lat, this._acPos.lon, 4);
        if (wxData.Count <= 0)
          return;
        this._acPos.altGnd = Math.Max(this.fsuipcAlt.Value - (double) this.fsuipcGndAlt.Value, 0.0) * 3.28;
        this._acPos.lat = this.fsuipcLat.Value;
        this._acPos.lon = this.fsuipcLon.Value;
        double num1 = (double) this.fsuipcCloudTurb.Value;
        double num2 = (double) this.fsuipcWindTurb.Value;
        if (num1 == 0.0)
          this._windinfo.turbCloudStrength = 0.0;
        else if (num1 == 72.0)
          this._windinfo.turbCloudStrength = 1.0;
        else if (num1 == 144.0)
          this._windinfo.turbCloudStrength = 2.0;
        else if (num1 == 216.0)
          this._windinfo.turbCloudStrength = 3.0;
        else if (num1 == 252.0)
          this._windinfo.turbCloudStrength = 4.0;
        if (num2 == 0.0)
          this._windinfo.turbWindStrength = 0.0;
        else if (num2 == 64.0)
          this._windinfo.turbWindStrength = 1.0;
        else if (num2 == 128.0)
          this._windinfo.turbWindStrength = 2.0;
        else if (num2 == 192.0)
          this._windinfo.turbWindStrength = 3.0;
        else if (num2 == (double) byte.MaxValue)
          this._windinfo.turbWindStrength = 4.0;
        this._windinfo.var = wxData[0].wind_variance;
        this._windinfo.dirDiff = GeoClass.BearingDiffAbs(wxData[0].wind_dir_degrees, wxData[1].wind_dir_degrees);
        this._windinfo.gusts = wxData[0].wind_gusts;
        this._windinfo.vrb = wxData[0].wind_vrb;
        this._windinfo.windSpdGnd = (double) wxData[0].winds_aloft_ext[0][3];
        List<int[]> numArrayList1 = new List<int[]>();
        WxGridClass.weightingStruct weighting = new WxGridClass.weightingStruct();
        List<double> doubleList = new List<double>();
        weighting.sum_dist = 0.0;
        weighting.sum_weights = 0.0;
        doubleList.Clear();
        for (int index = 0; index < wxData.Count; ++index)
          weighting.sum_dist += wxData[index].dist_from_ac;
        for (int index = 0; index < wxData.Count; ++index)
          doubleList.Add(1.0 - wxData[index].dist_from_ac / weighting.sum_dist);
        weighting.sum_weights = doubleList.Sum();
        List<int[]> numArrayList2 = wxGridObj.InterpolateWindsAloft(wxData, weighting, doubleList, 1.0, 1.0);
        int[] last = numArrayList2.FindLast((Predicate<int[]>) (n => (double) (n[0] * 100) <= Math.Max(0.0, this._acPos.altGnd)));
        int[] numArray = numArrayList2.Find((Predicate<int[]>) (n => (double) (n[0] * 100) > Math.Max(1.0, this._acPos.altGnd)));
        this._wind.spd = Math.Round(((double) numArray[3] - (double) last[3]) / ((double) numArray[0] - (double) last[0]) * (Math.Max(0.0, this._acPos.altGnd) / 100.0 - (double) last[0]) + (double) last[3]);
        if (GeoClass.BearingDiff(numArray[2], last[2]) <= 180)
        {
          int num3 = GeoClass.BearingDiff(numArray[2], last[2]);
          this._wind.dir = (double) GeoClass.BearingAdd(last[2], (int) Math.Round((Math.Max(0.0, this._acPos.altGnd) / 100.0 - (double) last[0]) / ((double) numArray[0] - (double) last[0]) * (double) num3));
        }
        else
        {
          int num3 = 360 - GeoClass.BearingDiff(numArray[2], last[2]);
          this._wind.dir = (double) GeoClass.BearingAdd(numArray[2], (int) Math.Round((Math.Max(0.0, this._acPos.altGnd) / 100.0 - (double) last[0]) / ((double) numArray[0] - (double) last[0]) * (double) num3));
        }
        WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
        double num4 = 9999.0;
        WxGridClass.wxStruct wxStruct2 = new WxGridClass.wxStruct();
        double num5 = 9999.0;
        WxGridClass.wxStruct wxStruct3 = new WxGridClass.wxStruct();
        double num6 = 9999.0;
        WxGridClass.wxStruct wxStruct4 = new WxGridClass.wxStruct();
        double num7 = 9999.0;
        if (wxData.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == wxGridObj.adep)))
        {
          wxStruct1 = wxData.Find((Predicate<WxGridClass.wxStruct>) (i => i.station_id == wxGridObj.adep));
          num4 = wxStruct1.dist_from_ac;
        }
        if (wxData.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == wxGridObj.ades)))
        {
          wxStruct2 = wxData.Find((Predicate<WxGridClass.wxStruct>) (i => i.station_id == wxGridObj.ades));
          num5 = wxStruct2.dist_from_ac;
        }
        if (wxData.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == wxGridObj.altn)))
        {
          wxStruct3 = wxData.Find((Predicate<WxGridClass.wxStruct>) (i => i.station_id == wxGridObj.altn));
          num6 = wxStruct3.dist_from_ac;
        }
        if (num4 <= num5 && num4 <= num6)
        {
          wxStruct4 = wxStruct1;
          num7 = num4;
        }
        else if (num5 <= num4 && num5 <= num6)
        {
          wxStruct4 = wxStruct2;
          num7 = num5;
        }
        else if (num6 <= num4 && num6 <= num5)
        {
          wxStruct4 = wxStruct3;
          num7 = num6;
        }
        if (num7 <= 100.0)
        {
          double num3 = Math.Max(1.0 - num7 / 100.0, 0.0) * Math.Max(1.0 - this._acPos.altGnd / 10000.0, 0.0) * (double) GeoClass.BearingDiffAbs((int) this._wind.dir, wxStruct4.wind_dir_degrees);
          double num8 = Math.Max(1.0 - num7 / 100.0, 0.0) * Math.Max(1.0 - this._acPos.altGnd / 10000.0, 0.0) * ((double) wxStruct4.wind_speed_kt - this._wind.spd);
          this._wind.dir = GeoClass.BearingDiff((int) this._wind.dir, wxStruct4.wind_dir_degrees) <= 180 ? (this._wind.dir = (double) GeoClass.BearingSub((int) this._wind.dir, (int) num3)) : (this._wind.dir = (double) GeoClass.BearingAdd((int) this._wind.dir, (int) num3));
          this._wind.spd = Math.Round(this._wind.spd + num8);
        }
        if (GeoClass.BearingDiffAbs(this._wind.dirLast, this._wind.dir) > 4.0)
          this._wind.dir = GeoClass.BearingDiff(this._wind.dirLast, this._wind.dir) <= 180.0 ? GeoClass.BearingSub(this._wind.dirLast, 2.0) : GeoClass.BearingAdd(this._wind.dirLast, 2.0);
        if (Math.Abs(this._wind.spdLast - this._wind.spd) > 4.0)
          this._wind.spd = this._wind.spd > this._wind.spdLast ? this._wind.spdLast + 2.0 : this._wind.spdLast - 2.0;
        this._wind.dirLast = this._wind.dir;
        this._wind.spdLast = this._wind.spd;
        this._wind.spdWrite = this._wind.spd;
        this._wind.dirWrite = this._wind.dir;
      }
      catch
      {
      }
    }

    private void WindReadMagTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        this._wind.magVar = (double) this.fsuipcMagVar.Value * 360.0 / 65536.0;
      }
      catch
      {
      }
    }

    private void WindDirWriteTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        this.fsuipcDirWrite.Value = GeoClass.BearingAdd(GeoClass.BearingAdd(GeoClass.BearingAdd(this._wind.dirWrite, this._windgusts.dir), this._windvar.dir), this._wind.magVar);
      }
      catch
      {
      }
    }

    private void WindSpdWriteTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        this.fsuipcSpdWrite.Value = this._wind.spdWrite + this._windgusts.spd + this._windturb.spd;
      }
      catch
      {
      }
    }

    private void VarianceTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        if (this.VarianceWriteTimer.Enabled)
          return;
        Random random = new Random();
        double num1 = 0.0;
        this._windvar.dirTickList.Clear();
        this._windvar.dirTickListPos = 0;
        double num2 = random.Next(0, 2) == 1 ? -1.0 : 1.0;
        double num3;
        if (this._dwcVarianceScale > 0.0 && this._windinfo.var > 0 && this._acPos.altGnd < this._windvar.maxHeight)
        {
          if (random.Next(0, 100) > 30)
          {
            num3 = this._windinfo.vrb ? (double) random.Next(20000, 40000) : (double) random.Next(8000, 24000);
            for (int index = 0; index < 1000; ++index)
            {
              num1 = (double) random.Next(0, this._windinfo.var) * num2;
              if (Math.Abs(this._windvar.dir + num1) >= (double) (this._windinfo.var / 3) * this._dwcVarianceScale * Math.Max(-1.0 / this._windvar.maxHeight * this._acPos.altGnd + 1.0, 0.2))
                num1 = -1.0 * this._windvar.dir;
              else
                break;
            }
            LogClass.LogWrite("FSXWXlogDwcDir.txt", "small var: " + num1.ToString() + "°/" + num3.ToString() + "ms");
          }
          else
          {
            num3 = this._windinfo.vrb ? (double) random.Next(40000, 80000) : (double) random.Next(16000, 48000);
            for (int index = 0; index < 1000; ++index)
            {
              num1 = (double) random.Next(0, this._windinfo.var) * num2;
              if (Math.Abs(this._windvar.dir + num1) >= (double) (this._windinfo.var / 2) * this._dwcVarianceScale * Math.Max(-1.0 / this._windvar.maxHeight * this._acPos.altGnd + 1.0, 0.2))
                num1 = -1.0 * this._windvar.dir;
              else
                break;
            }
            LogClass.LogWrite("FSXWXlogDwcDir.txt", "large var: " + num1.ToString() + "°/" + num3.ToString() + "ms");
          }
        }
        else if (this._dwcRandomScale > 0.0 && this._acPos.altGnd < this._windvar.maxHeight)
        {
          if (random.Next(0, 100) > 50)
          {
            num3 = (double) random.Next(20000, 40000);
            int maxValue = 20;
            for (int index = 0; index < 1000; ++index)
            {
              num1 = (double) random.Next(0, maxValue) * num2;
              if (Math.Abs(this._windvar.dir + num1) >= (double) (maxValue / 3) * this._dwcRandomScale * Math.Max(-1.0 / this._windvar.maxHeight * this._acPos.altGnd + 1.0, 0.2))
                num1 = -1.0 * this._windvar.dir;
              else
                break;
            }
          }
          else
          {
            num3 = 4000.0;
            num1 = 0.0;
          }
          LogClass.LogWrite("FSXWXlogDwcDir.txt", "random var: " + num1.ToString() + "°/" + num3.ToString() + "ms");
        }
        else
        {
          num3 = 20000.0;
          num1 = -1.0 * this._windvar.dir;
          LogClass.LogWrite("FSXWXlogDwcDir.txt", "fadeout var: " + num1.ToString() + "°/" + num3.ToString() + "ms");
        }
        double num4 = num3 / (double) this.WindDirWriteTimer.Interval;
        for (double num5 = 0.0; num5 < num4; ++num5)
          this._windvar.dirTickList.Add(Math.Round(this._windvar.dir + this.sinCurveHalf[Convert.ToInt32(Math.Min(Math.Round(100.0 / num4 * num5), 99.0))] * num1));
        this.VarianceWriteTimer.Start();
        this.VarianceTimer.Stop();
      }
      catch
      {
      }
    }

    private void VarianceWriteTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        this._windvar.dir = this._windvar.dirTickList[this._windvar.dirTickListPos];
        ++this._windvar.dirTickListPos;
        if (this._windvar.dirTickListPos != this._windvar.dirTickList.Count<double>())
          return;
        this.VarianceTimer.Start();
        this.VarianceWriteTimer.Stop();
      }
      catch
      {
      }
    }

    private void GustsSpdTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        if (this.GustsSpdWriteTimer.Enabled)
          return;
        Random random = new Random();
        double num1 = 0.0;
        this._windgusts.spdTickList.Clear();
        this._windgusts.tickListPos = 0;
        double num2;
        double num3;
        if (random.Next(0, 100) > 60)
        {
          num2 = (double) random.Next(1000, 16000);
          num3 = 0.0;
          LogClass.LogWrite("FSXWXlogDwcSpd.txt", "pause gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
        }
        else if (this._dwcGustsSpdScale > 0.0 && (double) this._windinfo.gusts > this._windinfo.windSpdGnd && this._acPos.altGnd < this._windgusts.maxHeightSpd)
        {
          if (random.Next(0, 100) > 30)
          {
            num2 = (double) random.Next(2000, 5000);
            num3 = (double) random.Next(10, 50) * ((double) this._windinfo.gusts - this._windinfo.windSpdGnd) / 100.0 * this._dwcGustsSpdScale;
            LogClass.LogWrite("FSXWXlogDwcSpd.txt", "small gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
          }
          else
          {
            num2 = (double) random.Next(2000, 8000);
            num3 = (double) random.Next(50, 100) * ((double) this._windinfo.gusts - this._windinfo.windSpdGnd) / 100.0 * this._dwcGustsSpdScale;
            LogClass.LogWrite("FSXWXlogDwcSpd.txt", "large gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
          }
        }
        else if (this._dwcRandomScale > 0.0 && this._acPos.altGnd < this._windgusts.maxHeightSpd)
        {
          if (this._windinfo.windSpdGnd > 8.0 && random.Next(0, 100) > 95)
          {
            num2 = (double) random.Next(2000, 6000);
            num3 = (double) random.Next(2, 6) * this._dwcRandomScale;
            LogClass.LogWrite("FSXWXlogDwcSpd.txt", "harsh random gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
          }
          else if (random.Next(0, 100) > 60)
          {
            num2 = (double) random.Next(6000, 20000);
            num3 = (double) random.Next(0, 4) * this._dwcRandomScale;
            LogClass.LogWrite("FSXWXlogDwcSpd.txt", "slow random gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
          }
          else
          {
            num2 = 10000.0;
            num3 = 0.0;
            LogClass.LogWrite("FSXWXlogDwcSpd.txt", "no random gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
          }
        }
        else
        {
          num2 = 10000.0;
          num3 = 0.0;
          LogClass.LogWrite("FSXWXlogDwcSpd.txt", "no gust: " + num3.ToString() + "kts/" + num2.ToString() + "ms");
        }
        int num4 = random.Next(1, 5);
        double num5 = num2 / (double) this.WindSpdWriteTimer.Interval;
        for (double num6 = 0.0; num6 < num5; ++num6)
        {
          int int32 = Convert.ToInt32(Math.Min(Math.Round(100.0 / num5 * num6), 99.0));
          switch (num4)
          {
            case 1:
              num1 = this.sinCurve[int32] * num3;
              break;
            case 2:
              num1 = this.centerPeakCurve[int32] * num3;
              break;
            case 3:
              num1 = this.frontPeakCurve[int32] * num3;
              break;
            case 4:
              num1 = this.backPeakCurve[int32] * num3;
              break;
          }
          num1 = Math.Round(Math.Max(-1.0 / this._windgusts.maxHeightSpd * this._acPos.altGnd + 1.0, 0.0) * num1);
          this._windgusts.spdTickList.Add(num1);
        }
        this.GustsSpdWriteTimer.Start();
        this.GustsSpdTimer.Stop();
      }
      catch
      {
      }
    }

    private void GustsSpdWriteTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        this._windgusts.spd = this._windgusts.spdTickList[this._windgusts.tickListPos];
        ++this._windgusts.tickListPos;
        if (this._windgusts.tickListPos != this._windgusts.spdTickList.Count<double>())
          return;
        this.GustsSpdTimer.Start();
        this.GustsSpdWriteTimer.Stop();
      }
      catch
      {
      }
    }

    private void GustsDirTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        if (this.GustsDirWriteTimer.Enabled)
          return;
        Random random = new Random();
        this._windgusts.dirTickList.Clear();
        this._windgusts.dirTickListPos = 0;
        double num1 = random.Next(0, 2) == 1 ? -1.0 : 1.0;
        double num2 = (double) this._windinfo.gusts - this._windinfo.windSpdGnd <= 18.0 ? ((double) this._windinfo.gusts - this._windinfo.windSpdGnd <= 12.0 ? 1.0 : 1.5) : 2.0;
        double num3 = this._windinfo.dirDiff <= 80 ? (this._windinfo.dirDiff <= 40 ? 1.0 : 1.5) : 2.0;
        double num4;
        double num5;
        if (random.Next(0, 100) > 60)
        {
          num4 = (double) random.Next(1000, 16000);
          num5 = 0.0;
          LogClass.LogWrite("FSXWXlogDwcDir.txt", "pause gustvar: " + num5.ToString() + "°/" + num4.ToString() + "ms");
        }
        else if (this._dwcGustsDirScale > 0.0 && this._windinfo.gusts > 0 && ((double) this._windinfo.gusts <= this._windinfo.windSpdGnd && this._acPos.altGnd < this._windgusts.maxHeightDir))
        {
          num4 = (double) random.Next(1000, 14000);
          num5 = (double) random.Next(1, 20) * num3 * num1 * this._dwcGustsDirScale;
          LogClass.LogWrite("FSXWXlogDwcDir.txt", "gust <= windspd gustvar: " + num5.ToString() + "°/" + num4.ToString() + "ms");
        }
        else if (this._dwcGustsDirScale > 0.0 && (double) this._windinfo.gusts > this._windinfo.windSpdGnd && this._acPos.altGnd < this._windgusts.maxHeightDir)
        {
          if (random.Next(0, 100) > 40)
          {
            num4 = (double) random.Next(1000, 10000);
            num5 = (double) random.Next(1, 20) * num3 * num2 * num1 * this._dwcGustsDirScale;
            LogClass.LogWrite("FSXWXlogDwcDir.txt", "small gustvar: " + num5.ToString() + "°/" + num4.ToString() + "ms");
          }
          else
          {
            num4 = (double) random.Next(6000, 12000);
            num5 = (double) random.Next(1, 60) * num3 * num2 * num1 * this._dwcGustsDirScale;
            LogClass.LogWrite("FSXWXlogDwcDir.txt", "large gustvar: " + num5.ToString() + "°/" + num4.ToString() + "ms");
          }
        }
        else
        {
          num4 = 10000.0;
          num5 = 0.0;
          LogClass.LogWrite("FSXWXlogDwcDir.txt", "no gustvar: " + num5.ToString() + "°/" + num4.ToString() + "ms");
        }
        double num6 = num4 / (double) this.WindDirWriteTimer.Interval;
        for (double num7 = 0.0; num7 < num6; ++num7)
          this._windgusts.dirTickList.Add(Math.Round(Math.Max(-1.0 / this._windgusts.maxHeightDir * this._acPos.altGnd + 1.0, 0.0) * (this.sinCurve[Convert.ToInt32(Math.Min(Math.Round(100.0 / num6 * num7), 99.0))] * num5)));
        this.GustsDirWriteTimer.Start();
        this.GustsDirTimer.Stop();
      }
      catch
      {
      }
    }

    private void GustsDirWriteTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        this._windgusts.dir = this._windgusts.dirTickList[this._windgusts.dirTickListPos];
        ++this._windgusts.dirTickListPos;
        if (this._windgusts.dirTickListPos != this._windgusts.dirTickList.Count<double>())
          return;
        this.GustsDirTimer.Start();
        this.GustsDirWriteTimer.Stop();
      }
      catch
      {
      }
    }

    private void TurbWriteTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        Random random = new Random();
        double num1 = Math.Min(Math.Max(this._windinfo.turbWindStrength, this._windinfo.turbCloudStrength), 3.0);
        double num2 = Math.Max(-1.0 / this._windturb.maxHeight * this._acPos.altGnd + 1.0, 0.0);
        if (num1 > 0.0 && this._dwcTurbScale > 0.0 && this._acPos.altGnd < this._windturb.maxHeight)
        {
          this._windturb.spd = num2 * Math.Round((double) random.Next((int) (-10.0 * this._dwcTurbScale * num1), (int) (10.0 * this._dwcTurbScale * num1)) / 10.0);
          this._windturb.spd = Math.Round(this._windturb.spd);
        }
        else
          this._windturb.spd = 0.0;
      }
      catch
      {
      }
    }

    private void TurbWriteOnOffTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        if (new Random().Next(0, 100) > 80)
        {
          if (!this.TurbWriteTimer.Enabled)
            return;
          this.TurbWriteTimer.Stop();
        }
        else
        {
          if (this.TurbWriteTimer.Enabled)
            return;
          this.TurbWriteTimer.Start();
        }
      }
      catch
      {
      }
    }

    public struct wxAcPos
    {
      public double lat;
      public double lon;
      public double altGnd;
    }

    public struct wxWindInfo
    {
      public double turbCloudStrength;
      public double turbWindStrength;
      public int gusts;
      public int dirDiff;
      public int var;
      public bool vrb;
      public double windSpdGnd;
    }

    public struct wxWindTurb
    {
      public double spd;
      public double maxHeight;
    }

    public struct wxWind
    {
      public double spd;
      public double spdLast;
      public double spdWrite;
      public double dir;
      public double dirLast;
      public double dirWrite;
      public double magVar;
    }

    public struct wxWindGusts
    {
      public double dir;
      public double spd;
      public double maxHeightSpd;
      public double maxHeightDir;
      public List<double> spdTickList;
      public int tickListPos;
      public List<double> dirTickList;
      public int dirTickListPos;
    }

    public struct wxWindVar
    {
      public double dir;
      public double maxHeight;
      public List<double> dirTickList;
      public int dirTickListPos;
    }
  }
}
