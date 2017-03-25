using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.BHTree
{
    public class ScanningBehaviour : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        
        public ScanningBehaviour(FighterBlackboard blackboard)
        {
            m_blackboard = blackboard;

            Sequence GetWingman = AddBehaviour<Sequence>();
            {                
                GetWingman.AddBehaviour<Behaviour>().BUpdate = TargetClosestFriendly;
                // all that squadron stuff
            }

            Selector engageOrRetreat = AddBehaviour<Selector>();
            {
                Sequence engage = engageOrRetreat.AddBehaviour<Sequence>();
                {
                    engage.AddBehaviour<Condition>().BCanRun = HasEnemy;
                    engage.AddBehaviour<Behaviour>().BUpdate = TargetClosestEnemy;
                }

                Selector retreat = engageOrRetreat.AddBehaviour<Selector>();
                {                    
                    Sequence goHome = retreat.AddBehaviour<Sequence>();
                    {
                        goHome.AddBehaviour<Condition>().BCanRun = HasMothership;
                        goHome.AddBehaviour<Behaviour>().BUpdate = TargetMothership;
                    }
                    goHome.AddBehaviour<Behaviour>().BUpdate = TargetEnemyMothership;
                }
                
                engageOrRetreat.AddBehaviour<Behaviour>().BUpdate = CommonBehaviours.AlwaysSucceed;
            }
        }


        private BHStatus AlwaysFail()
        {
            // Debug.Log("End of ScanTree");
            return BHStatus.Failure;
        }



        private bool HasEnemy()
        {
            if (m_blackboard.GetEnemies().Count > 0)
                return true;
            else
                return false;
        }


        private bool HasFriendly()
        {
            if (m_blackboard.spaceManager.shipCounter.fighterTeams[m_blackboard.fighter.teamNumber].Count > 1) // more than just this
                return true;
            else
                return false;
        }      


        private BHStatus TargetClosestEnemy()
        {
            if (m_blackboard.GetEnemies().Count > 0)
            {
                List<GameObject> sortedByRange = m_blackboard.GetEnemies().OrderBy(x => Vector3.Distance(m_blackboard.parentObject.transform.position, x.transform.position)).ToList();
                m_blackboard.target = sortedByRange[0];
            }
            else
            {
                m_blackboard.target = null;
            }
            return BHStatus.Success;
        }


        private BHStatus TargetClosestFriendly()
        {
            if (m_blackboard.spaceManager.shipCounter.fighterTeams[m_blackboard.fighter.teamNumber].Count > 1)
            {
                // Debug.Log("Targeting Wingman");
                List<GameObject> sortedByRange = m_blackboard.GetFriendlies().OrderBy(x => Vector3.Distance(m_blackboard.parentObject.transform.position, x.transform.position)).ToList();
                // because 0 is this, we go for 1. Pretty shitty
                m_blackboard.wingMan = sortedByRange[1];
            }
            else
                m_blackboard.wingMan = null;
            return BHStatus.Success;
        }


        private bool HasMothership()
        {
            if (m_blackboard.mothership != null)
            {
                // Debug.Log("Gunna go home now");
                return true;                
            }
            else
            {
                return false;
            }         
        }
        

        private BHStatus TargetMothership()
        {
            // Debug.Log("Going home");
            m_blackboard.wingMan = m_blackboard.mothership;
            return BHStatus.Success;
        }


        private BHStatus TargetEnemyMothership()
        {
            // Debug.Log("Going home");
            m_blackboard.target = m_blackboard.fighter.enemyMothership;
            return BHStatus.Success;
        }
    }
}
