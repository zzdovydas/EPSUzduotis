﻿using EPSuzduotis;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// State object for reading client data asynchronously  
public class StateObject
{
    // Client  socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 1024;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousSocketListener
{
    // Thread signal.  
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    public static string path;

    public AsynchronousSocketListener()
    {
    }

    public static void StartListening(int port)
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[1];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
 
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                allDone.Reset();
 
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);
 
                allDone.WaitOne();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static void AcceptCallback(IAsyncResult ar)
    {  
        allDone.Set();

        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0)
        {
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            content = state.sb.ToString();

            HTTPHeaders header = new HTTPHeaders();

            if (content.IndexOf((char)13) > -1)
            {
                if (state.sb.ToString().StartsWith("GET /"))
                {
                    string getPath = content.ToString().Split(' ')[1].Replace('/', '\\');
                    string pathDest = path + getPath;
                    FileHandleClass fH = new FileHandleClass(pathDest);
                    if (fH.FileExists())
                    {
                        byte[] fileData = fH.GetFileBytes();
                        byte[] headerData = System.Text.Encoding.UTF8.GetBytes(header.HeaderSuccess(fileData.Length));
                        byte[] allData = new byte[fileData.Length + headerData.Length + 2];
                        Array.Copy(headerData, allData, headerData.Length);
                        Array.Copy(fileData, 0, allData, headerData.Length, fileData.Length);
                        allData[allData.Length - 2] = 13;
                        allData[allData.Length - 1] = 10;
                        content = header.HeaderSuccess(fileData.Length);
                        Send(handler, allData);
                    }
                    else
                    {
                        content = header.HeaderNotFound();
                        Send(handler, content);
                    }
                }
                else
                {
                    content = header.HeaderBadRequest();
                    Send(handler, content);
                }

                Console.WriteLine("Read {0} bytes. \n Data : {1}",
                    content.Length, content);
            }
            else
            {
                content = header.HeaderBadRequest();
                Send(handler, content);
            }
        }
    }

    private static void Send(Socket handler, String data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        try
        {
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        catch
        { }
    }

    private static void Send(Socket handler, byte[] data)
    {
        byte[] byteData = data;
        try
        {
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        catch
        { }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket handler = (Socket)ar.AsyncState;
 
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        Console.WriteLine("Asynchronous TCP Socket server handling HTTP request by Dovydas Marius Zapkus");

        int port = -1;

        while (port < 0 || port > 65535)
        {
            Console.WriteLine("Please specify server Port!");
            port = int.Parse(Console.ReadLine());
        }

        while (!System.IO.Directory.Exists(path))
        {
            Console.WriteLine(@"Please specify Document root directory! C:\...\...\...");
            path = Console.ReadLine();
        }

        StartListening(port);
        return 0;
    }
}