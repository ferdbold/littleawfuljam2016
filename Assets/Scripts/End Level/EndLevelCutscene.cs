using UnityEngine;
using System.Collections;

public class EndLevelCutscene : MonoBehaviour {

    [SerializeField]
    private float animTime;

    public void StartAnim() {
        StartCoroutine(AnimTimer());
    }

    IEnumerator AnimTimer() {
        yield return new WaitForSeconds(animTime);
        LevelManager.instance.SwitchState(LevelManager.GameState.end);
    }
}
