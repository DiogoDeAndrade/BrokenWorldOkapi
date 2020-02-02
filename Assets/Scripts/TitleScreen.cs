using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public CanvasGroup      titleCanvasGroup;
    public RectTransform    textRT;

    void Start()
    {
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

        while ((y < 1200.0f) && (!Input.anyKeyDown))
        {
            y += Time.deltaTime * 50.0f;

            textRT.anchoredPosition = new Vector2(0, y);

            yield return null;
        }

        SceneManager.LoadScene("Level1");
    }
}
