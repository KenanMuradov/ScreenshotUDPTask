using System.Drawing;
using System.Net.Sockets;
using System.Net;




var listener = new Socket(AddressFamily.InterNetwork,
    SocketType.Dgram,
    ProtocolType.Udp);


var ip = IPAddress.Parse("127.0.0.1");
var port = 45678;

var buffer = new byte[1024];
var ep = new IPEndPoint(ip, port);
listener.Bind(ep);


EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);


while (true)
{
    var result = await listener.ReceiveFromAsync(buffer, SocketFlags.None, remoteEp);

    Console.WriteLine("Hello");

    var img = TakeScreenShot();

    var imgBuffer = ImageToByte(img);

    var chunk = imgBuffer.Chunk(ushort.MaxValue - 29);

    var newBuffer = chunk.ToArray();

    for (int i = 0; i < newBuffer.Length; i++)
        listener.SendTo(newBuffer[i], result.RemoteEndPoint);
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
    memoryImage = new Bitmap(720, 480);

    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
    memoryGraphics.CopyFromScreen(0, 0, 0, 0, memoryImage.Size);

    return (Image)memoryImage;
}