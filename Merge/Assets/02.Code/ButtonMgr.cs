using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMgr : MonoBehaviour
{
    public enum BtnType
    {
        none,
        exit,
        tabs,
        resume,
        pause,
    }

    public BtnType type = BtnType.none;
    public Button btn;

    private void Start()
    {
        if(btn != null)
        {
            btn.onClick.AddListener(Btn);
        }
    }

    void Btn()
    {
        switch (type)
        {
            case BtnType.exit:
                AudioMgr.Inst.PlayBgm(false);
                GameManager.Inst.GoLobby();
                break;

            case BtnType.tabs:
                AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Button);
                break;

            case BtnType.resume:
                GameManager.Inst.Resume();
                AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Button);
                break;

            case BtnType.pause:
                GameManager.Inst.Pause();
                break;
        }    
    }
}
