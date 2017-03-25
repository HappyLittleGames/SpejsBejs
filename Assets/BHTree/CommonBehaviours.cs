using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    static class CommonBehaviours
    {
        public static BHStatus AlwaysFail()
        {
            return BHStatus.Failure;
        }

        public static BHStatus AlwaysSucceed()
        {
            return BHStatus.Success;
        }
    }
}
