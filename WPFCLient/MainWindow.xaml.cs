namespace Iridium.WPFClient
{
    using System;
    using System.Text;
    using System.Windows;

    using Iridium.Utils.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ClientClass client;


        public MainWindow()
        {
            InitializeComponent();
            this.StartPage.LogginButton.Click+=LogginButtonOnClick;
            this.GamesControl.Visibility = Visibility.Collapsed;
            this.LevelsControl.Visibility = Visibility.Collapsed;
            this.CodeEditControl.Visibility = Visibility.Collapsed;
        }

        private void LogginButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.StartPage.IsEnabled = false;
            this.Login();
        }

        private async void Login()
        {

            this.client = new ClientClass(27001, "104.40.216.136");
            PacketsFromMaster.LoginResult result = null;
            try
            {
                this.client.Connect();
                Packet packet = await this.client.ReadNextPacket();
                Console.WriteLine("Server info: {0}", ((PacketsFromMaster.ServerInfo)packet).ServerVersion);
                this.client.SendPacket(new PacketsFromClient.Login(this.StartPage.LoginBox.Text, Encoding.UTF8.GetBytes(
                                                                                                 this.StartPage
                                                                                                     .PasswordBox
                                                                                                     .Password)));
                packet = await this.client.ReadNextPacket();
                result = packet as PacketsFromMaster.LoginResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (result == null)
                {
                    this.StartPage.IsEnabled = true;
                }
                else
                {
                    LoadGames();
                }             
            }
        }

        private async void LoadGames()
        {
            PacketsFromMaster.GamesList result = null;
            try
            {
                this.client.Connect();
                this.client.SendPacket(new PacketsFromClient.GetGames());
                var packet = await this.client.ReadNextPacket();
                result = packet as PacketsFromMaster.GamesList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (result == null)
                {
                    this.StartPage.IsEnabled = true;
                }
                else
                {
                    this.StartPage.Visibility = Visibility.Collapsed;
                    this.GamesControl.SetGames(result.Games);
                    this.GamesControl.Visibility = Visibility.Visible;                    
                }             
            }
        }
    }
}
