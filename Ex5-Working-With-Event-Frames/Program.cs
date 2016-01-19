using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex5_Working_With_Event_Frames
{
    class Program
    {
        static void Main(string[] args)
        {
            AFEventFrameCreator.Create();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
