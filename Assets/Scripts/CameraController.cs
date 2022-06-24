using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float speed = 2.5f;
    private Vector3 _playerpos;

    public float xshift, yshift;
    private int _dir = 1;

    public float fieldOfView = 3f;

    public float shakeDuration,
        shakeMagnitude = 0.02f,
        shakeSpeed = 5f;

    private Vector2 _beforeShakePos;

    private void Start()
    {
    }

    void Update()
    {   
        Camera.main.orthographicSize = fieldOfView;
        _beforeShakePos = transform.position;
        _playerpos = player.transform.position;

        if (shakeDuration > 0)
        {
            Vector3 pos = _beforeShakePos + Random.insideUnitCircle * shakeMagnitude;
            pos.z = -1;

            transform.localPosition = pos;
            shakeDuration -= Time.deltaTime * shakeSpeed;
            
            return;
        }

        /*var playerVx = player.GetComponent<Rigidbody2D>().velocity.x;

        _dir = playerVx > 0 ? 1 : _dir;
        _dir = playerVx < 0 ? -1 : _dir;

        var nextPos = new Vector3(_playerpos.x + _dir * xshift, _playerpos.y + yshift, -1);
        transform.position = Vector3.Lerp(transform.position, nextPos, speed * Time.deltaTime);*/
        
        transform.position = new Vector3(
            _playerpos.x,
            _playerpos.y + 1,
            -1
        );
    }

    public void TriggerShake(float duration)
    {
        shakeDuration = duration;
    }
}