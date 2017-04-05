using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave
{

    public Vector3 location { get; private set; }
    public float lifeTime { get; private set; }
    public float strength { get; private set; }
    private float m_peakStrength;
    public float decay { get; private set; }
    public float speed { get; private set; }
    private float m_elapsedTime = 0.0f;
    public bool isActive = true;
    private List<ShockWaveDebris> m_shockWaveDebris = new List<ShockWaveDebris>();

    public ShockWave(Vector3 location, float lifeTime, float strength, float decay, float speed, GravityManager gravityManager, int debrisAmount, Object sampleDebris)
    {
        this.location = location;
        this.lifeTime = lifeTime;
        this.strength = strength;
        m_peakStrength = strength;
        this.decay = decay;
        this.speed = speed;

        SpawnDebris(debrisAmount, gravityManager, sampleDebris);
    }
	
	public void UpdateLifeTime(float deltaTime, SpaceManager spaceManager)
    {
        if (m_elapsedTime > lifeTime)
            isActive = false;
        m_elapsedTime += Time.deltaTime;
        strength -= m_peakStrength * decay * Time.deltaTime;
        speed -= decay * Time.deltaTime;

        UpdateDebris(deltaTime, spaceManager);
    }

    private void UpdateDebris(float deltaTime, SpaceManager spaceManager)
    {
        foreach (ShockWaveDebris debris in m_shockWaveDebris)
        {
            debris.UpdateDebris(speed, deltaTime, spaceManager);
        }
    }

    private void SpawnDebris(int amount, GravityManager gravityManager, Object sampleDebris)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            m_shockWaveDebris.Add(new ShockWaveDebris(randomDirection, location, Random.Range(0.8f, 1.2f), gravityManager, lifeTime, sampleDebris));
        }
    }

    public float WaveFrontOffset()
    {
        return m_elapsedTime * speed;
    }

    public float GetRemainingLifetime()
    {
        return Mathf.Clamp(lifeTime - m_elapsedTime, 0.0f, 1.0f);
    }
}
