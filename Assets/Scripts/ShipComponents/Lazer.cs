using System;
using System.Collections.Generic;
using UnityEngine;


public class Lazer : Weapon
{
    public Lazer(Color lazerColor, float range, float accuracy, GameObject weaponVisuals)
    {
        m_coolDown = .3f;
        m_color = lazerColor;

        this.range = range;
        this.accuracy = accuracy;
        if (weaponVisuals)
        {
            weaponVisuals.transform.localRotation = Quaternion.Euler(90, 0, 0);
            weaponVisuals.transform.localScale = new Vector3(0.0f, range / 2, 0.0f);
            weaponVisuals.transform.localPosition += new Vector3(0, 0, range / 2);
            this.m_weaponVisuals = weaponVisuals;
        }
    }

    public override GameObject Pew(Vector3 start, Vector3 direction, float dur)
    {
        if (m_coolingTime > m_coolDown)
        {
            m_coolingTime = 0;
            //Debug.DrawRay(start, (direction.normalized * range), m_color, dur);
            Pew(start, direction);
            RaycastHit hit;
            if (Physics.Raycast(start, (direction.normalized), out hit, range))
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }
        else return null;
    }


    private void Pew(Vector3 start, Vector3 direction)
    {
        // material method
        if (m_weaponVisuals)
            m_weaponVisuals.transform.localScale = new Vector3(0.6f, m_weaponVisuals.transform.localScale.y, 0.6f);

        // dirty draw method
        // DrawLine(start, direction.normalized * range, m_color);
    }

    // Someguy McRandomperson's method from UnityAsk
    //void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
    //{
    //    GameObject myLine = new GameObject();
    //    myLine.transform.position = start;
    //    myLine.AddComponent<LineRenderer>();
    //    LineRenderer lr = myLine.GetComponent<LineRenderer>();
    //    lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
    //    lr.SetColors(color, color);
    //    lr.SetWidth(0.5f, 0.5f);
    //    lr.SetPosition(0, start);
    //    lr.SetPosition(1, end);
    //    GameObject.Destroy(myLine, duration);
    //}
}

