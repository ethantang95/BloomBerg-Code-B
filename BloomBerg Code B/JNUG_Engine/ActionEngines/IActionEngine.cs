using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomBerg_Code_B.JNUG_Engine.ActionEngines {
    public interface IActionEngine {

        Optional<IActionEngine> ReceiveStatus(StatusUpdateModel model);
        void Run(StatusUpdateModel model);
    }
}
