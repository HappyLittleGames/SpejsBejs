﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager
{
    public GameObject planet = null;
    private List<DeliberateScaler> m_spawnerScalers = new List<DeliberateScaler>();
    private List<DeliberateScaler> m_gravityWellScalers = new List<DeliberateScaler>();
    private List<ShockWave> m_activeShockwaves = new List<ShockWave>();
    public SpaceManager spaceManager { get; private set; }

    public GravityManager(SpaceManager spaceManager, List<GameObject> spawners)
    {
        this.spaceManager = spaceManager;

        foreach (GameObject spawner in spawners)
        {
            DeliberateScaler[] scalers = spawner.GetComponentsInChildren<DeliberateScaler>();
            foreach ( DeliberateScaler scaler in scalers )
            {
                if (scaler.gameObject == spawner)
                {
                    m_spawnerScalers.Add(scaler);
                    Debug.Log("added a spawnerScaler");
                }
                else
                {
                    m_gravityWellScalers.Add(scaler);
                    Debug.Log("added a gravityScaler");
                }
            }
        }
    }

    
    public void UpdateGravities(float deltaTime)
    {
        //Debug.Log("Wave fronts: " + m_activeShockwaves.Count);
        for (int i = m_activeShockwaves.Count - 1; i >= 0; i--)
        {
            if (!m_activeShockwaves[i].isActive)
                m_activeShockwaves.RemoveAt(i);
            else
                m_activeShockwaves[i].UpdateLifeTime(deltaTime, spaceManager);
        }
    }


    public Vector3 GetBlackHoleInfluenceAtPosition(Vector3 position, float power = 25000.0f)
    {
        Vector3 gravity = Vector3.zero;

        foreach (DeliberateScaler gravityWell in m_gravityWellScalers)
        {
            float gravityOverDistance = (1 * power) / Vector3.SqrMagnitude(gravityWell.GetPosition() - position);

            gravityOverDistance /= ((gravityWell.GetMaxScale() == 0) ? 1 : gravityWell.GetMaxScale());
            gravityOverDistance *= gravityWell.GetCurrentScale();

            gravity += (gravityWell.GetPosition() - position).normalized * gravityOverDistance;
        }
        return gravity;
    }


    public Vector3 GetFakeShockwaveInfluenceAtPosition(Vector3 position, float power, List<GameObject> shockWaves)
    {
        Vector3 gravity = Vector3.zero;

        foreach (GameObject shockWaveOrigin in shockWaves)
        {
            if (shockWaveOrigin != null)
            {
                float gravityOverDistance = (-1 * power) / Vector3.SqrMagnitude(shockWaveOrigin.transform.position - position);
                //gravityOverDistance /= ((shockWaveOrigin.transform.localScale.x >= 0) ? 1 : shockWaveOrigin.transform.localScale.x);
                //gravityOverDistance *= shockWaveOrigin.transform.localScale.x;

                gravity += (shockWaveOrigin.transform.position - position).normalized * gravityOverDistance;
            }
        }
        return gravity;
    }


    public void AddShockwave(GameObject entity, float lifeTime, float strength, float decay, float speed, int debrisAmount, Object sampleDebris)
    {
        if (m_activeShockwaves.Count >= 1)
        {
            for (int i = m_activeShockwaves.Count - 1; i >= 0; i--)
            {
                if (m_activeShockwaves[i] == null)
                    m_activeShockwaves.RemoveAt(i);
            }
        }
        m_activeShockwaves.Add(new ShockWave(entity.transform.position, lifeTime, strength, decay, speed, this, debrisAmount, sampleDebris));
    }


    public Vector3 GetShockwaveInfluenceAtPosition(Vector3 position, float power = 1000.0f)
    {
        Vector3 force = Vector3.zero;

        foreach (ShockWave shockwave in m_activeShockwaves)
        {
            if (shockwave != null)
            {
                if ((shockwave.location - position).magnitude > 0.01f) // checking towards ourselves causes NaN vectors
                {
                    if ((shockwave.location - position).magnitude < shockwave.WaveFrontOffset())
                    {
                        float strength = (-1 * shockwave.strength);
                        force += (shockwave.location - position).normalized * strength;
                    }
                }
            }
        }
        return force;
    }

    public List<DeliberateScaler> GetTeamScalers(int index)
    {
        List<DeliberateScaler> scalers = new List<DeliberateScaler>();
        if (m_spawnerScalers.Count > index)
            scalers.Add(m_spawnerScalers[index]);
        if (m_gravityWellScalers.Count > index)
            scalers.Add(m_gravityWellScalers[index]);
        return scalers;
    }

    public List<DeliberateScaler> GetAllScalers()
    {
        List<DeliberateScaler> scalers = new List<DeliberateScaler>();
        foreach (DeliberateScaler scaler in m_gravityWellScalers)
        {
            scalers.Add(scaler);
        }
        foreach (DeliberateScaler scaler in m_spawnerScalers)
        {
            scalers.Add(scaler);
        }
        return scalers;
    }    
}
