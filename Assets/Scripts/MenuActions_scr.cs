using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions_scr : MonoBehaviour
{
    public enum Actions
    {
        DoNothing,
        QuitFromMenu,
        StartNewGame,
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

    static public void LoadGame()
    {
        GameStatic_scr.Load();
    }

    static public void SaveGame()
    {
        GameStatic_scr.Save();
    }

    static public void ContinueGameFromPause(List<GameObject> activateObjs, List<GameObject> deactivateObjs, List<GameObject> enableObjs, List<GameObject> disableObjs, General_scr genaralScript)
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

        genaralScript.curGameState = General_scr.GameState.GAME;
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
                ContinueGameFromPause(new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new General_scr());
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
        }
    }

    static public void DoAction(Actions action, List<GameObject> activateObjs, List<GameObject> deactivateObjs, List<GameObject> enableObjs, List<GameObject> disableObjs, General_scr genaralScript)
    {
        switch (action)
        {
            case Actions.ContinueGameFromPause:
                ContinueGameFromPause(activateObjs, deactivateObjs, enableObjs, disableObjs, genaralScript);
                break;
        }
    }
}
