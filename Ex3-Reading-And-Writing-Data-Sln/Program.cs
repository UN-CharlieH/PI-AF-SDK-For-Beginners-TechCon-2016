using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External;

namespace Ex3_Reading_And_Writing_Data_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AssetDataProvider dataProvider = new AssetDataProvider(Constants.AFSERVERNAME, "Magical Power Company");
            //dataProvider.PrintHistorical("Meter001", "*-30s", "*");
            //dataProvider.PrintInterpolated("Meter001", "*-30s", "*", TimeSpan.FromSeconds(10));
            //dataProvider.PrintHourlyAverage("Meter001", "y", "t");
            dataProvider.PrintEnergyUsageForMeters("t+10h");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
