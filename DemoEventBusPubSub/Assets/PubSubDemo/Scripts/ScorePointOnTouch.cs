using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointOnTouch : MonoBehaviour
{
    static int total_score = 0;

    void OnTriggerEnter(Collider other)
    {
        total_score++;
        EventBus.Publish<ScoreEvent>(new ScoreEvent(total_score));    
    }
}

public class ScoreEvent
{
    public int new_score = 0;
    public ScoreEvent(int _new_score) { new_score = _new_score; }

    public override string ToString()
    {
        return "new_score : " + new_score;
    }
}