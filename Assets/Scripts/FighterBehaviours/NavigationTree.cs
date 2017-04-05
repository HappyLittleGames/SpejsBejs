using UnityEngine;

namespace Assets.BHTree
{
    public class NavigationTree : Sequence
    {
        private FighterBlackboard m_blackboard = null;
        private Propulsion m_propulsion = null;
        private Vector3 m_desiredDirection = Vector3.zero;       
        //private float m_orbitDistance = 10;
        private float m_topSpeed = 40;
 
        public NavigationTree(FighterBlackboard blackboard, Navigator navigator, Propulsion prop)
        {
            m_blackboard = blackboard;
            m_propulsion = prop;
            m_topSpeed = prop.thrust * 5; // this is just bad

            AddBehaviour<Behaviour>().BUpdate = GetDestinationToTarget;
            Selector shootOrTraverse = AddBehaviour<Selector>();
            {
                // completely redundant
                //Sequence shoot = shootOrTraverse.AddBehaviour<Sequence>();
                //{
                //    shoot.AddBehaviour<Condition>().BCanRun = TargetEnteringRange;
                //    shoot.AddBehaviour<Behaviour>().BUpdate = TurnToTarget;
                //}

                Sequence traverse = shootOrTraverse.AddBehaviour<Sequence>();
                {                    
                    traverse.AddBehaviour<Behaviour>().BUpdate = Stabilize;
                    traverse.AddBehaviour<Behaviour>().BUpdate = SetDestination;
                    Sequence stayInFormation = traverse.AddBehaviour<Sequence>();
                    {
                        stayInFormation.AddBehaviour<Condition>().BCanRun = HasWingman;
                        stayInFormation.AddBehaviour<Behaviour>().BUpdate = VectoringThrust;
                    }
                }
                shootOrTraverse.AddBehaviour<Behaviour>().BUpdate = CommonBehaviours.AlwaysSucceed;
            }

            AddBehaviour<Behaviour>().BUpdate = SetThrottle;
        }


        private BHStatus GetDestinationToTarget()
        {
            if (m_blackboard.target != null)
            {
                if (m_blackboard.target.GetComponent<Rigidbody>() != null)
                {
                    // We require a direction that will cause intersect at current velocities
                    //Vector3 behindTarget = -(m_blackboard.target.GetComponent<Rigidbody>().velocity.normalized * (m_blackboard.fighter.weapon.range / 2)); // Good spot for _boldness                    
                    m_desiredDirection = ((m_blackboard.target.transform.position + (m_propulsion.rigidbody.position - m_blackboard.target.transform.position) * 0.1f) + m_blackboard.target.GetComponent<Rigidbody>().velocity * m_blackboard.tickInterval - m_propulsion.rigidbody.velocity * m_blackboard.tickInterval) - m_propulsion.rigidbody.position + m_blackboard.fighter.spaceManager.GetGravity(m_propulsion.rigidbody.position);
                    //float leadingAmount =  Mathf.Clamp(((m_desiredDirection - m_propulsion.rigidbody.position).magnitude / m_propulsion.rigidbody.velocity.magnitude), 0, 1);
                    //m_desiredMovement = m_blackboard.target.transform.position + m_blackboard.target.GetComponent<Rigidbody>().velocity + -m_propulsion.rigidbody.velocity ; // maybe do a little fewer GetComponents :)
                }
                else
                {   // not a good situation
                    m_desiredDirection = m_blackboard.target.transform.position - m_blackboard.parentObject.transform.position - m_propulsion.rigidbody.velocity * m_blackboard.tickInterval;
                }
                return BHStatus.Success;
            }
            else
                return BHStatus.Failure;
        }


        private bool TargetEnteringRange()
        {
            if (Vector3.Distance(m_blackboard.parentObject.transform.position, m_blackboard.target.transform.position) < m_blackboard.fighter.weapon.range)
            {
                return true;
            }
            return false;
        }


        private BHStatus TurnToTarget()
        {
            m_blackboard.navigator.destination = m_blackboard.target.transform.position;
            return BHStatus.Success;
        }


        private BHStatus Stabilize()
        {
            // Debug.Log("magnitude of velocity = " + m_propulsion.rigidbody.velocity.magnitude + ", velocity taking us further from destination.");
            if (Vector3.Distance(m_blackboard.parentObject.transform.position + m_propulsion.rigidbody.velocity.normalized, m_desiredDirection) >
                Vector3.Distance(m_blackboard.parentObject.transform.position, m_desiredDirection))
            {
                // Debug.Log("Setting destination to opposite velocity");
                m_desiredDirection = (-m_propulsion.rigidbody.velocity.normalized * m_topSpeed * 100) - m_blackboard.parentObject.transform.position;
                return BHStatus.Success;
            }
            // but if speed is too damn high, the tick rate on scans is too damn low for updating the destinations like this
            if (m_propulsion.rigidbody.velocity.magnitude > m_topSpeed)
            {
                // Debug.Log("Setting destination to opposite velocity");
                m_desiredDirection = (-m_propulsion.rigidbody.velocity.normalized * m_topSpeed * 100) - m_blackboard.parentObject.transform.position;
                return BHStatus.Success;
            }

            // I guess we're stable
            return BHStatus.Success;
        }        


        private BHStatus SetDestination()
        {               
            m_blackboard.navigator.destination = m_desiredDirection;
            //Debug.DrawLine(m_propulsion.rigidbody.position,  m_propulsion.rigidbody.position + m_desiredDirection, Color.blue, 0.1f);
            return BHStatus.Success;
        }


        private BHStatus SetThrottle()
        {
            if (Vector3.Angle(m_blackboard.parentObject.transform.forward, m_desiredDirection) < 15)
            {
                // throttling really needs to be more clever than this
                m_blackboard.navigator.thrustThrottle = 1;                
            }
            return BHStatus.Success;
        }


        private bool HasWingman()
        {
            if (m_blackboard.wingMan != null)
            {
                return true;
            }
            return false;
        }


        private BHStatus VectoringThrust()
        {
            float distance = Vector3.Distance(m_blackboard.parentObject.transform.position, m_blackboard.wingMan.transform.position);
            float m_squadTightness = 20;            
            Vector3 direction = m_blackboard.wingMan.transform.position - m_blackboard.parentObject.transform.position;  // maintain distance
            float vectoringAmount = distance - m_squadTightness;
            m_propulsion.VectoringThrust(direction, vectoringAmount, Time.fixedDeltaTime);

            Debug.DrawRay(m_blackboard.parentObject.transform.position, direction, Color.red);
                
            
            return BHStatus.Success;
        }


        private BHStatus AlwaysFails()
        {
            Debug.Log("End of NavTree");
            return BHStatus.Failure;
        }
    }
}
