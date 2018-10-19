using System;
using System.Diagnostics;
namespace xvideos_downloader
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Buscando videos...");
            xvideossuperscraper super = new xvideossuperscraper();
        var ax= super.getvideos(5);

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Seleccione el video que desea");
            Console.WriteLine("---------------------------------------------------------");
            var select=int.Parse( Console.ReadLine());
            Console.Clear();
            Console.WriteLine("seleccione la calidad");
            Console.WriteLine("0-baja");
            Console.WriteLine("1-alta");
            var qualidad = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("extrayendo video....");
            var link = super.getdownloadlink(ax.videos[select].link, qualidad);
           
            Process.Start(new ProcessStartInfo("cmd", $"/c start {link.Replace("&","^&")}"));

        }
    }
}
