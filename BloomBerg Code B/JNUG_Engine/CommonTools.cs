using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.JNUG_Engine {
    public static class CommonTools {
        private static Random _seed;

        static CommonTools() {
            _seed = new Random();
        }
        public static double RadToDeg(double rad) {
            return rad * 180 / Math.PI;
        }

        public static double DegToRad(double deg) {
            return deg * Math.PI / 180;
        }

        public static double CalHypo(double x1, double x2, double y1, double y2) {
            var xDiff = Math.Abs(x1 - x2);
            var yDiff = Math.Abs(y1 - y2);

            return Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
        }

        public static int GetRandom(int max) {
            return _seed.Next(max);
        }

        public static double GetRandomDegree() {
            return _seed.Next(360);
        }

        public static double CalculateAngle(double x1, double x2, double y1, double y2) {
            var xDiff = x2 - x1;
            var yDiff = y2 - y1;

            var beta = Math.Atan(yDiff / xDiff);

            var quad = FindQuadrant(x1, x2, y1, y2);
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

        private static int FindQuadrant(double x1, double x2, double y1, double y2) {
            if (x2 > x1 && y2 > y1) {
                return 1;
            } else if (x2 < x1 && y2 > y1) {
                return 2;
            } else if (x2 < x1 && y2 < y1) {
                return 3;
            } else {
                return 4;
            }
        }
    }
}