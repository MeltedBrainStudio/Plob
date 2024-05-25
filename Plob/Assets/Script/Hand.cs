using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : EnemyBase
{
    [Header("Setting")]
    public float Speed;
    public float DeathTiming = 3;

    [Space]
    [Header("See Only")]
    [SerializeField] private float DeathTimingCount;
    [SerializeField] private BigBoat bigBoat;

    public void SetHand(int _hp, BigBoat _bigBoat)
    {
        HP = _hp;
        bigBoat = _bigBoat;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, Speed * Time.deltaTime);

        Vector3 _direction = GameManager.Instance.Player.transform.position - transform.position;
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        Quaternion _targetRotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, 5 * Time.deltaTime);
    }

    public override void TakeDamage(int _damage)
    {
        Anim.SetTrigger("Hand_Hurt");
        base.TakeDamage(_damage);
    }

    public override void Death()
    {
        base.Death();
        Destroy(bigBoat.gameObject);
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            DeathTimingCount += Time.deltaTime;

            if (DeathTimingCount >= DeathTiming)
                GameManager.Instance.EndGame();
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
            DeathTimingCount = 0;
    }
}
