using System;
using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_ActiveRelativeRotation = true;

    private Quaternion m_RelativeRotation;

    private void Start()//初始化 记录血槽旋转角度
    {
        m_RelativeRotation = transform.parent.localRotation;
        //throw new NotImplementedException();
    }

    private void Update()//实现血槽相对静止 非转动
    {
        if (m_ActiveRelativeRotation)
        {
            transform.rotation = m_RelativeRotation;
        }
        //throw new NotImplementedException();
    }
}
