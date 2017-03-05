using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Parser {
    public class StatusParser : ParserAbstract<StatusUpdateModel>{
        private StatusUpdateModel _model;
        private string _raw;

        public StatusParser(string toParse) : base(toParse) {
            _model = new StatusUpdateModel();
            _model.Available = true;
            _raw = toParse;
        }

        public override StatusUpdateModel GetModel() {
            FillPlayerShipLocation();
            PopScanOut();
            FillMines();
            FillPlayers();
            FillBombs();
            _model.Raw = _raw;
            return _model;
        }

        private void FillPlayerShipLocation() {
            if (!PopIfMessage("STATUS_OUT")) {
                return;
            }
            var playerLocation = new ShipStatus(PopDouble(), PopDouble(), PopDouble(), PopDouble());
            _model.ShipStatus = playerLocation;
        }

        private void PopScanOut() {
            PopIfMessage("SCAN_OUT");
        }

        private void FillMines() {
            if (!PopIfMessage("MINES")) {
                return;
            };
            var localMines = PopInt();
            var mineList = new List<SurfacePosition>();
            for (int i = 0; i < localMines; i++) {
                var pos = new SurfacePosition(PopString(), PopDouble(), PopDouble());
                mineList.Add(pos);
            }
            _model.GoldMines = mineList;
        }

        private void FillPlayers() {
            if (!PopIfMessage("PLAYERS")) {
                return;
            };
            var localPlayers = PopInt();
            var playerList = new List<ShipStatus>();
            for (int i = 0; i < localPlayers; i++) {
                var playerLocation = new ShipStatus(PopDouble(), PopDouble(), PopDouble(), PopDouble());
                playerList.Add(playerLocation);
            }
            _model.OtherShips = playerList;
        }

        private void FillBombs() {
            if (!PopIfMessage("BOMBS")) {
                return;
            };
            //assuming it is {x} {y}
            var localBombs = PopInt();
            var bombList = new List<SurfacePosition>();
            for (int i = 0; i < localBombs; i++) {
                var pos = new SurfacePosition("--", PopDouble(), PopDouble());
                bombList.Add(pos);
            }
            _model.Bombs = bombList;
        }
    }
}