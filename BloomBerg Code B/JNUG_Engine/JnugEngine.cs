using BloomBerg_Code_B.JNUG_Engine.ActionEngines;
using BloomBerg_Code_B.JNUG_Engine.Interfaces;
using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BloomBerg_Code_B.JNUG_Engine {
    public class JnugEngine : IScanner {

        public DataStore Data { get; private set; }
        private IActionClient _actions;
        private bool _halt;

        private static JnugEngine _engine;
        private string _user;
        private IActionEngine _currentEngine;

        public JnugEngine(IActionClient actionsPortal, string user) {
            _actions = actionsPortal;
            Data = new DataStore();
            _halt = false;
            _user = user;
        }

        public void HaltEngine() {
            _halt = true;
        }

        public StatusUpdateModel Scan(double x, double y) {
            if (!CanScan()) {
                return StatusUpdateModel.MakeUnAvailableModel();
            }
            var model = _actions.Scan(x, y);
            if (!model.Available) {
                return StatusUpdateModel.MakeUnAvailableModel();
            }
            Data.LastScan = DateTime.Now;
            Data.LastScanResults = model;
            AddUniqueMines(model.GoldMines);
            return model;
        }

        public string GetEngineType() {
            return _currentEngine.GetType().ToString();
        }

        private bool CanScan() {
            var lastScan = Data.LastScan;
            return lastScan.AddSeconds(5) < DateTime.Now;
        }

        private void Run() {
            InitData();
            var randomEngine = new RandomExplorerEngine(_actions, this, _user);
            randomEngine.Run(_actions.GetStatus());
            _currentEngine = randomEngine;
            //run these two async
            Task.Run(() => PeriodicScan());
            Task.Run(() => PeriodicScoreboardUpdates());
        }

        private void InitData() {
            var config = _actions.GetConfig();
            Data.Configuration = config;

            var score = _actions.GetScore();
            Data.Scoreboard = score;
        }

        private void PeriodicScan() {
            while (!_halt) {
                try {
                    var status = _actions.GetStatus();
                    if (!status.Available) {
                        continue;
                    }
                    SetStatus(status);
                    AddUniqueMines(status.GoldMines);
                    Data.LastVisionResults = status;
                    SendDirEngine(status);
                    var result = _currentEngine.ReceiveStatus(status);
                    if (result.Present) {
                        _currentEngine = result.Value;
                        if (_currentEngine is ApproacherEngine) {
                            Data.TargetDirection = (_currentEngine as ApproacherEngine).GetTarget();
                        }
                    }
                    Task.Delay(3).Wait();
                } catch {
                    //fuck it, do nothing
                }
            }
        }

        private void PeriodicScoreboardUpdates() {
            while (!_halt) {
                var score = _actions.GetScore();
                Data.Scoreboard = score;
                Task.Delay(1000).Wait();
            }
        }

        private void SendDirEngine(StatusUpdateModel model) {
            if (!model.Available) {
                return;
            }

            if (_currentEngine is RandomExplorerEngine && Data.Mines.Count > 8 && CommonTools.GetRandom(100) < Data.Mines.Count * 3) {

                var closestDistance = Data.Mines.Values.Where(s => s.Owner != _user)
                    .Select(s => CommonTools.CalHypo(model.ShipStatus.XPos, s.X, model.ShipStatus.YPos, s.Y)).Distinct().OrderBy(s => s).Where(s => s < 2000).ToList();

                if (closestDistance.Count == 0) {
                    if (CommonTools.GetRandom(100) < 30) {
                        closestDistance = Data.Mines.Values.Where(s => s.Owner != _user)
                    .Select(s => CommonTools.CalHypo(model.ShipStatus.XPos, s.X, model.ShipStatus.YPos, s.Y)).Distinct().OrderBy(s => s).Where(s => s < 4000).ToList();
                    } else {
                        return;
                    }
                }

                var closestList = Data.Mines.Where(s => closestDistance.Contains(CommonTools.CalHypo(model.ShipStatus.XPos, s.Value.X, model.ShipStatus.YPos, s.Value.Y))).Distinct().ToList();

                var closest = closestList[CommonTools.GetRandom(closestList.Count)];
                var distance = CommonTools.CalHypo(model.ShipStatus.XPos, closest.Value.X, model.ShipStatus.YPos, closest.Value.Y);

                var dirEngine = new InfluencedDirEngine(_actions, this, _user, closest.Value);
                _currentEngine = dirEngine;
                Data.TargetDirection = closest.Value;
                dirEngine.Run(model);
            }
        }

        private void SendLongRangeApproacher(StatusUpdateModel model) {
            if (CommonTools.GetRandom(100) < 10 && _currentEngine is RandomExplorerEngine) {
                var location = Data.Mines.ToList()[CommonTools.GetRandom(Data.Mines.Count)];
                var longRangeEngine = new LongRangeApproacherEngine(location.Value, _currentEngine, _actions, _user);
                _currentEngine = longRangeEngine;
                longRangeEngine.Run(model);
            }
        }

        private void SetStatus(StatusUpdateModel status) {
            Data.ShipStatus = status.ShipStatus;
        }

        private void AddUniqueMines(List<SurfacePosition> mines) {
            foreach (var mine in mines) {
                if (!Data.Mines.ContainsKey(mine.GetHashString())) {
                    Data.Mines.Add(mine.GetHashString(), mine);
                } else {
                    //updates it
                    Data.Mines[mine.GetHashString()] = mine;
                }
            }
        }

        public static void Start(IActionClient actionsPortal, string user) {
            _engine = new JnugEngine(actionsPortal, user);
            _engine.Run();
        }

        public static JnugEngine GetJnug() {
            return _engine;
        }
    }

    public enum ExplorationStatus { RANDOM_SURVEY, SPIRAL_SURVEY, CAPTURING, STALKING }
}