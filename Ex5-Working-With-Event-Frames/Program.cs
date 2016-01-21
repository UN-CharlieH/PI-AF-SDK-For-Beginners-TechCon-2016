using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External;

namespace Ex5_Working_With_Event_Frames
{
    class Program
    {
        static void Main(string[] args)
        {
            AFEventFrameCreator efCreator = new AFEventFrameCreator(Constants.AFSERVERNAME, "Wizardry Power Company");
            efCreator.CreateEventFrameTemplate();
            efCreator.CreateEventFrames();
            efCreator.CaptureValues();
            efCreator.PrintReport();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
