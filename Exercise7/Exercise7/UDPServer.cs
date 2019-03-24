using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;



namespace Exercise7
{
    public class UDPServer
    {
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

		private const int listenPort = 9000;

        private static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

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

                        byte[] data = Encoding.ASCII.GetBytes(UptimeInfo);
                        listener.Send(data, data.Length, groupEP);

					}
					else if(ReceivedString == "l")
					{
						string UptimeInfo = "Data her om loadavg"

                        byte[] data = Encoding.ASCII.GetBytes(UptimeInfo);
                        listener.Send(data, data.Length, groupEP);
					}
					else
					{
						// Skriv til client at svar ikke var gyldigt.
						string UptimeInfo = "Invalid request"

                        byte[] data = Encoding.ASCII.GetBytes(UptimeInfo);
                        listener.Send(data, data.Length, groupEP);
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
