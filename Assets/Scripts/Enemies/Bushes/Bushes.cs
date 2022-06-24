using UnityEngine;

public class Bushes : Enemy
{
    private void Start()
    {
        FieldOfView = new Vector2(5, 0);
        _fovAngle = 180;

        Health = 100;
        Damage = 35;
        _fovAngle = 180;

        Init();
        
        GetComponent<EnemyAI>().Init(this);
    }

    private void Update()
    {
        outsideForces = Vector2.Lerp(outsideForces, Vector2.zero, knockBackDuration * Time.deltaTime);
        if (!CheckIfPlayerWithinFov(FieldOfView)) return;
        Animator.SetBool(PlayerSpotted, true);
    }

    public void LaunchAI()
    {
        GetComponent<EnemyAI>().enabled = true;
        transform.GetChild(0).GetComponent<EnemyAttack>().Init(this);
    }

    public override void Die()
    {
        Animator.SetBool(_isDead, true);
        Animator.SetBool(PlayerSpotted, false);

        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        
        Disable();
    }

}