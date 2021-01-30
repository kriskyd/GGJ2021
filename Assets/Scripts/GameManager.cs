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
    public GameObject playerPrefab;
    [SerializeField]
    public Transform playerSpawnPoint;

    public PlayerController PlayerController { get; private set; }


    private void Awake()
    {
        _instance = this;
        InitializeGame();
    }

    private void InitializeGame()
    {
        GameObject playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, transform.rotation);
        PlayerController = playerInstance.GetComponent<PlayerController>();
    }


}
