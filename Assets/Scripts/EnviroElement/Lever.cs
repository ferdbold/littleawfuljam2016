using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Lever : Interactable {

    [SerializeField]
    private UnityEvent relatedEvent;

    private bool activated = false;

    public override void Activate() {
        if (!activated) {
            relatedEvent.Invoke();
            //TODO Start Anim
        }
    }
}
