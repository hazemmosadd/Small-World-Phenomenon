using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Small_World_Phenomenon
{
    class Program
    {
        static string moviesPath = "", queriesPath = "" , solutionPath = "" , sl;
        static Queue<string > answers = new Queue<string>();


        public static void parseSolution()
        {
            StreamReader reader = new StreamReader(solutionPath);
            string answer = "";
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                    break;
                if (line == "")
                {
                    answers.Enqueue(answer);
                    answer = "";
                }
                else
                {
                    answer += line + "\n";
                }

            }
            Console.WriteLine("Solution Parsing Done, {0} answers", answers.Count());
        }
        static void menu()
        {
            Console.WriteLine("==========================");
            Console.WriteLine("     CHOOSE TESTCASE");
            Console.WriteLine("==========================");
            Console.WriteLine(" ");
            Console.WriteLine("   ------   Small    --------");

            Console.WriteLine(" 1  - 139 Movies 110 Quieries");
            Console.WriteLine(" 2  - 187 Movies 50 Quieries");
            Console.WriteLine(" ");
            Console.WriteLine("   ------   Medium    --------");
            Console.WriteLine(" 3  - 967 Movies 85 Quieries");
            Console.WriteLine(" 4  - 967 Movies 4000 Quieries");
            Console.WriteLine(" 5  - 4736 Movies 110 Quieries");
            Console.WriteLine(" 6  - 4736 Movies 2000 Quieries");
            Console.WriteLine(" ");
            Console.WriteLine("   ------   Large    --------");
            Console.WriteLine(" 7  - 14129 Movies 26 Quieries");
            Console.WriteLine(" 8  - 14129 Movies 600 Quieries");
            Console.WriteLine(" ");

            Console.WriteLine("   ------  Extreme   --------");
            Console.WriteLine(" 9  - 122806 Movies 22 Quieries");
            Console.WriteLine(" 10 - 122806 Movies 200 Quieries");



            int choice = 0;
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    moviesPath = @"small\Case1\Movies193.txt";
                    queriesPath = @"small\Case1\queries110.txt";
                    sl = @"small\Case1\queries110 - Solution.txt";
                    break;

                case 2:
                    moviesPath = @"small\Case2\Movies187.txt";
                    queriesPath = @"small\Case2\queries50.txt";
                    sl = @"small\Case2\queries50 - Solution.txt";
                    break;

                case 3:
                    moviesPath = @"medium\Case1\Movies967.txt";
                    queriesPath = @"medium\Case1\queries85.txt";
                    sl = @"medium\Case1\Solutions\queries85 - Solution.txt";
                    break;

                case 4:
                    moviesPath = @"medium\Case1\Movies967.txt";
                    queriesPath = @"medium\Case1\queries4000.txt";
                    sl = @"medium\Case1\Solutions\queries4000 - Solution.txt";
                    break;

                case 5:
                    moviesPath = @"medium\Case2\Movies4736.txt";
                    queriesPath = @"medium\Case2\queries110.txt";
                    sl = @"medium\Case2\Solutions\queries110 - Solution.txt";
                    break;
                case 6:
                    moviesPath = @"medium\Case2\Movies4736.txt";
                    queriesPath = @"medium\Case2\queries2000.txt";
                    sl = @"medium\Case2\Solutions\queries2000 - Solution.txt";
                    break;

                case 7:
                    moviesPath = @"large\Movies14129.txt";
                    queriesPath = @"large\queries26.txt";
                    sl = @"\large\Solutions\queries26 - Solution.txt";
                    break;
                case 8:
                    moviesPath = @"large\Movies14129.txt";
                    queriesPath = @"large\queries600.txt";
                    sl = @"\large\Solutions\queries600 - Solution.txt"; 
                    break;
                case 9:
                    moviesPath = @"extreme\Movies122806.txt";
                    queriesPath = @"extreme\queries22.txt";
                    sl = @"extreme\Solutions\queries22 - Solution.txt";
                    

                    break; 
                case 10:
                    moviesPath = @"extreme\Movies122806.txt";
                    queriesPath = @"extreme\queries200.txt";
                    sl = @"extreme\Solutions\queries200 - Solution.txt";
                    break;
            }
        }

        static void Main(string[] args)
        {

            menu();

             solutionPath = @"C:\Users\LENOVO\source\repos\Small World Phenomenon\Small World Phenomenon\Testcases\Complete\" + sl; 
            parseSolution();



            string[] lines = File.ReadAllLines(@"C:\Users\LENOVO\source\repos\Small World Phenomenon\Small World Phenomenon\Testcases\Complete\" + moviesPath, Encoding.UTF8);
            List<List<string>> MovieActors = new List<List<string>>();
            Dictionary<string, HashSet<string>> Actor_Movies = new Dictionary<string, HashSet<string>>();
            PriorityQueue<string, int> queue = new PriorityQueue<string, int>();
            int x = 0;
            foreach (var line in lines)
            {

                var Line = line.Split("/");
                MovieActors.Add(new List<string>());
                for (int i = 1; i < Line.Length; i++)
                {
                    MovieActors[x].Add(Line[i]);
                    if (!Actor_Movies.ContainsKey(Line[i]))
                        Actor_Movies.Add(Line[i], new HashSet<string>());
                    Actor_Movies[Line[i]].Add(Line[0]);
                }
                x++;
            }

            string[] Qlines = File.ReadAllLines(@"C:\Users\LENOVO\source\repos\Small World Phenomenon\Small World Phenomenon\Testcases\Complete\" + queriesPath, Encoding.UTF8);
            var queries = new List<KeyValuePair<string, string>>();
            foreach (var line in Qlines)
            {
                string[] lineParts = line.Split('/');
                queries.Add(new KeyValuePair<string, string>(lineParts[0], lineParts[1]));
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            smallWorld.solve(MovieActors, queries, Actor_Movies , answers);
            stopwatch.Stop();
            Console.WriteLine("Elapsed Time : " +(float) stopwatch.ElapsedMilliseconds / 1000  +" second");





        }
    }
}
