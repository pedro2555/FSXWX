// Decompiled with JetBrains decompiler
// Type: FSXWX.LogClass
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System;
using System.IO;

namespace FSXWX
{
  internal static class LogClass
  {
    public static void LogWrite(string file, string newEntry)
    {
      File.AppendAllText(Path.GetTempPath() + file, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + ": " + newEntry + Environment.NewLine);
    }

    public static void DeleteLogs()
    {
      foreach (string file in Directory.GetFiles(Path.GetTempPath(), "FSXWXlog*.txt"))
        File.Delete(file);
    }
  }
}
