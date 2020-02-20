using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMember : MonoBehaviour
{
    public Color[]  crowdColors;

    Animator        anim;
    SpriteRenderer  spriteRenderer;
    Coroutine       cheerCR;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anim.SetBool("Grounded", true);
        anim.speed = Random.Range(0.25f, 0.5f);

        spriteRenderer.color = crowdColors[Random.Range(0, crowdColors.Length)];

        cheerCR = StartCoroutine(CheerCR());

        var t = transform.position;
        t.z = t.z + Random.Range(-0.1f, 0.1f);
        transform.position = t;
    }

    IEnumerator CheerCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));

            anim.SetTrigger("Celebrate");

            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));

            anim.SetTrigger("Hit");
        }

    }

    void Update()
    {
        
    }
}
