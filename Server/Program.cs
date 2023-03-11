using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Windows;

var listener = new Socket(AddressFamily.InterNetwork,
    SocketType.Dgram,
    ProtocolType.Udp);


var ip = IPAddress.Parse("127.0.0.1");
var port = 45678;

var buffer = new byte[ushort.MaxValue - 29];
var ep = new IPEndPoint(ip, port);
listener.Bind(ep);


EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);


while (true)
{
    var result = await listener.ReceiveFromAsync(buffer, SocketFlags.None, remoteEp);

    

    var img = TakeScreenShot();

    var imgBuffer = ImageToByte(img);

    Console.WriteLine(imgBuffer.Length);

    var chunk = imgBuffer.Chunk(ushort.MaxValue - 29);

    var newBuffer = chunk.ToArray();

    for (int i = 0; i < newBuffer.Length; i++)
    {
        await Task.Delay(100);
        listener.SendTo(newBuffer[i], result.RemoteEndPoint);
    }
}




byte[] ImageToByte(Image img)
{
    using (var stream = new MemoryStream())
    {
        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        return stream.ToArray();
    }
}


Image TakeScreenShot()
{
    Bitmap memoryImage;
    memoryImage = new Bitmap(1920, 1080);

    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
    memoryGraphics.CopyFromScreen(0, 0, 0, 0, memoryImage.Size);

    return (Image)memoryImage;
}