using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclesPrefabs;
    //[SerializeField] TextMeshProUGUI obstacleText;
    [SerializeField] Image obstacleImage;
    [SerializeField] GameObject obstacleMessage;
    [SerializeField] Sprite[] obstaclesSprites;

    float delayTime = 3f;
    float repeatRate = 10f;

    /// <summary>
    /// Start invokes for spawning enemies and powerups
    /// </summary>
    void Start()
    {
        Invoke(nameof(SpawnObstacle), delayTime);
    }

    /// <summary>
    /// Spawn enemy outside the camera border in a specific height
    /// </summary>
    void SpawnObstacle()
    {
        GameObject randomObstacle = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];
        randomObstacle.transform.position = new Vector3(OutsideCameraViewX(), randomObstacle.transform.position.y, randomObstacle.transform.position.z);
        Instantiate(randomObstacle);
        StartCoroutine(ShowObstacleWarning(randomObstacle));

        if (repeatRate > 5f)
        {
            repeatRate *= 0.95f;

        }
        Invoke(nameof(SpawnObstacle), repeatRate);
    }

    IEnumerator ShowObstacleWarning(GameObject obstacle)
    {
        obstacleMessage.SetActive(true);

        TextMeshProUGUI obstacleText = obstacleMessage.GetComponentInChildren<TextMeshProUGUI>();

        obstacleText.text = $"{obstacle.name} Incoming".ToUpper();
        switch (obstacle.tag)
        {
            case "Fire":
                {
                    obstacleText.color = new Color(0.7433963f, 0.009818403f, 0.009818403f);
                    obstacleImage.sprite = obstaclesSprites[1];
                    break;
                }

            case "Ice":
                {
                    obstacleText.color = Color.blue;
                    obstacleImage.sprite = obstaclesSprites[0];
                    break;
                }

            case "Cactus":
                {
                    obstacleText.color = Color.green;
                    obstacleImage.sprite = obstaclesSprites[2];
                    break;
                }
        }

        yield return new WaitForSeconds(5);

        obstacleMessage.SetActive(false);
    }

    /// <summary>
    /// Get the position outside the camera view point
    /// </summary>
    float OutsideCameraViewX()
    {
        float distanceFromBorder = 4f;
        Camera mainCamera = Camera.main;

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate spawn position only to the right side of the camera
        float spawnX = Random.Range(cameraWidth, cameraWidth + distanceFromBorder);

        return spawnX;
    }
}
