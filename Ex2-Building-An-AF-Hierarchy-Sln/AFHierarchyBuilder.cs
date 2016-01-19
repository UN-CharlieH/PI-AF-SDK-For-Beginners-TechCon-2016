using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using External;

namespace Ex2_Building_An_AF_Hierarchy_Sln
{
    public static class AFHierarchyBuilder
    {
        static AFDatabase _database;

        static AFElementTemplate _houseTemplate;
        static AFElementTemplate _storyTemplate;
        static AFElementTemplate _roomTemplate;

        public static void Build()
        {
            CreateDatabase();
            CreateTemplates();
            CreateElements();
        }

        private static void CreateDatabase()
        {
            PISystem piSystem = new PISystems()[Constants.AFSERVERNAME];
            _database = piSystem.Databases.Add("Hogwarts");
        }

        private static void CreateTemplates()
        {
            // Create the element templates
            _houseTemplate = _database.ElementTemplates.Add("House");
            _storyTemplate = _database.ElementTemplates.Add("Story");
            _roomTemplate = _database.ElementTemplates.Add("Room");

            // Get a reference to the PI Point Data Reference
            AFPlugIn piPointDR = AFDataReference.GetPIPointDataReference(_database.PISystem);

            // Create the enumeration set
            AFEnumerationSet windowState = _database.EnumerationSets.Add("WindowState");
            windowState.Add("Open", 0);
            windowState.Add("Closed", 1);

            // Get a reference to the Celcius UOM
            UOM celsius = _database.PISystem.UOMDatabase.UOMs["degree Celsius"];

            // Create attribute templates for the House element template
            AFAttributeTemplate houseAvgTemp = _houseTemplate.AttributeTemplates.Add("Average Temperature");
            houseAvgTemp.DataReferencePlugIn = piPointDR;
            houseAvgTemp.ConfigString = @"\\< machinename>\%Element%.%Attribute%";
            houseAvgTemp.Type = typeof(double);
            houseAvgTemp.DefaultUOM = celsius;

            // Create attribute templates for the Story element template
            AFAttributeTemplate storyAvgTemp = _storyTemplate.AttributeTemplates.Add("Average Temperature");
            storyAvgTemp.DataReferencePlugIn = piPointDR;
            storyAvgTemp.ConfigString = @"\\<machinename>\%..\Element%.%Element%.%Attribute%";
            storyAvgTemp.Type = typeof(double);
            storyAvgTemp.DefaultUOM = celsius;

            // Create attribute templates for the Room element template
            AFAttributeTemplate roomTemp = _roomTemplate.AttributeTemplates.Add("Temperature");
            roomTemp.DataReferencePlugIn = piPointDR;
            roomTemp.ConfigString = @"\\<machinename>\%..\..\Element%.%..\Element%.%Element%.%Attribute%";
            roomTemp.Type = typeof(double);
            roomTemp.DefaultUOM = celsius;

            AFAttributeTemplate roomWindow = _roomTemplate.AttributeTemplates.Add("Window Position");
            roomWindow.DataReferencePlugIn = piPointDR;
            roomWindow.ConfigString = @"\\<machinename>\%..\..\Element%.%..\Element%.%Element%.%Attribute%";
            roomWindow.TypeQualifier = windowState;

            // Do a checkin at the end instead of one-by-one.
            _database.CheckIn();
        }

        private static void CreateElements()
        {
            AFElement gryffindor = _database.Elements.Add("Gryffindor", _houseTemplate);
            AFElement hufflepuff = _database.Elements.Add("Hufflepuff", _houseTemplate);
            AFElement ravenclaw = _database.Elements.Add("Ravenclaw", _houseTemplate);
            AFElement slytherin = _database.Elements.Add("Slytherin", _houseTemplate);

            CreateStoriesAndRooms(new[] { gryffindor, hufflepuff, ravenclaw, slytherin });

            _database.CheckIn();
        }

        private static void CreateStoriesAndRooms(IEnumerable<AFElement> houses)
        {
            foreach (AFElement house in houses)
            {
                foreach (int i in Enumerable.Range(1, 3))
                {
                    AFElement story = house.Elements.Add($"Story{i}", _storyTemplate);
                    foreach (int j in Enumerable.Range(1, 4))
                    {
                        AFElement room = story.Elements.Add($"Room{j}", _roomTemplate);
                    }
                }
            }
        }
    }
}
