using Colyseus;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{  
    public float RTT { get; private set; }
   
    public Action<Player> OnCreatePlayerLocal;
    public Action<Player> OnCreateEnemy;
    public Action<Player> OnRemoveEnemy;

    private ColyseusRoom<State> _room;
    private long _pingStartTime;

    public bool IsInit { get; private set; }

    protected override void Start()
    {
        IsInit = false;
        base.Start();
        Initialized().Forget();
    }

    //тут хотелосьс бы узнать о порядке срабатывания евентов у room, какие могут быть подводные камни, просто мы как ждем пока нам придет экземпляр ,
    //и только потом подписываемся, может ли произойти такое что евент OnStateChange мы пропустим.. не совсем понял как он работает. и евент OnJoint что делает.
    private async UniTaskVoid Initialized()
    {
        await UniTask.WaitUntil(() => Instance != null);

        Instance.InitializeClient();
        _room = await Instance.client.JoinOrCreate<State>("state_handler");

        _room.OnMessage<string>("pong", OnPongReceived);

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
            
            state.players.ForEach(ForEachEnemysCreate);

            state.players.OnAdd += ForEachEnemysCreate;
            state.players.OnRemove += RemoveEnemy;
        }
    }

    private void ForEachEnemysCreate(string key, Player enemy)
    {
        if (key == _room.SessionId) return;

        OnCreateEnemy?.Invoke(enemy);
    }

    private void OnErrorRoomHandler(int code, string message)
    {
        Debug.Log($"[{this.name}]code : {code}, message: {message}");
    }

    private void RemoveEnemy(string key, Player player)
    {
        OnRemoveEnemy?.Invoke(player);
    }

    public void SendMessage(string key , Dictionary<string , object> data)
    {
        _room.Send(key,data);
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

    protected override void OnDestroy()
    {
        _room.OnError -= OnErrorRoomHandler;
        _room.OnStateChange -= OnChangeRoomHandler;

        base.OnDestroy();
        _room.Leave();
    }
}
