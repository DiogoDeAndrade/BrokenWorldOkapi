using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float        baseDamage = 25.0f;
    public float        freezeAfterDamage = 2.0f;
    public AudioSource  deathSound;
    public GameObject   deathFXPrefab;
    public AudioSource  hitSound;

    protected TimeScaler2d      timeScaler;
    protected SpriteRenderer    spriteRenderer;
    protected float             freezeTimer;
    protected HealthSystem      healthSystem;
    protected Vector3           spawnPos;
    protected Animator          anim;

    public bool isInvulnerable
    {
        get
        {
            return healthSystem.isInvulnerable;
        }
    }

    public bool isDead
    {
        get
        {
            return healthSystem.isDead;
        }
    }

    protected virtual void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.isInvulnerable = true;
        healthSystem.onHit += OnHit;
        healthSystem.onDead += OnDead;
        freezeTimer = healthSystem.invulnerabilityTime;
        spawnPos = transform.position;
        anim = GetComponent<Animator>();

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
        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
        }
        else if (!healthSystem.isDead)
        {
            RunEnemy();
        }
    }

    protected virtual void RunEnemy()
    {

    }

    protected virtual void DealtDamage(PlayerController player)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player)
        {
            if (isDead) return;
            if (player.isInvulnerable) return;
            if (isInvulnerable) return;

            HealthSystem hs = player.GetComponent<HealthSystem>();

            if (hs.DealDamage(baseDamage))
            { 
                freezeTimer = freezeAfterDamage;

                DealtDamage(player);
            }
        }
    }

    private void OnDead()
    {
        anim.SetTrigger("Dead");

        if (deathSound) deathSound.Play();
        if (deathFXPrefab)
        {
            Instantiate(deathFXPrefab, transform.position, transform.rotation);
        }
    }

    private void OnHit(float damage)
    {
        healthSystem.isInvulnerable = true;

        anim.SetTrigger("Hit");

        if (hitSound) hitSound.Play();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
