using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoat : EnemyBase
{
    public float Speed;

    [SerializeField] private Transform middleScreen;
    [SerializeField] private Transform handSpawnPoint;

    [Header("Hand")]
    [SerializeField] private GameObject Hand;

    [Header("Boat Status")]
    [SerializeField] private bool isReachedMiddleScreen;

#if UNITY_EDITOR
    public override void OnValidate()
    {
        base.OnValidate();
    }
#endif

    public override void Init()
    {
        base.Init();
        SetMiddleScreen();
    }

    private void SetMiddleScreen()
    {
        if (!middleScreen)
            middleScreen = GameObject.Find("MiddleScreen").GetComponent<Transform>();
    }

    private void Update()
    {
        BoatLogic();
    }

    private void BoatLogic()
    {
        if (!isReachedMiddleScreen)
        {
            transform.position = Vector3.MoveTowards(transform.position, middleScreen.position, Speed * Time.deltaTime);

            if (transform.position == middleScreen.position)
            {
                isReachedMiddleScreen = true;
                SpawnHand();
            }
        }
    }

    public override void TakeDamage(int _damage)
    {
        if (Hand) return;

        base.TakeDamage(_damage);
    }

    private void SpawnHand()
    {
        Hand _hand = Instantiate(Hand, handSpawnPoint.position, Quaternion.identity).GetComponent<Hand>();
        _hand.SetHand(HP, this);
    }
}
