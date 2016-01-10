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
            Converable body = other.GetComponent<Converable>();
            if (body != null && !body.locked) {
                body.transform.position += transform.forward * speed * Time.deltaTime;
                body.locked = true;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (isOn) {
            Converable body = other.GetComponent<Converable>();
            if (body != null) {
                body.locked = false;
            }
        }
    }
}
