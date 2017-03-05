using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Models {
    public class SurfacePosition {
        public double X { get; set; }
        public double Y { get; set; }
        public string Owner { get; set; }
        public SurfacePosition(string owner, double x, double y) {
            X = x;
            Y = y;
            Owner = owner;
        }

        public override int GetHashCode() {
            return $"{X} {Y}".GetHashCode();
        }

        public string GetHashString() {
            return $"{X} {Y}";
        }

        public override bool Equals(object obj) {
            if (!(obj is SurfacePosition)) {
                return false;
            }

            var model = obj as SurfacePosition;

            return X == model.X && Y == model.Y;
        }
    }
}