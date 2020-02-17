using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public GameObject prefab;
    public float seconds_between_spawns = 2.0f;
    public Vector3 initial_velocity = Vector3.zero;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        timer = seconds_between_spawns;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            Spawn();
            timer = seconds_between_spawns;
        }
    }

    void Spawn()
    {
        GameObject new_object = GameObject.Instantiate(prefab, transform.position + UnityEngine.Random.insideUnitSphere * 0.1f, transform.rotation);

        if (new_object.GetComponent<Rigidbody>() != null)
        {
            new_object.GetComponent<Rigidbody>().velocity = initial_velocity;
        }
    }
}
