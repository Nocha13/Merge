using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabCtrl : MonoBehaviour
{
    static TabCtrl Inst = null;

    public static TabCtrl inst
    {
        get
        {
            if (Inst == null)
            {
                GameObject.FindObjectOfType<TabCtrl>();

                if(Inst == null)
                {
                    Debug.LogError("There's no active TabCtrl objects");
                }
            }
            return Inst;
        }
    }

    TabBtn tabBtn;

    public void SelTabBtn(TabBtn btn)
    {
        if(tabBtn != null)
        {
            tabBtn.Deselect();
        }

        tabBtn = btn;
        tabBtn.Select();
    }
}
