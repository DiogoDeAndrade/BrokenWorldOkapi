using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : ActivatedComponent
{
    public LayerMask    playerDetectionMask;
    public float        detectionRadius = 50.0f;
    public float        enterRadius = 10.0f;
    public string       targetScene = "";
    public AudioSource  successSound;

    Animator    anim;
    Coroutine   enteringCR;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("Lock", !active);

        if (active)
        {
            var collider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerDetectionMask);

            if (collider)
            {
                var player = collider.GetComponentInParent<PlayerController>();

                if (player)
                {
                    anim.SetBool("Open", true);

                    if (enteringCR == null)
                    {
                        if (Vector3.Distance(collider.transform.position, transform.position) < enterRadius)
                        {
                            enteringCR = StartCoroutine(EnterCR(player));
                        }
                    }
                }
            }
            else
            {
                anim.SetBool("Open", false);
            }
        }
    }

    IEnumerator EnterCR(PlayerController player)
    {
        if (successSound) successSound.Play();

        player.EnableMovement(false);
        player.GetComponent<Backpack>().enabled = false;

        player.Celebrate();

        yield return new WaitForSeconds(1.0f);

        FullscreenFader.FadeOut(1.0f);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(targetScene);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enterRadius);
    }
}
