using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Root;
    public static EventManager Event;
    public static GameManager Game;
    public static InstanceManager Instance;
    public static DataManager Data;
    public static SceneManager Scene;
    public static UIManager UI;
    public static AudioManager Audio;

    private T Init<T>() where T : ManagerBase
    {
        T manager = Util.GetOrAddComponent<T>(this.gameObject);
        if (manager == null)
        {
            Debug.LogError("Can't Get Manager Component");
            return null;
        }
        manager.Init();
        return manager;
    }

    public void Init()
    {
        Event = Init<EventManager>();
        Game = Init<GameManager>();
        Instance = Init<InstanceManager>();
        Data = Init<DataManager>();
        UI = Init<UIManager>();
        Audio = Init<AudioManager>();
        Scene= Init<SceneManager>();
    }
}
