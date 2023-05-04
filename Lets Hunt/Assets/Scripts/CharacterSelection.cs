using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public Button[] characterButtons;
    private int selectedCharacterIndex = 0;

    [SerializeField] private Button select;
    [SerializeField] private Button buy;
    [SerializeField] private TextMeshProUGUI price;

    [SerializeField] private int[] hunterPrices;
    [SerializeField] private GameObject[] locks;
    [SerializeField] private GameObject charLocks;

    public Button saveButton;

    private void Start()
    {
        selectedCharacterIndex = SaveManager.instance.currentHunter;

        SelectCharacter(selectedCharacterIndex);
        
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i;
            characterButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }

        saveButton.onClick.AddListener(() => SaveSelectedCharacter());

    }

    private void SelectCharacter(int index)
    {
        characters[selectedCharacterIndex].SetActive(false);
        characterButtons[selectedCharacterIndex].interactable = true;
        selectedCharacterIndex = index;
        characters[selectedCharacterIndex].SetActive(true);
        characterButtons[selectedCharacterIndex].interactable = false;

        UpdateUI();

    }

    private void UpdateUI()
    {
        if (SaveManager.instance.huntersUnlocked[selectedCharacterIndex])
        {
            select.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);
            locks[selectedCharacterIndex].SetActive(false);
            charLocks.SetActive(false);
        }
        else
        {
            select.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            price.text = hunterPrices[selectedCharacterIndex].ToString();
            locks[selectedCharacterIndex].SetActive(true);
            charLocks.SetActive(true);
            //Check if we have enough money!

            buy.interactable = (SaveManager.instance.coins >= hunterPrices[selectedCharacterIndex]);
        }
    }

    private void SaveSelectedCharacter()
    {
        SaveManager.instance.currentHunter = selectedCharacterIndex;
        SaveManager.instance.Save();
    }

    public void BuyHunter()
    {
        SaveManager.instance.coins -= hunterPrices[selectedCharacterIndex];
        SaveManager.instance.huntersUnlocked[selectedCharacterIndex] = true;

        SaveManager.instance.owned += 1;

        SaveManager.instance.Save();
        UpdateUI();
    }
}