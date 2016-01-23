using System;
using External;

namespace Ex2_Searching_For_Assets
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment the lines below in order to test
            // AFAssetSearcher searcher = new AFAssetSearcher(Constants.AFSERVERNAME, "Magical Power Company");
            // searcher.FindMetersByName("Meter00*");
            // searcher.FindMetersByTemplate("MeterBasic");
            // searcher.FindMetersBySubstation("Edinburgh");
            // searcher.FindMetersAboveUsage(300);
            // searcher.FindBuildingInfo("MeterAdvanced");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
