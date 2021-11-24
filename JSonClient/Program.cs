using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JSonClient
{
    internal class Program
    {
        /*
Clasx classx

     string json = JsonConvert.SerializeObject(classx, Formatting.Indented);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            var response = await client.PostAsync("http://.....", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            var clas2 = JsonConvert.DeserializeObject<Clasx>(result);
         
         * */
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            do
            {
                var menuSelection = GetMenuSelection();
                switch (menuSelection)
                {
                    case 0:
                        return;
                    case 1:
                        await ProcessWheatherForecasts();
                        break;
                    case 2:
                        await ProcessRepositories();
                        break;
                    case 3:
                        await SendSms();
                        break;
                    case 4:
                        await GetSmsWithGsmNumber();
                        break;
                    case 5:
                        await GetSmsWithTime();
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("Menüye dönmek için bir tuşa basınız...");
                Console.ReadKey();

            } while (true);

        }

        private static void WriteMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("MENÜ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Hava durumu apisi sonuçlarını göster.");
            Console.WriteLine("2. Github Api sonuçlarını göster.");
            Console.WriteLine("3. Sms Gönder");
            Console.WriteLine("4. Telefon No ile Sms sorgula");
            Console.WriteLine("5. Gönderim Zamanı ile Sms sorgula");
            Console.WriteLine("0. Çıkış");
            Console.WriteLine();
            Console.Write("Seçiminiz: ");
        }

        private static int GetMenuSelection()
        {
            WriteMenu();
            int result = 0;
            do
            {
                WriteMenu();
                var userInput = Console.ReadLine();
                try
                {
                    int.TryParse(userInput, out result);
                }
                catch (Exception)
                {
                    result = 0;
                }
            } while (result < 1 && result > 3);

            return result;
        }


        private static async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            //var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
            //var msg = await stringTask;
            //Console.Write(msg);

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            foreach (var repo in repositories)
                Console.WriteLine(repo.name + " - " + repo.description);
        }

        private static async Task ProcessWheatherForecasts()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            var streamTask = client.GetStreamAsync("https://localhost:44386/weatherforecast");
            var repositories = await JsonSerializer.DeserializeAsync<List<WeatherForecastResult>>(await streamTask);
            foreach (var repo in repositories)
                Console.WriteLine($"Tarih: {repo.date} - Sıcaklık: {repo.temperatureC} C° - {repo.temperatureF} F - Özet: {repo.summary}");
        }

        private static async Task SendSms()
        {
            SendSmsDto sendSmsRecord = new SendSmsDto();
            //(string GsmNumber, string Message) sendSmsRecord = (string.Empty, string.Empty);
            Console.Write("Gsm Number: ");
            sendSmsRecord.GsmNumber = Console.ReadLine();
            Console.Write("Message: ");
            sendSmsRecord.Message = Console.ReadLine();

            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var json = System.Text.Json.JsonSerializer.Serialize(sendSmsRecord);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //Mustafa Kemal Atatürk'ün öğretmenler hakkında söylediği "Öğretmenler yeni nesil sizin eseriniz olacaktır." sözü doğrultusunda tüm öğretmenlerimizin öğretmenler gününü kutlarız.

            var response = await client.PostAsync("https://localhost:44386/api/sms/sendsms", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine();
            Console.WriteLine($"SendSms Api Result = {result}");
        }

        private static async Task GetSmsWithGsmNumber()
        {
            SmsQueryDto smsQuery = new SmsQueryDto();
            //(string GsmNumber, string Message) sendSmsRecord = (string.Empty, string.Empty);
            Console.Write("Gsm Number: ");
            smsQuery.GsmNumber = Console.ReadLine();

            var json = System.Text.Json.JsonSerializer.Serialize(smsQuery);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.PostAsync("https://localhost:44386/api/sms/getsms", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine();
            Console.WriteLine($"GetSmsWithGsmNumber Api Result = {result}");
        }

        private static async Task GetSmsWithTime()
        {
            SmsQueryDto smsQueryDto = new SmsQueryDto();
            Console.Write("Başlangıç Zamanı (gg.aa.yyyy HH:mm:ss): ");
            var startTimeStr = Console.ReadLine();

            Console.Write("Bitiş Zamanı (gg.aa.yyyy HH:mm:ss)    : ");
            var endTimeStr = Console.ReadLine();

            smsQueryDto.StartTime = !string.IsNullOrWhiteSpace(startTimeStr) ? Convert.ToDateTime(startTimeStr, new CultureInfo("tr-TR")):(DateTime?)null;
            smsQueryDto.EndTime = !string.IsNullOrWhiteSpace(endTimeStr) ? Convert.ToDateTime(endTimeStr, new CultureInfo("tr-TR")) : (DateTime?)null;

            var json = System.Text.Json.JsonSerializer.Serialize(smsQueryDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //Mustafa Kemal Atatürk'ün öğretmenler hakkında söylediği "Öğretmenler yeni nesil sizin eseriniz olacaktır." sözü doğrultusunda tüm öğretmenlerimizin öğretmenler gününü kutlarız.

            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.PostAsync("https://localhost:44386/api/sms/getsms", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine();
            Console.WriteLine($"GetSmsWithTime Api Result = {result}");
        }

    }
}
