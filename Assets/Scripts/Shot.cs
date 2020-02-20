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
    Vector3         prevPos;
    int             layerMask = 0;

    void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();

        timeScaler.originalVelocity = transform.right * speed;

        Gradient      g = new Gradient();
        g.FromColor(color);

        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.colorGradient = g;

        prevPos = transform.position;

        var layer = gameObject.layer;
        layerMask = 0;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics2D.GetIgnoreLayerCollision(layer, i))
            {
                layerMask |= 1 << i;
            }
        }
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
                Vector3         dir = transform.position - prevPos;

                RaycastHit2D    rayHit = Physics2D.Raycast(prevPos, dir.normalized, dir.magnitude * 1.5f, layerMask);
                if (rayHit)
                {
                    Instantiate(explosionPrefab, rayHit.point, transform.rotation);
                }
            }
        }
    }

    private void LateUpdate()
    {
        prevPos = transform.position;
    }
}
