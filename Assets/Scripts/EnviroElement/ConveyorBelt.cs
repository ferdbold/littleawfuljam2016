using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool isOn;

    //if the conveyor belt is locking other
    private bool locking = false;

    public void TurnOn() { isOn = true; }
    public void TurnOff() { isOn = false; }

    void OnTriggerStay(Collider other) {
        if (!locking && isOn) {
            Converable body = other.GetComponent<Converable>();
            if (body != null) {
                body.locked = true;
                body.transform.position += transform.forward * speed * Time.deltaTime;
                locking = true;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (isOn) {
            Converable body = other.GetComponent<Converable>();
            if (locking && body != null) {
                body.locked = false;
                locking = false;
            }
        }
    }
}
