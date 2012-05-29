using System.Threading;
using STLK;

namespace RGBLampGateway
{
    public class Program
    {
        private static XBee _xBee;

        public static void Main()
        {
            new Listener(RequestReceived);

            _xBee = new XBee();
            _xBee.FrameReceived += XBeeFrameReceived;

            Thread.Sleep(Timeout.Infinite);
        }

        static void XBeeFrameReceived(byte[] data)
        {
            if (data.Length == 0)
            {
                return;
            }

            switch (data[0])
            {
                case 25:

                    break;
            }
        }

        static void RequestReceived(Request request)
        {
            string[] param = request.URL.Split('/');

            switch (param[1])
            {
                case "fade":
                    if (param.Length == 3)
                    {
                        short fadeDelay = short.Parse(param[2]);
                        _xBee.SendData(0x0013A200407A26AA, 0xFFFE, new byte[] { 20, (byte)(fadeDelay >> 8), (byte)(fadeDelay & 0x00FF) }, null);
                    }
                    break;
                case "mode":
                    if (param.Length == 3)
                    {
                        byte fadeMode = byte.Parse(param[2]);
                        _xBee.SendData(0x0013A200407A26AA, 0xFFFE, new byte[] { 21, fadeMode }, null);
                    }
                    break;
                case "color":
                    if (param.Length == 3)
                    {
                        byte color = byte.Parse(param[2]);
                        _xBee.SendData(0x0013A200407A26AA, 0xFFFE, new byte[] { 22, color }, null);
                    }
                    break;
                case "colorx":
                    if (param.Length == 5)
                    {
                        byte colorR = byte.Parse(param[2]);
                        byte colorG = byte.Parse(param[3]);
                        byte colorB = byte.Parse(param[4]);
                        _xBee.SendData(0x0013A200407A26AA, 0xFFFE, new byte[] { 23, colorR, colorG, colorB }, null);
                    }
                    break;
                case "status":
                    _xBee.SendData(0x0013A200407A26AA, 0xFFFE, new byte[] { 24 }, null);
                    break;
            }

            request.SendResponse("Response({'Sucess':'True'});");
        }
    }
}