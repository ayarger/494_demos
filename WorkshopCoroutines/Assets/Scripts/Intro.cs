using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    CanvasGroup cg;
    public AudioClip typing_sound_clip;
    public AudioClip background_music_clip;

    public Text t;

	// Use this for initialization
	void Start () {
        cg = GetComponent<CanvasGroup>();
        StartCoroutine(DoIntro());
	}

    IEnumerator DoIntro()
    {
        // Make canvas fully visible
        cg.alpha = 1.0f;

        // Dramatic pause
        yield return new WaitForSeconds(1.0f);

        // Perform type-text effect.
        yield return StartCoroutine(TypeTextEffect("Hello from Hyrule!", 0.01f));

        // Dramatic pause.
        yield return new WaitForSeconds(1.0f);

        AudioSource.PlayClipAtPoint(background_music_clip, Camera.main.transform.position);

        // Dramatic pause.
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(FadeOutCanvas(1.0f));
    }

    IEnumerator TypeTextEffect(string desired_text, float delay_between_characters_sec)
    {
        // Get reference to Text component on the Text gameobject.
        Text text_c = transform.Find("Panel").Find("Text").GetComponent<Text>();

        // Clear text in the component.
        text_c.text = "";

        // Loop forever until our desired text is displayed.
        while(true)
        {
            char next_char = desired_text[text_c.text.Length];
            text_c.text += next_char;

            if(next_char != ' ')
                AudioSource.PlayClipAtPoint(typing_sound_clip, Camera.main.transform.position);

            if (text_c.text.Length >= desired_text.Length)
                break;

            // This "yield" statement prevents the entire game from freezing.
            // During this "pause", other code may execute.
            yield return new WaitForSeconds(delay_between_characters_sec);
        }
    }

    IEnumerator FadeOutCanvas(float fade_duration_sec)
    {
        float start_time = Time.time;

        // This calculation gives us a float between 0.0 and 1.0.
        // It will progress from 0.0 to 1.0 in an amount of time equal to "fade_duration_sec".
        float progress = (Time.time - start_time) / fade_duration_sec;

        while(progress < 1.0f)
        {
            // recalculate progress now that time has passed.
            // Use it to set the transparency of our canvas.
            progress = (Time.time - start_time) / fade_duration_sec;
            cg.alpha = 1.0f - progress;

            // without this line, we get an infinite loop.
            // "yield" allows execution to "yield" to other code in other functions.
            yield return null;
        }

        // Ending condition.
        cg.alpha = 0.0f;
    }
}
