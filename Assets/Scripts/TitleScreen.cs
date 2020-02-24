using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public CanvasGroup      titleCanvasGroup;
    public RectTransform    textRT;
    public string           startLevel = "Level1";

    void Start()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif

        StartCoroutine(TitleCR());        
    }

    IEnumerator TitleCR()
    {
        yield return new WaitForSeconds(1.0f);

        float t = Time.time;

        while (((Time.time - t) < 2.0f) && (!Input.anyKeyDown))
        {
            yield return null;
        }

        while (Input.anyKeyDown)
        {
            yield return null;
        }

        float alpha = titleCanvasGroup.alpha;

        while (alpha > 0.1f)
        {
            alpha -= Time.deltaTime;

            titleCanvasGroup.alpha = alpha;

            yield return null;
        }

        float y = textRT.anchoredPosition.y;

        while ((y < 1500.0f) && (!Input.anyKeyDown))
        {
            y += Time.deltaTime * 50.0f;

            textRT.anchoredPosition = new Vector2(textRT.anchoredPosition.x, y);

            yield return null;
        }

        FullscreenFader.FadeOut(2.0f);

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(startLevel);
    }
}
