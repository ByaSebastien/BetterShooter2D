using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    public static EventSystemManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            EventSystem currentEventSystem = GetComponent<EventSystem>();
            if (currentEventSystem != null)
            {
                Destroy(currentEventSystem);
            }
            
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
