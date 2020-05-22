using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions_scr : MonoBehaviour
{
    public enum Actions
    {
        DoNothing,
        QuitFromMenu,
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

    static public void ContinueGameFromPause(List<GameObject> activateObjs, List<GameObject> deactivateObjs)
    {
        for (int i = 0; i < activateObjs.Count; i++)
        {
            activateObjs[i].SetActive(true);
        }

        for (int i = 0; i < deactivateObjs.Count; i++)
        {
            deactivateObjs[i].SetActive(false);
        }
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
                ContinueGameFromPause(new List<GameObject>(), new List<GameObject>());
                break;
        }
    }

    static public void DoAction(Actions action, List<GameObject> activateObjs, List<GameObject> deactivateObjs)
    {
        switch (action)
        {
            case Actions.ContinueGameFromPause:
                ContinueGameFromPause(activateObjs, deactivateObjs);
                break;
        }
    }
}
