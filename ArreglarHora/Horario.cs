using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArreglarHora
{
    public class Horario
    {
        public const string URL = "http://worldtimeapi.org/api/timezone/America/Lima";

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool SetSystemTime(ref SystemDate sysDate);

        public struct SystemDate
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;


            public SystemDate(DateTime dt)
            {
                Year = (ushort)dt.Year;
                Month = (ushort)dt.Month;
                DayOfWeek = (ushort)dt.DayOfWeek;
                Day = (ushort)dt.Day;
                Hour = (ushort)dt.Hour;
                Minute = (ushort)dt.Minute;
                Second = (ushort)dt.Second;
                Millisecond = (ushort)dt.Millisecond;
            }
        };

        protected int gmt;

        public Horario(int gmt=0)
        {
            this.gmt = gmt;
        }

        protected void ChangeValueDateTime(string datetime)
        {
            DateTime date = DateTime.Parse(datetime, CultureInfo.InvariantCulture).AddHours(this.gmt);

            SystemDate systime = new SystemDate(date);

            SetSystemTime(ref systime);
        }
        

        public async Task obtenerDataAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(URL);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if(!String.IsNullOrEmpty(responseBody))
            {
                JObject jsonObject = JObject.Parse(responseBody);

                string datetime = (string)jsonObject["datetime"];


                ChangeValueDateTime(datetime);
            }
            else
            {
                Console.WriteLine("Error al conectarse a Internet");
            }
                
        }

        
    }
}
