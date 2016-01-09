using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour {

    public GameObject player;
    [SerializeField] private Vector3 offsetPosition = new Vector3(0f,7f,-6.5f);
    [SerializeField] private float lerpValue = 0.1f;

    private Vector3 _targetPosition;

    void Start() {
        _targetPosition = transform.position;
        offsetPosition = transform.position - player.transform.position;
    }

	void Update () {
        FollowPlayer();
        OrientCamera();
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

    /// <summary> orients Camera smoothly </summary>
    private void OrientCamera() {
        Vector3 pos = player.transform.position - transform.position;
        Quaternion newRot = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 0.1f);
    }
}
