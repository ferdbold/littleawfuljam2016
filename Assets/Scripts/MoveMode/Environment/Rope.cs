using UnityEngine;
using System.Collections;

/// <summary>
/// Interactable rope the character can grip
/// </summary>
public class Rope : Interactable {

	[Tooltip("The rope speed (NOTE: This is relative to the length of the rope, so longer ropes will be faster)")]
	[SerializeField] private float _speed = 0.4f;
	[SerializeField] private float _grabCooldown = 0.5f; //Time until the player can grab again

	[Header("Rope Prompts")]
	[SerializeField] private string _gripPrompt = "Grip";
	[SerializeField] private string _releasePrompt = "Release";

	private BoxCollider _path;
	private Joint _cursor;
	private Snapper _slothSnapper;

	private bool _activated = false;
	private bool _canGrab = true;
	private string _grippingButton = string.Empty;
	
	public new void Awake() {
		base.Awake();

		_path = transform.Find("Path").GetComponent<BoxCollider>();
		_cursor = transform.Find("Path/Cursor").GetComponent<Joint>();

		_slothSnapper = GameObject.FindGameObjectWithTag("SlothNinja").GetComponent<Snapper>();
	}

	public new void Update() {
		base.Update();


		// Listen for inputs
		if (Input.GetButtonDown("GripLeft") && _focused && _canGrab) {
			Grip("left");
		} else if (Input.GetButtonDown("GripRight") && _focused && _canGrab) {
			Grip("right");
		} else if (Input.GetButtonUp("GripLeft") && _grippingButton == "left" && _activated) {
			Release();
		} else if (Input.GetButtonUp("GripRight") && _grippingButton == "right" && _activated) {
			Release();
		}

		// Move cursor forward
		if (_activated) {
			_cursor.transform.Translate(_speed * Time.deltaTime, 0, 0);

			if (_cursor.transform.localPosition.x > 1) {
				Release();
			}
		}
	}

	public override void Activate() {}

	/// <summary>
	/// Grip the rope
	/// </summary>
	/// <param name="button">"left" or "right"</param>
	public void Grip(string button) {
		_cursor.transform.localPosition = new Vector3(CalculateCursorStartingPosition(), 0, 0);

		_activated = true;
		_prompt = _releasePrompt;

		_grippingButton = button;

		_slothSnapper.Grip(_cursor, button);
		StartGrabCooldown();
	}

	/// <summary>
	/// Release the rope
	/// </summary>
	public void Release() {
		_activated = false;
		_prompt = _gripPrompt;

		_slothSnapper.Release();
		_cursor.connectedBody = null;
		_grippingButton = string.Empty;

		_cursor.transform.localPosition = Vector3.zero;
		StartGrabCooldown();
	}
		
	/// <summary>
	/// Calculates the cursor starting position upon a grip to be closest to the sloth
	/// </summary>
	/// <returns>The cursor starting X position, ranging from 0 to 1</returns>
	private float CalculateCursorStartingPosition() {
		Vector3 slothPos = _slothSnapper.transform.position;
		Vector3 worldPathStartPos = _path.transform.position;
		Vector3 worldPathEndPos = worldPathStartPos + Quaternion.Euler(_path.transform.rotation.eulerAngles) * new Vector3(_path.transform.localScale.x, 0, 0);
		Vector3 localPathEndPos = worldPathEndPos - worldPathStartPos;
		Vector3 projectedSlothPos = Vector3.Project(transform.rotation * slothPos - worldPathStartPos, localPathEndPos);

		Debug.Log("Projected sloth pos: " + projectedSlothPos);

		// Exit early if the sloth is before the rope
		// FIXME: The projection here is screwed up
		if (projectedSlothPos.x < 0) {
			return 0;
		}

		// Find closest intersection point
		Ray worldPath = new Ray(worldPathStartPos, worldPathEndPos - worldPathStartPos);
		Vector3 worldIntersection = worldPath.origin + worldPath.direction * Vector3.Dot(worldPath.direction, slothPos - worldPath.origin);
		Vector3 localIntersection = worldIntersection - worldPathStartPos;

		// Calculate cursor offset X
		return localIntersection.x / localPathEndPos.x;
	}

	/// <summary>
	/// Starts the grab cooldown coroutine
	/// </summary>
	private void StartGrabCooldown() {
		StopCoroutine("GrabCooldown");
		StartCoroutine("GrabCooldown");
	}

	IEnumerator GrabCooldown() {
		_canGrab = false;
		yield return new WaitForSeconds(_grabCooldown);
		_canGrab = true;
	}
}
