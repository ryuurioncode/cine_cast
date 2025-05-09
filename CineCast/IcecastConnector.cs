using IceCastLibrary;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CineCast
{
    public record IcecastConnectorProperties
    {
        public int Port { get; set; } = default!;
        public string Host { get; set; } = default!;
        public string Mount { get; set; } = default!;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string StreamTitle { get; set; } = default!;
    }

    // https://gist.github.com/ePirat/adc3b8ba00d85b7e3870
    public class IcecastConnector
    {
        private readonly IcecastConnectorProperties properties;
        private readonly UriBuilder uriBuilder;
        private readonly string authString;
        private TcpClient? icecastClient = null;
        private NetworkStream? icecastStream = null;
        public IcecastConnector(IcecastConnectorProperties properties)
        {
            this.properties = properties;
            this.uriBuilder = new UriBuilder();
            uriBuilder.Host = properties.Host;
            uriBuilder.Port = properties.Port;
            uriBuilder.Path = properties.Mount;
            this.authString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{properties.User}:{properties.Password}"));
        }

        // TODO: implement later, now does no process some symbols
        // https://icecast.org/docs/icecast-2.3.1/admin-interface.html
        // https://www.opennet.ru/base/sys/radio_protocols.txt.html
        private async Task SetTitleSync()
        {
            // Calling an async method inside a synchronous method (Main)
            await Task.Run(() => SetTitle());
        }

        private async Task SetTitle()
        {
            var queryDictionary = new Dictionary<string, string> {
                { "mode", HttpUtility.UrlEncode("updinfo") },
                { "charset", HttpUtility.UrlEncode("utf-8") },
                { "mount", HttpUtility.UrlEncode(properties.Mount.StartsWith("/") ? properties.Mount : $"/{properties.Mount}") },
                //{ "song", EncodingFixer.UrlEncode(properties.StreamTitle) },
                { "song", HttpUtility.UrlEncode(properties.StreamTitle) },
            };
            var url = "/admin/metadata?" + string.Join("&", queryDictionary.Select(kvp =>
                $"{kvp.Key}={kvp.Value}"));
            var uriBuilderTitle = new UriBuilder();
            uriBuilderTitle.Host= properties.Host;
            uriBuilderTitle.Port = properties.Port;
            using (var title_client = new HttpClient())
            {
                title_client.BaseAddress = uriBuilderTitle.Uri;
                title_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
                var response = await title_client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Не вышло выставить тайтл! {response.StatusCode} -> {response.Content}");
            }
        }
        public void Connect()
        {
            //if (properties.StreamTitle.Length > 33)
            //    throw new Exception($"Название стрима должны состоять из 33 символов и меньше (сейчас их {properties.StreamTitle.Length})");
            string headers = $"PUT /{properties.Mount} HTTP/1.1\r\n" +
                             $"Host: {properties.Host}:{properties.Port}\r\n" +
                             "User-Agent: CineCast\r\n" +
                             $"Authorization: Basic {authString}\r\n" +
                             // TODO: 33 chars max for title, need to use GET method instead
                             //$"ice-audio-info: title={EncodingFixer.UrlEncode(properties.StreamTitle)}\r\n" + 
                             "Content-Type: audio/mpeg\r\n" +
                             "Transfer-Encoding: chunked\r\n" +
                             "ice-public: 1\r\n" +
                             "ice-name: CineCast\r\n" +
                             "ice-description: CineCast\r\n" +
                             "ice-url: CineCast\r\n" +
                             "ice-genre: live\r\n\r\n";
            byte[] headerBytes = Encoding.ASCII.GetBytes(headers);
            icecastClient = new TcpClient(properties.Host, properties.Port);
            icecastStream = icecastClient?.GetStream();
            icecastStream?.Write(headerBytes, 0, headerBytes.Length);

            Task.Run(async () => await SetTitle()).Wait();
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            try
            {
                if (icecastStream != null)
                    icecastStream?.Write(buffer, offset, length);
                else
                    throw new Exception("icecastStream is null!");
            }
            catch(Exception)
            {
                Close();
                Connect();
            }
        }
        public void Close()
        {
            icecastStream?.Close();
            icecastStream = null;
            icecastClient?.Close();
            icecastClient = null;
        }
        ~IcecastConnector() {
            Close();
        }
    }
}
