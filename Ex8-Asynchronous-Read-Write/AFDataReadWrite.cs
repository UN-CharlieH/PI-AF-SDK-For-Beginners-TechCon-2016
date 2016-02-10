using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;

namespace Ex8_Asynchronous_Read_Write
{
    public class AFDataReadWrite
    {
        public static IDictionary<AFAttribute, AFValues> GetTotalsAsync(AFDatabase afDb)
        {
            AFAttributeList attributeList = GetAttributes(afDb);

            // Beginning of current month to beginning of today.
            Dictionary<AFAttribute, AFValues> totals = new Dictionary<AFAttribute, AFValues>();
            AFTimeRange timeRange = new AFTimeRange(new AFTime(string.Format("{0}-{1}-01", DateTime.Now.Year, DateTime.Now.Month)), new AFTime("T"));
            AFTimeSpan dayInterval = new AFTimeSpan(0, 0, 1);
            List<Task<IDictionary<AFSummaryTypes, AFValues>>> processingList = new List<Task<IDictionary<AFSummaryTypes, AFValues>>>();
            foreach (AFAttribute attribute in attributeList)
            {
                // Do not make the call if async is not supported
                if ((attribute.SupportedDataMethods & AFDataMethods.Asynchronous) == 0)
                    continue;

                try
                {
                    processingList.Add(
                    attribute.Data.SummariesAsync(timeRange, dayInterval, AFSummaryTypes.Total, AFCalculationBasis.TimeWeighted, AFTimestampCalculation.Auto));

                    // periodically evaluate
                    if (processingList.Count > Environment.ProcessorCount * 2)
                    {
                        Task.WhenAll(processingList.ToArray());
                        foreach (var item in processingList)
                        {
                            WriteSummaryItem(item.Result[AFSummaryTypes.Total]);
                        }

                        processingList = new List<Task<IDictionary<AFSummaryTypes, AFValues>>>();
                    }
                }
                catch (AggregateException ae)
                {
                    //if (ae.Flatten().InnerExceptions.Count == 1)
                    Console.WriteLine("{0}: {1}", attribute.Name, ae.Flatten().InnerException.Message);
                }
            }

            if (processingList.Count > 0)
            {
                Task.WhenAll(processingList.ToArray());
                foreach (var item in processingList)
                {
                    WriteSummaryItem(item.Result[AFSummaryTypes.Total]);
                }
            }

            return totals;
        }

        public static IDictionary<AFAttribute, AFValues> GetTotalsBulk(AFDatabase afDb)
        {
            AFAttributeList attributeList = GetAttributes(afDb);

            Dictionary<AFAttribute, AFValues> totals = new Dictionary<AFAttribute, AFValues>();
            AFTimeRange timeRange = new AFTimeRange(new AFTime(string.Format("{0}-{1}-01", DateTime.Now.Year, DateTime.Now.Month)), new AFTime("T"));
            AFTimeSpan dayInterval = new AFTimeSpan(0, 0, 1);

            PIPagingConfiguration pageConfig = new PIPagingConfiguration(PIPageType.TagCount, Environment.ProcessorCount * 2);
            foreach (var item in attributeList.Data.Summaries(timeRange, dayInterval, AFSummaryTypes.Total, AFCalculationBasis.TimeWeighted, AFTimestampCalculation.Auto,
                pageConfig))
            {
                WriteSummaryItem(item[AFSummaryTypes.Total]);
            }

            return totals;
        }

        private static void WriteSummaryItem(AFValues summaryitems)
        {
            Console.WriteLine(" First total for {0}: {1}", summaryitems.Attribute.Name, AFValueToString(summaryitems[0]));
        }

        public static string AFValueToString(AFValue value)
        {
            return string.Format("{0} @ {1}", value.Value.ToString(), value.Timestamp.LocalTime.ToString("MM-dd HH:mm:ss.F"));
        }

        public static int UpdateAttributeData(AFDatabase database)
        {
            AFAttributeList attributeList = GetAttributes(database);

            AFTimeRange timeRange = new AFTimeRange(new AFTime(string.Format("{0}-{1}-01", DateTime.Now.Year, DateTime.Now.Month)), new AFTime("T"));
            AFTimeSpan hourInterval = new AFTimeSpan(0, 0, 0, 1);

            // Question: What risk is run by the following code?
            List<Task<AFErrors<AFValue>>> processWrites = new List<Task<AFErrors<AFValue>>>();
            foreach (AFAttribute attribute in attributeList)
            {
                AFValues vals = GenerateValueSequence(attribute, timeRange.StartTime, timeRange.EndTime, hourInterval);
                processWrites.Add(attribute.Data.UpdateValuesAsync(vals, AFUpdateOption.Insert, AFBufferOption.DoNotBuffer));
            }

            int errorcount = 0;
            try
            {
                Task.WaitAll(processWrites.ToArray());
                foreach (var item in processWrites)
                {
                    AFErrors<AFValue> errors = item.Result;
                    // Count PIPoint errors
                    if (errors != null && errors.HasErrors)
                        errorcount += errors.Errors.Count;

                    // Report PI Server, AF Server errors
                }
            }
            catch (AggregateException ae)
            {
            }

            return errorcount;
        }

        #region Setup
        public static AFDatabase GetDatabase(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null)
                return piSystem.Databases[database];
            else
                return piSystem.Databases.DefaultDatabase;
        }

        public static AFAttributeList GetAttributes(AFDatabase database)
        {
            int startIndex = 0;
            int pageSize = 1000;
            int totalCount;

            AFAttributeList attrList = new AFAttributeList();

            do
            {
                AFAttributeList results = AFAttribute.FindElementAttributes(
                     database: database,
                     searchRoot: null,
                     nameFilter: null,
                     elemCategory: null,
                     elemTemplate: database.ElementTemplates["MeterBasic"],
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
        private static AFValues GenerateValueSequence(AFAttribute attribute, AFTime start, AFTime end, AFTimeSpan interval)
        {
            float zero = 100.0F;
            float span = 360.0F;
            AFValues values = new AFValues();
            AFTime timestamp = start;
            Random rnd = new Random((int)DateTime.Now.Ticks % 86400);
            int idx = 1;
            while (timestamp <= end)
            {
                values.Add(new AFValue(attribute, zero + (float)rnd.NextDouble() * span, timestamp));
                timestamp = interval.Multiply(start, idx++);
            }

            return values;
        }

        private static AFValue GenerateValue(AFAttribute attribute, Random rnd, float zero, float span, AFTime timestamp)
        {
            return new AFValue(attribute, zero + (float)rnd.NextDouble() * span, timestamp);
        }
        #endregion
    }
}
