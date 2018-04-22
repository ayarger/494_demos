using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeEffect : MonoBehaviour {

    public static ScreenShakeEffect instance;

    // Use this for initialization
    void Start () {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
            instance = this;
    }
    public static void Shake(float duration, float radius) {
        instance.StartCoroutine(instance.ShakeEffect(duration, radius));
    }

    public IEnumerator ShakeEffect(float duration, float radius) {
        for (float t = 0; t < duration; t += Time.deltaTime) {
            transform.localPosition = UnityEngine.Random.onUnitSphere * radius;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
}
