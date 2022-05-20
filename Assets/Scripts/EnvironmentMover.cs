using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    Bird bird;
    Rigidbody2D rb;
    Vector3 bound;
    public float moveSpeed = -2f;
    public float extraDistance = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        bound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0f));
        bird = transform.parent.Find("Bird").GetComponent<Bird>();

    }

    // Update is called once per frame
    void Update()
    {   
        
        if(bird.rb.simulated && !bird.gameOver)
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
        }

        if (transform.position.x < -(bound.x + extraDistance))
        {
            Destroy(gameObject);
        }
    }

    
    
}
