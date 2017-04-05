using UnityEngine;
using Assets.BHTree;

public class Fighter : MonoBehaviour
{
    private FighterBlackboard m_blackboard = null;
    private Propulsion m_propulsion = null;    
    private Rigidbody m_rigidbody;
    private float m_thrust = 10;
    private float m_turnRate = 4;
    private float m_speedLimit = 1500;

    public int teamNumber { get; set; }
    public Weapon weapon { get; set; }
    public GameObject weaponVisuals { private get; set; }
    public GameObject mothership { get; set; }
    public GameObject enemyMothership { get; set; }
    public SpaceManager spaceManager { get; set; }
    // editor variables
    [SerializeField] public bool isExploding = false;
    [SerializeField] public bool manualOverride = false;



    void Start()
    {
        
        weaponVisuals = gameObject.transform.Find("visibleLazer").gameObject;
        weaponVisuals.transform.parent = gameObject.transform;

        weapon = (gameObject.tag == "Team1") ? new Lazer(Color.red, 20.0f, 20.0f, weaponVisuals) : new Lazer(Color.green, 20.0f, 20.0f, weaponVisuals);
        m_rigidbody = gameObject.AddComponent<Rigidbody>();
        m_rigidbody.useGravity = false;
        m_propulsion = new Propulsion(m_rigidbody, gameObject, m_thrust, m_turnRate);
        InitializeBlackboard();    
        
        spaceManager = GameObject.FindGameObjectWithTag("SpaceManager").GetComponent<SpaceManager>();
    }


    void FixedUpdate()
    {
        m_rigidbody.AddForce(spaceManager.GetGravity(m_rigidbody.position), ForceMode.Acceleration);
        //Debug.Log((m_rigidbody.position));
        if (!manualOverride)
        {
            m_blackboard.navigator.Navigate(Time.fixedDeltaTime);
        }
        //ManualSteering();
    }

    void Update()
    {
        m_blackboard.BlackboardUpdate();
        weapon.Update(Time.deltaTime);
        if (weaponVisuals)
        {
            float shrinkRate = 3f * Time.deltaTime;
            weaponVisuals.transform.localScale = new Vector3(Mathf.Clamp01(weaponVisuals.transform.localScale.x - shrinkRate), weaponVisuals.transform.localScale.y, Mathf.Clamp01(weaponVisuals.transform.localScale.z - shrinkRate));
            weapon.Update(Time.deltaTime);
        }

        // rip if over-G, mostly because we don't want litter really
        if (m_rigidbody.velocity.magnitude > m_speedLimit)
            isExploding = true;
    }

    private void InitializeBlackboard()
    {
        m_blackboard = new FighterBlackboard(this, gameObject);
        m_blackboard.AddScanner();
        m_blackboard.AddWeapon(weapon);
        m_blackboard.AddNavigation(m_propulsion);
    }
    

    private void ManualSteering()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            m_propulsion.Rotate("pitch", transform.right * Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            m_propulsion.Rotate("yaw", transform.up * Input.GetAxis("Horizontal"));
        }
        if (Input.GetAxis("Roll") != 0)
        {
            m_propulsion.Rotate("roll", transform.forward * Input.GetAxis("Roll"));
        }
        if (Input.GetButton("Fire1")) // ctrl
        {
            m_propulsion.ApplyThrottle(1, Time.fixedDeltaTime);
        }
        if (Input.GetButton("Jump"))
        {
            m_propulsion.ApplyGravBreak(1, Time.fixedDeltaTime);
        }
    }


    public void SetThrust(float amount)
    {
        m_thrust = amount;
    }


    public void SetTurnRate(float amount)
    {
        m_turnRate = amount;
    }
}

