using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType type;
    public bool         isInfinite = false;
    [HideIf("isInfinite")]
    public float        resourceAmmount = 1.0f;

    virtual public bool canDump
    {
        get
        {
            return false;
        }
    }
        
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    virtual public void Drain(float r)
    {

    }
}
