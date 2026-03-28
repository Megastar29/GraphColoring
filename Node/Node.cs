using System.Collections.Generic;

namespace Graphs
{
    public class Node
    {
        public List<Node> Neighbors { get; set; }

        public int? Color { get; set; }

        public int Degree { get; set; }

        public int Id { get; private set; }

        public List<int?> AvailableColors { get; set; }

        private static int counter = 0;

        public Node()
        {
            this.Neighbors = new List<Node>();
            this.AvailableColors = new List<int?>();
            this.Color = null;
            this.Degree = 0;
            this.Id = counter;
            counter++;
        }

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

        public bool IsColored()
        {
            if (this.Color is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
