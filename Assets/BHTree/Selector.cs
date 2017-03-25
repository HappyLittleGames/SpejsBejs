using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Selector : Composite
    {
        private int m_selection;
        public Selector()
        {
            BUpdate = () =>
            {
                for (;;)
                {
                    BHStatus status = GetChild(m_selection).BTick();
                    if (status != BHStatus.Failure)
                    {
                        if (status == BHStatus.Success)
                        {
                            m_selection = 0;
                            return BHStatus.Success;
                        }
                    }
                    if (++m_selection >= ChildCount())
                    {
                        m_selection = 0;
                        return BHStatus.Failure;
                    }
                }
            };
            BInitialize = () =>  { m_selection = 0; };
        }
    }
}
