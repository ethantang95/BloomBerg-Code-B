using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using BloomBerg_Code_B.JNUG_Engine.Interfaces;

namespace BloomBerg_Code_B.JNUG_Engine.ActionEngines {
    public class LongRangeApproacherEngine : IActionEngine {

        private SurfacePosition _position;
        private IActionEngine _prevEngine;
        private ShipStatus _lastPosition;
        private IMovementClient _client;
        private ApproachStatus _status;
        private string _owner;
        private DateTime _attemptStart;
        private double _distanceAwayFromBreak;

        public LongRangeApproacherEngine(SurfacePosition position, IActionEngine prevEngine, IMovementClient client, string owner) {
            _client = client;
            _prevEngine = prevEngine;
            _position = position;
            _owner = owner;
            _attemptStart = DateTime.Now;
        }

        public Optional<IActionEngine> ReceiveStatus(StatusUpdateModel model) {
            _lastPosition = model.ShipStatus;
            if (_attemptStart.AddSeconds(30) < DateTime.Now) {
                return Optional.From(_prevEngine);
            }

            var location = model.GoldMines.Where(s => s.X == _position.X && s.Y == _position.Y).FirstOrDefault();

            if (_position.Owner == _owner) {
                return Optional.From(_prevEngine);
            }

            if (_lastPosition.XVelocity < 0.01 && _lastPosition.YVelocity < 0.01 && _status == ApproachStatus.BREAKING) {
                _status = ApproachStatus.APPROACHING;
                var angle = CalculateAngle();
                var distance = CalculateDistance();
                _distanceAwayFromBreak = distance;
                _client.Accelerate(angle, 1);
            } else if (_status == ApproachStatus.APPROACHING) {
                var currDistnace = CalculateDistance();
                if (currDistnace < 100) {
                    _client.Break();
                    _status = ApproachStatus.HOMING;
                }
            } else if (_status == ApproachStatus.HOMING) {
                if (location.Owner == _owner) {
                    //we caught it, time to move on
                    return Optional.From(_prevEngine);
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

        private double CalculateAngle() {
            var xDiff = _position.X - _lastPosition.XPos;
            var yDiff = _position.Y - _lastPosition.YPos;

            var beta = Math.Atan(yDiff / xDiff);

            var quad = FindQuadrant();
            if (quad == 2) {
                beta = beta + Math.PI;
            }
            if (quad == 3) {
                beta = beta + Math.PI;
            }
            if (quad == 4) {
                beta = beta + (2 * Math.PI);
            }

            return beta;
        }

        private int FindQuadrant() {
            if (_position.X > _lastPosition.XPos && _position.Y > _lastPosition.YPos) {
                return 1;
            } else if (_position.X < _lastPosition.XPos && _position.Y > _lastPosition.YPos) {
                return 2;
            } else if (_position.X < _lastPosition.XPos && _position.Y < _lastPosition.YPos) {
                return 3;
            } else {
                return 4;
            }
        }

        private double CalculateDistance() {
            return CommonTools.CalHypo(_position.X, _lastPosition.XPos, _position.Y, _lastPosition.YPos);
        }
    }
}