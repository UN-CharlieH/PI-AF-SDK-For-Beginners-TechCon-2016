using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;
using OSIsoft.AF.PI;

namespace Ex3_Reading_And_Writing_Data
{
    public class AssetDataProvider
    {
        AFDatabase _database;

        // Define other instance members here

        public AssetDataProvider(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
        }

        public void PrintHistorical(string meterName, string startTime, string endTime)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", _database);

            // Your code here
        }

        public void PrintInterpolated(string meterName, string startTime, string endTime, TimeSpan timeSpan)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", _database);

            // Your code here
        }

        public void PrintHourlyAverage(string meterName, string startTime, string endTime)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", _database);

            // Your code here
        }

        public void PrintEnergyUsageAtTime(string timeStamp)
        {
            AFAttributeList attrList = new AFAttributeList();

            // Use this method if you get stuck trying to find attributes
            // attrList = GetAttribute();

            // Your code here
        }

        public void PrintDailyAverageEnergyUsage(string startTime, string endTime)
        {
            AFAttributeList attrList = new AFAttributeList();

            // Use this method if you get stuck trying to find attributes
            // attrList = GetAttribute();

            // Your code here
        }

        public void SwapValues(string meter1, string meter2, string startTime, string endTime)
        {
            // Your code here
        }

        private AFAttributeList GetAttributes()
        {
            int startIndex = 0;
            int pageSize = 1000;
            int totalCount;

            AFAttributeList attrList = new AFAttributeList();

            do
            {
                AFAttributeList results = AFAttribute.FindElementAttributes(
                     database: _database,
                     searchRoot: null,
                     nameFilter: null,
                     elemCategory: null,
                     elemTemplate: _database.ElementTemplates["MeterBasic"],
                     elemType: AFElementType.Any,
                     attrNameFilter: "Energy Usage",
                     attrCategory: null,
                     attrType: TypeCode.Empty,
                     searchFullHierarchy: true,
                     sortField: AFSortField.Name,
                     sortOrder: AFSortOrder.Ascending,
                     startIndex: startIndex,
                     maxCount: pageSize,
                     totalCount: out totalCount);

                attrList.AddRange(results);

                startIndex += pageSize;
            } while (startIndex < totalCount);

            return attrList;
        }
    }
}
