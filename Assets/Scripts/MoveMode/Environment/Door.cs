using UnityEngine;
using System.Collections;

public class Door : Interactable {

	private BoxCollider _meshCollider;
	private Animator _animator;

	[Header("Door Prompts")]
	[SerializeField] private string _openPrompt = "Open";
	[SerializeField] private string _closePrompt = "Close";

    [Header("Door Attributes")]
    [SerializeField] private bool _doorLocked = false; //If the door is locked, it can only be opened by a lever or computer

	private bool _opened = false;

	public new void Awake() {
		base.Awake();
		Transform mesh = transform.Find("Mesh");

		_meshCollider = mesh.GetComponent<BoxCollider>();
		_animator = mesh.GetComponent<Animator>();
	}

	public override void Activate() {
        if (!_doorLocked) {
            _opened = !_opened;
            _meshCollider.enabled = !_opened;

            _prompt = (_opened) ? _closePrompt : _openPrompt;
            _animator.SetBool("Open", _opened);
        }
	}

    public void OpenDoor() {
        ChangeDoorState(true);
    }

    public void CloseDoor() {
        ChangeDoorState(false);
    }

    private void ChangeDoorState(bool newState) {
        _opened = newState;
        _meshCollider.enabled = !newState;

        _prompt = (newState) ? _closePrompt : _openPrompt;
        _animator.SetBool("Open", newState);
    }
}
