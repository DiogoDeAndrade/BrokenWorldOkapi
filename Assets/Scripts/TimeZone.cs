using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZone : Resource
{
    public float        speedModifier = 1.0f;
    public LayerMask    layersToAffect;

    float           initialSpeed;
    Color           color;
    SpriteRenderer  spriteRenderer;

    override public bool canDump
    {
        get
        {
            return (resourceAmmount < 1.0f);
        }
    }

    void Awake()
    {
        if (speedModifier > 1.0f)
            type = ResourceType.Speed;
        else if (speedModifier < 1)
            type = ResourceType.Slow;
        initialSpeed = speedModifier;
        resourceAmmount = 1.0f;

        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (layersToAffect.HasLayer(collider.gameObject.layer))
        {
            TimeScaler2d ts = collider.GetComponentInParent<TimeScaler2d>();

            if (ts)
            {
                ts.SetScale(speedModifier);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (layersToAffect.HasLayer(collider.gameObject.layer))
        {
            TimeScaler2d ts = collider.GetComponentInParent<TimeScaler2d>();

            if (ts)
            {
                ts.SetScale(1.0f);
            }
        }
    }

    public override void Drain(float r)
    {
        resourceAmmount = Mathf.Clamp(resourceAmmount - r, 0.0f, 1.0f);

        speedModifier = Mathf.Lerp(1.0f, initialSpeed, resourceAmmount);

        spriteRenderer.color = Color.Lerp(color.ChangeAlpha(0), color, resourceAmmount);
    }
}
