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
            if (args.Length == 0)
            {
                Console.WriteLine("null error");
                Environment.Exit(1);
            }

            Ping tweetie = new Ping();
            int timeout = 1000;
            string url = args[0];
            int packetloss = 0;
            int packethaz = 0;
            int packetsent = 0;
            int count = 5;

            try
            {
                for (int i = 0; i < count; i++)
                {
                    IPHostEntry host;
                    host = Dns.GetHostEntry(url);
                    PingReply reply = tweetie.Send(url, timeout);
                    packetsent++;
                    if (i == 0)
                    {
                        Console.WriteLine($"Pinging {url} [{host.AddressList[0]}] with 32 bytes of data");
                        //note: default packet sent by c#'s Send is 32 bytes
                    }

                    if (reply.Status == IPStatus.Success)
                    {
                        //Console.WriteLine($"Testing: {host.Aliases[0]}");
                        //send #  = i 

                        //matching to windows output:
                        Console.WriteLine($"Reply from {host.AddressList[0]}: bytes={reply.Buffer.Length} time={reply.RoundtripTime}ms TTL={reply.Options.Ttl}");
                        packethaz++;

                        Thread.Sleep(timeout);

                    }
                    if (reply.Status == IPStatus.TimedOut)
                    {
                        Console.WriteLine($"Ping to {url} timed out");
                        packetloss++;
                    }

                }
                Console.WriteLine("\n");
                Console.WriteLine($"Ping Statistics for {url}");
                Console.WriteLine($"\tPackets: Sent= {packetsent} , Received = {packethaz} , Lost = {packetloss} , {packetloss / packetsent}(%lost),");
                Console.WriteLine($"\tApproximate rount trip times in mil-seconds: \n\tMinimum = , Maximum = , Average =");
            }
            catch (PingException e)
            {
                Console.WriteLine($"Error: Ping exception for: {url}");
                Console.WriteLine(e.InnerException);
            }
            catch (SocketException)
            {
                Console.WriteLine($"Ping request could not find host {url}.  Please check the name and try again.");
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
