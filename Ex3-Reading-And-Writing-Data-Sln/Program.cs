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
            dataProvider.PrintHistorical("Meter001", "*-30s", "*");
            dataProvider.PrintInterpolated("Meter001", "*-30s", "*", TimeSpan.FromSeconds(10));
            dataProvider.PrintHourlyAverage("Meter001", "y", "t");
            dataProvider.PrintEnergyUsageAtTime("t+10h");
            dataProvider.PrintDailyAverageEnergyUsage("t-7d", "t");
            dataProvider.SwapValues("Meter001", "Meter002", "y", "y+1h");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
