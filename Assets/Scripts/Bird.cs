using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
    public Rigidbody2D rb;
    public AudioClip idleTheme, mainTheme, overTheme, clickTheme;
    AudioSource sound;
    SpriteRenderer oSpriteR;
    Animator anim;
    GameObject wings;
    GameObject tail;
    GameObject gObs;
    GameObject gPlat;
    Text scoreT;
    Text highScoreT;
    Vector3 bound;

    public GameObject prefabObstacle;
    public GameObject prefabPlatform;
    public GameObject prefabParticle;
    public GameObject p;
    public GameObject boundary;
    public Canvas mainCanvas;
    public Canvas menuCanvas;
    public int score = 0;
    public int highScore = 0;
    public float speed = 250f;

    float randomP = -3.7f;
    float randomH;
    int distance = 5;
    public bool gameOver = false;
    bool coStarted = false;
    bool spawnStarted = false;
    bool isRotated = false;
    bool isBodyRotated = true;
    bool topause = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sound = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
        scoreT = mainCanvas.transform.Find("Score").gameObject.GetComponent<Text>();
        highScoreT = menuCanvas.transform.Find("HighScore").gameObject.GetComponent<Text>();
        highScoreT.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();

        wings = transform.Find("Wings").gameObject;
        tail = transform.Find("Tail").gameObject;

        //Setting boundary for bird.
        bound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        boundary.transform.position = new Vector3(0f, bound.y + 0.5f, 0f);
        boundary.GetComponent<BoxCollider2D>().size = new Vector2(bound.x * 2, 1f);

        rb.simulated = false;
        //StartCoroutine(Obstacles());
    }


    // Update is called once per frame
    void Update()
    {

        WingsAndTail();

        //Score update
        scoreT.text = score.ToString();

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreT.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }

        /*
        //to delete before build
        if (Input.GetKey(KeyCode.R))
        {
            PlayerPrefs.DeleteKey("HighScore");
            highScoreT.text = "0";
            Debug.Log("Done "+ PlayerPrefs.GetInt("HighScore",0));
        }
        */

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "PauseButton" || EventSystem.current.currentSelectedGameObject.transform.parent.name == menuCanvas.name)
            {
                topause = false;
            }
        }
        else
        {
            topause = true;
        }

        //Resuming Sound
        if (!gameOver && sound.clip != mainTheme && !sound.isPlaying && Time.timeScale == 1)
            {
                sound.loop = true;
                if(!rb.simulated)
                {
                    sound.clip = idleTheme;    
                }
                else
                {
                    sound.clip = mainTheme;
                }
                sound.Play();
            }

        //Player Controls
        if (Input.GetKey(KeyCode.Space) && !gameOver || Input.GetMouseButton(0) && !gameOver && topause)
        {
            if (!rb.simulated)
            {
                rb.simulated = true;
                anim.enabled = false;
                isRotated = false;
                sound.clip = mainTheme;
                sound.Play();
                if (!coStarted)
                {
                    StartCoroutine(SpawnPipe());
                }
                else
                {
                    coStarted = true;
                }

                if (!spawnStarted)
                {
                    StartCoroutine(SpawnPlatform());
                }
                else
                {
                    spawnStarted = true;
                }
            }

            rb.velocity = new Vector3(0, 5, 0);

            if (transform.rotation != Quaternion.identity)
            {
                isBodyRotated = true;
            }



        }

        if (transform.rotation == Quaternion.identity && rb.simulated)
        {
            isBodyRotated = false;
        }

        if (!isBodyRotated && !gameOver)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -60), 50f * Time.deltaTime);
        }
        else if (isBodyRotated && !gameOver)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, speed * Time.deltaTime);
        }

    }

    void WingsAndTail()
    {
        //Wing Flaps
        if (!isRotated && !gameOver)
        {
            wings.transform.localRotation = Quaternion.RotateTowards(wings.transform.localRotation, Quaternion.Euler(0, 0, -60), speed * Time.deltaTime);
            tail.transform.localRotation = Quaternion.RotateTowards(tail.transform.localRotation, Quaternion.Euler(0, 0, 60), speed * Time.deltaTime);

            if (wings.transform.localRotation == Quaternion.Euler(0, 0, -60))
            {
                isRotated = true;
            }
        }
        else if (isRotated && !gameOver)
        {
            wings.transform.localRotation = Quaternion.RotateTowards(wings.transform.localRotation, Quaternion.identity, speed * Time.deltaTime);
            tail.transform.localRotation = Quaternion.RotateTowards(tail.transform.localRotation, Quaternion.identity, speed * Time.deltaTime);

            if (wings.transform.localRotation == Quaternion.identity)
            {
                isRotated = false;
            }
        }
    }

    //Spawn Pipes
    IEnumerator SpawnPipe()
    {
        while (true)
        {

            if (rb.simulated && !gameOver)
            {
                //Random pipe scaler
                if (Random.value <= 0.3f)
                {
                    randomH = 1f;
                }
                else if (Random.value <= 0.6f)
                {
                    randomH = 2f;
                }
                else
                {
                    randomH = 2.5f;
                }

                //Random pipe positioner
                if (randomP == 3.8f)
                {
                    randomP = -(3.8f - 0.5f);
                    gObs = Instantiate(prefabObstacle, new Vector2(11f, randomP), Quaternion.Euler(0, 0, 180));

                }
                else
                {
                    randomP = 3.8f;
                    gObs = Instantiate(prefabObstacle, new Vector2(11f, randomP), Quaternion.identity);

                }

                //Scale sprites using 9-slicing method to avoid stretching
                oSpriteR = gObs.GetComponent<SpriteRenderer>();
                oSpriteR.size = new Vector2(1f, randomH);
                gObs.transform.SetParent(p.transform);


            }
            if (score == 3)
            {
                distance = 3;
            }
            else if (score == 5)
            {
                distance = 2;
            }
            yield return new WaitForSeconds(distance);

        }


    }

    //Spawn Platforms
    IEnumerator SpawnPlatform()
    {
        while (true)
        {
            if (rb.simulated && !gameOver)
            {
                gPlat = Instantiate(prefabPlatform, new Vector3(prefabPlatform.transform.position.x + 39.2f, prefabPlatform.transform.position.y, 0f), Quaternion.identity);
                gPlat.transform.SetParent(transform.parent);
            }
            yield return new WaitForSeconds(19.6f);
        }
    }

    //Game Over & stops Pipes & bird
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.name == "Ground")
        {
            gameOver = true;
            Instantiate(prefabParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            mainCanvas.transform.Find("GameOver").gameObject.SetActive(true);

            if (sound.clip != overTheme)
            {
                sound.loop = false;
                sound.clip = overTheme;
                sound.Play();
            }


            //Finds & Stops all pipes &platforms in parent
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).tag == "Enemy" || gObs.transform.parent.GetChild(i).tag == "Platform")
                {
                    transform.parent.GetChild(i).GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }

        //Stops bird after falling on platform to create nice falling effect
        if (col.gameObject.tag == "Platform")
        {
            rb.simulated = false;
            sound.clip = overTheme;
            sound.Play();
        }
    }

    //Score Counter exclude platform
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.tag != "Platform" && !gameOver)
        {

            score = score + 1;
        }
    }

    public void ClickSounds()
    {
        sound.clip = clickTheme;
        sound.loop = false;
        sound.Play();
    }

}
