/* A component that implements the behavior of the 494 FunBall */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EECS494FunBallController : MonoBehaviour {

    // Function objects take the form of "delegates" in C#.
    // Should you wish to avoid the inconvenience of declaring a new
    // delegate data-type (as done in the line below), you can use an Action,
    // which is essentially a lambda.
    /* Action examples:
     * // Be sure to add "using System;" to your includes.
     * Action print_lambda = () => { print("Hello Action!"); };
     * Action<bool> a = (bool b) => { print("the bool's value is: " + b.ToString()); };
     */
    // - AY

    public delegate void VoidFunctionCollisionParam(Collision coll);

    // A list that contains all the listeners waiting for a collision event.
    public List<VoidFunctionCollisionParam> collision_callbacks = new List<VoidFunctionCollisionParam>();

    Rigidbody rb;
    public float acceleration = 1;

    void Awake() {
        rb = this.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        // Lock ball to the x-y plane.
        // This can be done far easier in the inspector (Rigidbody component).
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }

    void Update() {
        rb.velocity += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * acceleration * Time.deltaTime;
    }

    /* Detect collision and report to listeners */
    private void OnCollisionEnter(Collision collision)
    {
        foreach (VoidFunctionCollisionParam f in collision_callbacks)
            f(collision);
    }
}
