using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Manager/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private GameSettings gameSettings;
    public static GameSettings GameSettings { get { return Instance.gameSettings; } }
   
}
