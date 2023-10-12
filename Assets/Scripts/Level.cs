using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject[] rubbers;
    [SerializeField] private GameObject playerPowerUpPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    private GameObject playerPowerUp;
    private int count;
    [SerializeField] private bool check = false;
    private void Update()
    {
        if (check)
        {
            PowerUpPlayer();
        }
        check = false;
    }
    public int GetRubbersNumber()
    {
        count = 0;
        foreach(GameObject rubber in rubbers)
        {
            count += rubber.transform.childCount;
        }
        return count;
    }
    public void PowerUpPlayer()
    {
        playerPowerUp = Instantiate(playerPowerUpPrefab, player.transform.position, Quaternion.identity);
        
        Player playerPowerUpTemp = playerPowerUp.GetComponent<Player>();
        Player playerTemp = player.GetComponent<Player>();

        cinemachine.Follow = playerPowerUp.transform;
        cinemachine.LookAt = playerPowerUp.transform;

        playerPowerUpTemp.changeRotating = playerTemp.changeRotating;
        
        playerPowerUpTemp.ChangeRotationForPlayerPowerUp();
        
        //playerPowerUpTemp.rotating = playerTemp.rotating;
        

        playerPowerUpTemp.enemy = playerTemp.enemy;
        if (enemy != null)
        {
            enemy.player = playerPowerUp;
        }

        playerPowerUp.transform.rotation = player.transform.rotation;
        playerPowerUp.transform.position = player.transform.position;
        playerPowerUp.transform.SetParent(player.transform.parent);
        playerPowerUpTemp.start = true;

        Destroy(player);
    }
}
