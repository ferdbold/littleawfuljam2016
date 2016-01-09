using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Cutscene : MonoBehaviour {

    [System.Serializable]
    struct CutsceneEvent {
        public float timeFrame;
        public UnityEvent objectEvent;
    }
    
    [SerializeField]
    private float animTime;

    [SerializeField]
    private Vector3 destination;

    [SerializeField]
    private List<CutsceneEvent> events = new List<CutsceneEvent>();

    public void StartAnim() {
        StartCoroutine(AnimTimer());
    }

    IEnumerator AnimTimer() {
        //TODO path
        for (float time = 0; time < animTime; time += Time.deltaTime) {
            foreach (CutsceneEvent ev in events) {
                if (ev.timeFrame <= time) {
                    ev.objectEvent.Invoke();
                    events.Remove(ev);
                }
            }
            yield return new WaitForEndOfFrame();
        }
        LevelManager.instance.SwitchState(LevelManager.GameState.end);
    }
}
