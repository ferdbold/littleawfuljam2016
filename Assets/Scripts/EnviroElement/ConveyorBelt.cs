using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool isOn;

    public void TurnOn() { isOn = true; }
    public void TurnOff() { isOn = false; }

    void OnTriggerStay(Collider other) {
        if (isOn) {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null) {
                body.transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }
}
