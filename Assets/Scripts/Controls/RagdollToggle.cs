using UnityEngine;
using System.Collections;

/// <summary>
/// Turns ragdoll on and off on a humanoid character
/// </summary>
public class RagdollToggle : MonoBehaviour {

	private bool _ragdolled = false;
	private bool _chillIsHoldedDown = false;

	//Cooldown
	private float RAGDOLLCOOLDOWN = 2f;
	private float _curRagdollCooldown = 0f;
	private bool _ragdollOnCD = false;

	[Tooltip("The animator to turn off during ragdoll mode")]
	[SerializeField] private Animator _characterAnimator;

	[Tooltip("The root of the rig to ragdoll (pelvis bone)")]
	[SerializeField] private Transform _ragdollRoot;

	private BoxCollider _characterCollider;
	private Rigidbody _characterRigidbody;
    private Snapper _slothSnapper;

	public void Awake() {
		_characterCollider = GetComponent<BoxCollider>();
		_characterRigidbody = GetComponent<Rigidbody>();
        _slothSnapper = GetComponent<Snapper>();
		ragdolled = false;
	}

	public void Update () {
		if (_curRagdollCooldown > 0) _curRagdollCooldown -= Time.deltaTime;
		else if (_curRagdollCooldown <= 0) _ragdollOnCD = false;


		if (Input.GetAxis("Chill") > 0 && !_slothSnapper.IsGripped && !_chillIsHoldedDown && !_ragdollOnCD) {
			ragdolled = !ragdolled;
			_chillIsHoldedDown = true;
			if (!ragdolled)
			{
				_ragdollOnCD = true;
				_curRagdollCooldown = RAGDOLLCOOLDOWN;
			}
		} 
		if ((Input.GetAxis("Chill") <= 0)) {
			_chillIsHoldedDown = false;
		}
	}

	public bool ragdolled {
		get { return _ragdolled; }
		set {
			_ragdolled = value;

			// Turn on ragdoll colliders OR global character collider
			foreach (Collider collider in _ragdollRoot.GetComponentsInChildren<Collider>()) {
				collider.enabled = value;
			}
			_characterCollider.enabled = !value;

			// Turn off kinematics while ragdolling
			foreach (Rigidbody rigidbody in _ragdollRoot.GetComponentsInChildren<Rigidbody>()) {
				rigidbody.isKinematic = !value;
			}

			// Disable character animations while ragdolling
			_characterAnimator.enabled = !value;

			// Disable global prefab gravity while ragdolling
			_characterRigidbody.useGravity = !value;

			if (!_ragdolled) {
				// Move whole gameObject to same world position as ragdoll root to remove drift
				transform.position = new Vector3(_ragdollRoot.position.x, _ragdollRoot.position.y, _ragdollRoot.position.z);
				transform.rotation = Quaternion.identity;
				_ragdollRoot.localPosition = Vector3.zero;
				_ragdollRoot.rotation = Quaternion.identity;

				// Restart animator properly
				SendMessage("Start", SendMessageOptions.DontRequireReceiver);	
			}
		}
	}
}
