using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TankMovement : MonoBehaviour
{
    public float moveSpeed = 10f;//移动速度
    public float rotaSpeed = 120f;//转向速度
    public float turretRotaSpeed = 100f;//炮塔旋转速度
    public AudioSource tankMovementAudio;//坦克音源
    public AudioClip tankEngineIdling;  //坦克静止音效
    public AudioClip tankEngineDriving; //坦克移动音效
    public float pitchRange = 0.2f; //音效起伏度
    public TankHealth tankHealth;  //坦克生命值
    public GameObject turretRotaAxis; //炮塔旋转轴
    [HideInInspector] public int playerNumber;//玩家编号

    private string m_TankMovementAxisName;//坦克前后输入名
    private string m_TankRotationAxisName;//坦克转向输入名
    private string m_TestAxisName;
    private Rigidbody m_Rigidbody;//坦克刚体
    private float m_MovementInputValue;//坦克行进输入
    private float m_RotationInputValue;//坦克转向输入
    private float m_TestInputValue;
    private float m_OriginalPitch;//音源音高
    private float m_BasicCollideDamage = 20f;//基础撞击伤害
    private float m_BasicCollideDamageRatio = 0.1f;//基础撞击伤害比率
    //private float m_MinCollideDamageVelocityTrigger = 6f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); 
        //throw new NotImplementedException();
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_RotationInputValue = 0f;
        //throw new NotImplementedException();
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true; //禁用其运动学属性
        //throw new NotImplementedException();
    }

    private void Start()
    {
        //配置相应键位
        m_TankMovementAxisName = "Vertical" + playerNumber;
        m_TankRotationAxisName = "Horizontal" + playerNumber;
        m_TestAxisName = "TurretRotation" + playerNumber;
        //配置音源音高
        m_OriginalPitch = tankMovementAudio.pitch-2f;
        //throw new NotImplementedException();
    }


    private void Update()
    {
        m_MovementInputValue = Input.GetAxis(m_TankMovementAxisName);
        m_RotationInputValue = Input.GetAxis(m_TankRotationAxisName);
        m_TestInputValue = Input.GetAxis(m_TestAxisName);
        PlayMovementAudio();
        //throw new NotImplementedException();
    }

    private void PlayMovementAudio()
    {
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_RotationInputValue) < 0.1f)
        {
            if (tankMovementAudio.clip == tankEngineDriving)
            {
                tankMovementAudio.Pause();
                tankMovementAudio.clip = tankEngineIdling;
                tankMovementAudio.pitch = Random.Range(m_OriginalPitch - pitchRange, m_OriginalPitch + pitchRange);
                tankMovementAudio.Play();
            }
        }
        else
        {
            if (tankMovementAudio.clip == tankEngineIdling)
            {
                tankMovementAudio.Pause();
                tankMovementAudio.clip = tankEngineDriving;
                tankMovementAudio.pitch = Random.Range(m_OriginalPitch - pitchRange, m_OriginalPitch + pitchRange);
                tankMovementAudio.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        TankMove();
        TankRotate();
        TestRotate();
        //throw new NotImplementedException();
    }

    private void TestRotate()
    {
        float rotation = m_TestInputValue * rotaSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, rotation, 0f);

        turretRotaAxis.transform.Rotate(turnRotation.eulerAngles);
        //throw new NotImplementedException();
    }

    private void TankMove()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * moveSpeed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void TankRotate()
    {
        float rotation = m_RotationInputValue * rotaSpeed * Time.deltaTime * (m_MovementInputValue < 0 ? -1 : 1);

        Quaternion turnRotation = Quaternion.Euler (0f,rotation,0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Tank"))
        {
            /*if (Vector3.Magnitude(collision.rigidbody.velocity) >= m_MinCollideDamageVelocityTrigger ||
                Vector3.Magnitude(m_Rigidbody.velocity) >= m_MinCollideDamageVelocityTrigger)
            {*/
                float collideVelocity = Vector3.Magnitude(m_Rigidbody.velocity - collision.rigidbody.velocity);
                float collideDamageRatio = RatioCounter(collision);
                
                tankHealth.HealthDown(collideVelocity*collideDamageRatio*m_BasicCollideDamage);
            //}
        }
    }

    private float RatioCounter(Collision collision)
    {
        Vector3 tankForward = m_Rigidbody.transform.forward;
        Vector3 collideDirection = collision.GetContact(0).point - m_Rigidbody.position;
        return (Vector3.Angle(tankForward, collideDirection) / 90f + m_BasicCollideDamageRatio);
    }
}
