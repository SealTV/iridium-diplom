using System.Windows.Controls;

namespace Iridium.WPFClient
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for GetGamesControl.xaml
    /// </summary>
    public partial class GetGamesControl : UserControl
    {
        public GetGamesControl()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                var control = new GameControl(i+1);
                control.Click += SelectGameControlOnClick;
                this.GamesListBox.Items.Add(control);
            }
        }

        private void SelectGameControlOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            GameControl control = (GameControl) sender;
            Console.WriteLine(control.Id);
        }

    }
}
