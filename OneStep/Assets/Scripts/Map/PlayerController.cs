using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float m_Speed = 10.0f;                 // How fast the tank moves forward and back.
    public float m_deadTime = 10.0f;
    //public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    private Transform m_transform;
//     private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
//     private string m_TurnAxisName;              // The name of the input axis for turning.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.

    protected Animator m_animator;
    private Renderer m_spRenderer;
    private int m_runHash;
    private int m_attackHash;
    private int m_skillHash;
    private int m_hitHash;
    private int m_dieHash;
    private int m_cheerHash;
    private int m_directionHash;

    private E_NodeStatus m_curStatus;
    private int m_curStatusHash;

    //private GameObject m_target;
    
    private RoleNode m_NodeData;
    private float m_time;
    // Use this for initialization
    void Awake()
    {
        m_transform = this.transform;
        m_animator = m_transform.GetComponent<Animator>();
        m_spRenderer = m_transform.GetComponent<Renderer>();
        m_curStatusHash = -1;
    }

    void Start () {
        
//         m_MovementAxisName = "Vertical";
//         m_TurnAxisName = "Horizontal";

        m_runHash = Animator.StringToHash("Run");
        m_attackHash = Animator.StringToHash("Attack");
        m_skillHash = Animator.StringToHash("Skill");
        m_hitHash = Animator.StringToHash("Hit");
        m_dieHash = Animator.StringToHash("Die");
        m_cheerHash = Animator.StringToHash("Cheer");
        m_directionHash = Animator.StringToHash("Direction");
        m_curStatus = E_NodeStatus.Idle;
        m_curStatusHash = -1;
        SetRoleStatus(E_NodeStatus.Idle);
    }

    // Update is called once per frame
    private void Update()
    {
        // Store the value of both input axes.
//         m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
//         m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }

    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        //Move();
        //Turn();
    }

    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        //Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        // Apply this movement to the rigidbody's position.
        //m_transform.localPosition = m_transform.position + movement;
    }

    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        //float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        //Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        //m_transform.localRotation = m_transform.rotation * turnRotation;
    }

    public void SetRoleStatus(E_NodeStatus state)
    {
        if (state == m_curStatus || m_curStatus == E_NodeStatus.die)
            return;
        if (E_NodeStatus.Idle != state && state != E_NodeStatus.die)
        {
            if (m_curStatusHash != -1)
                return;
        }
        //m_animator.speed = 1.0f;
        m_curStatus = state;
        switch (state)
        {
            case E_NodeStatus.Idle:
                if (m_curStatusHash != -1)
                {
                    //m_animator.SetBool(m_curStatusHash, false);
                    m_animator.CrossFade("Idle", 0.2f, 0, 0);
                }
                m_curStatusHash = -1;
                break;
            case E_NodeStatus.walk:
                {
                    //m_animator.SetBool(m_runHash, true);
                    m_animator.CrossFade("Run", 0.2f, 0, 0);
                }
                m_curStatusHash = m_runHash;
                break;
            case E_NodeStatus.attack:
                {
                    //if (m_animator.GetBool(m_attackHash)) return;
                    //m_animator.SetBool(m_attackHash, true);
                    m_animator.CrossFade("Attack", 0.2f, 0, 0);
                    //m_animator.speed = 5.0f;
                }
                m_curStatusHash = m_attackHash;
                //Debug.Log("<<<<<<<<<<<<<<<<<<<<<<< attack <<<<<<<<<<<<<<< >>>>>>>");
                m_time = Time.time;
                break;
            case E_NodeStatus.skill:
                {
                    //if (m_animator.GetBool(m_skillHash)) return;
                    //m_animator.SetBool(m_skillHash, true);
                    m_animator.CrossFade("Skill", 0.2f, 0, 0);
                }
                m_curStatusHash = m_skillHash;
                break;
            case E_NodeStatus.jump:
                break;
            case E_NodeStatus.hit:
                {
                    //if (m_animator.GetBool(m_hitHash)) return;
                    //m_animator.SetBool(m_hitHash, true);
                    m_animator.CrossFade("Hit", 0.2f, 0, 0);
                }
                m_curStatusHash = m_hitHash;
                break;
            case E_NodeStatus.die:
                //                 m_animator.SetBool(m_runHash, false);
                //                 m_animator.SetBool(m_attackHash, false);
                //                 m_animator.SetBool(m_skillHash, false);
                //                 m_animator.SetBool(m_hitHash, false);
                //                 m_animator.SetBool(m_cheerHash, false);
                //                 if (m_animator.GetBool(m_dieHash)) return;
                {
                    //m_animator.SetBool(m_dieHash, true);
                    m_animator.CrossFade("Die", 0.2f, 0, 0);
                }
                m_curStatusHash = m_dieHash;
                break;
            case E_NodeStatus.cheer:
                {
                    //if (m_animator.GetBool(m_cheerHash)) return;
                    //m_animator.SetBool(m_cheerHash, true);
                    m_animator.CrossFade("Cheer", 0.2f, 0, 0);
                }
                m_curStatusHash = m_cheerHash;
                break;
        }
    }

    public E_NodeStatus GetCurStatus()
    {
        return m_curStatus;
    }

    /*public void SetTarget(GameObject target)
    {
        m_target = target;
    }*/

    public void SetNodeData(RoleNode nodeData)
    {
        m_NodeData = nodeData;
    }

    private void ActionStart()
    {
        //Debug.Log("-----------------------ActionStart--------------" + (Time.time - m_time));
    }

    private void EmissionCall()
    {
        m_NodeData.IsEmissionWatting = true;
        /*ProjectileNode bullet = new ProjectileNode(m_NodeData, m_NodeData.TargetObject);
        StartCoroutine(bullet.Shoot());*/
    }

    private void ActionEnd(int type)
    {
        //Debug.Log("-----------------------ActionEnd--------------" + (Time.time - m_time) + " type = " + type);
        //SetRoleStatus(E_NodeStatus.Idle);
        if (type != (int)E_NodeStatus.die)
            ActionPlayer.Instance.PlayStatus(m_NodeData, E_NodeStatus.Idle);
        else
        {
            m_transform.DOMoveY(-1, m_deadTime).OnComplete(onComplete);
        }
    }

    private void onComplete()
    {
        m_transform.gameObject.SetActive(false);
    }
}
