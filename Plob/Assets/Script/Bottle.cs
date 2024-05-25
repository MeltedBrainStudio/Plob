using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [Header("Setting")]
    public float RotationSpeed;
    public float BulletSpeed;
    public float Cooldown, MaxCooldown;

    [Header("Reference")]
    [SerializeField] private Animator bottleAnim;
    [SerializeField] private Animator canonAnim;
    [SerializeField] private Animator pirateAnim;

    [SerializeField] private GameObject cannonBall;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform camTransform;

    [Space]
    [Header("Status(See Only)")]
    [SerializeField] private bool isMoving;

    [SerializeField] private float horizontalAxis;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float rotateValue;

    //Property
    public bool IsMoving => isMoving;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!shootingPoint)
            shootingPoint = GameObject.Find("ShootingPoint").GetComponent<Transform>();

        if (!camTransform)
            camTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
#endif

    void Update()
    {
        if (GameManager.Instance.IsInit && !GameManager.Instance.IsEnd)
        {
            MoveControl();
            ShootControl();
        }
    }

    private void LateUpdate()
    {
        FollowCam();
    }

    private void FollowCam() => transform.position = new Vector2(camTransform.position.x, transform.position.y);

    private void MoveControl()
    {
        rotateValue = transform.rotation.z;
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        isMoving = horizontalAxis != 0;

        if (isMoving)
        {
            rotationAmount = horizontalAxis * RotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationAmount);
        }

        if (rotateValue > 0.6 || rotateValue < -0.6)
            Death();
    }

    private void Death()
    {
        Debug.Log("Death");
    }

    private void ShootControl()
    {
        if(Cooldown > 0)
           Cooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && Cooldown <= 0)
        {
            Rigidbody2D _rb = Instantiate(cannonBall, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            Vector2 _dir = transform.up * BulletSpeed;
            _rb.AddForce(_dir);

            bottleAnim.SetTrigger("Shooting");
            canonAnim.SetTrigger("Shooting");
            pirateAnim.SetTrigger("Shooting");

            Cooldown = MaxCooldown;

            GameManager.Instance.PlaySound(GameManager.Instance.ShootingSound);
        }
    }
}
