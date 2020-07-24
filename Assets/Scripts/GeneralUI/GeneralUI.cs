using System;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class GeneralUI : MonoBehaviour
{
    public GameManager gameManager;
    public Camera screenCamera;

    private float m_DampTime = 2f;
    private Vector3 m_Velocity;
    private Vector3 m_DeltaVector;
    public Vector3 m_originalPosition;

    private void Start()
    {
        m_originalPosition = screenCamera.transform.position;
    }

    private void Update()
    {
        m_DeltaVector.x = Mathf.Cos(Time.time/5f);
        m_DeltaVector.z = Mathf.Sin(Time.time/5f);
        m_DeltaVector.y = 0f;
        screenCamera.transform.position = Vector3.SmoothDamp(screenCamera.transform.position, 
            screenCamera.transform.position + 5f * m_DeltaVector, ref m_Velocity, m_DampTime);
        //throw new NotImplementedException();
    }

    public void resetTransform()
    {
        screenCamera.transform.position = m_originalPosition;
    }
}
