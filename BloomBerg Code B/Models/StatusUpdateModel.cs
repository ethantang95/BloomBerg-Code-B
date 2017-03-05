using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Models {
    public class StatusUpdateModel {
        public bool Available { get; set; }
        public ShipStatus ShipStatus { get; set; }
        public List<SurfacePosition> GoldMines { get; set; }
        public List<ShipStatus> OtherShips { get; set; }
        public List<SurfacePosition> Bombs { get; set; }
        public string Raw { get; set; }

        public StatusUpdateModel() { }

        public StatusUpdateModel(bool available, ShipStatus shipStatus,
            List<SurfacePosition> mines, List<ShipStatus> otherShips, List<SurfacePosition> bombs) {
            Available = available;
            ShipStatus = shipStatus;
            GoldMines = mines;
            OtherShips = otherShips;
            Bombs = bombs;
        }

        public static StatusUpdateModel MakeUnAvailableModel() {
            var model = new StatusUpdateModel();
            model.Available = false;
            return model;
        }
    }
}