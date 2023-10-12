using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject itemsToSpawn;
    public int numCount = 0;
    public List<Vector3> grids;
    public float gridSpacingOffset = 1f;
    //public Color bushColor;
    private Transform spawnPoint;
    private int count = 0;
    public bool spawnObj = false;
    public Transform bushParent;
    private void Start()
    {
        if(spawnObj)
        {
            spawnPoint = this.transform;
            for (int i = 0; i < grids.Count; i++)
            {
                SpawnGrid();
            }
        }
    }
    private void SpawnGrid()
    {
        for (int x = 0; x < grids[count].x; x++)
        {
            for (int z = 0; z < grids[count].z; z++)
            {      
                Vector3 spawnPos;
                spawnPos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + spawnPoint.position;
                    
                GameObject clone = Instantiate(itemsToSpawn, spawnPos, Quaternion.identity);                   
                clone.SetActive(true);
                clone.transform.SetParent(bushParent);
                numCount++;
            }
        }

        //Vector3 newPos = new Vector3(transform.position.x, 1,transform.position.z+grids[count].z*gridSpacingOffset);
        //spawnPoint.position = newPos;
        //print(spawnPoint.position);
        count++;
       
    }
}
