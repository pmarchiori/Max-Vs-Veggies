using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; 

    [SerializeField] private float projectileSpeed;

    public float ProjectileSpeed
    {
        get {return projectileSpeed;}
    }

    private SpriteRenderer spriteRenderer;

    private Enemy target; //towers current target

    public Enemy Target
    {
        get{return target;}
    }

    private Queue<Enemy> enemies = new Queue<Enemy>();

    private bool canAttack = true;

    [SerializeField] private float attackTimer;

    [SerializeField] private float attackCooldown;

    private Projectile projectile;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        //Debug.Log(target);
    }

    public void Select()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void Attack()
    {
        if(!canAttack)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }

        if(target == null && enemies.Count > 0)
        {
            target = enemies.Dequeue();
        }

        if(target != null && target.IsActive)
        {
            if(canAttack)
            {
                Shoot();

                canAttack = false;
            }
        }
    }

    private void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        
        //projectilePrefab.transform.position = this.transform.position;

        //Instantiate(projectilePrefab);

         if (projectile != null)
         {
            projectile.Initialize(this);
         }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            enemies.Enqueue(other.GetComponent<Enemy>()); //adds new monsters to the queue when they enter the range
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            target = null;
        }
    }
}
