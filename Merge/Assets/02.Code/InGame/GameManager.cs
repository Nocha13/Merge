using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    [Header("Merge")]
    public Merge lastMerge;

    [Header("Pooling")]
    public GameObject PoolMgrPrefeb;

    [Header("Objects")]
    public GameObject endGr;
    
    [Header("Game Datas")]
    public float gameTime; // = curTime
    public static int curScore = 0; //현재 점수
    public static int curCombo = 0; //현재 콤보
    public static int curTurn = 0;  //현재 이동수
    public static int maxLevel = 0; //MergeLv
    public static bool isTurn;

    public static bool isOver;
    public static bool isClear;
    bool isStart;

    void Awake()
    {
        isOver = false;
        isClear = false;
        Inst = this;
        AudioMgr.Inst.PlayBgm(AudioMgr.BGM.Title);
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        curScore = 0;       //초기화
        curTurn = 0;
        curCombo = 0;
        maxLevel = 0;
        Time.timeScale = 1;
        GameObject poolMgr = Instantiate(PoolMgrPrefeb);
        Invoke("NextMerge", 1.5f);
    }

    void Update()
    {
        if(isStart == true)
            gameTime += Time.deltaTime; 

        //디버그용
        //if (Input.GetKeyDown(KeyCode.Space))
        //    GameOver();
        //if (Input.GetKeyDown(KeyCode.C))
        //    GameClear();

        if (maxLevel == 7)  //게임 클리어 조건
            GameClear();
    }

    void NextMerge()
    {
        if (isOver)
            return;

        lastMerge = PoolMgr.poolInst.GetMerge();
        lastMerge.Inst = this;
        lastMerge.level = Random.Range(0, maxLevel);
        lastMerge.gameObject.SetActive(true);
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Next);
        isStart = true;
        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while (lastMerge != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        NextMerge();
    }

    public void Down()
    {
        if (lastMerge == null)
            return;

        //lastMerge.Drag();
    }

    public void Up()
    {
        if (lastMerge == null)
            return;

        curTurn++;
        lastMerge.Drop();
        lastMerge = null;
    }

    public void GameOver()
    {
        if (isOver)
            return;

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        //활성화 되있는 모든 오브젝트 가져오기(목록)
        Merge[] merges = FindObjectsOfType<Merge>();

        for (int idx = 0; idx < merges.Length; idx++)
        {
            merges[idx].rigid.simulated = false;  
        }

        //목록에 하나씩 접근 및 지우기
        for (int idx = 0; idx < merges.Length; idx++)
        {
            merges[idx].Hide(Vector3.up * 1000);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        AudioMgr.Inst.PlayBgm(false);

        isOver = true;
        if(isOver == true)
            GlobalGameData.GetExp();
        isTurn = true;

        endGr.SetActive(true);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.GameOver);
        GameResult.Inst.GameOver();

        Time.timeScale = 0;
    }

    public void GameClear()
    {
        if (isClear)
            return;

        StartCoroutine(GameClearRoutine());
    }

    IEnumerator GameClearRoutine()
    {
        //활성화 되있는 모든 오브젝트 가져오기(목록)
        Merge[] merges = FindObjectsOfType<Merge>();

        for (int idx = 0; idx < merges.Length; idx++)
        {
            merges[idx].rigid.simulated = false;
        }

        //목록에 하나씩 접근 및 지우기
        for (int idx = 0; idx < merges.Length; idx++)
        {
            merges[idx].Hide(Vector3.up * 1000);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        AudioMgr.Inst.PlayBgm(false);
        
        isClear = true;
        if (isClear == true)
            GlobalGameData.GetExp();
        isTurn = true;

        endGr.SetActive(true);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.GameOver);
        GameResult.Inst.GameClear();

        #region //최소 이동수
        int a_MinCount = curTurn;
        int a_OldCount = PlayerPrefs.GetInt("MinTurn", 0);
        if (0 < a_OldCount) //두번째 이상 플레이라는 의미(게임을 한 적이 있으면)
        {
            if (a_OldCount < a_MinCount)
            {
                a_MinCount = a_OldCount;
            }
        }
        PlayerPrefs.SetInt("MinTurn", a_MinCount);
        GlobalGameData.minTurn = a_MinCount;
        #endregion

        #region //최소 시간
        float a_MinTime = gameTime;
        float a_OldTime = PlayerPrefs.GetFloat("MinTime", 0);
        if (0 < a_OldTime)
        {
            if (a_OldTime < a_MinTime)
            {
                a_MinTime = a_OldTime;
            }
        }
        PlayerPrefs.SetFloat("MinTime", a_MinTime);
        GlobalGameData.minTime = a_MinTime;
        #endregion

        #region //콤보
        if(curCombo > GlobalGameData.bestCombo)
        PlayerPrefs.SetInt("BestCombo", curCombo);
        #endregion

        Time.timeScale = 0;
    }

    public void Reset()
    {
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Button);
        SceneManager.LoadScene("InGame");
    }

    public void GoLobby()
    {      
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Button);
        SceneManager.LoadScene("MainScene");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
