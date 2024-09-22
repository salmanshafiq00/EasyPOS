using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;


namespace CodeGen;
public partial class FileNameDialog : Window
{
    private const string DEFAULT_TEXT = "Select a entity name";
    private static readonly List<string> _tips = [
        "Tip: CQRS stands for Command/Query Responsibility Segregation, and it's a wonderful thing",
        "Tip: All business logic is in a use case",
        "Tip: Good monolith with clear use cases that you can split in microservices later on, once you’ve learned more about them ",
        "Tip: CI/CD processes and solutions help to generate more value for the end-users of software",
        "Tip: the architecture is decoupled from the underlying data store",
        "Tip: An effective testing strategy that follows the testing pyramid",
    ];

    public FileNameDialog(string folder, string[] entities)
    {
        InitializeComponent();
        lblFolder.Content = string.Format("{0}/", folder);
        foreach (var item in entities)
        {
            selectName.Items.Add(item);
        }
        selectName.Text = DEFAULT_TEXT;
        selectName.SelectionChanged += (s, e) =>
        {
            btnCreate.IsEnabled = true;
        };
        Loaded += (s, e) =>
    {
        BitmapImage icon = new();
        icon.BeginInit();
        icon.UriSource = new Uri("pack://application:,,,/CodeGen;component/Resources/AddApplicationInsights.png");
        icon.EndInit();
        Title = "Code Generation CA";
        SetRandomTip();
    };
    }

    public string Input => selectName.SelectedItem?.ToString();

    private void SetRandomTip()
    {
        Random rnd = new(DateTime.Now.GetHashCode());
        int index = rnd.Next(_tips.Count);
        lblTips.Content = _tips[index];
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
