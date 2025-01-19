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
using System.Text.Unicode;
using NVorbis.Contracts;

namespace CineCast
{
    public class IcecastMp3AudioOutput : IAudioOutput
    {
        private readonly WaveFormat format;
        private readonly MemoryStream dataStream;
        private LameMP3FileWriter? mp3Converter = null;
        //private Libshout? icecast = null;
        private IcecastConnector? connector = null;
        private readonly byte[] buffer;

        public string Name { get => $""; }
        public IcecastMp3AudioOutput(WaveFormat format, string? title = null, string? genre = null)
        {
            this.format = format;
            buffer = new byte[256*1024];
            dataStream = new MemoryStream(buffer);
            dataStream.SetLength(0);
        }

        public void Initialize(string host, int port, string mount, string user, string password, string? title = null, string? genre = null)
        {
            Close();
            var tagData = new ID3TagData() { Title = FixEncoding(title), Track = FixEncoding(title), Genre = genre };
            mp3Converter = new LameMP3FileWriter(dataStream, format, LAMEPreset.EXTREME_FAST, tagData);
            /*
            icecast = new Libshout();
            icecast.setProtocol(Libshout.PROTOCOL_HTTP);
            icecast.setHost(host);
            icecast.setPort(port);
            icecast.setMount(mount);
            icecast.setUser(user);
            icecast.setPassword(password);
            icecast.setFormat(Libshout.FORMAT_MP3);
            icecast.setMeta.setMeta("title", FixEncoding(title));
            if(!String.IsNullOrWhiteSpace(genre)) icecast.setGenre(FixEncoding(genre));
            if(!String.IsNullOrWhiteSpace(title)) icecast.setInfo("title", FixEncoding(title));
            icecast.open();
            */
            connector = new IcecastConnector(new IcecastConnectorProperties
            {
                Port = port,
                Host = host,
                Mount = mount,
                User = user,
                Password = password,
                StreamTitle = title,
            });
            connector.Connect();
        }
        public void Close()
        {
            /*
            icecast?.close();
            icecast = null;
            */
            connector = null;
            mp3Converter?.Close();
            mp3Converter = null;
        }
        public void Write(byte[] buffer, int offset, int length)
        {
            try
            {
                mp3Converter?.Write(buffer, offset, length);
                if (dataStream.Length > 0)
                {
                    //icecast?.send(this.buffer, (int)dataStream.Length);
                    connector?.Write(this.buffer, 0, (int)dataStream.Length);
                    dataStream.SetLength(0);
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private string FixEncoding(string source)
        {
            var encoding = Encoding.GetEncoding(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
            var defaultBytes = Encoding.Default.GetBytes(source);
            var unicodeString = encoding.GetString(defaultBytes);
            var bytes = encoding.GetBytes(unicodeString);
            //абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ
            // абвгдеёжзийклмнопрстуфхцчшщъыьэюя
            // АААААААААААААААААААААААААААААААААА
            // FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
            foreach (var b in bytes)
                Debug.Write(b + ", ");
            Debug.WriteLine("");
            return unicodeString;
        }

        public void Dispose()
        {
            mp3Converter?.Dispose();
            dataStream.Dispose();
        }
        ~IcecastMp3AudioOutput()
        {
            //icecast?.close();
            mp3Converter?.Close();
            mp3Converter?.Dispose();
            dataStream.Dispose();
        }
    }
}
