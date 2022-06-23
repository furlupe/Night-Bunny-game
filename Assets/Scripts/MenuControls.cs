using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{

    public void PlayPressed()
    {
        SceneManager.LoadScene("Scenes/level_1");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}