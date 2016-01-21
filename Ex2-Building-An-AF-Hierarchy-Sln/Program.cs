using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2_Building_An_AF_Hierarchy_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFHierarchyBuilder builder = new AFHierarchyBuilder();
            builder.CreateDatabase();
            builder.CreateTemplates();
            builder.CreateElements();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
