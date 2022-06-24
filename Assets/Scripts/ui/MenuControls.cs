using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    private GameObject[] _pauseObjects = {};
    private AudioSource _backMusic;
    private CameraController _cam;
    
    private GameObject[] _hpUI = { };

    private float _volume;

    private void Start()
    {
        Time.timeScale = 0;
        _pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        _hpUI = GameObject.FindGameObjectsWithTag("Health");
        
        _backMusic = GameObject.FindGameObjectWithTag("BackgroundMusic")
            .GetComponent<AudioSource>();
        
        _cam = GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<CameraController>();

        _volume = _backMusic.volume;
        
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

        foreach (var h in _hpUI)
        {
            h.SetActive(true);
        }
        
        _backMusic.volume = _volume;
        _cam.enabled = true;
    }

    private void ShowPaused()
    {
        foreach (var g in _pauseObjects)
        {
            g.SetActive(true);
        }
        
        foreach (var h in _hpUI)
        {
            h.SetActive(false);
        }
        
        _backMusic.volume = 0.005f;
        _cam.enabled = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
}