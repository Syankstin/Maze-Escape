using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public MazeGenerator mazeGenerator;
    public GameObject playerPrefab;
    public GameObject exitPrefab;
    public GameObject enemyPrefab;

    [SerializeField]
    private GameObject winText;

    [SerializeField]
    private GameObject loseText;


    void Awake() {
        if (mazeGenerator != null) {
            mazeGenerator.OnMazeGenerationComplete += SpawnObjects;
        }

        if (winText != null) {
            winText.SetActive(false);
        }

        if (loseText != null) {
            loseText.SetActive(false);
        }
    }
    
    private void SpawnObjects() {
        if (mazeGenerator.EntranceCell != null) {
            Vector3 playerPos = mazeGenerator.EntranceCell.transform.position + new Vector3(0, 0.1f, 0);
            Instantiate(playerPrefab, playerPos, Quaternion.identity);
        }

        if (mazeGenerator.ExitCell != null && exitPrefab != null) {
            Vector3 exitPos = mazeGenerator.ExitCell.transform.position + new Vector3(0, 0.1f, 0);
            GameObject exitInstance = Instantiate(exitPrefab, exitPos, Quaternion.identity);

            ExitTrigger trigger = exitInstance.GetComponent<ExitTrigger>();
            if (trigger == null) trigger = exitInstance.AddComponent<ExitTrigger>();
            trigger.Setup(this);
        }

        if(mazeGenerator.EnemyCell != null) {
            Vector3 enemyPos = mazeGenerator.EnemyCell.transform.position + new Vector3(0, 0.1f, 0);
            Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
        }

        mazeGenerator.OnMazeGenerationComplete -= SpawnObjects;
    }

    public void CompleteMaze() {
        if (winText != null) {
            winText.SetActive(true);
        }

        Time.timeScale = 0;
        Debug.Log("Game Stopped - Player Wins :)");
    }

    public void LoseMaze() {
        if (loseText != null) {
            loseText.SetActive(true);
        }

        Time.timeScale = 0;
        Debug.Log("Game Stopped - Player Loses :(");
    }

    void Update() {
        
    }
}
