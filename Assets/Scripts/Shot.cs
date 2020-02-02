using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public HealthSystem.Faction faction;
    public float                damage = 0;
    public float                speed = 0;
    public Color                color = Color.white;
    public LayerMask            treatAsSolidMask;
    public GameObject           explosionPrefab;

    TimeScaler2d    timeScaler;

    void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();

        timeScaler.originalVelocity = transform.right * speed;

        Gradient      g = new Gradient();
        g.FromColor(color);

        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.colorGradient = g;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (treatAsSolidMask.HasLayer(collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem)
            {
                if (healthSystem.faction != faction)
                {
                    healthSystem.DealDamage(damage);
                }
                else
                {
                    return;
                }
            }

            Destroy(gameObject);

            if (explosionPrefab)
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
            }
        }
    }
}
