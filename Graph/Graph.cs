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

        public void DeleteAvailableColors()
        {
            foreach (var node in this.Nodes)
            {
                node.AvailableColors.Clear();
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

        public void Clear()
        {
            this.Nodes.Clear();
        }

        public void GetGraphFromAdjacencyMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                this.Nodes.Add(new Node());
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i < j)
                    {
                        if (matrix[i, j] == 1)
                        {
                            this.Nodes[i].AddNeighbor(this.Nodes[j]);
                        }
                    }
                }
            }
        }

        public int[,] GetAdjacencyMatrix()
        {
            var matrix = MakeEmptyMatrix(this.Nodes.Count);

            for (int i = 0; i < this.Nodes.Count; i++)
            {
                for (int j = 0; j < this.Nodes.Count; j++)
                {
                    if (i < j && this.Nodes[i].Neighbors.Contains(this.Nodes[j]))
                    {
                        matrix[i, j] = 1;
                        matrix[j, i] = 1;
                    }
                }
            }

            return matrix;
        }

        private int[,] MakeEmptyMatrix(int side)
        {
            int[,] matrix = new int[side, side];

            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            
            return matrix;
        }
    }
}
