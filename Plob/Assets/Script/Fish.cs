using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : EnemyBase
{
    public float Speed;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, Speed * Time.deltaTime);
    }

    public override void TakeDamage(int _damage)
    {
        Anim.SetTrigger("Fish_Hurt");
        base.TakeDamage(_damage);
    }

    public override void Death()
    {
        Anim.SetBool("Fish_Death", true);
        base.Death();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
            GameManager.Instance.EndGame();
    }
}
