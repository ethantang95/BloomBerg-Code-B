using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Parser {
    public class ConfigurationParser : ParserAbstract<ConfigurationModel> {

        private ConfigurationModel _model;

        public ConfigurationParser(string toParse) : base(toParse) {
            _model = new ConfigurationModel();
        }

        public override ConfigurationModel GetModel() {
            PopString();
            PopString();
            _model.MapWidth = PopInt();
            PopString();
            _model.MapHeight = PopInt();
            PopString();
            _model.CaptureRadius = PopDouble();
            PopString();
            _model.VisionRadius = PopDouble();
            PopString();
            _model.Friction = PopDouble();
            PopString();
            _model.BrakeFriction = PopDouble();
            PopString();
            _model.BombPlaceRadius = PopDouble();
            PopString();
            _model.BombEffectRadius = PopDouble();
            PopString();
            _model.BombDelay = PopInt();
            PopString();
            _model.BombPower = PopDouble();
            PopString();
            _model.ScanRadius = PopDouble();
            PopString();
            _model.ScanDelay = PopInt();

            return _model;
        }
    }
}