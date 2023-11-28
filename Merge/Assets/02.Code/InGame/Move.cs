using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public static Move Inst;

    float deltaBLine;    //��(��) �̵����� �ִ밪
    Vector3 curPos;     //������ġ
    float sp = 2.5f;    //�̵��ӵ�

    private void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        curPos = transform.position;
    }

    void Update()
    {
        deltaBLine = -4.1f + transform.localScale.x / 2;
            
        Vector3 v = curPos;

        v.y = 6.45f;
        v.z = 0;

        v.x += deltaBLine * Mathf.Sin(Time.time * sp);
        transform.position = v;   
    }
}
