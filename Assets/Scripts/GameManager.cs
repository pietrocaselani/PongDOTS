using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    private static readonly int MaxScore = 3;

    public GUISkin layout;

    public GameObject ballPrefab;

    public float ballSpeed = 3f;

    private WaitForSeconds oneSecond;
    private WaitForSeconds delay;

    private EntityManager entityManager;
    private Entity ballEntityPrefab;

    private BlobAssetStore blobAssetStore;

    private Text mainText;

    private int player1Score = 0;
    private int player2Score = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        oneSecond = new WaitForSeconds(1f);
        delay = new WaitForSeconds(2f);

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        blobAssetStore = new BlobAssetStore();

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, settings);
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    // Start is called before the first frame update
    private void Start()
    {
        mainText = GetComponentInChildren<Text>(false);

        StartCoroutine(CountdownAndSpawnBall(withDelay: true));
    }

    private void OnGUI()
    {
        GUI.skin = layout;

        DrawScores();

        if (IsGameOver())
            DrawRestart();
    }

    public static void Score(int playerScored)
    {
        if (Instance == null)
            return;

        if (playerScored == 1)
            Instance.player1Score++;
        else if (playerScored == 2)
            Instance.player2Score++;

        Instance.CheckWinner();
    }

    private void DrawRestart()
    {
        var buttonWidth = 130;
        var buttonHeight = 55;

        var x = (Screen.width - buttonWidth) / 2;
        var y = Screen.height - buttonHeight * 2;

        if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "RESTART"))
        {
            player1Score = 0;
            player2Score = 0;
            mainText.text = "";

            StartCoroutine(CountdownAndSpawnBall(withDelay: true));
        }
    }

    private void DrawScores()
    {
        var spacing = 80;

        var labelSize = 100;

        var player1X = Screen.width / 2 - labelSize - spacing;
        var player2X = Screen.width / 2 + labelSize + spacing;

        var y = 20;

        GUI.Label(new Rect(player1X, y, labelSize, labelSize), "" + player1Score);
        GUI.Label(new Rect(player2X, y, labelSize, labelSize), "" + player2Score);
    }

    private void CheckWinner()
    {
        if (player1Score == MaxScore)
        {
            mainText.text = "PLAYER ONE WINS";
        }
        else if (player2Score == MaxScore)
        {
            mainText.text = "PLAYER TWO WINS";
        }
        else
        {
            mainText.text = "";
            StartCoroutine(CountdownAndSpawnBall(withDelay: false));
        }
    }

    private bool IsGameOver()
    {
        return player1Score == MaxScore || player2Score == MaxScore;
    }

    private IEnumerator CountdownAndSpawnBall(bool withDelay)
    {
        if (withDelay)
        {
            mainText.text = "Get Ready";
            yield return delay;
        }

        mainText.text = "3";
        yield return oneSecond;

        mainText.text = "2";
        yield return oneSecond;

        mainText.text = "1";
        yield return oneSecond;

        mainText.text = "";

        SpawnBall();
    }

    private void SpawnBall()
    {
        Vector3 dir = new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1, UnityEngine.Random.Range(-.5f, .5f), 0f).normalized;
        Vector3 speed = dir * ballSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity()
        {
            Linear = speed,
            Angular = float3.zero
        };

        var ball = entityManager.Instantiate(ballEntityPrefab);

        entityManager.AddComponentData(ball, velocity);
    }
}