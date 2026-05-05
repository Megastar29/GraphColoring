using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using FileMenagingClass;
using Graphs;
using System.IO;
using FileEmpty;
using GraphAlgorithms;

namespace GraphicUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AlgorithmNameInitText = "Algorithm name: ";
        private const string TimeInitText = "Time: ";
        private const string PractDifInitText = "Practical difficulty: ";
        private const string StartTime = "--";
        private const string StartPracDif = "--";
        private const string OutputFileName = "Output.txt";
        private const int AlgSelectionNoneIndex = 3;
        private const int InputSelectionNoneIndex = 2;
        private Graph MainGraph = new Graph();
        public const int MaxGraphSize = 20;
        private const int MaxGraphColorVal = MaxGraphSize;
        private SolidColorBrush[] colorBrushes;
        private readonly SolidColorBrush nullColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        public MainWindow()
        {
            InitializeComponent();

            AlgorithmNameText.Text += (AlgSelection.Items[AlgSelectionNoneIndex] as ComboBoxItem)?.Content.ToString();
            TimeText.Text += StartTime;
            PractDifText.Text += StartPracDif;
            this.colorBrushes = new SolidColorBrush[MaxGraphColorVal];
            this.GenerateColors();
        }

        private void GenerateColors()
        {
            double goldenRatioInv = 0.618033988749895;
            double h = 0; // Початковий тон (Hue)

            for (int i = 0; i < this.colorBrushes.Length; i++)
            {
                h += goldenRatioInv;
                h %= 1; // Залишаємося в межах [0; 1]

                // Чергуємо яскравість: один світліший, інший темніший
                double lightness = (i % 2 == 0) ? 0.45 : 0.75;
                // Чергуємо насиченість: один насичений, інший більш пастельний
                double saturation = (i % 3 == 0) ? 0.8 : 0.5;

                // Перетворюємо H (тон) у RGB колір (насиченість 0.6, яскравість 0.6 для гарних кольорів)
                Color color = HslToRgb(h * 360, saturation, lightness);
                this.colorBrushes[i] = new SolidColorBrush(color);
            }
        }
        
        private Color HslToRgb(double h, double s, double l)
        {
            double r, g, b;

            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                r = HueToRgb(p, q, h / 360 + 1.0 / 3);
                g = HueToRgb(p, q, h / 360);
                b = HueToRgb(p, q, h / 360 - 1.0 / 3);
            }

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        private double HueToRgb(double p, double q, double t)
        {
            if (t < 0)
            {
                t += 1;
            }

            if (t > 1)
            {
                t -= 1;
            }

            if (t < 1.0 / 6)
            {
                return p + (q - p) * 6 * t;
            }

            if (t < 1.0 / 2) 
            { 
                return q;
            }

            if (t < 2.0 / 3) 
            {
                return p + (q - p) * (2.0 / 3 - t) * 6;
            }

            return p;
        }

        private void AlgSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgorithmNameText is null)
            {
                return;
            }

            AlgorithmNameText.Text = AlgorithmNameInitText + (AlgSelection.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (MainGraph is not null)
            {
                MainGraph.ClearColors();
                this.GraphCanvas.Children.Clear();
                this.DrawGraph(MainGraph);
            }

            TimeText.Text = TimeInitText + StartTime;
            PractDifText.Text = PractDifInitText + StartPracDif;
            PrintBtn.Visibility = Visibility.Collapsed;
        }

        private void InputSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InputSelection is null)
            {
                return;
            }

            ComboBoxItem selected = InputSelection.SelectedItem as ComboBoxItem;
            if (selected is null)
            {
                return;
            }

            string choice = selected.Content.ToString();
            this.MainGraph?.Clear();

            if (PrintBtn is not null)
            {
                PrintBtn.Visibility = Visibility.Collapsed;
            }            

            switch (choice)
            {
                case "From file":
                    HandleFileImport();
                    break;

                case "By hand":
                    HandleManualInput();
                    break;

                default:
                    this.GraphCanvas.Children.Clear();

                    if (TimeText is not null)
                    {
                        TimeText.Text = TimeInitText + StartTime;
                    }

                    if (PractDifText is not null)
                    {
                        PractDifText.Text = PractDifInitText + StartPracDif;
                    }
                    break;
            }
        }

        private void HandleFileImport()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    int[,] matrix = FileManager.GetDataFromFile(openFileDialog.FileName);

                    if (matrix.GetLength(0) > MaxGraphSize)
                    {
                        throw new InvalidDataException($"The size of graph is too large. It must be between 1 and {MaxGraphSize}");
                    }

                    MainGraph?.GetGraphFromAdjacencyMatrix(matrix);                    

                    if (MainGraph is null)
                    {
                        throw new InvalidDataException("Can't load the graph");
                    }
                    
                    this.DrawGraph(MainGraph);       
                }
                catch (FileEmptyException feex)
                {
                    MessageBox.Show($"File is empty: {feex.Message}");
                    InputSelection.SelectedIndex = 2;
                }
                catch (FileNotFoundException fnfex)
                {
                    MessageBox.Show($"File not found: {fnfex.Message}");
                    InputSelection.SelectedIndex = 2;
                }
                catch (InvalidDataException iex)
                {
                    MessageBox.Show($"Invalid data: {iex.Message}");
                    InputSelection.SelectedIndex = 2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    InputSelection.SelectedIndex = 2;
                }
            }
            else
            {
                InputSelection.SelectedIndex = 2;
            }
        }

        private void HandleManualInput()
        {
            ManualInputWindow inputWindow = new ManualInputWindow();

            if (inputWindow.ShowDialog() == true)
            {
                try
                {
                    var matrix = inputWindow.ResultMatrix;

                    if (matrix is null)
                    {
                        InputSelection.SelectedIndex = 2;
                        throw new InvalidDataException("The matrix is empty");
                    }

                    this.MainGraph?.GetGraphFromAdjacencyMatrix(matrix);

                    if (MainGraph is null)
                    {
                        throw new InvalidDataException("Can't load the graph");
                    }

                    this.DrawGraph(MainGraph);
                }
                catch (InvalidDataException idex)
                {
                    InputSelection.SelectedIndex = 2;
                    MessageBox.Show($"Invalid data: {idex.Message}");
                }
                catch (Exception ex)
                {
                    InputSelection.SelectedIndex = 2;
                    MessageBox.Show($"Error: {ex.Message}");
                }                
            }
            else
            {
                InputSelection.SelectedIndex = 2;
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintBtn.Visibility = Visibility.Collapsed;

            if ((InputSelection.SelectedItem as ComboBoxItem) == (InputSelection.Items[InputSelectionNoneIndex] as ComboBoxItem))
            {
                MessageBox.Show("The graph has not been inputed");
                return;
            }

            if (MainGraph is null || MainGraph.Nodes.Count == 0)
            {
                MessageBox.Show("The graph is empty");
                return;
            }

            ComboBoxItem algorithmItem = AlgSelection.SelectedItem as ComboBoxItem;
            if (algorithmItem is null)
            {
                MessageBox.Show("The null value has been chosen");
                return;
            }

            if (algorithmItem == (AlgSelection.Items[AlgSelectionNoneIndex] as ComboBoxItem))
            {
                MessageBox.Show("The algorithm has not been selected");
                return;
            }

            switch (algorithmItem.Content.ToString())
            {
                case "Greedy algorithm":
                    PrintBtn.Visibility = Visibility.Visible;
                    if (MainGraph.IsAllNodesColored())
                    {
                        MessageBox.Show("The graph is colored");
                        return;
                    }

                    (int countIterations, long timeMs) = Algorithms.GreedyAlgorithm(MainGraph);
                    this.GraphCanvas.Children.Clear();
                    this.DrawGraph(MainGraph);

                    TimeText.Text = TimeInitText + timeMs + " ms";
                    PractDifText.Text = PractDifInitText + "Count of iterations: " + countIterations;
                    break;
                case "Backtracking MRV":
                    PrintBtn.Visibility = Visibility.Visible;
                    if (MainGraph.IsAllNodesColored())
                    {
                        MessageBox.Show("The graph is colored");
                        return;
                    }

                    (int totalNodesInSearchTree, long timeMRVMs) = Algorithms.BacktrackingMRVAlgorithm(MainGraph);
                    this.GraphCanvas.Children.Clear();
                    this.DrawGraph(MainGraph);

                    TimeText.Text = TimeInitText + timeMRVMs + " ms";
                    PractDifText.Text = PractDifInitText + "Count of nodes in search tree: " + totalNodesInSearchTree;
                    break;
                case "Backtracking degree":
                    PrintBtn.Visibility = Visibility.Visible;
                    if (MainGraph.IsAllNodesColored())
                    {
                        MessageBox.Show("The graph is colored");
                        return;
                    }

                    (int totalNodesInDegreeSearchTree, long timeDegreeMs) = Algorithms.BacktrackingDegreeAlgorithm(MainGraph);
                    this.GraphCanvas.Children.Clear();
                    this.DrawGraph(MainGraph);

                    TimeText.Text = TimeInitText + timeDegreeMs + " ms";
                    PractDifText.Text = PractDifInitText + "Count of nodes in search tree: " + totalNodesInDegreeSearchTree;
                    break;
                default:
                    MessageBox.Show("The non algorithm value has been chosen");
                    return;
            }
        }

        private void DrawGraph(Graph graph)
        {
            GraphCanvas.Children.Clear();

            if (graph.Nodes.Count == 0)
            {
                return;
            }

            double centerX = GraphCanvas.ActualWidth / 2;
            double centerY = GraphCanvas.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) - 40;

            var nodePositions = new Dictionary<Node, Point>();
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                double angle = 2 * Math.PI * i / graph.Nodes.Count;
                nodePositions[graph.Nodes[i]] = new Point(centerX + radius * Math.Cos(angle), centerY + radius * Math.Sin(angle));
            }

            foreach (var node in graph.Nodes)
            {
                foreach (var neighbor in node.Neighbors)
                {
                    if (node.Id < neighbor.Id)
                    {
                        Line edge = new Line
                        {
                            X1 = nodePositions[node].X,
                            Y1 = nodePositions[node].Y,
                            X2 = nodePositions[neighbor].X,
                            Y2 = nodePositions[neighbor].Y,
                            Stroke = Brushes.Gray,
                            StrokeThickness = 1.5
                        };
                        GraphCanvas.Children.Add(edge);
                    }
                }
            }

            foreach (var node in graph.Nodes)
            {
                Point pos = nodePositions[node];

                Ellipse circle = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = node.Color.HasValue ? this.colorBrushes[node.Color.Value] : this.nullColor
                };

                TextBlock txt = new TextBlock
                {
                    Text = node.Id.ToString(),
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Grid container = new Grid { Width = 30, Height = 30 };
                container.Children.Add(circle);
                container.Children.Add(txt);

                Canvas.SetLeft(container, pos.X - 15);
                Canvas.SetTop(container, pos.Y - 15);
                GraphCanvas.Children.Add(container);
            }
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileManager.LoadDataToFile(OutputFileName, this.MainGraph);
                MessageBox.Show($"The graph data is loaded to file at {System.IO.Path.GetFullPath(OutputFileName)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}