using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private SlothController slothController;

    /// <summary>  Time for input to be hold to be considered "holding"  </summary>
    [SerializeField]
    private float HOLDTIMEFORROTATION = 0.15f;

    private float holdTimeLeft = 0;
    private float holdTimeRight = 0;

    // Use this for initialization
    void Start () {
        slothController = GameObject.FindGameObjectWithTag("SlothNinja").GetComponent<SlothController>();
    }
	
	// Update is called once per frame
	void Update () {
        //Input Left Key
        if (Input.GetKeyDown(KeyCode.A)) { //Pressed
            DisactivatePreviousInputs();
            if (holdTimeLeft > 0) slothController.MoveLeft(holdTimeLeft);
            StartCoroutine("CheckLeftPressed");

        } else if(Input.GetKeyUp(KeyCode.A)) { //Released
            StopCoroutine("CheckLeftPressed");
            slothController.ToggleTurnLeft(false);
            if (holdTimeLeft > 0) slothController.MoveLeft(holdTimeLeft);
        }

        //Input Right Key
        if (Input.GetKeyDown(KeyCode.D)) { //Pressed
            DisactivatePreviousInputs();
            if (holdTimeRight > 0) slothController.MoveRight(holdTimeRight);
            StartCoroutine("CheckRightPressed");

        } else if(Input.GetKeyUp(KeyCode.D)) { //Released
            StopCoroutine("CheckRightPressed");
            slothController.ToggleTurnRight(false);
            if (holdTimeRight > 0) slothController.MoveRight(holdTimeRight);
        }

    }

    /// <summary> Deactivates coroutine and rotation when new input pressed </summary>
    private void DisactivatePreviousInputs() {
        StopCoroutine("CheckLeftPressed");
        StopCoroutine("CheckRightPressed");
        slothController.ToggleTurnLeft(false);
        slothController.ToggleTurnRight(false);
    }

    IEnumerator CheckLeftPressed() {
        holdTimeLeft = 0;
        while (holdTimeLeft < HOLDTIMEFORROTATION) {
            holdTimeLeft += Time.deltaTime;
            yield return null;
        }
        slothController.ToggleTurnLeft(true);
        holdTimeLeft = 0;
    }

    IEnumerator CheckRightPressed() {
        holdTimeRight = 0;
        while (holdTimeRight < HOLDTIMEFORROTATION) {
            holdTimeRight += Time.deltaTime;
            yield return null;
        }
        slothController.ToggleTurnRight(true);
        holdTimeRight = 0;
    }
}
