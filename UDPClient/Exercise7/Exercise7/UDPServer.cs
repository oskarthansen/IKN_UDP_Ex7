using System;
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
        

		public static TimeSpan UpTime
        {
            get
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();       //Call this an extra time before reading its value
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
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
                    
					if(ReceivedString == "u")
					{
						string UptimeInfo = UDPServer.UpTime.ToString();
						Send(UptimeInfo);
						Console.WriteLine($"Wrote: {UptimeInfo}");

					}
					else if(ReceivedString == "l")
					{
						string UptimeInfo = "Data her om loadavg";


                        byte[] data = Encoding.ASCII.GetBytes(UptimeInfo);
                        listener.Send(data, data.Length, groupEP);

						Send(UptimeInfo);
						Console.WriteLine($"Wrote: {UptimeInfo}");
      

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
