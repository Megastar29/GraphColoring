using Graphs;

namespace GraphAlgorithms
{
    public static class Algorithms
    {
        public static void GreedyAlgorithm(Graph graph)
        {
            graph.Nodes.Sort((x, y) => y.Degree.CompareTo(x.Degree));
            List<Node> listOfNodes = graph.Nodes.ToList();
            List<Node> NotToDeleteList = listOfNodes.ToList();
            List<Node> coloredNodes = new();

            int currentColor = 0;

            foreach (var node in NotToDeleteList)
            {
                // graph.Nodes inside foreach: if node.Color is null then: 
                node.Color = 0;
                node.Color = currentColor;
                coloredNodes.Add(node);
                listOfNodes.Remove(node);

                foreach (var otherNode in listOfNodes)
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
                        listOfNodes.Remove(otherNode);
                    }
                }

                currentColor++;
            }

            foreach (var node in coloredNodes)
            {
                if (node.Color is null)
                {
                    throw new ArgumentNullException(nameof(node), "The node's color has not been set in Greedy Algorithm");
                }

                graph.SetNodeColor(node.Id, node.Color ?? 0);
            }

            graph.Nodes.Sort((x, y) => x.Id.CompareTo(y.Id));
        }
    }
}
