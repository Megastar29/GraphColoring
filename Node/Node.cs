using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Graphs
{
    /// <summary>
    /// Represents a single vertex (node) within a graph, containing its state and adjacency information for graph coloring algorithms.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets or sets the list of adjacent nodes connected to this node by an edge.
        /// </summary>
        public List<Node> Neighbors { get; set; }

        // <summary>
        /// Gets or sets the assigned color of the node. A value of null indicates that the node is currently uncolored.
        /// </summary>
        public int? Color { get; set; }

        /// <summary>
        /// Gets or sets the degree of the node, which represents the total number of its neighbors.
        /// </summary>
        public int Degree { get; set; }

        /// <summary>
        /// Gets the unique identifier for this node.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the list of colors currently available for this node to be colored with. 
        /// Used primarily by algorithms employing Forward Checking and the MRV heuristic.
        /// </summary>
        public List<int?> AvailableColors { get; set; }

        private static int counter = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class, setting default values and assigning a unique ID.
        /// </summary>
        public Node()
        {
            this.Neighbors = new List<Node>();
            this.AvailableColors = new List<int?>();
            this.Color = null;
            this.Degree = 0;
            this.Id = counter;
            counter++;
        }

        /// <summary>
        /// Establishes a bidirectional edge (adjacency) between this node and the specified node.
        /// </summary>
        /// <param name="node">The target node to connect with.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="node"/> is null.</exception>
        public void AddNeighbor(Node node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));

            if (this.Neighbors.Contains(node))
            {
                return;
            }

            this.Degree++;
            this.Neighbors.Add(node);
            node.AddNeighbor(this);
        }

        /// <summary>
        /// Resets the global static ID counter to zero. 
        /// This should be called when clearing a graph to ensure the next created nodes start from ID 0.
        /// </summary>
        public static void ResetCounter()
        {
            Node.counter = 0;
        }
    }
}
