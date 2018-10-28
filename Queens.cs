using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AI_Lab2
{
    public class Queens
    {
        public int[] Colums { get; set; }
        public int Size { get; set; }
        public int Steps { get; set; }
        public int Solutions { get; set; }
        public DateTime Time { get; set; }


        private StringBuilder _log;
        private StringBuilder _foundSolutionLog;

        public Queens(int size)
        {
            Colums = new int[size];
            Size = size;
            Solutions = 0;
            Steps = 0;

            for (int i = 0; i < size; i++)
            {
                Colums[i] = 0;
            }
        }

        public bool Check(int i)
        {
            for (int j = 0; j < i; j++)
            {
                if (Colums[j] == Colums[i]) return false;
                if (Colums[j] - (i - j) == Colums[i]) return false;
                if (Colums[j] + (i - j) == Colums[i]) return false;
            }

            return true;
        }

        public bool CheckValue(int i, int value)
        {
            for (int j = 0; j < i; j++)
            {
                if (Colums[j] == value) return false;
                if (Colums[j] - (i - j) == value) return false;
                if (Colums[j] + (i - j) == value) return false;
            }

            return true;
        }

        public bool CheckAll()
        {
            bool result = true;
            for (int i = 0; i < Size && result; i++)
            {
                result = Check(i);
            }

            return result;
        }

        public double BackTracking()
        {
            InitNewTest();
            Console.WriteLine("QBT");
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            BackTracking(0);
            WriteLogToFile("QBT");
            return timeSpan.TotalMilliseconds;
        }

        private bool BackTracking(int i)
        {
            if (i >= Size)
            {
                if (CheckAll())
                {
                    Solutions++;
                    WriteSolutionFound();
                }
                return false;
            }

            Steps++;

            for (int j = 0; j < Size; j++)
            {
                Colums[i] = j;
                //Console.WriteLine(ToString());
                WriteToLog();
                if (Check(i))
                {
                    BackTracking(i + 1);
                }
            }

            return true;
        }

        public double ForwardChecking()
        {
            InitNewTest();
            Console.WriteLine("QFC");
            ForwardChecking(0, MakeDomain(0));
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("QFC");
            return timeSpan.TotalMilliseconds;
        }

        private void ForwardChecking(int i, List<int> domain)
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

            Steps++;

            foreach (int value in domain)
            {
                Colums[i] = value;
                //Console.WriteLine(ToString());
                WriteToLog();
                List<int> nextDomain = MakeDomain(i + 1);
                if (nextDomain.Count > 0) ForwardChecking(i + 1, domain);
            }
        }

        public double BackTrackingH()
        {
            InitNewTest();
            Console.WriteLine("QBTH");
            BackTrackingH(0);
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("QBTH");
            return timeSpan.TotalMilliseconds;
        }

        public void BackTrackingH(int i)
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

            Steps++;

            Random random = new Random(DateTime.Now.GetHashCode());
            List<int> values = new List<int>();
            WriteToLog();
            for (int j = 0; j < Size; j++)
            {
                values.Add(j);
            }

            do
            {
                int valueId = random.Next(0, values.Count);
                Colums[i] = values[valueId];
                values.RemoveAt(valueId);
                //Console.WriteLine(ToString());
                if (Check(i))
                {
                    BackTracking(i + 1);
                }
            } while (values.Count > 0);
        }

        public double ForwardCheckingH()
        {
            InitNewTest();
            Console.WriteLine("QFCH");
            ForwardCheckingH(0, MakeDomain(0));
            TimeSpan timeSpan = DateTime.Now - Time;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString());
            WriteLogToFile("QFCH");
            return timeSpan.TotalMilliseconds;
        }

        private void ForwardCheckingH(int i, List<int> domain)
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

            Steps++;
            domain = ShuffleDomain(domain);
            WriteToLog();

            foreach (int value in domain)
            {
                Colums[i] = value;
                //Console.WriteLine(ToString());
                WriteToLog();
                List<int> nextDomain = MakeDomain(i + 1);
                if (nextDomain.Count > 0) ForwardChecking(i + 1, domain);
            }
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
                    stringBuilder.Append((Colums[j] == i ? "Q " : "0 "));
                }

                stringBuilder.Append("\n");
            }

            stringBuilder.Append("\n");
            return stringBuilder.ToString();
        }

        private List<int> MakeDomain(int i)
        {
            List<int> domain = new List<int>();

            if (i >= Size)
            {
                domain.Add(-1);
            }

            for (int value = 0; value < Size; value++)
            {
                if (CheckValue(i, value)) domain.Add(value);
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
            TextWriter textWriter = File.CreateText(name + Size + "_" + DateTime.Now.GetHashCode() + ".csv");
            textWriter.Write(_log.ToString());
            textWriter.Close();
            textWriter = File.CreateText(name + Size + "_SF_" + DateTime.Now.GetHashCode() + ".csv");
            textWriter.Write(_foundSolutionLog.ToString());
            textWriter.Close();
        }

        private void InitNewTest()
        {
            Steps = 0;
            Solutions = 0;
            Time = DateTime.Now;
            _log = new StringBuilder("Step,Solutions\n");
            _foundSolutionLog = new StringBuilder("Step,Solutions\n");
        }

        private List<int> ShuffleDomain(List<int> domain)
        {
            Random random = new Random(Time.Millisecond);

            for (int i = domain.Count - 1; i > 1; i--)
            {
                int j = random.Next(i + 1);
                int tmp = domain[j];
                domain[j] = domain[i];
                domain[i] = tmp;
            }

            return domain;
        }
    }
}