using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class SpawnShips : MonoBehaviour
    {
        [SerializeField] private string m_spawnKey = null;
        [SerializeField] private string m_gravityGenKey = null;
        [SerializeField] private GameObject m_shipType = null;
        [SerializeField] public GameObject enemyMothership = null;
        [SerializeField] private int m_teamNumber = 0;
        [SerializeField] private float m_thrust = 10;
        [SerializeField] private float m_turnRate = 4;
        [SerializeField] private SpaceManager m_spaceManager = null;
        [SerializeField] private Text m_resourceText = null;
        [SerializeField] private int m_health = 10;

        private float m_resource = 30;
        private float m_resourceGatheringRate = 2;

        public void Start()
        {
            m_spaceManager = GameObject.FindGameObjectWithTag("SpaceManager").GetComponent<SpaceManager>();
            m_spaceManager.shipCounter.fighterTeams.Add(m_teamNumber, new List<GameObject>());
        }

        public void Update()
        {
            if (m_health <= 0)
                UnityEngine.SceneManagement.SceneManager.LoadScene("0");

            if (gameObject.transform.localScale.x > 1)
            {
                m_resource = Mathf.Clamp(m_resource + Time.deltaTime * (m_resourceGatheringRate / Time.timeScale), -3.0f, 50.0f);
            }
            m_resourceText.text = Mathf.Round(m_resource).ToString();
            // Debug.Log(m_resource);
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SpawnShip();
            }
            if (Input.GetKey(KeyCode.F2))
            {
                SpawnShip();
            }
            if (Input.GetButton(m_spawnKey))
            {
                SpawnShip();
            }
            if ((Input.GetButtonDown(m_gravityGenKey)) || (Input.GetButtonUp(m_gravityGenKey)))
            {
                foreach (DeliberateScaler scaler in m_spaceManager.gravityManager.GetTeamScalers(m_teamNumber -1))
                {
                    scaler.isGrowing = !scaler.isGrowing;
                }
            }
        }

        public void SpawnShip()
        {
            if (m_resource > 1)
            {
                m_resource -= 1.0f;

                GameObject ship = (GameObject)Instantiate(m_shipType, transform.position, Quaternion.identity);
                Fighter fighter = ship.AddComponent<Fighter>();
                fighter.SetThrust(m_thrust);
                fighter.SetTurnRate(m_turnRate);
                fighter.mothership = gameObject;
                fighter.teamNumber = m_teamNumber;
                fighter.spaceManager = m_spaceManager;
                fighter.enemyMothership = enemyMothership;
            }
        }

        public void AlterTurnRate(float change)
        {
            m_turnRate += change;
        }

        public void AlterThrust(float change)
        {
            m_thrust += change;
        }

        public void TakeDamage(int damage)
        {
            m_health -= damage;
        }
    }
}


