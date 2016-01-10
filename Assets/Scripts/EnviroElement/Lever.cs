using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Lever : Interactable {

    [SerializeField]
    private UnityEvent onEvent;

    [SerializeField]
    private UnityEvent offEvent;

    private Animator anim;

    private bool activated = false;

    void Awake() {
        anim = GetComponentInChildren<Animator>();
    }

    public override void Activate() {
        activated = !activated;
        (activated ? onEvent : offEvent).Invoke();
        anim.SetBool("isOpen", activated);
    }
}
