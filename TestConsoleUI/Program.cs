using Graphs;
using GraphAlgorithms;
using FileMenagingClass;

namespace TestConsoleUI
{
    internal class Program
    {
        private static void DisplayGraph(Graph graph)
        {
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                string? color = "";

                if (graph.Nodes[i].Color is null)
                {
                    color = "null";
                }
                else
                {
                    color = graph.Nodes[i].Color.ToString();
                }

                Console.Write($"#{i}, ID: {graph.Nodes[i].Id}, Degree {graph.Nodes[i].Degree}, Color: {color}, NeighborsID: ");

                for (int j = 0; j < graph.Nodes[i].Neighbors.Count; j++)
                {
                    Console.Write($"{graph.Nodes[i].Neighbors[j].Id} ");
                }

                Console.WriteLine();
            }
        }

        private static void DisplayMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public static void Main(string[] args)
        {
            Graph graph = new Graph();

            const string path = /*@"C:\Users\Lenovo\data.txt";*/@"D:\КПІ\ОП_Курсова\TestGraph.txt";
            int[,] matrix = FileManager.GetDataFromFile(path);

            graph.GetGraphFromAdjacencyMatrix(matrix);
            // create 6 nodes
            //for (int i = 0; i < 6; i++)
            //{
            //    graph.AddNode(new Node());
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    graph.AddNode(new Node());
            //}

            // link nodes
            //graph.Nodes[0].AddNeighbor(graph.Nodes[1]);
            //graph.Nodes[0].AddNeighbor(graph.Nodes[3]);
            //graph.Nodes[0].AddNeighbor(graph.Nodes[4]);

            //graph.Nodes[1].AddNeighbor(graph.Nodes[2]);
            //graph.Nodes[1].AddNeighbor(graph.Nodes[5]);

            //graph.Nodes[2].AddNeighbor(graph.Nodes[3]);
            //graph.Nodes[2].AddNeighbor(graph.Nodes[5]);

            //graph.Nodes[4].AddNeighbor(graph.Nodes[5]);

            //graph.Nodes[0].AddNeighbor(graph.Nodes[1]);
            //graph.Nodes[0].AddNeighbor(graph.Nodes[2]);
            //graph.Nodes[1].AddNeighbor(graph.Nodes[2]);
            //graph.Nodes[2].AddNeighbor(graph.Nodes[3]);

            //Algorithms.GreedyAlgorithm(graph);
            //Algorithms.BacktrackingMRVAlgorithm(graph);
            var difficulty = Algorithms.BacktrackingDegreeAlgorithm(graph);

            DisplayGraph(graph);
            Console.WriteLine($"Difficulty: {difficulty}");
            Console.WriteLine();
            DisplayMatrix(graph.GetAdjacencyMatrix());


            FileManager.LoadDataToFile(@"C:\Users\Lenovo\dataGraph.txt", graph);

            //Graph graph2 = new();
            //graph2.GetGraphFromAdjacencyMatrix(graph.GetAdjacencyMatrix());
            //Console.WriteLine();
            //DisplayGraph(graph2);
        }
    }
}
