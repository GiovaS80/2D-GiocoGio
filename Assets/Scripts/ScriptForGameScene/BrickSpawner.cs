using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject brickPrefab;

    public static int rows;
    public static int cols;

    float xDistanceBetweenBricks, yDistanceBetweenBricks;
    float xPositionBrickSpaw, yPositionBrickSpaw;



    // Start is called before the first frame update
    void Start()
    {
        xDistanceBetweenBricks = 1;
        yDistanceBetweenBricks = -0.5f;
        GameManager.gameManager.SetSpawnedBricks(rows * cols);                            
        SpawnBricks();                                                                    
    }

    void SpawnBricks()
    {//inizio funzione che spawna i bricks
        switch (cols)
        {//inizio switch posizionamento orizzontale Colonna
            case 1:
                xPositionBrickSpaw = 0f;
                break;
            case 3:
                xPositionBrickSpaw = -1f;
                break;
            default:
                xPositionBrickSpaw = -2f;
                break;
        }//fine switch posizionamento orizzontale Colonna

        switch (rows)
        {//inizio switch posizionamento verticale Righe
            case 1:
                yPositionBrickSpaw = 0.5f;
                break;
            case 2:
                yPositionBrickSpaw = 1f;
                break;
            case 3:
                yPositionBrickSpaw = 1.5f;
                break;
            case 4:
                yPositionBrickSpaw = 2f;
                break;
            case 5:
                yPositionBrickSpaw = 2.5f;
                break;
            case 6:
                yPositionBrickSpaw = 3f;
                break;
            default:
                yPositionBrickSpaw = 3.5f;
                break;
        }

        for (int i = 0; i < rows; i++)
        {//inizio for righe
            for (int j = 0; j < cols; j++)
            {//inizio for colonne
                Vector2 newBrickPosition = new Vector2(xPositionBrickSpaw + (j * xDistanceBetweenBricks), yPositionBrickSpaw + (i * yDistanceBetweenBricks));
                GameObject.Instantiate(brickPrefab, newBrickPosition, Quaternion.Euler(0, 0, 90), transform);
            }//fine for colonne
        }//fine for righe

    }//fine funzione che spawna i bricks


}//END
