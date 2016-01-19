using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;

namespace Ex1_Connection_Sln
{
    public static class AFConnection
    {
        public static AFDatabase ImplicitConnect(string afServerName, string afDatabaseName)
        {
            PISystems piSystems = new PISystems();
            PISystem piSystem = piSystems[afServerName];
            AFDatabase afDatabase = piSystem.Databases[afDatabaseName]; // connection is made implicity here
            return afDatabase;
        }

        public static AFDatabase ExplicitConnect(string afServerName, string afDatabaseName)
        {
            PISystems piSystems = new PISystems();
            PISystem piSystem = piSystems[afServerName];
            piSystem.Connect(); // connection is made explicitly
            AFDatabase afDatabase = piSystem.Databases[afDatabaseName];
            return afDatabase;
        }
    }
}
