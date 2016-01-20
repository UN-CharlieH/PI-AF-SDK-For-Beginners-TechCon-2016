using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Search;

namespace Ex3_Searching_For_Assets
{
    public class AFAssetSearcher
    {
        AFDatabase _database;
        // Define other instance members here

        public AFAssetSearcher(string server, string database)
        {
            // Your code here
        }

        public void FindMetersByName(string elementNameFilter)
        {
            // Your code here
        }

        public void FindMetersByTemplate(string templateName)
        {
            // Your code here
        }

        public void FindMetersBySubstation(string substationLocation)
        {
            // Your code here
        }

        public void FindMetersByUsage(AFSearchOperator op, double val)
        {
            // Your code here
        }

        public void FindBuildingInfo(string templateName)
        {
            // Your code here
        }

    }
}
