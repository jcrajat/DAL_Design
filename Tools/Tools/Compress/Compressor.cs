using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CM.Tools.Compress
{
	public class Compressor
	{
        public class GZip
        {
            public static byte[] Compress(string nText)
            {
                return Compress(Encoding.UTF8.GetBytes(nText));
            }

            public static byte[] Compress(byte[] buffer)
            {
                var ms = new MemoryStream();
                var zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(buffer, 0, buffer.Length);
                zip.Close();
                ms.Position = 0;
                
                var compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);

                var gzBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                return gzBuffer;
            }

            public static string DecompressToString(byte[] gzBuffer)
            {
                return Encoding.UTF8.GetString(DecompressToByteArray(gzBuffer));
            }

            public static byte[] DecompressToByteArray(byte[] gzBuffer)
            {
                var ms = new MemoryStream();
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                var buffer = new byte[msgLength];

                ms.Position = 0;
                var zip = new GZipStream(ms, CompressionMode.Decompress);
                zip.Read(buffer, 0, buffer.Length);

                return buffer;
            }            
        }
	}
}