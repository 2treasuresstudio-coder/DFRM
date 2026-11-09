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

    [Header("Lane System")]
    [Tooltip("Target racing lane offset from the track center.")]
    public float laneOffset;

    [HideInInspector]
    public float currentLaneOffset;

    [HideInInspector]
    public float laneChangeSpeed = 2f;

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

        GenerateLanePreference();
    }

    private void GenerateLanePreference()
    {
        // Keep player centered initially
        if (isPlayer)
        {
            laneOffset = 0f;
        }
        else
        {
            // AI cars choose a preferred racing groove
            laneOffset =
                Random.Range(
                    -1.0f,
                    1.0f);
        }

        currentLaneOffset =
            laneOffset;
    }

    public void SetTargetLane(
        float targetLane)
    {
        laneOffset =
            targetLane;
    }

    public void UpdateLaneOffset(
        float deltaTime)
    {
        currentLaneOffset =
            Mathf.Lerp(
                currentLaneOffset,
                laneOffset,
                laneChangeSpeed *
                deltaTime);
    }

    public float GetLaneOffset()
    {
        return currentLaneOffset;
    }

    public void SetLaneChangeSpeed(
        float speed)
    {
        laneChangeSpeed =
            Mathf.Max(
                0.1f,
                speed);
    }

    public void SetPerformanceRating(
        int rating)
    {
        performanceRating =
            rating;
    }

    public int GetPerformanceRating()
    {
        return performanceRating;
    }

    public void SetVisualSpeed(
        float speed)
    {
        visualSpeed =
            speed;
    }

    public float GetVisualSpeed()
    {
        return visualSpeed;
    }

    public void SetRacePosition(
        int position)
    {
        racePosition =
            position;
    }

    public int GetRacePosition()
    {
        return racePosition;
    }

    public void SetCurrentLap(
        int lap)
    {
        currentLap =
            lap;
    }

    public int GetCurrentLap()
    {
        return currentLap;
    }

    public bool IsPlayerCar()
    {
        return isPlayer;
    }
}