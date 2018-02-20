// Decompiled with JetBrains decompiler
// Type: FSXWX.Form1
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using Microsoft.FlightSimulator.SimConnect;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace FSXWX
{
  public class Form1 : Form
  {
    private System.Windows.Forms.Timer WxGridWindUpdateTimer = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer WxOnGroundInjectTimer = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer AutoConnectTimer = new System.Windows.Forms.Timer();
    private string _simChoice = "";
    private string _timeChoice = "";
    private bool _bFirstInject = true;
    private DateTime simRefDateTime = new DateTime(1, 1, 1, 0, 0, 0);
    private RegistryKey _regKeys = Registry.CurrentUser.CreateSubKey("Software\\FSXWX");
    private WxGridClass _wxGridObj;
    private WxInjClass _wxInjObj;
    private WxDwcClass _wxDwcObj;
    private bool _bRouteChanged;
    private Microsoft.FlightSimulator.SimConnect.SimConnect simconnect;
    private DateTime simconnectConnectionTime;
    private Form1.dataPos _acPos;
    private int _acHdgStatic;
    private int _mouseStatic;
    private const int WM_USER_SIMCONNECT = 1026;
    private IContainer components;
    private TextBox textBoxAdep;
    private TextBox textBoxAdes;
    private Button buttonDisconnect;
    private Button buttonConnect;
    private RichTextBox richResponseLog;
    private Button buttonWxReload;
    private TextBox textBoxAltn;
    private DateTimePicker dateTimePicker;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private Label label1;
    private RadioButton radioButtonTimeHistoric;
    private RadioButton radioButtonTimeCurrent;
    private Label label3;
    private Label label2;
    private GroupBox groupBox3;
    private GroupBox groupBox4;
    private CheckBox checkBoxReinjectOvc;
    private GroupBox groupBox5;
    private CheckBox checkBoxTrueWindsAloft;
    private Label label4;
    private TextBox textBoxAsCuThreshold;
    private CheckBox checkBoxFsxInternalWindTurb;
    private GroupBox groupBox6;
    private Button buttonTurbTest;
    private ComboBox comboBoxTurbTest;
    private GroupBox groupBox7;
    private ComboBox comboBoxDwcGustsSpd;
    private Label label6;
    private ComboBox comboBoxDwcTurb;
    private Label label5;
    private Label label8;
    private Label label7;
    private ComboBox comboBoxDwcRandom;
    private ComboBox comboBoxDwcVariance;
    private Button buttonInject;
    private TextBox textBoxWxString;
    private Button buttonWxRequest;
    private GroupBox groupBox8;
    private Button buttonCheck;
    private TextBox textBoxCheck;
    private Button buttonNearestMetar;
    private Label label9;
    private ComboBox comboBoxDwcGustsDir;
    private Label label10;
    private TextBox textBoxEzdok;
    private GroupBox groupBox9;
    private RadioButton radioButtonSimP3dVolFog;
    private RadioButton radioButtonSimP3d;
    private RadioButton radioButtonSimFsx;
    private CheckBox checkBoxAutoConnect;
    private RadioButton radioButtonTimeSim;

    public Form1()
    {
      this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      this.InitializeComponent();
      this.SetResetWxInjObj();
      this.SetResetWxDwcObj();
      this.dateTimePicker.MaxDate = DateTime.UtcNow.AddMinutes(1.0);
      this.dateTimePicker.Value = DateTime.UtcNow;
      if (this._regKeys.GetValue("TimeChoice") == null)
        this._regKeys.SetValue("TimeChoice", (object) "sim");
      this._timeChoice = this._regKeys.GetValue("TimeChoice").ToString();
      if (this._regKeys.GetValue("TimeChoice").ToString() == "sim")
        this.radioButtonTimeSim.Checked = true;
      else if (this._regKeys.GetValue("TimeChoice").ToString() == "current")
        this.radioButtonTimeCurrent.Checked = true;
      else if (this._regKeys.GetValue("TimeChoice").ToString() == "historic")
      {
        this.radioButtonTimeHistoric.Checked = true;
        this.dateTimePicker.Enabled = true;
      }
      if (this._regKeys.GetValue("SimChoice") == null)
        this._regKeys.SetValue("SimChoice", (object) "FSX");
      this._simChoice = this._regKeys.GetValue("SimChoice").ToString();
      if (this._regKeys.GetValue("SimChoice").ToString() == "FSX")
        this.radioButtonSimFsx.Checked = true;
      else if (this._regKeys.GetValue("SimChoice").ToString() == "P3D")
        this.radioButtonSimP3d.Checked = true;
      else if (this._regKeys.GetValue("SimChoice").ToString() == "P3DVF")
        this.radioButtonSimP3dVolFog.Checked = true;
      this.checkBoxAutoConnect.Checked = this._regKeys.GetValue("AutoConnect") == null || Convert.ToBoolean(this._regKeys.GetValue("AutoConnect"));
      this.checkBoxReinjectOvc.Checked = this._regKeys.GetValue("ReinjectOvcConditions") == null || Convert.ToBoolean(this._regKeys.GetValue("ReinjectOvcConditions"));
      this.checkBoxTrueWindsAloft.Checked = this._regKeys.GetValue("TrueWindsAloft") == null || Convert.ToBoolean(this._regKeys.GetValue("TrueWindsAloft"));
      this.checkBoxFsxInternalWindTurb.Checked = this._regKeys.GetValue("FsxInternalWindTurb") == null || Convert.ToBoolean(this._regKeys.GetValue("FsxInternalWindTurb"));
      if (this._regKeys.GetValue("EzdokGlobalEnable") == null)
        this._regKeys.SetValue("EzdokGlobalEnable", (object) "");
      this.textBoxEzdok.Text = (string) this._regKeys.GetValue("EzdokGlobalEnable");
      this.textBoxAsCuThreshold.Text = this._regKeys.GetValue("AsCuThreshold") == null ? "29.90" : (string) this._regKeys.GetValue("AsCuThreshold");
      this.comboBoxDwcGustsSpd.SelectedIndex = this._regKeys.GetValue("DwcGustsSpd") == null ? 2 : Convert.ToInt32(this._regKeys.GetValue("DwcGustsSpd"));
      this.comboBoxDwcGustsDir.SelectedIndex = this._regKeys.GetValue("DwcGustsDir") == null ? 2 : Convert.ToInt32(this._regKeys.GetValue("DwcGustsDir"));
      this.comboBoxDwcTurb.SelectedIndex = this._regKeys.GetValue("DwcTurb") == null ? 2 : Convert.ToInt32(this._regKeys.GetValue("DwcTurb"));
      this.comboBoxDwcVariance.SelectedIndex = this._regKeys.GetValue("DwcVariance") == null ? 2 : Convert.ToInt32(this._regKeys.GetValue("DwcVariance"));
      this.comboBoxDwcRandom.SelectedIndex = this._regKeys.GetValue("DwcRandom") == null ? 2 : Convert.ToInt32(this._regKeys.GetValue("DwcRandom"));
      this.comboBoxTurbTest.SelectedIndex = 0;
      this.setForm(false, (Control) this);
      this.WxGridWindUpdateTimer.Interval = 495000;
      this.WxGridWindUpdateTimer.Tick += new EventHandler(this.WxGridWindUpdateTimer_Tick);
      this.AutoConnectTimer.Interval = 30000;
      this.AutoConnectTimer.Tick += new EventHandler(this.AutoConnectTimer_Tick);
      if (this.checkBoxAutoConnect.Checked)
        this.AutoConnectTimer.Start();
      this.WxOnGroundInjectTimer.Interval = 5000;
      this.WxOnGroundInjectTimer.Tick += new EventHandler(this.WxOnGroundInjectTimer_Tick);
      LogClass.DeleteLogs();
      try
      {
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FSXWX.ezdok.exe");
        FileStream fileStream = new FileStream(Path.GetTempPath() + "/ezdok.exe", FileMode.Create);
        for (int index = 0; (long) index < manifestResourceStream.Length; ++index)
          fileStream.WriteByte((byte) manifestResourceStream.ReadByte());
        fileStream.Close();
      }
      catch
      {
        this.displayText("Error accessing resources!");
      }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.closeConnection();
    }

    protected override void DefWndProc(ref Message m)
    {
      if (m.Msg == 1026)
      {
        if (this.simconnect == null)
          return;
        this.simconnect.ReceiveMessage();
      }
      else
        base.DefWndProc(ref m);
    }

    private void setForm(bool bConnStatus, Control control)
    {
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
      {
        if (control1 is Button || control1 is ComboBox)
        {
          if (control1.Name == "buttonConnect")
          {
            control1.Enabled = !bConnStatus;
          }
          else
          {
            control1.Enabled = bConnStatus;
            if (!bConnStatus)
            {
              this.textBoxAdep.Clear();
              this.textBoxAdes.Clear();
              this.textBoxAltn.Clear();
            }
          }
        }
        else if (control1.Controls.Count > 0)
          this.setForm(bConnStatus, control1);
      }
    }

    private void closeConnection()
    {
      this.WxGridWindUpdateTimer.Stop();
      if (this.simconnect == null)
        return;
      try
      {
        this._wxInjObj.WxClear(this.simconnect);
        this._wxInjObj.WxSetGlobalMode(this.simconnect);
        this.SetResetWxGridObj();
        this.SetResetWxInjObj();
        this.SetResetWxDwcObj();
      }
      catch
      {
        this.displayText("Weather reset on closing failed");
      }
      this.simconnect.Dispose();
      this.simconnect = (Microsoft.FlightSimulator.SimConnect.SimConnect) null;
      this.setForm(false, (Control) this);
      this.displayText("Connection closed");
    }

    private void simconnectReconnect()
    {
      this._wxInjObj.WxClear(this.simconnect);
      this._wxInjObj.WxSetGlobalMode(this.simconnect);
      this.simconnect.Dispose();
      this.simconnect = (Microsoft.FlightSimulator.SimConnect.SimConnect) null;
      try
      {
        this.simconnect = new Microsoft.FlightSimulator.SimConnect.SimConnect("Managed Data Request", this.Handle, 1026U, (WaitHandle) null, 0U);
      }
      catch (COMException ex)
      {
        this.displayText(ex.Message);
        return;
      }
      this.simconnect_initialization();
    }

    private void simconnect_initialization()
    {
      try
      {
        this.simconnect.OnRecvOpen += new Microsoft.FlightSimulator.SimConnect.SimConnect.RecvOpenEventHandler(this.simconnect_OnRecvOpen);
        this.simconnect.OnRecvQuit += new Microsoft.FlightSimulator.SimConnect.SimConnect.RecvQuitEventHandler(this.simconnect_OnRecvQuit);
        this.simconnect.OnRecvException += new Microsoft.FlightSimulator.SimConnect.SimConnect.RecvExceptionEventHandler(this.simconnect_OnRecvException);
        this.simconnect.OnRecvSimobjectData += new Microsoft.FlightSimulator.SimConnect.SimConnect.RecvSimobjectDataEventHandler(this.simconnect_OnRecvSimobjectData);
        this.simconnect.OnRecvWeatherObservation += new Microsoft.FlightSimulator.SimConnect.SimConnect.RecvWeatherObservationEventHandler(this.simconnect_OnRecvWeatherObservation);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Plane Alt Above Ground", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Plane Heading Degrees Magnetic", "degree", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Airspeed Indicated", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Ground Velocity", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.AddToDataDefinition((Enum) Form1.DEFINITIONS.dataPosStruct, "Absolute Time", "seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_UNUSED);
        this.simconnect.RegisterDataDefineStruct<Form1.dataPosStruct>((Enum) Form1.DEFINITIONS.dataPosStruct);
        this.simconnect.RequestDataOnSimObject((Enum) Form1.DATA_REQUESTS.POSITION, (Enum) Form1.DEFINITIONS.dataPosStruct, Microsoft.FlightSimulator.SimConnect.SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT, 0U, 2U, 0U);
        this.simconnect.MapClientEventToSimEvent((Enum) Form1.EVENT_CTRL.PAUSE_ON, "PAUSE_ON");
        this.simconnect.MapClientEventToSimEvent((Enum) Form1.EVENT_CTRL.PAUSE_OFF, "PAUSE_OFF");
        this.simconnectConnectionTime = DateTime.UtcNow;
      }
      catch (COMException ex)
      {
        this.displayText(ex.Message);
      }
    }

    private void simconnect_OnRecvOpen(Microsoft.FlightSimulator.SimConnect.SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
      this._bFirstInject = true;
      this.setForm(true, (Control) this);
    }

    private void simconnect_OnRecvQuit(Microsoft.FlightSimulator.SimConnect.SimConnect sender, SIMCONNECT_RECV data)
    {
      Application.Exit();
    }

    private void simconnect_OnRecvException(Microsoft.FlightSimulator.SimConnect.SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
    {
      this.displayText("Exception received: " + (object) data.dwException);
    }

    private void simconnect_OnRecvSimobjectData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
    {
      switch (data.dwRequestID)
      {
        case 0:
          this._acPos.curr = (Form1.dataPosStruct) data.dwData[0];
          this.WxProcessing();
          this._wxDwcObj.CollectWindData(this._wxGridObj);
          break;
        case 1:
          this.displayText("Weather at station request");
          break;
        case 2:
          this.displayText("Weather at nearest station request");
          break;
        default:
          this.displayText("Unknown request ID: " + (object) data.dwRequestID);
          break;
      }
    }

    private void simconnect_OnRecvWeatherObservation(Microsoft.FlightSimulator.SimConnect.SimConnect sender, SIMCONNECT_RECV_WEATHER_OBSERVATION data)
    {
      this.displayText(data.szMetar);
    }

    private void buttonConnect_Click(object sender, EventArgs e)
    {
      this.SetResetWxGridObj();
      if (this.simconnect == null)
      {
        if (this.checkBoxAutoConnect.Checked)
          this.AutoConnectTimer.Start();
        try
        {
          this.simconnect = new Microsoft.FlightSimulator.SimConnect.SimConnect("Managed Data Request", this.Handle, 1026U, (WaitHandle) null, 0U);
          this.simconnect_initialization();
          this.displayText("SIMCONNECT connection established");
        }
        catch
        {
          this.displayText("SIMCONNECT connection failed");
          return;
        }
        if (this._wxDwcObj.DwcOpen())
        {
          this.displayText("FSUIPC connection established");
        }
        else
        {
          this.displayText("FSUIPC connection failed");
          this.closeConnection();
        }
      }
      else
      {
        this.displayText("Connection error");
        this.closeConnection();
      }
    }

    private void buttonDisconnect_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Do you really want to disconnect?", "Disconnect", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      this.AutoConnectTimer.Stop();
      this.closeConnection();
    }

    private void WxGridWindUpdateTimer_Tick(object sender, EventArgs e)
    {
      if (!(this._timeChoice != "historic"))
        return;
      if (this._wxGridObj.FetchWxData(this._acPos.curr.lat, this._acPos.curr.lon, 800.0))
        this.displayText(this._wxGridObj.WxGridWindUpdate(this._acPos.curr.lat, this._acPos.curr.lon, 400.0));
      else
        this.displayText("WX DATA download for DWC failed");
    }

    private void WxOnGroundInjectTimer_Tick(object sender, EventArgs e)
    {
      if (this._acPos.curr.spdGnd <= 1.0)
      {
        this.displayText(this._wxInjObj.WxInject(this.simconnect, this._wxGridObj.wxGrid, this._acPos.curr, "ovcgrnd", 100.0, 10, 0U, this._bFirstInject));
        this._acPos.lastInjOvc = this._acPos.curr;
        this._acPos.lastInjOvcTime = DateTime.UtcNow;
      }
      this.WxOnGroundInjectTimer.Stop();
    }

    private void AutoConnectTimer_Tick(object sender, EventArgs e)
    {
      if (this.simconnect != null || (!(this._simChoice == "FSX") || Process.GetProcessesByName("fsx").Length == 0) && (!(this._simChoice == "P3D") && !(this._simChoice == "P3DVF") || Process.GetProcessesByName("Prepar3D").Length == 0))
        return;
      this.displayText("Auto connect attempt");
      this.buttonConnect_Click(sender, e);
    }

    private void WxProcessing()
    {
      List<WxGridClass.wxStruct> wxStructList1 = new List<WxGridClass.wxStruct>();
      this._mouseStatic = Cursor.Position.ToString() == this._acPos.lastMousePos ? this._mouseStatic + 1 : 0;
      this._acPos.lastMousePos = Cursor.Position.ToString();
      this._acHdgStatic = Math.Abs(this._acPos.curr.hdg - this._acPos.last.hdg) <= 0.2 ? this._acHdgStatic + 1 : 0;
      DateTime dateTime1;
      if (this._timeChoice == "sim")
      {
        if (this._acPos.curr.absTime != 0.0)
        {
          DateTime dateTime2 = this.simRefDateTime.AddSeconds(this._acPos.curr.absTime);
          while (dateTime2 > DateTime.UtcNow)
            dateTime2 = dateTime2.AddYears(-1);
          DateTimePicker dateTimePicker1 = this.dateTimePicker;
          dateTime1 = DateTime.UtcNow;
          DateTime dateTime3 = dateTime1.AddMinutes(1.0);
          dateTimePicker1.MaxDate = dateTime3;
          DateTimePicker dateTimePicker2 = this.dateTimePicker;
          this._wxGridObj.datetime = dateTime1 = dateTime2;
          DateTime dateTime4 = dateTime1;
          dateTimePicker2.Value = dateTime4;
        }
      }
      else if (this._timeChoice == "current")
      {
        DateTimePicker dateTimePicker1 = this.dateTimePicker;
        dateTime1 = DateTime.UtcNow;
        DateTime dateTime2 = dateTime1.AddMinutes(1.0);
        dateTimePicker1.MaxDate = dateTime2;
        DateTimePicker dateTimePicker2 = this.dateTimePicker;
        this._wxGridObj.datetime = dateTime1 = DateTime.UtcNow;
        DateTime dateTime3 = dateTime1;
        dateTimePicker2.Value = dateTime3;
      }
      if (this._bFirstInject)
      {
        this._wxInjObj.WxClear(this.simconnect);
        if (this._wxGridObj.FetchWxData(this._acPos.curr.lat, this._acPos.curr.lon, 1600.0))
        {
          if (this.checkBoxAutoConnect.Checked && this.textBoxAdep.Text.Trim() == "")
          {
            List<WxGridClass.wxStruct> wxStructList2 = this._wxGridObj.FetchWxDataNearest(this._wxGridObj.wxMetar, this._acPos.curr.lat, this._acPos.curr.lon, 1);
            if (wxStructList2[0].dist_from_ac < 10.0)
            {
              this.textBoxAdep.Text = wxStructList2[0].station_id;
              this.displayText(this.textBoxAdep.Text + " found nearby and automatically entered as ADEP route input");
            }
          }
          if (this.textBoxAdep.Text.Trim().Length == 4 || this.textBoxAdep.Text.Trim().Length == 0)
            this._wxGridObj.adep = this.textBoxAdep.Text.Trim();
          if (this.textBoxAdes.Text.Trim().Length == 4 || this.textBoxAdes.Text.Trim().Length == 0)
            this._wxGridObj.ades = this.textBoxAdes.Text.Trim();
          if (this.textBoxAltn.Text.Trim().Length == 4 || this.textBoxAltn.Text.Trim().Length == 0)
            this._wxGridObj.altn = this.textBoxAltn.Text.Trim();
          this.displayText(this._wxGridObj.WxGridCreate(this._acPos.curr.lat, this._acPos.curr.lon, 400.0, true, this._simChoice));
          this.displayText(this._wxInjObj.WxInject(this.simconnect, this._wxGridObj.wxGrid, this._acPos.curr, "all", 400.0, 99, 0U, this._bFirstInject));
          this._wxDwcObj.StartTimer();
          this.WxGridWindUpdateTimer.Start();
          this._acPos.lastInjAll = this._acPos.curr;
          this._acPos.lastInjOvc = this._acPos.curr;
          this._acPos.lastInjAllTime = DateTime.UtcNow;
          this._acPos.lastInjOvcTime = DateTime.UtcNow;
          this._bFirstInject = false;
        }
        else
        {
          this.displayText("WX DATA initial download failed");
          this.AutoConnectTimer.Stop();
          this.closeConnection();
        }
      }
      else if (GeoClass.Dist(this._acPos.curr.lat, this._acPos.curr.lon, this._acPos.last.lat, this._acPos.last.lon) > 20.0)
      {
        if (this.textBoxAdes.Text.Trim() == "")
          this.textBoxAdep.Text = "";
        this.displayText("Large simulation position change detected");
        this.WxReload();
      }
      else if (this._timeChoice == "sim" && Math.Abs(this._acPos.curr.absTime - this._acPos.last.absTime) > 300.0)
      {
        this.displayText("Large simulation time change detected");
        this.WxReload();
      }
      else
      {
        if (!this._bRouteChanged)
        {
          if (GeoClass.Dist(this._acPos.curr.lat, this._acPos.curr.lon, this._acPos.lastInjAll.lat, this._acPos.lastInjAll.lon) > 166.0)
          {
            dateTime1 = DateTime.UtcNow;
            if (dateTime1.Subtract(this._acPos.lastInjOvcTime).TotalSeconds > 90.0 && this._acPos.curr.alt > 14000.0 && (this._acHdgStatic > 4 && this._mouseStatic > 2))
              goto label_30;
          }
          if (this.checkBoxReinjectOvc.Checked && GeoClass.Dist(this._acPos.curr.lat, this._acPos.curr.lon, this._acPos.lastInjOvc.lat, this._acPos.lastInjOvc.lon) > 35.0)
          {
            dateTime1 = DateTime.UtcNow;
            if (dateTime1.Subtract(this._acPos.lastInjOvcTime).TotalSeconds > 100.0 && this._acPos.curr.altGnd > 8000.0 && (this._acHdgStatic > 4 && this._mouseStatic > 2))
            {
              this.displayText(this._wxInjObj.WxInject(this.simconnect, this._wxGridObj.wxGrid, this._acPos.curr, "ovcdist", 100.0, 10, 0U, this._bFirstInject));
              this._acPos.lastInjOvc = this._acPos.curr;
              this._acPos.lastInjOvcTime = DateTime.UtcNow;
              goto label_49;
            }
          }
          if (this.checkBoxReinjectOvc.Checked)
          {
            dateTime1 = DateTime.UtcNow;
            if (dateTime1.Subtract(this._acPos.lastInjOvcTime).TotalMinutes > 12.0 && this._acPos.curr.altGnd > 2000.0 && (this._acHdgStatic > 4 && this._mouseStatic > 2))
            {
              this.displayText(this._wxInjObj.WxInject(this.simconnect, this._wxGridObj.wxGrid, this._acPos.curr, "ovctime", 100.0, 10, 0U, this._bFirstInject));
              this._acPos.lastInjOvc = this._acPos.curr;
              this._acPos.lastInjOvcTime = DateTime.UtcNow;
              goto label_49;
            }
          }
          if (this.checkBoxReinjectOvc.Checked)
          {
            dateTime1 = DateTime.UtcNow;
            if (dateTime1.Subtract(this._acPos.lastInjOvcTime).TotalMinutes > 8.0 && !this.WxOnGroundInjectTimer.Enabled && (this._acPos.curr.spdGnd <= 1.0 && this._mouseStatic > 2))
            {
              this.WxOnGroundInjectTimer.Start();
              goto label_49;
            }
            else
              goto label_49;
          }
          else
            goto label_49;
        }
label_30:
        if (this.textBoxAdep.Text.Trim().Length == 4 || this.textBoxAdep.Text.Trim().Length == 0)
          this._wxGridObj.adep = this.textBoxAdep.Text.Trim();
        if (this.textBoxAdes.Text.Trim().Length == 4 || this.textBoxAdes.Text.Trim().Length == 0)
          this._wxGridObj.ades = this.textBoxAdes.Text.Trim();
        if (this.textBoxAltn.Text.Trim().Length == 4 || this.textBoxAltn.Text.Trim().Length == 0)
          this._wxGridObj.altn = this.textBoxAltn.Text.Trim();
        if (this._wxGridObj.FetchWxData(this._acPos.curr.lat, this._acPos.curr.lon, 1600.0))
        {
          this.displayText(this._wxGridObj.WxGridCreate(this._acPos.curr.lat, this._acPos.curr.lon, 400.0, false, this._simChoice));
          this.displayText(this._wxInjObj.WxInject(this.simconnect, this._wxGridObj.wxGrid, this._acPos.curr, "all", 400.0, 99, 0U, this._bFirstInject));
          this._acPos.lastInjAll = this._acPos.curr;
          this._acPos.lastInjOvc = this._acPos.curr;
        }
        else
          this.displayText("WX DATA download failed");
        this._acPos.lastInjAllTime = DateTime.UtcNow;
        this._acPos.lastInjOvcTime = DateTime.UtcNow;
        this._bRouteChanged = false;
      }
label_49:
      this._acPos.last = this._acPos.curr;
    }

    public void displayText(string s)
    {
      s = s.Trim();
      if (!(s != ""))
        return;
      string text = this.richResponseLog.Text;
      int num = 7;
      string[] strArray = s.Split(new char[2]{ ' ', '|' });
      string str1 = text + DateTime.UtcNow.ToString("HH:mm") + ": ";
      foreach (string str2 in strArray)
      {
        num += str2.Length + 1;
        if (num > 102)
        {
          str1 = str1 + "\n       " + str2 + " ";
          num = 7 + str2.Length + 1;
        }
        else
          str1 = str1 + str2 + " ";
        this.richResponseLog.Text = str1 + "\n";
      }
      this.richResponseLog.SelectionStart = this.richResponseLog.Text.Length;
      this.richResponseLog.ScrollToCaret();
    }

    private void buttonCheck_Click(object sender, EventArgs e)
    {
      WxGridClass.wxStruct wxStruct = new WxGridClass.wxStruct();
      this.displayText("////////////// METAR //////////////");
      if (this._wxGridObj.wxAll.Exists((Predicate<WxGridClass.wxStruct>) (i => i.station_id == this.textBoxCheck.Text)))
        this.displayText(this._wxGridObj.wxAll.Find((Predicate<WxGridClass.wxStruct>) (i => i.station_id == this.textBoxCheck.Text)).raw_text);
      else
        this.displayText(this.textBoxCheck.Text.ToString() + " not found");
    }

    private void buttonNearestMetar_Click(object sender, EventArgs e)
    {
      List<WxGridClass.wxStruct> wxStructList1 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList2 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList3 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList4 = new List<WxGridClass.wxStruct>();
      List<WxGridClass.wxStruct> wxStructList5 = this._wxGridObj.FetchWxDataNearest(this._wxGridObj.wxMetar, this._acPos.curr.lat, this._acPos.curr.lon, 6);
      this._wxGridObj.FetchWxDataNearest(this._wxGridObj.wxGrib, this._acPos.curr.lat, this._acPos.curr.lon, 3);
      this._wxGridObj.FetchWxDataNearest(this._wxGridObj.wxNew, this._acPos.curr.lat, this._acPos.curr.lon, 3);
      List<WxGridClass.wxStruct> wxStructList6 = this._wxGridObj.FetchWxDataNearest(this._wxGridObj.wxGrid, this._acPos.curr.lat, this._acPos.curr.lon, 1);
      try
      {
        this.displayText("////////////// Nearest METAR //////////////");
        string str1 = wxStructList5[0].winds_aloft_string == "" ? "calc aloft" : wxStructList5[0].winds_aloft_source_type + " " + wxStructList5[0].winds_aloft_source;
        this.displayText(wxStructList5[0].raw_text + " + " + str1);
        string str2 = wxStructList5[1].winds_aloft_string == "" ? "calc aloft" : wxStructList5[1].winds_aloft_source_type + " " + wxStructList5[1].winds_aloft_source;
        this.displayText(wxStructList5[1].raw_text + " + " + str2);
        string str3 = wxStructList5[2].winds_aloft_string == "" ? "calc aloft" : wxStructList5[2].winds_aloft_source_type + " " + wxStructList5[2].winds_aloft_source;
        this.displayText(wxStructList5[2].raw_text + " + " + str3);
        string str4 = wxStructList5[3].winds_aloft_string == "" ? "calc aloft" : wxStructList5[3].winds_aloft_source_type + " " + wxStructList5[3].winds_aloft_source;
        this.displayText(wxStructList5[3].raw_text + " + " + str4);
        string str5 = wxStructList5[4].winds_aloft_string == "" ? "calc aloft" : wxStructList5[4].winds_aloft_source_type + " " + wxStructList5[4].winds_aloft_source;
        this.displayText(wxStructList5[4].raw_text + " + " + str5);
        string str6 = wxStructList5[5].winds_aloft_string == "" ? "calc aloft" : wxStructList5[5].winds_aloft_source_type + " " + wxStructList5[5].winds_aloft_source;
        this.displayText(wxStructList5[5].raw_text + " + " + str6);
      }
      catch
      {
        this.displayText("No METAR data");
      }
      try
      {
        this.displayText("////////////// Nearest FSX weather //////////////");
        this.displayText(wxStructList6[0].station_id + ": Interpolation: " + wxStructList6[0].interpolated_from.ToString());
        this.displayText(wxStructList6[0].station_id + ": QNH inHg: " + wxStructList6[0].altim_in_hg.ToString());
        this.displayText(wxStructList6[0].station_id + ": Temp: " + wxStructList6[0].winds_aloft_ext[0][1].ToString());
        this.displayText(wxStructList6[0].station_id + ": Wind: " + wxStructList6[0].winds_aloft_ext[0][2].ToString() + "/" + wxStructList6[0].winds_aloft_ext[0][3].ToString() + " - Variance: " + wxStructList6[0].wind_variance.ToString() + " - Gusts: " + wxStructList6[0].wind_gusts.ToString());
      }
      catch
      {
        this.displayText("No GRID data");
      }
    }

    private void buttonWxReload_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Do you really want to reload the weather?", "Reload", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      this.WxReload();
    }

    private void WxReload()
    {
      this.SetResetWxGridObj();
      this.SetResetWxInjObj();
      this.displayText("Weather Reload");
      this.simconnectReconnect();
      this._bFirstInject = true;
      this._bRouteChanged = false;
    }

    private void SetResetWxGridObj()
    {
      this._wxGridObj = (WxGridClass) null;
      this._wxGridObj = new WxGridClass();
      this._wxGridObj.truewindsaloft = this.checkBoxTrueWindsAloft.Checked;
      this._wxGridObj.fsxinternalwindturb = this.checkBoxFsxInternalWindTurb.Checked;
      this._wxGridObj.fsxinternalcloudturb = true;
      this._wxGridObj.ttgeneral = (string) null;
      this._wxGridObj.ttgeneralthreshold = (float) Convert.ToDouble(this.textBoxAsCuThreshold.Text);
      this._wxGridObj.datetime = this.dateTimePicker.Value;
    }

    private void SetResetWxInjObj()
    {
      this._wxInjObj = (WxInjClass) null;
      this._wxInjObj = new WxInjClass();
    }

    private void SetResetWxDwcObj()
    {
      this._wxDwcObj = (WxDwcClass) null;
      this._wxDwcObj = new WxDwcClass();
      this._wxDwcObj.DwcClose();
    }

    private void textBoxTT_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Convert.ToDouble(this.textBoxAsCuThreshold.Text);
      }
      catch
      {
        int num = (int) MessageBox.Show("Invalid pressure entered");
        this.textBoxAsCuThreshold.Text = "29.90";
      }
      this._regKeys.SetValue("AsCuThreshold", (object) this.textBoxAsCuThreshold.Text);
    }

    private void textBoxCheck_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return || this.simconnect == null)
        return;
      this.buttonCheck_Click(sender, (EventArgs) e);
    }

    private void textBoxAdep_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return || this.textBoxAdep.Text.Trim().Length != 4 && this.textBoxAdep.Text.Trim().Length != 0)
        return;
      if (this.simconnect == null)
      {
        this.buttonConnect_Click(sender, (EventArgs) e);
      }
      else
      {
        this._bRouteChanged = true;
        this.displayText("Route changes applied");
      }
    }

    private void textBoxAdes_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return || this.textBoxAdes.Text.Trim().Length != 4 && this.textBoxAdes.Text.Trim().Length != 0)
        return;
      if (this.simconnect == null)
      {
        this.buttonConnect_Click(sender, (EventArgs) e);
      }
      else
      {
        this._bRouteChanged = true;
        this.displayText("Route changes applied");
      }
    }

    private void textBoxAltn_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return || this.textBoxAltn.Text.Trim().Length != 4 && this.textBoxAltn.Text.Trim().Length != 0)
        return;
      if (this.simconnect == null)
      {
        this.buttonConnect_Click(sender, (EventArgs) e);
      }
      else
      {
        this._bRouteChanged = true;
        this.displayText("Route changes applied");
      }
    }

    private void textBoxAdep_Enter(object sender, EventArgs e)
    {
      this.textBoxAdep.SelectAll();
    }

    private void textBoxAdes_Enter(object sender, EventArgs e)
    {
      this.textBoxAdes.SelectAll();
    }

    private void textBoxAltn_Enter(object sender, EventArgs e)
    {
      this.textBoxAltn.SelectAll();
    }

    private void textBoxCheck_Enter(object sender, EventArgs e)
    {
      this.textBoxCheck.SelectAll();
    }

    private void comboBoxDwcGustsSpd_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._wxDwcObj.dwcGustsSpdScale = Convert.ToDouble(this.comboBoxDwcGustsSpd.Text);
      this._regKeys.SetValue("DwcGustsSpd", (object) this.comboBoxDwcGustsSpd.SelectedIndex);
    }

    private void comboBoxDwcGustsDir_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._wxDwcObj.dwcGustsDirScale = Convert.ToDouble(this.comboBoxDwcGustsDir.Text);
      this._regKeys.SetValue("DwcGustsDir", (object) this.comboBoxDwcGustsDir.SelectedIndex);
    }

    private void comboBoxDwcTurb_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._wxDwcObj.dwcTurbScale = Convert.ToDouble(this.comboBoxDwcTurb.Text);
      this._regKeys.SetValue("DwcTurb", (object) this.comboBoxDwcTurb.SelectedIndex);
    }

    private void comboBoxDwcVariance_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._wxDwcObj.dwcVarianceScale = Convert.ToDouble(this.comboBoxDwcVariance.Text);
      this._regKeys.SetValue("DwcVariance", (object) this.comboBoxDwcVariance.SelectedIndex);
    }

    private void comboBoxDwcRandom_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._wxDwcObj.dwcRandomScale = Convert.ToDouble(this.comboBoxDwcRandom.Text);
      this._regKeys.SetValue("DwcRandom", (object) this.comboBoxDwcRandom.SelectedIndex);
    }

    private void turbTest_Click(object sender, EventArgs e)
    {
      this._wxInjObj.WxTestTurb(this.simconnect, this.comboBoxTurbTest.Text);
    }

    private void buttonInject_Click(object sender, EventArgs e)
    {
      this.simconnect.WeatherSetObservation(0U, this.textBoxWxString.Text);
    }

    private void buttonWxRequest_Click(object sender, EventArgs e)
    {
      this.simconnect.WeatherRequestObservationAtNearestStation((Enum) Form1.DATA_REQUESTS.WEATHERATNEARESTSTATION, (float) this._acPos.curr.lat, (float) this._acPos.curr.lon);
    }

    private void textBoxEzdok_TextChanged(object sender, EventArgs e)
    {
      this._regKeys.SetValue("EzdokGlobalEnable", (object) this.textBoxEzdok.Text);
      File.WriteAllText(Path.GetTempPath() + "/ezdokHotkey.txt", (string) this._regKeys.GetValue("EzdokGlobalEnable"));
    }

    private void checkBoxAutoConnect_CheckedChanged(object sender, EventArgs e)
    {
      this._regKeys.SetValue("AutoConnect", (object) this.checkBoxAutoConnect.Checked);
      if (!this.checkBoxAutoConnect.Checked)
        this.AutoConnectTimer.Stop();
      else
        this.AutoConnectTimer.Start();
    }

    private void checkBoxReinjectOvc_CheckedChanged(object sender, EventArgs e)
    {
      this._regKeys.SetValue("ReinjectOvcConditions", (object) this.checkBoxReinjectOvc.Checked);
    }

    private void checkBoxTrueWindsAloft_CheckedChanged(object sender, EventArgs e)
    {
      this._regKeys.SetValue("TrueWindsAloft", (object) this.checkBoxTrueWindsAloft.Checked);
    }

    private void checkBoxFsxInternalWindTurb_CheckedChanged(object sender, EventArgs e)
    {
      this._regKeys.SetValue("FsxInternalWindTurb", (object) this.checkBoxFsxInternalWindTurb.Checked);
    }

    private void radioButtonTimeSim_Click(object sender, EventArgs e)
    {
      this._timeChoice = "sim";
      this._regKeys.SetValue("TimeChoice", (object) "sim");
      this.dateTimePicker.Enabled = false;
    }

    private void radioButtonTimeCurrent_Click(object sender, EventArgs e)
    {
      this._timeChoice = "current";
      this._regKeys.SetValue("TimeChoice", (object) "current");
      this.dateTimePicker.Enabled = false;
      this.dateTimePicker.MaxDate = DateTime.UtcNow.AddMinutes(1.0);
      this.dateTimePicker.Value = DateTime.UtcNow;
    }

    private void radioButtonTimeHistoric_Click(object sender, EventArgs e)
    {
      this._timeChoice = "historic";
      this._regKeys.SetValue("TimeChoice", (object) "historic");
      this.dateTimePicker.Enabled = true;
      this.dateTimePicker.MaxDate = DateTime.UtcNow.AddMinutes(1.0);
      this.dateTimePicker.Value = DateTime.Parse(this.dateTimePicker.Value.ToString("yyyy.MM.dd HH:00:00"));
    }

    private void radioButtonSimFsx_Click(object sender, EventArgs e)
    {
      this._simChoice = "FSX";
      this._regKeys.SetValue("SimChoice", (object) "FSX");
    }

    private void radioButtonSimP3d_Click(object sender, EventArgs e)
    {
      this._simChoice = "P3D";
      this._regKeys.SetValue("SimChoice", (object) "P3D");
    }

    private void radioButtonSimP3dVolFog_CheckedChanged(object sender, EventArgs e)
    {
      this._simChoice = "P3DVF";
      this._regKeys.SetValue("SimChoice", (object) "P3DVF");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.textBoxAdep = new TextBox();
      this.textBoxAdes = new TextBox();
      this.richResponseLog = new RichTextBox();
      this.buttonDisconnect = new Button();
      this.buttonConnect = new Button();
      this.buttonWxReload = new Button();
      this.textBoxAltn = new TextBox();
      this.dateTimePicker = new DateTimePicker();
      this.groupBox1 = new GroupBox();
      this.radioButtonTimeSim = new RadioButton();
      this.radioButtonTimeHistoric = new RadioButton();
      this.radioButtonTimeCurrent = new RadioButton();
      this.groupBox2 = new GroupBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.groupBox3 = new GroupBox();
      this.groupBox4 = new GroupBox();
      this.checkBoxAutoConnect = new CheckBox();
      this.label10 = new Label();
      this.textBoxEzdok = new TextBox();
      this.checkBoxFsxInternalWindTurb = new CheckBox();
      this.label4 = new Label();
      this.textBoxAsCuThreshold = new TextBox();
      this.checkBoxTrueWindsAloft = new CheckBox();
      this.checkBoxReinjectOvc = new CheckBox();
      this.groupBox5 = new GroupBox();
      this.groupBox6 = new GroupBox();
      this.buttonWxRequest = new Button();
      this.buttonInject = new Button();
      this.textBoxWxString = new TextBox();
      this.comboBoxTurbTest = new ComboBox();
      this.buttonTurbTest = new Button();
      this.groupBox7 = new GroupBox();
      this.label9 = new Label();
      this.comboBoxDwcGustsDir = new ComboBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.comboBoxDwcRandom = new ComboBox();
      this.comboBoxDwcVariance = new ComboBox();
      this.label6 = new Label();
      this.comboBoxDwcTurb = new ComboBox();
      this.label5 = new Label();
      this.comboBoxDwcGustsSpd = new ComboBox();
      this.groupBox8 = new GroupBox();
      this.buttonCheck = new Button();
      this.textBoxCheck = new TextBox();
      this.buttonNearestMetar = new Button();
      this.groupBox9 = new GroupBox();
      this.radioButtonSimP3dVolFog = new RadioButton();
      this.radioButtonSimP3d = new RadioButton();
      this.radioButtonSimFsx = new RadioButton();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.groupBox8.SuspendLayout();
      this.groupBox9.SuspendLayout();
      this.SuspendLayout();
      this.textBoxAdep.CharacterCasing = CharacterCasing.Upper;
      this.textBoxAdep.Font = new Font("Segoe UI", 12f);
      this.textBoxAdep.Location = new Point(7, 40);
      this.textBoxAdep.Margin = new Padding(3, 4, 3, 4);
      this.textBoxAdep.MaxLength = 4;
      this.textBoxAdep.Name = "textBoxAdep";
      this.textBoxAdep.Size = new Size(76, 29);
      this.textBoxAdep.TabIndex = 301;
      this.textBoxAdep.TextAlign = HorizontalAlignment.Center;
      this.textBoxAdep.Enter += new EventHandler(this.textBoxAdep_Enter);
      this.textBoxAdep.KeyUp += new KeyEventHandler(this.textBoxAdep_KeyUp);
      this.textBoxAdes.CharacterCasing = CharacterCasing.Upper;
      this.textBoxAdes.Font = new Font("Segoe UI", 12f);
      this.textBoxAdes.Location = new Point(85, 40);
      this.textBoxAdes.Margin = new Padding(3, 4, 3, 4);
      this.textBoxAdes.MaxLength = 4;
      this.textBoxAdes.Name = "textBoxAdes";
      this.textBoxAdes.Size = new Size(77, 29);
      this.textBoxAdes.TabIndex = 302;
      this.textBoxAdes.TextAlign = HorizontalAlignment.Center;
      this.textBoxAdes.Enter += new EventHandler(this.textBoxAdes_Enter);
      this.textBoxAdes.KeyUp += new KeyEventHandler(this.textBoxAdes_KeyUp);
      this.richResponseLog.BackColor = SystemColors.ControlLight;
      this.richResponseLog.BorderStyle = BorderStyle.None;
      this.richResponseLog.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.richResponseLog.Location = new Point(6, 25);
      this.richResponseLog.Margin = new Padding(3, 4, 3, 4);
      this.richResponseLog.Name = "richResponseLog";
      this.richResponseLog.ReadOnly = true;
      this.richResponseLog.Size = new Size(756, 659);
      this.richResponseLog.TabIndex = 501;
      this.richResponseLog.TabStop = false;
      this.richResponseLog.Text = "";
      this.richResponseLog.WordWrap = false;
      this.buttonDisconnect.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonDisconnect.FlatStyle = FlatStyle.Flat;
      this.buttonDisconnect.Location = new Point(82, 25);
      this.buttonDisconnect.Margin = new Padding(3, 4, 3, 4);
      this.buttonDisconnect.Name = "buttonDisconnect";
      this.buttonDisconnect.Size = new Size(82, 30);
      this.buttonDisconnect.TabIndex = 102;
      this.buttonDisconnect.Text = "Disconnect";
      this.buttonDisconnect.UseVisualStyleBackColor = true;
      this.buttonDisconnect.Click += new EventHandler(this.buttonDisconnect_Click);
      this.buttonConnect.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonConnect.FlatStyle = FlatStyle.Flat;
      this.buttonConnect.Location = new Point(6, 25);
      this.buttonConnect.Margin = new Padding(3, 4, 3, 4);
      this.buttonConnect.Name = "buttonConnect";
      this.buttonConnect.Size = new Size(74, 30);
      this.buttonConnect.TabIndex = 101;
      this.buttonConnect.Text = "Connect";
      this.buttonConnect.UseVisualStyleBackColor = true;
      this.buttonConnect.Click += new EventHandler(this.buttonConnect_Click);
      this.buttonWxReload.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonWxReload.FlatStyle = FlatStyle.Flat;
      this.buttonWxReload.Location = new Point(166, 25);
      this.buttonWxReload.Margin = new Padding(3, 4, 3, 4);
      this.buttonWxReload.Name = "buttonWxReload";
      this.buttonWxReload.Size = new Size(74, 30);
      this.buttonWxReload.TabIndex = 103;
      this.buttonWxReload.Text = "Reload";
      this.buttonWxReload.UseVisualStyleBackColor = true;
      this.buttonWxReload.Click += new EventHandler(this.buttonWxReload_Click);
      this.textBoxAltn.CharacterCasing = CharacterCasing.Upper;
      this.textBoxAltn.Font = new Font("Segoe UI", 12f);
      this.textBoxAltn.Location = new Point(164, 40);
      this.textBoxAltn.Margin = new Padding(3, 4, 3, 4);
      this.textBoxAltn.MaxLength = 4;
      this.textBoxAltn.Name = "textBoxAltn";
      this.textBoxAltn.Size = new Size(76, 29);
      this.textBoxAltn.TabIndex = 303;
      this.textBoxAltn.TextAlign = HorizontalAlignment.Center;
      this.textBoxAltn.Enter += new EventHandler(this.textBoxAltn_Enter);
      this.textBoxAltn.KeyUp += new KeyEventHandler(this.textBoxAltn_KeyUp);
      this.dateTimePicker.CustomFormat = " dd.MM.yyyy // HH UTC";
      this.dateTimePicker.Enabled = false;
      this.dateTimePicker.Format = DateTimePickerFormat.Custom;
      this.dateTimePicker.Location = new Point(6, 53);
      this.dateTimePicker.Margin = new Padding(3, 4, 3, 4);
      this.dateTimePicker.MinDate = new DateTime(2012, 11, 24, 0, 0, 0, 0);
      this.dateTimePicker.Name = "dateTimePicker";
      this.dateTimePicker.Size = new Size(174, 25);
      this.dateTimePicker.TabIndex = 410;
      this.dateTimePicker.Value = new DateTime(2012, 11, 24, 0, 0, 0, 0);
      this.groupBox1.Controls.Add((Control) this.radioButtonTimeSim);
      this.groupBox1.Controls.Add((Control) this.radioButtonTimeHistoric);
      this.groupBox1.Controls.Add((Control) this.radioButtonTimeCurrent);
      this.groupBox1.Controls.Add((Control) this.dateTimePicker);
      this.groupBox1.Location = new Point(12, 243);
      this.groupBox1.Margin = new Padding(3, 4, 3, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(3, 4, 3, 4);
      this.groupBox1.Size = new Size(250, 93);
      this.groupBox1.TabIndex = 4000;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Time";
      this.radioButtonTimeSim.AutoSize = true;
      this.radioButtonTimeSim.Checked = true;
      this.radioButtonTimeSim.Location = new Point(10, 25);
      this.radioButtonTimeSim.Name = "radioButtonTimeSim";
      this.radioButtonTimeSim.Size = new Size(47, 21);
      this.radioButtonTimeSim.TabIndex = 401;
      this.radioButtonTimeSim.TabStop = true;
      this.radioButtonTimeSim.Text = "Sim";
      this.radioButtonTimeSim.UseVisualStyleBackColor = true;
      this.radioButtonTimeSim.Click += new EventHandler(this.radioButtonTimeSim_Click);
      this.radioButtonTimeHistoric.AutoSize = true;
      this.radioButtonTimeHistoric.Location = new Point(138, 25);
      this.radioButtonTimeHistoric.Name = "radioButtonTimeHistoric";
      this.radioButtonTimeHistoric.Size = new Size(70, 21);
      this.radioButtonTimeHistoric.TabIndex = 403;
      this.radioButtonTimeHistoric.Text = "Historic";
      this.radioButtonTimeHistoric.UseVisualStyleBackColor = true;
      this.radioButtonTimeHistoric.Click += new EventHandler(this.radioButtonTimeHistoric_Click);
      this.radioButtonTimeCurrent.AutoSize = true;
      this.radioButtonTimeCurrent.Location = new Point(63, 25);
      this.radioButtonTimeCurrent.Name = "radioButtonTimeCurrent";
      this.radioButtonTimeCurrent.Size = new Size(69, 21);
      this.radioButtonTimeCurrent.TabIndex = 402;
      this.radioButtonTimeCurrent.Text = "Current";
      this.radioButtonTimeCurrent.UseVisualStyleBackColor = true;
      this.radioButtonTimeCurrent.Click += new EventHandler(this.radioButtonTimeCurrent_Click);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Controls.Add((Control) this.textBoxAdep);
      this.groupBox2.Controls.Add((Control) this.textBoxAltn);
      this.groupBox2.Controls.Add((Control) this.textBoxAdes);
      this.groupBox2.Location = new Point(12, 154);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(250, 82);
      this.groupBox2.TabIndex = 3000;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Route";
      this.label3.AutoSize = true;
      this.label3.ForeColor = SystemColors.ButtonShadow;
      this.label3.Location = new Point(169, 21);
      this.label3.Name = "label3";
      this.label3.Size = new Size(38, 17);
      this.label3.TabIndex = 210;
      this.label3.Text = "ALTN";
      this.label2.AutoSize = true;
      this.label2.ForeColor = SystemColors.ButtonShadow;
      this.label2.Location = new Point(89, 21);
      this.label2.Name = "label2";
      this.label2.Size = new Size(39, 17);
      this.label2.TabIndex = 209;
      this.label2.Text = "ADES";
      this.label1.AutoSize = true;
      this.label1.ForeColor = SystemColors.ButtonShadow;
      this.label1.Location = new Point(11, 21);
      this.label1.Name = "label1";
      this.label1.Size = new Size(39, 17);
      this.label1.TabIndex = 208;
      this.label1.Text = "ADEP";
      this.groupBox3.Controls.Add((Control) this.buttonConnect);
      this.groupBox3.Controls.Add((Control) this.buttonDisconnect);
      this.groupBox3.Controls.Add((Control) this.buttonWxReload);
      this.groupBox3.Location = new Point(12, 12);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(250, 65);
      this.groupBox3.TabIndex = 1000;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "System";
      this.groupBox4.Controls.Add((Control) this.checkBoxAutoConnect);
      this.groupBox4.Controls.Add((Control) this.label10);
      this.groupBox4.Controls.Add((Control) this.textBoxEzdok);
      this.groupBox4.Controls.Add((Control) this.checkBoxFsxInternalWindTurb);
      this.groupBox4.Controls.Add((Control) this.label4);
      this.groupBox4.Controls.Add((Control) this.textBoxAsCuThreshold);
      this.groupBox4.Controls.Add((Control) this.checkBoxTrueWindsAloft);
      this.groupBox4.Controls.Add((Control) this.checkBoxReinjectOvc);
      this.groupBox4.Location = new Point(12, 407);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(250, 172);
      this.groupBox4.TabIndex = 6000;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Options";
      this.checkBoxAutoConnect.AutoSize = true;
      this.checkBoxAutoConnect.Location = new Point(9, 25);
      this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
      this.checkBoxAutoConnect.Size = new Size(103, 21);
      this.checkBoxAutoConnect.TabIndex = 601;
      this.checkBoxAutoConnect.Text = "Auto connect";
      this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
      this.checkBoxAutoConnect.CheckedChanged += new EventHandler(this.checkBoxAutoConnect_CheckedChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(99, 138);
      this.label10.Name = "label10";
      this.label10.Size = new Size(128, 17);
      this.label10.TabIndex = 622;
      this.label10.Text = "EZdok global enable";
      this.textBoxEzdok.Location = new Point(7, 135);
      this.textBoxEzdok.Margin = new Padding(3, 4, 3, 4);
      this.textBoxEzdok.MaxLength = 10;
      this.textBoxEzdok.Name = "textBoxEzdok";
      this.textBoxEzdok.Size = new Size(90, 25);
      this.textBoxEzdok.TabIndex = 621;
      this.textBoxEzdok.TextChanged += new EventHandler(this.textBoxEzdok_TextChanged);
      this.checkBoxFsxInternalWindTurb.AutoSize = true;
      this.checkBoxFsxInternalWindTurb.Location = new Point(9, 85);
      this.checkBoxFsxInternalWindTurb.Name = "checkBoxFsxInternalWindTurb";
      this.checkBoxFsxInternalWindTurb.Size = new Size(129, 21);
      this.checkBoxFsxInternalWindTurb.TabIndex = 604;
      this.checkBoxFsxInternalWindTurb.Text = "Internal wind turb";
      this.checkBoxFsxInternalWindTurb.UseVisualStyleBackColor = true;
      this.checkBoxFsxInternalWindTurb.CheckedChanged += new EventHandler(this.checkBoxFsxInternalWindTurb_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(54, 110);
      this.label4.Name = "label4";
      this.label4.Size = new Size(135, 17);
      this.label4.TabIndex = 404;
      this.label4.Text = "AS/CU threshold inHg";
      this.textBoxAsCuThreshold.Location = new Point(7, 107);
      this.textBoxAsCuThreshold.Margin = new Padding(3, 4, 3, 4);
      this.textBoxAsCuThreshold.MaxLength = 5;
      this.textBoxAsCuThreshold.Name = "textBoxAsCuThreshold";
      this.textBoxAsCuThreshold.Size = new Size(45, 25);
      this.textBoxAsCuThreshold.TabIndex = 620;
      this.textBoxAsCuThreshold.TextChanged += new EventHandler(this.textBoxTT_TextChanged);
      this.checkBoxTrueWindsAloft.AutoSize = true;
      this.checkBoxTrueWindsAloft.Location = new Point(9, 65);
      this.checkBoxTrueWindsAloft.Name = "checkBoxTrueWindsAloft";
      this.checkBoxTrueWindsAloft.Size = new Size(119, 21);
      this.checkBoxTrueWindsAloft.TabIndex = 603;
      this.checkBoxTrueWindsAloft.Text = "True winds aloft";
      this.checkBoxTrueWindsAloft.UseVisualStyleBackColor = true;
      this.checkBoxTrueWindsAloft.CheckedChanged += new EventHandler(this.checkBoxTrueWindsAloft_CheckedChanged);
      this.checkBoxReinjectOvc.AutoSize = true;
      this.checkBoxReinjectOvc.Location = new Point(9, 45);
      this.checkBoxReinjectOvc.Name = "checkBoxReinjectOvc";
      this.checkBoxReinjectOvc.Size = new Size(166, 21);
      this.checkBoxReinjectOvc.TabIndex = 602;
      this.checkBoxReinjectOvc.Text = "Reinject OVC conditions";
      this.checkBoxReinjectOvc.UseVisualStyleBackColor = true;
      this.checkBoxReinjectOvc.CheckedChanged += new EventHandler(this.checkBoxReinjectOvc_CheckedChanged);
      this.groupBox5.Controls.Add((Control) this.richResponseLog);
      this.groupBox5.Location = new Point(268, 12);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(768, 691);
      this.groupBox5.TabIndex = 5000;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Log";
      this.groupBox6.Controls.Add((Control) this.buttonWxRequest);
      this.groupBox6.Controls.Add((Control) this.buttonInject);
      this.groupBox6.Controls.Add((Control) this.textBoxWxString);
      this.groupBox6.Controls.Add((Control) this.comboBoxTurbTest);
      this.groupBox6.Controls.Add((Control) this.buttonTurbTest);
      this.groupBox6.Location = new Point(12, 725);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new Size(1024, 64);
      this.groupBox6.TabIndex = 9000;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Test";
      this.buttonWxRequest.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonWxRequest.FlatStyle = FlatStyle.Flat;
      this.buttonWxRequest.Font = new Font("Segoe UI", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.buttonWxRequest.Location = new Point(796, 23);
      this.buttonWxRequest.Margin = new Padding(1, 4, 3, 4);
      this.buttonWxRequest.Name = "buttonWxRequest";
      this.buttonWxRequest.Size = new Size(80, 25);
      this.buttonWxRequest.TabIndex = 903;
      this.buttonWxRequest.TabStop = false;
      this.buttonWxRequest.Text = "WxRequest";
      this.buttonWxRequest.UseVisualStyleBackColor = true;
      this.buttonWxRequest.Click += new EventHandler(this.buttonWxRequest_Click);
      this.buttonInject.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonInject.FlatStyle = FlatStyle.Flat;
      this.buttonInject.Font = new Font("Segoe UI", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.buttonInject.Location = new Point(712, 23);
      this.buttonInject.Margin = new Padding(1, 4, 3, 4);
      this.buttonInject.Name = "buttonInject";
      this.buttonInject.Size = new Size(80, 25);
      this.buttonInject.TabIndex = 902;
      this.buttonInject.TabStop = false;
      this.buttonInject.Text = "WxInject";
      this.buttonInject.UseVisualStyleBackColor = true;
      this.buttonInject.Click += new EventHandler(this.buttonInject_Click);
      this.textBoxWxString.Location = new Point(16, 23);
      this.textBoxWxString.Name = "textBoxWxString";
      this.textBoxWxString.Size = new Size(692, 25);
      this.textBoxWxString.TabIndex = 901;
      this.textBoxWxString.TabStop = false;
      this.comboBoxTurbTest.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxTurbTest.FlatStyle = FlatStyle.Flat;
      this.comboBoxTurbTest.FormattingEnabled = true;
      this.comboBoxTurbTest.Items.AddRange(new object[5]
      {
        (object) "N",
        (object) "L",
        (object) "M",
        (object) "H",
        (object) "S"
      });
      this.comboBoxTurbTest.Location = new Point(890, 23);
      this.comboBoxTurbTest.Name = "comboBoxTurbTest";
      this.comboBoxTurbTest.Size = new Size(35, 25);
      this.comboBoxTurbTest.TabIndex = 904;
      this.comboBoxTurbTest.TabStop = false;
      this.buttonTurbTest.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonTurbTest.FlatStyle = FlatStyle.Flat;
      this.buttonTurbTest.Font = new Font("Segoe UI", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.buttonTurbTest.Location = new Point(931, 23);
      this.buttonTurbTest.Margin = new Padding(3, 4, 3, 4);
      this.buttonTurbTest.Name = "buttonTurbTest";
      this.buttonTurbTest.Size = new Size(80, 25);
      this.buttonTurbTest.TabIndex = 905;
      this.buttonTurbTest.TabStop = false;
      this.buttonTurbTest.Text = "TurbTest";
      this.buttonTurbTest.UseVisualStyleBackColor = true;
      this.buttonTurbTest.Click += new EventHandler(this.turbTest_Click);
      this.groupBox7.Controls.Add((Control) this.label9);
      this.groupBox7.Controls.Add((Control) this.comboBoxDwcGustsDir);
      this.groupBox7.Controls.Add((Control) this.label8);
      this.groupBox7.Controls.Add((Control) this.label7);
      this.groupBox7.Controls.Add((Control) this.comboBoxDwcRandom);
      this.groupBox7.Controls.Add((Control) this.comboBoxDwcVariance);
      this.groupBox7.Controls.Add((Control) this.label6);
      this.groupBox7.Controls.Add((Control) this.comboBoxDwcTurb);
      this.groupBox7.Controls.Add((Control) this.label5);
      this.groupBox7.Controls.Add((Control) this.comboBoxDwcGustsSpd);
      this.groupBox7.Location = new Point(12, 585);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new Size(250, 118);
      this.groupBox7.TabIndex = 7000;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "DWC";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(53, 55);
      this.label9.Name = "label9";
      this.label9.Size = new Size(61, 17);
      this.label9.TabIndex = 612;
      this.label9.Text = "Gusts Dir";
      this.comboBoxDwcGustsDir.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDwcGustsDir.FlatStyle = FlatStyle.Flat;
      this.comboBoxDwcGustsDir.FormattingEnabled = true;
      this.comboBoxDwcGustsDir.ItemHeight = 17;
      this.comboBoxDwcGustsDir.Items.AddRange(new object[5]
      {
        (object) "0.0",
        (object) "0.5",
        (object) "1.0",
        (object) "1.5",
        (object) "2.0"
      });
      this.comboBoxDwcGustsDir.Location = new Point(9, 52);
      this.comboBoxDwcGustsDir.Name = "comboBoxDwcGustsDir";
      this.comboBoxDwcGustsDir.Size = new Size(42, 25);
      this.comboBoxDwcGustsDir.TabIndex = 702;
      this.comboBoxDwcGustsDir.SelectedIndexChanged += new EventHandler(this.comboBoxDwcGustsDir_SelectedIndexChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(168, 55);
      this.label8.Name = "label8";
      this.label8.Size = new Size(57, 17);
      this.label8.TabIndex = 610;
      this.label8.Text = "Random";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(54, 82);
      this.label7.Name = "label7";
      this.label7.Size = new Size(57, 17);
      this.label7.TabIndex = 609;
      this.label7.Text = "Variance";
      this.comboBoxDwcRandom.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDwcRandom.FlatStyle = FlatStyle.Flat;
      this.comboBoxDwcRandom.FormattingEnabled = true;
      this.comboBoxDwcRandom.ItemHeight = 17;
      this.comboBoxDwcRandom.Items.AddRange(new object[5]
      {
        (object) "0.0",
        (object) "0.5",
        (object) "1.0",
        (object) "1.5",
        (object) "2.0"
      });
      this.comboBoxDwcRandom.Location = new Point(124, 52);
      this.comboBoxDwcRandom.Name = "comboBoxDwcRandom";
      this.comboBoxDwcRandom.Size = new Size(42, 25);
      this.comboBoxDwcRandom.TabIndex = 705;
      this.comboBoxDwcRandom.SelectedIndexChanged += new EventHandler(this.comboBoxDwcRandom_SelectedIndexChanged);
      this.comboBoxDwcVariance.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDwcVariance.FlatStyle = FlatStyle.Flat;
      this.comboBoxDwcVariance.FormattingEnabled = true;
      this.comboBoxDwcVariance.ItemHeight = 17;
      this.comboBoxDwcVariance.Items.AddRange(new object[5]
      {
        (object) "0.0",
        (object) "0.5",
        (object) "1.0",
        (object) "1.5",
        (object) "2.0"
      });
      this.comboBoxDwcVariance.Location = new Point(9, 79);
      this.comboBoxDwcVariance.Name = "comboBoxDwcVariance";
      this.comboBoxDwcVariance.Size = new Size(42, 25);
      this.comboBoxDwcVariance.TabIndex = 703;
      this.comboBoxDwcVariance.SelectedIndexChanged += new EventHandler(this.comboBoxDwcVariance_SelectedIndexChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(168, 28);
      this.label6.Name = "label6";
      this.label6.Size = new Size(62, 17);
      this.label6.TabIndex = 606;
      this.label6.Text = "Turb Spd";
      this.comboBoxDwcTurb.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDwcTurb.FlatStyle = FlatStyle.Flat;
      this.comboBoxDwcTurb.FormattingEnabled = true;
      this.comboBoxDwcTurb.Items.AddRange(new object[5]
      {
        (object) "0.0",
        (object) "0.5",
        (object) "1.0",
        (object) "1.5",
        (object) "2.0"
      });
      this.comboBoxDwcTurb.Location = new Point(124, 25);
      this.comboBoxDwcTurb.Name = "comboBoxDwcTurb";
      this.comboBoxDwcTurb.Size = new Size(42, 25);
      this.comboBoxDwcTurb.TabIndex = 704;
      this.comboBoxDwcTurb.SelectedIndexChanged += new EventHandler(this.comboBoxDwcTurb_SelectedIndexChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(53, 28);
      this.label5.Name = "label5";
      this.label5.Size = new Size(67, 17);
      this.label5.TabIndex = 604;
      this.label5.Text = "Gusts Spd";
      this.comboBoxDwcGustsSpd.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDwcGustsSpd.FlatStyle = FlatStyle.Flat;
      this.comboBoxDwcGustsSpd.FormattingEnabled = true;
      this.comboBoxDwcGustsSpd.ItemHeight = 17;
      this.comboBoxDwcGustsSpd.Items.AddRange(new object[5]
      {
        (object) "0.0",
        (object) "0.5",
        (object) "1.0",
        (object) "1.5",
        (object) "2.0"
      });
      this.comboBoxDwcGustsSpd.Location = new Point(9, 25);
      this.comboBoxDwcGustsSpd.Name = "comboBoxDwcGustsSpd";
      this.comboBoxDwcGustsSpd.Size = new Size(42, 25);
      this.comboBoxDwcGustsSpd.TabIndex = 701;
      this.comboBoxDwcGustsSpd.SelectedIndexChanged += new EventHandler(this.comboBoxDwcGustsSpd_SelectedIndexChanged);
      this.groupBox8.Controls.Add((Control) this.buttonCheck);
      this.groupBox8.Controls.Add((Control) this.textBoxCheck);
      this.groupBox8.Controls.Add((Control) this.buttonNearestMetar);
      this.groupBox8.Location = new Point(12, 83);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new Size(250, 65);
      this.groupBox8.TabIndex = 2000;
      this.groupBox8.TabStop = false;
      this.groupBox8.Text = "Information";
      this.buttonCheck.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonCheck.FlatStyle = FlatStyle.Flat;
      this.buttonCheck.Location = new Point(86, 25);
      this.buttonCheck.Margin = new Padding(3, 4, 3, 4);
      this.buttonCheck.Name = "buttonCheck";
      this.buttonCheck.Size = new Size(62, 30);
      this.buttonCheck.TabIndex = 202;
      this.buttonCheck.Text = "Check";
      this.buttonCheck.UseVisualStyleBackColor = true;
      this.buttonCheck.Click += new EventHandler(this.buttonCheck_Click);
      this.textBoxCheck.CharacterCasing = CharacterCasing.Upper;
      this.textBoxCheck.Font = new Font("Segoe UI", 12f);
      this.textBoxCheck.Location = new Point(7, 26);
      this.textBoxCheck.Margin = new Padding(3, 4, 3, 4);
      this.textBoxCheck.MaxLength = 4;
      this.textBoxCheck.Name = "textBoxCheck";
      this.textBoxCheck.Size = new Size(76, 29);
      this.textBoxCheck.TabIndex = 201;
      this.textBoxCheck.TextAlign = HorizontalAlignment.Center;
      this.textBoxCheck.Enter += new EventHandler(this.textBoxCheck_Enter);
      this.textBoxCheck.KeyDown += new KeyEventHandler(this.textBoxCheck_KeyDown);
      this.buttonNearestMetar.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
      this.buttonNearestMetar.FlatStyle = FlatStyle.Flat;
      this.buttonNearestMetar.Location = new Point(150, 25);
      this.buttonNearestMetar.Margin = new Padding(3, 4, 3, 4);
      this.buttonNearestMetar.Name = "buttonNearestMetar";
      this.buttonNearestMetar.Size = new Size(90, 30);
      this.buttonNearestMetar.TabIndex = 203;
      this.buttonNearestMetar.Text = "Nearest Wx";
      this.buttonNearestMetar.UseVisualStyleBackColor = true;
      this.buttonNearestMetar.Click += new EventHandler(this.buttonNearestMetar_Click);
      this.groupBox9.Controls.Add((Control) this.radioButtonSimP3dVolFog);
      this.groupBox9.Controls.Add((Control) this.radioButtonSimP3d);
      this.groupBox9.Controls.Add((Control) this.radioButtonSimFsx);
      this.groupBox9.Location = new Point(12, 343);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new Size(250, 58);
      this.groupBox9.TabIndex = 5000;
      this.groupBox9.TabStop = false;
      this.groupBox9.Text = "Simulator";
      this.radioButtonSimP3dVolFog.AutoSize = true;
      this.radioButtonSimP3dVolFog.Location = new Point(118, 25);
      this.radioButtonSimP3dVolFog.Name = "radioButtonSimP3dVolFog";
      this.radioButtonSimP3dVolFog.Size = new Size(97, 21);
      this.radioButtonSimP3dVolFog.TabIndex = 503;
      this.radioButtonSimP3dVolFog.Text = "P3D vol. fog";
      this.radioButtonSimP3dVolFog.UseVisualStyleBackColor = true;
      this.radioButtonSimP3dVolFog.CheckedChanged += new EventHandler(this.radioButtonSimP3dVolFog_CheckedChanged);
      this.radioButtonSimP3d.AutoSize = true;
      this.radioButtonSimP3d.Location = new Point(65, 25);
      this.radioButtonSimP3d.Name = "radioButtonSimP3d";
      this.radioButtonSimP3d.Size = new Size(49, 21);
      this.radioButtonSimP3d.TabIndex = 502;
      this.radioButtonSimP3d.Text = "P3D";
      this.radioButtonSimP3d.UseVisualStyleBackColor = true;
      this.radioButtonSimP3d.Click += new EventHandler(this.radioButtonSimP3d_Click);
      this.radioButtonSimFsx.AutoSize = true;
      this.radioButtonSimFsx.Checked = true;
      this.radioButtonSimFsx.Location = new Point(10, 25);
      this.radioButtonSimFsx.Name = "radioButtonSimFsx";
      this.radioButtonSimFsx.Size = new Size(47, 21);
      this.radioButtonSimFsx.TabIndex = 501;
      this.radioButtonSimFsx.TabStop = true;
      this.radioButtonSimFsx.Text = "FSX";
      this.radioButtonSimFsx.UseVisualStyleBackColor = true;
      this.radioButtonSimFsx.Click += new EventHandler(this.radioButtonSimFsx_Click);
      this.AutoScaleDimensions = new SizeF(7f, 17f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1052, 716);
      this.Controls.Add((Control) this.groupBox9);
      this.Controls.Add((Control) this.groupBox8);
      this.Controls.Add((Control) this.groupBox7);
      this.Controls.Add((Control) this.groupBox6);
      this.Controls.Add((Control) this.groupBox5);
      this.Controls.Add((Control) this.groupBox4);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(3, 4, 3, 4);
      this.MaximizeBox = false;
      this.Name = nameof (Form1);
      this.Text = "FSXWX 1.6.1";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.groupBox8.ResumeLayout(false);
      this.groupBox8.PerformLayout();
      this.groupBox9.ResumeLayout(false);
      this.groupBox9.PerformLayout();
      this.ResumeLayout(false);
    }

    public enum DEFINITIONS
    {
      dataPosStruct,
    }

    public enum DATA_REQUESTS
    {
      POSITION = 0,
      WEATHERATSTATION = 1,
      WEATHERATNEARESTSTATION = 2,
      WEATHERSTATIONNEW = 2097152, // 0x00200000
      WEATHERSTATIONREMOVE = 2097152, // 0x00200000
    }

    public enum EVENT_CTRL
    {
      PAUSE_ON,
      PAUSE_OFF,
    }

    public enum GROUP_IDS
    {
      GROUP_1,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct dataPosStruct
    {
      public double lat;
      public double lon;
      public double alt;
      public double altGnd;
      public double hdg;
      public double spdIas;
      public double spdGnd;
      public double absTime;
    }

    public struct dataPos
    {
      public Form1.dataPosStruct curr;
      public Form1.dataPosStruct last;
      public Form1.dataPosStruct lastInjAll;
      public DateTime lastInjAllTime;
      public Form1.dataPosStruct lastInjOvc;
      public DateTime lastInjOvcTime;
      public string lastMousePos;
    }
  }
}
