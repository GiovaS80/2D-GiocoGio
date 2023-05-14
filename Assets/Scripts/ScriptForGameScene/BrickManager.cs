using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField]
    Color oneLifeColor, twoLifeColor, threeLifeColor;
    [SerializeField]
    ParticleSystem brickHitParticle;

    int hitPoints;

    SpriteRenderer sprite;

    AudioSource brickHitSound;


    // Start is called before the first frame update
    void Start()
    {
        hitPoints = GameManager.hitPoint;                                                     
        sprite = GetComponent<SpriteRenderer>();
        ChangeColorOnLife();
        brickHitSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {//inizio funzione controllo collisione
        if(collision.gameObject.tag == "Ball")
        {//inizio if rilevamento collisione con tag Ball
            hitPoints--;
            GameManager.pointValue++;
            ChangeColorOnLife();
            brickHitSound.Play();
            brickHitParticle.Play();

            Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y);

            Vector3 ballDir = collision.gameObject.transform.position - collisionPoint;
            brickHitParticle.transform.rotation = Quaternion.LookRotation(ballDir.normalized, Vector3.back);
            brickHitParticle.transform.position = collisionPoint;

            if (hitPoints <= 0)
            {
                GameManager.gameManager.BrickDestroyed();                                         
                GetComponent<Renderer>().enabled = false;
                GetComponent<CapsuleCollider2D>().enabled = false;
                Destroy(gameObject, 0.3f);
            }
        }//fine if rilevamento collisione con tag Ball
    }//fine funzione controllo collisione

    void ChangeColorOnLife()
    {//inizio funzione cambia colore al mio brick
        switch (hitPoints)
        {//inizio switch
            case 2:
                sprite.color = twoLifeColor;
                break;
            case 3:
                sprite.color = threeLifeColor;
                break;
            default:
                sprite.color = oneLifeColor;
                break;
        }//fine switch

        ParticleSystem.MainModule particleModule = brickHitParticle.main;
        particleModule.startColor = sprite.color;
    }//fine funzione cambia colore al mio brick

}//END
