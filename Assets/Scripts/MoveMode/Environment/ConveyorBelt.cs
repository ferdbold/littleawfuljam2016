using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool isOn;

    public void TurnOn() {
		SetUsability(true);
	}
    public void TurnOff() {
		SetUsability(false);
	}

	void Start() {
		SetUsability(isOn);
	}

	private void SetUsability(bool state) {
		isOn = state;
		transform.GetChild(0).FindChild("Object004")
							 .GetComponent<MeshRenderer>()
							 .material
							 .SetFloat("_Speed", isOn ? 1f : 0f);
	}

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
