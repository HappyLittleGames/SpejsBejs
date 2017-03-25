using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave
{

    public Vector3 location { get; private set; }
    public float lifeTime { get; private set; }
    public float strength { get; private set; }
    public float decay { get; private set; }
    public float speed { get; private set; }
    private float m_elapsedTime = 0.0f;
    public bool isActive = true;

    public ShockWave(Vector3 location, float lifeTime, float strength, float decay, float speed)
    {
        this.location = location;
        this.lifeTime = lifeTime;
        this.strength = strength;
        this.decay = decay;
        this.speed = speed;
    }
	
	public void UpdateLifeTime(float deltaTime)
    {
        if (m_elapsedTime > lifeTime)
            isActive = false;
        m_elapsedTime += Time.deltaTime;
        strength -= decay * Time.deltaTime;
    }

    public float WaveFrontOffset()
    {
        return m_elapsedTime * speed;
    }
}
