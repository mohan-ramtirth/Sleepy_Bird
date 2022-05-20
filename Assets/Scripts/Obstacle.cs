using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 bound;

    public float moveSpeed = -2f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        bound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0f));

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
        
        if (transform.position.x < -(bound.x + 1f))
        {
            Destroy(gameObject);
        }
    }
    
}
