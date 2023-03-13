
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Text mapName;
    [SerializeField] private Text mapDescription;
    [SerializeField] private Image mapImage;
    public void DisplayMap(Map map)
    {
        mapName.text = map.mapName;
        mapDescription.text = map.mapDescription;
        mapImage.sprite = map.mapImage;
    }
}
    