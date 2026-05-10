using FileEmpty;
using Graphs;

namespace FileMenagingClass;

/// <summary>
/// Provides static methods for reading graph adjacency matrices from files and saving graph data to files.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// Reads an adjacency matrix from a specified text file and rigorously validates its format.
    /// </summary>
    /// <param name="path">The absolute or relative path to the text file containing the matrix.</param>
    /// <returns>A 2D integer array representing the validated adjacency matrix of the graph.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="FileEmptyException">Thrown when the file is empty or contains only whitespace.</exception>
    /// <exception cref="FormatException">Thrown when the matrix is not square, contains non-integer values, or contains values other than 0 and 1.</exception>
    /// <exception cref="InvalidDataException">Thrown when the matrix contains self-loops (non-zero elements on the main diagonal) or is not symmetrical.</exception>
    public static int[,] GetDataFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The file does not exist at specified path");
        }

        var res = File.ReadAllText(path).Trim();

        if (string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res))
        {
            throw new FileEmptyException("The file is empty and does not contain a matrix");
        }

        var lines = res.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length != lines.Length)
            {
                throw new FormatException($"The matrix is not in a correct format. Each row must contain exactly {lines.Length} elements (square matrix required)");
            }
        }

        int[,] matrix = new int[lines.Length, lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var lineOfNumbers = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < lineOfNumbers.Length; j++)
            {
                int number;
                if (!int.TryParse(lineOfNumbers[j], out number))
                {
                    throw new FormatException("The element of matrix is not in a correct format");
                }

                if (number != 0 && number != 1)
                {
                    throw new FormatException("The element of adjacency matrix must be 0 or 1");
                }

                matrix[i, j] = number;
            }
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, i] != 0)
            {
                throw new InvalidDataException("The main diagonal of the matrix must have only 0. No loops allowed");
            }
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = i; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != matrix[j, i])
                {
                    throw new InvalidDataException("The matrix must be symmetrical");
                }
            }
        }

        return matrix;
    }

    /// <summary>
    /// Saves the detailed information of a colored graph to a specified text file.
    /// </summary>
    /// <param name="path">The file path where the graph data will be saved.</param>
    /// <param name="graph">The graph object containing the nodes, their colors, degrees, and adjacency information.</param>
    public static void LoadDataToFile(string path, Graph graph)
    {
        using StreamWriter writer = new StreamWriter(path);

        for (int i = 0; i < graph.Nodes.Count; i++)
        {
            string? color = "";

            if (graph.Nodes[i].Color is null)
            {
                color = "null";
            }
            else
            {
                color = graph.Nodes[i].Color.ToString();
            }

            writer.Write($"#{i}, Degree {graph.Nodes[i].Degree}, Color: {color}, NeighborsID: ");

            for (int j = 0; j < graph.Nodes[i].Neighbors.Count; j++)
            {
                writer.Write($"{graph.Nodes[i].Neighbors[j].Id} ");
            }

            writer.WriteLine();
        }
    }
}
