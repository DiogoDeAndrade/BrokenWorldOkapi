using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
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

    enum ResourceType { None = 0, Fuel = 1, Slow = 2, Speed = 3, Space = 4 };

    PlayerController    playerController;
    ResourceType        currentType = ResourceType.Fuel;
    float               ammount = 1.0f;
    Transform           meterTransform;
    TimeScaler2d        timeScaler;
    float               gravityScaleNormal;
    float               timeOfJetpack;

    void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();

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
}
