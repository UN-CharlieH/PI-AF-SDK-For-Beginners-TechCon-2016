using System;
using External;

namespace Ex4_Building_An_AF_Hierarchy_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFHierarchyBuilder builder = new AFHierarchyBuilder(Constants.AFSERVERNAME);
            builder.CreateDatabase();
            builder.CreateCategories();
            builder.CreateEnumerationSets();
            builder.CreateTemplates();
            builder.CreateDataArchiveElement();
            builder.CreateElements();
            builder.SetAttributeValues();
            builder.CreateDistrictElements();
            builder.CreateWeakReferences();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
