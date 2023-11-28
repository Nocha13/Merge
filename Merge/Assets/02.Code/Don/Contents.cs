using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contents : MonoBehaviour
{
    public RectTransform contents;

    void OnDisable()
    {
        float x = contents.anchoredPosition.x;
        contents.anchoredPosition = new Vector3(x, 0, 0);
    }
}
