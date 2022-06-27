using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    private GameObject[] _pauseObjects = {};
    private AudioSource _backMusic;
    private CameraController _cam;
    
    public GameObject[] hpUI = { };
    public GameObject taskUI;
    public Image Shadow;

    private float _volume;
    private bool _enwhite;

    private void Start()
    {
        Time.timeScale = 0;
        _pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hpUI = GameObject.FindGameObjectsWithTag("Health");
        
        _backMusic = GameObject.FindGameObjectWithTag("BackgroundMusic")
            .GetComponent<AudioSource>();
        
        _cam = GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<CameraController>();

        _volume = _backMusic.volume;

        taskUI.transform.localPosition = new Vector3(
            taskUI.transform.localPosition.x, 
            taskUI.transform.localPosition.y,
            0);
        
        ShowPaused();
    }

    private void Update()
    {

        if (_enwhite)
        {
            Shadow.color = Color.Lerp(
                Shadow.color,
                Color.white,
                0.5f * Time.deltaTime
            );
            
            _cam.TriggerShake(2f);
        }
        
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

        foreach (var h in hpUI)
        {
            h.SetActive(true);
        }
        taskUI.SetActive(true);
        
        _backMusic.volume = _volume;
        _cam.enabled = true;
    }

    private void ShowPaused()
    {
        foreach (var g in _pauseObjects)
        {
            g.SetActive(true);
        }
        
        foreach (var h in hpUI)
        {
            h.SetActive(false);
        }
        taskUI.SetActive(false);
        
        _backMusic.volume = 0.005f;
        _cam.enabled = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void EnwhiteShadow()
    {
        Shadow.gameObject.SetActive(true);
        _enwhite = true;
    }
}