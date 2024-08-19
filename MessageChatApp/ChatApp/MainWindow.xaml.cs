using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Collections.Generic;
using System.Windows.Input;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Dictionary<TcpClient, string> clients;
        private string _clientsConnected;
        public MainWindow()
        {
            InitializeComponent();
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8642); // New listener on loopback address
            clients = new Dictionary<TcpClient, string>();

            // events
            Commands.KeyDown += Commands_KeyDown;

            tcpListener.Start(); // Starts Server
            ServerOutput.Text += $"\n({DateTime.Now}) Server Started";


            MainLoop(tcpListener);
        }

        private void Commands_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Commands.Text.Length > 0 && e.Key == Key.Enter)
            {
                string input = Commands.Text;
                switch (input)
                {
                    case "!clients":
                        foreach (var item in clients)
                        {
                            ServerOutput.Text += $"\n({item.Key.Client.RemoteEndPoint} {item.Value})";
                        }
                        
                        break;
                    default:
                        break;
                }
            }
        }

        private async void MainLoop(TcpListener tcpListener)
        {
            while (true)
            {
                // username logic
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                NetworkStream networkStream = tcpClient.GetStream();

                byte[] buffer1 = new byte[1024];
                int byteAmount1 = networkStream.Read(buffer1, 0, buffer1.Length);
                if (byteAmount1 > 0)
                {
                    string username = Encoding.UTF8.GetString(buffer1, 0, byteAmount1);
                    clients.Add(tcpClient, username);

                    // Add clients to the connected list
                    _clientsConnected = "";
                    foreach (KeyValuePair<TcpClient,string> client in clients)
                    {
                        _clientsConnected += $"{client.Value}\n";
                    }
                    ConnectedClients.Text = _clientsConnected;

                    ServerOutput.Text += $"\n({DateTime.Now}) {username}: connected";
                    foreach (KeyValuePair<TcpClient,string> client in clients)
                    {
                        byte[] buffer2 = new byte[1024];
                        buffer2 = Encoding.UTF8.GetBytes($"{username}: connected");
                        NetworkStream networkStream1 = client.Key.GetStream();
                        networkStream1.Write(buffer2, 0, buffer2.Length);
                    }
                }


                Task task = Task.Run(() =>
                {
                    while (tcpClient.Connected)
                    {
                        try
                        {
                            // Receiving Data from client
                            NetworkStream stream = tcpClient.GetStream();

                            byte[] buffer = new byte[1024];

                            int byteAmount = stream.Read(buffer, 0, buffer.Length);
                            if (byteAmount > 0)
                            {
                                string result = Encoding.UTF8.GetString(buffer, 0, byteAmount);
                                ServerOutput.Dispatcher.Invoke(() => { ServerOutput.Text += $"\n({DateTime.Now}) {clients[tcpClient]}: {result}"; });

                                // Resending data back to buffer
                                buffer = Encoding.UTF8.GetBytes(result);

                                // getting client username to send to all other users
                                string username = clients[tcpClient];

                                // Resend data to all clients
                                foreach (KeyValuePair<TcpClient, string> client in clients)
                                {
                                    if (client.Key.Connected)
                                    {
                                        string output1 = $"{username}: {result}";
                                        NetworkStream networkStream1 = client.Key.GetStream();
                                        buffer = Encoding.UTF8.GetBytes(output1);
                                        networkStream1.Write(buffer, 0, buffer.Length);
                                    }
                                }
                            }
                        }
                        catch
                        {
                            ServerOutput.Dispatcher.Invoke(() =>
                            {
                                if (clients.ContainsKey(tcpClient))
                                {
                                    ServerOutput.Text += $"\n({DateTime.Now}) {clients[tcpClient]}: Disconnected";
                                    clients.Remove(tcpClient);
                                    Console.WriteLine("removed client");

                                    // Remove clients from the connected list
                                    _clientsConnected = "";
                                    foreach (KeyValuePair<TcpClient, string> client in clients)
                                    {
                                        _clientsConnected += $"{client.Value}\n";
                                    }
                                    ConnectedClients.Text = _clientsConnected;
                                }
                            });
                            break;
                        }
                    }
                });
            }
        }
    }
}
