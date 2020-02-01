using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float baseDamage = 25.0f;
    public float freezeAfterDamage = 2.0f;

    protected TimeScaler2d      timeScaler;
    protected SpriteRenderer    spriteRenderer;
    protected float             freezeTimer;
    protected HealthSystem      healthSystem;

    public bool invulnerable
    {
        get
        {
            return healthSystem.isInvulnerable;
        }
    }

    protected virtual void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.isInvulnerable = true;
        freezeTimer = healthSystem.invulnerabilityTime;

        StartCoroutine(FadeInEnemy());
    }

    IEnumerator FadeInEnemy()
    {
        bool b = healthSystem.invulnerabilityBlink;
        healthSystem.invulnerabilityBlink = false;

        Color c = spriteRenderer.color;
        c.a = 0.0f;

        while (c.a < 1.0f)
        {
            spriteRenderer.color = c;

            yield return null;

            c.a += Time.deltaTime / healthSystem.invulnerabilityTime;
        }

        spriteRenderer.color = c.ChangeAlpha(1.0f);

        healthSystem.invulnerabilityBlink = b;
    }

    void Update()
    {
        if (healthSystem.isInvulnerable)
        { 
            // Run effect
        }

        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
        }
        else
        {
            RunEnemy();
        }
    }

    protected virtual void RunEnemy()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player)
        {
            if (player.invulnerable) return;
            if (invulnerable) return;

            player.DealDamage(baseDamage);

            freezeTimer = freezeAfterDamage;
        }
    }
}
