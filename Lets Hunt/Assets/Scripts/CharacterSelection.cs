using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public Button[] characterButtons;
    private int selectedCharacterIndex = 0;

    public Button saveButton;

    private void Start()
    {
        // Set the first character as the default selection
        characters[selectedCharacterIndex].SetActive(true);
        characterButtons[selectedCharacterIndex].interactable = false;

        // Add a click listener to each character button
        
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i;
            characterButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }

        saveButton.onClick.AddListener(() => SaveSelectedCharacterIndex());

    }

    private void SelectCharacter(int index)
    {
        // Disable the current character and enable the selected character
        characters[selectedCharacterIndex].SetActive(false);
        characterButtons[selectedCharacterIndex].interactable = true;
        selectedCharacterIndex = index;
        characters[selectedCharacterIndex].SetActive(true);
        characterButtons[selectedCharacterIndex].interactable = false;

        Debug.Log(selectedCharacterIndex);

        // Instantiate the selected character on the network
        //PhotonNetwork.Instantiate(characters[selectedCharacterIndex].name, Vector3.zero, Quaternion.identity);
    }

    private void SaveSelectedCharacterIndex()
    {
        // Save the selected character index to a variable or player preference
        int savedIndex = selectedCharacterIndex;

        PlayerPrefs.SetInt("SelectedCharacterIndex", savedIndex);

        Debug.Log("Selected character index saved: " + savedIndex);
    }
}