using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    private const float DISTANCE_TO_TARGET_NEEDED = 1f;

    void Start() {
        if (instance == null) {
			sloth = GameObject.FindGameObjectWithTag("SlothNinja");
			instance = this;
			currentState = GameState.moveMode;
            enemyTarget = GameObject.FindGameObjectWithTag("enemy-target");
			_enemyKillTargetAnchor = GameObject.FindGameObjectWithTag("EnemyTargetAnchor").transform;

			_ragdollToggle = sloth.GetComponent<RagdollToggle>();
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
        Debug.Log("Current Level State: " + state);
        switch (state) {
            case GameState.moveMode:
                StartCoroutine(CheckDistanceToTarget());   
                break;

            case GameState.killMode:
                sloth.GetComponent<SlothController>().DeactivateControls();
                sloth.transform.DOMove(new Vector3(_enemyKillTargetAnchor.transform.position.x + 0,
													_enemyKillTargetAnchor.transform.position.y - 0,
													_enemyKillTargetAnchor.transform.position.z - 0.00f), 2f);
                sloth.transform.DORotate((_enemyKillTargetAnchor.transform.rotation.eulerAngles)+ new Vector3(0,0,0), 2f);
                KillMode_StartKilling();
                break;

            case GameState.endLevelCutscene:
                SwitchState(GameState.end); //TODO replace with cutscene
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
                enemyTarget.GetComponent<ModeEnabler>().disableScript();
                break;

            case GameState.endLevelCutscene:

                break;
        }
    }

    //------------------------------------------------------------------------------------------------------------------

    private IEnumerator CheckDistanceToTarget() {
        bool notFound = true;
        while (notFound) {
            if (!_ragdollToggle.ragdolled && Vector3.Distance(sloth.transform.position, enemyTarget.transform.position) <= DISTANCE_TO_TARGET_NEEDED) {
                notFound = false;
                SwitchState(GameState.killMode);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    //------------------------------------------------------------------------------------------------------------------

    public GameObject enemyTarget { get; private set; }
    public GameObject sloth { get; private set; }
	private RagdollToggle _ragdollToggle;
	private Transform _enemyKillTargetAnchor;

    void KillMode_StartKilling() {
        enemyTarget.GetComponent<ModeEnabler>().enableScript();
    }

    //------------------------------------------------------------------------------------------------------------------
}
