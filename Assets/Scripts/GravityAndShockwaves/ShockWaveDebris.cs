using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveDebris
{
    private Vector3 m_direction;
    private float m_speedFactor;
    private GameObject m_pieceOfDebris;

    public void UpdateDebris(float currentSpeed, float deltaTime, SpaceManager spaceManager)
    {
        if (m_pieceOfDebris != null)
        {
            Vector3 gravity = Vector3.zero;            
            gravity += spaceManager.GetGravity(m_pieceOfDebris.transform.position);
            m_pieceOfDebris.transform.position += (m_direction * currentSpeed * deltaTime) + Vector3.ClampMagnitude(gravity, 1.0f);
            m_pieceOfDebris.transform.localScale -= Vector3.one * deltaTime;
        }
    }

    public ShockWaveDebris(Vector3 direction, Vector3 location, float speedFactor, GravityManager gravityManager, float lifeTime, Object sampleDebris)
    {
        m_direction = direction.normalized;
        m_speedFactor = speedFactor;

        m_pieceOfDebris = (GameObject)Object.Instantiate(sampleDebris, location, Random.rotation);
        GameObject.Destroy(m_pieceOfDebris, lifeTime);
    }
}
