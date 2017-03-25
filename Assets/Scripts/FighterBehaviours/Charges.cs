using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BHTree
{
    class Charges : Sequence
    {
        private int m_charges;
        private int m_capacity;
        private FighterBlackboard m_blackboard;

        public Charges(FighterBlackboard blackboard, int amount, int capacity)
        {
            m_blackboard = blackboard;
            m_charges = amount;
            m_capacity = capacity;
                       
            AddBehaviour<Behaviour>().BUpdate = UseCharges;
            AddBehaviour<Condition>().BCanRun = IsOutOfCharges;
            AddBehaviour<Behaviour>().BUpdate = NeedsRecharge;
        }

        public BHStatus UseCharges(/* int amount */)
        {
            int amount = 1;
            if (m_charges < amount)
            {
                return BHStatus.Failure;
            }
            m_charges = Mathf.Clamp(m_charges - amount, 0, m_capacity);
            return BHStatus.Success;
        }

        public bool IsOutOfCharges()
        {
            return m_charges <= 0;
        }

        public BHStatus NeedsRecharge()
        {
            if (m_charges == m_capacity)
            {
                return BHStatus.Success;
            }
            else
            {
                // m_blackboard.fighter.SetDestObject(m_spaceManager.GetTeams().GetBase(m_blackboard.fighter.gameObject.tag));
                return BHStatus.Failure;

                // if close to reload thing dock up and get ammo?
            }
        }
    }
}

