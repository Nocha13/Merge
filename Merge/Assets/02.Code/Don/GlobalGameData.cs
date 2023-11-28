using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

//�ּ� Ŭ���� �ð�(minTime) : ���� Ŭ��� �󸶳� ������ �߳�()
//�ּ� �̵���(minTurn) : ���� Ŭ���� �������� �󸶳� ���� �̵�(���)�� Ŭ���� �߳�(2023.10.30 �ذ�)

//�޺� ���� �ۼ� : �޺��� ���� 3��

//1. �ִ� �޺�(maxCombo) = ���� �޺� > �ִ� �޺�
//2. ���� �޺�(addCombo) : ������ ������ ���� �޺� �� ����. (��, ���� �ð� ������ �������� �ʴ´ٸ� ���� �޺��� 0���� �ʱ�ȭ)
//3. �÷��� �� ���� �޺�(curCombo) : ���� �޺� + ���� �޺�

public class LevelTable
{
    public int totalExp = 0; //���� �������� �ʿ��� �� ����ġ
}

public class GlobalGameData : MonoBehaviour
{
    static List<LevelTable> lvTable = new List<LevelTable>();

    public static int bestScore = 0;    //�ְ� ����
    public static int bestCombo = 0;    //�ִ� �޺�


    public static int minTurn;          //�ּ� �̵���
    public static float minTime;        //Ŭ���� �ּ� �ð�

    public static int userLevel = 1;    //���� ����
    public static int curExp = 0;       //�������ġ


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

