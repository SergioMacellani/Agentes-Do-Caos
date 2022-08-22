using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menuObjects;

    [SerializeField]
    private ScrollRect scrollMenu;

    public void SwitchMenu(int menuKey)
    {
        foreach (var menu in menuObjects)
        {
            menu.SetActive(false);
        }
        menuObjects[menuKey].SetActive(true);
    }
    
    public void SwitchScrollMenu(int menuKey)
    {
        foreach (var menu in menuObjects)
        {
            menu.SetActive(false);
        }
        menuObjects[menuKey].SetActive(true);
        scrollMenu.content = menuObjects[menuKey].GetComponent<RectTransform>();
    }
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayerSceneOnline(PlayerSelect PlayerSelectScript)
    {
        if (PlayerSelectScript.playerSelect < PlayerSelectScript.lostPlayersCount)
        {
            PlayerPrefs.SetInt("PlayerSelect", PlayerSelectScript.playerSelect);
            PlayerPrefs.SetInt("OfflineMode",0);
            SceneManager.LoadScene("Loading");
        }
    }
    
    public void PlayerSceneOffline(PlayerSelect PlayerSelectScript)
    {
        PlayerPrefs.SetInt("PlayerSelect", PlayerSelectScript.playerSelect);
        PlayerPrefs.SetInt("OfflineMode",1);
        SceneManager.LoadScene("Loading");
    }
    
    public void PlayerSceneCreate(PlayerSelectCreate PlayerSelectScript)
    {
        if (PlayerSelectScript.createPlayer())
        {
            SceneManager.LoadScene("CreatePlayer");
        }
        else
        {
            PlayerPrefs.SetInt("PlayerCreateSelect", PlayerSelectScript.playerSelect);
            PlayerPrefs.SetInt("OfflineMode",2);
            SceneManager.LoadScene("Loading");
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
