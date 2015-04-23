using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageScreensaver
{
    public class Dashboard
    {
        public Dashboard(string[] csvArray)
        {
            Url = csvArray[0];
            Period = Convert.ToInt32(csvArray[1]);
            Scale = Convert.ToInt32(csvArray[2]);
            StartHour = Convert.ToInt32(csvArray[3]);
            EndHour = Convert.ToInt32(csvArray[4]);
        }

        public int EndHour { get; set; }
        public int Period { get; set; }
        public int Scale { get; set; }
        public int StartHour { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3},{4}", Url, Period, Scale, StartHour, EndHour);
        }
    }   
}
