using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	public float upForce;					//Upward force of the "flap".
	private bool isDead = false;			//Has the player collided with a wall?

	private Animator anim;					//Reference to the Animator component.
	private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.

	/// ///////////
	FlappyBird.NeuralNetwork neuralNetwork;

	float fitness;
	int score;
	bool isWinner = false;
	float timeSinceSpawned;

	public float Fitness { get => fitness; set => fitness = value; }
	public int Score { get => score; set => score = value; }
	public bool IsWinner { get => isWinner; set => isWinner = value; }
    public FlappyBird.NeuralNetwork NeuralNetwork { get => neuralNetwork; set => neuralNetwork = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    void Start()
	{
		//Get reference to the Animator component attached to this GameObject.
		anim = GetComponent<Animator> ();
		//Get and store a reference to the Rigidbody2D attached to this GameObject.
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		//Don't allow control if the bird has died.
		if (IsDead == false) 
		{
			timeSinceSpawned += Time.deltaTime;
			fitness = timeSinceSpawned;

			/* old code for player
			//Look for input to trigger a "flap".
			if (Input.GetMouseButtonDown(0)) 
			{
				//...tell the animator about it and then...
				anim.SetTrigger("Flap");
				//...zero out the birds current y velocity before...
				rb2d.velocity = Vector2.zero;
				//	new Vector2(rb2d.velocity.x, 0);
				//..giving the bird some upward force.
				rb2d.AddForce(new Vector2(0, upForce));
			}*/
		}
	}

	public void DoFlap() {
		//...tell the animator about it and then...
		anim.SetTrigger("Flap");
		//...zero out the birds current y velocity before...
		rb2d.velocity = Vector2.zero;
		//	new Vector2(rb2d.velocity.x, 0);
		//..giving the bird some upward force.
		rb2d.AddForce(new Vector2(0, upForce));
	}

	public void AddNeuralNetwork(int index, int inputLayerCount, int hiddenLayerCount, int outputLayerCount) {
		NeuralNetwork = new FlappyBird.NeuralNetwork(new int[] { inputLayerCount, hiddenLayerCount, outputLayerCount });
		fitness = 0;
		score = 0;
		timeSinceSpawned = 0;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.name.Contains("Bird")) {
			// Zero out the bird's velocity
			rb2d.velocity = Vector2.zero;
			// If the bird collides with something set it to dead...
			IsDead = true;
			//...tell the Animator about it...
			anim.SetTrigger ("Die");
			//...and tell the game control about it.
			//GameControl.instance.BirdDied ();
        }
	}
}
