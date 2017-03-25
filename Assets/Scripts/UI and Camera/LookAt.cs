using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject m_beholder;
    [SerializeField] private GameObject m_beholden;

	void Update()
    {
        if (m_beholder && m_beholden)
            m_beholder.transform.LookAt(m_beholden.transform.position);	
        else if (m_beholden)
            gameObject.transform.LookAt(m_beholden.transform.position);
    }
}
