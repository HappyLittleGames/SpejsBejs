using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Behaviour : IBehaviour
    {
        public BHStatus BStatus { get; set; }
        public Action BInitialize { protected get; set; }
        public Func<BHStatus> BUpdate { protected get; set; }
        public Action<BHStatus> BTerminate { protected get; set; }
        public IBehaviour BParent { get; set; }
        public BHStatus BTick()
        {
            if ((BStatus == BHStatus.Invalid) && (BInitialize != null))
            {
                BInitialize();
            }

            BStatus = BUpdate();

            if ((BStatus != BHStatus.Running) && (BTerminate != null))
            {
                BTerminate(BStatus);
            }

            return BStatus;
        }
    }
}