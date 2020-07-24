using System;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float tankFullHealth = 100f;
    public Slider slider;
    public Image fillImage;
    public Color healthWheelActive = Color.green;
    public Color healthWheelDown = Color.red;
    public GameObject explosionPrefab;

    private AudioSource m_ExplosionAudioSource;
    private ParticleSystem m_ExplosionParticle;
    private bool m_IsDead;
    private float m_CurrentHealth;
    private float m_HealthWheelDampTime = 0.2f;
    private float m_HealthDecreaseVelocity = 2f;
    private void Awake()
    {
        m_ExplosionParticle = (Instantiate(explosionPrefab)).GetComponent<ParticleSystem>();
        m_ExplosionAudioSource = m_ExplosionParticle.GetComponent<AudioSource>();
        
        m_ExplosionParticle.gameObject.SetActive(false);
        //throw new NotImplementedException();
    }

    private void OnEnable()
    {
        m_CurrentHealth = tankFullHealth;
        m_IsDead = false;
        
        SetHealthWheelUI();
        //throw new NotImplementedException();
    }

    public void HealthDown(float damage)
    {
        m_CurrentHealth -= damage;
        
        if (m_CurrentHealth <= 0f && !m_IsDead)
        {
            m_CurrentHealth = 0f;
            OnDeath();
        }
    }

    private void Update()
    {
        SetHealthWheelUI();
        //throw new NotImplementedException();
    }

    public void SetHealthWheelUI()
    {
        slider.value = Mathf.SmoothDamp(slider.value, m_CurrentHealth, ref m_HealthDecreaseVelocity, m_HealthWheelDampTime);
        fillImage.color = Color.Lerp(healthWheelDown, healthWheelActive, m_CurrentHealth / tankFullHealth);
    }

    public void OnDeath()
    {
        m_IsDead = true;

        m_ExplosionParticle.transform.position = transform.position;
        m_ExplosionParticle.gameObject.SetActive(true);
        
        m_ExplosionParticle.Play();
        m_ExplosionAudioSource.Play();
        
        gameObject.SetActive(false);
    }
}
