using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Setting")]
    public int HP;

    [Space]
    [Header("Reference")]
    [SerializeField] private Collider2D collider;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Collider2D Collider => collider;
    public Animator Anim => anim;
    public Rigidbody2D Rb => rb;

#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        if (!collider)
            collider = GetComponent<Collider2D>();

        if (!anim)
            anim = GetComponentInChildren<Animator>();

        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        if (!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    } 

#endif

    public virtual void Init()
    {

    }

    public void FlipSprite() => spriteRenderer.flipX = true;

    public virtual void TakeDamage(int _damage)
    {
        Debug.Log("TakeDamage");
        HP -= _damage;

        if (HP <= 0)
            Death();
    }

    public virtual void Death()
    {
        Debug.Log("TryDeath");

        if(collider)
           collider.enabled = false;

        rb.Sleep();
        Destroy(gameObject);

        GameManager.Instance.TryDeQueue();
        GameManager.Instance.Scoring();
        GameManager.Instance.PlaySound(GameManager.Instance.EnemyDeathSound);
    }

    public void SetTriggerAnim(string _setAnim) => anim.SetTrigger(_setAnim);
    public void SetBoolAnim(string _setAnim, bool _value) => anim.SetBool(_setAnim, _value);
}
