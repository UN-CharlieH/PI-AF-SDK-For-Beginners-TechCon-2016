using System;
using External;

namespace Ex1_Connection_And_Hierarchy_Basics_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFPrinter afPrinter = new AFPrinter(Constants.AFSERVERNAME, "Magical Power Company");
            afPrinter.PrintRootElements();
            afPrinter.PrintElementTemplates();
            afPrinter.PrintAttributeTemplates("MeterAdvanced");
            afPrinter.PrintEnergyUOMs();
            afPrinter.PrintEnumerationSets();
            afPrinter.PrintCategories();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
