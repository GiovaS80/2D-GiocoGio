using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField]
    GameObject topPanel;

    [SerializeField]
    GameObject startPanel, playMsg, nowMsg, againMsg, nextLevelMsg;

    [SerializeField]
    GameObject startMsgPanel, gameOverPanel, gameWonPanel, pauseMsgPanel, quitPanel1, quitPanel2;

    [SerializeField]
    GameObject levelByUserPanel;

    [SerializeField]
    TextMeshProUGUI pointDisplay, levelDisplay, levelDisplayUser, wonMsg, lostMsg;

    [SerializeField]
    GameObject explosionEffect;

    [SerializeField]
    GameObject loadingScreenPanel;

    [SerializeField]
    CanvasGroup canvasGroup;

    public bool gameStarted, gameOver, gamePause, gameQuit, timerStartBall;

    public static int hitPoint, pointValue;

    int levelValue, levelsExceeded;
    int spawnerBricks;
    string levelMsg, pointMsg, msgToTheUser, quantityBricksMsg;

    Rigidbody2D ball;
    GameObject bar;

    private void Awake()
    {
        gameManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BallController.ballSpeed = 10f;
        gameStarted = false;
        gameOver = true;
        gamePause = false;
        gameQuit = false;
        timerStartBall = false;
        pointValue = 0;
        levelValue = 0;
        levelsExceeded = 0;
        spawnerBricks = 0;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingScreenPanel.transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        pointMsg = (pointValue <= 1) ? "Point" : "Points";
        levelMsg = (levelValue <= 1) ? "Level" : "Levels";
        pointDisplay.text = pointMsg + " = " + pointValue.ToString();
        levelDisplay.text = levelMsg + " = " + levelValue.ToString();
        levelDisplayUser.text = levelValue.ToString("D2");
    }
    /// <summary>
    /// 
    /// </summary>
    public void Increases()
    {//inizio funzione aumenta livello CHIAMATA dal pulsante dedicato
        if (levelValue < 50)
            levelValue++;
    }//fine funzione aumenta livello CHIAMATA dal pulsante dedicato

    public void Decreases()
    {//inizio funzione diminuisce livello CHIAMATA dal pulsante dedicato
        if (levelValue > 0)
            levelValue--;
    }//fine funzione diminuisce livello CHIAMATA dal pulsante dedicato

    public void StartTheGame()
    {//inizio funzione che mi fa partire il gioco CHIAMATA dal pulsante Start

        if(!gameStarted)
        {//inizio SE il gioco non e' in run
            if(gameOver)
            {//inizio SE in game over resetta punti. Di default e' in game over
                pointValue = 0;
                levelsExceeded = 0;
                gameOver = false;
                gameOverPanel.SetActive(gameOver);
                againMsg.SetActive(gameOver);
                StartTheGame();//chiama se stessa
            }//fine SE in game over resetta punti. Di default e' in game over
            else
            {//inizio else, SE NON in game over, prendi livello da utente ed inizia il gioco
                CheckTheGameLevel();

                startPanel.SetActive(false);
                quitPanel1.SetActive(false);
                playMsg.SetActive(false);
                nowMsg.SetActive(false);
                startMsgPanel.SetActive(false);
                levelByUserPanel.SetActive(false);
                gameWonPanel.SetActive(false);
                nextLevelMsg.SetActive(false);
                //disabilito i pannelli
                gameStarted = true;//e' iniziato il gioco
                topPanel.SetActive(gameStarted);
                if (SceneManager.GetSceneByName("GameScene").name == "GameScene")
                    SceneManager.UnloadSceneAsync(1, UnloadSceneOptions.None);
                
                StartCoroutine(LoadScene(1));

            }//fine else, SE NON in game over, prendi livello da utente ed inizia il gioco

        }//fine SE il gioco non e' in run

        if (gamePause)
        {//inizio if gioco e' in pausa
            gamePause = false;
            startPanel.SetActive(gamePause);
            playMsg.SetActive(gamePause);
            pauseMsgPanel.SetActive(gamePause);
            quitPanel1.SetActive(gamePause);
            Time.timeScale = 1;
        }//fine if gioco e' in pausa

    }//fine funzione che mi fa partire il gioco CHIAMATA dal pulsante Start

    void CheckTheGameLevel()
    {//inizio funzione che in base al livello, inserisco un numero di colonne / righe
        if(levelValue < 30)
        {
            BallController.ballSpeed = 10;
            hitPoint = (int)((levelValue + 1) % 3);
            //in base al livello, multiplo di 3, quantita' di colpi per distruggere un brick
            if (hitPoint == 0)//SE il modulo e' Zero, metti 3 colpi per distruggere un brick
                hitPoint = 3;
        }
        else
        {
            BallController.ballSpeed = levelValue - 19;
            hitPoint = 3;
        }
            
        if (levelValue < 3)
            BrickSpawner.cols = 3;
        else
            BrickSpawner.cols = 5;
        if (levelValue < 3)
            BrickSpawner.rows = 1;
        else if (levelValue > 2 && levelValue < 30)
            BrickSpawner.rows = (levelValue / 3);
        else
            BrickSpawner.rows = 9;
            
    }//fine funzione che in base al livello, inserisco un numero di colonne / righe

    IEnumerator LoadScene(int num)
    {//inizio funzione LoadScene
        loadingScreenPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(num, LoadSceneMode.Additive);

        while (!operation.isDone)
        {//inizio while
            yield return null;
        }//fine while
        StartCoroutine(LoadingScreenFadeOut(2f));

    }//fine funzione LoadScene

    IEnumerator LoadingScreenFadeOut(float duration)
    {//inizio funzione LoadingScreenFadeOut
        float timePassed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (timePassed < duration)
        {//inizio while
            timePassed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timePassed / duration);
            yield return null;
        }//fine while
        loadingScreenPanel.SetActive(false);
        canvasGroup.alpha = 1f;
        StartBall();
    }//fine funzione LoadingScreenFadeOut

    void StartBall()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
        ball.AddForce(Vector2.up);
    }

    public void GameInPause()
    {//inizio funzione Pausa CHIAMATA dal pulsante dedicato
        gamePause = true;
        startPanel.SetActive(gamePause);
        playMsg.SetActive(gamePause);
        pauseMsgPanel.SetActive(gamePause);
        quitPanel1.SetActive(gamePause);
        Time.timeScale = 0;
    }//fine funzione Pausa CHIAMATA dal pulsante dedicato

    public void QuitButton()
    {
        gameQuit = !gameQuit;
        quitPanel2.SetActive(gameQuit);
    }

    public void QuitGame()
    {//inizio funzione Esci CHIAMATA dal pulsante dedicato
        Application.Quit();
    }//fine funzione Esci CHIAMATA dal pulsante dedicato

    public void SetSpawnedBricks(int value)
    {//inizio funzione calcola bricks attivi CHIAMATA dallo Script BrickSpawner
        spawnerBricks = value;
    }//inizio funzione calcola bricks attivi CHIAMATA dallo Script BrickSpawner

    public void BrickDestroyed()
    {//inizio funzione conta brick distrutti CHIAMATA dallo Script BrickManager
        spawnerBricks--;
        if (spawnerBricks <= 0)
        {
            GameWon();
        }
    }//fine funzione conta brick distrutti

    void GameWon()
    {//inizio funzione vincita
        if (bar == null)
        {//inizio if trova la barra
            bar = GameObject.FindGameObjectWithTag("Bar");
        }//fine if trova la barra
        GameObject.Instantiate(explosionEffect, bar.transform.position, Quaternion.identity, bar.transform);
        bar.SetActive(false);
        Destroy(ball.gameObject);
        levelValue++;
        levelsExceeded++;
        gameWonPanel.SetActive(true);
        startPanel.SetActive(true);
        nextLevelMsg.SetActive(true);
        quitPanel1.SetActive(true);
        gameStarted = false;
        topPanel.SetActive(gameStarted);
        CheckTheGameLevel();
        quantityBricksMsg = (spawnerBricks <= 1) ? "Brick" : "Bricks";
        if (levelValue < 30)
            msgToTheUser = $"<sprite=2> \n CONGRATULATIONS \n You Have Scored {pointValue} {pointMsg} \n Now, Go To Level Number {levelValue}. \n In This Level, \n You Will Need To Destroy {(BrickSpawner.cols* BrickSpawner.rows)} {quantityBricksMsg}. \n To Destroy A brick, \n You Will Have To Hit It {hitPoint} Times.";
        else
            msgToTheUser = $"WOW <sprite=2> CONGRATULATIONS \n You Have Scored {pointValue} {pointMsg} \n Now,  Go To Level Number {levelValue}. \n From Now On, The Bricks Do Not Rise. \n They Are Always 45 Bricks \n And You Must Always Hit Them \n 3 Times. \n \n But The Speed Of The Ball Increases, \n From {BallController.ballSpeed - 1} To {BallController.ballSpeed}.";
        wonMsg.text = msgToTheUser;
    }//fine funzione vincita

    public void GameOver()
    {//inizio funzione game over CHIAMATA dallo Script BallController
        if (bar == null)
        {//inizio if trova la barra
            bar = GameObject.FindGameObjectWithTag("Bar");
        }//fine if trova la barra
        GameObject.Instantiate(explosionEffect, bar.transform.position, Quaternion.identity, bar.transform);
        levelValue = 0;
        bar.SetActive(false);
        gameOver = true;
        startPanel.SetActive(gameOver);
        playMsg.SetActive(gameOver);
        againMsg.SetActive(gameOver);
        gameOverPanel.SetActive(gameOver);
        levelByUserPanel.SetActive(gameOver);
        gameStarted = false;
        topPanel.SetActive(gameStarted);
        quitPanel1.SetActive(true);
        levelMsg = (levelsExceeded <= 1) ? "Level" : "Levels";
        msgToTheUser = $"You Have Passed {levelsExceeded} {levelMsg}. \n You Have Scored {pointValue} {pointMsg}.";
        lostMsg.text = msgToTheUser;
    }//fine funzione game over

}//END
