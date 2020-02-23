using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : Enemy
{
    public float moveSpeed = 0.05f;
    public float idleTime = 2.0f;
    public float maxDist = 100.0f;
    public float maxDetectionRadius = 200.0f;

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
                if ((pc) && (!pc.isInvulnerable) && (!pc.isDead) && (Vector3.Distance(pc.transform.position, transform.position) < maxDetectionRadius))
                {
                    targetPos = pc.GetFollowTarget().position;

                    Vector3 toTarget = targetPos - transform.position;
                    if (toTarget.sqrMagnitude > 0.01f)
                    {
                        targetPos = transform.position + toTarget.normalized * Mathf.Min(toTarget.magnitude, maxDist);
                    }
                }
                else
                {
                    targetPos = spawnPos.xy() + Random.insideUnitCircle * 50.0f;
                }
            }
        }

        Vector3 newPos = transform.position + (targetPos - transform.position) * moveSpeed * timeScaler.timeScale;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }

    protected override void DealtDamage(PlayerController player)
    {
        targetPos = spawnPos.xy() + Random.insideUnitCircle * 50.0f;
    }
}

