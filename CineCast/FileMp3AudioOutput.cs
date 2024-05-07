using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using static System.Windows.Forms.DataFormats;

namespace CineCast
{
    internal class FileMp3AudioOutput : IAudioOutput
    {
        private readonly WaveFormat format;
        private readonly string filePath;
        private readonly FileStream fileStream;
        private readonly LameMP3FileWriter mp3Converter;
        private readonly ID3TagData info = new ID3TagData();

        public string Name { get => $"mp3://{filePath}"; }
        public FileMp3AudioOutput(WaveFormat format, string filePrefix, string? title = null, string? genre = null)
        {
            info.Genre = genre;
            info.Title = title;
            this.format = format;
            filePath = $"{filePrefix}_{DateTime.UtcNow.Ticks}.mp3";
            fileStream = File.Create(filePath);
            filePath = Path.GetFullPath(filePath);
            mp3Converter = new LameMP3FileWriter(fileStream, format, LAMEPreset.EXTREME_FAST, info);
        }
        public void Write(byte[] buffer, int offset, int length)
        {
            mp3Converter.Write(buffer, offset, length);
            fileStream.Flush(true);
        }
        public void Stop()
        {
            mp3Converter.Flush();
            mp3Converter.Close();
            fileStream.Flush();
            fileStream.Close();
        }

        public void Dispose()
        {
            mp3Converter.Dispose();
            fileStream.Dispose();
        }
    }
}
