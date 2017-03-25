using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Decorator : Composite
    {
        public Func<bool> BCanRun { protected get; set; }
        public BHStatus DefaultReturnStatus { protected get; set; }

        public Decorator()
        {
            BUpdate = () =>
            {
                if ((BCanRun != null) && (BCanRun()) && (Children != null) && (ChildCount() > 0))
                {
                    return Children[0].BTick();
                }
                return DefaultReturnStatus;
            };
        }
    }
}
