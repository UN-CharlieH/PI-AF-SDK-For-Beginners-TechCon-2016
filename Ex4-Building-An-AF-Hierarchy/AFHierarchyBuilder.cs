using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using External;

namespace Ex2_Building_AF_Database
{
    public class AFHierarchyBuilder
    {
        private PISystem _piSystem;
        private AFDatabase _database;

        // Define other instance members here

        public AFHierarchyBuilder(string server)
        {
            _piSystem = new PISystems()[server];
        }

        public void CreateDatabase()
        {
            // Your code here
        }

        public void CreateCategories()
        {
            // Your code here
        }

        public void CreateEnumerationSets()
        {
            // Your code here
        }

        public void CreateTemplates()
        {
            // Your code here
        }

        public void CreateElements()
        {
            // Your code here
        }

        public void SetAttributeValues()
        {
            // Your code here
        }

        public void CreateDistrictElements()
        {
            // Your code here
        }

        public void CreateWeakReferences()
        {
            // Your code here
        }
    }
}
