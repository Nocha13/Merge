using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAchieveMgr : MonoBehaviour
{
    public GameObject[] lockMerge;      //잠금
    public GameObject[] unlockMerge;    //해금
    //public GameObject uiNotification;
    WaitForSecondsRealtime wait;

    enum Achieve { unlockLv2, unlockLv3, unlockLv4, unlockLv5, unlockLv6, unlockLv7, unlockLv8 }
    Achieve[] achieves;

    void Awake()
    {
        achieves = (Achieve[])System.Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(3);

        if (!PlayerPrefs.HasKey("MergeData"))
            Init();
    }

    void Init() //초기화
    {
        PlayerPrefs.SetInt("MergeData", 1);

        foreach(Achieve achieve in achieves)
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
        for (int idx = 0; idx < lockMerge.Length; idx++)
        {
            string achiveName = achieves[idx].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockMerge[idx].SetActive(!isUnlock);
            unlockMerge[idx].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach(Achieve achieve in achieves) //모든 업적 확인
        {
            AchiveCheck(achieve);
        }
    }

    void AchiveCheck(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.unlockLv2:
                isAchieve = GameManager.maxLevel == 1;
                break;

            case Achieve.unlockLv3:
                isAchieve = GameManager.maxLevel == 2;
                break;

            case Achieve.unlockLv4:
                isAchieve = GameManager.maxLevel == 3;
                break;

            case Achieve.unlockLv5:
                isAchieve = GameManager.maxLevel == 4;
                break;

            case Achieve.unlockLv6:
                isAchieve = GameManager.maxLevel == 5;
                break;

            case Achieve.unlockLv7:
                isAchieve = GameManager.maxLevel == 6;
                break;

            case Achieve.unlockLv8:
                isAchieve = GameManager.maxLevel == 7;
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)    //처음 업적 달성시
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);  //업적 달성시        
        }
    }
}
