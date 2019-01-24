using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    // reminder : "public static" means this variable is associated only with the class "GameManager", and no "GameManager" instances.
    // this means there is only one copy of this variable.
	public static GameManager instance;

    /* Public tunables. These show up in the inspector, and may be modified */
	public Text scoreText;						
	public GameObject gameOvertext;
    public float scrollSpeed = -1.5f;

    /* This data is inherent to the single GameManager, so we can make it static and easier to access within this class */
    static int score = 0;
    static int highscore = 0;
	static bool gameOver = false;

    /* Delegates : Essentially, this allows us to turn a function signature into a data type */
    public delegate void VoidIntParam(int f);
    public delegate void VoidNoParam();

    /* Here, the delegate 'data types' above are used to define some callbacks / events for the GameManager system */
    public static VoidIntParam on_player_scored;
    public static VoidNoParam on_game_end;
    public static VoidNoParam on_game_reset;

    void Awake()
	{
        // Typical singleton code
        if(instance != null && instance != this)
        {
            // If a GameManager already exists, we need to go away.
            Destroy(gameObject);
            return;
        } else
        {
            // If we are the first GameManager to exist, we claim the "instance" variable which makes others go away.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Reset();
	}

	void Update()
	{
		//If the game is over and the player has pressed some input...
		if (gameOver && Input.GetMouseButtonDown(0)) 
		{
            Reset();
        }
	}

    static void Reset()
    {
        SetScore(0);
        gameOver = false;
        instance.gameOvertext.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(on_game_reset != null)
            on_game_reset();
    }

    public void EndGame()
    {
        gameOver = true;
    }

    public static bool IsGameOver()
    {
        return gameOver;
    }

    public void BirdScored()
	{
        SetScore(score+1);

        // A player has scored, and there may be other systems who want to receive that event.
        // By "calling" our on_player_scored delegate and giving it the new score, every system that has registered for the
        // "on_player_scored" event will receive that information (such as UserMessageManager's _OnScored function).
        if (on_player_scored != null)
            on_player_scored(score);
	}

    static void SetScore(int s)
    {
        score = s;

        // Update the UI
        // Recall-- while the "score" variable is static and associated with the GameManager class,
        // the "scoreText" variable is associated with the actual single instance of the GameManager class (public fields appear in the inspector, whereas public static ones do not).
        instance.scoreText.text = "Score: " + score.ToString();
    }

	public static void BirdDied()
	{
		//Activate the game over text.
		instance.gameOvertext.SetActive (true);

		//Set the game to be over.
		gameOver = true;

        // "Launch" the "on_game_end" event by calling our delegate.
        if (on_game_end != null)
            on_game_end();
    }
}
