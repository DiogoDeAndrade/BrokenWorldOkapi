using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public enum Mode { Auto, Los };

    public Mode         mode = Mode.Auto;
    public float        cooldown = 1.0f;
    public Shot         shotPrefab;
    public float        wakeTime = 0.0f;
    public float        damage = 25;
    public float        speed = 100;
    public Color        color = Color.white;

    [ShowIf("IsAuto")]
    public List<bool>   sequence;
    [ShowIf("IsAutoAndSequence")]
    public float        timeBetweenSequence;

    [ShowIf("IsLos")]
    public float        range = 100.0f;
    [ShowIf("IsLos")]
    public float        spreadAngle = 90.0f;
    [ShowIf("IsLos")]
    public LayerMask    targetMask;
    [ShowIf("IsLos")]
    public bool         directAtTarget = false;

    int                     index = 0;
    float                   cooldownTimer;
    HealthSystem.Faction    faction;

    bool IsAuto() { return mode == Mode.Auto; }
    bool IsLos() { return mode == Mode.Los; }
    bool IsAutoAndSequence() { return IsAuto() && (sequence != null) && (sequence.Count > 1); }

    void Start()
    {
        cooldownTimer = 0.0f;
        faction = GetComponentInParent<HealthSystem>().faction;
    }

    void Update()
    {
        if (wakeTime > 0)
        {
            wakeTime -= Time.deltaTime;
            return;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        { 
            if (mode == Mode.Auto)
            {
                if ((sequence != null) && (sequence.Count > 0))
                {
                    if (sequence[index]) SpawnShot(Vector3.zero);

                    index = (index + 1) % (sequence.Count);

                    cooldownTimer = timeBetweenSequence;
                }
                else
                {
                    SpawnShot(Vector3.zero);
                    cooldownTimer = cooldown;
                }
            }
            else if (mode == Mode.Los)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, range, targetMask);
                foreach (var collider in colliders)
                {
                    HealthSystem hs = collider.GetComponentInParent<HealthSystem>();
                    if (hs)
                    {
                        if (faction != hs.faction)
                        {
                            Vector3 toTarget = (hs.transform.position - transform.position).normalized;

                            float dp = Vector3.Dot(toTarget, transform.right);
                            if (dp > Mathf.Cos(Mathf.Deg2Rad * spreadAngle))
                            {
                                SpawnShot(toTarget);
                                cooldownTimer = cooldown;
                            }
                        }
                    }
                }
            }
        }

    }

    void SpawnShot(Vector3 toTarget)
    {
        Quaternion rot = transform.rotation;

        Shot shot = Instantiate(shotPrefab, transform.position, transform.rotation);
        if ((directAtTarget) && (toTarget.sqrMagnitude > 0))
        {
            shot.transform.right = toTarget;
        }
        shot.damage = damage;
        shot.speed = speed;
        shot.color = color;
        shot.faction = faction;
    }
}
