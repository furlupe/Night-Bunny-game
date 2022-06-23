using UnityEngine;

public class Spider : Enemy
{
    public GameObject projectile;

    private bool _isActive;
    private const float Cooldown = 2.5f;
    private float _timerCooldown;

    private bool _isFlipped;

    public GameObject corpsePrefab;

    void Start()
    {
        
        const int h = 100;
        const int dmg = 0;
        
        Init(h, dmg);
        FieldOfView = new Vector2(5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckIfPlayerWithinFov(FieldOfView)) return;
        if (!_isActive)
        {
            Animator.SetTrigger("Awake");
        }

        if (!(Time.time > _timerCooldown) || !_isActive) return;

        Animator.SetTrigger("Shoot");
        _timerCooldown = Time.time + Cooldown;
    }

    private void Shoot()
    {
        var prj = Instantiate(projectile);
        prj.transform.position = transform.position;

        prj.GetComponent<SpiderProjectile>().Launch(
            Player.transform.position
        );
    }

    public void ReadyToShoot()
    {
        Animator.SetBool("isActive", true);
        _isActive = true;
    }

    public override void Die()
    {
        Animator.SetBool(_isDead, true);
        Animator.SetBool(PlayerSpotted, false);

        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        GetComponent<Spider>().enabled = false;

        Disable();
    }

    public void spawnCorpse()
    {
        var corpse = Instantiate(corpsePrefab);
        corpse.transform.position = transform.position;
    }
}