// Decompiled with JetBrains decompiler
// Type: FSXWX.WxInjClass
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace FSXWX
{
  public class WxInjClass
  {
    private List<string> _createdWxStations = new List<string>();
    private List<string> _removedWxStations = new List<string>();
    private string _ezdokExe = Path.GetTempPath() + "/ezdok.exe";
    private RegistryKey _regKeys = Registry.CurrentUser.CreateSubKey("Software\\FSXWX");

    public List<string> createdWxStations
    {
      get
      {
        return this._createdWxStations;
      }
      set
      {
        this._createdWxStations = value;
      }
    }

    public List<string> removedWxStations
    {
      get
      {
        return this._removedWxStations;
      }
      set
      {
        this._removedWxStations = value;
      }
    }

    public string WxInject(Microsoft.FlightSimulator.SimConnect.SimConnect simconnect, List<WxGridClass.wxStruct> wxGrid, Form1.dataPosStruct acPos, string type, double radius, int maxstations, uint seconds, bool firstInject)
    {
      List<WxGridClass.wxStruct> source1 = new List<WxGridClass.wxStruct>();
      WxGridClass.wxStruct wxStruct1 = new WxGridClass.wxStruct();
      bool flag1 = false;
      bool flag2 = false;
      int num1 = 0;
      int millisecondsTimeout = 0;
      string str1 = (string) this._regKeys.GetValue("EzdokGlobalEnable");
      string str2 = "Injection mode: " + type + " - ";
      foreach (WxGridClass.wxStruct wxStruct2 in wxGrid)
      {
        WxGridClass.wxStruct wxStruct3 = wxStruct2;
        wxStruct3.dist_from_ac = GeoClass.Dist(acPos.lat, acPos.lon, (double) wxStruct2.lat, (double) wxStruct2.lon);
        if (wxStruct3.dist_from_ac <= radius)
          source1.Add(wxStruct3);
      }
      List<WxGridClass.wxStruct> source2 = source1.OrderBy<WxGridClass.wxStruct, double>((Func<WxGridClass.wxStruct, double>) (o => o.dist_from_ac)).ToList<WxGridClass.wxStruct>();
      if (!(type == "ovconly"))
      {
        if (!(type == "ovcdist"))
        {
          if (!(type == "ovctime"))
          {
            if (!(type == "ovcgrnd"))
            {
              if (!(type == "all"))
                return "Invalid injection type";
              flag1 = true;
              flag2 = true;
              millisecondsTimeout = 5000;
            }
            else
            {
              num1 = 1;
              foreach (WxGridClass.wxStruct wxStruct2 in source2)
              {
                str2 = str2 + wxStruct2.station_id + " nearby - ";
                Regex regex = new Regex("\\b[345678](AS|CU)(\\d\\d\\d)", RegexOptions.Compiled);
                if (regex.Matches(wxStruct2.sky_fsx_smooth).Count > 0 && Convert.ToInt32(regex.Match(wxStruct2.sky_fsx_smooth).Groups[2].Value) < 100)
                {
                  str2 = str2 + regex.Match(wxStruct2.sky_fsx_smooth).Groups[0].ToString() + " - ";
                  flag1 = true;
                  flag2 = false;
                  millisecondsTimeout = 0;
                  break;
                }
              }
            }
          }
          else
          {
            num1 = 1;
            double num2 = (double) GeoClass.BearingSub((int) acPos.hdg, 100);
            double num3 = (double) GeoClass.BearingAdd((int) acPos.hdg, 100);
            foreach (WxGridClass.wxStruct wxStruct2 in source2)
            {
              double num4 = GeoClass.DegreeBearing(acPos.lat, acPos.lon, (double) wxStruct2.lat, (double) wxStruct2.lon);
              if (num3 > num2 && num4 >= num2 && num4 <= num3 || num3 < num2 && (num4 >= num2 || num4 <= num3))
              {
                str2 = str2 + wxStruct2.station_id + " ahead - ";
                Regex regex = new Regex("\\b[345678](AS|CU)(\\d\\d\\d)", RegexOptions.Compiled);
                if (regex.Matches(wxStruct2.sky_fsx_smooth).Count > 0 && Convert.ToInt32(regex.Match(wxStruct2.sky_fsx_smooth).Groups[2].Value) < 100)
                {
                  str2 = str2 + regex.Match(wxStruct2.sky_fsx_smooth).Groups[0].ToString() + " - ";
                  flag1 = true;
                  flag2 = false;
                  millisecondsTimeout = 0;
                  break;
                }
              }
            }
          }
        }
        else
        {
          num1 = 1;
          double num2 = (double) GeoClass.BearingSub((int) acPos.hdg, 100);
          double num3 = (double) GeoClass.BearingAdd((int) acPos.hdg, 100);
          foreach (WxGridClass.wxStruct wxStruct2 in source2)
          {
            double num4 = GeoClass.DegreeBearing(acPos.lat, acPos.lon, (double) wxStruct2.lat, (double) wxStruct2.lon);
            if (num3 > num2 && num4 >= num2 && num4 <= num3 || num3 < num2 && (num4 >= num2 || num4 <= num3))
            {
              str2 = str2 + wxStruct2.station_id + " ahead - ";
              Regex regex = new Regex("\\b[5678](AS|CU)(\\d\\d\\d)", RegexOptions.Compiled);
              if (regex.Matches(wxStruct2.sky_fsx_smooth).Count > 0 && Convert.ToInt32(regex.Match(wxStruct2.sky_fsx_smooth).Groups[2].Value) < 100)
              {
                str2 = str2 + regex.Match(wxStruct2.sky_fsx_smooth).Groups[0].ToString() + " - ";
                flag1 = true;
                flag2 = false;
                millisecondsTimeout = 0;
                break;
              }
            }
          }
        }
      }
      else
      {
        List<WxGridClass.wxStruct> wxStructList = new List<WxGridClass.wxStruct>();
        num1 = 1;
        double num2 = (double) GeoClass.BearingSub((int) acPos.hdg, 100);
        double num3 = (double) GeoClass.BearingAdd((int) acPos.hdg, 100);
        foreach (WxGridClass.wxStruct wxStruct2 in source2)
        {
          double num4 = GeoClass.DegreeBearing(acPos.lat, acPos.lon, (double) wxStruct2.lat, (double) wxStruct2.lon);
          if (num3 > num2 && num4 >= num2 && num4 <= num3 || num3 < num2 && (num4 >= num2 || num4 <= num3))
          {
            str2 = str2 + wxStruct2.station_id + " ahead - ";
            Regex regex = new Regex("\\b[5678](AS|CU)(\\d\\d\\d)", RegexOptions.Compiled);
            if (regex.Matches(wxStruct2.sky_fsx_smooth).Count > 0 && Convert.ToInt32(regex.Match(wxStruct2.sky_fsx_smooth).Groups[2].Value) < 100)
            {
              str2 = str2 + regex.Match(wxStruct2.sky_fsx_smooth).Groups[0].ToString() + " - ";
              flag1 = true;
              flag2 = false;
              millisecondsTimeout = 0;
              wxStructList.Add(wxStruct2);
            }
          }
        }
        source2.Clear();
        source2 = wxStructList;
      }
      int num5 = 0;
      foreach (WxGridClass.wxStruct wxStruct2 in source2)
      {
        if (!this._createdWxStations.Contains(wxStruct2.station_id))
        {
          simconnect.WeatherCreateStation((Enum) Form1.DATA_REQUESTS.WEATHERSTATIONNEW, wxStruct2.station_id, wxStruct2.station_id, wxStruct2.lat, wxStruct2.lon, (float) (wxStruct2.elevation_m + 60));
          this._createdWxStations.Add(wxStruct2.station_id);
          this._removedWxStations.Remove(wxStruct2.station_id);
          ++num5;
        }
      }
      string str3 = str2 + num5.ToString() + " new stations created - ";
      if (str1 == "" | firstInject)
        flag2 = false;
      if (flag1)
      {
        if (flag2)
        {
          Process.Start(this._ezdokExe);
          str3 += "EZdok disabled - ";
        }
        if (source2.Count > maxstations)
          source2 = source2.GetRange(0, maxstations);
        List<WxGridClass.wxStruct> list = source2.OrderBy<WxGridClass.wxStruct, string>((Func<WxGridClass.wxStruct, string>) (o => o.station_id)).ToList<WxGridClass.wxStruct>();
        for (int index = 0; index <= num1; ++index)
        {
          num5 = 0;
          foreach (WxGridClass.wxStruct wxStruct2 in list)
          {
            simconnect.WeatherSetObservation(seconds, wxStruct2.fsx);
            ++num5;
          }
        }
        if (flag2)
        {
          Thread.Sleep(millisecondsTimeout);
          Process.Start(this._ezdokExe);
        }
      }
      string str4 = str3 + num5.ToString() + " stations injected" + (num1 > 0 ? " - " + num1.ToString() + " repeats" : "");
      simconnect.WeatherSetDynamicUpdateRate(0U);
      return str4;
    }

    public void WxClear(Microsoft.FlightSimulator.SimConnect.SimConnect simconnect)
    {
      simconnect.WeatherSetModeGlobal();
      simconnect.WeatherSetModeTheme("");
      simconnect.WeatherSetModeCustom();
      simconnect.WeatherSetDynamicUpdateRate(0U);
      Thread.Sleep(1000);
    }

    public void WxSetGlobalMode(Microsoft.FlightSimulator.SimConnect.SimConnect simconnect)
    {
      simconnect.WeatherSetModeGlobal();
    }

    public void WxTestTurb(Microsoft.FlightSimulator.SimConnect.SimConnect simconnect, string strength)
    {
      string input = "GLOB 01005KT&D305###G 01005KT&A605###G 02205KT&A905###G 03405KT&A1205###G 04605KT&A1505###G 05806KT&A1805###G 07006KT&A2105###G 07005KT&A2405###G 07205KT&A2705###G 04804KT&A3305###G 03803KT&A3905###G 01403KT&A4505###G 00604KT&A5105###G 34204KT&A5705###G 31806KT&A6305NG 27011KT&A7505###G 25913KT&A8705###G 26816KT&A9905###G 27216KT&A11105###G 26213KT&A15000###G 270/10SM&B-500&D1920 285/10SM&B1421&D720 312/10SM&B2142&D600 346/10SM&B2743&D600 377/10SM&B3344&D600 400/10SM&B3945&D720 420/10SM&B4666&D780 471/10SM&B5447&D1600 547/10SM&B7048&D1600 600/10SM&B8649&D8000 4CI382 22/17&A0 22/17&A300 17/12&A600 15/10&A900 13/08&A1200 10/05&A1500 08/03&A1800 06/01&A2100 05/00&A2400 01/-04&A3000 -03/-08&A3600 -07/-12&A4200 -12/-17&A4800 -16/-21&A5400 -21/-26&A6000 -31/-36&A7200 -41/-46&A8400 -50/-55&A9600 -54/-59&A10800 -54/-59&A15000 A2992";
      this.WxClear(simconnect);
      simconnect.WeatherSetModeGlobal();
      simconnect.WeatherSetObservation(0U, Regex.Replace(input, "###", strength));
    }
  }
}
