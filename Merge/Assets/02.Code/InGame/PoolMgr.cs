using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : MonoBehaviour
{
    public static PoolMgr poolInst;
    public GameManager Inst;
    public List<Merge> mergePool;
    public List<ParticleSystem> particlePool;

    public GameObject mergePrefeb;
    public Transform mergeGroup;

    public GameObject effectPrefeb;
    public Transform effectGroup;

    [Range(1, 30)]
    public int poolSize;
    public int poolCursor;

    void Awake()
    {
        poolInst = this;
        mergePool = new List<Merge>();
        particlePool = new List<ParticleSystem>();

        for(int idx = 0; idx < poolSize; idx++)
        {
            MakeMerge();
        }
    }

    public Merge MakeMerge()
    {
        //Effect
        GameObject instEffectObj = Instantiate(effectPrefeb, effectGroup);
        instEffectObj.name = "Effect " + particlePool.Count;
        ParticleSystem instEffect = instEffectObj.GetComponent<ParticleSystem>();
        particlePool.Add(instEffect);

        //Merge
        GameObject instMergeObj = Instantiate(mergePrefeb, mergeGroup);
        instMergeObj.name = "Merge " + mergePool.Count;
        Merge instMerge = instMergeObj.GetComponent<Merge>();
        instMerge.particle = instEffect;
        mergePool.Add(instMerge);
        return instMerge;
    }

    public Merge GetMerge()
    {
        for (int idx = 0; idx < mergePool.Count; idx++)
        {
            poolCursor = (poolCursor + 1) % mergePool.Count;
            if (!mergePool[poolCursor].gameObject.activeSelf)
                return mergePool[poolCursor];
        }

        return MakeMerge();
    }
}
