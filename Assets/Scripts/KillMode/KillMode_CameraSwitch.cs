using UnityEngine;
using System.Collections;
using DG.Tweening;

public class KillMode_CameraSwitch : MonoBehaviour {

    public delegate void KillModeCallback();

    [SerializeField]
    private float cameraSpeed = 30f;

    [SerializeField]
    private float distanceFromTarget = 2f;

    //The target is the one that is being killed
    public void StartCameraAnim(KillModeCallback callback, Vector3 targetPosition, Vector3 frontVector) {
        StartCoroutine(CameraAnim(callback, targetPosition, targetPosition + frontVector * distanceFromTarget, 5f));
    }

    private IEnumerator CameraAnim(KillModeCallback callback, Vector3 targetPos, Vector3 position, float time) {
        transform.DOLookAt(targetPos, 1.5f);
        yield return new WaitForSeconds(1.5f);
        transform.DOMove(position, time);

        while (transform.position != position) {
            transform.DOLookAt(targetPos, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        callback();
    }
}
