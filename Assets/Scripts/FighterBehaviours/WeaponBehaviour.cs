    using UnityEngine;

namespace Assets.BHTree
{
    public class WeaponBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Weapon m_weapon = null;

        public WeaponBehaviour(FighterBlackboard blackboard, Weapon weapon)
        {
            m_blackboard = blackboard;
            m_weapon = weapon;

            AddBehaviour<Condition>().BCanRun = HasTarget;
            AddBehaviour<Condition>().BCanRun = IsTargetingEnemy;
            AddBehaviour<Behaviour>().BUpdate = Reloading;
            AddBehaviour<Behaviour>().BUpdate = TakingAim;
            AddBehaviour<Behaviour>().BUpdate = OpeningFire;      
        }


        private bool HasTarget()
        {
            if (m_blackboard.target != null)
                return true;
            else
                return false;
        }


        private bool IsTargetingEnemy()
        {
            if (m_blackboard.target.GetComponent<Fighter>() != null)
            {
                if (m_blackboard.target.GetComponent<Fighter>().teamNumber != m_blackboard.fighter.teamNumber)
                {
                    return true;
                }
                else
                {                
                    return false;
                }
            }
            if (m_blackboard.target == m_blackboard.fighter.enemyMothership)
            {
                return true;
            }
            else
                return false;
        }


        private BHStatus TakingAim()
        {                       
            if ((m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position).magnitude <= m_weapon.range)
            {
                // Debug.Log("Taking Aim (Debug.AnyKey to fire)");
                if (Vector3.Angle(m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position, m_blackboard.parentObject.transform.forward) < m_weapon.accuracy)
                {
                        // Debug.Log("Target locked, Weapons Free");
                        return BHStatus.Success;
                }
                return BHStatus.Failure;
            }
            else
            {
                // Debug.Log("Target not in Range");
                return BHStatus.Failure;
            }          
        }


        private BHStatus OpeningFire()
        {
            // Debug.Log("Opening Fire");
            GameObject hit = m_weapon.Pew(m_blackboard.parentObject.transform.position, m_blackboard.parentObject.transform.forward, 0.1f);
            if (hit != null && hit.GetComponent<Fighter>() != null)
            {
                // GameObject.Destroy(hit);
                hit.GetComponent<Fighter>().isExploding = true;
            }
            else if (hit != null && hit.GetComponent<Scripts.SpawnShips>() != null)
            {
                hit.GetComponent<Scripts.SpawnShips>().TakeDamage(1);
            }
            return BHStatus.Success;
        }


        private BHStatus Reloading()
        {
            // Debug.Log("Attempting to recharge");
            return BHStatus.Success;
        }
    }
}
