using UnityEngine;

/// <summary>
/// Allows a ragdoll rig to snap and unsnap to a transform
/// </summary>
public class Snapper : MonoBehaviour {
	/// <summary>
	/// The snapping ragdoll root bone
	/// </summary>
	[SerializeField] private Transform _ragdollRoot;

	[SerializeField] private Rigidbody _leftHand;
	[SerializeField] private Rigidbody _rightHand;

	public Rigidbody leftHand { get { return _leftHand; } private set {} }
	public Rigidbody rightHand { get { return _rightHand; } private set {} }
}
