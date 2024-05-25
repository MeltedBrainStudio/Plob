using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Player => player;

    [Header("Reference")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private AudioSource sfxSource;

    [Header("UI")]
    [SerializeField] private Image logo;
    [SerializeField] private Image howto;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("EndGame")]
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private TextMeshProUGUI scoreEndGameText;
    [SerializeField] private Button newGameButton;

    [Header("Status")]
    public int Score;
    public bool IsInit;
    public bool IsEnd;

    [Space]
    [Header("Spawn Point / Up")]
    [SerializeField] private Transform up_rightSpawnPoint, down_rightSpawnPoint;

    [Space]
    [Header("Spawn Point / Down")]
    [SerializeField] private Transform up_leftSpawnPoint, down_leftSpawnPoint;

    [Space]
    [Header("Enemy Ship Setting")]
    public int EnemySpawnChance_Ship;

    [Space]
    [Header("Enemy Fish Setting")]
    public int EnemySpawnChance_Fish;

    [Space]
    [Header("SFXs")]
    public AudioClip GameOverSound;
    public AudioClip ShootingSound;
    public AudioClip EnemyDeathSound;

    //Queueing
    public Queue<QueueData> queueData = new Queue<QueueData>();

    private void Awake()
    {
        Singleton();
    }

    private void Init()
    {
        RandomEvent(5);
        SpawnNewWave();
    }

    private void StartGame()
    {
        logo.enabled = false;
        text.enabled = false;
        howto.enabled = false;
        scoreText.enabled = true;
        Init();
    }

    private void Update()
    {
        if (Input.anyKey && !IsInit)
        {
            StartGame();
            IsInit = true;
        }
    }

    private void Singleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Scoring()
    {
        Score++;
        scoreText.text = Score.ToString();
    }

    private void RandomEvent(int _round)
    {
        for(int i = 0; i < _round; i++)
        {
            int _randomShip = UnityEngine.Random.Range(0, 100);
            int _randomFish = UnityEngine.Random.Range(0, 100);

            bool _isShip = _randomShip <= EnemySpawnChance_Ship;
            bool _isFish = _randomFish <= EnemySpawnChance_Fish;

            QueueData _queueData = new QueueData(_isShip, _isFish);
            queueData.Enqueue(_queueData);
        }
    }


    public void TryDeQueue()
    {
        QueueData _queueData = queueData.Peek();
        _queueData.Count--;

        if (_queueData.Count <= 0)
        {
            queueData.Dequeue();
            RandomEvent(1);

            SpawnNewWave();
        }
    }

    private IEnumerator Cooling()
    {
        yield return new WaitForSeconds(3);
        RandomEvent(1);
        SpawnNewWave();
    }

    private void StartCooling() => StartCoroutine(Cooling());

    private void SpawnNewWave()
    {
        QueueData _queueData = queueData.Peek();
        if (!_queueData.IsFishWillSpawn && !_queueData.IsShipWillSpawn)
        {
            StartCooling();
            return;
        }

        for(int i = 0; i < _queueData.Count; i++)
        {
            int _value = UnityEngine.Random.Range(0, 4);

            bool _isRight = false;
            if(_value > 2)
                _isRight = true;

            Debug.Log(_isRight);

            EnemyBase _enemyBase;

            if (_isRight)
            {
                if (_queueData.IsFishWillSpawn)
                {
                    _queueData.IsFishWillSpawn = false;

                    _enemyBase = Instantiate(fishPrefab, down_rightSpawnPoint.position, Quaternion.identity).GetComponent<EnemyBase>();
                    _enemyBase.Init();
                }

                if (_queueData.IsShipWillSpawn)
                {
                    _queueData.IsShipWillSpawn = false;

                    _enemyBase = Instantiate(shipPrefab, up_rightSpawnPoint.position, Quaternion.identity).GetComponent<EnemyBase>();
                    _enemyBase.Init();
                }
            }
            else
            {
                if (_queueData.IsFishWillSpawn)
                {
                    _queueData.IsFishWillSpawn = false;

                    _enemyBase = Instantiate(fishPrefab, down_leftSpawnPoint.position, Quaternion.identity).GetComponent<EnemyBase>();
                    _enemyBase.FlipSprite();
                    _enemyBase.Init();
                }

                if (_queueData.IsShipWillSpawn)
                {
                    _queueData.IsShipWillSpawn = false;

                    _enemyBase = Instantiate(shipPrefab, up_leftSpawnPoint.position, Quaternion.identity).GetComponent<EnemyBase>();
                    _enemyBase.FlipSprite();
                    _enemyBase.Init();
                }
            }
        }
    }

    public void PlaySound(AudioClip _clip)
    {
        sfxSource.clip = _clip;
        sfxSource.Play();
    }

    public void EndGame()
    {
        if (!IsEnd)
        {
            scoreText.enabled = false;

            endGameText.enabled = true;
            scoreEndGameText.enabled = true;
            logo.enabled = true;
            text.enabled = false;

            scoreEndGameText.text = Score.ToString();

            newGameButton.gameObject.SetActive(true);
            newGameButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });

            PlaySound(GameOverSound);
            IsEnd = true;
        }
        
    }
}

public class QueueData
{
    public bool IsShipWillSpawn;
    public bool IsFishWillSpawn;

    public int Count;

    public QueueData(bool _isShip, bool _isFish)
    {
        IsShipWillSpawn = _isShip;
        IsFishWillSpawn = _isFish;

        if (IsShipWillSpawn)
            Count++;

        if (IsFishWillSpawn)
            Count++;
    }
}
