using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    void Start() {
        if (instance == null) {
            instance = this;
            currentState = GameState.moveMode;
            enemyTarget = GameObject.FindGameObjectWithTag("enemy-target");
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

    public enum GameState { moveMode, killMode }
    public GameState currentState { get; private set; }

    public void Activate_KillMode() {
        SwitchState(GameState.killMode);
    }

    public void Activate_PlayMode() {
        SwitchState(GameState.moveMode);
    }

    private void SwitchState(GameState state) {
        OnEnd_State(currentState);
        currentState = state;
        OnStart_State(currentState);
    }

    private void OnStart_State(GameState state) {
        switch (state) {
            case GameState.moveMode:

                break;

            case GameState.killMode:
                Camera.main.GetComponent<KillModeCameraSwitch>().StartCameraAnim(KillMode_StartKilling, enemyTarget.transform.position, enemyTarget.transform.forward);
                break;
        }
    }

    private void OnEnd_State(GameState state) {
        switch (state) {
            case GameState.moveMode:

                break;

            case GameState.killMode:

                break;
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    public GameObject enemyTarget { get; private set; }

    private void KillMode_StartKilling() {
        Debug.Log("Starting Kill Mode");
    }
}
