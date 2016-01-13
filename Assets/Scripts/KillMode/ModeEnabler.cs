using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ModeEnabler : MonoBehaviour {

    public MiniGameController script;
    public Canvas killCamCanvas;
    public Animator animator;
    public Transform killModeCameraAnchor;

    //Cameras
    private Camera _camera;
    private TopDownCamera _topDownCamera;
    private LookAtCamera _lookAtCamera;

    void Awake() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _topDownCamera = _camera.GetComponent<TopDownCamera>();
        _lookAtCamera = _camera.GetComponent<LookAtCamera>();
    }

	void Start () {
        script.enabled = false;
        killCamCanvas.enabled = false;
        _lookAtCamera.enabled = false;
    }

    public void enableScript()
    {
        script.enabled = true;
        killCamCanvas.enabled = true;
        animator.SetBool("InKillMode", true);
        _topDownCamera.enabled = false;
        _lookAtCamera.enabled = true;
        StartCoroutine(CameraAnimOnDelay()); 
    }

    public void disableScript()
    {
        script.enabled = false;
        killCamCanvas.enabled = false;
        animator.SetBool("InKillMode", false);
        _topDownCamera.enabled = true;
        _lookAtCamera.enabled = false;
    }

    IEnumerator CameraAnimOnDelay() {
        yield return new WaitForSeconds(2.5f);
        _camera.transform.DOMove(killModeCameraAnchor.transform.position, 3.5f);
    }
}
