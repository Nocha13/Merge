using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//userInfoType { userExp, userLevel }  //���� ����ġ, ���� ����
//scoreType { curScore, bestScore }    //���� ����, �ְ� ����
//moveType { curTurn, minTurn }        //���� �̵���, �ּ� �̵���
//timeType { curTime, clearMinTime }   //���� �÷��� �ð�, �ּ� Ŭ���� �ð�
//countDType
//LevelBar

public enum Type {none, expLevelType , scoreType, turnType, comboType, timeType, countDType, levelBar }

public class InGameUI : MonoBehaviour
{
    public Type type = Type.none;
    public int typeNumber;      //���� ���п�

    [Header("UI")]
    TextMeshProUGUI text;
    Slider slider;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            #region //����ġ, ����
            case Type.expLevelType:
                if (typeNumber == 0)         //���� ����ġ
                {
                    
                }
                else if (typeNumber == 1)    //���� ����
                {
                    text.text = GlobalGameData.userLevel.ToString();
                    PlayerPrefs.SetInt("UserLevel", GlobalGameData.userLevel);
                }
                else
                {
                    text.text = "Lv. " + GlobalGameData.userLevel.ToString();
                    PlayerPrefs.SetInt("UserLevel", GlobalGameData.userLevel);
                }
                break;
            #endregion

            #region ���� ����
            case Type.scoreType:
                if (typeNumber == 0)        //���� ����
                {
                    text.text = GameManager.curScore.ToString();
                }
                else if (typeNumber == 1)   //�ְ� ����
                {
                    text.text = GlobalGameData.bestScore.ToString();
                    PlayerPrefs.SetInt("BestScore", GlobalGameData.bestScore);
                }
                break;
            #endregion

            #region �޺��� ����
            case Type.comboType:
                if (typeNumber == 0)        //���� �޺���
                {
                    text.text = GameManager.curCombo.ToString();
                }
                else if (typeNumber == 1)   //�ִ� �޺���
                {
                    text.text = GlobalGameData.bestCombo.ToString();
                    PlayerPrefs.SetInt("BestCombo", GlobalGameData.bestCombo);
                }
                //else if(typeNumber == 2)    //���� �޺���
                //{

                //}
                break;
            #endregion

            #region �̵��� ����
            case Type.turnType:
                if (typeNumber == 0)        //���� �̵���
                {
                    text.text = GameManager.curTurn.ToString();
                }
                else if (typeNumber == 1)   //�ּ� �̵���
                {
                    text.text = GlobalGameData.minTurn.ToString();
                    PlayerPrefs.SetInt("MinTurn", GlobalGameData.minTurn);
                }
                break;
            #endregion

            #region �ð� ����
            case Type.timeType:
                if (typeNumber == 0)        //���� �÷��� �ð�
                {
                    text.text = GetRealTime(GameManager.Inst.gameTime);
                    #region //Old Code
                    //float reTime = GameManager.Inst.gameTime;
                    //float min = Mathf.Floor(reTime / 60);
                    //float sec = Mathf.RoundToInt(reTime % 60);
                    //string minS = "";
                    //string secS = "";

                    //if (min < 10)
                    //    minS = "0" + min.ToString();
                    //else
                    //    minS = min.ToString();

                    //if (sec < 10)
                    //    secS = "0" + Mathf.RoundToInt(sec).ToString();
                    //else
                    //    secS = Mathf.RoundToInt(sec).ToString();

                    //text.text = string.Format("{0:D2} : {1:D2}", minS, secS);
                    #endregion
                }
                else if (typeNumber == 1)   //�ּ� Ŭ���� �ð�
                {
                    text.text = GetRecordTime(GlobalGameData.minTime);
                    PlayerPrefs.SetFloat("MinTime", GlobalGameData.minTime);
                }
                break;
            #endregion

            #region //ī��Ʈ�ٿ�
            case Type.countDType:
                if (LInes.Inst.deadTouch == true)
                {
                    text.enabled = true;
                    text.text = Mathf.Round(LInes.Inst.countDown).ToString();
                }
                else if (LInes.Inst.deadTouch == false)
                {
                    text.enabled = false;
                }
                break;
            #endregion

            #region
            case Type.levelBar:
                int maxLevel = GameManager.maxLevel;
                slider.value = maxLevel;
                break;
            #endregion
        }
        #region //Time
        string GetRealTime(float reTime)
        {
            reTime = GameManager.Inst.gameTime;
            int min = (int)reTime / 60 % 60;
            int sec = (int)reTime % 60;
            string minStr = "";
            string secStr = "";

            if (min < 10)
                minStr = "0" + min.ToString();
            else
                minStr = min.ToString();

            if (sec < 10)
                secStr = "0" + sec.ToString();
            else
                secStr = sec.ToString();

            string realTime = string.Format("{0:D2} : {1:D2}", minStr, secStr);
            return realTime;
        }

        string GetRecordTime(float reTime)
        {
            reTime = GlobalGameData.minTime;
            int min = (int)reTime / 60 % 60;
            int sec = (int)reTime % 60;
            string minStr = "";
            string secStr = "";

            if (min < 10)
                minStr = "0" + min.ToString();
            else
                minStr = min.ToString();

            if (sec < 10)
                secStr = "0" + sec.ToString();
            else
                secStr = sec.ToString();

            string recordTime = string.Format("{0:D2} : {1:D2}", minStr, secStr);
            return recordTime;
        }
        #endregion
    }
}
