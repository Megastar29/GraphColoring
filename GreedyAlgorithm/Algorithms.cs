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
    }
}
