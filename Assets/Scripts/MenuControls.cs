using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    private GameObject[] _pauseObjects = {};
    private AudioSource _backMusic;
    private CameraController _cam;

    private void Start()
    {
        Time.timeScale = 0;
        _pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        
        _backMusic = GameObject.FindGameObjectWithTag("BackgroundMusic")
            .GetComponent<AudioSource>();
        
        _cam = GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<CameraController>();
        
        ShowPaused();
    }

    private void Update()
    {
        if (! Input.GetKeyDown(KeyCode.Escape)) return;
        PauseControl();
    }
    public void PauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowPaused();
            return;
        }

        Time.timeScale = 1;
        HidePaused();
    }

    private void HidePaused()
    {
        foreach (var g in _pauseObjects)
        {
            g.SetActive(false);
        }
        _backMusic.GetComponent<AudioSource>().Play();
        _cam.enabled = true;
    }

    private void ShowPaused()
    {
        foreach (var g in _pauseObjects)
        {
            g.SetActive(true);
        }
        _backMusic.Pause();
        _cam.enabled = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
}