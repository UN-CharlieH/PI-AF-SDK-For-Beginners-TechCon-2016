using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External;
using OSIsoft.AF.Search;

namespace Ex3_Searching_For_Assets_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFAssetSearcher searcher = new AFAssetSearcher(Constants.AFSERVERNAME, "Wizardry Power Company");
            searcher.FindMetersByName("Meter00*");
            searcher.FindMetersByTemplate("MeterAdvanced");
            searcher.FindMetersBySubstation("Edinburgh");
            searcher.FindMetersByUsage(AFSearchOperator.LessThan, 300);
            searcher.FindBuildingInfo("MeterAdvanced");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
