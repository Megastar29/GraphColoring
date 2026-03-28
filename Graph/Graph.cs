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

        public void SetNodeColor(int id, int color)
        {
            foreach (var node in this.Nodes)
            {
                if (node.Id == id)
                {
                    node.Color = color;
                }
            }
        }

        public void AssignAvailableColors()
        {
            foreach(var node in this.Nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    node.AvailableColors.Add(i);
                }
            }
        }

        public bool IsAllNodesColored()
        {
            foreach (var node in this.Nodes)
            {
                if (node.Color is null)
                {
                    return false;
                }
            }

            return true;
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
