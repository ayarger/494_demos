using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdCanvas : MonoBehaviour {

    static FlappyBirdCanvas instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
