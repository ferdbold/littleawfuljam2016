using UnityEngine;
using System.Collections;

/// <summary>
/// Turns ragdoll on and off on a humanoid character
/// </summary>
public class RagdollToggle : MonoBehaviour {

	private bool _ragdolled = false;

	/// <summary>
	/// The animator to turn off during ragdoll mode
	/// </summary>
	[SerializeField] private Animator _characterAnimator;

	/// <summary>
	/// The root of the rig to ragdoll (pelvis bone)
	/// </summary>
	[SerializeField] private Transform _ragdollRoot;

	private BoxCollider _characterCollider;

	public void Awake() {
		_characterCollider = GetComponent<BoxCollider>();
		ragdolled = false;
	}

	public void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			ragdolled = !ragdolled;
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

			if (!_ragdolled) {
				// Move whole gameObject to same world position as ragdoll root to remove drift
				transform.position = new Vector3(_ragdollRoot.position.x, 0f, _ragdollRoot.position.z);
				transform.rotation = Quaternion.identity;
				_ragdollRoot.localPosition = Vector3.zero;
				_ragdollRoot.rotation = Quaternion.identity;

				// Restart animator properly
				SendMessage("Start", SendMessageOptions.DontRequireReceiver);	
			}
		}
	}
}
