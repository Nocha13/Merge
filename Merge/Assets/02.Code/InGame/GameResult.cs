using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject[] Titles;
    public static GameResult Inst;

    public void Awake()
    {
        Inst = this;
    }

    public void GameClear()
    {
        Titles[0].SetActive(true);
    }

    public void GameOver()
    {
        Titles[1].SetActive(true);
    }
}

