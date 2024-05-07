using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCastLibrary;
using NAudio.CoreAudioApi;
using System.Linq.Expressions;
using System.Diagnostics;

namespace CineCast
{
    public class IcecastMp3AudioOutput : IAudioOutput
    {
        private readonly WaveFormat format;
        private readonly MemoryStream dataStream;
        private readonly LameMP3FileWriter mp3Converter;
        private Libshout? icecast = null;
        private readonly byte[] buffer;

        public string Name { get => $""; }
        public IcecastMp3AudioOutput(WaveFormat format)
        {
            this.format = format;
            buffer = new byte[256*1024];
            dataStream = new MemoryStream(buffer);
            mp3Converter = new LameMP3FileWriter(dataStream, format, LAMEPreset.STANDARD);
            dataStream.SetLength(0);
        }

        public void Initialize(string host, int port, string mount, string user, string password, string? title = null, string? genre = null)
        {
            Close();
            icecast = new Libshout();
            icecast.setProtocol(Libshout.PROTOCOL_HTTP);
            icecast.setHost(host);
            icecast.setPort(port);
            icecast.setMount(mount);
            icecast.setUser(user);
            icecast.setPassword(password);
            icecast.setFormat(Libshout.FORMAT_MP3);
            if(!String.IsNullOrWhiteSpace(genre)) icecast.setGenre(genre);
            if(!String.IsNullOrWhiteSpace(title)) icecast.setInfo("title", title);
            icecast.open();
        }
        public void Close()
        {
            icecast?.close();
            icecast = null;
        }
        public void Write(byte[] buffer, int offset, int length)
        {
            try
            {
                mp3Converter.Write(buffer, offset, length);
                if (dataStream.Length > 0)
                {
                    icecast?.send(this.buffer, (int)dataStream.Length);
                    dataStream.SetLength(0);
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public void Dispose()
        {
            mp3Converter.Dispose();
            dataStream.Dispose();
        }
        ~IcecastMp3AudioOutput()
        {
            icecast?.close();
            mp3Converter.Close();
            mp3Converter.Dispose();
            dataStream.Dispose();
        }
    }
}
