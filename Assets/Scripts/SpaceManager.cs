using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShipCounter))]
public class SpaceManager : MonoBehaviour {


    [SerializeField] private Text teamOneCount = null;
    [SerializeField] private Text teamTwoCount = null;
    [SerializeField] private Camera m_camera = null;
    public ShipCounter shipCounter { get; private set; }
    [SerializeField] private GameObject m_planet = null;
    [SerializeField] private List<GameObject> m_spawners = null;
    [SerializeField] private  float m_blackHolePower = 25000;
    [SerializeField] private float m_explosionPower = 500;
    [SerializeField] private float m_timeScale = 1;
    public GravityManager gravityManager { get; private set; }
    private List<GameObject> m_shockwaves = new List<GameObject>();
    float m_gravityAtPlanet = 65000000;

    public float m_dragSpeed = 16;
    private Vector3 m_dragOrigin;

    void Start()
    {
        shipCounter = gameObject.GetComponent<ShipCounter>();

        teamOneCount.text = "#0";
        teamTwoCount.text = "#0";

        gravityManager = new GravityManager(this, m_spawners);
    }
    

    void FixedUpdate()
    {
        if (Time.timeScale != m_timeScale)
            Time.timeScale = m_timeScale;
    }


    void Update()
    {
        if (teamOneCount != null)        
            teamOneCount.text = "#" + shipCounter.fighterTeams[1].Count;
        if (teamTwoCount != null)
            teamTwoCount.text = "#" + shipCounter.fighterTeams[2].Count;

        gravityManager.UpdateGravities(Time.deltaTime);

        UpdateCamera(Time.deltaTime);

        // Funny acceleration of gameplay hehe
        if (Time.timeSinceLevelLoad > 36.0f)
        {
            m_timeScale += 0.05f * Time.deltaTime;

            // nobody ever wins
            if (m_timeScale > 60.0f)
                UnityEngine.SceneManagement.SceneManager.LoadScene("0");
        }


        //  Debug inputs

        if (Input.GetKeyDown(KeyCode.F3) || Input.GetKeyUp(KeyCode.F3))
        {
            foreach (DeliberateScaler scaler in gravityManager.GetAllScalers())
            {
                scaler.isGrowing = !scaler.isGrowing;
            }
        }

        if (Input.GetKeyDown(KeyCode.F4) || Input.GetKeyUp(KeyCode.F4))
        {
            foreach (DeliberateScaler scaler in gravityManager.GetTeamScalers(0))
            {
                scaler.isGrowing = !scaler.isGrowing;
            }
        }
        if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyUp(KeyCode.F5))
        {
            foreach (DeliberateScaler scaler in gravityManager.GetTeamScalers(1))
            {
                scaler.isGrowing = !scaler.isGrowing;
            }
        }
    }

    private void UpdateCamera(float deltaTime)
    {
        if (m_camera != null)
        {
            if (shipCounter.averagePosition != Vector3.zero)
            {                            
                m_camera.transform.position += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10,10)) * deltaTime;
            }

            var change = Input.GetAxis("Mouse ScrollWheel");

            if (change > 0f)
            {
                AlterZoom(-2);
            }
            else if (change < 0f)
            {
                AlterZoom(2);
            }

            DragCamera();

            if (Input.GetKey(KeyCode.W))
                AlterZoom(-2);
            if (Input.GetKey(KeyCode.S))
                AlterZoom(1);
        }
    }


    // courtesy of Mr Percy McPersonface https://forum.unity3d.com/threads/click-drag-camera-movement.39513/
    private void DragCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = m_camera.ScreenToViewportPoint(Input.mousePosition - m_dragOrigin);
        Vector3 move = new Vector3(pos.x * m_dragSpeed, pos.z * m_dragSpeed, pos.y * m_dragSpeed);

        m_camera.transform.Translate(move, Space.World);
    }



    public void AlterZoom(float amount)
    {
        if (m_camera != null)
        {
            m_camera.fieldOfView = Mathf.Clamp(m_camera.fieldOfView + amount, 1, 179);
        }
    }


    public Vector3 GetGravity(Vector3 myPosition)
    {
        if (m_planet != null)
        {
            Vector3 gravity = Vector3.zero;

            // and plain old planetary gravity
            {
                float gravityOverDistance = m_gravityAtPlanet / Vector3.SqrMagnitude(m_planet.transform.position - myPosition);
                gravity += (m_planet.transform.position - myPosition).normalized * gravityOverDistance;
            }

            // add any player driven gravity wells
            gravity += gravityManager.GetBlackHoleInfluenceAtPosition(myPosition, m_blackHolePower);

            // and the explosions
            gravity += gravityManager.GetShockwaveInfluenceAtPosition(myPosition, m_explosionPower);

            //if (m_shockwaves.Count > 0)
            //{
            //    foreach (GameObject explosion in m_shockwaves)
            //    {
            //        if (explosion != null)
            //            gravity += gravityManager.GetShockwaveInfluenceAtPosition(myPosition, m_explosionPower);
            //    }
            //}

            // cap it a bit to prevent light-speed fighters :):)
            gravity = Vector3.ClampMagnitude(gravity, 300);
            return gravity;
        }
        else
            return Vector3.zero;
    }

    public void AddFakeShockwave(GameObject entity)
    {
        if (m_shockwaves.Count >= 1)
        {
            for (int i = m_shockwaves.Count - 1; i >= 0; i--)
            {
                if (m_shockwaves[i] == null)
                    m_shockwaves.RemoveAt(i);
            }
        }
        m_shockwaves.Add(entity);
    }

    public void AddShockwave(GameObject entity)
    {
        float lifeTime = 1.6f;
        float decay = m_explosionPower * lifeTime;
        float speed = 1.0f;
        // some less arbitrary values here maybe?
        gravityManager.AddShockwave(entity, lifeTime, m_explosionPower, decay, speed);
    }
}