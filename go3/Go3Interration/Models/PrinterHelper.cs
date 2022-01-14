using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace Go3Interration
{
    public class PrinterHelper
    {

        public static bool SendPrinter(string IP,byte[] data) {

            using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                clientSocket.NoDelay = true;
                IPAddress ip = IPAddress.Parse(IP);
                IPEndPoint ipep = new IPEndPoint(ip, 9100);
                clientSocket.Connect(ipep);
              // byte[] fileBytes = bytes;

                clientSocket.Send(data);
                clientSocket.Close();
            }

            return true;
        }


       
            private readonly IPAddress PrinterIPAddress;

            private readonly byte[] FileData;

            private readonly int PortNumber;
            private ManualResetEvent connectDoneEvent { get; set; }

            private ManualResetEvent sendDoneEvent { get; set; }

            public PrinterHelper(byte[] fileData, string printerIPAddress, int portNumber = 9100)
            {
                FileData = fileData;
                PortNumber = portNumber;
                if (!IPAddress.TryParse(printerIPAddress, out PrinterIPAddress))
                    throw new Exception("Hatalı IP Addresi!");
            }

            public PrinterHelper(byte[] fileData, IPAddress printerIPAddress, int portNumber = 9100)
            {
                FileData = fileData;
                PortNumber = portNumber;
                PrinterIPAddress = printerIPAddress;
            }

            /// <inheritDoc />
            public bool PrintData()
            {
                //this line is Optional for checking before send data
                if (!NetworkHelper.CheckIPAddressAndPortNumber(PrinterIPAddress, PortNumber))
                    return false;
                IPEndPoint remoteEP = new IPEndPoint(PrinterIPAddress, PortNumber);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.NoDelay = true;
                connectDoneEvent = new ManualResetEvent(false);
                sendDoneEvent = new ManualResetEvent(false);

                try
                {
                    client.BeginConnect(remoteEP, new AsyncCallback(connectCallback), client);
                    connectDoneEvent.WaitOne();
                    client.BeginSend(FileData, 0, FileData.Length, 0, new AsyncCallback(sendCallback), client);
                    sendDoneEvent.WaitOne();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    // Shutdown the client
                    this.shutDownClient(client);
                }
            }

            private void connectCallback(IAsyncResult ar)
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                // Signal that the connection has been made.
                connectDoneEvent.Set();
            }

            private void sendCallback(IAsyncResult ar)
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent.
                sendDoneEvent.Set();
            }
            private void shutDownClient(Socket client)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

    public static class NetworkHelper
    {
        public static bool CheckIPAddressAndPortNumber(IPAddress ipAddress, int portNumber)
        {
            return PingIPAddress(ipAddress) && CheckPortNumber(ipAddress, portNumber);
        }
        public static bool PingIPAddress(IPAddress iPAddress)
        {
            var ping = new Ping();
            PingReply pingReply = ping.Send(iPAddress);

            if (pingReply.Status == IPStatus.Success)
            {
                //Server is alive
                return true;
            }
            else
                return false;
        }
        public static bool CheckPortNumber(IPAddress iPAddress, int portNumber)
        {
            var retVal = false;
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(iPAddress, portNumber);
                    retVal = tcpClient.Connected;
                    tcpClient.Close();
                }
                return retVal;
            }
            catch (Exception)
            {
                return retVal;
            }

        }
    }

}