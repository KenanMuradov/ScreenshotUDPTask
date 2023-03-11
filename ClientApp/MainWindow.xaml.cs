using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp;

public partial class MainWindow : Window
{
    private Socket client;
    private EndPoint remoteEP;

    public MainWindow()
    {
        InitializeComponent();

        client = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);


        var ip = IPAddress.Parse("127.0.0.1");
        var port = 45678;
        remoteEP = new IPEndPoint(ip, port);

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var buffer = new byte[ushort.MaxValue - 29];
        client.SendTo(buffer, remoteEP);

        while (true)
        {
            var len = client.ReceiveFrom(buffer, SocketFlags.None, ref remoteEP);


        }
    }
}
