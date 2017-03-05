using BloomBerg_Code_B.Agent;
using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace BloomBerg_Code_B.Client {
    public class BloomBergLoginClient {

        public static BloomBergLoginClient Client {
            get {
                if (_client == null) {
                    throw new NullReferenceException("The login client hasn't been initalized yet, please pass in a set of credentials");
                } else {
                    return _client;
                }
            }
        }

        private IPEndPoint _ipEndPoint;
        private BloomBergLoginAgent _loginAgent;

        private static BloomBergLoginClient _client;

        private BloomBergLoginClient(BloomBergLoginModel model) {
            _loginAgent = new BloomBergLoginAgent(model);
            var ip = new IPAddress(Constants.MAIN_IP);
            _ipEndPoint = new IPEndPoint(ip, Constants.MAIN_PORT);
        }

        public static void CreateClient(Optional<BloomBergLoginModel> model) {
            if (!model.Present) {
                throw new NullReferenceException("The login client passed on is null");
            } else {
                EstablishClient(model.Value);
            }
        }

        public static bool IsLoggedIn() {
            return _client != null;
        }

        public void Connect() {
            var credentials = GetCredentials();
            var content = ExecuteRequestAndGetResponse(credentials);
            HandleLoginContentResults(content);
        }

        public bool GetConnectionStatus() {
            return _loginAgent.Connected;
        }

        public string GetCredentials() {
            return _loginAgent.GetLoginString();
        }

        public BloomBergLoginAgent GetAgent() {
            return _loginAgent;
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
            var bytes = new byte[1024];
            var bytesRec = 0;

            if (socket.Available != 0) {
                bytesRec = socket.Receive(bytes);
            }
            return Encoding.ASCII.GetString(bytes, 0, bytesRec);
        }

        private void ShutDownSocket(Socket socket) {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void HandleLoginContentResults(string result) {
            if (IsConnectionError(result)) {
                _loginAgent.SetConnectionFailed();
                throw new HttpException($"Connection to login not established due to {result}");
            } else {
                _loginAgent.SetConnectionSuccess();
            }
        }

        private bool IsConnectionError(string connectionContent) {
            if (connectionContent.Contains("Unknown User")) {
                return true;
            }
            return false;
        }

        private void SetNewLoginModel(BloomBergLoginModel model) {
            _loginAgent.ChangeLogin(model);
        }

        private static void EstablishClient(BloomBergLoginModel model) {
            if (_client == null) {
                _client = new BloomBergLoginClient(model);
            } else {
                _client.SetNewLoginModel(model);
            }
        }
    }
}