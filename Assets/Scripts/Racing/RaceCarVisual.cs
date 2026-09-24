using UnityEngine;

public class RaceCarVisual : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer carSpriteRenderer;

    [HideInInspector]
    public string driverName;

    [HideInInspector]
    public bool isPlayer;

    [HideInInspector]
    public int performanceRating;

    [HideInInspector]
    public float visualSpeed;

    [HideInInspector]
    public float trackProgress;

    [HideInInspector]
    public int racePosition;

    [HideInInspector]
    public int currentLap;

    public void Initialize(
        Sprite carSprite,
        string newDriverName,
        bool playerCar)
    {
        if (carSpriteRenderer != null)
        {
            carSpriteRenderer.sprite =
                carSprite;
        }

        driverName =
            newDriverName;

        isPlayer =
            playerCar;

        performanceRating = 0;

        visualSpeed = 0f;

        trackProgress = 0f;

        racePosition = 10;

        currentLap = 0;
    }

    public void SetPerformanceRating(
        int rating)
    {
        performanceRating = rating;
    }

    public int GetPerformanceRating()
    {
        return performanceRating;
    }

    public void SetVisualSpeed(
        float speed)
    {
        visualSpeed = speed;
    }

    public float GetVisualSpeed()
    {
        return visualSpeed;
    }

    public void SetRacePosition(
        int position)
    {
        racePosition = position;
    }

    public int GetRacePosition()
    {
        return racePosition;
    }
}