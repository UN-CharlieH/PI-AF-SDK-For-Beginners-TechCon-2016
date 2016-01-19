using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External;

namespace Ex5_Working_With_EventFrames
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
