using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External;
using OSIsoft.AF;

namespace Ex8_Asynchronous_Read_Write
{
    class Program
    {
        static void Main(string[] args)
        {
            AFDatabase database = AFDataReadWrite.GetDatabase(Constants.AFSERVERNAME, "Magical Power Company");
            AFDataReadWrite.UpdateAttributeData(database);
            AFDataReadWrite.GetTotalsAsync(database);
            // For comparison
            AFDataReadWrite.GetTotalsBulk(database);
        }
    }
}
