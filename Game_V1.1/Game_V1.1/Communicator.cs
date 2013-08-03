using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Game_V1._2
{
    class Communicator
    {
        private TcpClient clientSocket;
        private TcpListener serverSocket;
        private NetworkStream serverStream;
        private NetworkStream clientStream;
        private string reply;
        public bool launched;
        public bool ended;
        private Stopwatch sw;
        private BinaryWriter writer;
        private String serverIP;
        private String clientIP;
        private int serverPortNumber;
        private int clientPortNumber;
        public bool isNew;
        public bool joined;
        public String output;

        public Communicator(String serverip, String cleintip, int sport, int cport)
        {
            this.clientSocket = new TcpClient();            
            this.launched = false;
            this.ended = false;
            this.isNew = false;
            this.sw = new Stopwatch();
            this.joined = false;
            this.serverIP = serverip;
            this.clientIP = cleintip;
            this.serverPortNumber = sport;
            this.clientPortNumber = cport;

            try
            {
                this.serverSocket = new TcpListener(IPAddress.Parse(clientIP), clientPortNumber);
                this.serverSocket.Start();
                launched = true;
            }
            catch (Exception ex)
            {
                DialogResult dr = MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dr == DialogResult.OK)
                    Application.Exit();
                launched = false;
            }

            this.reply = "";
            this.output = "";
        }

        public void joinGame()
        {
            try
            {
                this.clientSocket.Connect(serverIP, serverPortNumber);                
                this.clientStream = this.clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Util.Constants.JoinRequest);
                clientStream.Write(outStream, 0, outStream.Length);
                clientStream.Dispose();
                clientStream.Flush();               
                launched = true;                
                this.receiveData();

            }
            catch (Exception ex)
            {                
                DialogResult dr = MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dr == DialogResult.OK)
                    Application.Exit();
                launched = false;
            }
        }

        public void move(int dir)   //dir 0=up  1=right 2=down  3=left
        {
            String message = "";
            switch (dir)
            {
                case 0:
                    message = Util.Constants.MoveUp;
                    break;
                case 1:
                    message = Util.Constants.MoveRight;
                    break;
                case 2:
                    message = Util.Constants.MoveDown;
                    break;
                case 3:
                    message = Util.Constants.MoveLeft;
                    break;
            }
            sendData(message);            
        }

        public void shoot()
        {
            sendData(Util.Constants.Shoot);
        }

        public void sendData(String sendMessage)
        {
            
            this.clientSocket = new TcpClient();

            try
            {
                this.clientSocket.Connect(serverIP, serverPortNumber);

                if (this.clientSocket.Connected)
                {
                    this.clientStream = clientSocket.GetStream();

                    this.writer = new BinaryWriter(clientStream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(sendMessage);

                    this.writer.Write(tempStr);                    
                    this.writer.Close();
                    this.clientStream.Close();                    
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }
            finally
            {
                this.clientSocket.Close();
            }
            
        }

        public void receiveData()
        {
            bool errorOcurred = false;
            Socket connection = null;

            try
            {
                while (!ended)
                {
                    connection = serverSocket.AcceptSocket();
                    if (connection.Connected)
                    {
                        //To read from socket create NetworkStream object associated with socket
                        this.serverStream = new NetworkStream(connection);
                        SocketAddress sockAdd = connection.RemoteEndPoint.Serialize();
                        string s = connection.RemoteEndPoint.ToString();
                        List<Byte> inputStr = new List<byte>();

                        int asw = 0;

                        while (asw != -1)
                        {
                            asw = this.serverStream.ReadByte();
                            if (asw != -1)
                                inputStr.Add((Byte)asw);
                        }

                        reply = Encoding.UTF8.GetString(inputStr.ToArray());
                        this.serverStream.Close();

                        while (isNew)
                        {
                        }
                        //Console.WriteLine(reply);
                        this.output = reply;
                        isNew = true;
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.Message);
                errorOcurred = true;
            }
            finally
            {
                if (connection != null)
                    if (connection.Connected)
                        connection.Close();
                if (errorOcurred)
                    this.receiveData();
            }
        }
    }
}
