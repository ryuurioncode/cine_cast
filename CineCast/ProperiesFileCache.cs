using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace CineCast
{
    public class AppProperties
    {
        public TrackInfo trackInfo { get; set; } = new TrackInfo();
        public OutputMixerProperties outputMixerProperties { get; set; } = new OutputMixerProperties();
        public IcecastProperties icecastProperties { get; set; } = new IcecastProperties();
        public Mp3FileOutputProperties mp3FileOutputProperties { get; set; } = new Mp3FileOutputProperties();
        public Dictionary<string, InputProperties> inputProperties { get; set; } = new Dictionary<string, InputProperties>();
    }
    public class ProperiesFileCache
    {
        public string fileName { get; set; }
        public AppProperties properties { get; set; }
        public ProperiesFileCache(string fileName)
        {
            this.fileName = fileName;
            try
            {
                var data = File.ReadAllText(fileName);
                properties = JsonSerializer.Deserialize<AppProperties>(data) ?? new AppProperties();
            } catch {
                properties = new AppProperties();
            }
        }
        public void Save()
        {
            try
            {
                var data = JsonSerializer.Serialize(properties);
                File.WriteAllText(fileName, data);
            } catch { }
        }
    }
}
