using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;

namespace Ex3_Reading_And_Writing_Data_Sln
{
    public class AssetDataProvider
    {
        AFDatabase _database;

        public AssetDataProvider(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
        }

        public void PrintHistorical(string meterName, string startTime, string endTime)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", _database);

            AFTime start = new AFTime(startTime);
            AFTime end = new AFTime(endTime);
            AFTimeRange timeRange = new AFTimeRange(start, end);
            AFValues vals = attr.Data.RecordedValues(
                timeRange: timeRange, 
                boundaryType: AFBoundaryType.Inside, 
                desiredUOM: _database.PISystem.UOMDatabase.UOMs["kilojoule"],
                filterExpression: null,
                includeFilteredValues: false);

            foreach (AFValue val in vals)
            {
                Console.WriteLine("Timestamp (UTC): {0}, Value (kJ): {1}", val.Timestamp.UtcTime, val.Value);
            }
        }

        public void PrintInterpolated(string meterName, string startTime, string endTime, TimeSpan timeSpan)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", _database);

            AFTime start = new AFTime(startTime);
            AFTime end = new AFTime(endTime);
            AFTimeRange timeRange = new AFTimeRange(start, end);

            AFTimeSpan interval = new AFTimeSpan(timeSpan);

            AFValues vals = attr.Data.InterpolatedValues(
                timeRange: timeRange,
                interval: interval,
                desiredUOM: null,
                filterExpression: null,
                includeFilteredValues: false);

            foreach (AFValue val in vals)
            {
                Console.WriteLine("Timestamp (Local): {0}, Value (kilowatt hour): {1}", val.Timestamp.LocalTime, val.Value);
            }
        }

        public void PrintHourlyAverage(string meterName, string startTime, string endTime)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", _database);

            AFTime start = new AFTime(startTime);
            AFTime end = new AFTime(endTime);
            AFTimeRange timeRange = new AFTimeRange(start, end);

            IDictionary<AFSummaryTypes, AFValues> vals = attr.Data.Summaries(
                timeRange: timeRange,
                summaryDuration: new AFTimeSpan(TimeSpan.FromHours(1)),
                summaryType: AFSummaryTypes.Average,
                calcBasis: AFCalculationBasis.TimeWeighted,
                timeType: AFTimestampCalculation.EarliestTime);


            foreach (AFValue val in vals[AFSummaryTypes.Average])
            {
                Console.WriteLine("Timestamp (Local): {0}, Value (kilowatt hour): {1}", val.Timestamp.LocalTime, val.Value);
            }
        }

        public void PrintEnergyUsageForMeters(string timeStamp)
        {
            int startIndex = 0;
            int pageSize = 1000;
            int totalCount;

            AFAttributeList attrs = new AFAttributeList();

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

                attrs.AddRange(results);

                startIndex += pageSize;
            } while (startIndex < totalCount);       

            AFTime time = new AFTime(timeStamp);

            IList<AFValue> vals = attrs.Data.RecordedValue(time);

            foreach (AFValue val in vals)
            {
                Console.WriteLine("Meter name: {0}, Timestamp (Local): {1}, Value (kilowatt hour): {2}", 
                    val.Attribute.Element.Name,
                    val.Timestamp.LocalTime, 
                    val.Value);
            }
        }
    }
}
