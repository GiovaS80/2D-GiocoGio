using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    AudioSource ballSound, deathSound;

    [SerializeField]
    GameObject gamePanel;

    [SerializeField]
    Color runColor, pauseColor;

    public static float ballSpeed;//variabile controllata da GameManager

    Rigidbody2D body;
   SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.gamePause || GameManager.gameManager.gameOver)
        {//Inizio if di controllo da GameManager IF il gioco e' in Pausa o Game Over
            gamePanel.SetActive(false);
            sprite.color = pauseColor;
        }//fine if di controllo da GameManager IF il gioco e' in Pausa o Game Over
        else
        {//Inizio else il gioco NON e' in Pausa o Game Over
            gamePanel.SetActive(true);
            sprite.color = runColor;
        }//fine else il gioco NON e' in Pausa o Game Over
        
    }

    private void FixedUpdate()
    {
        KeepCostantVelocity();
    }

    void KeepCostantVelocity()
    {//inizio funzione che rende velocita palla costante
        body.velocity = ballSpeed * body.velocity.normalized;
    }//fine funzione che rende velocita palla costante

    private void OnCollisionExit2D(Collision2D collision)
    {
       ballSound.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {//inizio funzione rilevamento collisione palla pavimento
        if (collision.gameObject.tag == "Death")
        {//inizio if collisione
            GameManager.gameManager.GameOver();
            deathSound.Play();
        }//fine if collisione
    }//fine funzione rilevamento collisione palla pavimento



}//END
