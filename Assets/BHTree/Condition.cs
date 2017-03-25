using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Condition : Behaviour
    {
        public Condition()
        {
            BUpdate = () =>
            {
                if ((BCanRun != null) && (BCanRun()))
                {
                    return BHStatus.Success;
                }
                return BHStatus.Failure;
            };
        }

        public Func<bool> BCanRun { protected get; set; }
    }
}
