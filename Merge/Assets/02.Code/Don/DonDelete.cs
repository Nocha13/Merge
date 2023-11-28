using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDelete : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
