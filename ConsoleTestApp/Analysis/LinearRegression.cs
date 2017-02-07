using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleTestApp
{
    class LinearRegression
    {
        //private double xArray[8] = { 1, 2, 3, 4, 5, 6, 7};
        //private double[] xData = {1.0, 2.3, 3.1, 4.8, 5.6, 6.3};
        //private double[] yData = {2.6, 2.8, 3.1, 4.7, 5.1, 5.3};
        private double[] yData = {2130.75, 2130.5, 2130.5, 2130.5, 2130.25, 2130.25, 2130, 2130, 2130, 2130.5, 2130.25, 2130.25, 2130.75, 2130.75, 2130.75, 2130.75, 2130.5, 2130.5, 2130.5, 2130.5, 2130.25, 2130.25, 2129.5, 2129.75, 2130.5, 2130, 2129.25, 2129.25, 2129.5, 2130, 2130, 2130.25, 2130, 2130.25, 2129.5, 2130.25, 2130.5, 2130.25, 2129.75, 2130.25, 2130, 2131, 2130.75, 2131.25, 2132.25, 2132.25, 2131.75, 2130, 2130.5, 2130.75, 2131.25, 2131.25, 2131.25, 2131.5, 2132, 2132.75, 2131.75, 2132, 2132, 2131.25, 2131.5, 2131.25, 2131.5, 2131.5, 2131.5, 2131.25, 2131, 2131.5, 2131.5, 2131.75, 2131.25, 2131.25, 2131.5, 2131.25, 2131.5, 2131.5, 2132.25, 2132.25, 2132, 2132.25, 2132, 2131.5, 2131, 2131, 2131.25, 2131, 2130.25, 2131.5, 2131.5, 2130.75, 2130.5, 2130.25, 2130, 2130.25, 2131, 2130.75, 2131.25, 2131.5, 2131.5, 2131.75, 2131, 2132, 2132, 2132.25, 2133.5, 2134.75, 2134.75, 2134.75, 2136, 2136.5, 2137.25, 2137.25, 2136.75, 2137.25, 2137, 2137, 2137, 2134, 2134.25, 2134.5, 2135.5, 2136.25, 2137.25, 2138, 2138.75, 2139.75, 2139, 2139, 2140.25, 2140.75, 2141, 2140, 2139.5, 2140, 2139.5, 2140, 2140, 2140.5, 2140.25, 2140, 2140.25, 2140.5, 2140, 2139.5, 2139.5, 2140, 2139.75, 2139.75, 2139.25, 2139.75, 2140, 2140.5, 2141, 2141.25, 2141.25, 2140.5, 2140.25, 2139.75, 2139.75, 2139.5, 2140.75, 2140.5, 2140, 2140, 2140.25, 2140.25, 2140.75, 2140.25, 2140.5, 2141.25, 2141.25, 2139.75, 2139.25, 2140, 2140.75, 2140.5, 2141, 2140.5, 2140.5, 2140.75, 2141.25, 2140.75, 2141.25, 2142, 2141.75, 2141};
       // private double[] xData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186 };
        private int[] xData = Enumerable.Range(1, 186).ToArray();
        
        public LinearRegression()
        {
            Console.WriteLine("conducting linear regression of hardcoded data");
        GetSlope(xData, yData);
            Console.WriteLine(yData.Count() + " " + xData.Count());
        }

        public static double GetSlope(int[] x, double[] y)
{
            Console.WriteLine("Insde the GetSlope Method");
    

            double[] xyData = new double[Math.Max(x.Count(),y.Count())];
            double[] xxData = new double[Math.Max(x.Count(), y.Count())];
            double[] yyData = new double[Math.Max(x.Count(), y.Count())];

            double xSum = 0.0;
            double ySum = 0.0;
            double xySum = 0.0;
            double xxSum = 0.0;
            double yySum = 0.0;

            double m_slope = 0.0;
            double y_int = 0.0;
            double r_value = 0.0;

            for (int i = 0; i < xyData.Count(); i++)
            {
                xyData[i] = x[i]*y[i];
                xxData[i] = x[i]*x[i];
                yyData[i] = y[i]*y[i];

                xSum = xSum + x[i];
                ySum = ySum + y[i];
                xxSum = xxSum + xxData[i];
                xySum = xySum + xyData[i];
                yySum = yySum + yyData[i];

                Console.WriteLine(string.Format("{0} * {1} = xy {2}  x^2 {3}  y^2  {4}",x[i],y[i],xyData[i],xxData[i],yyData[i],xSum,ySum,xySum,xxSum,yySum));

            }

            m_slope = (xyData.Count()*xySum - xSum*ySum)/(xyData.Count()*xxSum - (xSum*xSum));
            y_int = (ySum - m_slope*xSum)/xyData.Count();
            r_value = (xyData.Count()*xySum - xSum*ySum)/
                      Math.Sqrt((xyData.Count()*xxSum - xSum*xSum)*(xyData.Count()*yySum - ySum*ySum));



            Console.WriteLine(string.Format("sum(x) {0} sum(y) {1} sum(xy) {2} sum(x^2) {3} sum(y^2) {4} (sum(x))^2 {5}  (sum(y))^2 {6}",xSum,ySum,xySum,xxSum,yySum,(xSum*xSum),(ySum*ySum)));
            Console.WriteLine(string.Format("slope_m {0}  int_y {1}  r {2}",m_slope,y_int,r_value));


            return 1;
}

    }
}
