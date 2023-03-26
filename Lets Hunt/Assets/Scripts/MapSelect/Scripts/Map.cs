
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Scenes/Maps")]
public class Map : ScriptableObject
{

    public int mapIndex;
    public string mapName;
    public string mapDescription;
    public Color mapColor;
    public Sprite mapImage;
    public Object sceneToLoad;
}
