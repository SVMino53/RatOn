using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions_scr : MonoBehaviour
{
    public enum Actions
    {
        DoNothing,
        QuitFromMenu,
        StartNewGame,
        LoadLevel,
        LoadGame,
        SaveGame,
        ContinueGameFromPause
    }

    static public void DoNothing()
    {
        return;
    }

    static public void QuitFromMenu()
    {
        Application.Quit();
    }

    static public void StartNewGame()
    {
        GameStatic_scr.level = 0;
        GameStatic_scr.score = 0;
        GameStatic_scr.money = 0;

        SaveGame();
    }

    static public void LoadLevel()
    {
        SceneManager.LoadScene(GameStatic_scr.level, LoadSceneMode.Single);
    }

    static public void LoadGame()
    {
        GameStatic_scr.Load();
    }

    static public void SaveGame()
    {
        GameStatic_scr.Save();
    }

    static public void ContinueGameFromPause(List<GameObject> activateObjs, List<GameObject> deactivateObjs, List<GameObject> enableObjs, List<GameObject> disableObjs, GameState_scr gameStateScr)
    {
        for (int i = 0; i < activateObjs.Count; i++)
        {
            activateObjs[i].SetActive(true);
        }

        for (int i = 0; i < deactivateObjs.Count; i++)
        {
            deactivateObjs[i].SetActive(false);
        }

        for (int i = 0; i < enableObjs.Count; i++)
        {
            enableObjs[i].GetComponent<MonoBehaviour>().enabled = true;
        }

        for (int i = 0; i < disableObjs.Count; i++)
        {
            disableObjs[i].GetComponent<MonoBehaviour>().enabled = false;
        }

        gameStateScr.curGameState = GameState_scr.GameState.GAME;
        gameStateScr.changeState = true;
    }


    static public void DoAction(Actions action)
    {
        switch (action)
        {
            case Actions.DoNothing:
                DoNothing();
                break;
            case Actions.QuitFromMenu:
                QuitFromMenu();
                break;
            case Actions.ContinueGameFromPause:
                ContinueGameFromPause(new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new GameState_scr());
                break;
            case Actions.StartNewGame:
                StartNewGame();
                break;
            case Actions.LoadGame:
                LoadGame();
                break;
            case Actions.SaveGame:
                SaveGame();
                break;
            case Actions.LoadLevel:
                LoadLevel();
                break;
        }
    }

    static public void DoAction(Actions action, List<GameObject> activateObjs, List<GameObject> deactivateObjs, List<GameObject> enableObjs, List<GameObject> disableObjs, GameState_scr gameStateScr)
    {
        switch (action)
        {
            case Actions.ContinueGameFromPause:
                ContinueGameFromPause(activateObjs, deactivateObjs, enableObjs, disableObjs, gameStateScr);
                break;
        }
    }
}
