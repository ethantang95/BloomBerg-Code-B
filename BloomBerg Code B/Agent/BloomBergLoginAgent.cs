using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Agent {
    public class BloomBergLoginAgent {
        public bool Connected { get; private set; }
        private BloomBergLoginModel _loginModel;

        public BloomBergLoginAgent(BloomBergLoginModel model) {
            NewLoginSetters(model);
        }

        public void SetConnectionSuccess() {
            Connected = true;
        }

        public void SetConnectionFailed() {
            Connected = false;
        }

        public void ChangeLogin(BloomBergLoginModel model) {
            NewLoginSetters(model);
        }

        public string GetLoginString() {
            return $"{_loginModel.Username} {_loginModel.Password}{Environment.NewLine}";
        }

        public string GetLoginName() {
            return _loginModel.Username;
        }

        private void NewLoginSetters(BloomBergLoginModel model) {
            _loginModel = model;
            Connected = false;
        }
    }
}