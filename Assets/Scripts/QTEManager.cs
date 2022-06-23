using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class QTEManager : MonoBehaviour
{
    public new CameraController camera;

    public float fillAmount;
    public float timeThreshold;

    private float _fillingAmount = .2f;
    private float _defillingAmount = .02f;

    private float _shakeAmount = 0.7f;

    public bool eventComplete;
    private Dictionary<KeyCode, GameObject> _qte = new();

    private KeyCode[] _keys;

    private KeyCode _currentKey;
    private GameObject _currentKeyGo;

    private Random _rndIndex;


    public void Init(Dictionary<KeyCode, GameObject> qte, float fA = 0.2f, float dfA = 0.02f, float sA = 0.7f)
    {
        _fillingAmount = fA;
        _defillingAmount = dfA;
        _shakeAmount = sA;

        _qte = qte;
    }

    private void Start()
    {
        _rndIndex = new Random();
        _keys = _qte.Keys.ToArray();

        ChangeKey();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_currentKey))
        {
            camera.TriggerShake(_shakeAmount);

            fillAmount += _fillingAmount;

            ChangeKey();
        }

        timeThreshold += Time.deltaTime;

        if (timeThreshold > .1f)
        {
            timeThreshold = 0f;
            fillAmount -= fillAmount > 0 ? _defillingAmount : 0f;
        }

        if (!(fillAmount > 1)) return;

        eventComplete = true;
        enabled = false;
        _currentKeyGo.SetActive(false);
    }

    public void DisableQte()
    {
        if (!_currentKeyGo) return;

        _currentKeyGo.SetActive(false);
    }

    public void EnableQte()
    {
        if (!_currentKeyGo) return;

        _currentKeyGo.SetActive(true);
    }

    private void ChangeKey()
    {
        _currentKey = _keys[_rndIndex.Next(0, _keys.Length)];

        DisableQte();
        _currentKeyGo = _qte[_currentKey];
        EnableQte();
    }
}