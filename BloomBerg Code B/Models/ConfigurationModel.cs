using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Models {
    public class ConfigurationModel {
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public double CaptureRadius { get; set; }
        public double VisionRadius { get; set; }
        public double Friction { get; set; }
        public double BrakeFriction { get; set; }
        public double BombPlaceRadius { get; set; }
        public double BombEffectRadius { get; set; }
        public int BombDelay { get; set; }
        public double BombPower { get; set; }
        public double ScanRadius { get; set; }
        public int ScanDelay { get; set; }
    }
}