using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MainApp.Pages
{
    public partial class APIPage : Page
    {
        public APIPage() => InitializeComponent();

        private static HttpClient? _client;
        private const string ApiUrl = "http://localhost:5000";

        private void Load(object sender, RoutedEventArgs e)
        {
            var input = BoxCountOfEntriesToLoad.Text;
            if (!int.TryParse(input, out var countOfEntriesToLoad) && countOfEntriesToLoad <= 0)
            {
                ErrorBlock.Text = "Count of Entries to load must be integer and greater that 0";
                return;
            }

            _client ??= ConfigureClient();

            Thread loader = new Thread(new ParameterizedThreadStart(LoadEntries));
            loader.Start(new ThreadDTO() { Client = _client, CountOfEntriesToLoad = countOfEntriesToLoad });
        }

        private void LoadEntries(object? arg)
        {
            if (arg is not ThreadDTO threadDTO) return;
            try
            {

                var response = threadDTO.Client.Send(new HttpRequestMessage(HttpMethod.Get, $"/entries/{threadDTO.CountOfEntriesToLoad}"));
                if (!response.IsSuccessStatusCode) return;

                var stream = response.Content.ReadAsStream();
                using var readStream = new StreamReader(stream);

                // TODO: there must be cut request on chunks !!!!!

                var json = readStream.ReadToEnd();

                var responses = JsonSerializer.Deserialize<IEnumerable<ApiResponse>>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (var dto in responses)
                    {
                        DataView.Items.Add(dto);
                    }
                }));
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ErrorBlock.Text = ex.Message;
                }));
            }
        }

        private HttpClient ConfigureClient()
        {
            var httpHandler = new SocketsHttpHandler { MaxConnectionsPerServer = 2 };
            var client = new HttpClient(httpHandler, true)
            {
                BaseAddress = new Uri(ApiUrl)
            };
            return client;
        }

        private class ThreadDTO
        {
            public HttpClient Client { get; init; }
            public int CountOfEntriesToLoad { get; init; }
        }
        private record ApiResponse(int Id, string Word, int Value);
    }
}
