using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private int startMenu = -1;
    
    [SerializeField]
    private List<GameObject> menuObjects;

    [SerializeField]
    private int currentMenu = 0;

    private void OnEnable()
    {
        foreach (var menu in menuObjects)
        {
            menu.SetActive(false);
        }
        
        menuObjects[(startMenu < 0) ? currentMenu : startMenu].SetActive(true);
    }

    public void SwitchMenu(int menuKey)
    {
        foreach (var menu in menuObjects)
        {
            menu.SetActive(false);
        }
        
        menuObjects[menuKey].SetActive(true);
        currentMenu = menuKey;
    }
    
    public void SwitchMenu(string menuName)
    {
        foreach (var menu in menuObjects)
        {
            menu.SetActive(false);
        }
        
        currentMenu = menuObjects.FindIndex(x => x.name == menuName);
        menuObjects[currentMenu].SetActive(true);
    }
    
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    private void OnValidate()
    {
        currentMenu = Mathf.Clamp(currentMenu, 0, menuObjects.Count - 1);
        SwitchMenu(currentMenu);
    }
}
