using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{

    public float moveSpeed = 0.1f;
    public float jumpForce = 6f;
    private Rigidbody2D rigidbody;
	
	public LayerMask groundLayer;
	public Animator animator;
	private bool isWalking;
	
	private int score = 0;
	
	private Vector2 startPosition;
	private float killOffset=0.2f;
	
	private int lives = 3;
    private int scrollNumber = 0;
    private int maxScrollNumber = 3;
	
	private AudioSource source;
	public AudioClip coinSound;
	public AudioClip heartSound;
	public AudioClip scrollSound;
	public AudioClip fallSound;
	public AudioClip goblinDeathSound;
	
	public bool leftClicked = false;
	public bool rightClicked = false;
	
	
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
		source = GetComponent<AudioSource>();
		startPosition = this.transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
		
		if(GameManager.instance.currentGameState == GameManager.GameState.GS_GAME)
		{
			isWalking = false;
					
			if (Input.GetKeyDown("space") /*|| Input.GetKey(KeyCode.W) ||  Input.GetKey(KeyCode.UpArrow)*/)
			{
				if(transform.parent != null)
					Unlock();
				Jump();
			}
			
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || rightClicked)
			{
				if(transform.parent != null)
					Unlock();
				
				isWalking = true;
				transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
				
				if (transform.localScale.x < 0)
				{
					Vector3 newScale = transform.localScale;
					newScale.x *= -1;
					transform.localScale = newScale;
				}

			}
			else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || leftClicked)
			{
				if(transform.parent != null)
					Unlock();
				
				isWalking = true;
				transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
				
				if (transform.localScale.x > 0)
				{
					Vector3 newScale = transform.localScale;
					newScale.x *= -1;
					transform.localScale = newScale;
				}

			}

			animator.SetBool("isGrounded", isGrounded());  
			animator.SetBool("isWalking", isWalking);
			
			GameManager.instance.updateClock();
		}
    }
	
	
	bool isGrounded(){
		return Physics2D.Raycast(this.transform.position, Vector2.down, 2f, groundLayer.value);
	}
	
	public void Jump()
	{
			if(isGrounded())
			{
				rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Coin")){
			source.PlayOneShot(coinSound, AudioListener.volume);
			GameManager.instance.addCoins(1);
			score += 1;
			Debug.Log ("Score: " + score);
			other.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Enemy"))
		{
			if(other.gameObject.transform.position.y + killOffset < this.transform.position.y)
			{
				source.PlayOneShot(goblinDeathSound, AudioListener.volume);
				score += 10;
				Debug.Log("Killed an enemy! Score: " + score);
				GameManager.instance.addGoblins();
			}
			else
			{
				source.PlayOneShot(fallSound, AudioListener.volume);
				lives -= 1;
				GameManager.instance.lostHeart(lives);
				if (lives <= 0)
				{
					Debug.Log("Game Over");
					//GameManager.instance.GameState = 
					//GameOver();
				}
				else
					Debug.Log("You got hit! Lives: " + lives);
				this.transform.position = startPosition;
			}
		}
		else if (other.CompareTag("Scroll"))
		{
			source.PlayOneShot(scrollSound, AudioListener.volume);
			GameManager.instance.addScrolls(scrollNumber);
			scrollNumber += 1;
			Debug.Log("Scrolls: " + scrollNumber);
			other.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Heart"))
		{
			source.PlayOneShot(heartSound, AudioListener.volume);
			Debug.Log("You collected a heart, you gain 1 life extra");
			lives += 1;
			GameManager.instance.addHearts(1);
			other.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Exit"))
		{
			if(scrollNumber == maxScrollNumber)
			{
				Debug.Log("Congratulations!!!");
				GameManager.instance.LevelCompleted();
			}
			else
				Debug.Log("Not enough scrolls to enter");
			
		}
		else if (other.CompareTag("MovingPlatform"))
		{
			;
		}
		else if (other.CompareTag("FallLevel"))
		{
			source.PlayOneShot(fallSound, AudioListener.volume);
			//Debug.Log("You fell");
			//this.transform.position = startPosition;
			
			
			lives -= 1;
			GameManager.instance.lostHeart(lives);
			if (lives <= 0)
			{
				Debug.Log("Game Over");
				//GameManager.instance.GameState = 
				//GameOver();
			}
			else
				Debug.Log("You fell! Lives: " + lives);
			
			this.transform.position = startPosition;
			
			//GameManager.instance.GameOver();
			//GameManager.instance.OnRestartButtonClicked();
		}
				
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.CompareTag("MovingPlatform"))
		{
			rigidbody.isKinematic = true;
			transform.parent = other.transform;
		}
	}
	
	private void Unlock()
	{
		rigidbody.isKinematic = false;
		transform.parent = null;
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("MovingPlatform"))
		{
			Unlock();
		}
		
	}
	
	public void setLeftClicked(bool clicked)
	{
		leftClicked = clicked;
	}
	
	public void setRightClicked(bool clicked)
	{
		rightClicked = clicked;
	}
	
}
