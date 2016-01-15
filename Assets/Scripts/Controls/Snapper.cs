using UnityEngine;

/// <summary>
/// Allows a ragdoll rig to snap and unsnap to a rigidbody
/// </summary>
[RequireComponent(typeof(RagdollToggle))]
public class Snapper : MonoBehaviour {
    [Tooltip("The snapping ragdoll root bone")]
    [SerializeField] private Transform _ragdollRoot;

    [SerializeField] private Rigidbody _leftHand;
    [SerializeField] private Rigidbody _rightHand;

    public bool IsGripped { get; private set; }

	private RagdollToggle _ragdoller;


	public void Awake() {
		_ragdoller = GetComponent<RagdollToggle>();
	}

	/// <summary>
	/// Grip a rigidbody with a hand
	/// </summary>
	/// <param name="target">Target rigidbody</param>
	/// <param name="hand">Which hand to grip with, "left" or "right"</param>
	public void Grip(Joint target, string hand) {
		_ragdoller.ragdolled = true;
        IsGripped = true;


        if (hand == "left") {
			target.connectedBody = this._leftHand;
		} else {
			target.connectedBody = this._rightHand;
		}
	}

	/// <summary>
	/// Release a hand
	/// </summary>
	/// <param name="hand">Which hand to release, "left" or "right"</param>
	public void Release() {
		_ragdoller.ragdolled = false;
        IsGripped = false;

    }

	public Rigidbody leftHand { get { return _leftHand; } }
	public Rigidbody rightHand { get { return _rightHand; } }
}
