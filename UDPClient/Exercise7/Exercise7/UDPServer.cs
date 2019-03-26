using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;



namespace Exercise7
{
    public class UDPServer
    {
		static Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        static IPAddress broadcast = IPAddress.Parse("10.0.0.2");
        private const int listenPort = 9000;

        public UDPServer()
        {
			
        }
        
		private static string line;
		public static string UpTime
        {
            get
            {
				try
				{
					using (StreamReader sr = new StreamReader("/~/../proc/uptime"))
					{
						line = sr.ReadToEnd();
						Console.WriteLine($"Uptime: {line}");

					}
				}
				catch(IOException e)
				{
					Console.WriteLine(e.Message);
				}
				return line;
          
            }
        }
        

		public static string CPULoad
        {
            get
            {
                try
                {
                    using (StreamReader sr = new StreamReader("/~/../proc/loadavg"))
                    {
                        line = sr.ReadToEnd();
                        Console.WriteLine($"CPU load: {line}");

                    }
                }
                catch(IOException e)
                {
                    Console.WriteLine(e.Message);
                }
                return line;
          
            }
        }
        


		public static void Send(string request)
        {
            byte[] sendbuf = Encoding.ASCII.GetBytes(request);
            IPEndPoint ep = new IPEndPoint(broadcast, 9000);

            s.SendTo(sendbuf, ep);

            Console.WriteLine($"Message sent to server: {request}");

        }

        public static void StartListener()
        {
            
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
			UdpClient listener = new UdpClient(listenPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for client");

                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received request from {groupEP} :");
					Console.WriteLine($"Message: {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");


					var ReceivedString = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    
					ReceivedString = ReceivedString.ToLower();

					System.Threading.Thread.Sleep(100);

					if(ReceivedString == "u")
					{
						string UptimeInfo = "Uptime: " + UDPServer.UpTime;
						Send(UptimeInfo);

					}
					else if(ReceivedString == "l")
					{
<<<<<<< HEAD:Exercise7/Exercise7/UDPServer.cs
						string CPUload = "CPU load: " + UDPServer.CPULoad;
						Send(CPUload);
=======
						string UptimeInfo = "Data her om loadavg";


                        byte[] data = Encoding.ASCII.GetBytes(UptimeInfo);
                        listener.Send(data, data.Length, groupEP);

						Send(UptimeInfo);
						Console.WriteLine($"Wrote: {UptimeInfo}");
      

>>>>>>> 617c0116f1c1f0b4634362f09af9bfef6e6a4f5c:UDPClient/Exercise7/Exercise7/UDPServer.cs
					}
					else
					{

						string UptimeInfo = "Invalid request";

                        byte[] data = Encoding.ASCII.GetBytes(UptimeInfo);

						string req = "Invalid request";
						Send(req);
						Console.WriteLine($"Wrote: {req}");

					}               
                }
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


    }
}
