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

			if (_cursor.transform.position.x > 1) {
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
}
