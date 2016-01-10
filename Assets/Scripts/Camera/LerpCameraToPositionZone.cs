using UnityEngine;
using System.Collections;

public class LerpCameraToPositionZone : MonoBehaviour {

    public Transform targetTransform;

    private Transform SlothReference;
    private float startSlothDistance;
    private TopDownCamera topDownCamera;


    void Start() {
        topDownCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TopDownCamera>();
    }


    void OnTriggerEnter(Collider other) {
        SlothController slothController = other.gameObject.GetComponentInChildren<SlothController>();
        if(slothController != null) {
            SlothReference = slothController.transform;
            startSlothDistance = (SlothReference.transform.position - targetTransform.position).magnitude;
            topDownCamera.lerpCameraToPositionZone = this;
        }

        
    }

    void OnTriggerExit(Collider other) {
        SlothController slothController = other.gameObject.GetComponentInChildren<SlothController>();
        if (slothController != null) {
            topDownCamera.lerpCameraToPositionZone = null;
        }

        
    }

    public float GetLerpAlpha() {
        float alpha = 1 - ((SlothReference.transform.position - targetTransform.position).magnitude / startSlothDistance);
        return (alpha + 0.2f);
    }
}
