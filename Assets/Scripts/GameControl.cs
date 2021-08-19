using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Newtonsoft.Json;

public class GameControl : MonoBehaviour 
{
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.
	public GameObject gameOvertext;				//A reference to the object that displays the text which appears when the player dies.

	private int score = 0;						//The player's score.
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -1.5f;

	public GameObject geneticAlgorithmGA;
	public GeneticAlgorithmController GAC;


	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null) {
			//...set this one to be it...
			instance = this;
			Initialize();
        }
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}

    private void Initialize() {
		geneticAlgorithmGA = GameObject.Find("GeneticAlgorithm");
		DontDestroyOnLoad(geneticAlgorithmGA);
		if (!geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().IsInitialized) {
			geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().Initialize(10, 4);
        } else {
			geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().ReInitialize();
		}
	}

    void Update()
	{
		/* Player stuff
		//If the game is over and the player has pressed some input...
		if (gameOver && Input.GetMouseButtonDown(0)) 
		{
			//...reload the current scene.
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		*/
		if (geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().bestFitnessCurrentPopulation > 3000f && !gameOver) {
			//END GAME - perfect Bird found
			gameOver = true;
			gameOvertext.SetActive(true);
			string json = JsonConvert.SerializeObject(geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().getFinalStats(), Formatting.Indented);
			Debug.Log(json);
			using (StreamWriter file = File.CreateText(Application.dataPath + @"\FinalStats.json")) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().getFinalStats());
			}
		}
	}

	public void RestartScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		/* player code
		score++;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
		*/
		scoreText.text = "Score: " + geneticAlgorithmGA.GetComponent<GeneticAlgorithmController>().bestScoreCurrentPopulation.ToString();
	}

	public void BirdDied()
	{
		//Activate the game over text.
		gameOvertext.SetActive (true);
		//Set the game to be over.
		gameOver = true;
	}
}
