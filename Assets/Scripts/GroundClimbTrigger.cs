using UnityEngine;

public class GroundClimbTrigger : MonoBehaviour
{
    private Transform _parent;
    private Vector3 _parentpos;

    public bool left;
    public float shift, yshift;

    private Vector2[] _points;
    private float _width;
    private float _height;

    // Start is called before the first frame update
    void Start()
    {
        _parent = transform.parent;
        _points = _parent.GetComponent<PolygonCollider2D>().points;
        
        _width = Mathf.Abs(_points[0].x - _points[2].x);
        _height = Mathf.Abs(_points[0].y - _points[1].y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _parentpos = _parent.position;
        float x = _parentpos.x + (left ? shift : _width),
            y = other.transform.position.y + yshift * _height,
            z = _parentpos.z;

        other.GetComponent<CharacterController>().ClimbOnEdgeStart(new Vector3(x, y, z));
    }
}