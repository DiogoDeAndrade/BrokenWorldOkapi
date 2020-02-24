using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraFollow : MonoBehaviour
{
    public CameraFollow cameraFollowRef;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraFollowRef.mode == CameraFollow.Mode.SimpleFeedbackLoop)
        {
            float dist = Vector3.Distance(cameraFollowRef.transform.position, cameraFollowRef.GetTargetPos());
            if (dist <= 10.0f)
            {
                cameraFollowRef.mode = CameraFollow.Mode.Box;

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            cameraFollowRef.enabled = true;
            cameraFollowRef.mode = CameraFollow.Mode.SimpleFeedbackLoop;
        }
    }
}
