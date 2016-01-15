using UnityEngine;

/// <summary>
/// Interactable rope the character can grip
/// </summary>
public class Rope : Interactable {

	[Tooltip("The rope speed (NOTE: This is relative to the length of the rope, so longer ropes will be faster)")]
	[SerializeField] private float _speed = 0.4f;

	[Header("Rope Prompts")]
	[SerializeField] private string _gripPrompt = "Grip";
	[SerializeField] private string _releasePrompt = "Release";

	private BoxCollider _path;
	private Joint _cursor;
	private Snapper _slothSnapper;

	private bool _activated = false;
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
		if (Input.GetButtonDown("GripLeft") && _focused) {
			Grip("left");
		} else if (Input.GetButtonDown("GripRight") && _focused) {
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
		Debug.Log("Grip");
		_cursor.transform.localPosition = new Vector3(CalculateCursorStartingPosition(), 0, 0);

		_activated = true;
		_prompt = _releasePrompt;

		_grippingButton = button;

		_slothSnapper.Grip(_cursor, button);
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

		// Exit early if the sloth is out of bounds
		if (slothPos.x < worldPathStartPos.x) {
			//return 0;
		} else if (slothPos.x > worldPathEndPos.x) {
		//	return 1;
		}

		// Find closest intersection point
		Ray worldPath = new Ray(worldPathStartPos, worldPathEndPos - worldPathStartPos);
		Vector3 worldIntersection = worldPath.origin + worldPath.direction * Vector3.Dot(worldPath.direction, slothPos - worldPath.origin);
		Vector3 localIntersection = worldIntersection - worldPathStartPos;

		// Calculate cursor offset X
		Debug.Log(localIntersection.x / localPathEndPos.x);
		return localIntersection.x / localPathEndPos.x;
	}
}
