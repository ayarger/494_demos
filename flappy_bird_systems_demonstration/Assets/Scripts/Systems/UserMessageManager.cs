using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMessageManager : MonoBehaviour {

    // Remember, marking this variable as "static" associated it with the class, rather than the instances.
    // Because of this, there is ever only one copy of this variable, which makes it useful to us.
    static UserMessageManager instance;

    static int high_score = 0;

    /* Singleton Formation */
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            // If there already exists a UserMessageManager, we need to go away.
            Destroy(gameObject);
            return;
        } else
        {
            // If we are the first UserMessageManager, we become the "instance".
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start () {

        // Here, we tell the "GameControl" system that we would like to receive "on_player_scored" events.
        // These events will be delivered to our _OnScored function when they occur.
        GameManager.on_player_scored += _OnScored;
    }

    void OnDestroy()
    {
        // It's important to "deregister" our callback function if we aren't going to use it again.
        // If we fail to do this, it creates a memory leak. Over time, this kind of bug can slow down performance.
        GameManager.on_player_scored -= _OnScored;
    }

    void _OnScored(int score)
    {
        if (score > high_score)
        {
            high_score = score;
            ToastManager.Toast("New High Score: " + high_score.ToString());
        }
    }
}
