using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResearchNode
{
    [SerializeField]
    public ResearchNode parentNode;

    public int  villagers,
                builders,
                fighters,
                houses,
                trainingGrounds,
                universities,
                decorated;

    public int resourceCost, researchTime;

    public bool isAtRisk,
                isForgotten;
}

public class KnowledgeManager : Singleton<KnowledgeManager>
{
    [SerializeField]
    public ResearchNode[] nodes;

  //  [SerializeField]
//    public bool[] nodes;

    [SerializeField]
    public int[] learnQuotas;

    public void Forget(int node)
    {
        nodes[node].isForgotten = true;
//        nodes[node] = false;
    }
}
