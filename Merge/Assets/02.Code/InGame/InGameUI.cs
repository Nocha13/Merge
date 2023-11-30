using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//userInfoType { userExp, userLevel }  //유저 경험치, 유저 레벨
//scoreType { curScore, bestScore }    //현재 점수, 최고 점수
//moveType { curTurn, minTurn }        //현재 이동수, 최소 이동수
//timeType { curTime, clearMinTime }   //현재 플레이 시간, 최소 클리어 시간
//countDType
//LevelBar

public enum Type {none, expLevelType , scoreType, turnType, comboType, timeType, countDType, levelBar }

public class InGameUI : MonoBehaviour
{
    public Type type = Type.none;
    public int typeNumber;      //쓰임 구분용

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
            #region //경험치, 레벨
            case Type.expLevelType:
                if (typeNumber == 0)         //유저 경험치
                {
                    
                }
                else if (typeNumber == 1)    //유저 레벨
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

            #region 점수 관련
            case Type.scoreType:
                if (typeNumber == 0)        //현재 점수
                {
                    text.text = GameManager.curScore.ToString();
                }
                else if (typeNumber == 1)   //최고 점수
                {
                    text.text = GlobalGameData.bestScore.ToString();
                    PlayerPrefs.SetInt("BestScore", GlobalGameData.bestScore);
                }
                break;
            #endregion

            #region 콤보수 관련
            case Type.comboType:
                if (typeNumber == 0)        //현재 콤보수
                {
                    text.text = GameManager.curCombo.ToString();
                }
                else if (typeNumber == 1)   //최대 콤보수
                {
                    text.text = GlobalGameData.bestCombo.ToString();
                    PlayerPrefs.SetInt("BestCombo", GlobalGameData.bestCombo);
                }
                //else if(typeNumber == 2)    //증가 콤보수
                //{

                //}
                break;
            #endregion

            #region 이동수 관련
            case Type.turnType:
                if (typeNumber == 0)        //현재 이동수
                {
                    text.text = GameManager.curTurn.ToString();
                }
                else if (typeNumber == 1)   //최소 이동수
                {
                    text.text = GlobalGameData.minTurn.ToString();
                    PlayerPrefs.SetInt("MinTurn", GlobalGameData.minTurn);
                }
                break;
            #endregion

            #region 시간 관련
            case Type.timeType:
                if (typeNumber == 0)        //현재 플레이 시간
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
                else if (typeNumber == 1)   //최소 클리어 시간
                {
                    text.text = GetRecordTime(GlobalGameData.minTime);
                    PlayerPrefs.SetFloat("MinTime", GlobalGameData.minTime);
                }
                break;
            #endregion

            #region //카운트다운
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
