using Graphs;
using System.Collections.Specialized;
using System.Diagnostics;

namespace GraphAlgorithms
{
    public static class Algorithms
    {
        public static (int, long) GreedyAlgorithm(Graph graph)
        {
            int countChecks = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();

            graph.Nodes.Sort((x, y) => y.Degree.CompareTo(x.Degree));

            int currentColor = 0;

            foreach (var node in graph.Nodes)
            {
                if (node.Color is null)
                {
                    node.Color = currentColor;

                    foreach (var otherNode in graph.Nodes)
                    {
                        if (otherNode.Color is null)
                        {
                            bool setColor = true;

                            foreach (var neighbor in otherNode.Neighbors)
                            {
                                countChecks++;
                                if (neighbor.Color == currentColor)
                                {
                                    setColor = false;
                                    break;
                                }
                            }

                            if (setColor)
                            {
                                otherNode.Color = currentColor;
                            }
                        }
                    }

                    currentColor++;
                }
            }

            stopwatch.Stop();

            graph.Nodes.Sort((x, y) => x.Id.CompareTo(y.Id));

            return (countChecks, stopwatch.ElapsedMilliseconds);
        }

        public static (int, long) BacktrackingMRVAlgorithm(Graph graph)
        {
            graph.AssignAvailableColors();

            int totalNodesInTree = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            bool res = BacktrackingMRVAlgorithmRecursion(graph, ref totalNodesInTree);
            stopwatch.Stop();

            graph.DeleteAvailableColors();

            return (totalNodesInTree, stopwatch.ElapsedMilliseconds);
        }

        public static (int, long) BacktrackingDegreeAlgorithm(Graph graph)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            graph.Nodes.Sort((x, y) => y.Degree.CompareTo(x.Degree));

            int totalNodesInTree = 0;

            BacktrackingDegreeAlgorithmRecursion(graph, 0, ref totalNodesInTree);

            stopwatch.Stop();

            graph.Nodes.Sort((x, y) => x.Id.CompareTo(y.Id));

            return (totalNodesInTree, stopwatch.ElapsedMilliseconds);
        }

        private static bool BacktrackingDegreeAlgorithmRecursion(Graph graph, int currentNodeIndex, ref int totalNodesInTree)
        {
            totalNodesInTree++;

            if (graph.IsAllNodesColored())
            {
                return true;
            }

            if (currentNodeIndex >= graph.Nodes.Count)
            {
                return true;
            }

            var node = graph.Nodes[currentNodeIndex];

            for (int color = 0; color < graph.Nodes.Count; color++)
            {
                if (!node.Neighbors.Any(x => x.Color == color))
                {
                    node.Color = color;

                    if (BacktrackingDegreeAlgorithmRecursion(graph, currentNodeIndex + 1, ref totalNodesInTree))
                    {
                        return true;
                    }
                    else
                    {
                        node.Color = null;
                    }
                }
            }

            return false;
        }

        private static bool BacktrackingMRVAlgorithmRecursion(Graph graph, ref int totalNodesInTree)
        {
            totalNodesInTree++;

            if (graph.IsAllNodesColored())
            {
                return true;
            }

            Node? node = ChooseNodeWithMinAvailableColors(graph);

            if (node is null)
            {
                return true;
            }

            foreach (var color in node.AvailableColors)
            {
                if (IsAssignmentPossible(node, color))
                {
                    node.Color = color;

                    List<List<int?>> colorsCopy = new List<List<int?>>();
                    foreach (var element in graph.Nodes)
                    {
                        colorsCopy.Add(element.AvailableColors.ToList());
                    }

                    if (UpdateColors(node))
                    {
                        bool res = BacktrackingMRVAlgorithmRecursion(graph, ref totalNodesInTree);

                        if (res)
                        {
                            return res;
                        }
                    }

                    int counter = 0;
                    node.Color = null;
                    foreach (var element in graph.Nodes)
                    {
                        element.AvailableColors = colorsCopy[counter];
                        counter++;
                    }
                }
            }

            return false;
        }

        private static Node? ChooseNodeWithMinAvailableColors(Graph graph)
        {
            Node? selectedNode = null;
            int minRemainingColors = int.MaxValue;

            foreach (var node in graph.Nodes)
            {
                if (node.Color is null)
                {
                    int remainingColors = node.AvailableColors.Count;

                    if (remainingColors < minRemainingColors)
                    {
                        minRemainingColors = remainingColors;
                        selectedNode = node;
                    }
                }
            }

            return selectedNode;
        }

        private static bool IsAssignmentPossible(Node node, int? color)
        {
            foreach (var neighbor in node.Neighbors)
            {
                if (neighbor.Color == color)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool UpdateColors(Node node)
        {
            foreach(var neighbor in node.Neighbors)
            {
                if (neighbor.AvailableColors.Contains(node.Color))
                {
                    neighbor.AvailableColors.Remove(node.Color);
                }

                if (neighbor.AvailableColors.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
