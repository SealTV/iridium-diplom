using System.Windows.Controls;

namespace Iridium.WPFClient
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        public event RoutedEventHandler Click;
        public readonly int Id;

        public GameControl()
        {
            InitializeComponent();
            this.Id = -1;
            Console.WriteLine("asdad");
        }

        public GameControl(int id)
        {
            InitializeComponent();
            this.Id = id;
            Console.WriteLine(id);
        }

        private void ButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Console.WriteLine("Clicked!");
            if (this.Click != null)
            {
                Click.Invoke(this, routedEventArgs);
            }
        }
    }
}
