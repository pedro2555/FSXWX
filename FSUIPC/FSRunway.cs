// Decompiled with JetBrains decompiler
// Type: FSUIPC.FSRunway
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;

namespace FSUIPC
{
  [Serializable]
  public struct FSRunway
  {
    public byte Number;
    public FSRunwayDesignator Designator;

    public override string ToString()
    {
      string str = "";
      if ((int) this.Number > 0)
      {
        switch (this.Number)
        {
          case 37:
            str = "N";
            break;
          case 38:
            str = "NE";
            break;
          case 39:
            str = "E";
            break;
          case 40:
            str = "SE";
            break;
          case 41:
            str = "S";
            break;
          case 42:
            str = "SW";
            break;
          case 43:
            str = "W";
            break;
          case 44:
            str = "NW";
            break;
          default:
            str = this.Number.ToString();
            break;
        }
        if (str.Length == 1)
          str = "0" + str;
      }
      if (this.Designator > FSRunwayDesignator.none)
      {
        switch (this.Designator)
        {
          case FSRunwayDesignator.left:
            str += "L";
            break;
          case FSRunwayDesignator.right:
            str += "R";
            break;
          case FSRunwayDesignator.centre:
            str += "C";
            break;
          case FSRunwayDesignator.water:
            str += "W";
            break;
        }
      }
      return str;
    }
  }
}
