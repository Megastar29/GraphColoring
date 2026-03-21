namespace Graphs
{
    public class Graph
    {
        public List<Node> Nodes { get; private set; }

        public Graph()
        {
            this.Nodes = new List<Node>();
        }

        public Graph(List<Node> nodes)
        {
            ArgumentNullException.ThrowIfNull(nodes, nameof(nodes));

            this.Nodes = nodes;
        }

        public void AddNode(Node node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));

            this.Nodes.Add(node);
        }

        public void GetGraphFromAdjacencyMatrix(int[,] matrix)
        {
            throw new NotImplementedException();
        }

        public int[,] GetAdjacencyMatrix()
        {
            throw new NotImplementedException();
        }
    }
}
