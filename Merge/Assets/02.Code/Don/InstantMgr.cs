using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantMgr : MonoBehaviour
{
    public enum MgrType
    {
        none,
        main,
        game,
    }

    public MgrType type = MgrType.none;

    [Header("------ ConfigBox ------")]
    public Button SettingBtn = null;
    public GameObject canvas = null;
    GameObject settingBox = null;

    void Start()
    {
        if (SettingBtn != null)
            SettingBtn.onClick.AddListener(() =>
            {
                switch (type)
                {
                    case MgrType.main:
                        if (settingBox == null)
                            settingBox = Resources.Load("MainSettingGr") as GameObject;

                        GameObject a_MsettingBox = Instantiate(settingBox) as GameObject;
                        a_MsettingBox.transform.SetParent(canvas.transform, false);
                        break;

                    case MgrType.game:
                        if (settingBox == null)
                            settingBox = Resources.Load("GameSettingGr") as GameObject;

                        GameObject a_GsettingBox = Instantiate(settingBox) as GameObject;
                        a_GsettingBox.transform.SetParent(canvas.transform, false);
                        break;
                }
            });
    }
}
