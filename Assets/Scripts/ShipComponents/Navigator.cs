using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.BHTree
{
    public class Navigator
    {
        private Propulsion m_propulsion;
        private Vector3 m_yoke = Vector3.zero;
        public float thrustThrottle { private get; set; }
        public Vector3 destination { get; set; }        

        public Navigator(Propulsion prop)
        {
            m_propulsion = prop;
            destination = prop.rigidbody.position;           
        }


        public void Navigate(float fixedDeltaTime)
        {
            //these needs some more work
            //m_propulsion.Rotate("pitch", m_yoke);
            //m_propulsion.Rotate("yaw", m_yoke);
            m_propulsion.FakeRotate(destination, fixedDeltaTime);
            m_propulsion.ApplyThrottle(thrustThrottle, fixedDeltaTime);          
        }


        public void SetYoke(FighterBlackboard blackboard, Vector3 destination)
        {
            this.destination = destination;

            float pitchAngle = Mathf.Asin(Vector3.Cross(this.destination.normalized, blackboard.parentObject.transform.forward).x) * Mathf.Rad2Deg;
            float yawAngle = Mathf.Asin(Vector3.Cross(this.destination.normalized, blackboard.parentObject.transform.forward).y) * Mathf.Rad2Deg;
            // float rollAngle  = Mathf.Asin(Vector3.Cross(targetDirection.normalized, blackboard.parentObject.transform.forward).z) * Mathf.Rad2Deg;

            // Add 90 degrees to Arcsin curve if destination is behind transform
            if (Vector3.Distance(destination, blackboard.parentObject.transform.position) <
                Vector3.Distance(destination, blackboard.parentObject.transform.position + blackboard.parentObject.transform.forward * .01f))
            {
                // some inaccuracy at 90-95 degrees due to checking slightly in front of transform
                // Debug.Log("testTarget is behind transform");
                if (pitchAngle != 0)
                    pitchAngle = (pitchAngle > 0) ? (180 - pitchAngle) : (-180 + Mathf.Abs(pitchAngle));
                if (yawAngle != 0)
                    yawAngle = (yawAngle > 0) ? (180 - yawAngle) : (-180 + Mathf.Abs(yawAngle));
            }

            Vector3 localVelocity = blackboard.parentObject.transform.InverseTransformDirection(m_propulsion.rigidbody.angularVelocity);

            // Debug.Log("amount of Pitch: " + - (pitchAngle + (localVelocity.x * Mathf.Rad2Deg)) + " /// amount of Yaw: " + -(pitchAngle + (localVelocity.y * Mathf.Rad2Deg)));
            m_yoke.x = -(pitchAngle + (localVelocity.x * Mathf.Rad2Deg)) / 180;
            m_yoke.y = -(yawAngle + (localVelocity.y * Mathf.Rad2Deg)) / 180;
            m_yoke.z = 0;
        }
    }
}
