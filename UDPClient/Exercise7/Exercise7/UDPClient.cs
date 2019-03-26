using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Exercise7
{
    public class UDPClient
    {
		Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPAddress broadcast = IPAddress.Parse("10.0.0.1");
		private const int listenPort = 9000;

		public static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {

                    Console.WriteLine("Waiting for server");

                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received request from {groupEP} :");
                    Console.WriteLine($"Message:"); 
				    Console.WriteLine($"{Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
				Console.WriteLine("");
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }

        public UDPClient()
        {
			
        }

        public void Send(string request)
		{
			
			byte[] sendbuf = Encoding.ASCII.GetBytes(request);
            IPEndPoint ep = new IPEndPoint(broadcast, 9000);

            s.SendTo(sendbuf, ep);

            Console.WriteLine("Message sent to server");


		}
    }
}
