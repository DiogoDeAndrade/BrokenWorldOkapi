using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Weapon : MonoBehaviour
{
    public Shot         shotPrefab;
    public float        damage = 25;
    public float        speed = 100;
    public Color        color = Color.white;
    public float        cooldown = 1.0f;
    public GameObject   muzzleFlash;
    public bool         shakeEnable;
    [ShowIf("shakeEnable")]
    public float        shakeStrength = 1;
    [ShowIf("shakeEnable")]
    public float        shakeTime = 0.05f;

    [Header("References")]
    public Transform    shootPosition;

    [Header("Controls")]
    public string fire = "Fire2";

    Animator                anim;
    TimeScaler2d            timeScaler;
    float                   cooldownTimer = 0.0f;
    bool                    buttonsReleased = true;
    HealthSystem.Faction    faction;

    void Start()
    {
        anim = GetComponent<Animator>();
        timeScaler = GetComponent<TimeScaler2d>();
        faction = GetComponent<HealthSystem>().faction;
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
                    Instantiate(muzzleFlash, shootPosition.position - Vector3.forward * 0.1f, shootPosition.rotation);
                }

                Shot shot = Instantiate(shotPrefab, shootPosition.position, shootPosition.rotation);
                shot.damage = damage;
                shot.speed = speed;
                shot.color = color;
                shot.faction = faction;

                cooldownTimer = cooldown;
                buttonsReleased = false;

                if (shakeEnable)
                {
                    CameraShake2d.Shake(shakeStrength, shakeTime);
                }
            }
        }
        else
        {
            buttonsReleased = true;
        }
    }
}
