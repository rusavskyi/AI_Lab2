using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            LatinSquare latinSquare = new LatinSquare(4);
            Queens queens = new Queens(5);
            int count = 1;
            double lsbt = 0;
            double lsbth = 0;
            double lsfc = 0;
            double lsfch = 0;
            double qbt = 0;
            double qbth = 0;
            double qfc = 0;
            double qfch = 0;

            for (int i = 0; i < count; i++)
            {
                lsbt += latinSquare.BackTracking();
                lsbth += latinSquare.BackTrackingH();
                lsfc += latinSquare.ForwardChecking();
                lsfch += latinSquare.ForwardCheckingH();
                qbt += queens.BackTracking();
                qbth += queens.BackTrackingH();
                qfc += queens.ForwardChecking();
                qfch += queens.ForwardCheckingH(); 
            }

            Console.WriteLine("avg time");
            Console.WriteLine(lsbt / count);
            Console.WriteLine(lsbth / count);
            Console.WriteLine(lsfc / count);
            Console.WriteLine(lsfch / count);
            Console.WriteLine(qbt / count);
            Console.WriteLine(qbth / count);
            Console.WriteLine(qfc / count);
            Console.WriteLine(qfch / count);
            Console.ReadLine();
        }
    }
}
