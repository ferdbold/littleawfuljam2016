using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SlothController : MonoBehaviour {

    //Variables
    [Header("Forces")]
    [SerializeField]
    public float FORWARDFORCE = 5f; //Force used to move sloth forward when pushed
    [SerializeField]
    public float SIDEFORCE = 3f; //Force used to move sloth sideways when pushed
    [SerializeField]
    public float ROTATIONFORCE = 2.5f; //Force used to rotate sloth when pushed
    [SerializeField]
    public float TURNRATE = 25f; //Turn Rate when rotating in degrees per second (not pushed)
    public LayerMask wallLayerMask;

    [Header("Controls")]
    [SerializeField]
    public float MOVECOOLDOWN = 2f;
    [SerializeField]
    public float ANIMTIMEBEFOREMOVE = 0.5f;

    //Components
    [Header("Components")]
    public SphereCollider rightHandCollider;
    public SphereCollider leftHandCollider;
    private Rigidbody _rigidBody;


    //States Variables
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private bool _hasHitWallLeft = false; //have we hit a wall with left hand
    private bool _hasHitWallRight = false; //have we hit a wall with right hand
    private bool _isRotatingLeft = false;
    private bool _isRotatingRight = false;
    private float currentHoldTime = 0;
   

	void Awake () {
        _rigidBody = GetComponent<Rigidbody>();
        gameObject.tag = "SlothNinja";
    }
	
	// Update is called once per frame
	void Update () {
        if (_isRotatingLeft) {
            Debug.Log("RotatingLeft!");
            transform.Rotate(0, -TURNRATE * Time.deltaTime, 0);
        }
        if (_isRotatingRight) {
            Debug.Log("RotatingRight!");
            transform.Rotate(0, TURNRATE * Time.deltaTime, 0);
        }
    }

    /// <summary>
    /// Check controls
    /// </summary>
    private void CheckControls() {
        //Left
        if (Input.GetKeyDown(KeyCode.A) && _canMoveLeft) {
            StartCoroutine(StartCooldownMoveLeft());
            StartCoroutine(MoveSloth(false,0f));
        } else if(Input.GetKeyUp(KeyCode.A)) {
        }
        //Right
        if (Input.GetKeyDown(KeyCode.D) && _canMoveRight) {
            StartCoroutine(StartCooldownMoveRight());
            StartCoroutine(MoveSloth(true,0f));
        } else if (Input.GetKeyUp(KeyCode.D)) {
        }
    }

    /// <summary>
    /// Move Left Input was pressed
    /// </summary>
    public void MoveLeft(float holdTime) {
        if (_canMoveLeft) {
            StartCoroutine(StartCooldownMoveLeft());
            StartCoroutine(MoveSloth(false, holdTime));
        }
    }

    /// <summary>
    /// Move Right Input was pressed
    /// </summary>
    public void MoveRight(float holdTime) {
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
        Debug.Log("Has hit wall left");
        _hasHitWallLeft = true;
    }
    /// <summary> Called when RightHand Collider hits a wall </summary>
    public void OnCollisionRightHand() {
        Debug.Log("Has hit wall right");
        _hasHitWallRight = true;
    }


    private void PushSloth(Vector3 force) {
        _rigidBody.AddRelativeForce(force, ForceMode.Impulse);
    }
    
    private void RotateSloth(Vector3 rotation) {
        _rigidBody.AddTorque(rotation,ForceMode.Impulse);
    }



    /// <summary>
    /// Initiate a sloth movement
    /// </summary>
    /// <param name="isRight"> Are we moving right or left </param>
    /// <returns></returns>
    IEnumerator MoveSloth(bool isRight, float holdTime) {
        yield return new WaitForSeconds(ANIMTIMEBEFOREMOVE - holdTime);
       
        if (isRight) { 
            if(!_hasHitWallRight) PushSloth(new Vector3(SIDEFORCE, 0, FORWARDFORCE)); //Push only if wall wasnt hit
            RotateSloth(new Vector3(0, ROTATIONFORCE, 0));
        } else {
            if (!_hasHitWallLeft) PushSloth(new Vector3(-SIDEFORCE, 0, FORWARDFORCE));
            RotateSloth(new Vector3(0, -ROTATIONFORCE, 0));
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
