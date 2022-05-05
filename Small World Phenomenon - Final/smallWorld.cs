using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace Small_World_Phenomenon
{
    class smallWorld
    {
        static string src ="", dest="";
        static Dictionary<string, string> parent = new Dictionary<string, string>();

        public static Dictionary<string, List<string>> makeTheGraph(List<List<string>> MovieActors)
        {
            Dictionary<string, List<string>> adjList = new Dictionary<string, List<string>>();
            foreach (var movie in MovieActors)
            {
                for (int i = 0; i < movie.Count; i++)
                {
                    if (!adjList.ContainsKey(movie[i]))
                        adjList.Add(movie[i], new List<string>());

                    for (int j = i + 1; j < movie.Count; j++)
                    {
                        if (!adjList.ContainsKey(movie[j]))
                            adjList.Add(movie[j], new List<string>());

                        if (!adjList[movie[i]].Contains(movie[j]))
                            adjList[movie[i]].Add(movie[j]);
                        if (!adjList[movie[j]].Contains(movie[i]))
                            adjList[movie[j]].Add(movie[i]);


                    }
                }
            }
            return adjList;
        }
        public static List<string> getBestPathFinal(Dictionary<string, List<string>> adjList, Dictionary<string, HashSet<string>> ActorMoviesList)
        {
            Queue<string> pqNext = new Queue<string>();
            Queue<string> pq = new Queue<string>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            Dictionary<string, int> Strength = new Dictionary<string, int>();
            Dictionary<string, int> level = new Dictionary<string, int>();


            pq.Enqueue(src);
            parent[src] = "-1";
            level[src] = 0;
            Strength[src] = 0;
            bool found = false;


            while (pq.Count != 0)
            {
                string node = pq.Dequeue();
                visited[node] = true;
                foreach (string child in adjList[node])
                {
   
                    if (!level.ContainsKey(child))
                        level[child] = 9999;
                    if (!visited.ContainsKey(child))
                        visited[child] = false;
                    if (!Strength.ContainsKey(child))
                        Strength[child] = -9999;

                    if (child == dest)
                        found = true;
                   
                    if (!visited[child] && (level[child] > level[node]))
                    {

                        level[child] = level[node] + 1;
                        int strength = CalcStrength(node, child, ActorMoviesList) + Strength[node];
                        if (strength > Strength[child])
                        {
                            Strength[child] = strength;
                            parent[child] = node;
                        }
                        pqNext.Enqueue(child);

                    }
                }
                if (pq.Count == 0)
                {
                    pq = pqNext;
                    pqNext = new Queue<string>();

                    if (found)
                        break;
                }
            }
            List<string> path = new List<string>();
            String cur = dest;
            while (cur != "-1")
            {
                path.Add(cur);
                cur = parent[cur];

            }
            path.Reverse();


            return path;





        }
        public static int CalcStrength(string a, string b, Dictionary<string, HashSet<string>> ActorMoviesList)
        {
            List<string> movies = new List<string>();
            string A = a, B = b;
            if (ActorMoviesList[a].Count > ActorMoviesList[b].Count)
            {
                A = b;
                B = a;
            }

            foreach (var movie in ActorMoviesList[A])
            {

                if (ActorMoviesList[B].Contains(movie))
                    movies.Add(movie);
            }
            return movies.Count;
        }
        public static List<List<string>> getAllMoviesInPath(List<string> path, Dictionary<string, HashSet<string>> ActorMoviesList)
        {

            int relationStrength = 0;
            List<List<string>> moviesInPath = new List<List<string>>();
            for (int i = 1; i < path.Count; i++)
            {
                List<string> moviesBetweenTwoActors = containsBoth(path[i], path[i - 1], ActorMoviesList);
                moviesInPath.Add(moviesBetweenTwoActors);
            }
            return moviesInPath;
        }
        public static List<string> containsBoth(string a, string b, Dictionary<string, HashSet<string>> ActorMoviesList)
        {
            List<string> movies = new List<string>();
            string A = a, B = b;
            if (ActorMoviesList[a].Count > ActorMoviesList[b].Count)
            {
                A = b; B = a;
            }

            foreach (var movie in ActorMoviesList[A])
            {
                if (ActorMoviesList[B].Contains(movie))
                    movies.Add(movie);

            }
            return movies;
        }
        public static string answerString(List<List<string>> moviesInPath, List<string> BestPath)
        {
            //calc rs 
            int RS = 0;
            string chainOfMovies = "";
            string chainOfActors = ""; 
            foreach (var ListOfMovies in moviesInPath)
            {
                RS += ListOfMovies.Count;
                chainOfMovies += ListOfMovies[0] + " => ";
            }
            StringBuilder sb = new StringBuilder(chainOfMovies);
            sb.Remove(chainOfMovies.Length-1, 1);
            chainOfMovies = sb.ToString();


            int DoS = BestPath.Count - 1;
            for (int i =0; i < BestPath.Count-1; i++)
            {
                chainOfActors+=BestPath[i]+ " -> ";

            }
            chainOfActors += BestPath[BestPath.Count - 1];
            string answer = ""; 
            answer+=src+"/"+dest+"\n"+"DoS = "+DoS+", RS = "+RS+"\n"+ "CHAIN OF ACTORS: "+chainOfActors+"\n" + "CHAIN OF MOVIES:  => "+ chainOfMovies+"\n" ;
            return answer; 


        }
        public static void solve(List<List<string>> MovieActors, List<KeyValuePair<string, string>> queries, Dictionary<string, HashSet<string>> ActorMoviesList , Queue<string> Answers )
        {

            Dictionary<string, List<string>> adjList = makeTheGraph(MovieActors);
            int i = 1;

            List<string> path;
            List<List<string>> MoviesInPath;

            foreach (var query in queries)
            {
                src = query.Key; dest = query.Value;
                path = getBestPathFinal(adjList, ActorMoviesList);
                MoviesInPath = getAllMoviesInPath(path, ActorMoviesList);
                string ans = answerString(MoviesInPath, path);
                string expected = Answers.Dequeue();
                if (ans == expected)
                {
                    Console.WriteLine("Test #" + i + "Passed!");
                }
                else
                {
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("Wrong answer on Test #" + i);
                    Console.WriteLine("Output :");
                    Console.WriteLine(ans);
                    Console.WriteLine();
                    Console.WriteLine("Expected Output :");
                    Console.WriteLine(expected);
                    Console.WriteLine("------------------------------------");

                }
           
                i++;
            }
        }

    }
}

