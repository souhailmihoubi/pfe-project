using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Animator animator;
    public float minX, minY, maxX, maxY;
    public int characterValue = 0;


    public void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 11.486f, Random.Range(minY, maxY));

        animator = GetComponent<Animator>();

        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

        //AddCharacter(0);

    }

    void AddCharacter(int whichCharacter)
    {
         characterValue = whichCharacter;

        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 11.486f, Random.Range(minY, maxY));

        playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "man-golf"), randomPosition, Quaternion.identity); //instantiate the player accross the network
        
        playerPrefab.transform.parent = transform;
        
        animator = playerPrefab.GetComponent<Animator>();
    }







}
