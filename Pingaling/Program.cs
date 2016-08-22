using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Net.Sockets;

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
            string url = args[0];


            //Reply from 52.40.4.203: bytes=32 time=51ms TTL=43
            // takes string and shows the ip address resolution
            /*
     Ping statistics for 52.40.4.203:
     Packets: Sent = 4, Received = 4, Lost = 0(0 % loss),
 Approximate round trip times in milli - seconds:
    Minimum = 49ms, Maximum = 59ms, Average = 52ms
   */

            Console.WriteLine($"Pinging {url} with BLAH bytes of data");
            try
            {

                for (int i = 0; i < 5; i++)
                {

                    if (i == 0)
                    {
                        IPHostEntry host;
                        host = Dns.GetHostEntry(url);
                        Console.WriteLine($"The url {url} resolves to {host.AddressList[0]}");
                    }
                    PingReply reply = tweetie.Send(url, timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        //time amount, bytes in package                  
                        Console.WriteLine($"Reply from: {reply.Address.ToString()}: Send # {i}");
                        Console.WriteLine($"Time: {reply.RoundtripTime} ... TTl: {reply.Options.Ttl}");
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
            catch (SocketException)
            {
                Console.WriteLine($"Error: Ping request could not find host {url}.  Please check the name and try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"other exception: {e}");
            }


            Console.Write("oh hi. You are the weakest link.  Goodbye!");
            Console.ReadLine();



        }
    }
}
