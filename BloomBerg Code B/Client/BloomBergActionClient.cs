using BloomBerg_Code_B.Agent;
using BloomBerg_Code_B.JNUG_Engine.Interfaces;
using BloomBerg_Code_B.Models;
using BloomBerg_Code_B.Parser;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BloomBerg_Code_B.Client {
    public class BloomBergActionClient : IActionClient {

        public static BloomBergActionClient Client {
            get {
                if (_client == null) {
                    throw new NullReferenceException("The action client hasn't been initalized yet");
                } else {
                    return _client;
                }
            }
        }

        private IPEndPoint _ipEndPoint;
        private BloomBergLoginAgent _loginAgent;

        private static BloomBergActionClient _client;

        private const string ACTION_PATH = "";

        public BloomBergActionClient(BloomBergLoginAgent agent) {
            _loginAgent = agent;
            var ip = new IPAddress(Constants.MAIN_IP);
            _ipEndPoint = new IPEndPoint(ip, Constants.MAIN_PORT);
        }

        public static void CreateClient(BloomBergLoginAgent agent) {
            _client = new BloomBergActionClient(agent);
        }

        public StatusUpdateModel GetStatus() {
            var request = CreateRequestFromAction("STATUS");
            var response = ExecuteRequestAndGetResponse(request);
            if (response.Contains("limit")) {
                return StatusUpdateModel.MakeUnAvailableModel();
            }
            try {
                var parser = new StatusParser(response);
                var model = parser.GetModel();
                return model;
            } catch (Exception e) {
                return StatusUpdateModel.MakeUnAvailableModel();
            }
        }

        public bool Accelerate(double direction, double magnitude) {
            var request = CreateRequestFromAction($"ACCELERATE {direction} {magnitude}");
            var response = ExecuteRequestAndGetResponse(request);
            if (response.Contains("DONE")) {
                return true;
            } else {
                return false; //IDK what other status comes out of this
            }
        }

        public bool Break() {
            var request = CreateRequestFromAction("BRAKE");
            var response = ExecuteRequestAndGetResponse(request);
            if (response.Contains("DONE")) {
                return true;
            } else {
                return false; //IDK what other status comes out of this
            }
        }

        public bool Bomb(double x, double y, int fuse = 0) {
            var request = CreateRequestFromAction($"BOMB {x} {y}");
            if (fuse > 0) {
                request = request + $" {fuse}";
            }
            var response = ExecuteRequestAndGetResponse(request);
            if (response.Contains("DONE")) {
                return true;
            } else {
                return false; //IDK what other status comes out of this
            }
        }

        public StatusUpdateModel Scan(double x, double y) {
            var request = CreateRequestFromAction($"SCAN {x} {y}");
            var response = ExecuteRequestAndGetResponse(request);

            if (response.Contains("ERROR")) {
                return StatusUpdateModel.MakeUnAvailableModel();
            } else {
                try {
                    var parser = new StatusParser(response);
                    var model = parser.GetModel();
                    return model;
                } catch (Exception ex) {
                    return StatusUpdateModel.MakeUnAvailableModel();
                }
            }
        }

        public ScoreboardModel GetScore() {
            var request = CreateRequestFromAction($"SCOREBOARD");
            var response = ExecuteRequestAndGetResponse(request);
            if (response.Contains("limit")) {
                return new ScoreboardModel(new List<ScoreboardEntry>());
            }
            var parser = new ScoreboardParser(response);
            var model = parser.GetModel();
            return model;
        }

        public ConfigurationModel GetConfig() {
            var request = CreateRequestFromAction($"CONFIGURATIONS");
            var response = ExecuteRequestAndGetResponse(request);
            var parser = new ConfigurationParser(response);
            var model = parser.GetModel();
            return model;   
        }

        private string CreateRequestFromAction(string action) {
            var builder = new StringBuilder().AppendLine(_loginAgent.GetLoginString());
            builder.AppendLine(action);
            return builder.ToString();
        }

        private string ExecuteRequestAndGetResponse(string request) {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ConnectAndSendMessage(socket, request);
            var msg = GetSenderMessage(socket);
            ShutDownSocket(socket);

            return msg;
        }

        private void ConnectAndSendMessage(Socket socket, string message) {
            socket.Connect(_ipEndPoint);
            var msg = Encoding.ASCII.GetBytes(message);
            socket.Send(msg);
        }

        private string GetSenderMessage(Socket socket) {
            var bytes = new byte[8192];

            while (socket.Available == 0) { }

            var received = Receive(socket, bytes, 0, socket.Available);
            
            return Encoding.ASCII.GetString(bytes, 0, received);
        }

        private int Receive(Socket socket, byte[] buffer, int offset, int size) {
            int received = 0;
            do {
                received += socket.Receive(buffer, offset + received, size - received, SocketFlags.None);
            } while (received < size);
            return received;
        }

        private void ShutDownSocket(Socket socket) {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}