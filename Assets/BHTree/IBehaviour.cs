using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public interface IBehaviour
    {
        BHStatus BTick();

        BHStatus BStatus { get; }
        IBehaviour BParent { get; set; }
        Action BInitialize { set; }
        Func<BHStatus> BUpdate { set; }
        Action<BHStatus> BTerminate { set; }
    }
}