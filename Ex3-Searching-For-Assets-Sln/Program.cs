using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex3_Searching_For_Assets_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFAssetSearcher searcher = new AFAssetSearcher("BSHANGE6430S","Wizardry Power Company");
            //searcher.FindMetersByName("Meter00*");
            //searcher.FindMetersByTemplate("MeterAdvanced");
            //searcher.FindMetersBySubstation("Edinburgh");
            //searcher.FindMetersBelowLimit(8);
            searcher.FindBuildingInfo("MeterBasic");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
