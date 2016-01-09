using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

    [SerializeField]
    private float speed;

    void OnTriggerStay(Collider other) {
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body != null) {
            body.transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
