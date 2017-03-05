using BloomBerg_Code_B.Client;
using BloomBerg_Code_B.JNUG_Engine;
using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BloomBerg_Code_B.Controllers
{
    [RoutePrefix("Core")]
    public class CoreController : ApiController {

        [HttpGet]
        [Route("Updates")]
        public UIContentInfoModel GetUpdates() {
            try {
                var data = JnugEngine.GetJnug().Data;

                var model = new UIContentInfoModel();
                model.Engine = JnugEngine.GetJnug()?.GetEngineType();
                model.ShipX = data?.ShipStatus?.XPos;
                model.ShipY = data?.ShipStatus?.YPos;
                model.ShipDx = data?.ShipStatus?.XVelocity;
                model.ShipDy = data?.ShipStatus?.YVelocity;
                model.TargetX = data?.TargetDirection?.X;
                model.TargetY = data?.TargetDirection?.Y;
                model.MinesFound = data?.Mines?.Count;
                model.MinesCount = data?.LastVisionResults?.GoldMines?.Count;
                model.PlayersCount = data?.LastVisionResults?.OtherShips?.Count;
                model.BombsCount = data?.LastVisionResults?.Bombs?.Count;
                model.ScoreboardEntries = data?.Scoreboard?.Entries;
                model.Mines = data?.Mines?.Values.ToList();

                return model;
            } catch (Exception ex) {
                return new UIContentInfoModel();
            }
        }

        // POST: api/Activate
        [HttpPost]
        [Route("Start")]
        public void Start() {
            ThrowIfNotLoggedIn();
            BloomBergActionClient.CreateClient(BloomBergLoginClient.Client.GetAgent());
            var client = BloomBergActionClient.Client;
            JnugEngine.Start(client, BloomBergLoginClient.Client.GetAgent().GetLoginName());
        }

        private void ThrowIfNotLoggedIn() {
            if (!BloomBergLoginClient.IsLoggedIn()) {
                throw new Exception("The client is not yet logged in, the AI cannot be activated");
            }
        }
    }
}
