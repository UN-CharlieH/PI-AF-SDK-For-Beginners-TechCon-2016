using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Time;
using External;

namespace Ex5_Working_With_Event_Frames
{
    public class AFEventFrameCreator
    {
        private AFDatabase _database;
        private AFElementTemplate _eventFrameTemplate;

        public AFEventFrameCreator(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
        }

        public void CreateEventFrameTemplate()
        {
            _eventFrameTemplate = _database.ElementTemplates.Add("Daily Usage");
            _eventFrameTemplate.InstanceType = typeof(AFEventFrame);
            _eventFrameTemplate.NamingPattern = @"%TEMPLATE%-%ELEMENT%-%STARTTIME:yyyy-MM-dd%*";
            AFAttributeTemplate usage = _eventFrameTemplate.AttributeTemplates.Add("Average Energy Usage");

            usage.Type = typeof(double);
            usage.DataReferencePlugIn = AFDataReference.GetPIPointDataReference();
            usage.ConfigString = @".\Elements[.]|Energy Usage;TimeRangeMethod=Average";
            usage.DefaultUOM = _database.PISystem.UOMDatabase.UOMs["kilowatt hour"];

            _database.CheckIn();
        }

        public void CreateEventFrames()
        {
            const int pageSize = 200;
            int startIndex = 0;
            int totalCount;
            do
            {
                AFNamedCollectionList<AFBaseElement> results = _database.ElementTemplates["MeterBasic"].FindInstantiatedElements(
                    includeDerived: true,
                    sortField: AFSortField.Name,
                    sortOrder: AFSortOrder.Ascending,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    totalCount: out totalCount
                    );

                IList<AFElement> meters = results.Select(elm => (AFElement)elm).ToList();

                foreach (AFElement meter in meters)
                {
                    foreach (int day in Enumerable.Range(1, 31))
                    {
                        DateTime start = new DateTime(2015, 12, day, 0, 0, 0, DateTimeKind.Local);
                        AFTime startTime = new AFTime(start);
                        AFTime endTime = new AFTime(start.AddDays(1));
                        AFEventFrame ef = new AFEventFrame(_database, "*", _eventFrameTemplate);
                        ef.SetStartTime(startTime);
                        ef.SetEndTime(endTime);
                        ef.PrimaryReferencedElement = meter;
                    }
                }

                _database.CheckIn();

                startIndex += pageSize;
            } while (startIndex < totalCount);
        }

        public void CaptureValues()
        {
            const int pageSize = 200;
            int startIndex = 0;

            AFTime startTime = new AFTime(new DateTime(2015, 12, 1, 0, 0, 0, DateTimeKind.Local));
            AFNamedCollectionList<AFEventFrame> efs;
            do
            {
                efs = AFEventFrame.FindEventFrames(
                    database: _database,
                    searchRoot: null,
                    startTime: startTime,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    searchMode: AFEventFrameSearchMode.ForwardFromStartTime,
                    nameFilter: "*",
                    referencedElementNameFilter: "*",
                    eventFrameCategory: null,
                    eventFrameTemplate: _eventFrameTemplate,
                    referencedElementTemplate: null,
                    searchFullHierarchy: true
                    );

                foreach (AFEventFrame ef in efs)
                {
                    if (!ef.AreValuesCaptured)
                        ef.CaptureValues();
                }

                _database.CheckIn();

                startIndex += pageSize;
            } while (efs != null && efs.Count > 0);
        }

        public void PrintReport()
        {
            const int pageSize = 200;
            int startIndex = 0;

            AFTime startTime = new AFTime(new DateTime(2015, 12, 29, 0, 0, 0, DateTimeKind.Local));
            AFTime endTime = new AFTime(new DateTime(2015, 12, 31, 0, 0, 0, DateTimeKind.Local));

            AFNamedCollectionList<AFEventFrame> efs;
            do
            {
                efs = AFEventFrame.FindEventFrames(
                    database: _database,
                    searchRoot: null,
                    searchMode: AFSearchMode.StartInclusive,
                    startTime: startTime,
                    endTime: endTime,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    nameFilter: "*",
                    referencedElementNameFilter: "Meter003",
                    eventFrameCategory: null,
                    eventFrameTemplate: _eventFrameTemplate,
                    referencedElementTemplate: null,
                    durationQuery: null,
                    searchFullHierarchy: true,
                    sortField: AFSortField.Name,
                    sortOrder: AFSortOrder.Ascending
                    );

                AFEventFrame.LoadEventFrames(efs);

                foreach (AFEventFrame ef in efs)
                {
                    Console.WriteLine("{0}, {1}, {2}", 
                        ef.Name, 
                        ef.PrimaryReferencedElement.Name, 
                        ef.Attributes["Average Energy Usage"].GetValue().Value);
                }

                startIndex += pageSize;
            } while (efs != null && efs.Count > 0);

        }
    }
}
