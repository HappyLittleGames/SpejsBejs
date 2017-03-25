using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public class Sequence : Composite
    {
        protected int m_sequence = 0;
        public Sequence()
        {
            BUpdate = () =>
            {
                for (;;)
                {
                    BHStatus status = GetChild(m_sequence).BTick();
                    if (status != BHStatus.Success)
                    {
                        if (status == BHStatus.Failure)
                        {
                            m_sequence = 0;
                        }
                        return status;
                    }
                    if (++m_sequence == ChildCount())
                    {
                        m_sequence = 0;
                        return BHStatus.Success;
                    }
                }
            };

            BInitialize = () => { m_sequence = 0; };
        }

        public void SetSequence(List<IBehaviour> behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                Children.Add(behaviour);
            }
        }
    }

}