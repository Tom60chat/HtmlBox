using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HtmlBox.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void Start()
        {
            //var htmlFile = await GetHtmlFileAsync();
            var htmlPath = await GetHtmlPathAsync();
            var elements = Converter.ConvertToUI(htmlPath);

            LogicalChildren.Clear();
            LogicalChildren.Add(elements);
        }

        private async Task<string> GetHtmlFileAsync()
        {
            try
            {
                var args = Environment.GetCommandLineArgs();

                if (args.Length > 1)
                {
                    string htmlFilePath = args[1];

                    return File.ReadAllText(htmlFilePath);
                }
                else
                {
                    OpenFileDialog openFileDialog = new();
                    openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Web page", Extensions = { "html" } });
                    openFileDialog.Title = "Choose a web page";

                    var files = await openFileDialog.ShowAsync(this);

                    if (files == null)
                        Environment.Exit(0);

                    return File.ReadAllText(files[0]);
                }
            }
            catch (Exception e)
            {
                var messageBoxStandardWindow = MessageBoxManager
                    .GetMessageBoxStandardWindow("Exception", e.Message, MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error);
                await messageBoxStandardWindow.Show();

                Environment.Exit(1);
                return string.Empty;
            }
        }

        private async Task<string> GetHtmlPathAsync()
        {
            try
            {
                var args = Environment.GetCommandLineArgs();

                if (args.Length > 1)
                {
                    return args[1];;
                }
                else
                {
                    OpenFileDialog openFileDialog = new();
                    openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Web page", Extensions = { "html" } });
                    openFileDialog.Title = "Choose a web page";

                    var files = await openFileDialog.ShowAsync(this);

                    if (files == null)
                        Environment.Exit(0);

                    return files[0];
                }
            }
            catch (Exception e)
            {
                var messageBoxStandardWindow = MessageBoxManager
                    .GetMessageBoxStandardWindow("Exception", e.Message, MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error);
                await messageBoxStandardWindow.Show();

                Environment.Exit(1);
                return string.Empty;
            }
        }
    }
}
