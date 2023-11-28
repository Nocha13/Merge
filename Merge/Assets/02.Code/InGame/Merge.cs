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

    void OnEnable()
    {
        #region �ִ뷹�� 0�̻� 2����
        if (GameManager.maxLevel <= 2) //2���� = ���� 0�� ����
        {
            level = 0;
        }
        #endregion
        #region �ִ뷹�� 3�̻� 5�̸�
        else if (GameManager.maxLevel >= 3 && GameManager.maxLevel < 5)        //�ִ뷹���� 3�̻�, 5�̸��̸�
        {
            int ran = Random.Range(0, 4);   //0 ~ 3 ����

            if (ran >= 2)                   //0 ~ 2�̸�
                level = Random.Range(0, 3); //���� = 0 ~ 2

            else if (ran == 3)              //3�̸�
                level = Random.Range(2, 4); //���� = 2 ~ 3
        }
        #endregion
        #region �ִ뷹�� 5�̻�
        else if (GameManager.maxLevel >= 5) //�ִ뷹�� 5�̻�
        {
                int ran1 = Random.Range(0, 4); //0 ~ 3 ����

            if (ran1 > 3)                  //3�̸��̸�
                level = Random.Range(2, 5); //���� = 2 ~ 4

            else if (ran1 == 3)             //3�̸�
                level = Random.Range(3, 6); //���� = 3 ~ 5 
        }
        #endregion

        anim.SetInteger("Level", level);
    }

    void OnDisable()
    {
        //Merge �Ӽ� �ʱ�ȭ
        level = 0;
        isDrag = true;
        isMerge = false;
        isAttach = false;

        //Merge Ʈ������ �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        //Merge ���� �ʱ�ȭ
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
    #region
    //public void Drag()
    //{
    //    isDrag = true;
    //}
    #endregion

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
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Attach);

        yield return new WaitForSeconds(0.2f);

        isAttach = false;
    }

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

                //���� �Ʒ� ���� ��
                //���� ������ ��, ���� ������, ���ʿ� ���� ��
                if(myY < otherY || (myY == otherY && myX > otherX && myX < otherX))
                {//���� �����
                    other.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }

    public void Hide(Vector3 targetPos)
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

    IEnumerator HideRoutine(Vector3 targetPos)
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

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);

        EffectPlay();
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.LevelUp);

        yield return new WaitForSeconds(0.3f);

        level++;

        GameManager.maxLevel = Mathf.Max(level, GameManager.maxLevel);

        isMerge = false;
    }

    void EffectPlay()
    {
        particle.transform.position = transform.position;
        particle.transform.localScale = transform.localScale;
        particle.Play();
    }
}
