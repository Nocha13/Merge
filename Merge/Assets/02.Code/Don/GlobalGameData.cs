using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

//최소 클리어 시간(minTime) : 게임 클리어를 얼마나 빠르게 했나()
//최소 이동수(minTurn) : 게임 클리어 시점에서 얼마나 적게 이동(드롭)로 클리어 했나(2023.10.30 해결)

//콤보 관련 작성 : 콤보의 종류 3개

//1. 최대 콤보(maxCombo) = 증가 콤보 > 최대 콤보
//2. 증가 콤보(addCombo) : 합쳐질 때마다 증가 콤보 수 증가. (단, 일정 시간 지나도 합쳐지지 않는다면 증가 콤보수 0으로 초기화)
//3. 플레이 중 누적 콤보(curCombo) : 누적 콤보 + 증가 콤보

public class LevelTable
{
    public int totalExp = 0; //다음 레벨업에 필요한 총 경험치
}

public class GlobalGameData : MonoBehaviour
{
    static List<LevelTable> lvTable = new List<LevelTable>();

    public static int bestScore = 0;    //최고 점수
    public static int bestCombo = 0;    //최대 콤보


    public static int minTurn;          //최소 이동수
    public static float minTime;        //클리어 최소 시간

    public static int userLevel = 1;    //유저 레벨
    public static int curExp = 0;       //현재경험치


    void Start()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("LevelTable");
        string strJsonData = jsonData.text;
        var parJson = JSON.Parse(strJsonData);

        if (parJson["LvTable"] != null)
            for (int idx = -1; idx < parJson["LvTable"].Count; idx++)
            {
                int total = parJson["LvTable"][idx].AsInt;
                LevelTable node = new LevelTable();
                node.totalExp = total;
                lvTable.Add(node);
            }

        userLevel = PlayerPrefs.GetInt("UserLevel", 1);
        curExp = PlayerPrefs.GetInt("CurExp", 0);

        bestScore = Mathf.Max(GameManager.curScore, PlayerPrefs.GetInt("BestScore", 0));
        bestCombo = Mathf.Max(GameManager.curCombo, PlayerPrefs.GetInt("BestCombo", 0));
       
        minTurn = PlayerPrefs.GetInt("MinTurn", 0);
        minTime = PlayerPrefs.GetFloat("MinTime", 0);
    }

    public static void GetExp()
    {
        if (userLevel < lvTable.Count)
        {
            curExp += 1;
            if (lvTable[userLevel].totalExp <= curExp)
            {
                userLevel++;
                curExp = 0;
            }
            PlayerPrefs.SetInt("UserLevel", userLevel);
            PlayerPrefs.SetInt("CurExp", curExp);
        }
    }
}

