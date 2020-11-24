using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement_controller : MonoBehaviour
{
    
    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private Player_controller playerController;
    public event Action<bool> OnGetHurt = delegate { };

    [Header("Horisontal movement")]
    [SerializeField] private float speed;
    private bool faceRight = true;
    private bool canMove = true;

    [Header("Jumping")]
    [SerializeField] private float jumpForse;
    [SerializeField] private float radius;
    [SerializeField] private bool airControll;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool doubleJump;
    private bool grounded;

    [Header("Crawling")]
    [SerializeField] private Transform crawlingCheck;    
    [SerializeField] private BoxCollider2D headCollider;
    private bool canStand;

    [Header("Cast")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireBallSpeed;
    [SerializeField] private int castCost;
    private bool isCasting;

    [Header("Strike")]
    [SerializeField] private Transform strikePoint;
    [SerializeField] private int damage;
    [SerializeField] private float atackRange;
    [SerializeField] private LayerMask _enemies;

    [Header("Power Strike")]
    [SerializeField] private float chargeTime;
    public float ChargeTime => chargeTime;
    [SerializeField] private float powerStrikeSpeed;
    [SerializeField] private Collider2D powerStrikeCollider;
    [SerializeField] private int powerStrikeDamage;
    [SerializeField] private int powerStrikeCost;
    private List<EnemyControllerBase> damagedEnemies = new List<EnemyControllerBase>();
    private bool isSwordAttack;

    [SerializeField] private float pushForce;
    private float lastHurtTime;

    [Header("Blink")]
    [SerializeField] private float blinkSpeed;
    private bool isBlink = false;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<Player_controller>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);

        if (playerAnimator.GetBool("Hurt") && grounded && Time.time - lastHurtTime > 0.5f)
            EndHurt();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
        Gizmos.DrawWireSphere(crawlingCheck.position, radius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(strikePoint.position, atackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyControllerBase enemy = collision.collider.GetComponent<EnemyControllerBase>();
        if (enemy == null || damagedEnemies.Contains(enemy))
            return;
        enemy.TakeDamage(powerStrikeDamage);
        damagedEnemies.Add(enemy);
    }

    void Flip ()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }

    public void Move (float move, bool jump, bool crawling)
    {
        if (!canMove) return;
        #region Movement
        if (move != 0 && (grounded || airControll))
            playerRB.velocity = new Vector2(speed * move, playerRB.velocity.y);
        else if (move != 0 || grounded)
            playerRB.velocity = new Vector2(speed * move, playerRB.velocity.y);

        playerAnimator.SetFloat("Speed", Mathf.Abs(move));


        if (move > 0 && !faceRight) Flip();
        else if (move < 0 && faceRight) Flip();
        #endregion

        #region Jump
        if (grounded)
            doubleJump = true;
        if (jump && grounded)
        {
            playerRB.AddForce(Vector2.up * jumpForse);
            playerRB.velocity = Vector2.zero;
            jump = false;
            
        }
        else if (jump && doubleJump)
        {
            playerRB.AddForce(Vector2.up * jumpForse);
            playerRB.velocity = Vector2.zero;
            jump = false;
            doubleJump = false;
        }
        #endregion

        #region Crawling
        canStand = !Physics2D.OverlapCircle(crawlingCheck.position, radius, whatIsGround);
        if (crawling) headCollider.enabled = false;
        else if (!crawling && canStand) headCollider.enabled = true;
        #endregion

        playerAnimator.SetBool("Jump", !grounded);
        playerAnimator.SetBool("Crouch", crawling || !canStand);
    }

    public void GetHurt(Vector2 position)
    {
        lastHurtTime = Time.time;
        canMove = false;
        OnGetHurt(false);
        playerRB.velocity = Vector2.zero;
        Vector2 pushDirection = new Vector2();
        pushDirection.x = position.x > transform.position.x ? -1 : 1;
        pushDirection.y = 1;
        playerAnimator.SetBool("Hurt", true);
        playerRB.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    }

    private void EndHurt()
    {
        ResetPlayer();
        OnGetHurt(true);
    }

    private void ResetPlayer()
    {
        playerAnimator.SetBool("Strike", false);
        playerAnimator.SetBool("PowerStrike", false);
        playerAnimator.SetBool("Casting", false);
        playerAnimator.SetBool("Hurt", false);
        playerAnimator.SetBool("Blink", false);
        isCasting = false;
        isSwordAttack = false;
        canMove = true;
        isBlink = false;
    }

    public void StartCasting()
    {
        if (isCasting || !playerController.ChangeMP(-castCost))
            return;
        isCasting = true;
        if (grounded)
            playerAnimator.SetBool("Casting", true);
        else
            playerAnimator.SetBool("JumpCasting", true);
    }

    private void CastFire()
    {
        GameObject fire_ball = Instantiate(fireBall, firePoint.position, Quaternion.identity);
        fire_ball.SetActive(true);
        fire_ball.GetComponent<Rigidbody2D>().velocity = transform.right * fireBallSpeed;
        fire_ball.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(fire_ball, 5f);
    }

    private void EndCast()
    {
        isCasting = false;
        playerAnimator.SetBool("Casting", false);
        playerAnimator.SetBool("JumpCasting", false);
    }

    public void StartSwordAtack( float holdTime)
    {
        if (isSwordAttack)
            return;
        isSwordAttack = true;
        if (grounded && holdTime < chargeTime)
        {
            
            playerAnimator.SetBool("Strike", true);
        }
            
            
        else if (grounded && holdTime >= chargeTime)
        {  
            if (!playerController.ChangeMP(-powerStrikeCost))
            {
                isSwordAttack = false;
                return;
            }
                
            playerAnimator.SetBool("PowerStrike", true);
            canMove = false;
        }
            
        else
            playerAnimator.SetBool("JumpStrike", true);
    }

    private void StartPowerStrike()
    {
        playerRB.velocity = transform.right * powerStrikeSpeed;
        powerStrikeCollider.enabled = true;
    }

    private void DisablePowerStrike()
    {
        playerRB.velocity = Vector2.zero;
        powerStrikeCollider.enabled = false;
        damagedEnemies.Clear();
    }

    private void EndPowerStrike ()
    {
        playerAnimator.SetBool("PowerStrike", false);
        canMove = true;
        isSwordAttack = false;
    }

    public void Atack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(strikePoint.position, atackRange, _enemies);
        for(int i = 0; i < enemies.Length; i++)
        {
            EnemyControllerBase enemy = enemies[i].GetComponent<EnemyControllerBase>();
            enemy.TakeDamage(damage);
        }
    }

    private void EndSwordAtack()
    {
        isSwordAttack = false;
        playerAnimator.SetBool("Strike", false);
        playerAnimator.SetBool("JumpStrike", false);
    }

    public void Blink ()
    {
        if (isBlink)
            return;
        isBlink = true;
        canMove = false;
        playerAnimator.SetBool("Blink", true);
    }

    public void StartBlink ()
    {
        playerRB.velocity = transform.right * blinkSpeed;
    }

    public void EndBlink ()
    {
        isBlink = false;
        playerAnimator.SetBool("Blink", false);
        playerRB.velocity = Vector2.zero;
        canMove = true;
    }
}

