using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex2_Searching_For_Assets
{
    public class AFAssetSearcher
    {
        AFDatabase _database;

        // Define other instance members here

        public AFAssetSearcher(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
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

        public void FindMetersAboveLimit(double val)
        {
            // Your code here
        }

        public void FindBuildingInfo(string templateName)
        {
            // Your code here
        }

    }
}
