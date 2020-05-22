using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public GameObject playGround;

    public GameObject targetCube;

    private float bigTimer;

    private float spawnTime;

    public TextMeshPro timerText;
    private float timer;
    private int timeLeft;
    private float textTimer;

    private GameObject[,] cells;
    private float xSize;
    private float ySize;

    private GameObject copyTargetCube;

    private int amountOfMisses;

    private bool miss;

    private bool existanceOfCell;

    private int score;

    public TextMeshPro ruleText;
    private string rules;

    public TextMeshPro missCount;

    private Vector3[,] cellsDistanation;

    private GameObject usingCopy;

    // Start is called before the first frame update
    void Start()
    {
        existanceOfCell = false;
        miss = false;
        cells = new GameObject[5, 5];
        bigTimer = 0;
        timeLeft = 60;
        textTimer = 3;
        score = 0;
        timer = 0;
        amountOfMisses = 0;

        Mesh playGroundMesh = playGround.GetComponent<MeshFilter>().mesh;
        xSize = playGroundMesh.bounds.size.x * playGround.transform.localScale.x / 5;
        ySize = playGroundMesh.bounds.size.y * playGround.transform.localScale.y / 5;

        rules = "Для начала игры нажмите на квадрат на панели перед вами." +
            "                                                                                                               После этого нажимайте на появляющиеся квадраты как можно быстрее. ";
        
        cellsDistanation = new Vector3[5,5];
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Vector3 position = new Vector3((i * xSize), (j * ySize), 0);
                position += playGround.transform.position;
                cellsDistanation[i, j] = position;
            }
        }

        copyTargetCube = targetCube;
        copyTargetCube.transform.localScale = new Vector3(xSize, ySize, 0.002f);

        /*
        copyTargetCubeTransform.localScale = copyTargetCubeTransformLoacalScale;
        copyTargetCube.transform = copyTargetCubeTransform;
        */

        timerText.SetText("");

        ruleText.SetText(rules);

        missCount.SetText("");
    }

    void CreateCell()
    {
        int i = Random.Range(0, 4);
        int j = Random.Range(0, 4);
        usingCopy = Instantiate(copyTargetCube, cellsDistanation[i, j], Quaternion.identity);
    }
        

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        bigTimer += Time.deltaTime;

        if (timer >= spawnTime && existanceOfCell == false)
        {
            CreateCell();
            timer = 0;
            existanceOfCell = true;
        }

        if (bigTimer >= 15)
        {
            missCount.SetText("Кол-во промахов: " + amountOfMisses);
            ruleText.text = rules;
            timerText.SetText("Осталось: " + timeLeft + " сек");
            textTimer += 1 * Time.deltaTime;
            if (textTimer >= 1)
            {
                timeLeft -= 1;
                textTimer = 0;

            }
            if (timeLeft <= 15)
            {
                timerText.color = Color.red;
            }

            spawnTime -= 0.065f;
        }

        if (existanceOfCell == true && timer >= spawnTime && score >= 4 && bigTimer >= 15)
        {
            Destroy(usingCopy);
            amountOfMisses++;
            missCount.SetText("Кол-во промахов: " + amountOfMisses);
        }

        if (timeLeft <= 0)
        {
            textTimer = 0;
        }
    }

    public void PointClick()
    {
        score++;
        Destroy(usingCopy);
        existanceOfCell = false;
    }
}
