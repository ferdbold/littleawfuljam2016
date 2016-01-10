using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    private float _killMode_timer = 10f;
    private const float DISTANCE_TO_TARGET_NEEDED = 5f;

    void Start() {
        if (instance == null) {
            instance = this;
            currentState = GameState.moveMode;
            enemyTarget = GameObject.FindGameObjectWithTag("enemy-target");
            sloth = GameObject.FindGameObjectWithTag("SlothNinja");
            OnStart_State(currentState);
        }
        else {
            Destroy(this);
        }
    }

    void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------

    public enum GameState { moveMode, killMode, endLevelCutscene, end }
    public GameState currentState { get; private set; }

    public void SwitchState(GameState state) {
        OnEnd_State(currentState);
        currentState = state;
        OnStart_State(currentState);
    }

    private void OnStart_State(GameState state) {
        switch (state) {
            case GameState.moveMode:
                GameManager.instance.songManager.PlaySong(SongManager.Song.MoveMode);
                StartCoroutine(CheckDistanceToTarget());   
                break;

            case GameState.killMode:
                Camera.main.GetComponent<KillModeCameraSwitch>().StartCameraAnim(KillMode_StartKilling, enemyTarget.transform.position, enemyTarget.transform.forward);
                break;

            case GameState.endLevelCutscene:

                break;

            case GameState.end:
                GameManager.instance.GoToNextPlayLevel();
                break;
        }
    }

    private void OnEnd_State(GameState state) {
        switch (state) {
            case GameState.moveMode:

                break;

            case GameState.killMode:

                break;

            case GameState.endLevelCutscene:

                break;
        }
    }

    //------------------------------------------------------------------------------------------------------------------

    private IEnumerator CheckDistanceToTarget() {
        bool notFound = true;
        while (notFound) {
            if (Vector3.Distance(sloth.transform.position, enemyTarget.transform.position) >= DISTANCE_TO_TARGET_NEEDED) {
                notFound = true;
                SwitchState(GameState.killMode);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    //------------------------------------------------------------------------------------------------------------------

    public GameObject enemyTarget { get; private set; }
    public GameObject sloth { get; private set; }

    private void KillMode_StartKilling() {
        Debug.Log("Starting Kill Mode");
    }

    IEnumerator KillMode_Timer() {
        yield return new WaitForSeconds(_killMode_timer);
        KillMode_StopKilling();
    }

    private void KillMode_StopKilling() {
        SwitchState(GameState.endLevelCutscene);
    }

    //------------------------------------------------------------------------------------------------------------------
}
