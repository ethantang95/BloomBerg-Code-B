using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Models {
    public class ShipStatus {
        public double XPos { get; set; }
        public double YPos { get; set; }
        public double XVelocity { get; set; }
        public double YVelocity { get; set; }

        public ShipStatus(double x, double y, double dx, double dy) {
            XPos = x;
            YPos = y;
            XVelocity = dx;
            YVelocity = dy;
        }
    }
}