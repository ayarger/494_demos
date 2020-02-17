using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateView : MonoBehaviour
{
    public GameObject view_prefab;
    GameObject current_view;

    // Start is called before the first frame update
    void Awake()
    {
        current_view = GameObject.Instantiate(view_prefab, transform.position, transform.rotation);
        current_view.GetComponent<FollowTarget>().target = transform;
    }

    public GameObject GetCurrentView()
    {
        return current_view;
    }
}
