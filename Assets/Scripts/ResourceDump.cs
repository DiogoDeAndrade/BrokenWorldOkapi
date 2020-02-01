using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDump : Resource
{
    public SpriteRenderer fillSprite;
    public float          maxAmmount = 1.0f;
    public Color[]        resourceColors;

    override public bool canDump
    {
        get
        {
            return (resourceAmmount < maxAmmount);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Drain(float r)
    {
        resourceAmmount = Mathf.Clamp(resourceAmmount - r, 0.0f, 1.0f);

        fillSprite.transform.localScale = new Vector3(1, Mathf.Lerp(0.2f, 1.0f, resourceAmmount / maxAmmount), 1);

        fillSprite.color = resourceColors[(int)type];
    }
}
