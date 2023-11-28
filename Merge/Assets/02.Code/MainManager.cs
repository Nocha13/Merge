using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    void InGameSceneLoad()
    {
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Button);
        SceneManager.LoadScene("InGame");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
    }
}
