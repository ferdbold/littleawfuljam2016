using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Lever : Interactable {

    [SerializeField]
    private UnityEvent onEvent;

    [SerializeField]
    private UnityEvent offEvent;

    private bool activated = false;

    public override void Activate() {
        activated = !activated;
        (activated ? onEvent : offEvent).Invoke();
        //TODO Start Anim
    }
}
