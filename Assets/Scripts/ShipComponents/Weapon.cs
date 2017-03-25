using UnityEngine;
using System.Collections;

public abstract class Weapon
{
    protected Color m_color;    
    public float range { get; protected set; }
    public float accuracy { get; protected set; }
    protected GameObject m_weaponVisuals = null;
    protected float m_coolDown = 0;
    protected float m_coolingTime = 0;

    public virtual void Update(float deltaTime)
    {
        m_coolingTime += deltaTime;
    }

    public abstract GameObject Pew(Vector3 start, Vector3 direction, float dur);
}
