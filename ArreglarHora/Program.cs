using IniParser;
using IniParser.Model;
using System;
using static ArreglarHora.Libs;

namespace ArreglarHora
{
    public class Program
    {
        
        
        static void Main(string[] args)
        {
            if (IsAdministrator())
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("config.ini");

                string gmtStrValue = data["CONFIG"]["gmt"];

                int gmtValue = 0;

                bool check = int.TryParse(gmtStrValue, out gmtValue);

                if (check)
                {
                    Horario h = new Horario(gmtValue);
                    
                    try
                    {
                        h.obtenerDataAsync().Wait();
                        Console.WriteLine("Fecha cambiada");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ha ocurrido un error, verifique su conexion a internet");
                        Console.WriteLine(ex.Message);
                    }
                       

                    
                }
                else
                {
                    Console.WriteLine("Error en leer .INI");
                }

                


                
            }
            else
            {
                Console.WriteLine("Ejecutelo como administrador");
            }

            

            Console.ReadLine();
        }

        


    }

}
