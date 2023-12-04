using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merge : MonoBehaviour
{
    public Rigidbody2D rigid;
    CircleCollider2D circle;
    Animator anim;

    public GameManager Inst;
    public PoolMgr poolInst;
    public ParticleSystem particle;

    bool isDrag;
    bool isMerge;
    bool isAttach;

    public int level;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        isDrag = true;
    }

    void OnEnable()    //최대 레벨 상황 별 생성 오브젝트 레벨 
    {
        #region 최대레벨 0이상 2이하
        if (GameManager.maxLevel <= 2) //2이하 = 레벨 0만 생성
        {
            level = 0;
        }
        #endregion
        #region 최대레벨 3이상 5미만
        else if (GameManager.maxLevel >= 3 && GameManager.maxLevel < 5)        //최대레벨이 3이상, 5미만이면
        {
            int ran = Random.Range(0, 4);   //0 ~ 3 랜덤

            if (ran >= 2)                   //0 ~ 2이면
                level = Random.Range(0, 3); //레벨 = 0 ~ 2

            else if (ran == 3)              //3이면
                level = Random.Range(2, 4); //레벨 = 2 ~ 3
        }
        #endregion
        #region 최대레벨 5이상
        else if (GameManager.maxLevel >= 5) //최대레벨 5이상
        {
                int ran1 = Random.Range(0, 4); //0 ~ 3 랜덤

            if (ran1 > 3)                  //3미만이면
                level = Random.Range(2, 5); //레벨 = 2 ~ 4

            else if (ran1 == 3)             //3이면
                level = Random.Range(3, 6); //레벨 = 3 ~ 5 
        }
        #endregion

        anim.SetInteger("Level", level);
    }

    void OnDisable()    //오브젝트 재사용
    {
        //Merge 속성 초기화
        level = 0;
        isDrag = true;
        isMerge = false;
        isAttach = false;

        //Merge 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        //Merge 물리 초기화
        rigid.simulated = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;
        circle.enabled = true;
    }

    void Start()
    {
        //curPos = transform.position;
    }

    void Update()
    {
        if (isDrag)
        {
            transform.position = Move.Inst.transform.position + new Vector3(0, -1, 0);
        }
    }
    
    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        StartCoroutine(AttachRoutine());
    }

    IEnumerator AttachRoutine()
    {
        if (isAttach)
            yield break;
        
        isAttach = true;
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Attach);    //어딘가 부딪히면 오디오 활성

        yield return new WaitForSeconds(0.2f);

        isAttach = false;
    }

    // ORDER : #01) 오브젝트 기본 로직(합치기)
    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Merge")
        {
            Merge other = coll.gameObject.GetComponent<Merge>();

            if(level == other.level && !isMerge && !other.isMerge && level < 7)
            {//Start Merge
                float myX = transform.position.x;
                float myY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                //아래 있을 때
                //동일 높일 때, 오른쪽, 왼쪽에 있을 때
                if(myY < otherY || (myY == otherY && myX > otherX && myX < otherX))
                {//상대방 숨기기
                    other.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }

    public void Hide(Vector3 targetPos)    //합쳐지면서 비활성화
    {
        isMerge = true;

        rigid.simulated = false;
        circle.enabled = false;

        if(targetPos == Vector3.up * 1000)
        {
            EffectPlay();
        }

        if(this.gameObject.activeSelf == true)
            StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)    //모든 오브젝트 순환
    {
        int count = 0;

        while(count < 20)
        {
            count++;

            if(targetPos != Vector3.up * 1000)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            else if(targetPos == Vector3.up * 1000)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            
            yield return null;
        }

        GameManager.curScore += (int)Mathf.Pow(2, level);
        GameManager.curCombo += 1;

        isMerge = false;
        gameObject.SetActive(false);
    }

    void LevelUp()    
    {
        isMerge = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()    //레벨업(같은 오브젝트 합쳤을 때)
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);    //오브젝트 레벨

        EffectPlay();    
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.LevelUp);

        yield return new WaitForSeconds(0.3f);

        level++;

        GameManager.maxLevel = Mathf.Max(level, GameManager.maxLevel);    //오브젝트 최대 레벨

        isMerge = false;
    }

    void EffectPlay()    //합칠때 이펙트 생성
    {
        particle.transform.position = transform.position;
        particle.transform.localScale = transform.localScale;
        particle.Play();
    }
}
