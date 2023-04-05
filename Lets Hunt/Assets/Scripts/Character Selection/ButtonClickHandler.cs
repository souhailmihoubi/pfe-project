using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonClickHandler : MonoBehaviour
{
    public TextMeshProUGUI healthMesh;
    public TextMeshProUGUI rangeMesh;
    public TextMeshProUGUI damageMesh;

    public string healthText;
    public string rangeText;
    public string damageText;

    public void OnButtonClick()
    {
        healthMesh.text = healthText.ToString();
        damageMesh.text = damageText.ToString();
        rangeMesh.text = rangeText.ToString();

    }
}
