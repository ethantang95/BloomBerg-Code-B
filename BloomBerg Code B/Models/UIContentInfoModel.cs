using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Models {
    public class UIContentInfoModel {
        public string Engine { get; set; }
        public double? ShipX { get; set; }
        public double? ShipY { get; set; }
        public double? ShipDx { get; set; }
        public double? ShipDy { get; set; }
        public double? TargetX { get; set; }
        public double? TargetY { get; set; }
        public int? MinesFound { get; set; }
        public int? MinesCount { get; set; }
        public int? PlayersCount { get; set; }
        public int? BombsCount { get; set; }
        public List<ScoreboardEntry> ScoreboardEntries { get; set; }
        public List<SurfacePosition> Mines { get; set; }
    }
}