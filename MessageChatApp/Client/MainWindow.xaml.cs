using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly TcpClient tcpClient = new TcpClient();
        NetworkStream stream;
        private bool _isConnected = false;
        private string _username;
        public MainWindow()
        {
            InitializeComponent();
            ConnectButton.Click += HandleConnect;
            TB.KeyDown += TB_KeyDown;
            Application.Current.Exit += OnApplicationQuit;
            Task.Run(() =>
            {
                MainLoop();
            });
        }

        private void OnApplicationQuit(object sender, EventArgs e)
        {
            // notify when a user disconnects to all clients
            byte[] buffer = new byte[1024];
            buffer = Encoding.UTF8.GetBytes("Disconnected");
            stream.Write(buffer, 0, buffer.Length);

            _isConnected = false;
        }
        private void HandleConnect(object sender, RoutedEventArgs e)
        {
            if (!_isConnected && UserNameBox.Text.Length > 0)
            {
                try
                {
                    tcpClient.Connect("127.0.0.1", 8642); // Connecting client to server
                    Chat.Text += $"\nConnected to server.";
                    stream = tcpClient.GetStream();
                    _isConnected = true;

                    // Username logic
                    _username = UserNameBox.Text;
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.UTF8.GetBytes(_username);
                    stream.Write(buffer, 0, buffer.Length);

                    // hide username textbox
                    UserNameBox.Visibility = Visibility.Collapsed;

                    ConnectButton.Content = "Disconnect";
                }
                catch
                {
                    MessageBox.Show("Server is off", "Error");
                    ConnectButton.Content = "Connect";
                }
            }
            else
            {
                ConnectButton.Click -= HandleConnect;
                TB.KeyDown -= TB_KeyDown;
                Application.Current.Shutdown();

                // notify when a user disconnects to all clients
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes("Disconnected");
                stream.Write(buffer, 0, buffer.Length);

                _isConnected = false;
            }
        }
        private bool HasText()
        {
            bool result = (TB.Text.Length > 0 ? true : false);
            return result;
        }
        private async Task SendMessageAsnyc()
        {
            if (_isConnected && HasText())
            {
                Console.WriteLine("Sent Message");
                stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(TB.Text);
                stream.Write(buffer, 0, buffer.Length);
                TB.Text = ""; // empty textbox when user sends a message

                await Task.Yield();
            }
            else
            {
                MessageBox.Show("Client not connected or no text in field", "Error");
            }
        }

        private void TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Task task = SendMessageAsnyc();
            }
        }
        private void MainLoop()
        {
            Console.WriteLine("Main loop started");
            while (true)
            {
                while (_isConnected)
                {
                    try
                    {
                        if (_isConnected)
                        {
                            stream = tcpClient.GetStream();
                            byte[] buffer = new byte[1024];
                            int byteAmount = stream.Read(buffer, 0, buffer.Length);
                            if (byteAmount > 0)
                            {
                                string output = Encoding.UTF8.GetString(buffer);
                                Chat.Dispatcher.Invoke(() => { Chat.Text += $"\n({DateTime.Now}) {output}"; });
                                Console.WriteLine("message written");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"aborted: {e.Message}");
                        break;
                    }

                }
            }
        }
    }
}
