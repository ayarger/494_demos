/* A component controlling the behavior of the Aesthetic aspect of EECS494FunBall */

/* A note on separation of visuals and controller
 * 
 * It's often prudent to separate a GameObject from its visuals. Doing so allows
 * the visuals of the object to deform, re-shape, alter, etc, without effecting the real
 * object or its colliders.
 * 
 * For example, the 494FunBall appears to get temporarily larger when the player touches it.
 * If the actual object ACTUALLY got larger, then its colliders would too, causing the player
 * and ball to overlap, resulting in physics glitches and oddities.
 * To prevent this, In our current setup, the 494FunBall never actually resizes, but the object representing it
 * visually (FunBallAesthetic) DOES get larger. This aesthetic has no colliders, and doesn't 
 * effect gameplay in any way. Thus we have decoupled the game from its visuals in a manner
 * akin to the MVC pattern.
 * https://www.wikiwand.com/en/Model%E2%80%93view%E2%80%93controller
 * 
 * - AY
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EECS494FunBallAesthetics : MonoBehaviour {

    /* Inspector Tunables */
    public EECS494FunBallController target;

    /* Private Data */
    Material mat;
    Color original_color;

    // Use this for initialization
    void Start() {
        GameObject cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = Camera.main.transform.position;
        Camera.main.gameObject.AddComponent<ScreenShakeEffect>();
        Camera.main.transform.parent = cameraContainer.transform;

        // Register a callback, so this aesthetic object knows when the target
        // object collided with something.
        // - AY
        if (target != null)
            target.collision_callbacks.Add(CollisionCallback);
        else
            Debug.LogError("target of EECS494FunBallAesthetic has yet to be defined in the inspector");

        mat = GetComponent<Renderer>().material;
        original_color = mat.color;
    }

    void Update() {
        MatchTarget();

        ProcessScale();
        ProcessColor();
    }

    /* Move and rotate this aesthetic to match that of the invisible ball. */
    void MatchTarget() {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
    }

    /* Ensure the ball return to normal size over time */
    void ProcessScale() {
        if (transform.localScale.x > 1.0f) {
            transform.localScale -= Vector3.one * 0.1f;
        }
        else
            transform.localScale = Vector3.one;
    }

    /* Ensure the ball returns to normal color over time */
    void ProcessColor() {
        mat.color = Color.Lerp(mat.color, original_color, 0.1f);
    }

    void CollisionCallback(Collision coll) {
        mat.color = Color.red;
        transform.localScale = Vector3.one * 1.5f;
        ScreenShakeEffect.Shake(.3f, 0.05f * coll.relativeVelocity.magnitude);
    }

    void OnDestroy() {
        // Un-register our collision callback to prevent performance leaks.
        // - AY
        target.collision_callbacks.Remove(CollisionCallback);
    }
}
