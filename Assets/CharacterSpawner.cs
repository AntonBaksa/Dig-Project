using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    public Image healthBarP1;
    public Image healthBarP2;

    void Start()
    {
        GameObject p1 = Instantiate(CharacterSelectionManager.selectedPrefabP1, spawnPointP1.position, Quaternion.identity);
        GameObject p2 = Instantiate(CharacterSelectionManager.selectedPrefabP2, spawnPointP2.position, Quaternion.identity);

        PlayerController controller1 = p1.GetComponent<PlayerController>();
        PlayerController controller2 = p2.GetComponent<PlayerController>();

        controller1.playerID = 1;
        controller2.playerID = 2;

        controller1.healthBar = healthBarP1;
        controller2.healthBar = healthBarP2;
    }
}
