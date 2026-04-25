using FileEmpty;

namespace FileMenagingClass;

public static class FileManager
{
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
            if (line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length != lines.Length)
            {
                throw new FormatException($"The matrix is not in a correct format. Each row must contain exactly {lines.Length} elements (square matrix required)");
            }
        }

        int[,] matrix = new int[lines.Length, lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var lineOfNumbers = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
}
