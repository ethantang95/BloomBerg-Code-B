using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Models {
    public class ScoreboardModel {
        public List<ScoreboardEntry> Entries { get; set; }

        public ScoreboardModel(List<ScoreboardEntry> entires) {
            Entries = entires;
        }
    }

    public class ScoreboardEntry : IComparable{
        public string PlayerName { get; set; }
        public int Points { get; set; }
        public int MinesOwned { get; set; }

        public ScoreboardEntry(string player, int points, int mines) {
            PlayerName = player;
            Points = points;
            MinesOwned = mines;
        }

        public int CompareTo(object obj) {
            if (!(obj is ScoreboardEntry)) {
                throw new Exception("Bad object comparison");
            }

            if (obj == null) {
                return 1;
            }

            var toCompare = obj as ScoreboardEntry;

            if (Points > toCompare.Points) {
                return 1;
            } else if (Points < toCompare.Points) {
                return -1;
            }

            if (MinesOwned > toCompare.MinesOwned) {
                return 1;
            } else if (MinesOwned < toCompare.MinesOwned) {
                return -1;
            }

            return PlayerName.CompareTo(toCompare.PlayerName);
        }
    }
}