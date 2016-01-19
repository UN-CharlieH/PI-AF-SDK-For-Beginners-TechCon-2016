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
        // Define any members here
        private static AFDatabase _database;
        private static AFElementTemplate _efTemp; // For AF Event Frame template

        public static void Create()
        {
            CreateEventFrameTemplate();
            CreateEventFrames();
            CaptureValues();
            PrintReport();
        }

        private static void CreateEventFrameTemplate()
        {
            // Your code here
        }

        private static void CreateEventFrames()
        {
            // Your code here
        }

        private static void CaptureValues()
        {
            // Your code here
        }

        private static void PrintReport()
        {
            // Your code here
        }
    }
}
