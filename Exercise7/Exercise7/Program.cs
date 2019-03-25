using System;

namespace Exercise7
{
    class MainClass
    {
        public static void Main(string[] args)
        {

			UDPClient client = new UDPClient();
			string choice = "";

			while(choice.ToLower() != "q")
			{
				Console.WriteLine("Enter character to send to server:");
				choice = Console.ReadLine();
				if (choice.ToLower() != "q")
					client.Send(choice);
				UDPClient.StartListener();



			UDPServer.StartListener();

        }
    }
}
