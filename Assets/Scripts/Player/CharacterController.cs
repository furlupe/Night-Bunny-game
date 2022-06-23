using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float move;
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    public Vector2 knockBackForce = new(35f, 0);
    public float knockBackDuration = 1f;

    private bool _facingRight = true;
    private Vector2 _outsideForces;

    private Animator _animator = new ();
    private AudioSource _audio;
    public AudioClip walking;

    private bool _grounded;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int YSpeed = Animator.StringToHash("YSpeed");

    private void Flip()
    {
        _facingRight = !_facingRight;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ClimbOnEdge(Vector3 pos)
    {
        if (!_grounded)
            transform.position = pos;
    }

    private void AddForce(Vector2 force)
    {
        _outsideForces += force;
    }

    public void Knockback(int dir)
    {
        AddForce(knockBackForce * dir);
    }

    private void Start()
    {
        _audio = GetComponent<Player>().audio;
        _animator = GetComponent<Player>().animator;
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        _animator.SetBool(Grounded, _grounded);
        _animator.SetFloat(YSpeed, GetComponent<Rigidbody2D>().velocity.y);

        move = Input.GetAxis("Horizontal");

        _animator.SetFloat(Speed, Mathf.Abs(move));

        var newVelocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        GetComponent<Rigidbody2D>().velocity = newVelocity + _outsideForces;
        
        if (Mathf.Abs(move) > 0 && !_audio.isPlaying && _grounded) _audio.PlayOneShot(walking);
        
        if (move > 0 && !_facingRight || move < 0 && _facingRight)
            Flip();
    }

    private void Update()
    {
        _outsideForces = Vector2.Lerp(_outsideForces, Vector2.zero, knockBackDuration * Time.deltaTime);
        if (!_grounded || !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.UpArrow)) return;
        
        _animator.SetBool(Grounded, false);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
    }
}