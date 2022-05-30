using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void LoadLevel1()
    {
        SceneManager.LoadScene("Level01");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level02");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level03");
    }
    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level04");
    }
    public void LoadLevel5()
    {
        SceneManager.LoadScene("Level05");
    }
}
