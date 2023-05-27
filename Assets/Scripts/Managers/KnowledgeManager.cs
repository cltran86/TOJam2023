using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeManager : Singleton<KnowledgeManager>
{
    [SerializeField]
    public bool[] nodes;

    [SerializeField]
    public int[] learnQuotas;

    public void Forget(int node)
    {
        nodes[node] = false;
    }
}
