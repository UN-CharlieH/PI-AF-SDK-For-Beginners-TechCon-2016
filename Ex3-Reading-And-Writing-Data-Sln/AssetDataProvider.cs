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

        public void PrintEnergyUsageAtTime(string timeStamp)
        {
            AFAttributeList attrList = GetAttributes();

            AFTime time = new AFTime(timeStamp);

            IList<AFValue> vals = attrList.Data.RecordedValue(time);

            foreach (AFValue val in vals)
            {
                Console.WriteLine("Meter name: {0}, Timestamp (Local): {1}, Value (kilowatt hour): {2}", 
                    val.Attribute.Element.Name,
                    val.Timestamp.LocalTime, 
                    val.Value);
            }
        }

        public void PrintDailyAverageEnergyUsage(string startTime, string endTime)
        {
            AFAttributeList attrList = GetAttributes();

            AFTime start = new AFTime(startTime);
            AFTime end = new AFTime(endTime);
            AFTimeRange timeRange = new AFTimeRange(start, end);

            // Ask for 100 PI Points at a time
            PIPagingConfiguration pagingConfig = new PIPagingConfiguration(PIPageType.TagCount, 100);

            IEnumerable<IDictionary<AFSummaryTypes, AFValues>> summaries = attrList.Data.Summaries(
                timeRange: timeRange,
                summaryDuration: new AFTimeSpan(TimeSpan.FromDays(1)),
                summaryTypes: AFSummaryTypes.Average,
                calculationBasis: AFCalculationBasis.TimeWeighted,
                timeType: AFTimestampCalculation.EarliestTime,
                pagingConfig: pagingConfig);

            // Loop through attributes
            foreach (IDictionary<AFSummaryTypes,AFValues> dict in summaries)
            {
                AFValues values = dict[AFSummaryTypes.Average];
                Console.WriteLine("Averages for Meter: {0}", values.Attribute.Element.Name);

                // Loop through values per attribute
                foreach (AFValue val in dict[AFSummaryTypes.Average])
                {
                    Console.WriteLine("Timestamp (Local): {0}, Avg. Value (kilowatt hour): {1}",
                        val.Timestamp.LocalTime,
                        val.Value);
                }
                Console.WriteLine();

            }
        }

        public void SwapValues(string meter1, string meter2, string startTime, string endTime)
        {
            // NOTE: This method does not ensure that there is no data loss if there is failure.
            // Persist the data first in case you need to rollback.

            AFAttribute attr1 = AFAttribute.FindAttribute(@"\Meters\" + meter1 + @"|Energy Usage", _database);
            AFAttribute attr2 = AFAttribute.FindAttribute(@"\Meters\" + meter2 + @"|Energy Usage", _database);

            AFTime start = new AFTime(startTime);
            AFTime end = new AFTime(endTime);
            AFTimeRange timeRange = new AFTimeRange(start, end);

            // Get values to delete for meter1
            AFValues valsToRemove1 = attr1.Data.RecordedValues(
                timeRange: timeRange,
                boundaryType: AFBoundaryType.Inside,
                desiredUOM: null,
                filterExpression: null,
                includeFilteredValues: false);

            // Get values to delete for meter2
            AFValues valsToRemove2 = attr2.Data.RecordedValues(
                timeRange: timeRange,
                boundaryType: AFBoundaryType.Inside,
                desiredUOM: null,
                filterExpression: null,
                includeFilteredValues: false);

            List<AFValue> valsToRemove = valsToRemove1.ToList();
            valsToRemove.AddRange(valsToRemove2.ToList());

            // Remove the values
            AFListData.UpdateValues(valsToRemove, AFUpdateOption.Remove);

            // Create new AFValues from the other meter and assign them to this meter
            List<AFValue> valsToAdd1 = valsToRemove2.Select(v => new AFValue(attr1, v.Value, v.Timestamp)).ToList();
            List<AFValue> valsToAdd2 = valsToRemove1.Select(v => new AFValue(attr2, v.Value, v.Timestamp)).ToList();

            List<AFValue> valsCombined = valsToAdd1;
            valsCombined.AddRange(valsToAdd2);

            AFListData.UpdateValues(valsCombined, AFUpdateOption.Insert);
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
