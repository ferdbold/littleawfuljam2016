using UnityEngine;
using System.Collections;

public class InGameMode : MonoBehaviour {

    public static InGameMode instance;

    void Start() {
        if (instance == null) {
            instance = this;
            currentState = GameState.playMode;
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

    public enum GameState { playMode, killMode }
    public GameState currentState { get; private set; }

    public void Activate_KillMode() {
        SwitchState(GameState.killMode);
    }

    public void Activate_PlayMode() {
        SwitchState(GameState.playMode);
    }

    private void SwitchState(GameState state) {
        OnEnd_State(currentState);
        currentState = state;
        OnStart_State(currentState);
    }

    private void OnStart_State(GameState state) {
        switch (state) {
            case GameState.playMode:

                break;

            case GameState.killMode:

                break;
        }
    }

    private void OnEnd_State(GameState state) {
        switch (state) {
            case GameState.playMode:

                break;

            case GameState.killMode:

                break;
        }
    }
}
