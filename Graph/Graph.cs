namespace Graphs
{
    /// <summary>
    /// Represents a graph data structure consisting of a collection of nodes, used primarily for graph coloring algorithms.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Gets the list of nodes contained within the graph.
        /// </summary>
        public List<Node> Nodes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class with an empty list of nodes.
        /// </summary>
        public Graph()
        {
            this.Nodes = new List<Node>();
        }

        /// <summary>
        /// Initializes the pool of available colors for every node in the graph. 
        /// The maximum number of available colors is assumed to be equal to the total number of nodes.
        /// </summary>
        public void AssignAvailableColors()
        {
            foreach (var node in this.Nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    node.AvailableColors.Add(i);
                }
            }
        }

        /// <summary>
        /// Clears the pool of available colors for all nodes in the graph.
        /// </summary>
        public void DeleteAvailableColors()
        {
            foreach (var node in this.Nodes)
            {
                node.AvailableColors.Clear();
            }
        }

        /// <summary>
        /// Determines whether every node in the graph has been assigned a valid color.
        /// </summary>
        /// <returns><c>true</c> if all nodes have a color assigned; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Removes all nodes from the graph and resets the global static node identifier counter.
        /// </summary>
        public void Clear()
        {
            Node.ResetCounter();
            this.Nodes.Clear();
        }

        /// <summary>
        /// Resets the assigned color of all nodes in the graph to null, leaving the graph structure (nodes and edges) intact.
        /// </summary>
        public void ClearColors()
        {
            foreach (var node in this.Nodes)
            {
                node.Color = null;
            }
        }

        /// <summary>
        /// Constructs the graph's nodes and establishes adjacency relationships (edges) 
        /// based on a provided two-dimensional adjacency matrix.
        /// </summary>
        /// <param name="matrix">A symmetrical 2D integer array representing the adjacency matrix of the graph, where 1 indicates an edge.</param>
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
    }
}
