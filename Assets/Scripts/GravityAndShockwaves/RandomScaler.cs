using UnityEngine;
using System.Collections;

public class RandomScaler : MonoBehaviour
{

    public bool isScaling = true;
    [SerializeField]
    private float m_maxScale = 1;
    [SerializeField]
    private float m_minScale = 1;
    [SerializeField]
    public float m_shrinkRate = 1;
    private float m_originalMaxScale = 1;

    [SerializeField]
    private Transform m_transform = null; 

    void Start ()
    {
        m_originalMaxScale = m_maxScale;
        if (m_transform == null)
        {
            m_transform = gameObject.GetComponent<Transform>();
        }
	}
	
	
	void Update ()
    {
        if (isScaling)
        {
            if (m_transform != null)
            {
                float scale = Random.Range(m_minScale, m_maxScale);
                m_transform.localScale = new Vector3(scale, scale, scale);
                m_transform.Rotate(new Vector3(scale, scale, scale));
            }
            m_maxScale -= Mathf.Clamp(m_originalMaxScale * m_shrinkRate * Time.deltaTime, 0, 10000);
        }
	}
}
