using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float _length, _startpos;
    public GameObject cam;
    public float parallaxEffect;

    public void Start()
    {
        _startpos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var position = cam.transform.position;

        var temp = (position.x * (1 - parallaxEffect));
        var dist = (position.x * parallaxEffect);

        transform.position = new Vector3(_startpos + dist, transform.position.y, transform.position.z);

        if (temp > _startpos + _length) _startpos += _length;
        else if (temp < _startpos - _length) _startpos -= _length;
    }
}