using UnityEngine;
using System.Collections;

public class Door : Interactable {

	private BoxCollider _meshCollider;
	private Animator _animator;

	[Header("Door Prompts")]
	[SerializeField] private string _openPrompt = "Open";
	[SerializeField] private string _closePrompt = "Close";

	private bool _opened = false;

	public new void Awake() {
		base.Awake();
		Transform mesh = transform.Find("Mesh");

		_meshCollider = mesh.GetComponent<BoxCollider>();
		_animator = mesh.GetComponent<Animator>();
	}

	public override void Activate() {
		_opened = !_opened;
		_meshCollider.enabled = !_opened;

		_prompt = (_opened) ? _closePrompt : _openPrompt;
		_animator.SetBool("Opened", _opened);
	}
}
