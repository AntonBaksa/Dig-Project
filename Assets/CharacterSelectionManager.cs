using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public Sprite characterImage;
        public GameObject characterPrefab;
    }

    public CharacterData[] characters;

    public Image player1Image;
    public TMP_Text player1Name;

    public Image player2Image;
    public TMP_Text player2Name;

    private int index1 = 0;
    private int index2 = 0;

    public static GameObject selectedPrefabP1;
    public static GameObject selectedPrefabP2;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Player 1 navigation
        if (Input.GetKeyDown(KeyCode.A))
        {
            index1 = (index1 - 1 + characters.Length) % characters.Length;
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            index1 = (index1 + 1) % characters.Length;
            UpdateUI();
        }

        // Player 2 navigation
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index2 = (index2 - 1 + characters.Length) % characters.Length;
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index2 = (index2 + 1) % characters.Length;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }
    }

    void UpdateUI()
    {
        player1Image.sprite = characters[index1].characterImage;
        player1Name.text = characters[index1].characterName;

        player2Image.sprite = characters[index2].characterImage;
        player2Name.text = characters[index2].characterName;
    }

    public void ConfirmSelection()
    {
        selectedPrefabP1 = characters[index1].characterPrefab;
        selectedPrefabP2 = characters[index2].characterPrefab;

        SceneManager.LoadScene("AlexanderScene"); // ändra till ditt spelnamn
    }
}
