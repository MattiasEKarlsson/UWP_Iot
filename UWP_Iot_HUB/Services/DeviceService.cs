using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP_Iot_HUB.Models;
using static UWP_Iot_HUB.Models.TemperatureApiModel;

namespace UWP_Iot_HUB.Services
{
    public static class DeviceService
    {
       
        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
               var httpClient = HttpClientFactory.Create();
               TemperatureModel senddata = new TemperatureModel();

                try
                {
                    var url = "https://api.openweathermap.org/data/2.5/onecall?lat=59.27412&units=metric&lon=15.2066&exclude=hourly,daily,minutely&appid=5bf919005c4c20e778ba98f74c7f2e33";
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var content = httpResponseMessage.Content;                                    //Hämtar Api
                        var datatemp = await content.ReadAsAsync<Rootobject>();

                        double temp = datatemp.current.temp;                                                // Sätter Temp och Humidity
                        int humidity = datatemp.current.humidity;

                        senddata = new TemperatureModel                                              //Lägger Temp och Humidity i samma objekt.
                        {
                            Temperature = temp,
                            Humidity = humidity
                        };

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }




                try
                {
                    var json = JsonConvert.SerializeObject(senddata);                                        // Converterar till Json format.

                    var payload = new Message(Encoding.UTF8.GetBytes(json));                             // Packeterar meddelandena

                    await deviceClient.SendEventAsync(payload);                                         // Skickar

                    Console.WriteLine($"Message Sent: {json}");
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                }

                

            

        }

        public static async Task ReceiveMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                var payload = await deviceClient.ReceiveAsync();

                if (payload == null)
                {
                    continue;
                }
                string newmessage = Encoding.UTF8.GetString(payload.GetBytes());
                Console.WriteLine($"Message recived: {newmessage}");
                await deviceClient.CompleteAsync(payload);
            }



        }

        // public static async Task SendMessageToDeviceAsync(MAD.ServiceClient serviceClient, string targetDeviceId, string message)
        // {
        // var payload = new MAD.Message(Encoding.UTF8.GetBytes(message));
        // await serviceClient.SendAsync(targetDeviceId, payload);


    }
    // }
}

