using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SlothController : MonoBehaviour {

    //Variables
    [Header("Forces")]
    public float forwardForce = 5f;
    public float sideForce = 3f;
    public float rotationForce = 2.5f;
    public LayerMask wallLayerMask;

    [Header("Controls")]
    public float moveCooldown = 2f;
    public float animTimeBeforeMove = 0.5f;

    //Components
    [Header("Components")]
    public SphereCollider rightHandCollider;
    public SphereCollider leftHandCollider;
    private Rigidbody _rigidBody;


    //Variables
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private bool _hasHitWallLeft = false; //have we hit a wall with left hand
    private bool _hasHitWallRight = false; //have we hit a wall with right hand
   

	void Awake () {
        _rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckControls(); //Check player inputs
    }

    /// <summary>
    /// Check controls
    /// </summary>
    private void CheckControls() {
        //Left
        if (Input.GetKeyDown(KeyCode.A) && _canMoveLeft) {
            StartCoroutine(StartCooldownMoveLeft());
            StartCoroutine(MoveSloth(false));
        }
        //Right
        if (Input.GetKeyDown(KeyCode.D) && _canMoveRight) {
            StartCoroutine(StartCooldownMoveRight());
            StartCoroutine(MoveSloth(true));
        }
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
    IEnumerator MoveSloth(bool isRight) {
        yield return new WaitForSeconds(animTimeBeforeMove);
       
        if (isRight) { 
            if(!_hasHitWallRight) PushSloth(new Vector3(sideForce, 0, forwardForce)); //Push only if wall wasnt hit
            RotateSloth(new Vector3(0, rotationForce, 0));
        } else {
            if (!_hasHitWallLeft) PushSloth(new Vector3(-sideForce, 0, forwardForce));
            RotateSloth(new Vector3(0, -rotationForce, 0));
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
            yield return new WaitForSeconds(moveCooldown / amtCollidingChecks);
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
            yield return new WaitForSeconds(moveCooldown/ amtCollidingChecks);
            Collider[] hitWalls = Physics.OverlapSphere(rightHandCollider.transform.position, rightHandCollider.radius, wallLayerMask);
            if (hitWalls.Length > 0) OnCollisionRightHand();
        }
        
        _canMoveRight = true;
        _hasHitWallRight = false;
    }
}
