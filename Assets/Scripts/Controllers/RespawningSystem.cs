using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawningSystem : MonoBehaviour
{
    private static RespawningSystem _instance;

    public static RespawningSystem GetInstance()
    {
        return _instance;
    }


    private Dictionary<IActor,bool> _playerState = null;
    private List<IActor> players = null;
    [SerializeField] private float _respawnTime=2f;
    private bool _gameOver = false;



    public UnityEvent OnGameOver;
    public UnityEvent<int> OnPlayerRespawn;
    public UnityEvent<int> OnPlayerDead;
    public float respawnTime => _respawnTime;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);



        _playerState = new Dictionary<IActor, bool>();
        players = new List<IActor>();


    }

    private void Start()
    {
        var actors = gameObject.FindInterfacesOfType<IActor>();
        for (int i = 0; i < actors.Length; i++)
        {
            if (actors[i].GetActorComponent<IActorIdentity>(0).characterIdentifier < 10)
            {
                var player = actors[i];
                player.transform.position = CheckPoint.GetActiveCheckPointPosition(player.GetActorComponent<IActorIdentity>(0).characterIdentifier);

                players.Add(player);
                _playerState[player] = true;
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        var deadCount = 0;
        foreach(var player in players) { 
            if (player.GetActorComponent<IActorHealth>(0).IsDead())
            {
                deadCount++;
                if (_playerState[player] == true)
                {
                    StartCoroutine(RespawnPlayer(player));
                    _playerState[player] = false;
                    OnPlayerDead.Invoke(player.GetActorComponent<IActorIdentity>(0).characterIdentifier);
                }
            }
        }
        //if (deadCount == players.Count && _gameOver == false)
        //{
        //    Debug.Log("GameOver");
        //    _gameOver = true;
        //    StopAllCoroutines();
        //    OnGameOver.Invoke();
        //}else if (deadCount < players.Count)
        //{
        //    _gameOver = false;
        //}
        
    }
    internal IEnumerator RespawnPlayer(IActor player)
    {
        player.transform.GetComponentInChildren<LightDamager>().enabled = false;

        yield return new WaitForSeconds(_respawnTime);


        player.transform.position = CheckPoint.GetActiveCheckPointPosition(player.GetActorComponent<IActorIdentity>(0).characterIdentifier);

        Debug.Log("RespawningSystem::RespawnPlayer");
        yield return null;

        player.GetActorComponent<IActorHealth>(0).Heal(player.GetActorComponent<IActorHealth>(0).GetMaxValue());


        _playerState[player] = true;
        OnPlayerRespawn.Invoke(player.GetActorComponent<IActorIdentity>(0).characterIdentifier);

        yield return new WaitForSeconds(0.5f);

        player.transform.GetComponentInChildren<LightDamager>().enabled = true;
    }


    public void ResetCheckpoints()
    {
        CheckPoint.ResetCheckpoints();
    }

    public void RespawnPlayers()
    {
        players.Clear();
        var actors = gameObject.FindInterfacesOfType<IActor>();
        for (int i = 0; i < actors.Length; i++)
        {
            if (actors[i].GetActorComponent<IActorIdentity>(0).characterIdentifier < 10)
            {
                var player = actors[i];
                players.Add(player);
            }
        }
        foreach (var player in players)
        {
            player.transform.position = CheckPoint.GetActiveCheckPointPosition(player.GetActorComponent<IActorIdentity>(0).characterIdentifier);
            player.GetActorComponent<IActorHealth>(0).Heal(player.GetActorComponent<IActorHealth>(0).GetMaxValue());
            _playerState[player] = true;
        }
    }

}
