using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LInes : MonoBehaviour
{
    public static LInes Inst;

    public enum lineType { none, Warning, Dead }

    public lineType type = lineType.none;

    public SpriteRenderer warningRen = null;    
    Vector2 warPos = Vector2.zero;
    Vector2 warSize = Vector2.zero;
    public bool warTouch;
    float warLineTimer;

    public SpriteRenderer deadRen = null;
    Vector2 deadPos = Vector2.zero;
    Vector2 deadSize = Vector2.zero;
    public bool deadTouch;
    float deadLineTimer;
    public float countDown;

    LayerMask mergeMask = -1;
    float timer = 0;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        switch (type)
        {
            case lineType.Warning:    //경고 알림
                if (warningRen != null)
                {
                    warPos = warningRen.transform.position;
                    warSize.x = warningRen.bounds.size.x;
                    warSize.y = warningRen.bounds.size.y;
                }
                break;

            case lineType.Dead:    //게임 오버 알림
                if (deadRen != null)
                {
                    deadPos = deadRen.transform.position;
                    deadSize.x = deadRen.bounds.size.x;
                    deadSize.y = deadRen.bounds.size.y;
                }
                break;
        }

        mergeMask = 1 << LayerMask.NameToLayer("Touch");
    }

    // ORDER : #03) 경계선 이벤트
    void Update()
    {
        if(warTouch == true)
        {
            if (Physics2D.OverlapBox(warPos, warSize, 0, mergeMask) != null)
            {    
                GameObject.Find("Walls").transform.Find("Color").gameObject.SetActive(true); //배경색 변경
                
                timer = 0.02f;
            }
        }

        if(0.0f < timer)
        {
            timer -= Time.deltaTime;

            if(timer <= 0.0f)
            {
                GameObject.Find("Walls").transform.Find("Color").gameObject.SetActive(false); //배경색 변경(원래대로)         
            }
        }

        if (deadTouch == true)
        {
            if (Physics2D.OverlapBox(deadPos, deadSize, 0, mergeMask) != nul초

                if (countDown <= 0.0f)
                {
                    GameManager.Inst.GameOver(); //게임오버
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.tag == "Merge")    //"Merge" 태그
        {
            switch (type)
            {
                case lineType.Warning:
                    warLineTimer += Time.deltaTime;
                    if (warLineTimer > 2)    //2초 이상 닿으면 경고 알림 활성
                        warTouch = true;
                    break;

                case lineType.Dead:
                    deadLineTimer += Time.deltaTime;
                    if (deadLineTimer > 2)    //2초 이상 닿으면 오버 이벤트 활성
                        deadTouch = true;
                    break;
            }   
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Merge")
        {
            switch(type)
            {
                case lineType.Warning:
                    if (Physics2D.OverlapBox(warPos, warSize, 0, mergeMask) == null)    //모든 오브젝트 없을 시
                    {
                        //경고 초기화
                        warLineTimer = 0;
                        warTouch = false;
                    }
                    break;

                case lineType.Dead:
                    if(Physics2D.OverlapBox(deadPos, deadSize, 0, mergeMask) == null)    //모든 오브젝트 없을 시
                    {
                        //오버 초기화
                        deadLineTimer = 0;
                        countDown = 10;
                        deadTouch = false;
                    }
                    break;
            }
        }
    }

    #region //Etc Code
    ////void ColorChange(float alpha)
    ////{
    ////    Color color = canvas.GetComponent<Image>().color;
    ////    color.a = alpha;
    ////    canvas.GetComponent<Image>().color = color;
    ////}
    #endregion
}
