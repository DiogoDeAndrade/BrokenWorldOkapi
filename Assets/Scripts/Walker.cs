using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    public float     moveSpeed;
    public Transform groundScanPoint;
    public Transform wallScanPoint;
    public LayerMask solidMask;

    float           inc;
    float           startInc;

    override protected void Start()
    {
        base.Start();

        startInc = inc = Mathf.Sign(wallScanPoint.position.x - transform.position.x);

        Walk();
    }

    void Update()
    {
        if (isDead) return;

        if (groundScanPoint)
        {
            if (!Physics2D.OverlapCircle(groundScanPoint.position, 2.0f, solidMask))
            {
                inc = -inc;
            }
        }

        if (wallScanPoint)
        {
            if (Physics2D.OverlapCircle(wallScanPoint.position, 2.0f, solidMask))
            {
                inc = -inc;
            }
        }

        Walk();
    }

    void Walk()
    {
        timeScaler.originalVelocity = new Vector2(inc * moveSpeed, timeScaler.originalVelocity.y);

        if ((inc * startInc) < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
