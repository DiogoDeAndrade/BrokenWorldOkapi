using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMember : MonoBehaviour
{
    public ParticleSystem   psDead;
    public Color[]          crowdColors;

    Animator        anim;
    SpriteRenderer  spriteRenderer;
    Coroutine       cheerCR;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anim.SetBool("Grounded", true);
        anim.speed = UnityEngine.Random.Range(0.25f, 0.5f);

        spriteRenderer.color = crowdColors[UnityEngine.Random.Range(0, crowdColors.Length)];

        cheerCR = StartCoroutine(CheerCR());

        var t = transform.position;
        t.z = t.z + UnityEngine.Random.Range(-0.1f, 0.1f);
        transform.position = t;

        GetComponent<HealthSystem>().onDead += OnDead;

        var main = psDead.main;
        main.startColor = spriteRenderer.color;
    }

    IEnumerator CheerCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 6.0f));

            anim.SetTrigger("Celebrate");

            yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 2.0f));

            anim.SetTrigger("Hit");
        }

    }

    void Update()
    {
        
    }

    private void OnDead()
    {
        anim.SetTrigger("Dead");
        anim.speed = 1.0f;        
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
