using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [Header("Absorb")]
    public float            absorbTime = 0.2f;
    public Transform        absorbPoint;
    public LayerMask        absorbMask;
    public ResourceShower   absorbFX;
    public float            timeBetweenParticles = 0.1f;
    [Header("Jetpack")]
    public float            jetpackDrainSpeed = 0.2f;
    public float            jetpackMaxSpeed = 40.0f;
    public float            jetpackAcceleration = 5.0f;
    [Header("Visuals")]
    public SpriteRenderer   meterSprite;
    public ParticleSystem   jetpackPS;
    public Color[]          resourceColors;

    [Header("Controls")]
    public string           absorb = "Absorb";
    public string           use = "Fire2";

    PlayerController    playerController;
    Animator            anim;
    ResourceType        currentType = ResourceType.None;
    float               ammount = 0.0f;
    Transform           meterTransform;
    TimeScaler2d        timeScaler;
    float               gravityScaleNormal;
    float               timeOfJetpack;
    float               timeOfLastParticle;
    bool                isAbsorbing = false;
    bool                isDumping = false;

    void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();
        anim = GetComponent<Animator>();

        meterTransform = meterSprite.transform;

        var emission = jetpackPS.emission;
        emission.enabled = false;

        playerController = GetComponent<PlayerController>();
        gravityScaleNormal = playerController.gravityJumpMultiplier;
    }
    
    void Update()
    {
        if (currentType == ResourceType.None)
        {
            meterSprite.enabled = false;
        }
        else
        {
            meterSprite.enabled = true;
            meterSprite.color = resourceColors[(int)currentType];
            meterTransform.localScale = new Vector3(1, ammount, 1);
        }

        isDumping = isAbsorbing = false;

        bool enableAbsorbFX = false;

        if (Input.GetButton(absorb))
        {
            if (playerController.isGrounded)
            {
                isAbsorbing = true;

                Resource res = GetResource();

                if ((res) &&
                    (ammount < 1.0f) &&
                    (res.type != ResourceType.None) &&
                    ((res.resourceAmmount > 0.0f) || (res.isInfinite)))
                {
                    if ((currentType == ResourceType.None) ||
                        (currentType == res.type) ||
                        (ammount < 0.05f))
                    {
                        if (currentType != res.type) ammount = 0;

                        ammount = Mathf.Clamp(ammount + Time.deltaTime * absorbTime, 0.0f, 1.0f);
                        currentType = res.type;

                        enableAbsorbFX = true;
                        absorbFX.color = resourceColors[(int)res.type];

                        res.Drain(Time.deltaTime * absorbTime);
                    }
                }
            }
        }
        else
        {
            if (Input.GetButton(use))
            {
                if (ammount > 0)
                {
                    switch (currentType)
                    {
                        case ResourceType.None:
                            break;
                        case ResourceType.Fuel:
                            UseJetpack();
                            break;
                        case ResourceType.Slow:
                            UseSlowOrFast();
                            break;
                        case ResourceType.Speed:
                            UseSlowOrFast();
                            break;
                        case ResourceType.Space:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    var emission = jetpackPS.emission;
                    emission.enabled = false;
                }
            }
            else
            {
                var emission = jetpackPS.emission;
                emission.enabled = false;
            }
        }

        anim.SetBool("Absorb", isAbsorbing || isDumping);
        absorbFX.emit = enableAbsorbFX;
        playerController.EnableMovement(!(isAbsorbing || isDumping));

        if ((timeScaler.time - timeOfJetpack) > 1)
        {
            playerController.gravityJumpMultiplier = gravityScaleNormal;
        }
    }

    void UseJetpack()
    {
        ammount -= Time.deltaTime * jetpackDrainSpeed;

        var emission = jetpackPS.emission;
        emission.enabled = true;

        timeScaler.AddForce(new Vector2(0.0f, jetpackAcceleration), ForceMode2D.Force);

        timeOfJetpack = timeScaler.time;

        playerController.gravityJumpMultiplier = 1.0f;
    }

    void UseSlowOrFast()
    {
        isDumping = true;

        Resource res = GetResource();

        // Check if it is a dump
        if ((res.canDump) &&
            ((res.type == ResourceType.None) || (res.type == currentType)))
        {
            float tmp = Time.deltaTime * absorbTime;

            ammount = Mathf.Clamp(ammount - tmp, 0.0f, 1.0f);

            res.type = currentType;
            res.Drain(-tmp);

            if (Time.time - timeOfLastParticle > timeBetweenParticles)
            {
                absorbFX.SpawnParticle(resourceColors[(int)currentType]);
                timeOfLastParticle = Time.time;
            }

            if (ammount <= 0.0f) currentType = ResourceType.None;
        }
    }

    Resource GetResource()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(absorbPoint.position, 2.0f, absorbMask);

        foreach (var collider in colliders)
        {
            Resource res = collider.GetComponent<Resource>();
            if (res) return res;
        }

        return null;
    }
}
