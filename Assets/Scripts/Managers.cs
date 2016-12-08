using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(WeatherManager))]
[RequireComponent(typeof(ImagesManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : MonoBehaviour {
    public static PlayerManager Player {get; private set;}
    public static InventoryManager Inventory {get; private set;}
    public static WeatherManager Weather {get; private set;}
    public static ImagesManager Images {get; private set;}
    public static MissionManager Mission {get; private set;}
    public static DataManager Data {get; private set;}

    private List<IGameManager> _startSequence;

    void Awake() {
        DontDestroyOnLoad(gameObject);//用于让对象在场景之间持久化

        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Weather = GetComponent<WeatherManager>();
        Images = GetComponent<ImagesManager>();
        Mission = GetComponent<MissionManager>();
        Data = GetComponent<DataManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
        _startSequence.Add(Inventory);
        //_startSequence.Add(Weather);
        _startSequence.Add(Images);
        _startSequence.Add(Mission);
        _startSequence.Add(Data);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {
        NetworkService network = new NetworkService();
        foreach (IGameManager manager in _startSequence) {
            manager.Startup(network);
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence) {
                if (manager.status == ManagerStatus.Started) {
                    numReady++;
                }
            }
            if (numReady > lastReady) {
                Debug.Log("Progress: " + numReady + "/" + numModules);
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);//Startup事件根据事件广播数据
            }
            yield return null;
        }
        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }
}
