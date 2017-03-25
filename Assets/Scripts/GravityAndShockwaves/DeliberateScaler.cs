using UnityEngine;
using System.Collections;

public class DeliberateScaler : MonoBehaviour
{

    public bool isScaling = true;
    [SerializeField]
    public bool isGrowing = true;
    [SerializeField]
    private float m_maxScale = 1;
    [SerializeField]
    private float m_minScale = 1;
    [SerializeField]
    public float m_scalingRate = 1;

    [SerializeField]
    private Transform m_transform = null;

    void Start()
    {
        if (m_transform == null)
        {
            m_transform = gameObject.GetComponent<Transform>();
        }
    }


    void Update()
    {
        if (m_transform != null)
        {
            if (isScaling)
            {
                if (isGrowing)
                {               
                    float scale = Mathf.Clamp(m_transform.localScale.x + Time.deltaTime * m_scalingRate * 1, m_minScale, m_maxScale);
                    // scale *= Mathf.Clamp(m_transform.lossyScale.x, 1, 100);
                    m_transform.localScale = new Vector3(scale, scale, scale);                              
                }
                else
                {
                    float scale = Mathf.Clamp(m_transform.localScale.x - Time.deltaTime * m_scalingRate * 5, m_minScale, m_maxScale);
                    m_transform.localScale = new Vector3(scale, scale, scale);                    
                }
            }
        }
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public float GetCurrentScale()
    {
        return m_transform.localScale.x;
    }

    public float GetMaxScale()
    {
        return m_maxScale;
    }
}
