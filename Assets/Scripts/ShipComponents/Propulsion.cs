using UnityEngine;


public class Propulsion
{
    private bool m_debugLogs = false;    

    public float turnRate { get; set; }
    public float thrust { get; set; }
    public float vectoringThrust { get; set; }

    public Rigidbody rigidbody { get; private set; }
    private GameObject m_rootObject = null;

    public Propulsion(Rigidbody rigidBody, GameObject rootObject, float thrust, float turnRate)
    {
        rigidbody = rigidBody;
        m_rootObject = rootObject;
        this.thrust = thrust;
        this.turnRate = turnRate;
        vectoringThrust = 1;       
        
    }

    public void ApplyThrottle(float throttle, float fixedDeltaTime)
    {
        {
            if (m_debugLogs)
            {
                Debug.Log("current velocity = " + rigidbody.velocity.magnitude);
            }
            float amount = Mathf.Clamp(throttle, -1f, 1f);
            rigidbody.AddForce((m_rootObject.transform.forward * thrust * amount) * fixedDeltaTime, ForceMode.Impulse);
        }
    }

    public void ApplyGravBreak(float throttle, float fixedDeltaTime)
    {
        float amount = 1 - Mathf.Clamp(throttle, 0f, 1f);
        Vector3 reverseThrust = -rigidbody.velocity.normalized;
        rigidbody.AddForce((reverseThrust * thrust * amount) * fixedDeltaTime, ForceMode.Impulse);
        //Debug.DrawRay(m_rootObject.transform.position, rigidbody.velocity * (thrust * -amount * fixedDeltaTime));
    }

    public void VectoringThrust(Vector3 direction, float throttle, float fixedDeltaTime)
    {
        float amount = 1 - Mathf.Clamp(throttle, -1f, 1f);
        rigidbody.AddForce((direction.normalized * (vectoringThrust * amount)) * fixedDeltaTime, ForceMode.Acceleration); // impulse mode
    }

    public void Rotate(string input, Vector3 rotation)
    {
        //Rigidbody.AddRelativeTorque(rotation.normalized * m_turnRate * amount, ForceMode.Force);

        if (rotation != Vector3.zero)
        {
            switch (input)
            {
                case "pitch":
                    rigidbody.AddTorque((m_rootObject.transform.right * turnRate) * rotation.x, ForceMode.Force);
                    break;
                case "yaw":
                    rigidbody.AddTorque((m_rootObject.transform.up * turnRate) * rotation.y, ForceMode.Force);
                    break;
                case "roll":
                    rigidbody.AddTorque((m_rootObject.transform.forward * turnRate) * rotation.z, ForceMode.Force);
                    break;
                default:
                    Debug.Log(this + ", Rotate took invalid string");
                    break;
            }
        }
    }

    public void FakeRotate(Vector3 desiredDirection, float deltaTime)
    {
        if (desiredDirection != Vector3.zero && desiredDirection.magnitude != 0.0f)
        {
            Quaternion direction = Quaternion.LookRotation(desiredDirection);
            direction = Quaternion.Slerp(rigidbody.rotation, direction, turnRate * deltaTime);
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.MoveRotation(direction);
        }
    }
}

