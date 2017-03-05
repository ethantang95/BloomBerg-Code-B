using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomBerg_Code_B.JNUG_Engine.Interfaces {
    public interface IActionClient : IMovementClient{
        StatusUpdateModel GetStatus();
        StatusUpdateModel Scan(double x, double y);
        ScoreboardModel GetScore();
        ConfigurationModel GetConfig();
    }
}
