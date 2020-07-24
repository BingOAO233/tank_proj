using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;//过渡时间           
    public float screenEdgeBuffer = 4f;//边框距离
    public float minSize = 6.5f;//最小画幅
    /*[HideInInspector]*/ public Transform[] targets; //取景对象
    
    private Camera m_Camera;//camera对象
    private float m_ZoomSpeed;//缩放速度             
    private Vector3 m_MoveVelocity;//移动速度   
    private Vector3 m_DesiredPosition;//目标位置              
    
    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
        
    }
    private void FixedUpdate()
    {
        Move();
        Zoom();
    }
    private void Move()
    {
        m_DesiredPosition = FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, dampTime);
    }
    
    private Vector3 FindAveragePosition()
    {
        Vector3 averagePos = new Vector3(0,0,0);
        int numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;
            averagePos += targets[i].position;
            numTargets++;
        }
        if (numTargets > 0)
            averagePos /= numTargets;
        averagePos.y = transform.position.y;
        return averagePos;
    }
    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, dampTime);
    }
    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);
        float size = 0f;
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;
            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
        }
        size += screenEdgeBuffer;
        size = Mathf.Max(size, minSize);
        return size;
    }
    
    public void SetStartPositionAndSize()
    {
        
        transform.position = FindAveragePosition();
        m_Camera.orthographicSize = FindRequiredSize();
    }
}