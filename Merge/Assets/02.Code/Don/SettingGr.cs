using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingGr : MonoBehaviour
{
    public Button closeBtn = null;
    public Toggle bgmToggle = null;
    public Toggle sfxToggle = null;
    
    void Start()
    {
        if (closeBtn != null)
            closeBtn.onClick.AddListener(CloseClick);

        if (bgmToggle != null)
            bgmToggle.onValueChanged.AddListener(BgmOnOff);

        if (sfxToggle != null)
            sfxToggle.onValueChanged.AddListener(SfxOnOff);

        int bgmOnOff = PlayerPrefs.GetInt("BgmOnOff", 1);
        if(bgmToggle != null)
        {
            if (bgmOnOff == 1)
                bgmToggle.isOn = true;
            else
                bgmToggle.isOn = false;
        }

        int sfxOnOff = PlayerPrefs.GetInt("SfxOnOff", 1);
        if (sfxToggle != null)
        {
            if (sfxOnOff == 1)
                sfxToggle.isOn = true;
            else
                sfxToggle.isOn = false;
        }
    }

    public void BgmOnOff(bool val)
    {
        if (bgmToggle != null)
        {
            if (val == true)
                PlayerPrefs.SetInt("BgmOnOff", 1);  //On
            else
                PlayerPrefs.SetInt("BgmOnOff", 0);  //Off

            AudioMgr.Inst.BGMOnOff(val);
        }
    }

    public void SfxOnOff(bool val)
    {

        if (sfxToggle != null)
        {
            if (val == true)
                PlayerPrefs.SetInt("SfxOnOff", 1);  //On
            else
                PlayerPrefs.SetInt("SfxOnOff", 0);  //Off

            AudioMgr.Inst.SFXOnOff(val);
        }
    }

    void CloseClick()
    {
        Destroy(gameObject);
    }
}
