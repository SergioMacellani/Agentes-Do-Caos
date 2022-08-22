using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public InputField player;

    public void GoPlayer()
    {
        if(player.text == "wings")
        {
            SceneManager.LoadScene("Lucy");
        }
        else if(player.text == "zelda")
        {
            SceneManager.LoadScene("Norman");
        }
        else if (player.text == "gris")
        {
            SceneManager.LoadScene("Harper");
        }
        else if (player.text == "coin")
        {
            SceneManager.LoadScene("Cedrich");
        }
        else if (player.text == "darkness")
        {
            SceneManager.LoadScene("Hanna");
        }
        else if (player.text == "jedi")
        {
            SceneManager.LoadScene("Odyssey");
        }
        else if (player.text == "hellboy")
        {
            SceneManager.LoadScene("Jason");
        }
        else if (player.text == "precioso")
        {
            SceneManager.LoadScene("Sauron");
        }
    }

    public void GoPlayerBtt(string name)
    {
        if (name == "wings")
        {
            SceneManager.LoadScene("Lucy");
        }
        else if (name == "zelda")
        {
            SceneManager.LoadScene("Norman");
        }
        else if (name == "gris")
        {
            SceneManager.LoadScene("Harper");
        }
        else if (name == "coin")
        {
            SceneManager.LoadScene("Cedrich");
        }
        else if (name == "darkness")
        {
            SceneManager.LoadScene("Hanna");
        }
        else if (name == "jedi")
        {
            SceneManager.LoadScene("Odyssey");
        }
    }
}
