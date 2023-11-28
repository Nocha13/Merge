using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public static Move Inst;

    float deltaBLine;    //좌(우) 이동가능 최대값
    Vector3 curPos;     //현재위치
    float sp = 2.5f;    //이동속도

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
