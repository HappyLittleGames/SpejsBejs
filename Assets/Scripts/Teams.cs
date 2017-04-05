using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Teams : MonoBehaviour
{
    [SerializeField] private List<string> m_teamTags = null;

    [SerializeField] private Dictionary<string, List<GameObject>> m_teams = null;

    void Awake()
    {
        m_teams = new Dictionary<string, List<GameObject>>();

        if (m_teamTags != null)
        {
            foreach (string teamTag in m_teamTags)
            {
                m_teams.Add(teamTag, GameObject.FindGameObjectsWithTag(teamTag).ToList());
            }
        }
        else
        {
            Debug.Log("Teams:Awake has no teamTags to look for");
        }

        Debug.Log("Teams: " + m_teams.Count());
    }


    /// do not call this in update
    public bool AddToTeam(GameObject teamObject) 
    {
        if(m_teams.ContainsKey(teamObject.tag))
        {
            m_teams[teamObject.tag].Add(teamObject);
            return true;
        }
        else
            return false;
    }


    public List<GameObject> GetTeam(string teamTag)
    {
        return m_teams[teamTag];
    }


    public List<GameObject> GetOtherTeams(string teamTag)
    {
        List<GameObject> others = new List<GameObject>();

        foreach (string tag in m_teams.Keys)
        {
            if (tag != teamTag)
            {
                foreach (GameObject entity in m_teams[tag])
                {
                    others.Add(entity);
                }
                foreach (GameObject testTarget in GameObject.FindGameObjectsWithTag("TargetPracticeTarget"))
                {
                    others.Add(testTarget);
                }
            }
        }
        return others;
    }

    public GameObject GetBase(string teamTag)
    {
        return GameObject.FindGameObjectWithTag(teamTag + "Base"); // should return vector of supply-positions really
    }

    public void Kill(GameObject entity)
    {
        if (entity != null)
        {
            foreach (List<GameObject> list in m_teams.Values)
            {
                if (list.Remove(entity))
                {
                    Debug.Log("Killed " + entity.name);
                    Destroy(entity);
                }
                else
                {
                    Debug.Log("Attempted to kill " + entity.name + ", but couldn't find it");
                }
            }
        }
    }
}
