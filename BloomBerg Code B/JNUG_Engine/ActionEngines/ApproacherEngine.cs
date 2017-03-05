using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using BloomBerg_Code_B.JNUG_Engine.Interfaces;

namespace BloomBerg_Code_B.JNUG_Engine.ActionEngines {
    public class ApproacherEngine : IActionEngine {

        private SurfacePosition _position;
        private IActionEngine _prevEngine;
        private ShipStatus _lastPosition;
        private IMovementClient _client;
        private ApproachStatus _status;
        private string _owner;
        private DateTime _attemptStart;
        private double _distanceAwayFromBreak;
        private double _angle;
        private SurfacePosition _next;

        public ApproacherEngine(SurfacePosition position, IActionEngine prevEngine, IMovementClient client, string owner) {
            _position = position;
            _prevEngine = prevEngine;
            _client = client;
            _status = ApproachStatus.BREAKING;
            _owner = owner;
            _attemptStart = DateTime.Now;
            _next = null;
        }

        public Optional<IActionEngine> ReceiveStatus(StatusUpdateModel model) {
            CheckIfOtherMines(model);

            _lastPosition = model.ShipStatus;
            if (_attemptStart.AddSeconds(10) < DateTime.Now) {
                return Optional.From(_prevEngine);
            }

            var location = model.GoldMines.Where(s => s.X == _position.X && s.Y == _position.Y).FirstOrDefault();

            if (location != null && location.Owner == _owner) {
                return Optional.From(_prevEngine);
            }

            if (Math.Abs(_lastPosition.XVelocity) < 0.05 && Math.Abs(_lastPosition.YVelocity) < 0.05 && _status == ApproachStatus.BREAKING) {
                _status = ApproachStatus.APPROACHING;
                var angle = CalculateAngle();
                _angle = angle;
                var distance = CalculateDistance();
                _distanceAwayFromBreak = distance;
                _client.Accelerate(angle, 0.7);
            } else if (_status == ApproachStatus.APPROACHING) {
                _client.Accelerate(_angle, 0.7);
                var currDistnace = CalculateDistance();
                if (currDistnace < 0.35 * _distanceAwayFromBreak) {
                    _client.Break();
                    _status = ApproachStatus.HOMING;
                }
            } else if (_status == ApproachStatus.HOMING) {
                if (location == null) {
                    _client.Break();
                    _status = ApproachStatus.APPROACHING;
                }
                if (location != null && location.Owner == _owner) {
                    //we caught it, time to move on
                    if (_next != null) {
                        var newApproacher = new ApproacherEngine(_next, _prevEngine, _client, _owner);
                        return Optional<IActionEngine>.From(newApproacher);
                    } else {
                        return Optional.From(_prevEngine);
                    }
                } else {
                    _status = ApproachStatus.APPROACHING;
                    var angle = CalculateAngle();
                    var distance = CalculateDistance();
                    _distanceAwayFromBreak = distance;
                    _client.Accelerate(angle, 0.7);
                }
            } else {
                //still break
                _client.Break();
            }

            return Optional<IActionEngine>.FromNull();
        }

        public void Run(StatusUpdateModel model) {
            _client.Break();
        }

        public SurfacePosition GetTarget() {
            return _position;
        }

        private void CheckIfOtherMines(StatusUpdateModel model) {
            if (_next != null) {
                return;
            }
            var targets = model.GoldMines.Where(s => s.Owner != _owner && s != _position);
            if (targets.Count() > 0) {
                _next = targets.First();
            }
        }

        private double CalculateAngle() {
            return CommonTools.CalculateAngle(_lastPosition.XPos, _position.X, _lastPosition.YPos, _position.Y);
        }

        private double CalculateDistance() {
            return CommonTools.CalHypo(_position.X, _lastPosition.XPos, _position.Y, _lastPosition.YPos);
        }
    }

    public enum ApproachStatus { BREAKING, APPROACHING, HOMING }
}
