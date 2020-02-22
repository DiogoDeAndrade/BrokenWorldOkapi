using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.LWRP;

public class ExplosionCutscene : MonoBehaviour
{
    public Transform    targetPos;
    public Animator     commander;
    new public Light2D  light;
    public AudioSource  explosionSound;
    public Transform[]  explosionPoints;
    public GameObject   explosionPrefab;
    public AudioSource  alarmSound;
    public AudioSource  crowdSound;

    LevelManager    levelManager;

    public void RunCutscene()
    {
        levelManager = GetComponent<LevelManager>();

        StartCoroutine(RunCutsceneCR());
    }

    IEnumerator RunCutsceneCR()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();

        commander.SetBool("Grounded", true);
        pc.enableControls = false;

        yield return new WaitForSeconds(1.0f);

        pc.enabled = false;

        Animator playerAnim = pc.GetComponent<Animator>();
        playerAnim.SetFloat("AbsSpeedX", pc.moveSpeed);

        Destroy(pc.GetComponent<HealthSystem>());

        while (playerAnim.transform.position.x < targetPos.position.x)
        {
            playerAnim.transform.position = Vector3.MoveTowards(playerAnim.transform.position, targetPos.position, pc.moveSpeed * Time.deltaTime);

            yield return null;
        }

        playerAnim.SetFloat("AbsSpeedX", 0.0f);

        yield return new WaitForSeconds(1.0f);

        commander.SetTrigger("Celebrate");
        commander.speed = 0.1f;

        levelManager.DisplayText("Congratulation, agent!");
        while (levelManager.textEnabled) { yield return null; }

        levelManager.DisplayText("You are ready to join these men and women in the field, fighting for our Universe's salvation!");
        while (levelManager.textEnabled) { yield return null; }

        levelManager.DisplayText("Now, just let me...");
        while (levelManager.textEnabled) { yield return null; }

        StartCoroutine(AlarmCR());
        alarmSound.Play();
        crowdSound.Stop();

        yield return new WaitForSeconds(1.0f);

        levelManager.DisplayText("What's this? An alarm? Impossible!");
        while (levelManager.textEnabled) { yield return null; }

        for (int i = 0; i < 3; i++)
        {
            CameraShake2d.Shake(1.0f * (i + 1), 0.5f);
            explosionSound.volume = 0.2f * (i + 1);
            explosionSound.Play();

            yield return new WaitForSeconds(0.75f);
        }

        levelManager.DisplayText("What...");
        while (levelManager.textEnabled) { yield return null; }

        for (int i = 0; i < explosionPoints.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);

            CameraShake2d.Shake(2.0f, 0.5f);
            explosionSound.volume = 0.8f;
            explosionSound.Play();

            Instantiate(explosionPrefab, explosionPoints[i].position, explosionPoints[i].rotation);
        }

        FullscreenFader.FadeOut(0.6f, Color.white);

        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadScene("RealGame");
    }

    IEnumerator AlarmCR()
    {
        float a = 0.0f;

        while (true)
        {
            float t = Mathf.Cos(Mathf.PI * 0.5f + a * 5.0f) * 0.5f + 0.5f;

            light.color = Color.Lerp(new Color(1.0f, 0.2f, 0.2f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), t);

            a += Time.deltaTime;

            yield return null;
        }
    }
}
