using Graphs;

namespace GraphAlgorithms
{
    public static class Algorithms
    {
        public static void GreedyAlgorithm(Graph graph)
        {
            graph.Nodes.Sort((x, y) => y.Degree.CompareTo(x.Degree));
            List<Node> coloredNodes = new();

            int currentColor = 0;

            foreach (var node in graph.Nodes)
            {
                if (node.Color is null)
                {
                    node.Color = currentColor;
                    coloredNodes.Add(node);

                    foreach (var otherNode in graph.Nodes)
                    {
                        if (otherNode.Color is null)
                        {
                            bool setColor = true;

                            foreach (var item in coloredNodes)
                            {
                                if (item.Neighbors.Contains(otherNode))
                                {
                                    setColor = false;
                                }
                            }

                            if (setColor)
                            {
                                otherNode.Color = currentColor;
                                coloredNodes.Add(otherNode);
                            }
                        }
                    }

                    currentColor++;
                    coloredNodes.Clear();
                }
            }

            graph.Nodes.Sort((x, y) => x.Id.CompareTo(y.Id));
        }

        public static void BacktrackingMRVAlgorithm(Graph graph)
        {
            graph.AssignAvailableColors();

            BacktrackingMRVAlgorithmRecursion(graph);

            graph.DeleteAvailableColors();
        }

        public static void BacktrackingDegreeAlgorithm(Graph graph)
        {
            graph.Nodes.Sort((x, y) => y.Degree.CompareTo(x.Degree));

            BacktrackingDegreeAlgorithmRecursion(graph, 0);

            graph.Nodes.Sort((x, y) => x.Id.CompareTo(y.Id));
        }

        private static bool BacktrackingDegreeAlgorithmRecursion(Graph graph, int currentNodeIndex)
        {
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

                    if (BacktrackingDegreeAlgorithmRecursion(graph, currentNodeIndex + 1))
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

        private static bool BacktrackingMRVAlgorithmRecursion(Graph graph)
        {
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
                        bool res = BacktrackingMRVAlgorithmRecursion(graph);

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
