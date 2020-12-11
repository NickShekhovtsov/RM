using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RM.Models
{   public enum flagTypeServer
    {
        Accept,Send
    }
    public class SocketManager
    {

        private Socket serverSocket;
        private Socket clientSocket;
        private flagTypeServer fl;

        public SocketManager(string ip, int port, flagTypeServer _fl)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            fl = _fl;
            // создаем сокет
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // связываем сокет с локальной точкой, по которой будем принимать данные
            serverSocket.Bind(ipPoint);
        }

        public void Run()
        {
            waitClient();
        }



        private void waitClient()
        {
            while (true)
            {
                // начинаем прослушивание
                serverSocket.Listen(10);
                Console.WriteLine("Client connected");
                clientSocket = serverSocket.Accept();

                //Console.WriteLine("Сервер запущен. Ожидание подключений...");
                try
                {
                    if (fl == flagTypeServer.Accept)
                        AcceptInfo(clientSocket);

                    if (fl == flagTypeServer.Send)
                        SendCommands(clientSocket);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Client disconnected");
                }
                Thread.Sleep(100);
            }
        }
        public void SendCommands(Socket handler)
        {
            byte[] data = new byte[128];

            while (true)
            {
                //Console.WriteLine("Enter Command");
                if (Case.flag)
                {
                    if (Case.button == "play")
                        data = Encoding.Unicode.GetBytes("play");
                    if (Case.button == "next")
                        data = Encoding.Unicode.GetBytes("next");
                    if (Case.button == "previous")
                        data = Encoding.Unicode.GetBytes("prev");
                    Case.flag = false;
                    Case.button = "";
                    try
                    {
                        handler.Send(data);
                    }
                    catch (SocketException e)
                    {
                        throw e;
                    }
                }
                Thread.Sleep(100);
            }
        }
        public async void AcceptInfoAsync(Socket handler)
        {
            await Task.Run(() => AcceptInfo(handler));
        }
        public void AcceptInfo(Socket handler)
        {
            SongInfo js;
            while (true)
            {
                if (handler.Available > 0)
                {
                    byte[] data;
                    // получаем ответ
                    data = new byte[4000]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байт
                    do
                    {
                        bytes = handler.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    
                    string sr = builder.ToString();
                    Console.WriteLine(sr);
                    
                     js = JsonSerializer.Deserialize<SongInfo>(sr);

                    Case.name = js.name;
                    Case.duration = js.duration;
                    

                }
            }

        }
    }
}
    
   

