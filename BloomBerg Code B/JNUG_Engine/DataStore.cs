using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.JNUG_Engine {
    public class DataStore {
        public Dictionary<string, SurfacePosition> Mines { get; set; }
        public ScoreboardModel Scoreboard { get; set; }
        public ConfigurationModel Configuration { get; set; }
        public ShipStatus ShipStatus { get; set; }
        public DateTime LastBombDropped { get; set; }
        public DateTime LastScan { get; set; }
        public StatusUpdateModel LastVisionResults { get; set; }
        public StatusUpdateModel LastScanResults { get; set; }
        public SurfacePosition TargetDirection { get; set; }

        public DataStore() {
            Mines = new Dictionary<string, SurfacePosition>();
        }
    }
}