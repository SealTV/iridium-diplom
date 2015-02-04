using System.Windows;

namespace Iridium.WPFClient
{
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLiginButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(LoginBox.Text);
            Console.WriteLine(PasswordBox.Password);
//            Console.WriteLine(codeEditor.Text);
        }
    }
}
