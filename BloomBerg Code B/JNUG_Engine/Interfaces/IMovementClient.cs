using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomBerg_Code_B.JNUG_Engine.Interfaces {
    public interface IMovementClient {
        bool Accelerate(double direction, double magnitude);
        bool Break();
        bool Bomb(double x, double y, int fuse = 0);
    }
}
