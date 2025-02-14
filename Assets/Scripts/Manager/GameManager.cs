﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Singleton

    public static GameManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentPlayLevel = 0;
			//OnStart_Level(currentLevel);
			gameObject.AddComponent<EscapeScript>();
            if (songManager == null) songManager = GetComponentInChildren<SongManager>();
            if (currentLevel == GameLevel.playLevel) gameObject.AddComponent<LevelManager>();
            currentPlayLevel = LevelID(SceneManager.GetActiveScene().name);
			songManager.PlaySong(SongManager.Song.MoveMode);
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }

	void OnLevelWasLoaded() {
		if (currentLevel == GameLevel.playLevel) {
			gameObject.AddComponent<LevelManager>();
		}
	}

    #endregion

    #region Reference

    public SongManager songManager { get; private set; }

	#endregion

	#region Switch Level

	public enum GameLevel { menu, playLevel }
    public GameLevel currentLevel;
    public int currentPlayLevel { get; private set; }

    const string MENU_LEVEL = "Menu";
    const string LOADING_SCREEN = "LoadingScreen";

    public void SwitchLevel(GameLevel level) {
        OnEnd_Level(currentLevel);
        currentLevel = level;
        OnStart_Level(currentLevel);
    }

    private void OnStart_Level(GameLevel level) {
        switch (level) {
            case GameLevel.menu:
                currentPlayLevel = 0;
                SceneManager.LoadScene(MENU_LEVEL);
                break;

            case GameLevel.playLevel:
                if (!GoToNextPlayLevel()) {
                    currentPlayLevel = 0;
                    GoToNextPlayLevel();
                }
                break;
        }
    }

    private void OnEnd_Level(GameLevel level) {
   
        switch (level) {
            case GameLevel.menu:

                break;

            case GameLevel.playLevel: 
                break;
        }
    }

    public void RestartCurrentPlayLevel() {
        SceneManager.LoadScene(LevelName(currentPlayLevel));
    }

    //Returns False if the game is at the last playlevel
    public bool GoToNextPlayLevel() {
        currentPlayLevel++;
        string nextLevelName = LevelName(currentPlayLevel);
        if (nextLevelName != "") {
            SceneManager.LoadScene(LOADING_SCREEN);
            SceneManager.LoadSceneAsync(nextLevelName);
            return true;
        }
        else {
            SceneManager.LoadSceneAsync(MENU_LEVEL);
            return false;
        }
    }

    private string LevelName(int levelID) {
        switch (levelID) {
            case 1: return "Blockin_lvl_1";
			case 2: return "Blocking_LVL_3";
			case 3: return "Blocking_lvl_2";
            default: return "";
        }
    }

    private int LevelID(string name) {
        switch (name) {
            case "Blockin_lvl_1": return 1;
			case "Blocking_LVL_3": return 2;
			case "Blocking_lvl_2": return 3;
			default: return 0;
        }
    }

	public void Play() {
		SwitchLevel(GameLevel.playLevel);
	}

	public void Quit() {
		Application.Quit();
	}

    #endregion
}
