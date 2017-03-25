using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    public class Regulator : Decorator
    {
        private float m_tickTimer = 0;
        private float m_tickInterval = 0;
        public Regulator(Behaviour behaviour, float tickInterval)
        {
            Children.Add(behaviour);
            m_tickInterval = tickInterval;
            DefaultReturnStatus = BHStatus.Failure;
            this.BCanRun = TickTimer;
        }

        private bool TickTimer()
        {
            m_tickTimer += Time.deltaTime;
            if (m_tickTimer > m_tickInterval)
            {
                m_tickTimer = 0;
                return true;
            }
            else
                return false;
        }
    }
}
