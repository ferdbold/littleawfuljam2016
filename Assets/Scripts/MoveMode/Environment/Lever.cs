using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Lever : Interactable {

    [Header("Door Prompts")]
    [SerializeField]
    private string _openPrompt = "On";
    [SerializeField]
    private string _closePrompt = "Off";

    [Header("Events")]
    [SerializeField]
    private UnityEvent onEvent;
    [SerializeField]
    private UnityEvent offEvent;

    private Animator anim;

    [Header("Lever Attributes")]
    [SerializeField]
    private bool activated = false;

    void Awake() {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        _prompt = activated ? _closePrompt : _openPrompt;
        (activated ? onEvent : offEvent).Invoke();
        anim.SetBool("isOpen", activated);
    }

    public override void Activate() {
        activated = !activated;
        _prompt = activated ? _closePrompt : _openPrompt;
        (activated ? onEvent : offEvent).Invoke();
        anim.SetBool("isOpen", activated);
    }
}
