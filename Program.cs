using HtmlAgilityPack;
using RestSharp;
using System;
using System.IO;
using System.Net;

namespace TextToSpeech
{
    class Program
    {
        private static string url = @"https://freetts.com";
        private static string voice = "Penelope_Female";
        private static string id = "Penelope";
        private static string type = "1";
        private static string language = "Language";

        private static string pathOut = @"C:\test\audio.mp3";

        static void Main(string[] args)
        {
            DownloadMp3(TextToSpeech("Hola, esto es una prueba de texto a audio."));
        }

        public static string TextToSpeech(string message)
        {
            
            var client = new RestClient(url);
            var request = new RestRequest("Home/PlayAudio", Method.GET);

            request.AddQueryParameter("Language", language);
            request.AddQueryParameter("Voice", voice);
            request.AddQueryParameter("TextMessage", message);
            request.AddQueryParameter("id", id);
            request.AddQueryParameter("type", type);

            var response = client.Execute(request);

            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);
            var pathMp3 = doc.GetElementbyId("audio_").ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["src"].Value;

            return url + pathMp3;
        }

        public static void DownloadMp3(string url)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(new Uri(url));
            File.WriteAllBytes(pathOut, data);
        }
    }
}
