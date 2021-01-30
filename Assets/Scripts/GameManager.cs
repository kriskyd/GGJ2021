using SA.ScriptableData.Collection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private RocketSystem.TempRocketScript rocketScript;

    [SerializeField]
    private ListVector3Value rocketPartPositions;

    [SerializeField]
    private InGameMenu inGameMenu;

    [SerializeField]
    public GameObject playerPrefab;
    [SerializeField]
    public Transform playerSpawnPoint;

    public RocketSystem.TempRocketScript RocketScript { get => rocketScript; }
    public PlayerController PlayerController { get; private set; }


    private int placedRocketParts = 0;

    private void Awake()
    {
        _instance = this;
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            inGameMenu.Show();
        }
    }

    private void InitializeGame()
    {
        GameObject playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, transform.rotation);
        PlayerController = playerInstance.GetComponent<PlayerController>();
        rocketPartPositions.Clear(); 
        placedRocketParts = 0;
    }

    public void OnRocketPartPlaced()
    {
        ++placedRocketParts;
        CheckRocketCompletion();
    }
    public void OnRocketPartRemoved()
    {
        --placedRocketParts;
        CheckRocketCompletion();
    }

    private void CheckRocketCompletion()
    {
        Debug.Log($"Placed {placedRocketParts} of {RocketSystem.RocketPartSlot.slotsCount} rocket parts.");
        if (placedRocketParts >= RocketSystem.RocketPartSlot.slotsCount)
        {
            GameWon();
        }
    }

    public void GameWon()
    {
        Debug.Log("Game won");
    }

    public void GameOver()
    {
        Debug.Log("Game over");
    }
}
