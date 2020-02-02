using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : Enemy
{
    public float        patrolRadius = 50.0f;
    public float        moveSpeed = 50.0f;
    public float        waitTime = 0.0f;   
    public Transform    frontFacing;

    Vector2 targetPos;
    float   waitTimer;
    Vector3 prevPos;
    float   front;

    protected override void Start()
    {
        base.Start();

        targetPos = spawnPos + Random.insideUnitSphere * patrolRadius;

        front = Mathf.Sign(frontFacing.position.x - transform.position.x);
    }

    void Update()
    {
        if (waitTimer > 0.0f)
        {
            waitTimer -= timeScaler.deltaTime;
            return;
        }

        float dist = Vector3.Distance(targetPos, transform.position);

        if (dist < 1.0f)
        {
            waitTimer = waitTime;
            targetPos = spawnPos + Random.insideUnitSphere * patrolRadius;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * timeScaler.deltaTime);
        }

        float dir = Mathf.Sign(transform.position.x - prevPos.x);

        if ((dir * (front * transform.right.x)) < 0.0f)
        {
            if (dir != front) transform.rotation = Quaternion.Euler(0, 180, 0);
            else transform.rotation = Quaternion.identity;
        }

        prevPos = transform.position;
    }
}
