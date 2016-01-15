using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SlothController : MonoBehaviour {

    //Variables
    [Header("Forces")]
    [SerializeField] private float FORWARDFORCE = 5f; //Force used to move sloth forward when pushed
    [SerializeField] private float SIDEFORCE = 3f; //Force used to move sloth sideways when pushed
    [SerializeField] private float ROTATIONFORCE = 2.5f; //Force used to rotate sloth when pushed in degrees per second
    [SerializeField] private float PULLFORCETIME = 0.8f;
    [SerializeField] private AnimationCurve rotationAnimationCurve; //Curve which rotation follows
    [SerializeField] private float rotationAnimationTime = 1f;
    [SerializeField] private float TURNRATE = 25f; //Turn Rate when rotating in degrees per second (not pushed)
    public LayerMask wallLayerMask;

    [Header("Controls")]
    [SerializeField] private float MOVECOOLDOWN = 2f;
    private float ANIMTIMEBEFOREMOVE = 0.5f;

    //Components
    [Header("Components")]
    public SphereCollider rightHandCollider;
    public SphereCollider leftHandCollider;
    private Rigidbody _rigidBody;
    private Animator _animator;
	private RagdollToggle _ragdollToggle;

    [Header("Animations")]
    public AnimationClip ExtendArmClip;


    //States Variables
    private bool _controlsDisabled = false;
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private bool _hasHitWallLeft = false; //have we hit a wall with left hand
    private bool _hasHitWallRight = false; //have we hit a wall with right hand
    private bool _isRotatingLeft = false;
    private bool _isRotatingRight = false;
    private float currentHoldTime = 0;
   

	void Awake () {
        //Get components
        _rigidBody = GetComponent<Rigidbody>();
        _animator = gameObject.GetComponentInChildren<Animator>();
		_ragdollToggle = GetComponent<RagdollToggle>();
		//Set default values
		gameObject.tag = "SlothNinja";
        ANIMTIMEBEFOREMOVE = ExtendArmClip.length;
    }


    void Start() {
        //Dont put anything here since can be called for ragdoll purposes
    }
	
	// Update is called once per frame
	void Update () {
        if (_controlsDisabled || _ragdollToggle.ragdolled) return;
        //Rotate Left
        if (_isRotatingLeft && _canMoveLeft) {
            _animator.SetBool("TurnLeft", true); //Set Animation value
            transform.Rotate(0, -TURNRATE * Time.deltaTime, 0);
        } else {
            _animator.SetBool("TurnLeft", false); //Set Animation value
        }
        //Rotate Right
        if (_isRotatingRight && _canMoveRight) {
            _animator.SetBool("TurnRight", true); //Set Animation value
            transform.Rotate(0, TURNRATE * Time.deltaTime, 0);
        } else {
            _animator.SetBool("TurnRight", false); //Set Animation value
        }
    }

    public void ActivateControls() {
        _controlsDisabled = false;
        _rigidBody.isKinematic = false;
        _animator.SetBool("Kill", false);
    }

    public void DeactivateControls() {
        _controlsDisabled = true;
        _rigidBody.isKinematic = true;
        _animator.SetBool("TurnLeft", false); //Set Animation value
        _animator.SetBool("TurnRight", false); //Set Animation value
        _animator.SetBool("MoveLeft", false); //Set Animation value
        _animator.SetBool("MoveRight", false); //Set Animation value
        _animator.SetBool("Kill", true); 
    }

	public bool AreControlsEnabled() {
		if (_controlsDisabled == true) return false;
		else return true;
	}

    /// <summary>
    /// Move Left Input was pressed
    /// </summary>
    public void MoveLeft(float holdTime) {
        if (_controlsDisabled || _ragdollToggle.ragdolled) return;
        if (_canMoveLeft) {
            StartCoroutine(StartCooldownMoveLeft());
            StartCoroutine(MoveSloth(false, holdTime));
        }
    }

    /// <summary>
    /// Move Right Input was pressed
    /// </summary>
    public void MoveRight(float holdTime) {
        if (_controlsDisabled || _ragdollToggle.ragdolled) return;
        if (_canMoveRight) {
            StartCoroutine(StartCooldownMoveRight());
            StartCoroutine(MoveSloth(true, holdTime));
        }
    }

    /// <summary>
    /// Input for Turn Left was modified
    /// </summary>
    /// <param name="isTurning"></param>
    public void ToggleTurnLeft(bool isTurning) {
        _isRotatingLeft = isTurning;
    }

    /// <summary>
    /// Input for Turn Left was modified
    /// </summary>
    /// <param name="isTurning"></param>
    public void ToggleTurnRight(bool isTurning) {
        _isRotatingRight = isTurning;
    }


    /// <summary> Called when LeftHand Collider hits a wall </summary>
    public void OnCollisionLeftHand() {
        //Debug.Log("Has hit wall left");
        _hasHitWallLeft = true;
    }
    /// <summary> Called when RightHand Collider hits a wall </summary>
    public void OnCollisionRightHand() {
        //Debug.Log("Has hit wall right");
        _hasHitWallRight = true;
    }


    IEnumerator PushSloth(Vector3 force) {
        yield return null;
        for (float i = 0f; i < 1f; i += Time.deltaTime / PULLFORCETIME) {
			if (_controlsDisabled || _ragdollToggle.ragdolled) break;
            _rigidBody.AddForce(force*Time.deltaTime, ForceMode.Acceleration);
            yield return null;
        }
        //_rigidBody.AddRelativeForce(force, ForceMode.Impulse);
    }
    
    /// <summary>
    /// Rotate the transform based on given rotation, time and curve
    /// </summary>
    /// <param name="rotation">Rotation to do in degrees</param>
    /// <param name="animTime">Time of the animation</param>
    /// <param name="animCurve">Curve at which to execute rotation</param>
    /// <returns></returns>
    IEnumerator RotateSloth(Vector3 rotation, float animTime, AnimationCurve animCurve) {
        //_rigidBody.AddTorque(rotation,ForceMode.Impulse);
        float prevI = 0;
        for (float i = 0; i < 1f; i += Time.deltaTime / animTime) {
            float curI = animCurve.Evaluate(i);
            //Get i difference
            float Idiff = curI - prevI;
            //rotate sloth accordingly
            transform.Rotate(Idiff*rotation);
            //Debug.Log("idiff : " + Idiff + "   final rotate : " + Idiff * rotation);
            yield return null;
            prevI = curI;
        }
    }



    /// <summary>
    /// Initiate a sloth movement
    /// </summary>
    /// <param name="isRight"> Are we moving right or left </param>
    /// <returns></returns>
    IEnumerator MoveSloth(bool isRight, float holdTime) {
        //Play Arm extend animation
        Vector3 relativeForce;
        if (isRight) {
            _animator.SetBool("MoveRight", true);
            StartCoroutine(RotateSloth(new Vector3(0, ROTATIONFORCE, 0), rotationAnimationTime * 2f, rotationAnimationCurve));
            relativeForce = _rigidBody.transform.rotation * new Vector3(SIDEFORCE, 0, FORWARDFORCE);
        } else {
            _animator.SetBool("MoveLeft", true);
            StartCoroutine(RotateSloth(new Vector3(0, -ROTATIONFORCE, 0), rotationAnimationTime * 2f, rotationAnimationCurve));
            relativeForce = _rigidBody.transform.rotation * new Vector3(-SIDEFORCE, 0, FORWARDFORCE);
        }
        yield return new WaitForSeconds(ANIMTIMEBEFOREMOVE);
        if (isRight) _animator.SetBool("MoveRight", false);
        else _animator.SetBool("MoveLeft", false);

        //Apply force
        if (isRight) { 
            if(!_hasHitWallRight) StartCoroutine(PushSloth(relativeForce)); //Push only if wall wasnt hit
            
        } else {
            if (!_hasHitWallLeft) StartCoroutine(PushSloth(relativeForce));

        }
    }

    /// <summary> Left Move Cooldown </summary>
    /// <returns></returns>
    IEnumerator StartCooldownMoveLeft() {
        int amtCollidingChecks = 5;
        _hasHitWallLeft = false;
        _canMoveLeft = false;

        //Check multiple time during animations if we're colliding with a wall 
        for (int i = 0; i < amtCollidingChecks; i++) {
            yield return new WaitForSeconds(MOVECOOLDOWN / amtCollidingChecks);
            Collider[] hitWalls = Physics.OverlapSphere(leftHandCollider.transform.position, leftHandCollider.radius, wallLayerMask);
            if (hitWalls.Length > 0) OnCollisionLeftHand();
        }

        _canMoveLeft = true;
        _hasHitWallLeft = false;
    }

    /// <summary> Right Move Cooldown </summary>
    /// <returns></returns>
    IEnumerator StartCooldownMoveRight() {
        int amtCollidingChecks = 5;
        _hasHitWallRight = false;
        _canMoveRight = false;

        //Check multiple time during animations if we're colliding with a wall 
        for(int i = 0; i < amtCollidingChecks; i++) {
            yield return new WaitForSeconds(MOVECOOLDOWN/ amtCollidingChecks);
            Collider[] hitWalls = Physics.OverlapSphere(rightHandCollider.transform.position, rightHandCollider.radius, wallLayerMask);
            if (hitWalls.Length > 0) OnCollisionRightHand();
        }
        
        _canMoveRight = true;
        _hasHitWallRight = false;
    }



}
