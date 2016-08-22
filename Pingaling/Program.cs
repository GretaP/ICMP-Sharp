using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;

namespace Pingaling
{
    class Program
    {
        static void Main(string[] args)
        {

            /*testing variables
            //string url = ("52.40.4.203");
            //string url= ("gretaparks.com");
            //string url = ("foobarislalalalaa.c");
            */

            //define variables
            Ping tweetie = new Ping();
            string exitresponse = string.Empty;
            int timeout = 1000;

            do
            {
                Console.WriteLine("Please enter a URL or IP address to ping");
                string url = args[0];
                //string url = Console.ReadLine();
                //Console.WriteLine($"PING {url}");

                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        PingReply reply = tweetie.Send(url, timeout);
                        if (reply.Status == IPStatus.Success)
                        {                             
                            Console.WriteLine($"Pinging: {reply.Address.ToString()} : Send # {i}");
                            Console.WriteLine($"Round trip time: {reply.RoundtripTime} ... TTl: {reply.Options.Ttl}");
                            //buffer??? >.>
                            Console.WriteLine($"Buffer length:{reply.Buffer.Length}");
                            Console.WriteLine("\n\n");
                            Thread.Sleep(timeout);
                            
                        }
                        if (reply.Status == IPStatus.TimedOut) 
                        {
                            Console.WriteLine($"Ping to {url} timed out");
                        }
                    }
                }
                catch (PingException e)
                {
                    if (e.InnerException == null)
                    {
                        Console.WriteLine("Null error");
                    }
                    Console.WriteLine($"Error: Ping exception for: {url}");
                    Console.WriteLine(e.InnerException);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"other exception: {e}");
                }
                           
                Console.WriteLine("Type \"e\" to exit, anything else to try again");
                exitresponse = Console.ReadLine();

            } while (exitresponse !="e");

            Console.Write("oh hi. You are the weakest link.  Goodbye!");
            Console.ReadLine();



        }
    }
}
