using UnityEngine;

public class EnemyTouch : MonoBehaviour {
    private GameManager _manager;
    void Start() {
        _manager = Object.FindAnyObjectByType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _manager.LoseMaze();
        }
    }
}