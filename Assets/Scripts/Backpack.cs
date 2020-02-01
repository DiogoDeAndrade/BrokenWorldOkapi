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

        bool isAbsorbing = false;
        bool enableAbsorbFX = false;

        if (Input.GetButton(absorb))
        {
            if (playerController.isGrounded)
            {
                isAbsorbing = true;

                Resource res = GetResource();

                if ((res) && (ammount < 1.0f))
                {
                    if ((currentType == ResourceType.None) ||
                        (currentType == res.type))
                    {
                        ammount = Mathf.Clamp(ammount + Time.deltaTime * absorbTime, 0.0f, 1.0f);
                        currentType = res.type;

                        enableAbsorbFX = true;

                        res.Drain(Time.deltaTime * absorbTime);
                    }
                }
            }
        }

        anim.SetBool("Absorb", isAbsorbing);
        absorbFX.emit = enableAbsorbFX;
        playerController.EnableMovement(!isAbsorbing);

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
                        break;
                    case ResourceType.Speed:
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
