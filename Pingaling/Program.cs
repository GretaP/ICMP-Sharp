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
            //include more args.  yar har.    xD
            //fix the switchie.  or switchie the switchie for candy.
            //find buffer size

            if (args.Length == 0)
            {
                Console.WriteLine("null error");
                Environment.Exit(1);
            }

            Ping tweetie = new Ping();
            int packetloss = new int();
            int packethaz = new int();
            int packetsent = new int();
            long totaltime = new long();
            long mintime = new long();
            long maxtime = new long();

            
            //testing new variables
            int timeout = 1000;
            //string url  needs to include args value
            string url = "";
            int count = new int();
            
            
            //parse args into usable values
            for (int i = 0; i <= args.Length; i++)
            {
                Console.WriteLine($"lala {i}");
            }

            //if this ends up being the check, get rid of args check.
            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("null error");
                Environment.Exit(1);
            }


            /* OLD working variables to be used as default values
            int timeout = 1000;
            string url = args[0];
            //number of times ping sent
            int count = 5;
            */


            //Nested try blocks allow program to check first for Socket errors from IPHostEntry, then for other errors 
            try
            {
                IPHostEntry host;
                string realhost;

                try
                {
                    host = Dns.GetHostEntry(url);
                    //ternary: if url is IP address, realhost matches.
                    IPAddress ipaddy;
                    realhost = (IPAddress.TryParse(args[0], out ipaddy) ? host.HostName : Dns.GetHostEntry(host.AddressList.First()).HostName);
                    

                for (int i=0; i < count; i++)
                {                    
                    PingReply reply = tweetie.Send(url, timeout);
                    packetsent++;
                    if (i == 0)
                    {
                            //note: default packet sent by c#'s Send is 32 bytes
                            Console.WriteLine($"Pinging {url} [{host.AddressList[0]}] with 32 bytes of data");
                    }


                    /*Checks Status Value.
                     * Success: Prints values for ping,  keeps track of statistics
                     * Timeout: packet loss, timeout message
                     * Hardware Error: System exit, error message
                     * Ttl expired: packet loss, ttl expired message
                     */
                    switch (reply.Status)
                    {
                        //Checks if realhost has a value AND if the url is NOT an ip address , if so prints out reverse lookup
                        case IPStatus.Success:
                            if (!string.IsNullOrEmpty(realhost) && url != realhost)
                                Console.Write($"Reply from {realhost};");
                            else
                                Console.Write($"Reply from");

                            //Prints out following info about ping reply: (IP address, packet #, buffer length of reply, roundtrip time, TTL)
                            Console.WriteLine($" {host.AddressList[0]}: seq={packetsent} bytes={reply.Buffer.Length} time={reply.RoundtripTime}ms TTL={reply.Options.Ttl}");

                            //For calculating statistics:
                            totaltime += reply.RoundtripTime;
                            if (reply.RoundtripTime < mintime || mintime == 0)
                                mintime = reply.RoundtripTime;
                            if (reply.RoundtripTime > maxtime)
                                maxtime = reply.RoundtripTime;

                            //increments counter for successful package recieved, and sleeps 
                            packethaz++;
                            Thread.Sleep(timeout);
                            break;

                        case IPStatus.TimedOut:
                            Console.WriteLine($"Ping to {url} timed out");
                            packetloss++;
                            break;

                        case IPStatus.HardwareError:
                            Console.WriteLine("Hardware Error.  Please check your internet settings/hardware and try again.");
                            Environment.Exit(1);
                            break;
                        case IPStatus.TtlExpired:
                            Console.WriteLine("TTL expired");
                            packetloss++;
                            break;
                    }

                }

                    //makes sure there is no division by 0 for the average time and percent of package lost statistics
                    //??? if this is necessary - when timeout values were not good, this came up.  Otherwise keep totaltime / packetsent in Console.WriteLine.
                    long averagetime = new long();
                    long percentlost = new long();
                    if (packetsent == 0)
                    {
                        averagetime = 0;
                        percentlost = 100;
                    }
                    else
                    {
                        averagetime = totaltime / packetsent;
                        percentlost = packetloss / packetsent;
                    }
                        
                    
                //print ping statistics
                Console.WriteLine($"\nPing Statistics for {url}");
                Console.WriteLine($"\tPackets: Sent= {packetsent} , Received = {packethaz} , Lost = {packetloss} , {percentlost}(%lost),");
                Console.WriteLine($"\tApproximate rount trip times in mil-seconds: \n\tMinimum = {mintime} , Maximum = {maxtime} , Average = {averagetime}");
                
    }

                //handles a situation where a GetHostEntry (IP address lookup) fails.
                catch (SocketException)
                {
                    Console.WriteLine($"Ping request could not find host {url}.  Please check the name and try again.");
                }
            }

            catch (PingException e)
            {
                Console.WriteLine($"Error: Ping exception for: {url}");
                Console.WriteLine($"Error message: {e.Message}");
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
