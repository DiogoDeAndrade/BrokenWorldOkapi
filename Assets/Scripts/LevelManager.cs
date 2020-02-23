using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.Events;

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

    [Header("Events")]
    public UnityEvent       onSpawn;

    PlayerController player;
    Resource[]       resources;
    AudioSource      blipSound;

    public bool textEnabled
    {
        get { return textBox.gameObject.activeInHierarchy; }
    }

    void Start()
    {
        blipSound = textBox.GetComponent<AudioSource>();

        CheckAndSpawnPlayer();

        if ((text != null) && (text.Length > 0))
        {
            StartCoroutine(TutorialCR());
        }
        else
        {
            textBox.gameObject.SetActive(false);
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
                if (onSpawn != null)
                {
                    onSpawn.Invoke();
                }

                CameraFollow cf = FindObjectOfType<CameraFollow>();
                if (cf)
                {
                    cf.targetObject = (player.followPos) ? player.followPos : player.transform;
                }
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
            if (blipSound) blipSound.Play();

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

        if (blipSound) blipSound.Play();

        textBox.gameObject.SetActive(false);

        if (player)
        {
            player.EnableControls(true);
        }
    }

    public void DisplayText(string str)
    {
        StartCoroutine(DisplayTextCR(str));
    }

    IEnumerator DisplayTextCR(string str)
    {
        textBox.gameObject.SetActive(true);

        if (player)
        {
            player.EnableControls(false);
        }

        if (blipSound) blipSound.Play();

        textComponent.text = str;

        while (Input.anyKeyDown)
        {
            yield return null;
        }

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        textBox.gameObject.SetActive(false);

        if (blipSound) blipSound.Play();

        if (player)
        {
            player.EnableControls(true);
        }
    }
}
