using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    private GameObject[] _pauseObjects = {};
    private GameObject _backMusic;
    void Start()
    {
        Time.timeScale = 0;
        _pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        _backMusic = GameObject.FindGameObjectWithTag("BackgroundMusic");
        showPaused();
    }

    void Update()
    {
        if (! Input.GetKeyDown(KeyCode.Escape)) return;
        pauseControl();
    }


    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
            return;
        }

        Time.timeScale = 1;
        hidePaused();
    }

    public void hidePaused()
    {
        foreach (var g in _pauseObjects)
        {
            g.SetActive(false);
        }
        _backMusic.GetComponent<AudioSource>().Play();
    }
    
    public void showPaused()
    {
        foreach (var g in _pauseObjects)
        {
            g.SetActive(true);
        }
        _backMusic.GetComponent<AudioSource>().Pause();
    }

    public void Exit()
    {
        Application.Quit();
    }
}