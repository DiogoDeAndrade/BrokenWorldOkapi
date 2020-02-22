using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillRedShirts : MonoBehaviour
{
    public float explosionRadius = 0.0f;
    public float explosionTime = 1.0f;

    float elapsedTime = 0.0f;

    HealthSystem[] crowd;

    void Start()
    {
        crowd = FindObjectsOfType<HealthSystem>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > explosionTime)
        {
            Destroy(gameObject);
            return;
        }

        float currentRadius = Mathf.Lerp(0.0f, explosionRadius, elapsedTime / explosionTime);

        foreach (var c in crowd)
        {
            if (c.health > 0.0f)
            {
                if (Vector3.Distance(transform.position, c.transform.position) < currentRadius)
                {
                    c.DealDamage(1000.0f);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
