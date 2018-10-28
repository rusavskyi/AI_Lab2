using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace AI_Lab2
{
    public class LatinSquare
    {
        public int[,] Matrix { get; set; }
        public int Size { get; set; }

        public int Steps { get; set; }
        public int Solutions { get; set; }

        public DateTime Time { get; set; }

        private StringBuilder _log;
        private StringBuilder _foundSolutionLog;


        public LatinSquare(int size)
        {
            Matrix = new int[size, size];
            Size = size;
            Steps = 0;
            Solutions = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Matrix[i, j] = 0;
                }
            }
        }

        public bool Check(int i, int j)
        {
            for (int k = 0; k < Size; k++)
            {
                if (k == i || k == j) continue;

                if (Matrix[i, k] == Matrix[i, j] && Matrix[i, k] != 0 ||
                    Matrix[k, j] == Matrix[i, j] && Matrix[k, j] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckValue(int i, int j, int value)
        {
            for (int k = 0; k < Size; k++)
            {
                if (k == i || k == j) continue;

                if (Matrix[i, k] == value && Matrix[i, k] != 0 ||
                    Matrix[k, j] == value && Matrix[k, j] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckAll()
        {
            bool result = true;
            for (int i = 0; i < Size && result; i++)
            {
                for (int j = 0; j < Size && result; j++)
                {
                    result = Check(i, j);
                }
            }

            return result;
        }

        public double BackTracking()
        {
            Steps = 0;
            Solutions = 0;
            _log = new StringBuilder("Step,Solutions\n");
            _foundSolutionLog = new StringBuilder("Step,Solutions\n");
            Time = DateTime.Now;
            BackTracking(0, 0);
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("BT");
            return timeSpan.TotalMilliseconds;
        }

        private bool BackTracking(int i, int j)
        {
            bool result = false;

            if (i >= Size)
            {
                if (CheckAll())
                {
                    Solutions++;
                    WriteSolutionFound();
                }

                return false;
            }

            if (j >= Size)
            {
                return BackTracking(i + 1, 0);
            }

            Steps++;

            for (int value = 1; value <= Size && !result; value++)
            {
                Matrix[i, j] = value;
                //Console.Clear();
                //Console.WriteLine(ToString());
                WriteToLog();
                //Console.ReadKey();
                if (Check(i, j))
                    result = BackTracking(i, j + 1);
            }

            if (!result)
            {
                Matrix[i, j] = 0;
                //Steps++;
            }

            return result;
        }

        public double BackTrackingH()
        {
            Steps = 0;
            Solutions = 0;
            _log = new StringBuilder("Step,Solutions\n");
            _foundSolutionLog = new StringBuilder("Step,Solutions\n");
            Time = DateTime.Now;
            BackTrackingH(0, 0);
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("BTH");
            return timeSpan.TotalMilliseconds;
        }

        private bool BackTrackingH(int i, int j)
        {
            bool result = false;

            if (i >= Size)
            {
                if (CheckAll())
                {
                    Solutions++;
                    WriteSolutionFound();
                }

                return false;
            }

            if (j >= Size)
            {
                return BackTracking(i + 1, 0);
            }

            Steps++;
            List<int> checkedValues = new List<int>();
            for (int k = 1; k <= Size; k++)
            {
                checkedValues.Add(k);
            }
            Random random = new Random(DateTime.Now.GetHashCode());
            while (checkedValues.Count > 0)
            {
                int valueId = random.Next(0, checkedValues.Count);
                Matrix[i, j] = checkedValues[valueId];
                checkedValues.RemoveAt(valueId);
                //Console.WriteLine(ToString());
                WriteToLog();
                if (Check(i, j))
                    result = BackTracking(i, j + 1);
            }

            if (!result)
            {
                Matrix[i, j] = 0;
            }

            return result;
        }

        public double ForwardChecking()
        {
            Steps = 0;
            Solutions = 0;
            _log = new StringBuilder("Step,Solutions\n");
            _foundSolutionLog = new StringBuilder("Step,Solutions\n");
            Time = DateTime.Now;
            ForwardChecking(0, 0, MakeDomain(0, 0));
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("FC");
            return timeSpan.TotalMilliseconds;
        }

        private void ForwardChecking(int i, int j, List<int> domain)
        {

            if (i >= Size)
            {
                if (CheckAll())
                {
                    Solutions++;
                    WriteSolutionFound();
                }

                return;
            }

            if (j >= Size)
            {
                ForwardChecking(i + 1, 0, domain);
                return;
            }

            Steps++;

            foreach (int value in domain)
            {
                Matrix[i, j] = value;
                //Console.WriteLine(ToString());
                WriteToLog();
                List<int> nextDomain = MakeDomain(i, j + 1);
                if (nextDomain.Count > 0)
                {
                    ForwardChecking(i, j + 1, nextDomain);
                }
            }

            Matrix[i, j] = 0;
        }

        public double ForwardCheckingH()
        {
            Steps = 0;
            Solutions = 0;
            _log = new StringBuilder("Step,Solutions\n");
            _foundSolutionLog = new StringBuilder("Step,Solutions\n");
            Time = DateTime.Now;
            ForwardCheckingH(0, 0, MakeDomain(0, 0));
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("FCH");
            return timeSpan.TotalMilliseconds;
        }

        private void ForwardCheckingH(int i, int j, List<int> domain)
        {
            if (i >= Size)
            {
                if (CheckAll())
                {
                    Solutions++;
                    WriteSolutionFound();
                }

                return;
            }

            if (j >= Size)
            {
                ForwardChecking(i + 1, 0, domain);
                return;
            }

            Steps++;
            Random random = new Random(DateTime.Now.GetHashCode());
            do
            {
                int valueId = random.Next(0, domain.Count);
                Matrix[i, j] = domain[valueId];
                domain.RemoveAt(valueId);
                //Console.WriteLine(ToString());
                WriteToLog();
                List<int> nextDomain = MakeDomain(i, j + 1);
                if (nextDomain.Count > 0)
                {
                    ForwardChecking(i, j + 1, nextDomain);
                }
            } while (domain.Count > 0);

            Matrix[i, j] = 0;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            stringBuilder.Append("Step: ");
            stringBuilder.Append(Steps);
            stringBuilder.Append(" Solutions found: ");
            stringBuilder.Append(Solutions);
            stringBuilder.Append("\n\n");
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    stringBuilder.Append(Matrix[i, j]);
                    stringBuilder.Append(" ");
                }

                stringBuilder.Append("\n");
            }

            stringBuilder.Append("\n");
            return stringBuilder.ToString();
        }

        private List<int> MakeDomain(int i, int j)
        {
            List<int> domain = new List<int>();

            if (i >= Size)
            {
                domain.Add(0);
                return domain;
            }

            if (j >= Size)
            {
                return MakeDomain(i + 1, 0);
            }

            for (int value = 1; value <= Size; value++)
            {
                if (CheckValue(i, j, value))
                {
                    domain.Add(value);
                }
            }
            return domain;
        }

        private void WriteToLog()
        {
            _log.Append(Steps);
            _log.Append(",");
            _log.Append(Solutions);
            _log.Append("\n");
        }

        private void WriteSolutionFound()
        {
            _foundSolutionLog.Append(Steps);
            _foundSolutionLog.Append(",");
            _foundSolutionLog.Append(Solutions);
            _foundSolutionLog.Append("\n");
        }

        private void WriteLogToFile(string name)
        {
            TextWriter textWriter = File.CreateText(name + Size + "_" +DateTime.Now.GetHashCode() + ".csv");
            textWriter.Write(_log.ToString());
            textWriter.Close();
            textWriter = File.CreateText(name + Size + "_SF_" + DateTime.Now.GetHashCode() + ".csv");
            textWriter.Write(_foundSolutionLog.ToString());
            textWriter.Close();
        }
    }
}