using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using BloomBerg_Code_B.JNUG_Engine.Interfaces;

namespace BloomBerg_Code_B.JNUG_Engine.ActionEngines {
    public class InfluencedDirEngine : IActionEngine {

        private IMovementClient _actions;
        private IScanner _scanner;

        SurfacePosition _position;
        private string _owner;
        private DateTime _lastScan;
        private double _angle;
        private DateTime _start;

        public InfluencedDirEngine(IMovementClient actions, IScanner scanner, string owner, SurfacePosition position) {
            _actions = actions;
            _scanner = scanner;
            _owner = owner;
            _position = position;
            _lastScan = DateTime.Now;
        }
        public Optional<IActionEngine> ReceiveStatus(StatusUpdateModel model) {
            if (_start.AddSeconds(3) > DateTime.Now) {
                return Optional<IActionEngine>.FromNull();
            }

            var notOwnedMines = model.GoldMines.Where(s => s.Owner != _owner);
            if (CommonTools.CalHypo(model.ShipStatus.XPos, _position.X, model.ShipStatus.YPos, _position.Y) < 300) {
                var randomEng = new RandomExplorerEngine(_actions, _scanner, _owner);
                return Optional<IActionEngine>.From(randomEng);
            }

            if (notOwnedMines.Count() > 0) {
                return Optional.From<IActionEngine>(new ApproacherEngine(notOwnedMines.First(), this, _actions, _owner));
            } else {
                PivotTowardPosition(model);
                MakeScans();
                return Optional<IActionEngine>.FromNull();
            }
        }

        public void Run(StatusUpdateModel model) {
            _start = DateTime.Now;
            var angle = CommonTools.CalculateAngle(model.ShipStatus.XPos, _position.X, model.ShipStatus.YPos, _position.Y);
            _angle = angle;
            PivotTowardPosition(model);
        }

        private void PivotTowardPosition(StatusUpdateModel model) {
            var angle = CommonTools.CalculateAngle(model.ShipStatus.XPos, _position.X, model.ShipStatus.YPos, _position.Y);
            _angle = angle;
            _actions.Accelerate(_angle, 1);
        }

        private void MakeScans() {
            if (_lastScan.AddSeconds(5) < DateTime.Now) {
                var result = _scanner.Scan(CommonTools.GetRandom(5000), CommonTools.GetRandom(5000));
                _lastScan = DateTime.Now;
            }
        }

    }
}