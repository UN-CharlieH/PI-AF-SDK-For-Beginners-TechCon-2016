using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex3_Searching_For_Assets_Sln
{
    public class AFAssetSearcher
    {
        AFDatabase _database;

        public AFAssetSearcher(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
        }

        public void FindMetersByName(string elementNameFilter)
        {
            AFNamedCollectionList<AFElement> foundElements = AFElement.FindElements(
                                                                database: _database,
                                                                searchRoot: null,
                                                                query: elementNameFilter,
                                                                field: AFSearchField.Name,
                                                                searchFullHierarchy: true,
                                                                sortField: AFSortField.Name,
                                                                sortOrder: AFSortOrder.Ascending,
                                                                maxCount: 100);

            foreach (AFElement element in foundElements)
            {
                Console.WriteLine("Element: {0}, Template: {1}, Categories: {2}",
                    element.Name,
                    element.Template.Name,
                    element.CategoriesString);
            }
        }

        public void FindMetersByTemplate(string templateName)
        {
            AFElementTemplate elemTemplate = _database.ElementTemplates[templateName];

            AFNamedCollectionList<AFElement> foundElements = AFElement.FindElementsByTemplate(
                                                                database: _database,
                                                                searchRoot: null,
                                                                template: elemTemplate,
                                                                includeDerived: true,
                                                                sortField: AFSortField.Name,
                                                                sortOrder: AFSortOrder.Ascending,
                                                                maxCount: 100);

            foreach (AFElement element in foundElements)
            {
                Console.WriteLine("Element: {0}, Template: {1}", element.Name, element.Template.Name);
            }
        }

        public void FindMetersBySubstation(string substationLocation)
        {
            AFElementTemplate elemTemplate = _database.ElementTemplates["MeterBasic"];
            AFAttributeTemplate attrTemplate = elemTemplate.AttributeTemplates["Substation"];

            AFAttributeValueQuery[] query = new AFAttributeValueQuery[1];
            query[0] = new AFAttributeValueQuery(attrTemplate, AFSearchOperator.Equal, substationLocation);


            AFNamedCollectionList<AFElement> foundElements = AFElement.FindElementsByAttribute(
                                                    searchRoot: null,
                                                    nameFilter: "*",
                                                    valueQuery: query,
                                                    searchFullHierarchy: true,
                                                    sortField: AFSortField.Name,
                                                    sortOrder: AFSortOrder.Ascending,
                                                    maxCount: 100);

            string[] meterNames = new string[foundElements.Count];
            int i = 0;
            foreach (AFElement element in foundElements)
            {
                meterNames[i++] = element.Name;
            }
            Console.WriteLine(string.Join(", ", meterNames));
        }

        public void FindMetersAboveUsage(double val)
        {
            AFElementTemplate elemTemplate = _database.ElementTemplates["MeterBasic"];
            AFAttributeTemplate attrTemplate = elemTemplate.AttributeTemplates["Energy Usage"];

            AFAttributeValueQuery[] query = new AFAttributeValueQuery[1];
            query[0] = new AFAttributeValueQuery(attrTemplate, AFSearchOperator.GreaterThan, val);

            AFNamedCollectionList<AFElement> foundElements = AFElement.FindElementsByAttribute(
                                                    searchRoot: null,
                                                    nameFilter: "*",
                                                    valueQuery: query,
                                                    searchFullHierarchy: true,
                                                    sortField: AFSortField.Name,
                                                    sortOrder: AFSortOrder.Ascending,
                                                    maxCount: 100);

            string[] meterNames = new string[foundElements.Count];
            int i = 0;
            foreach (AFElement element in foundElements)
            {
                meterNames[i++] = element.Name;
            }
            Console.WriteLine(string.Join(", ", meterNames));
        }

        public void FindBuildingInfo(string templateName)
        {
            AFElementTemplate elemTemp = _database.ElementTemplates[templateName];
            AFCategory buildingInfoCat = _database.AttributeCategories["Building Info"];

            AFElement root = _database.Elements["Wizarding World"];

            AFNamedCollectionList<AFAttribute> foundAttributes = AFAttribute.FindElementAttributes(
                                                    database: _database,
                                                    searchRoot: root,
                                                    nameFilter: "*",
                                                    elemCategory: null,
                                                    elemTemplate: elemTemp,
                                                    elemType: AFElementType.Any,
                                                    attrNameFilter: "*",
                                                    attrCategory: buildingInfoCat,
                                                    attrType: TypeCode.Empty,
                                                    searchFullHierarchy: true,
                                                    sortField: AFSortField.Name,
                                                    sortOrder: AFSortOrder.Ascending,
                                                    maxCount: 100);

            Console.WriteLine("Found {0} attributes:", foundAttributes.Count);
        }
    }
}
