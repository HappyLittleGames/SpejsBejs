using UnityEngine;
using System.Collections;

public class RotateABit : MonoBehaviour
{

    [SerializeField]
    float m_rotX;
    [SerializeField]
    float m_rotY;
    [SerializeField]
    float m_rotZ;

    void FixedUpdate()
    {
        gameObject.transform.Rotate((new Vector3(m_rotX, m_rotY, m_rotZ) * Time.fixedDeltaTime));
    }
}