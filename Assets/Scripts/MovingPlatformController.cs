using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
	public float XMin = 10f;
	public float XMax = 20f;
	public float moveSpeed = 0.1f;
	
	private float startPositionX;
	private bool isMovingRight = true;
	private Rigidbody2D rigidBody;
	
	
	void Awake()
	{
		startPositionX = this.transform.position.x;
		rigidBody = GetComponent<Rigidbody2D>();
	}		
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(isMovingRight)
		{
			if(this.transform.position.x < startPositionX + XMax)
				MoveRight();
			else
			{
				isMovingRight = false;
				MoveLeft();
			}
		}
		else
		{
			if (this.transform.position.x > startPositionX - XMin)
				MoveLeft();
			else
			{
				isMovingRight = true;
				MoveRight();
			}
			
		}
        
    }
	
	void MoveLeft()
	{
		if(rigidBody.velocity.x > -moveSpeed)
		{
			rigidBody.velocity = new Vector2 (-moveSpeed, rigidBody.velocity.y);
			rigidBody.AddForce(Vector2.left * 9.6f, ForceMode2D.Impulse);
		}
	}

	void MoveRight()
	{
		if(rigidBody.velocity.x < moveSpeed)
		{
			rigidBody.velocity = new Vector2 (-moveSpeed, rigidBody.velocity.y);
			rigidBody.AddForce(Vector2.right * 9.6f, ForceMode2D.Impulse);
		}
	}
}
