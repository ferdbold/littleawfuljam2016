using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour {

    public GameObject player;
    public Vector3 offsetPosition;
    public float lerpValue = 0.5f;

    private Vector3 _targetPosition;

    void Start() {
        _targetPosition = transform.position;
    }

	void Update () {
        FollowPlayer();
	}

    private void FollowPlayer() {
        GetPlayerPosition();
        RepositionCamera();
    }

    private void GetPlayerPosition() {
        _targetPosition = player.transform.position + offsetPosition;
    }

    private void RepositionCamera() {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, lerpValue);
    }
}
