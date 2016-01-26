using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;

namespace Ex5_Working_With_EventFrames
{
    public class AFEventFrameCreator
    {
        private AFDatabase _database;
        private AFElementTemplate _efTemp; // For AF Event Frame template

        // Define other instance members here

        public AFEventFrameCreator(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
        }

        public void CreateEventFrameTemplate()
        {
            // Your code here
        }

        public void CreateEventFrames()
        {
            // Your code here
        }

        public void CaptureValues()
        {
            // Your code here
        }

        public void PrintReport()
        {
            // Your code here
        }
    }
}
