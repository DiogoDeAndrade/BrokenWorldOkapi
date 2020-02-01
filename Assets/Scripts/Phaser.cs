using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : Enemy
{
    public float moveSpeed = 0.05f;
    public float idleTime = 2.0f;

    Vector3 targetPos;
    float   waitTime;

    protected override void Start()
    {
        base.Start();

        targetPos = transform.position;
        waitTime = idleTime;
    }

    protected override void RunEnemy()
    {
        float dist = Vector3.Distance(targetPos, transform.position);
        if (dist < 1.0f)
        {
            if (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime * timeScaler.timeScale;
            }
            else
            {
                waitTime = idleTime;

                // Find player
                PlayerController pc = GameObject.FindObjectOfType<PlayerController>();
                if (pc)
                {
                    targetPos = pc.GetFollowTarget().position;
                }
            }
        }

        Vector3 newPos = transform.position + (targetPos - transform.position) * moveSpeed * timeScaler.timeScale;
        transform.position = newPos;
    }
}
