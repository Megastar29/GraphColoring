using Graphs;

namespace GraphAlgorithms
{
    public static class Algorithms
    {
        public static void GreedyAlgorithm(Graph graph)
        {
            //graph.Nodes.Sort((x, y) => y.CompareTo(x));
            List<Node> listOfNodes = graph.Nodes.OrderByDescending(x => x.Degree).ToList();

            int currentColor = 0;

            foreach (var node in listOfNodes)
            {
                node.Color = 0;
                node.Color = currentColor;

                foreach (var otherNode in listOfNodes)
                {
                    if (node != otherNode)
                    {

                    }
                }

                currentColor++;
            }
        }
    }
}
