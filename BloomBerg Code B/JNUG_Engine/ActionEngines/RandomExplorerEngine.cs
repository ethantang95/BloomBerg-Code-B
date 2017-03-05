using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using BloomBerg_Code_B.JNUG_Engine.Interfaces;

namespace BloomBerg_Code_B.JNUG_Engine.ActionEngines {
    public class RandomExplorerEngine : IActionEngine {

        private IMovementClient _actions;
        private IScanner _scanner;

        private double _lastRoll;
        private DateTime _lastDecisionTime;
        private string _owner;
        private DateTime _lastScan;
        private DateTime _start;

        public RandomExplorerEngine(IMovementClient actions, IScanner scanner, string owner) {
            _actions = actions;
            _scanner = scanner;
            _owner = owner;
        }

        public Optional<IActionEngine> ReceiveStatus(StatusUpdateModel model) {
            if (_start.AddSeconds(3) > DateTime.Now) {
                return Optional<IActionEngine>.FromNull();
            }
            var notOwnedMines = model.GoldMines.Where(s => s.Owner != _owner);
            if (notOwnedMines.Count() > 0) {
                return Optional.From<IActionEngine>(new ApproacherEngine(notOwnedMines.First(), this, _actions, _owner));
            } else {
                RollAgain(model);
                MakeScans();
                return Optional<IActionEngine>.FromNull();
            }
        }

        public void Run(StatusUpdateModel model) {
            MakeDecision();
            _start = DateTime.Now;
        }


        private void RollAgain(StatusUpdateModel model) {
            if (_lastDecisionTime.AddSeconds(15) < DateTime.Now) {
                MakeDecision();
            } else {
                var lastRollRad = CommonTools.RadToDeg(_lastRoll);
                lastRollRad += CommonTools.GetRandom(21) - 10;
                _lastRoll = CommonTools.DegToRad(lastRollRad);
                _actions.Accelerate(_lastRoll, 1);
            }
        }

        private void MakeDecision() {
            _lastDecisionTime = DateTime.Now;
            var direction = CommonTools.GetRandomDegree();
            _lastRoll = direction;
            _actions.Accelerate(direction, 1);
        }

        private void MakeScans() {
            if (_lastScan.AddSeconds(5) < DateTime.Now) {
                var result = _scanner.Scan(CommonTools.GetRandom(5000), CommonTools.GetRandom(5000));
                _lastScan = DateTime.Now;
            }
        }
    }
}