using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Shot         shotPrefab;
    public float        damage = 25;
    public float        speed = 100;
    public Color        color = Color.white;
    public float        cooldown = 1.0f;
    public GameObject   muzzleFlash;

    [Header("References")]
    public Transform    shootPosition;

    [Header("Controls")]
    public string fire = "Fire2";

    Animator        anim;
    TimeScaler2d    timeScaler;
    float           cooldownTimer = 0.0f;
    bool            buttonsReleased = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        timeScaler = GetComponent<TimeScaler2d>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime * ((timeScaler) ? (timeScaler.timeScale) : (1.0f));
        }

        if ((Input.GetButtonDown(fire)) || (Input.GetAxis("Shoot") != 0))
        {
            if ((cooldownTimer <= 0.0f) && (buttonsReleased))
            {
                anim.SetTrigger("Attack");

                if (muzzleFlash)
                {
                    Instantiate(muzzleFlash, shootPosition.position, shootPosition.rotation);
                }

                Shot shot = Instantiate(shotPrefab, shootPosition.position, shootPosition.rotation);
                shot.damage = damage;
                shot.speed = speed;
                shot.color = color;

                cooldownTimer = cooldown;
                buttonsReleased = false;
            }
        }
        else
        {
            buttonsReleased = true;
        }
    }
}
