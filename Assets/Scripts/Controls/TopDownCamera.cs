using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour {

    public GameObject player;
    public GameObject playerOverShoulder;
    private Vector3 _startOffsetPosition;
    private Vector3 _startShoulderOffsetPosition;
    private Vector3 _offsetPosition;
    [SerializeField] private float lerpValue = 0.1f;

    private Vector3 _targetPosition;
    public LerpCameraToPositionZone lerpCameraToPositionZone;

    void Start() {
        _targetPosition = transform.position;
        _startOffsetPosition = transform.position - player.transform.position;
        _startShoulderOffsetPosition = playerOverShoulder.transform.position - player.transform.position;
    }

	void Update () {
        //Calculate Changes
        Vector3 lookAtPos;
        if (lerpCameraToPositionZone == null) {
            _offsetPosition = _startOffsetPosition;
            lookAtPos = player.transform.position - transform.position;
        } else {
            float lerpAlpha = lerpCameraToPositionZone.GetLerpAlpha();
            _offsetPosition = Vector3.Lerp(_startOffsetPosition, _startShoulderOffsetPosition, lerpAlpha);
            lookAtPos = Vector3.Lerp(player.transform.position - transform.position, lerpCameraToPositionZone.targetTransform.position - transform.position, lerpAlpha);
        }

        //Apply Changes
        _targetPosition = player.transform.position + _offsetPosition;
        RepositionCamera();
        OrientCamera(lookAtPos);
    }

    private void RepositionCamera() {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, lerpValue);
    }

    /// <summary> orients Camera smoothly </summary>
    private void OrientCamera(Vector3 lookAtPos) {
        Quaternion newRot = Quaternion.LookRotation(lookAtPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 0.1f);
    }
}
