using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZone : MonoBehaviour
{
    public float        speedModifier = 1.0f;
    public LayerMask    layersToAffect;

    void Start()
    {
        
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
}
