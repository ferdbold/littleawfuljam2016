using UnityEngine;
using System.Collections;

/// <summary>
/// Turns ragdoll on and off on the sloth
/// </summary>
[RequireComponent(typeof(Animator))]
public class RagdollSloth : MonoBehaviour {

	private bool _ragdolled = false;

	private Animator _animator;
	[SerializeField] private Transform _ragdollRoot;

	public void Awake() {
		_animator = GetComponent<Animator>();
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

			foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>()) {
				rigidbody.isKinematic = !value;
			}

			_animator.enabled = !value;

			if (!_ragdolled) {
				// Move whole gameObject to same world position as ragdoll root and remove drift
				transform.position = _ragdollRoot.position;
				_ragdollRoot.localPosition = Vector3.zero;

				// Restart animator properly
				SendMessage("Start", SendMessageOptions.DontRequireReceiver);	
			}
		}
	}
}
