using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissonAchieveMgr : MonoBehaviour
{
    public GameObject[] lockMisson;      //���
    public GameObject[] unlockMisson;    //�ر�
    //public GameObject uiNotification;
    WaitForSecondsRealtime wait;

    enum Achieve { misson01, misson02, misson03, misson04 }
    Achieve[] achieves;

    void Awake()
    {
        achieves = (Achieve[])System.Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(3);

        if (!PlayerPrefs.HasKey("MissonData"))
            Init();
    }

    void Init() //�ʱ�ȭ
    {
        PlayerPrefs.SetInt("MissonData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockMerge();
    }

    void UnlockMerge()
    {
        for (int idx = 0; idx < lockMisson.Length; idx++)
        {
            string achiveName = achieves[idx].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockMisson[idx].SetActive(!isUnlock);
            unlockMisson[idx].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach (Achieve achieve in achieves) //��� ���� Ȯ��
        {
            AchiveCheck(achieve);
        }
    }

    void AchiveCheck(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.misson01:
                isAchieve = GameManager.isClear == true;    //�ѹ� Ŭ���� �Ϸ�
                break;

            case Achieve.misson02:
                isAchieve = GlobalGameData.userLevel == 3;  //3���� �޼��ϱ�
                break;

            case Achieve.misson03:
                isAchieve = GameManager.maxLevel == 2;      //�� 3�ܰ� ����
                break;

            case Achieve.misson04:
                isAchieve = GameManager.isOver == true;     //�ѹ� �����ϱ�
                break;

                //case Achieve.unlockLv6:
                //    isAchieve = GameManager.maxLevel == 5;
                //    break;

                //case Achieve.unlockLv7:
                //    isAchieve = GameManager.maxLevel == 6;
                //    break;

                //case Achieve.unlockLv8:
                //    isAchieve = GameManager.maxLevel == 7;
                //    break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)    //ó�� ���� �޼���
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);  //���� �޼���
            #region
            //for (int idx = 0; idx < uiNotification.transform.childCount; idx++)
            //{
            //    bool isActive = idx == (int)achieve;    //�˸� â �ڽ� ������Ʈ ��ȸ, ���� ������ Ȱ��ȭ
            //    uiNotification.transform.GetChild(idx).gameObject.SetActive(isActive);
            //}

            //StartCoroutine(NotifiRoutine());
            #endregion
        }
    }
    #region
    //IEnumerator NotifiRoutine()
    //{
    //    uiNotification.SetActive(true);

    //    //AudioMgr.Inst.PlaySfx(AudioMgr.SFX.LevelUp);

    //    yield return wait;

    //    uiNotification.SetActive(false);
    //}
    #endregion
}
