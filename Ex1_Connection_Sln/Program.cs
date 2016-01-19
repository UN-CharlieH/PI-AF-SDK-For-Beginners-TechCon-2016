using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using External;

namespace Ex1_Connection_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFDatabase db = AFConnection.ImplicitConnect(Constants.AFSERVERNAME, "databaseName");
            // AFDatabase db = AFConnection.ExplicitConnect("serverName", "databaseName"); 

            if (db != null) Console.WriteLine("Connected to database: {0}", db.Name);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
