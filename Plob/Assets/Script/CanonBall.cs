using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CanonBall : MonoBehaviour
{
    [Header("Setting")]
    public int Damage;
    public float DestroyTiming;
    public float ParticleTiming;

    [Header("Reference")]
    [SerializeField] private GameObject particleEffect;

    private void OnEnable()
    {
        Destroy(gameObject, DestroyTiming);
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Enemy"))
        {
            var _enemyScript = _collision.gameObject.GetComponent<EnemyBase>();
            GameObject _particleOBJ = Instantiate(particleEffect, transform.position, Quaternion.identity);

            _enemyScript.TakeDamage(Damage);

            Destroy(_particleOBJ, ParticleTiming);
            Destroy(this.gameObject);
        }
    }
}
