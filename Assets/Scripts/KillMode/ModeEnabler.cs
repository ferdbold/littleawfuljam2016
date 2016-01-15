using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ModeEnabler : MonoBehaviour {
    [Header("References")]
    public MiniGameController script;
    public Canvas killCamCanvas;
    public Animator animator;
    public Transform killModeCameraAnchor;

    [Header("Sloth animator")]
    public RuntimeAnimatorController _slothDefaultController;
    public RuntimeAnimatorController _slothIKController;
    public Transform LeftHandAnchor;
    public Transform RightHandAnchor;
    public AnimationClip ClimbAnimClip; //Used for anim time
    private Animator _slothAnimator;
    private IKSlothController _ikSlothController;

    //Cameras
    private Camera _camera;
    private TopDownCamera _topDownCamera;
    private LookAtCamera _lookAtCamera;

    void Awake() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _topDownCamera = _camera.GetComponent<TopDownCamera>();
        _lookAtCamera = _camera.GetComponent<LookAtCamera>();
        _slothAnimator = GameObject.FindGameObjectWithTag("SlothNinja").GetComponentInChildren<Animator>();
        _ikSlothController = _slothAnimator.GetComponent<IKSlothController>();
        killCamCanvas.worldCamera = _camera;
        killCamCanvas.planeDistance = 0.35f;
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
        //Camera
        _topDownCamera.enabled = false;
        _lookAtCamera.enabled = true;
        StartCoroutine(CameraAnimOnDelay());
        //Sloth Controller
        StartCoroutine(SlothIKControllerOnDelay());
    }

    public void disableScript()
    {
        script.enabled = false;
        killCamCanvas.enabled = false;
        animator.SetBool("InKillMode", false);
        //Camera
        _topDownCamera.enabled = true;
        _lookAtCamera.enabled = false;
        //Sloth Controller
        _slothAnimator.runtimeAnimatorController = _slothDefaultController;
        _ikSlothController.ikActive = false;
    }

    IEnumerator CameraAnimOnDelay() {
        yield return new WaitForSeconds(2.5f);
        _camera.transform.DOMove(killModeCameraAnchor.transform.position, 3.5f);
    }

    IEnumerator SlothIKControllerOnDelay() {
        yield return new WaitForSeconds(ClimbAnimClip.length);
        //_slothAnimator.runtimeAnimatorController = _slothIKController;
        _ikSlothController.leftHandObj = LeftHandAnchor;
        _ikSlothController.lookObjLeft = LeftHandAnchor;
        _ikSlothController.rightHandObj = RightHandAnchor;
        _ikSlothController.lookObjRight = RightHandAnchor;
        _ikSlothController.ikActive = true;
    }
}
