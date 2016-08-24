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
            int packetloss = new int();
            int packethaz = new int();
            int packetsent = new int();
            int count = 5;
            long totaltime = new long();
            long mintime = new long();
            long maxtime = new long();
          
            try
            {
                IPHostEntry host;
                IPAddress ipaddy;
                string realhost;
                host = Dns.GetHostEntry(url);

                realhost = (IPAddress.TryParse(args[0], out ipaddy) ? host.HostName : Dns.GetHostEntry(host.AddressList.First()).HostName);

                /* above replaces all this >.> gir. and the doom song. yus.
                //avoids two lookups >.>
                if (IPAddress.TryParse(args[0], out ipaddy))
                    realhost = host.HostName;
                else
                    realhost = Dns.GetHostEntry(host.AddressList.First()).HostName;
                    */

                for (int i=0; i < count; i++)
                {                    
                    PingReply reply = tweetie.Send(url, timeout);
                    packetsent++;
                    if (i == 0)
                    {
                        Console.WriteLine($"Pinging {url} [{host.AddressList[0]}] with 32 bytes of data");
                        //note: default packet sent by c#'s Send is 32 bytes
                    }

                    if (reply.Status == IPStatus.Success)
                    {
                        if (!string.IsNullOrEmpty(realhost))
                            Console.Write($"Reply from {realhost};");
                        else
                            Console.Write($"Reply from");

                        Console.WriteLine($" {host.AddressList[0]}: seq={packetsent} bytes={reply.Buffer.Length} time={reply.RoundtripTime}ms TTL={reply.Options.Ttl}");

                        //For calculating statistics:
                        totaltime += reply.RoundtripTime;
                        if (reply.RoundtripTime < mintime || mintime == 0)
                            mintime = reply.RoundtripTime;
                        if (reply.RoundtripTime > maxtime)
                            maxtime = reply.RoundtripTime;

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
                Console.WriteLine($"\tApproximate rount trip times in mil-seconds: \n\tMinimum = {mintime} , Maximum = {maxtime} , Average = {totaltime/packetsent}");

            }
            catch (PingException e)
            {
                Console.WriteLine($"Error: Ping exception for: {url}");
                Console.WriteLine($"Error message: {e.Message}");
            }
            catch (SocketException)
            {
                Console.WriteLine($"Ping request could not find host {url}.  Please check the name and try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was another error, see below: \n {e.Message}");
            }

            Console.Write("oh hi. You are the weakest link.  Goodbye!");
            Console.ReadLine();

        }
    }
}
