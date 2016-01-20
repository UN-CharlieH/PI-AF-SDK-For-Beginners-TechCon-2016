using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            PISystem ps  = new PISystems()[server];
            if (ps != null) _database = ps.Databases[database];
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

                string categories = "[" + String.Join(",", element.Categories.Select(c => c.Name)) + "]";

                Console.WriteLine("{0}, {1}, {2}, {3}", 
                    element.Name, 
                    element.Template, 
                    categories,
                    element.Attributes.Count);
            }
        }

        public void FindMetersByTemplate(string templateName)
        {
            AFElementTemplate elemTemplate = _database.ElementTemplates[templateName];

            AFNamedCollectionList<AFElement> foundElements = AFElement.FindElementsByTemplate(
                                                                database: _database,
                                                                searchRoot: null,
                                                                template: elemTemplate,
                                                                includeDerived: false,
                                                                sortField: AFSortField.Name,
                                                                sortOrder: AFSortOrder.Ascending,
                                                                maxCount: 100);

            StringBuilder sb = new StringBuilder();
            foreach (AFElement element in foundElements)
            {
                sb.Append(element.Name).Append(", ");
            }
            sb.Length--;
            sb.Length--;
            Console.WriteLine(sb.ToString());
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

            StringBuilder sb = new StringBuilder();
            foreach (AFElement element in foundElements)
            {
                sb.Append(element.Name).Append(", ");
            }
            sb.Length--;
            sb.Length--;
            Console.WriteLine(sb.ToString());
        }

        public void FindMetersByUsage(AFSearchOperator op, double val)
        {
            AFElementTemplate elemTemplate = _database.ElementTemplates["MeterBasic"];
            AFAttributeTemplate attrTemplate = elemTemplate.AttributeTemplates["Energy Usage"];

            AFAttributeValueQuery[] query = new AFAttributeValueQuery[1];
            query[0] = new AFAttributeValueQuery(attrTemplate, op, val);

            AFNamedCollectionList<AFElement> foundElements = AFElement.FindElementsByAttribute(
                                                    searchRoot: null,
                                                    nameFilter: "*",
                                                    valueQuery: query,
                                                    searchFullHierarchy: true,
                                                    sortField: AFSortField.Name,
                                                    sortOrder: AFSortOrder.Ascending,
                                                    maxCount: 100);

            StringBuilder sb = new StringBuilder();
            foreach (AFElement element in foundElements)
            {
                sb.Append(element.Name).Append(", ");
            }
            sb.Length--;
            sb.Length--;
            Console.WriteLine(sb.ToString());
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

            IEnumerable<string> elementNames = foundAttributes.Select(a => a.Element.Name).Distinct();

            Console.WriteLine(String.Join(", ", elementNames));
        }
    }
}
