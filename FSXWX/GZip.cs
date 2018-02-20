// Decompiled with JetBrains decompiler
// Type: FSXWX.GZip
// Assembly: FSXWX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 350F6F96-0E70-487C-B582-1157C6BB993A
// Assembly location: C:\Program Files (x86)\Lockheed Martin\Prepar3D v3\FSXWX.exe

using System.IO;
using System.IO.Compression;

namespace FSXWX
{
  public static class GZip
  {
    public static void DecompressFile(string compressedFilePath, string uncompressedFilePath)
    {
      using (FileStream fileStream1 = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read))
      {
        using (GZipStream gzipStream = new GZipStream((Stream) fileStream1, CompressionMode.Decompress))
        {
          using (FileStream fileStream2 = new FileStream(uncompressedFilePath, FileMode.Create, FileAccess.Write))
          {
            byte[] buffer = new byte[4096];
            int count;
            while ((count = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
              fileStream2.Write(buffer, 0, count);
          }
        }
      }
    }

    public static void CompressFile(string uncompressedFilePath, string compressedFilePath)
    {
      using (FileStream fileStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write))
      {
        using (GZipStream gzipStream = new GZipStream((Stream) fileStream, CompressionMode.Compress))
        {
          byte[] buffer = File.ReadAllBytes(uncompressedFilePath);
          gzipStream.Write(buffer, 0, buffer.Length);
        }
      }
    }
  }
}
