using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabBtn : MonoBehaviour
{
    public UnityEvent onTabSel;
    public UnityEvent OnTabDesel;

    public void Select()
    {
        if (onTabSel != null)
            onTabSel.Invoke();
    }

    public void Deselect()
    {
        if (OnTabDesel != null)
            OnTabDesel.Invoke();
    }

    public void OnSelTab(TabBtn btn)
    {
        TabCtrl.inst.SelTabBtn(btn);
    }
}
