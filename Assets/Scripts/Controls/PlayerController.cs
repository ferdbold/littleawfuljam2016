using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private SlothController slothController;

    /// <summary>  Time for input to be hold to be considered "holding"  </summary>
    [SerializeField]
    private float HOLDTIMEFORROTATION = 0.15f;

    private float holdTimeLeft = 0;
    private float holdTimeRight = 0;
    private bool leftIsDown = false;
    private bool rightIsDown = false;


    // Use this for initialization
    void Start () {
        slothController = GameObject.FindGameObjectWithTag("SlothNinja").GetComponent<SlothController>();
    }
	
	// Update is called once per frame
	void Update () {
        //Input Left Key
        if (!leftIsDown && (Input.GetAxis("Left") >= 1)) { //Pressed
            
            leftIsDown = true;
            DisactivatePreviousInputs();
            if (holdTimeLeft > 0) {
                slothController.MoveLeft(holdTimeLeft);
            }
            StartCoroutine("CheckLeftPressed");

        } else if(leftIsDown && (Input.GetAxis("Left") < 1)) { //Released
            leftIsDown = false;
            StopCoroutine("CheckLeftPressed");
            slothController.ToggleTurnLeft(false);
            if (holdTimeLeft > 0) {
                holdTimeLeft = -1;
                slothController.MoveLeft(holdTimeLeft);
            }
        }

        //Input Right Key
        if (!rightIsDown && (Input.GetAxis("Right") >= 1)) { //Pressed
            rightIsDown = true;
            DisactivatePreviousInputs();
            if (holdTimeRight > 0) {
                slothController.MoveRight(holdTimeRight);
            }
            StartCoroutine("CheckRightPressed");

        } else if(rightIsDown && (Input.GetAxis("Right") < 1)) { //Released
            rightIsDown = false;
            StopCoroutine("CheckRightPressed");
            slothController.ToggleTurnRight(false);
            if (holdTimeRight > 0) {
                holdTimeRight = -1;
                slothController.MoveRight(holdTimeRight);
            }
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
        holdTimeLeft = -1;
    }

    IEnumerator CheckRightPressed() {
        holdTimeRight = 0;
        while (holdTimeRight < HOLDTIMEFORROTATION) {
            holdTimeRight += Time.deltaTime;          
            yield return null;
        }
        slothController.ToggleTurnRight(true);
        holdTimeRight = -1;
    }
}
