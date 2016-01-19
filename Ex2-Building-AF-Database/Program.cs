using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2_Building_AF_Database
{
    class Program
    {
        static void Main(string[] args)
        {
            AFHierarchyBuilder builder = new AFHierarchyBuilder();
            builder.Build();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
