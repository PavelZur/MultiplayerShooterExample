using Colyseus;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    public float RTT { get; private set; }
    public string PlayerID { get; private set; }

    public Action<Player> OnCreatePlayerLocal;
    public Action<Player, string> OnCreateEnemy;
    public Action<Player, string> OnRemoveEnemy;

    public Action<ShootingInfo> OnShootingEnemyEvent;
    public Action<string> OnEnemyGunReloadEvent;
    public Action<string> OnChangeHealthEvent;
    public Action<string> OnDiePlayerEvent;

    private ColyseusRoom<State> _room;
    private long _pingStartTime;

    public bool IsInit { get; private set; }

    protected override void Start()
    {
        IsInit = false;
        base.Start();
        Initialized().Forget();
    }

    private async UniTaskVoid Initialized()
    {
        await UniTask.WaitUntil(() => Instance != null);

        Instance.InitializeClient();

        Transform transformPoint = SpawnPointManager.Instance.GetRandomTransformPoint();
        Vector3 spawnPosition = transformPoint.position;
        float rotationY = transformPoint.eulerAngles.y;

        Dictionary<string, object> _initPlayerData = new()
        {
            { "maxHealth", 50 },
            { "curHealth", 50 },
            { "weaponId", 1 },
            { "px", spawnPosition.x },
            { "py", spawnPosition.y },
            { "pz", spawnPosition.z },
            { "ry", rotationY },
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", _initPlayerData);

        _room.OnMessage<string>("pong", OnPongReceived);
        _room.OnMessage<string>("Shoot", OnShootingEnemy);
        _room.OnMessage<string>("ReloadWeapon", OnReloadGunEnemy);
        _room.OnMessage<string>("Die", OnDiePlayer);

        _room.OnStateChange += OnChangeRoomHandler;
        _room.OnError += OnErrorRoomHandler;
        
        InvokeRepeating(nameof(SendPing), 1f, 3f);

        IsInit = true;
    }

    private void OnChangeRoomHandler(State state, bool isFirstState)
    {
        if (isFirstState)
        {
            var player = state.players[_room.SessionId];
            OnCreatePlayerLocal?.Invoke(player);
            PlayerID = _room.SessionId;

            state.players.ForEach(ForEachEnemysCreate);

            state.players.OnAdd += ForEachEnemysCreate;
            state.players.OnRemove += RemoveEnemy;
        }
    }

    private void ForEachEnemysCreate(string key, Player enemy)
    {
        if (key == _room.SessionId) return;

        OnCreateEnemy?.Invoke(enemy, key);
    }

    private void OnErrorRoomHandler(int code, string message)
    {
        Debug.Log($"[{this.name}]code : {code}, message: {message}");
    }

    private void RemoveEnemy(string key, Player player)
    {
        OnRemoveEnemy?.Invoke(player, key);
    }

    public void SendMessageColyseus(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessageColyseus(string key, string data)
    {
        _room.Send(key, data);
    }

    public void SendMessageColyseus(string key)
    {
        _room.Send(key);
    }

    private void SendPing()
    {
        if (_room != null)
        {
            _pingStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _room.Send("ping");
        }
    }

    private void OnPongReceived(string message)
    {
        long pongTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        RTT = pongTime - _pingStartTime;
    }

    private void OnShootingEnemy(string data)
    {
        if (string.IsNullOrEmpty(data)) return;

        ShootingInfo info = JsonUtility.FromJson<ShootingInfo>(data);
        OnShootingEnemyEvent?.Invoke(info);
    }

    private void OnReloadGunEnemy(string sessionId)
    {
        OnEnemyGunReloadEvent?.Invoke(sessionId);
    }

    private void OnDiePlayer(string sessionId)
    {
        OnDiePlayerEvent?.Invoke(sessionId);
    }

    protected override void OnDestroy()
    {
        _room.OnError -= OnErrorRoomHandler;
        _room.OnStateChange -= OnChangeRoomHandler;

        base.OnDestroy();
        _room.Leave();
    }
}
