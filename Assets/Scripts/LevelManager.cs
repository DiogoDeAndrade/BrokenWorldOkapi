using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
    [Header("Rules")]
    public  bool            openDoorWhenNoAnomalies = false;
    [ShowIf("openDoorWhenNoAnomalies")]
    public  Door            door;

    [TextArea]
    public string[]         text;

    [Header("References")]
    public Transform        spawnPoint;
    public PlayerController playerPrefab;
    public RectTransform    textBox;
    public TextMeshProUGUI  textComponent;

    PlayerController player;
    Resource[]       resources;

    void Start()
    {
        CheckAndSpawnPlayer();

        if ((text != null) && (text.Length > 0))
        {
            StartCoroutine(TutorialCR());
        }

        resources = FindObjectsOfType<Resource>();
    }

    void CheckAndSpawnPlayer()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
            if (player == null)
            {
                player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

    void Update()
    {
        CheckAndSpawnPlayer();

        if (openDoorWhenNoAnomalies)
        {
            bool allGone = true;
            foreach (var r in resources)
            {
                if ((r.type == ResourceType.Slow) ||
                    (r.type == ResourceType.Speed) ||
                    (r.type == ResourceType.Space))
                {
                    if (r is ResourceDump) continue;

                    if (r.resourceAmmount > 0) allGone = false;
                }
            }

            if (allGone)
            {
                door.active = true;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Escape)) &&
            (Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.RightShift))))
        {
            Application.Quit();
        }
    }

    IEnumerator TutorialCR()
    {
        textBox.gameObject.SetActive(true);

        if (player)
        {
            player.EnableControls(false);
        }

        foreach (var t in text)
        {
            textComponent.text = t;

            while (!Input.anyKeyDown)
            {
                yield return null;
            }

            while (Input.anyKeyDown)
            {
                yield return null;
            }
        }

        textBox.gameObject.SetActive(false);

        if (player)
        {
            player.EnableControls(true);
        }
    }
}
