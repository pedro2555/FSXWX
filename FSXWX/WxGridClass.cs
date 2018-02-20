// Decompiled with JetBrains decompiler
// Type: FSXWX.WxGridClass
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace FSXWX
{
  public class WxGridClass
  {
    private int[] _aloftlevelsStd = new int[10]
    {
      30,
      60,
      90,
      120,
      180,
      240,
      300,
      340,
      390,
      430
    };
    private int[] _aloftlevelsOut = new int[20]
    {
      0,
      10,
      20,
      30,
      40,
      50,
      60,
      70,
      80,
      100,
      120,
      140,
      160,
      180,
      200,
      240,
      280,
      320,
      360,
      400
    };
    private string _adep = "";
    private string _ades = "";
    private string _altn = "";
    private string _datetimeStamp = "";
    private string _datetimeHour = "";
    private List<WxGridClass.wxGridListStruct> _wxGridListAllPts = new List<WxGridClass.wxGridListStruct>();
    private bool _truewindsaloft = true;
    private bool _fsxinternalwindturb = true;
    private bool _fsxinternalcloudturb = true;
    private float _ttgeneralThreshold = 29.92f;
    private List<string> _createdWxGridPts = new List<string>();
    private List<string> _newWxGridPts = new List<string>();
    private List<WxGridClass.wxStruct> _wxAll = new List<WxGridClass.wxStruct>();
    private List<WxGridClass.wxStruct> _wxMetar = new List<WxGridClass.wxStruct>();
    private List<WxGridClass.wxStruct> _wxGrib = new List<WxGridClass.wxStruct>();
    private List<WxGridClass.wxStruct> _wxNew = new List<WxGridClass.wxStruct>();
    private List<WxGridClass.wxStruct> _wxGrid = new List<WxGridClass.wxStruct>();
    private const int _visFrL_Std = 30;
    private const int _visToL_Std = 44;
    private const int _visToU_Std = 60;
    private const int _layerL_Std = 7;
    private const int _layerU_Std = 3;
    private const int _visTrl_Std = 16000;
    private string _metarLatestTime;
    private string _windsaloftLatestTime;
    private string _gribLatestTime;
    private DateTime _datetime;
    private string _ttgeneral;

    public string adep
    {
      get
      {
        return this._adep;
      }
      set
      {
        this._adep = value;
      }
    }

    public string ades
    {
      get
      {
        return this._ades;
      }
      set
      {
        this._ades = value;
      }
    }

    public string altn
    {
      get
      {
        return this._altn;
      }
      set
      {
        this._altn = value;
      }
    }

    public DateTime datetime
    {
      get
      {
        return this._datetime;
      }
      set
      {
        this._datetime = value;
        this._datetimeStamp = this._datetime.Year.ToString() + string.Format("{0:00}", (object) this._datetime.Month) + string.Format("{0:00}", (object) this._datetime.Day);
        this._datetimeHour = string.Format("{0:00}", (object) this._datetime.Hour);
      }
    }

    public bool truewindsaloft
    {
      get
      {
        return this._truewindsaloft;
      }
      set
      {
        this._truewindsaloft = value;
      }
    }

    public bool fsxinternalwindturb
    {
      get
      {
        return this._fsxinternalwindturb;
      }
      set
      {
        this._fsxinternalwindturb = value;
      }
    }

    public bool fsxinternalcloudturb
    {
      get
      {
        return this._fsxinternalcloudturb;
      }
      set
      {
        this._fsxinternalcloudturb = value;
      }
    }

    public string ttgeneral
    {
      get
      {
        return this._ttgeneral;
      }
      set
      {
        this._ttgeneral = value;
      }
    }

    public float ttgeneralthreshold
    {
      get
      {
        return this._ttgeneralThreshold;
      }
      set
      {
        this._ttgeneralThreshold = value;
      }
    }

    public List<WxGridClass.wxStruct> wxAll
    {
      get
      {
        return this._wxAll;
      }
      set
      {
        this._wxAll = value;
      }
    }

    public List<WxGridClass.wxStruct> wxMetar
    {
      get
      {
        return this._wxMetar;
      }
      set
      {
        this._wxMetar = value;
      }
    }

    public List<WxGridClass.wxStruct> wxGrib
    {
      get
      {
        return this._wxGrib;
      }
      set
      {
        this._wxGrib = value;
      }
    }

    public List<WxGridClass.wxStruct> wxNew
    {
      get
      {
        return this._wxNew;
      }
      set
      {
        this._wxNew = value;
      }
    }

    public List<WxGridClass.wxStruct> wxGrid
    {
      get
      {
        return this._wxGrid;
      }
      set
      {
        this._wxGrid = value;
      }
    }

    public WxGridClass()
    {
      double distanceKilometres = 85.0;
      GeoClass.Coord startPoint = new GeoClass.Coord();
      GeoClass.Coord coord1 = new GeoClass.Coord();
      startPoint.lat = -58.0;
      startPoint.lon = -180.0;
      int num = 0;
      do
      {
        GeoClass.Coord coord2;
        do
        {
          string str = string.Format("{0:X4}", (object) num);
          this._wxGridListAllPts.Add(new WxGridClass.wxGridListStruct()
          {
            id = str,
            lat = (float) startPoint.lat,
            lon = (float) startPoint.lon
          });
          coord2 = GeoClass.PointAtDistance(startPoint, GeoClass.ToRad(90.0), distanceKilometres);
          startPoint.lon = coord2.lon;
          ++num;
        }
        while (startPoint.lon < 180.0);
        coord2 = GeoClass.PointAtDistance(startPoint, GeoClass.ToRad(0.0), distanceKilometres);
        startPoint.lat = coord2.lat;
        startPoint.lon = -180.0;
      }
      while (startPoint.lat < 89.0);
    }

    public string WxGridCreate(double aclat, double aclon, double radius, bool clearGrid, string simChoice)
    {
      string str1 = "";
      string str2;
      try
      {
        str2 = str1 + (object) this._wxMetar.Count<WxGridClass.wxStruct>() + " METAR reports + " + (object) this._wxGrib.Count<WxGridClass.wxStruct>() + " GRIB points processed - " + this.WxGridInterpolate(aclat, aclon, radius, clearGrid);
        this.WxGridSkyCondition(aclat, aclon);
        this.WxGridSmooth();
        this.WxGridFsxString(simChoice);
      }
      catch
      {
        return "WX DATA processing failed";
      }
      return str2;
    }

    public string WxGridWindUpdate(double aclat, double aclon, double radius)
    {
      string str1 = "";
      List<WxGridClass.wxStruct> wxData = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct row = new WxGridClass.wxStruct();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      WxGridClass.weightingStruct weighting = new WxGridClass.weightingStruct();
      List<double> doubleList = new List<double>();
      int num = 0;
      try
      {
        string str2 = str1 + (object) this._wxMetar.Count<WxGridClass.wxStruct>() + " METAR reports + " + (object) this._wxGrib.Count<WxGridClass.wxStruct>() + " GRIB points processed for DWC";
        for (int index1 = 0; index1 < this._wxGrid.Count; ++index1)
        {
          row = this._wxGrid[index1];
          if (GeoClass.Dist(aclat, aclon, (double) row.lat, (double) row.lon) <= radius)
          {
            wxData.Clear();
            wxData = this.FetchWxDataNearest(this._wxNew, (double) row.lat, (double) row.lon, 4);
            if (this._wxNew.Exists((Predicate<WxGridClass.wxStruct>) (j => j.station_id == row.station_id)) && (row.station_id == this._adep || row.station_id == this._ades || row.station_id == this._altn))
            {
              WxGridClass.wxStruct wxStruct2 = this._wxNew.Find((Predicate<WxGridClass.wxStruct>) (j => j.station_id == row.station_id));
              row.winds_aloft_ext = wxStruct2.winds_aloft_ext;
              row.winds_aloft_ext = this.LimitWindsAloftTransitions(row.winds_aloft_ext);
              row.wind_variance = wxStruct2.wind_variance;
              row.wind_gusts = wxStruct2.wind_gusts;
              row.wind_dir_degrees = wxStruct2.wind_dir_degrees;
              row.wind_speed_kt = wxStruct2.wind_speed_kt;
              row.wind_vrb = wxStruct2.wind_vrb;
              str2 = str2 + " - " + row.station_id + " updated";
            }
            else
            {
              weighting.sum_dist = 0.0;
              weighting.sum_weights = 0.0;
              weighting.num_wind_gusts = 0.0;
              weighting.num_wind_variance = 0.0;
              doubleList.Clear();
              for (int index2 = 0; index2 < wxData.Count; ++index2)
                weighting.sum_dist += wxData[index2].dist_from_ac;
              for (int index2 = 0; index2 < wxData.Count; ++index2)
                doubleList.Add(1.0 - wxData[index2].dist_from_ac / weighting.sum_dist);
              weighting.sum_weights = doubleList.Sum();
              for (int index2 = 0; index2 < wxData.Count; ++index2)
              {
                weighting.num_wind_gusts += doubleList[index2] * (double) wxData[index2].wind_gusts;
                weighting.num_wind_variance += doubleList[index2] * (double) wxData[index2].wind_variance;
              }
              row.wind_gusts = (int) (weighting.num_wind_gusts / weighting.sum_weights);
              row.wind_variance = (int) (weighting.num_wind_variance / weighting.sum_weights);
              row.wind_dir_degrees = wxData[0].wind_dir_degrees;
              row.wind_speed_kt = wxData[0].wind_speed_kt;
              row.winds_aloft_ext = this.InterpolateWindsAloft(wxData, weighting, doubleList, 1.0, 1.0);
              row.winds_aloft_ext = this.LimitWindsAloftTransitions(row.winds_aloft_ext);
            }
            this._wxGrid[index1] = row;
            ++num;
          }
        }
        return str2 + " - " + (object) num + " grid points updated";
      }
      catch
      {
        return "WX DATA processing for DWC failed";
      }
    }

    private string WxGridInterpolate(double aclat, double aclon, double radius, bool clearGrid)
    {
      string str1 = "";
      List<WxGridClass.wxStruct> wxData = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      WxGridClass.weightingStruct weighting = new WxGridClass.weightingStruct();
      List<double> doubleList = new List<double>();
      if (clearGrid)
      {
        this._wxGrid.Clear();
        this._createdWxGridPts.Clear();
      }
      this._newWxGridPts.Clear();
      List<WxGridClass.wxGridListStruct> wxGridListStructList = new List<WxGridClass.wxGridListStruct>();
      foreach (WxGridClass.wxGridListStruct wxGridListStruct in this.CreateWxGridListExtract(this._wxGridListAllPts, aclat, aclon, radius))
      {
        if (!this._createdWxGridPts.Contains(wxGridListStruct.id))
        {
          wxData.Clear();
          wxData = this.FetchWxDataNearest(this._wxNew, (double) wxGridListStruct.lat, (double) wxGridListStruct.lon, 4);
          weighting.sum_dist = 0.0;
          weighting.sum_weights = 0.0;
          weighting.num_temp_c = 0.0;
          weighting.num_dewpoint_c = 0.0;
          weighting.num_humidity = 0.0;
          weighting.num_visibility_statute_mi = 0.0;
          weighting.num_altim_in_hg = 0.0;
          weighting.num_elevation_m = 0.0;
          weighting.num_wind_gusts = 0.0;
          weighting.num_wind_variance = 0.0;
          doubleList.Clear();
          for (int index = 0; index < wxData.Count; ++index)
            weighting.sum_dist += wxData[index].dist_from_ac;
          for (int index = 0; index < wxData.Count; ++index)
            doubleList.Add(1.0 - wxData[index].dist_from_ac / weighting.sum_dist);
          weighting.sum_weights = doubleList.Sum();
          for (int index = 0; index < wxData.Count; ++index)
          {
            weighting.num_temp_c += doubleList[index] * (double) wxData[index].temp_c;
            weighting.num_dewpoint_c += doubleList[index] * (double) wxData[index].dewpoint_c;
            weighting.num_humidity += doubleList[index] * (double) wxData[index].humidity;
            weighting.num_visibility_statute_mi += doubleList[index] * (double) wxData[index].visibility_statute_mi;
            weighting.num_altim_in_hg += doubleList[index] * (double) wxData[index].altim_in_hg;
            weighting.num_elevation_m += doubleList[index] * (double) wxData[index].elevation_m;
            weighting.num_wind_gusts += doubleList[index] * (double) wxData[index].wind_gusts;
            weighting.num_wind_variance += doubleList[index] * (double) wxData[index].wind_variance;
          }
          wxStruct1.station_id = wxGridListStruct.id;
          wxStruct1.lat = wxGridListStruct.lat;
          wxStruct1.lon = wxGridListStruct.lon;
          wxStruct1.raw_text = wxData[0].raw_text;
          wxStruct1.raw_time = wxData[0].raw_time;
          string str2 = "";
          foreach (WxGridClass.wxStruct wxStruct2 in wxData)
            str2 = str2 + wxStruct2.station_id + " ";
          wxStruct1.interpolated_from = str2;
          wxStruct1.temp_c = (int) (weighting.num_temp_c / weighting.sum_weights);
          wxStruct1.dewpoint_c = (int) (weighting.num_dewpoint_c / weighting.sum_weights);
          wxStruct1.humidity = (int) (weighting.num_humidity / weighting.sum_weights);
          wxStruct1.visibility_statute_mi = (float) (Math.Round(weighting.num_visibility_statute_mi / weighting.sum_weights * 100.0) / 100.0);
          wxStruct1.altim_in_hg = (float) (Math.Round(weighting.num_altim_in_hg / weighting.sum_weights * 100.0) / 100.0);
          wxStruct1.elevation_m = (int) (weighting.num_elevation_m / weighting.sum_weights);
          wxStruct1.wx_string = wxData[0].wx_string;
          wxStruct1.sky_condition = this.ChooseSkyCondition(wxData);
          wxStruct1.wind_gusts = (int) (weighting.num_wind_gusts / weighting.sum_weights);
          wxStruct1.wind_variance = (int) (weighting.num_wind_variance / weighting.sum_weights);
          wxStruct1.wind_dir_degrees = wxData[0].wind_dir_degrees;
          wxStruct1.wind_speed_kt = wxData[0].wind_speed_kt;
          wxStruct1.winds_aloft_ext = this.InterpolateWindsAloft(wxData, weighting, doubleList, 1.0, 1.0);
          wxStruct1.winds_aloft_ext = this.LimitWindsAloftTransitions(wxStruct1.winds_aloft_ext);
          this._wxGrid.Add(wxStruct1);
          this._createdWxGridPts.Add(wxStruct1.station_id);
          this._newWxGridPts.Add(wxStruct1.station_id);
        }
      }
      for (int index = 0; index < this._wxNew.Count; ++index)
      {
        WxGridClass.wxStruct wxStruct2 = this._wxNew[index];
        if ((wxStruct2.station_id == this._adep || wxStruct2.station_id == this._ades || wxStruct2.station_id == this._altn) && !this._createdWxGridPts.Contains(wxStruct2.station_id))
        {
          str1 = str1 + wxStruct2.station_id + " added to grid - ";
          wxStruct2.interpolated_from = wxStruct2.station_id;
          wxStruct2.winds_aloft_ext = this.LimitWindsAloftTransitions(wxStruct2.winds_aloft_ext);
          this._wxGrid.Add(wxStruct2);
          this._createdWxGridPts.Add(wxStruct2.station_id);
          this._newWxGridPts.Add(wxStruct2.station_id);
          wxStructList.Clear();
          wxStructList = this.FetchWxDataNearest(this._wxGrid, (double) wxStruct2.lat, (double) wxStruct2.lon, 4);
          foreach (WxGridClass.wxStruct wxStruct3 in wxStructList)
          {
            WxGridClass.wxStruct row = wxStruct3;
            if (this._createdWxGridPts.Contains(row.station_id) && GeoClass.Dist((double) wxStruct2.lat, (double) wxStruct2.lon, (double) row.lat, (double) row.lon) < 20.0)
            {
              str1 = str1 + row.station_id + " removed from grid because nearby " + wxStruct2.station_id + " - ";
              this._wxGrid.Remove(this._wxGrid.Find((Predicate<WxGridClass.wxStruct>) (j => j.station_id == row.station_id)));
              this._createdWxGridPts.Remove(row.station_id);
              this._newWxGridPts.Remove(row.station_id);
            }
          }
        }
      }
      if (this._adep != "")
      {
        if (!this._wxAll.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == this._adep)))
          str1 = str1 + this._adep + " not found - ";
        else if (!this._createdWxGridPts.Contains(this._adep))
          str1 = str1 + this._adep + " still too far away - ";
      }
      if (this._ades != "")
      {
        if (!this._wxAll.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == this._ades)))
          str1 = str1 + this._ades + " not found - ";
        else if (!this._createdWxGridPts.Contains(this._ades))
          str1 = str1 + this._ades + " still too far away - ";
      }
      if (this._altn != "")
      {
        if (!this._wxAll.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == this._altn)))
          str1 = str1 + this._altn + " not found - ";
        else if (!this._createdWxGridPts.Contains(this._altn))
          str1 = str1 + this._altn + " still too far away - ";
      }
      return str1 + (object) this._createdWxGridPts.Count + " points in total - " + (object) this._newWxGridPts.Count + " new points created";
    }

    private void WxGridSkyCondition(double aclat, double aclon)
    {
      List<WxGridClass.wxStruct> wxData = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      Random random = new Random();
      int[] numArray = new int[8]{ 2, 2, 2, 4, 4, 4, 7, 8 };
      for (int index1 = 0; index1 < this._wxGrid.Count; ++index1)
      {
        if (this._newWxGridPts.Contains(this._wxGrid[index1].station_id))
        {
          WxGridClass.wxStruct wxStruct2 = this._wxGrid[index1];
          wxData.Clear();
          wxData = this.FetchWxDataNearest(this._wxNew, (double) wxStruct2.lat, (double) wxStruct2.lon, 4);
          wxStruct2.sky_fsx_native = wxStruct2.sky_condition;
          this._ttgeneral = ((double) wxStruct2.altim_in_hg + (double) wxData[0].altim_in_hg + (double) wxData[1].altim_in_hg + (double) wxData[2].altim_in_hg + (double) wxData[3].altim_in_hg) / 5.0 > (double) this._ttgeneralThreshold ? "CU" : "AS";
          if (Regex.IsMatch(wxStruct2.sky_fsx_native, "(CLR|SKC)"))
            wxStruct2.sky_fsx_native = "";
          if (wxStruct2.sky_fsx_native == "CAVOK" || wxStruct2.sky_fsx_native == "NSC" || wxStruct2.sky_fsx_native == "NCD")
          {
            int index2 = 0;
            while (wxData[index2].sky_condition != null)
            {
              wxStruct2.sky_fsx_native = wxData[index2].sky_condition;
              ++index2;
              wxStruct2.sky_fsx_native = Regex.Replace(wxStruct2.sky_fsx_native, "(FEW|SCT|BKN|OVC)0[01234]\\d\\s*", "");
              wxStruct2.sky_fsx_native = wxStruct2.sky_fsx_native.Trim();
              if (Regex.IsMatch(wxStruct2.sky_fsx_native, "(CLR|SKC)"))
              {
                wxStruct2.sky_fsx_native = "";
                goto label_13;
              }
              else if (index2 == 4)
              {
                wxStruct2.sky_fsx_native = random.Next(0, 100) <= 50 ? "" : (random.Next(0, 100) < 40 ? "3CI0" + (object) random.Next(5, 9) + "0" : "1CI0" + (object) random.Next(5, 9) + "0");
                goto label_13;
              }
              else if (!Regex.IsMatch(wxStruct2.sky_fsx_native, "(CAVOK|NSC|NCD)") && !(wxStruct2.sky_fsx_native == ""))
                goto label_13;
            }
            wxStruct2.sky_fsx_native = "";
          }
label_13:
          foreach (Match match in new Regex("\\b((FEW|SCT|BKN|OVC)(\\d\\d\\d))(CB|TCU)\\b", RegexOptions.Compiled).Matches(wxStruct2.raw_text))
            wxStruct2.sky_fsx_native = Regex.IsMatch(wxData[0].wx_string, "TS") || Regex.IsMatch(wxData[1].wx_string, "TS") || (Regex.IsMatch(wxData[2].wx_string, "TS") || Regex.IsMatch(wxData[3].wx_string, "TS")) ? Regex.Replace(wxStruct2.sky_fsx_native, "\\b" + match.Groups[1].Value + "(?!&)", match.Groups[1].Value + "&CB") : Regex.Replace(wxStruct2.sky_fsx_native, "\\b" + match.Groups[1].Value + "(?!&)", match.Groups[1].Value + "&CU");
          if ((double) wxStruct2.visibility_statute_mi < 20.0)
          {
            Regex regex = new Regex("\\b(\\w\\w\\w)(0[01234]\\d(?!&))", RegexOptions.Compiled);
            if (regex.Matches(wxStruct2.sky_fsx_native).Count > 0)
            {
              Match match = regex.Matches(wxStruct2.sky_fsx_native)[regex.Matches(wxStruct2.sky_fsx_native).Count - 1];
              if ((double) wxStruct2.visibility_statute_mi < 8.0 && !Regex.IsMatch(wxStruct2.sky_fsx_native, "\\b(OVC)(0[01234]\\d)"))
                wxStruct2.sky_fsx_native = Regex.Replace(wxStruct2.sky_fsx_native, match.ToString(), "8" + this._ttgeneral + match.Groups[2].Value);
              else if ((double) wxStruct2.visibility_statute_mi < 14.0 && !Regex.IsMatch(wxStruct2.sky_fsx_native, "\\b(BKN|OVC)(0[01234]\\d)"))
                wxStruct2.sky_fsx_native = Regex.Replace(wxStruct2.sky_fsx_native, match.ToString(), "6" + this._ttgeneral + match.Groups[2].Value);
              else if ((double) wxStruct2.visibility_statute_mi < 20.0 && !Regex.IsMatch(wxStruct2.sky_fsx_native, "\\b(SCT|BKN|OVC)(0[01234]\\d)"))
                wxStruct2.sky_fsx_native = Regex.Replace(wxStruct2.sky_fsx_native, match.ToString(), "2AS" + match.Groups[2].Value);
            }
            else
            {
              wxStruct2.sky_fsx_native = (double) wxStruct2.visibility_statute_mi >= 8.0 ? ((double) wxStruct2.visibility_statute_mi >= 14.0 ? "2AS016 " + wxStruct2.sky_fsx_native : "6AS016 " + wxStruct2.sky_fsx_native) : "8AS016 " + wxStruct2.sky_fsx_native;
              wxStruct2.sky_fsx_native = wxStruct2.sky_fsx_native.Trim();
            }
          }
          if (Regex.IsMatch(wxStruct2.wx_string, "(DZ|SH|RA)") && !Regex.IsMatch(wxStruct2.sky_fsx_native, "(FEW|SCT|BKN|OVC)(0[012345678]\\d)"))
          {
            if (this._ttgeneral == "AS")
              wxStruct2.sky_fsx_native = "SCT050&AS " + wxStruct2.sky_fsx_native;
            else if (this._ttgeneral == "CU")
              wxStruct2.sky_fsx_native = "SCT030&CU " + wxStruct2.sky_fsx_native;
            wxStruct2.sky_fsx_native = wxStruct2.sky_fsx_native.Trim();
          }
          wxStruct2.sky_fsx_native = this.TransformSkyStringAndSetCoverage(wxStruct2.sky_fsx_native, wxData);
          wxStruct2.sky_fsx_native = this.SortSkyString(wxStruct2.sky_fsx_native);
          foreach (Match match1 in new Regex("([8])(AS)(0[012345]\\d)", RegexOptions.Compiled).Matches(wxStruct2.sky_fsx_native))
          {
            if (match1.Success && Regex.Matches(wxStruct2.sky_fsx_native, "(AS|CU)").Count <= 2)
            {
              Match match2 = Regex.Match(wxStruct2.sky_fsx_native, match1.Groups[0].Value + "\\s(\\d)(\\w\\w)(\\d\\d\\d)");
              if (!match2.Success || Convert.ToInt32(match2.Groups[3].Value) - Convert.ToInt32(match1.Groups[3].Value) > 60)
              {
                wxStruct2.sky_fsx_native = Regex.Replace(wxStruct2.sky_fsx_native, match1.Groups[0].Value, match1.Groups[0].Value + " " + match1.Groups[1].Value + match1.Groups[2].Value + string.Format("{0:000}", (object) (Convert.ToInt32(match1.Groups[3].Value) + 26)));
                break;
              }
            }
          }
          wxStruct2.sky_fsx_native = this.SortSkyString(wxStruct2.sky_fsx_native);
          Match match3 = Regex.Match(wxStruct2.sky_fsx_native, "(\\d)(\\w\\w)(\\d\\d\\d)", RegexOptions.RightToLeft);
          if (match3.Success && Convert.ToInt32(match3.Groups[3].Value) < 200 && (match3.Groups[2].Value == "AS" && random.Next(0, 100) > 90 || match3.Groups[2].Value == "CU" && random.Next(0, 100) > 70))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            string& local = @wxStruct2.sky_fsx_native;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^local = ^local + " " + numArray[random.Next(0, 8)].ToString() + "CI" + random.Next(22, 28).ToString() + "0";
            wxStruct2.sky_fsx_native = wxStruct2.sky_fsx_native.Trim();
          }
          if (random.Next(0, 100) > 90)
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            string& local = @wxStruct2.sky_fsx_native;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^local = ^local + " " + numArray[random.Next(0, 8)].ToString() + "CI" + random.Next(44, 48).ToString() + "0";
            wxStruct2.sky_fsx_native = wxStruct2.sky_fsx_native.Trim();
          }
          wxStruct2.wx_string_fsx = wxStruct2.wx_string;
          if (wxStruct2.wx_string_fsx != "")
          {
            wxStruct2.wx_string_fsx = Regex.Replace(wxStruct2.wx_string_fsx, "(VC|BC|GS)", "");
            wxStruct2.wx_string_fsx = Regex.Replace(wxStruct2.wx_string_fsx, "(DZ|SH)", "RA");
            wxStruct2.wx_string_fsx = Regex.Replace(wxStruct2.wx_string_fsx, "RARA", "RA");
            wxStruct2.wx_string_fsx = wxStruct2.wx_string_fsx.Trim();
          }
          this._wxGrid[index1] = wxStruct2;
        }
      }
    }

    private void WxGridSmooth()
    {
      List<WxGridClass.wxStruct> wxStructList = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      Random random = new Random();
      for (int index1 = 0; index1 < this._wxGrid.Count; ++index1)
      {
        if (this._newWxGridPts.Contains(this._wxGrid[index1].station_id))
        {
          WxGridClass.wxStruct wxStruct2 = this._wxGrid[index1];
          wxStruct2.sky_fsx_smooth = wxStruct2.sky_fsx_native;
          string str1 = "";
          wxStructList.Clear();
          wxStructList = this.FetchWxDataNearest(this._wxGrid, (double) wxStruct2.lat, (double) wxStruct2.lon, 4);
          foreach (WxGridClass.wxStruct wxStruct3 in wxStructList)
            str1 = str1 + wxStruct3.sky_fsx_native + " ";
          string input = str1.Trim();
          int num;
          for (int index2 = 3; index2 <= 16; ++index2)
          {
            string str2 = string.Format("{0:00}", (object) (index2 - 4));
            string str3 = string.Format("{0:00}", (object) (index2 - 3));
            string str4 = string.Format("{0:00}", (object) (index2 - 2));
            string str5 = string.Format("{0:00}", (object) (index2 - 1));
            string str6 = string.Format("{0:00}", (object) index2);
            string str7 = string.Format("{0:00}", (object) (index2 + 1));
            string str8 = string.Format("{0:00}", (object) (index2 + 2));
            string str9 = string.Format("{0:00}", (object) (index2 + 3));
            string str10 = string.Format("{0:00}", (object) (index2 + 4));
            if (!Regex.IsMatch(wxStruct2.sky_fsx_native, "(\\w\\w)(" + str2 + "|" + str3 + "|" + str4 + "|" + str5 + "|" + str6 + "|" + str7 + "|" + str8 + "|" + str9 + "|" + str10 + ")\\d") && Regex.Matches(wxStruct2.sky_fsx_native, "(AS|CU)").Count <= 2)
            {
              Match match1 = Regex.Match(input, "([5678])(AS|CU)(" + str6 + "\\d)");
              if (match1.Success)
              {
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                string& local1 = @wxStruct2.sky_fsx_smooth;
                // ISSUE: variable of a reference type
                string& local2 = local1;
                // ISSUE: explicit reference operation
                string[] strArray = new string[6]
                {
                  ^local1,
                  " ",
                  null,
                  null,
                  null,
                  null
                };
                int index3 = 2;
                num = Convert.ToInt32(match1.Groups[1].Value) - 2;
                string str11 = num.ToString();
                strArray[index3] = str11;
                strArray[3] = match1.Groups[2].Value;
                strArray[4] = string.Format("{0:00}", (object) index2);
                strArray[5] = "0";
                string str12 = string.Concat(strArray);
                // ISSUE: explicit reference operation
                ^local2 = str12;
                index2 += 5;
              }
              Match match2 = Regex.Match(input, "([34])(CU)(" + str6 + "\\d)");
              if (match2.Success)
              {
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                string& local1 = @wxStruct2.sky_fsx_smooth;
                // ISSUE: variable of a reference type
                string& local2 = local1;
                // ISSUE: explicit reference operation
                string[] strArray = new string[6]
                {
                  ^local1,
                  " ",
                  null,
                  null,
                  null,
                  null
                };
                int index3 = 2;
                num = Convert.ToInt32(match2.Groups[1].Value) - 2;
                string str11 = num.ToString();
                strArray[index3] = str11;
                strArray[3] = "CU";
                strArray[4] = string.Format("{0:00}", (object) index2);
                strArray[5] = "0";
                string str12 = string.Concat(strArray);
                // ISSUE: explicit reference operation
                ^local2 = str12;
                index2 += 5;
              }
            }
          }
          wxStruct2.sky_fsx_smooth = this.SortSkyString(wxStruct2.sky_fsx_smooth);
          foreach (Match match1 in new Regex("(\\d)(CU)(\\d\\d\\d)", RegexOptions.Compiled).Matches(wxStruct2.sky_fsx_smooth))
          {
            if (match1.Success && Convert.ToInt32(match1.Groups[3].ToString()) > 300)
            {
              if (Convert.ToInt32(match1.Groups[1].Value) > 4)
              {
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                WxGridClass.wxStruct& local = @wxStruct2;
                string skyFsxSmooth = wxStruct2.sky_fsx_smooth;
                string pattern = match1.Groups[0].Value;
                string[] strArray = new string[6]
                {
                  "8CI",
                  match1.Groups[3].Value,
                  " 7CI",
                  null,
                  null,
                  null
                };
                int index2 = 3;
                num = Convert.ToInt32(match1.Groups[3].Value) + 10;
                string str2 = num.ToString();
                strArray[index2] = str2;
                strArray[4] = " 4CI";
                int index3 = 5;
                num = Convert.ToInt32(match1.Groups[3].Value) + 30;
                string str3 = num.ToString();
                strArray[index3] = str3;
                string replacement = string.Concat(strArray);
                string str4 = Regex.Replace(skyFsxSmooth, pattern, replacement);
                // ISSUE: explicit reference operation
                (^local).sky_fsx_smooth = str4;
              }
              else
                wxStruct2.sky_fsx_smooth = Regex.Replace(wxStruct2.sky_fsx_smooth, match1.Groups[0].Value, match1.Groups[1].Value + "AS" + match1.Groups[3].Value);
            }
            else if (match1.Success && Convert.ToInt32(match1.Groups[3].ToString()) > 220)
              wxStruct2.sky_fsx_smooth = Regex.Replace(wxStruct2.sky_fsx_smooth, match1.Groups[0].Value, match1.Groups[1].Value + "AS" + match1.Groups[3].Value);
            else if (match1.Success && Convert.ToInt32(match1.Groups[3].ToString()) > 160)
            {
              Match match2 = Regex.Match(wxStruct2.sky_fsx_smooth, match1.Groups[0].Value + "\\s(\\d)(\\w\\w)(\\d\\d\\d)");
              if (Convert.ToInt32(match1.Groups[1].Value) <= 4)
                wxStruct2.sky_fsx_smooth = Regex.Replace(wxStruct2.sky_fsx_smooth, match1.Groups[0].Value, match1.Groups[1].Value + "AS" + match1.Groups[3].Value);
              else if (!match2.Success || Convert.ToInt32(match2.Groups[3].Value) - Convert.ToInt32(match1.Groups[3].Value) > 30)
              {
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                WxGridClass.wxStruct& local = @wxStruct2;
                string skyFsxSmooth = wxStruct2.sky_fsx_smooth;
                string pattern = match1.Groups[0].Value;
                string str2 = match1.Groups[0].Value;
                string str3 = " 1CI";
                num = Convert.ToInt32(match1.Groups[3].Value) + 20;
                string str4 = num.ToString();
                string replacement = str2 + str3 + str4;
                string str5 = Regex.Replace(skyFsxSmooth, pattern, replacement);
                // ISSUE: explicit reference operation
                (^local).sky_fsx_smooth = str5;
              }
            }
            else if (match1.Success && Convert.ToInt32(match1.Groups[3].Value) <= 8)
              wxStruct2.sky_fsx_smooth = Regex.Replace(wxStruct2.sky_fsx_smooth, match1.Groups[0].Value, match1.Groups[1].Value + "AS" + match1.Groups[3].Value);
          }
          wxStruct2.sky_fsx_smooth = this.SortSkyString(wxStruct2.sky_fsx_smooth);
          wxStruct2.sky_fsx_smooth = this.StretchSkyString(wxStruct2.sky_fsx_smooth);
          this._wxGrid[index1] = wxStruct2;
        }
      }
    }

    private void WxGridFsxString(string simChoice)
    {
      List<WxGridClass.wxStruct> wxStructList = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      Random random = new Random();
      double num1 = 1.0;
      double num2 = 1.0;
      if (simChoice == "P3D")
        num1 = 1.8;
      else if (simChoice == "P3DVF")
        num1 = 3.2;
      for (int index1 = 0; index1 < this._wxGrid.Count; ++index1)
      {
        if (this._newWxGridPts.Contains(this._wxGrid[index1].station_id))
        {
          WxGridClass.wxStruct wxStruct2 = this._wxGrid[index1];
          List<int[]> windsAloftExt = wxStruct2.winds_aloft_ext;
          string str1 = "" + wxStruct2.station_id + " ";
          string input = string.Format("{0:000}", (object) windsAloftExt[0][2]) + string.Format("{0:00}", (object) windsAloftExt[0][3]) + "KT&D305NG ";
          for (int index2 = 1; index2 < ((IEnumerable<int>) this._aloftlevelsOut).Count<int>(); ++index2)
          {
            int num3 = index2 < ((IEnumerable<int>) this._aloftlevelsOut).Count<int>() - 1 ? 305 : 3000;
            input = input + string.Format("{0:000}", (object) windsAloftExt[index2][2]) + string.Format("{0:00}", (object) windsAloftExt[index2][3]) + "KT&A" + ((int) ((double) windsAloftExt[index2][0] * 100.0 * 0.3 + (double) num3)).ToString() + "NG ";
          }
          wxStructList.Clear();
          wxStructList = this.FetchWxDataNearest(this._wxGrid, (double) wxStruct2.lat, (double) wxStruct2.lon, 4);
          wxStruct2.fsx_turb_descr = "";
          int val1 = 0;
          int num4 = 5;
          if (Math.Abs(wxStruct2.temp_c - wxStructList[0].temp_c) >= num4 || Math.Abs(wxStruct2.temp_c - wxStructList[1].temp_c) >= num4 || (Math.Abs(wxStruct2.temp_c - wxStructList[2].temp_c) >= num4 || Math.Abs(wxStruct2.temp_c - wxStructList[3].temp_c) >= num4))
          {
            val1 = Math.Max(val1, 2);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "temp: 2 - ";
          }
          int num5 = 3;
          if (Math.Abs(wxStruct2.temp_c - wxStructList[0].temp_c) >= num5 || Math.Abs(wxStruct2.temp_c - wxStructList[1].temp_c) >= num5 || (Math.Abs(wxStruct2.temp_c - wxStructList[2].temp_c) >= num5 || Math.Abs(wxStruct2.temp_c - wxStructList[3].temp_c) >= num5))
          {
            val1 = Math.Max(val1, 1);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "temp: 1 - ";
          }
          int num6 = 4;
          if ((double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[0].altim_in_hg) >= (double) num6 || (double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[1].altim_in_hg) >= (double) num6 || ((double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[2].altim_in_hg) >= (double) num6 || (double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[3].altim_in_hg) >= (double) num6))
          {
            val1 = Math.Max(val1, 2);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "press: 2 - ";
          }
          int num7 = 2;
          if ((double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[0].altim_in_hg) >= (double) num7 || (double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[1].altim_in_hg) >= (double) num7 || ((double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[2].altim_in_hg) >= (double) num7 || (double) Math.Abs(wxStruct2.altim_in_hg - wxStructList[3].altim_in_hg) >= (double) num7))
          {
            val1 = Math.Max(val1, 1);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "press: 1 - ";
          }
          if (windsAloftExt[0][3] > 20)
          {
            val1 = Math.Max(val1, 2);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "surf wind: 2 - ";
          }
          else if (windsAloftExt[0][3] > 10)
          {
            val1 = Math.Max(val1, 1);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "surf wind: 1 - ";
          }
          if (wxStruct2.wind_gusts > 30)
          {
            val1 = Math.Max(val1, 3);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "gusts: 3 - ";
          }
          else if (wxStruct2.wind_gusts > 20)
          {
            val1 = Math.Max(val1, 2);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "gusts: 2 - ";
          }
          else if (wxStruct2.wind_gusts > 6)
          {
            val1 = Math.Max(val1, 1);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "gusts: 1 - ";
          }
          if (Regex.IsMatch(wxStruct2.wx_string_fsx, "(\\-RA|\\-SN)"))
          {
            val1 = Math.Max(val1, 1);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "-RA/-SN: 1 - ";
          }
          else if (Regex.IsMatch(wxStruct2.wx_string_fsx, "(RA|SN)"))
          {
            val1 = Math.Max(val1, 2);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            ^@wxStruct2.fsx_turb_descr += "RA/SN: 2 - ";
          }
          int num8 = 1;
          foreach (Match match in new Regex("(D|A)(\\d+)NG", RegexOptions.Compiled).Matches(input))
          {
            int int32 = Convert.ToInt32(match.Groups[2].Value);
            int num3 = val1;
            if (int32 > 4000)
              num3 = Math.Max(num3 - 2, 0);
            else if (int32 > 2000)
              num3 = Math.Max(num3 - 1, 0);
            string str2;
            switch (num3)
            {
              case 1:
                str2 = "L";
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                string& local = @wxStruct2.fsx_turb_descr;
                // ISSUE: explicit reference operation
                // ISSUE: explicit reference operation
                ^local = ^local + "rnd at " + num8.ToString() + ": 1 - ";
                break;
              case 2:
                str2 = "M";
                break;
              case 3:
                str2 = num8 > 2 ? "M" : "H";
                break;
              default:
                if (random.Next(0, 100) > 15)
                {
                  str2 = "N";
                  break;
                }
                goto case 1;
            }
            input = this._fsxinternalwindturb ? Regex.Replace(input, match.Groups[0].Value, match.Groups[1].Value + match.Groups[2].Value + str2 + "G") : Regex.Replace(input, match.Groups[0].Value, match.Groups[1].Value + match.Groups[2].Value + "NG");
            ++num8;
          }
          string str3 = str1 + input;
          double val2 = (double) wxStruct2.visibility_statute_mi;
          double num9 = 44.0;
          double num10 = 60.0;
          int num11 = 7;
          int num12 = 3;
          double num13 = 16000.0;
          if (Regex.IsMatch(wxStruct2.wx_string_fsx, "-RA"))
            val2 = Math.Min(18.0, val2);
          else if (Regex.IsMatch(wxStruct2.wx_string_fsx, "RA"))
            val2 = Math.Min(12.0, val2);
          if (Regex.IsMatch(wxStruct2.wx_string_fsx, "-SN"))
            val2 = Math.Min(14.0, val2);
          else if (Regex.IsMatch(wxStruct2.wx_string_fsx, "SN"))
            val2 = Math.Min(8.0, val2);
          Match match1 = Regex.Match(wxStruct2.sky_fsx_smooth, "(\\d)(\\w\\w)(\\d\\d\\d)");
          if (match1.Success && Convert.ToInt32(match1.Groups[1].Value) <= 4)
          {
            if (Convert.ToInt32(match1.Groups[3].Value) <= 10)
              val2 = Math.Min(18.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 14)
              val2 = Math.Min(22.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 18)
              val2 = Math.Min(26.0, val2);
          }
          else if (match1.Success && Convert.ToInt32(match1.Groups[1].Value) <= 7)
          {
            if (Convert.ToInt32(match1.Groups[3].Value) <= 10)
              val2 = Math.Min(14.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 14)
              val2 = Math.Min(18.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 18)
              val2 = Math.Min(22.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 22)
              val2 = Math.Min(26.0, val2);
          }
          else if (match1.Success && Convert.ToInt32(match1.Groups[1].Value) == 8)
          {
            if (Convert.ToInt32(match1.Groups[3].Value) <= 10)
              val2 = Math.Min(10.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 14)
              val2 = Math.Min(14.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 18)
              val2 = Math.Min(18.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 22)
              val2 = Math.Min(22.0, val2);
            else if (Convert.ToInt32(match1.Groups[3].Value) <= 26)
              val2 = Math.Min(26.0, val2);
          }
          if (val2 < 30.0)
            num9 -= (30.0 - val2) * 0.5;
          if ((double) wxStruct2.visibility_statute_mi == 30.0)
          {
            if (wxStruct2.humidity < 30)
            {
              val2 += 12.0;
              num9 += 14.0;
              num10 += 17.0;
            }
            else if (wxStruct2.humidity < 38)
            {
              val2 += 10.0;
              num9 += 12.0;
              num10 += 14.0;
            }
            else if (wxStruct2.humidity < 46)
            {
              val2 += 8.0;
              num9 += 9.0;
              num10 += 11.0;
            }
            else if (wxStruct2.humidity < 54)
            {
              val2 += 6.0;
              num9 += 7.0;
              num10 += 8.0;
            }
            else if (wxStruct2.humidity < 62)
            {
              val2 += 4.0;
              num9 += 4.0;
              num10 += 5.0;
            }
            else if (wxStruct2.humidity < 70)
            {
              val2 += 2.0;
              num9 += 2.0;
              num10 += 2.0;
            }
          }
          double num14 = -1001.0;
          double num15 = 0.0;
          double num16 = 0.0;
          double num17;
          double num18;
          for (int index2 = 0; index2 < num11; ++index2)
          {
            double num3 = Math.Round(((double) index2 * (num9 - val2) / (double) num11 + val2) * 10.0 * num1);
            num17 = Math.Round(num14 + num15 + 1.0);
            num18 = Math.Round(num13 * 0.3 / (double) num11);
            switch (index2)
            {
              case 0:
                num3 = Math.Round(num3 * 0.9);
                num18 = Math.Round(num18 * 1.2) + 1200.0;
                break;
              case 1:
                num3 = Math.Round(num3 - (num3 - num16) * 0.7);
                num18 = Math.Round(num18 * 1.2);
                break;
              case 2:
                num3 = Math.Round(num3 - (num3 - num16) * 0.5);
                break;
              case 3:
                num3 = Math.Round(num3 - (num3 - num16) * 0.3);
                break;
              case 4:
                num3 = Math.Round(num3 - (num3 - num16) * 0.1);
                break;
              default:
                if (index2 == num11 - 2)
                {
                  num18 = Math.Round(num18 * 1.2);
                  break;
                }
                if (index2 == num11 - 1)
                {
                  num18 = Math.Round(num18 * 1.3);
                  break;
                }
                break;
            }
            if (num3 == 100.0)
              num3 = 99.0;
            else if (num3 < 1.0)
              num3 = 1.0;
            str3 = str3 + num3.ToString() + "/10SM&B" + num17.ToString() + "&D" + num18.ToString() + " ";
            num14 = num17;
            num15 = num18;
            num16 = num3;
          }
          double num19 = num9;
          double num20 = 30000.0 - num13;
          if (simChoice == "FSX" || simChoice == "P3D")
          {
            for (int index2 = 1; index2 <= num12; ++index2)
            {
              double num3 = Math.Round(((double) index2 * (num10 - num19) / (double) num12 + num19 * num1) * 10.0 * num2);
              if (num3 == 100.0)
                num3 = 99.0;
              else if (num3 == 0.0)
                num3 = 1.0;
              num17 = Math.Round(num14 + num15 + 1.0);
              num18 = Math.Round(num20 * 0.3 / (double) num12);
              if (index2 == 1)
                num3 = Math.Round(num3 - (num3 - num16) * 0.3);
              if (index2 == num12)
                num18 = 8000.0;
              str3 = str3 + num3.ToString() + "/10SM&B" + num17.ToString() + "&D" + num18.ToString() + " ";
              num14 = num17;
              num15 = num18;
              num16 = num3;
            }
          }
          if (wxStruct2.wx_string_fsx != "")
            str3 = str3 + wxStruct2.wx_string_fsx + " ";
          if (wxStruct2.sky_fsx_smooth != "")
            str3 = str3 + wxStruct2.sky_fsx_smooth + " ";
          for (int index2 = 0; index2 < ((IEnumerable<int>) this._aloftlevelsOut).Count<int>(); ++index2)
          {
            int num3 = windsAloftExt[index2][1];
            int num21 = num3 - (100 - wxStruct2.humidity) / 5;
            string str2 = num3 >= 0 ? string.Format("{0:00}", (object) Math.Abs(num3)) : "-" + string.Format("{0:00}", (object) Math.Abs(num3));
            string str4 = num21 >= 0 ? string.Format("{0:00}", (object) Math.Abs(num21)) : "-" + string.Format("{0:00}", (object) Math.Abs(num21));
            int num22 = index2 < ((IEnumerable<int>) this._aloftlevelsOut).Count<int>() - 1 ? 0 : 3000;
            str3 = str3 + str2 + "/" + str4 + "&A" + ((int) ((double) windsAloftExt[index2][0] * 100.0 * 0.3 + (double) num22)).ToString() + " ";
          }
          string str5 = str3 + "A" + (wxStruct2.altim_in_hg * 100f).ToString();
          wxStruct2.fsx = str5;
          this._wxGrid[index1] = wxStruct2;
        }
      }
    }

    private List<WxGridClass.wxGridListStruct> CreateWxGridListExtract(List<WxGridClass.wxGridListStruct> ptsin, double aclat, double aclon, double radius)
    {
      List<WxGridClass.wxGridListStruct> wxGridListStructList1 = new List<WxGridClass.wxGridListStruct>();
      List<WxGridClass.wxGridListStruct> wxGridListStructList2 = new List<WxGridClass.wxGridListStruct>();
      foreach (WxGridClass.wxGridListStruct wxGridListStruct in ptsin)
      {
        if (GeoClass.Dist(aclat, aclon, (double) wxGridListStruct.lat, (double) wxGridListStruct.lon) < radius)
          wxGridListStructList2.Add(wxGridListStruct);
      }
      return wxGridListStructList2;
    }

    private string TransformSkyStringAndSetCoverage(string sky, List<WxGridClass.wxStruct> wxData)
    {
      string str1 = "";
      string input1 = "";
      string[] strArray = sky.Split(' ');
      Hashtable hashtable = new Hashtable();
      hashtable.Add((object) "FEW", (object) 2);
      hashtable.Add((object) "SCT", (object) 3);
      hashtable.Add((object) "BKN", (object) 6);
      hashtable.Add((object) "OVC", (object) 8);
      foreach (WxGridClass.wxStruct wxStruct in wxData)
        input1 = input1 + wxStruct.sky_condition + " ";
      foreach (string input2 in strArray)
      {
        Match match = Regex.Match(input2, "(FEW|SCT|BKN|OVC)(\\d\\d\\d)(&?(\\w?\\w?)(.*))");
        string str2;
        if (match.Success)
        {
          string str3 = !(match.Groups[4].Value != "") ? this._ttgeneral : match.Groups[4].Value;
          int num = (int) hashtable[(object) match.Groups[1].Value];
          if (Convert.ToInt32(match.Groups[2].Value) <= 200 && ((IEnumerable<string>) strArray).Count<string>() > 1)
          {
            if (match.Groups[1].Value == "BKN" && Regex.Matches(input1, "(BKN|OVC)").Count >= 2)
              num = 8;
            else if (match.Groups[1].Value == "BKN" && Regex.Matches(input1, "(BKN|OVC)").Count == 1)
              num = 7;
            else if (match.Groups[1].Value == "SCT" && Regex.Matches(input1, "(SCT|BKN|OVC)").Count >= 2)
              num = 7;
            else if (match.Groups[1].Value == "SCT" && Regex.Matches(input1, "(SCT|BKN|OVC)").Count == 1)
              num = 6;
            else if (match.Groups[1].Value == "FEW" && Regex.Matches(input1, "(SCT|BKN|OVC)").Count >= 2)
              num = 3;
          }
          str2 = str1 + " " + num.ToString() + str3 + match.Groups[2].Value;
          if (match.Groups[5].Value != "")
            str2 += match.Groups[5].Value;
        }
        else
          str2 = str1 + " " + input2;
        str1 = str2.Trim();
      }
      return str1;
    }

    private string SortSkyString(string sky)
    {
      string str = "";
      sky = sky.Trim();
      if (sky.Length > 1)
      {
        string[] array = sky.Trim().Split(' ');
        for (int index = 0; index < ((IEnumerable<string>) array).Count<string>(); ++index)
        {
          Match match = Regex.Match(array[index], "(\\d\\d\\d)");
          array[index] = match.Groups[1].Value + array[index];
        }
        Array.Sort<string>(array);
        for (int index = 0; index < ((IEnumerable<string>) array).Count<string>(); ++index)
        {
          array[index] = array[index].Substring(3);
          str = str + array[index] + " ";
        }
      }
      return str.Trim();
    }

    private string StretchSkyString(string sky)
    {
      string str1 = "";
      if (sky.Length > 1)
      {
        string[] strArray = sky.Split(' ');
        int num1 = -500;
        string str2 = "";
        for (int index = 0; index < ((IEnumerable<string>) strArray).Count<string>(); ++index)
        {
          Match match = Regex.Match(strArray[index], "(\\d)(\\w\\w)(\\d\\d\\d)");
          if (match.Success)
          {
            string str3 = match.Groups[2].Value;
            int num2 = Convert.ToInt32(match.Groups[3].Value);
            if (num2 - num1 < 26 && str2 == "AS")
            {
              int num3 = 26 - (num2 - num1);
              num2 = Convert.ToInt32(match.Groups[3].Value) + num3;
              strArray[index] = Regex.Replace(strArray[index], match.Groups[3].Value, string.Format("{0:000}", (object) num2));
            }
            else if (num2 - num1 <= 0)
            {
              int num3 = 10 - (num2 - num1);
              num2 = Convert.ToInt32(match.Groups[3].Value) + num3;
              strArray[index] = Regex.Replace(strArray[index], match.Groups[3].Value, string.Format("{0:000}", (object) num2));
            }
            str2 = str3;
            num1 = num2;
          }
        }
        for (int index = 0; index < ((IEnumerable<string>) strArray).Count<string>(); ++index)
          str1 = str1 + strArray[index] + " ";
      }
      return str1.Trim();
    }

    private List<int[]> LimitWindsAloftTransitions(List<int[]> windsaloft)
    {
      List<int[]> numArrayList = new List<int[]>();
      int num1 = 12;
      int num2 = 6;
      numArrayList.Add(windsaloft[0]);
      int num3 = windsaloft[0][0];
      int num4 = windsaloft[0][2];
      int num5 = windsaloft[0][3];
      for (int index = 1; index < windsaloft.Count<int[]>(); ++index)
      {
        int num6 = windsaloft[index][0];
        int bear2 = windsaloft[index][2];
        int num7 = windsaloft[index][3];
        int num8 = bear2;
        int num9 = num7;
        int num10 = windsaloft[index][1];
        if (GeoClass.BearingDiffAbs(bear2, num4) > (num6 - num3) / 10 * num1)
          num8 = GeoClass.BearingDiff(bear2, num4) > 180 ? GeoClass.BearingSub(num4, (num6 - num3) / 10 * num1) : GeoClass.BearingAdd(num4, (num6 - num3) / 10 * num1);
        if (num7 >= num5 && num7 - num5 > (num6 - num3) / 10 * num2)
          num9 = num5 + (num6 - num3) / 10 * num2;
        else if (num7 < num5 && num5 - num7 > (num6 - num3) / 10 * num2)
          num9 = Math.Max(0, num5 - (num6 - num3) / 10 * num2);
        numArrayList.Add(new int[4]
        {
          num6,
          num10,
          num8,
          num9
        });
        num3 = num6;
        num4 = num8;
        num5 = num9;
      }
      return numArrayList;
    }

    private List<int[]> CreateWindsAloftExt(WxGridClass.wxStruct wxRow)
    {
      List<int[]> numArrayList1 = new List<int[]>();
      List<int[]> numArrayList2 = new List<int[]>();
      numArrayList1.Add(new int[4]
      {
        0,
        wxRow.temp_c,
        wxRow.wind_dir_degrees,
        wxRow.wind_speed_kt
      });
      numArrayList1.Add(new int[4]
      {
        10,
        wxRow.temp_c,
        wxRow.wind_dir_degrees,
        wxRow.wind_speed_kt
      });
      List<int[]> numArrayList3 = !(wxRow.winds_aloft_string != "") ? wxRow.winds_aloft_calc : wxRow.winds_aloft;
      for (int i = 2; i < ((IEnumerable<int>) this._aloftlevelsOut).Count<int>(); i++)
      {
        int num1;
        int num2;
        int num3;
        if (numArrayList3.Exists((Predicate<int[]>) (n => n[0] == this._aloftlevelsOut[i])))
        {
          int[] numArray = numArrayList3.Find((Predicate<int[]>) (n => n[0] == this._aloftlevelsOut[i]));
          num1 = numArray[1];
          num2 = numArray[2];
          num3 = numArray[3];
        }
        else
        {
          int[] last = numArrayList3.FindLast((Predicate<int[]>) (n => n[0] < this._aloftlevelsOut[i]));
          int[] numArray = numArrayList3.Find((Predicate<int[]>) (n => n[0] > this._aloftlevelsOut[i]));
          num1 = (int) Math.Round(((double) numArray[1] - (double) last[1]) / ((double) numArray[0] - (double) last[0]) * ((double) this._aloftlevelsOut[i] - (double) last[0]) + (double) last[1]);
          num3 = (int) Math.Round(((double) numArray[3] - (double) last[3]) / ((double) numArray[0] - (double) last[0]) * ((double) this._aloftlevelsOut[i] - (double) last[0]) + (double) last[3]);
          if (GeoClass.BearingDiff(numArray[2], last[2]) <= 180)
          {
            int num4 = GeoClass.BearingDiff(numArray[2], last[2]);
            num2 = GeoClass.BearingAdd(last[2], (int) Math.Round(((double) this._aloftlevelsOut[i] - (double) last[0]) / ((double) numArray[0] - (double) last[0]) * (double) num4));
          }
          else
          {
            int num4 = 360 - GeoClass.BearingDiff(numArray[2], last[2]);
            num2 = GeoClass.BearingAdd(numArray[2], (int) Math.Round(((double) this._aloftlevelsOut[i] - (double) last[0]) / ((double) numArray[0] - (double) last[0]) * (double) num4));
          }
        }
        numArrayList1.Add(new int[4]
        {
          this._aloftlevelsOut[i],
          num1,
          num2,
          num3
        });
      }
      return numArrayList1;
    }

    public string ChooseSkyCondition(List<WxGridClass.wxStruct> wxData)
    {
      int index = 0;
      if (wxData[0].sky_condition.Contains("OVC"))
        index = 0;
      else if (wxData[0].sky_condition.Contains("BKN"))
      {
        string[] strArray = new string[1]{ "OVC" };
        if (((IEnumerable<string>) strArray).Any<string>(new Func<string, bool>(wxData[1].sky_condition.Contains)) && ((IEnumerable<string>) strArray).Any<string>(new Func<string, bool>(wxData[2].sky_condition.Contains)))
          index = 1;
      }
      else if (wxData[0].sky_condition.Contains("SCT"))
      {
        string[] strArray = new string[2]{ "OVC", "BKN" };
        if (((IEnumerable<string>) strArray).Any<string>(new Func<string, bool>(wxData[1].sky_condition.Contains)) && ((IEnumerable<string>) strArray).Any<string>(new Func<string, bool>(wxData[2].sky_condition.Contains)))
          index = 1;
      }
      else if (wxData[0].sky_condition.Contains("FEW"))
      {
        string[] strArray = new string[3]
        {
          "OVC",
          "BKN",
          "SCT"
        };
        if (((IEnumerable<string>) strArray).Any<string>(new Func<string, bool>(wxData[1].sky_condition.Contains)) && ((IEnumerable<string>) strArray).Any<string>(new Func<string, bool>(wxData[2].sky_condition.Contains)))
          index = 1;
      }
      return wxData[index].sky_condition;
    }

    public List<int[]> InterpolateWindsAloft(List<WxGridClass.wxStruct> wxData, WxGridClass.weightingStruct weighting, List<double> weights, double scalesurf, double scalealoft)
    {
      List<int[]> numArrayList = new List<int[]>();
      int[] numArray = new int[4];
      for (int index1 = 0; index1 < ((IEnumerable<int>) this._aloftlevelsOut).Count<int>(); ++index1)
      {
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double a1 = 0.0;
        for (int index2 = 0; index2 < wxData.Count; ++index2)
        {
          List<int[]> windsAloftExt = wxData[index2].winds_aloft_ext;
          num1 += weights[index2] * (Math.Sin(GeoClass.ToRad((double) windsAloftExt[index1][2])) * (double) windsAloftExt[index1][3]);
          num2 += weights[index2] * (Math.Cos(GeoClass.ToRad((double) windsAloftExt[index1][2])) * (double) windsAloftExt[index1][3]);
          num3 += weights[index2] * (double) windsAloftExt[index1][1];
        }
        double num4 = num1 / weighting.sum_weights;
        double num5 = num2 / weighting.sum_weights;
        double num6 = index1 == 0 ? scalesurf : scalealoft;
        double a2 = Math.Sqrt(num4 * num4 + num5 * num5) * num6;
        if (num4 == 0.0 && num5 >= 0.0)
          a1 = 0.0;
        else if (num4 == 0.0 && num5 < 0.0)
          a1 = 180.0;
        else if (num5 == 0.0 && num4 >= 0.0)
          a1 = 90.0;
        else if (num5 == 0.0 && num4 < 0.0)
          a1 = 270.0;
        else if (num4 > 0.0 && num5 > 0.0)
          a1 = GeoClass.ToDeg(Math.Atan(Math.Abs(num4) / Math.Abs(num5)));
        else if (num4 > 0.0 && num5 < 0.0)
          a1 = 180.0 - GeoClass.ToDeg(Math.Atan(Math.Abs(num4) / Math.Abs(num5)));
        else if (num4 < 0.0 && num5 < 0.0)
          a1 = 180.0 + GeoClass.ToDeg(Math.Atan(Math.Abs(num4) / Math.Abs(num5)));
        else if (num4 < 0.0 && num5 > 0.0)
          a1 = 360.0 - GeoClass.ToDeg(Math.Atan(Math.Abs(num4) / Math.Abs(num5)));
        double a3 = num3 / weighting.sum_weights;
        numArrayList.Add(new int[4]
        {
          this._aloftlevelsOut[index1],
          (int) Math.Round(a3),
          (int) Math.Round(a1),
          (int) Math.Round(a2)
        });
      }
      return numArrayList;
    }

    public bool CheckIfStationExists(string station_id)
    {
      return this._wxAll.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == station_id));
    }

    public List<WxGridClass.wxStruct> FetchWxDataNearest(List<WxGridClass.wxStruct> wxData, double lat, double lon, int number)
    {
      List<WxGridClass.wxStruct> source = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      try
      {
        foreach (WxGridClass.wxStruct wxStruct2 in wxData)
        {
          WxGridClass.wxStruct wxStruct3 = wxStruct2;
          wxStruct3.dist_from_ac = GeoClass.Dist(lat, lon, (double) wxStruct2.lat, (double) wxStruct2.lon);
          if (wxStruct3.dist_from_ac != 0.0)
            source.Add(wxStruct3);
        }
        source = source.OrderBy<WxGridClass.wxStruct, double>((Func<WxGridClass.wxStruct, double>) (o => o.dist_from_ac)).ToList<WxGridClass.wxStruct>();
        if (source.Count > number)
          source = source.GetRange(0, number);
        return source;
      }
      catch
      {
        return source;
      }
    }

    public bool FetchWxData(double aclat, double aclon, double radius)
    {
      List<WxGridClass.wxStruct> wxData1 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxData2 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList1 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList2 = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      WxGridClass.wxStruct wxStruct2 = new WxGridClass.wxStruct();
      WxGridClass.wxAloftStruct wxAloftStruct1 = new WxGridClass.wxAloftStruct();
      WxGridClass.wxAloftStruct wxAloftStruct2 = new WxGridClass.wxAloftStruct();
      List<WxGridClass.wxAloftStruct> wxAloft = new List<WxGridClass.wxAloftStruct>();
      List<WxGridClass.wxAloftStruct> wxAloftGrib = new List<WxGridClass.wxAloftStruct>();
      Random random = new Random();
      string str1 = Path.GetTempPath() + "FSXWXmetar.txt";
      string str2 = Path.GetTempPath() + "FSXWXmetarTime.txt";
      string str3 = Path.GetTempPath() + "FSXWXwindsaloft.txt";
      string str4 = Path.GetTempPath() + "FSXWXwindsaloftTime.txt";
      string str5 = Path.GetTempPath() + "FSXWXgrib.txt";
      string str6 = Path.GetTempPath() + "FSXWXgribTime.txt";
      string address1;
      string address2;
      string str7;
      string address3;
      string address4;
      string str8;
      string address5;
      string address6;
      string str9;
      if (DateTime.UtcNow.Subtract(this._datetime).TotalMinutes <= 60.0)
      {
        address1 = "http://www.plane-pics.de/fsxwx/data/metar/latest.txt.gz";
        address2 = "http://www.plane-pics.de/fsxwx/data/metar/latestTime.txt";
        str7 = (string) null;
        address3 = "http://www.plane-pics.de/fsxwx/data/windsaloft/latest.txt.gz";
        address4 = "http://www.plane-pics.de/fsxwx/data/windsaloft/latestTime.txt";
        str8 = (string) null;
        address5 = "http://www.plane-pics.de/fsxwx/data/grib/latest.txt.gz";
        address6 = "http://www.plane-pics.de/fsxwx/data/grib/latestTime.txt";
        str9 = (string) null;
      }
      else
      {
        address1 = "http://www.plane-pics.de/fsxwx/data/metar/" + this._datetimeStamp + "/" + this._datetimeHour + ".txt.gz";
        address2 = "http://www.plane-pics.de/fsxwx/data/metar/latestTime.txt";
        str7 = this._datetimeStamp + this._datetimeHour + "0000";
        address3 = "http://www.plane-pics.de/fsxwx/data/windsaloft/" + this._datetimeStamp + "/" + this._datetimeHour + ".txt.gz";
        address4 = "http://www.plane-pics.de/fsxwx/data/windsaloft/latestTime.txt";
        str8 = this._datetimeStamp + this._datetimeHour + "0000";
        string str10;
        string datetimeStamp;
        if (Convert.ToInt32(this._datetimeHour) >= 18)
        {
          str10 = "12";
          datetimeStamp = this._datetimeStamp;
        }
        else if (Convert.ToInt32(this._datetimeHour) >= 12)
        {
          str10 = "06";
          datetimeStamp = this._datetimeStamp;
        }
        else if (Convert.ToInt32(this._datetimeHour) >= 6)
        {
          str10 = "00";
          datetimeStamp = this._datetimeStamp;
        }
        else
        {
          str10 = "18";
          datetimeStamp = this._datetime.AddDays(-1.0).ToString("yyyyMMdd");
        }
        string str11 = "f006";
        address5 = "http://www.plane-pics.de/fsxwx/data/grib/" + datetimeStamp + "/" + str10 + str11 + ".txt.gz";
        address6 = "http://www.plane-pics.de/fsxwx/data/grib/latestTime.txt";
        str9 = datetimeStamp + str10 + "0000";
      }
      try
      {
        try
        {
          if (str8 == null)
          {
            new WebClient().DownloadFile(address4, str4);
            str8 = System.IO.File.ReadAllText(str4);
          }
          if (this._windsaloftLatestTime == null || this._windsaloftLatestTime != str8)
          {
            new WebClient().DownloadFile(address3, str3 + ".gz");
            GZip.DecompressFile(str3 + ".gz", str3);
            this._windsaloftLatestTime = str8;
          }
          foreach (string readAllLine in System.IO.File.ReadAllLines(str3))
          {
            char[] chArray = new char[1]{ ';' };
            string[] strArray = readAllLine.Split(chArray);
            wxAloftStruct1.station_id = strArray[0];
            wxAloftStruct1.lat = (float) Convert.ToDouble(strArray[1]);
            wxAloftStruct1.lon = (float) Convert.ToDouble(strArray[2]);
            wxAloftStruct1.last_update = strArray[3];
            wxAloftStruct1.winds_aloft_string = strArray[4];
            if (GeoClass.Dist(aclat, aclon, (double) wxAloftStruct1.lat, (double) wxAloftStruct1.lon) <= radius)
              wxAloft.Add(wxAloftStruct1);
          }
        }
        catch
        {
        }
        try
        {
          if (str9 == null)
          {
            new WebClient().DownloadFile(address6, str6);
            str9 = System.IO.File.ReadAllText(str6);
          }
          if (this._gribLatestTime == null || this._gribLatestTime != str9)
          {
            new WebClient().DownloadFile(address5, str5 + ".gz");
            GZip.DecompressFile(str5 + ".gz", str5);
            this._gribLatestTime = str9;
          }
          foreach (string readAllLine in System.IO.File.ReadAllLines(str5))
          {
            char[] chArray = new char[1]{ ';' };
            string[] strArray1 = readAllLine.Split(chArray);
            wxStruct2.station_id = strArray1[0];
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            WxGridClass.wxStruct& local1 = @wxStruct2;
            DateTime utcNow = DateTime.UtcNow;
            string str10 = utcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            // ISSUE: explicit reference operation
            (^local1).raw_time = str10;
            wxStruct2.lat = (float) Convert.ToDouble(strArray1[1]);
            wxStruct2.lon = (float) Convert.ToDouble(strArray1[2]);
            wxStruct2.temp_c = Convert.ToInt32(strArray1[8]);
            wxStruct2.dewpoint_c = Convert.ToInt32(strArray1[9]);
            wxStruct2.wind_dir_degrees = Convert.ToInt32(strArray1[3]);
            wxStruct2.wind_speed_kt = Convert.ToInt32(strArray1[4]);
            wxStruct2.wind_gusts = 0;
            wxStruct2.wind_variance = 0;
            wxStruct2.wind_vrb = false;
            wxStruct2.visibility_statute_mi = 30f;
            wxStruct2.altim_in_hg = (float) Convert.ToDouble(strArray1[10]);
            wxStruct2.elevation_m = 0;
            wxStruct2.dist_from_ac = 0.0;
            wxStruct2.humidity = Convert.ToInt32(Math.Round(6.112 * Math.Exp(17.62 * (double) wxStruct2.dewpoint_c / (243.12 + (double) wxStruct2.dewpoint_c)) / (6.112 * Math.Exp(17.62 * (double) wxStruct2.temp_c / (243.12 + (double) wxStruct2.temp_c))) * 100.0, 0));
            wxStruct2.wx_string = Convert.ToDouble(strArray1[11]) > 0.0 ? "RA" : "";
            wxStruct2.sky_condition = "";
            if (Convert.ToInt32(strArray1[5]) > 90)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "OVC0" + random.Next(1, 4).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[5]) > 60)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "BKN0" + random.Next(1, 4).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[5]) > 30)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "SCT0" + random.Next(1, 4).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[5]) > 0)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "FEW0" + random.Next(1, 4).ToString() + "0 ";
            }
            if (Convert.ToInt32(strArray1[6]) > 90)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "OVC0" + random.Next(5, 9).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[6]) > 60)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "BKN0" + random.Next(5, 9).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[6]) > 30)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "SCT0" + random.Next(5, 9).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[6]) > 0)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "FEW0" + random.Next(5, 9).ToString() + "0 ";
            }
            if (Convert.ToInt32(strArray1[7]) > 90)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "OVC1" + random.Next(0, 3).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[7]) > 60)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "BKN1" + random.Next(0, 3).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[7]) > 30)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "SCT1" + random.Next(0, 3).ToString() + "0 ";
            }
            else if (Convert.ToInt32(strArray1[7]) > 0)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              string& local2 = @wxStruct2.sky_condition;
              // ISSUE: explicit reference operation
              // ISSUE: explicit reference operation
              ^local2 = ^local2 + "FEW1" + random.Next(0, 3).ToString() + "0 ";
            }
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            WxGridClass.wxStruct& local3 = @wxStruct2;
            string[] strArray2 = new string[13];
            strArray2[0] = wxStruct2.station_id;
            strArray2[1] = " ";
            int index = 2;
            utcNow = DateTime.UtcNow;
            string str11 = utcNow.ToString("ddHHmmZ");
            strArray2[index] = str11;
            strArray2[3] = " ";
            strArray2[4] = string.Format("{0:000}", (object) wxStruct2.wind_dir_degrees);
            strArray2[5] = string.Format("{0:00}", (object) wxStruct2.wind_speed_kt);
            strArray2[6] = "KT 10SM ";
            strArray2[7] = wxStruct2.sky_condition;
            strArray2[8] = string.Format("{0:00}", (object) wxStruct2.temp_c);
            strArray2[9] = "/";
            strArray2[10] = string.Format("{0:00}", (object) wxStruct2.dewpoint_c);
            strArray2[11] = " A";
            strArray2[12] = wxStruct2.altim_in_hg.ToString().Replace(".", "");
            string str12 = string.Concat(strArray2);
            // ISSUE: explicit reference operation
            (^local3).raw_text = str12;
            if (GeoClass.Dist(aclat, aclon, (double) wxStruct2.lat, (double) wxStruct2.lon) <= radius && wxStruct2.temp_c != 9999 && (wxStruct2.dewpoint_c != 9999 && (double) wxStruct2.altim_in_hg != 9999.0))
              wxData2.Add(wxStruct2);
            wxAloftStruct2.station_id = strArray1[0];
            wxAloftStruct2.lat = (float) Convert.ToDouble(strArray1[1]);
            wxAloftStruct2.lon = (float) Convert.ToDouble(strArray1[2]);
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            WxGridClass.wxAloftStruct& local4 = @wxAloftStruct2;
            utcNow = DateTime.UtcNow;
            string str13 = utcNow.ToString("yyyy-MM-dd HH:mm:ss");
            // ISSUE: explicit reference operation
            (^local4).last_update = str13;
            wxAloftStruct2.winds_aloft_string = strArray1[12];
            if (GeoClass.Dist(aclat, aclon, (double) wxAloftStruct2.lat, (double) wxAloftStruct2.lon) <= radius && !wxAloftStruct2.winds_aloft_string.Contains("9999"))
              wxAloftGrib.Add(wxAloftStruct2);
          }
        }
        catch
        {
        }
        if (str7 == null)
        {
          new WebClient().DownloadFile(address2, str2);
          str7 = System.IO.File.ReadAllText(str2);
        }
        if (this._metarLatestTime == null || this._metarLatestTime != str7)
        {
          new WebClient().DownloadFile(address1, str1 + ".gz");
          GZip.DecompressFile(str1 + ".gz", str1);
          this._metarLatestTime = str7;
        }
        foreach (string readAllLine in System.IO.File.ReadAllLines(str1))
        {
          char[] chArray = new char[1]{ ';' };
          string[] strArray = readAllLine.Split(chArray);
          wxStruct1.station_id = strArray[1];
          wxStruct1.raw_text = strArray[2];
          wxStruct1.raw_time = strArray[3];
          wxStruct1.lat = (float) Convert.ToDouble(strArray[4]);
          wxStruct1.lon = (float) Convert.ToDouble(strArray[5]);
          wxStruct1.temp_c = Convert.ToInt32(strArray[6]);
          wxStruct1.dewpoint_c = Convert.ToInt32(strArray[7]);
          wxStruct1.wind_dir_degrees = Convert.ToInt32(strArray[8]);
          wxStruct1.wind_speed_kt = Convert.ToInt32(strArray[9]);
          wxStruct1.humidity = Convert.ToInt32(Math.Round(6.112 * Math.Exp(17.62 * (double) wxStruct1.dewpoint_c / (243.12 + (double) wxStruct1.dewpoint_c)) / (6.112 * Math.Exp(17.62 * (double) wxStruct1.temp_c / (243.12 + (double) wxStruct1.temp_c))) * 100.0, 0));
          Match match1 = Regex.Match(wxStruct1.raw_text, "\\b(\\d\\d\\d)V(\\d\\d\\d)\\b");
          wxStruct1.wind_variance = match1.Success ? GeoClass.BearingDiffAbs(Convert.ToInt32(match1.Groups[1].Value), Convert.ToInt32(match1.Groups[2].Value)) : 0;
          Match match2 = Regex.Match(wxStruct1.raw_text, "G(\\d\\d)KT\\b");
          wxStruct1.wind_gusts = match2.Success ? Convert.ToInt32(match2.Groups[1].Value) : 0;
          Match match3 = Regex.Match(Regex.Replace(wxStruct1.raw_text, "[QA]\\d\\d\\d\\d\\b.*", ""), "\\bVRB\\d\\d");
          wxStruct1.wind_vrb = match3.Success;
          wxStruct1.visibility_statute_mi = Convert.ToDouble(strArray[10]) > 30.0 || Convert.ToDouble(strArray[10]) == 6.21000003814697 || Convert.ToDouble(strArray[10]) == 10.0 ? 30f : (float) Convert.ToDouble(strArray[10]);
          wxStruct1.altim_in_hg = (float) Convert.ToDouble(strArray[11]);
          wxStruct1.wx_string = strArray[12];
          wxStruct1.sky_condition = strArray[13];
          wxStruct1.elevation_m = Convert.ToInt32(strArray[16]);
          wxStruct1.dist_from_ac = 0.0;
          wxStructList2.Add(wxStruct1);
          if (GeoClass.Dist(aclat, aclon, (double) wxStruct1.lat, (double) wxStruct1.lon) <= radius)
            wxData1.Add(wxStruct1);
        }
        List<WxGridClass.wxStruct> wxMetar = this.FetchWxDataCloneRouteMetar(this.FetchWxDataAloft(this.FetchWxDataVrb(wxData1), wxAloft, wxAloftGrib, "metar"));
        List<WxGridClass.wxStruct> wxGrib = this.FetchWxDataAloft(wxData2, wxAloft, wxAloftGrib, "grib");
        List<WxGridClass.wxStruct> wxStructList3 = this.FetchWxDataCombine(wxMetar, wxGrib);
        this._wxMetar.Clear();
        this._wxGrib.Clear();
        this._wxNew.Clear();
        this._wxAll.Clear();
        this._wxMetar = wxMetar;
        this._wxGrib = wxGrib;
        this._wxNew = wxStructList3;
        this._wxAll = wxStructList2;
        return true;
      }
      catch
      {
        return false;
      }
    }

    private List<WxGridClass.wxStruct> FetchWxDataVrb(List<WxGridClass.wxStruct> wxData)
    {
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      List<WxGridClass.wxStruct> wxStructList1 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList2 = new List<WxGridClass.wxStruct>();
      for (int index = 0; index < wxData.Count; ++index)
      {
        WxGridClass.wxStruct wxStruct2 = wxData[index];
        if (wxStruct2.wind_vrb)
        {
          foreach (WxGridClass.wxStruct wxStruct3 in this.FetchWxDataNearest(this._wxMetar, (double) wxStruct2.lat, (double) wxStruct2.lon, 8))
          {
            if (!wxStruct3.wind_vrb)
            {
              wxStruct2.wind_dir_degrees = wxStruct3.wind_dir_degrees;
              wxStruct2.wind_variance = 270;
              break;
            }
          }
        }
        wxData[index] = wxStruct2;
      }
      return wxData;
    }

    private List<WxGridClass.wxStruct> FetchWxDataAloft(List<WxGridClass.wxStruct> wxData, List<WxGridClass.wxAloftStruct> wxAloft, List<WxGridClass.wxAloftStruct> wxAloftGrib, string source)
    {
      WxGridClass.wxStruct row = new WxGridClass.wxStruct();
      WxGridClass.wxAloftStruct wxAloftStruct1 = new WxGridClass.wxAloftStruct();
      for (int index1 = 0; index1 < wxData.Count; ++index1)
      {
        row = wxData[index1];
        row.winds_aloft = new List<int[]>();
        row.winds_aloft_string = "";
        row.winds_aloft_last_update = "";
        row.winds_aloft_source = "";
        row.winds_aloft_source_type = "";
        WxGridClass.wxAloftStruct wxAloftStruct2;
        TimeSpan timeSpan;
        if (this._truewindsaloft && source == "metar" && wxAloft.Exists((Predicate<WxGridClass.wxAloftStruct>) (j => j.station_id == row.station_id)))
        {
          wxAloftStruct2 = wxAloft.Find((Predicate<WxGridClass.wxAloftStruct>) (j => j.station_id == row.station_id));
          timeSpan = this._datetime - DateTime.Parse(wxAloftStruct2.last_update);
          if (timeSpan.TotalHours <= 48.0)
          {
            string[] strArray1 = wxAloftStruct2.winds_aloft_string.Split('|');
            List<int[]> numArrayList = new List<int[]>();
            numArrayList.Add(new int[4]
            {
              0,
              row.temp_c,
              row.wind_dir_degrees,
              row.wind_speed_kt
            });
            for (int index2 = 0; index2 < ((IEnumerable<string>) strArray1).Count<string>(); ++index2)
            {
              string[] strArray2 = strArray1[index2].Split(',');
              if (Convert.ToInt32(strArray2[0]) > 50)
              {
                int[] numArray = new int[4]
                {
                  Convert.ToInt32(strArray2[0]),
                  Convert.ToInt32(strArray2[1]),
                  Convert.ToInt32(strArray2[2]),
                  Convert.ToInt32(strArray2[3])
                };
                numArrayList.Add(numArray);
              }
            }
            if (this.FetchWxDataAloftSanityCheck(row))
            {
              row.winds_aloft = numArrayList;
              row.winds_aloft_string = wxAloftStruct2.winds_aloft_string;
              row.winds_aloft_last_update = wxAloftStruct2.last_update;
              row.winds_aloft_source = wxAloftStruct2.station_id;
              row.winds_aloft_source_type = "true aloft";
            }
          }
        }
        if (this._truewindsaloft && source == "metar" && (row.winds_aloft_string == "" && wxAloft.Count > 0))
        {
          for (int index2 = 0; index2 < wxAloft.Count; ++index2)
          {
            wxAloftStruct2 = wxAloft[index2];
            wxAloftStruct2.dist = GeoClass.Dist((double) wxAloftStruct2.lat, (double) wxAloftStruct2.lon, (double) row.lat, (double) row.lon);
            wxAloft[index2] = wxAloftStruct2;
          }
          wxAloft = wxAloft.OrderBy<WxGridClass.wxAloftStruct, double>((Func<WxGridClass.wxAloftStruct, double>) (o => o.dist)).ToList<WxGridClass.wxAloftStruct>();
          wxAloftStruct2 = wxAloft[0];
          if (wxAloftStruct2.dist < 500.0)
          {
            timeSpan = this._datetime - DateTime.Parse(wxAloftStruct2.last_update);
            if (timeSpan.TotalHours <= 48.0)
            {
              string[] strArray1 = wxAloftStruct2.winds_aloft_string.Split('|');
              List<int[]> numArrayList = new List<int[]>();
              numArrayList.Add(new int[4]
              {
                0,
                row.temp_c,
                row.wind_dir_degrees,
                row.wind_speed_kt
              });
              for (int index2 = 0; index2 < ((IEnumerable<string>) strArray1).Count<string>(); ++index2)
              {
                string[] strArray2 = strArray1[index2].Split(',');
                if (Convert.ToInt32(strArray2[0]) > 50)
                {
                  int[] numArray = new int[4]
                  {
                    Convert.ToInt32(strArray2[0]),
                    Convert.ToInt32(strArray2[1]),
                    Convert.ToInt32(strArray2[2]),
                    Convert.ToInt32(strArray2[3])
                  };
                  numArrayList.Add(numArray);
                }
              }
              if (this.FetchWxDataAloftSanityCheck(row))
              {
                row.winds_aloft = numArrayList;
                row.winds_aloft_string = wxAloftStruct2.winds_aloft_string;
                row.winds_aloft_last_update = wxAloftStruct2.last_update;
                row.winds_aloft_source = wxAloftStruct2.station_id;
                row.winds_aloft_source_type = "true aloft nearest";
              }
            }
          }
        }
        if (this._truewindsaloft && row.winds_aloft_string == "" && wxAloftGrib.Count > 0)
        {
          if (source == "grib" && wxAloftGrib.Exists((Predicate<WxGridClass.wxAloftStruct>) (j => j.station_id == row.station_id)))
          {
            wxAloftStruct2 = wxAloftGrib.Find((Predicate<WxGridClass.wxAloftStruct>) (j => j.station_id == row.station_id));
            wxAloftStruct2.dist = 0.0;
          }
          else
          {
            for (int index2 = 0; index2 < wxAloftGrib.Count; ++index2)
            {
              wxAloftStruct2 = wxAloftGrib[index2];
              wxAloftStruct2.dist = GeoClass.Dist((double) wxAloftStruct2.lat, (double) wxAloftStruct2.lon, (double) row.lat, (double) row.lon);
              wxAloftGrib[index2] = wxAloftStruct2;
            }
            wxAloftGrib = wxAloftGrib.OrderBy<WxGridClass.wxAloftStruct, double>((Func<WxGridClass.wxAloftStruct, double>) (o => o.dist)).ToList<WxGridClass.wxAloftStruct>();
            wxAloftStruct2 = wxAloftGrib[0];
          }
          if (wxAloftStruct2.dist < 1000.0)
          {
            timeSpan = this._datetime - DateTime.Parse(wxAloftStruct2.last_update);
            if (timeSpan.TotalHours <= 48.0)
            {
              string[] strArray1 = wxAloftStruct2.winds_aloft_string.Split('|');
              List<int[]> numArrayList = new List<int[]>();
              numArrayList.Add(new int[4]
              {
                0,
                row.temp_c,
                row.wind_dir_degrees,
                row.wind_speed_kt
              });
              for (int index2 = 0; index2 < ((IEnumerable<string>) strArray1).Count<string>(); ++index2)
              {
                string[] strArray2 = strArray1[index2].Split(',');
                if (Convert.ToInt32(strArray2[0]) > 50)
                {
                  int[] numArray = new int[4]
                  {
                    Convert.ToInt32(strArray2[0]),
                    Convert.ToInt32(strArray2[1]),
                    Convert.ToInt32(strArray2[2]),
                    Convert.ToInt32(strArray2[3])
                  };
                  numArrayList.Add(numArray);
                }
              }
              if (this.FetchWxDataAloftSanityCheck(row))
              {
                row.winds_aloft = numArrayList;
                row.winds_aloft_string = wxAloftStruct2.winds_aloft_string;
                row.winds_aloft_last_update = wxAloftStruct2.last_update;
                row.winds_aloft_source = wxAloftStruct2.station_id;
                row.winds_aloft_source_type = "grib aloft";
              }
            }
          }
        }
        int tempC = row.temp_c;
        string str = "";
        for (int index2 = 0; index2 < ((IEnumerable<int>) this._aloftlevelsStd).Count<int>(); ++index2)
        {
          if (index2 == 0)
            tempC -= 10;
          else if (index2 < 5)
            tempC -= 6;
          else if (index2 < 9)
            tempC -= 10;
          str = str + (object) this._aloftlevelsStd[index2] + "," + tempC.ToString() + "," + row.wind_dir_degrees.ToString() + "," + (row.wind_speed_kt + (index2 + 1) * 4).ToString() + "|";
        }
        row.winds_aloft_calc_string = str.Substring(0, str.Length - 1);
        string[] strArray3 = row.winds_aloft_calc_string.Split('|');
        List<int[]> numArrayList1 = new List<int[]>();
        numArrayList1.Add(new int[4]
        {
          0,
          row.temp_c,
          row.wind_dir_degrees,
          row.wind_speed_kt
        });
        for (int index2 = 0; index2 < ((IEnumerable<string>) strArray3).Count<string>(); ++index2)
        {
          string[] strArray1 = strArray3[index2].Split(',');
          if (Convert.ToInt32(strArray1[0]) > 50)
          {
            int[] numArray = new int[4]
            {
              Convert.ToInt32(strArray1[0]),
              Convert.ToInt32(strArray1[1]),
              Convert.ToInt32(strArray1[2]),
              Convert.ToInt32(strArray1[3])
            };
            numArrayList1.Add(numArray);
          }
        }
        row.winds_aloft_calc = numArrayList1;
        row.winds_aloft_ext = this.CreateWindsAloftExt(row);
        wxData[index1] = row;
      }
      return wxData;
    }

    private bool FetchWxDataAloftSanityCheck(WxGridClass.wxStruct wxRow)
    {
      foreach (int[] numArray in wxRow.winds_aloft)
      {
        if (numArray[1] > wxRow.temp_c + 20)
          return false;
      }
      return true;
    }

    private List<WxGridClass.wxStruct> FetchWxDataCloneRouteMetar(List<WxGridClass.wxStruct> wxData)
    {
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      WxGridClass.wxStruct wxStruct2 = new WxGridClass.wxStruct();
      string[] strArray = new string[2]
      {
        this._ades,
        this._adep
      };
      foreach (string str in strArray)
      {
        string airport = str;
        if (airport != "" && wxData.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == airport)))
        {
          WxGridClass.wxStruct wxStruct3 = wxData.Find((Predicate<WxGridClass.wxStruct>) (i => i.station_id == airport));
          for (int index = 0; index < wxData.Count; ++index)
          {
            WxGridClass.wxStruct wxStruct4 = wxData[index];
            double num = GeoClass.Dist((double) wxStruct4.lat, (double) wxStruct4.lon, (double) wxStruct3.lat, (double) wxStruct3.lon);
            if (num < 80.0)
            {
              wxStruct4.visibility_statute_mi = wxStruct3.visibility_statute_mi;
              if (num < 40.0)
                wxStruct4.winds_aloft_ext = wxStruct3.winds_aloft_ext;
              wxData[index] = wxStruct4;
            }
          }
        }
      }
      return wxData;
    }

    private List<WxGridClass.wxStruct> FetchWxDataCombine(List<WxGridClass.wxStruct> wxMetar, List<WxGridClass.wxStruct> wxGrib)
    {
      List<WxGridClass.wxStruct> wxStructList1 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList2 = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      List<WxGridClass.wxStruct> wxStructList3 = new List<WxGridClass.wxStruct>((IEnumerable<WxGridClass.wxStruct>) wxMetar);
      for (int index = 0; index < wxGrib.Count; ++index)
      {
        WxGridClass.wxStruct wxStruct2 = wxGrib[index];
        List<WxGridClass.wxStruct> wxStructList4 = this.FetchWxDataNearest(wxMetar, (double) wxStruct2.lat, (double) wxStruct2.lon, 1);
        if (wxStructList4.Count == 0 || GeoClass.Dist((double) wxStruct2.lat, (double) wxStruct2.lon, (double) wxStructList4[0].lat, (double) wxStructList4[0].lon) > 100.0)
          wxStructList3.Add(wxGrib[index]);
      }
      return wxStructList3;
    }

    private struct wxGridListStruct
    {
      public string id;
      public float lat;
      public float lon;
    }

    public struct wxStruct
    {
      public string station_id;
      public string raw_text;
      public string raw_time;
      public string interpolated_from;
      public float lat;
      public float lon;
      public int temp_c;
      public int dewpoint_c;
      public int humidity;
      public int wind_dir_degrees;
      public int wind_speed_kt;
      public int wind_variance;
      public int wind_gusts;
      public bool wind_vrb;
      public float visibility_statute_mi;
      public float altim_in_hg;
      public string wx_string;
      public string wx_string_fsx;
      public string sky_condition;
      public string sky_fsx_native;
      public string sky_fsx_smooth;
      public int elevation_m;
      public string winds_aloft_string;
      public string winds_aloft_calc_string;
      public string winds_aloft_source;
      public string winds_aloft_source_type;
      public string winds_aloft_last_update;
      public List<int[]> vis_aloft;
      public List<int[]> winds_aloft;
      public List<int[]> winds_aloft_calc;
      public List<int[]> winds_aloft_ext;
      public string fsx;
      public string fsx_turb_descr;
      public double dist_from_ac;
    }

    public struct wxAloftStruct
    {
      public string station_id;
      public float lat;
      public float lon;
      public string last_update;
      public string winds_aloft_string;
      public double dist;
    }

    public struct weightingStruct
    {
      public double sum_dist;
      public double sum_weights;
      public double num_temp_c;
      public double num_dewpoint_c;
      public double num_humidity;
      public double num_visibility_statute_mi;
      public double num_altim_in_hg;
      public double num_elevation_m;
      public double num_wind_gusts;
      public double num_wind_variance;
    }
  }
}
