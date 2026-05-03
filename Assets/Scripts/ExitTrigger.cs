using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private GameManager _manager;

    public void Setup(GameManager manager) {
        _manager = manager;
    }

    public void OnTriggerEnter(Collider other) {
       if (other.CompareTag("Player")) {
            if (_manager != null) {
                _manager.CompleteMaze();
            }
        }
    }
}
