using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForPlayerInputToTransition());
    }

    IEnumerator WaitForPlayerInputToTransition()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 initial_position = transform.position;
                Vector3 final_position = new Vector3(20, 0, -10);

                /* Transition to new "room" */
                yield return StartCoroutine(
                    CoroutineUtilities.MoveObjectOverTime(transform, initial_position, final_position, 2.5f)
                );

                /* Hang around a little bit */
                yield return new WaitForSeconds(2);

                /* Return to the previous room */
                yield return StartCoroutine(
                    CoroutineUtilities.MoveObjectOverTime(transform, final_position, initial_position, 2.5f)
                );
            }

            /* We must yield here to let time pass, or we will hardlock the game (due to infinite while loop) */
            yield return null;
        }
    }
}
