﻿using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using PNGCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace CreateIMG
{
    internal class Program
    {
        // height = 100
        private static readonly byte[] _imageBuffer = new byte[4 * 256 * 100];

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //for (int y = 0; y < 100; y++)
            //{
            //    for (int x = 0; x < 256; x++)
            //    {
            //        byte val = (byte)x;
            //        PlotPixel(x, y, val, val, val);
            //    }
            //}

            // read png bytes
            //byte[] buffer = File.ReadAllBytes(@"C:\Users\brianding\Desktop\1.png");
            //var list = GetChunks(buffer);

            //byte[] data = new byte[] { 24, 87, 99, 252, 255, 255, 63, 3, 3, 3, 19, 16, 51, 48, 48, 0, 0, 36, 6, 3, 1 };
            //byte[] data = new byte[] { 24, 87, 99, 248, 255, 255, 63, 0, 5, 254, 2, 254 };
            //var s = ZLibDecompress(data);
            //byte[] buffer = new byte[s.Length];
            //s.Read(buffer, 0, buffer.Length);

            // before compress
            //byte[] data = new byte[] { 0, 237, 28, 36 };
            //Compress(data);

            //// after compress
            //byte[] data = new byte[] { 24, 87, 99, 120, 43, 163, 2, 0, 3, 39, 1, 46 };
            byte[] data = new byte[] { 120, 156, 99, 120, 43, 163, 2, 0, 3, 39, 1, 46 };
            Depress(data);

            //ZLibDecompress(new byte[] { 120, 156, 99, 120, 43, 163, 2, 0, 3, 39, 1, 46 });
            ZLibDecompress(data);
        }

        private static Stream ZLibDecompress(byte[] data)
        {
            var outputStream = new MemoryStream();
            using (var compressedStream = new MemoryStream(data))
            using (var inputStream = new InflaterInputStream(compressedStream))
            {
                inputStream.CopyTo(outputStream);
                outputStream.Position = 0;
                return outputStream;
            }
        }

        private static List<string> GetChunks(byte[] buffer)
        {
            List<string> names = new List<string>();
            int chunkIndex = 8;
            while (chunkIndex < buffer.Length)
            {
                char[] chars = new char[] { (char)buffer[chunkIndex + 4], (char)buffer[chunkIndex + 5], (char)buffer[chunkIndex + 6], (char)buffer[chunkIndex + 7] };
                string name = new string(chars);
                names.Add(name);
                int length = (buffer[chunkIndex] << 24) + (buffer[chunkIndex + 1] << 16) + (buffer[chunkIndex + 2] << 8) + (buffer[chunkIndex + 3] << 0);

                Console.WriteLine(name);
                for (int i = 0; i < length + 4 + 4 + 4; i++)
                {
                    Console.Write(buffer[chunkIndex + i].ToString() + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                chunkIndex += 4 + 4 + 4 + length;
            }

            return names;
        }

        private static void PlotPixel(int x, int y, byte redValue, byte greenValue, byte blueValue)
        {
            int offset = ((256 * 4) * y) + (x * 4);
            // windows use 'b,g,r' instead of 'r,g,b'
            _imageBuffer[offset] = blueValue;
            _imageBuffer[offset + 1] = greenValue;
            _imageBuffer[offset + 2] = redValue;
            // alpha
            _imageBuffer[offset + 3] = 255;
        }

        private static void Compress(byte[] buffer)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                using (var compressor =
                       new Ionic.Zlib.ZlibStream(ms,
                                                  CompressionMode.Compress,
                                                  CompressionLevel.BestSpeed))
                {
                    compressor.Write(buffer, 0, buffer.Length);
                }

                byte[] data = ms.ToArray();
            }
        }

        private static void Depress(byte[] buffer)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                using (var compressor =
                       new Ionic.Zlib.ZlibStream(ms,
                                                  CompressionMode.Decompress))
                {
                    compressor.Write(buffer, 0, buffer.Length);
                }

                byte[] data = ms.ToArray();
            }
        }
    }
}