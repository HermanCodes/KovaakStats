using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;

namespace KovaakStats
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> scenarioNames = getScenarioNames();

            foreach (var scenario in scenarioNames)
            {
                List<string> stats = getStatistics("Score", scenario);

                Console.WriteLine("\n"+scenario+":\n");
                Console.WriteLine($"    {stats[0]}");
                Console.WriteLine($"    {stats[1]}");
                Console.WriteLine($"    {stats[2]}");
            }

            Console.ReadLine();
        }

        public static List<string> getScenarioNames()
        {
            List<string> filePaths = new List<string> { };
            string[] newFilePath = new string[3];
            string[] files = Directory.GetFiles(@"D:\SteamLibrary\steamapps\common\FPSAimTrainer\FPSAimTrainer\stats");

            foreach (var file in files)
            {
                bool fileTypeRecorded;

                var fileName = Path.GetFileName(file);
                newFilePath = fileName.Split('-');

                if (filePaths.Contains(newFilePath[0]))
                {
                    fileTypeRecorded = true;
                }
                else
                {
                    fileTypeRecorded = false;
                }

                if (fileTypeRecorded != true)
                {
                    filePaths.Add(newFilePath[0]);
                }
            }

            return filePaths;
        }

        public static List<string> getStatistics(string searchTerm, string filePath)
        {
            string[] files = Directory.GetFiles(@"D:\SteamLibrary\steamapps\common\FPSAimTrainer\FPSAimTrainer\stats", $"{filePath}*");

            List<double> stats = new List<double> {};
            string[] temp = new string[2];

            foreach (var file in files)
            {
                string stat = readRecord(searchTerm, file);
                temp = stat.Split(',');
                var statNumber = Math.Round(Convert.ToDouble(temp[1]), 2);
                stats.Add(statNumber);
            }

            List<string> retList = new List<string> 
            {
                $"Best {searchTerm}: " + Convert.ToString(stats.Max()),
                $"Worst {searchTerm}: " + Convert.ToString(stats.Min()),
                $"Average {searchTerm}: " + Convert.ToString(Math.Round(stats.Average(), 2))
            };

            return retList;
        }

        public static string readRecord(string searchTerm, string filePath)
        {
            string recordNotFound = "Record Not Found";

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    if (line.Contains(searchTerm))
                    {
                        return line;
                    }
                }

                return recordNotFound;
            }
            catch (Exception ex)
            {
                Console.WriteLine("This program did an oopsie");
                return recordNotFound;
                throw new ApplicationException("This program did an oopsie :", ex);
            }
        }
    }
}
