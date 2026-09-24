using System.Collections.Generic;
using UnityEngine;

public class RaceTrackVisualManager : MonoBehaviour
{
    public static RaceTrackVisualManager Instance;

    [Header("References")]
    public GameObject raceCarPrefab;

    public Transform[] spawnPoints;

    public Transform[] raceWaypoints;

    [Header("Car Sprites")]
    public Sprite playerCarSprite;

    public Sprite[] aiCarSprites;

    [Header("Handling")]
    public float rotationSpeed = 10f;

    [Range(0f, 25f)]
    public float maxDriftAngle = 10f;

    [Header("Sprite Setup")]
    public float spawnRotationOffset = -90f;

    public float movementRotationOffset = 90f;

    private readonly List<RaceCarVisual> activeCars =
        new List<RaceCarVisual>();

    private readonly string[] aiNames =
    {
        "Sarah Walker",
        "Ryan Adams",
        "Mia Torres",
        "Noah Smith",
        "Alex Carter",
        "Emma Scott",
        "Lucas Hill",
        "Olivia Brooks",
        "Ethan Reed"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (RaceFlowManager.Instance == null)
            return;

        if (!RaceFlowManager.Instance.IsRaceRunning())
            return;

        UpdateCars();
    }

    public void SpawnRaceGrid()
    {
        ClearGrid();

        DriverData player =
            DriverManager.Instance.GetDriver(0);

        if (player == null)
            return;

        SpawnCar(
            playerCarSprite,
            player.DriverName,
            true,
            0);

        for (int i = 1; i < 10; i++)
        {
            SpawnCar(
                GetRandomAiCarSprite(),
                aiNames[i - 1],
                false,
                i);
        }

        Debug.Log(
            "Race Grid Spawned");
    }

    public void ResetCarsToGrid()
    {
        for (int i = 0;
             i < activeCars.Count;
             i++)
        {
            RaceCarVisual car =
                activeCars[i];

            if (car == null)
                continue;

            car.trackProgress =
                i * 0.15f;

            car.racePosition =
                i + 1;

            car.currentLap = 0;

            if (i < spawnPoints.Length)
            {
                car.transform.position =
                    spawnPoints[i].position;

                car.transform.rotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        spawnRotationOffset);
            }
        }

        Debug.Log(
            "Cars Reset To Starting Grid");
    }

    private void SpawnCar(
        Sprite carSprite,
        string driverName,
        bool playerCar,
        int spawnIndex)
    {
        if (spawnIndex >= spawnPoints.Length)
            return;

        GameObject car =
            Instantiate(
                raceCarPrefab,
                spawnPoints[spawnIndex].position,
                Quaternion.identity);

        RaceCarVisual visual =
            car.GetComponent<RaceCarVisual>();

        if (visual != null)
        {
            visual.Initialize(
                carSprite,
                driverName,
                playerCar);

            visual.trackProgress =
                spawnIndex * 0.15f;

            visual.currentLap = 0;

            visual.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    spawnRotationOffset);
        }

        activeCars.Add(visual);
    }

    private void UpdateCars()
    {
        if (raceWaypoints == null ||
            raceWaypoints.Length == 0)
        {
            return;
        }

        foreach (RaceCarVisual car in activeCars)
        {
            if (car == null)
                continue;

            float previousProgress =
                car.trackProgress;

            float speed =
                car.visualSpeed;

            if (RaceManager.Instance != null)
            {
                if (RaceManager.Instance.pushMode &&
                    car.isPlayer)
                {
                    speed *= 1.5f;
                }
            }

            car.trackProgress +=
                speed *
                Time.deltaTime;

            car.currentLap =
                Mathf.Clamp(
                    Mathf.FloorToInt(
                        car.trackProgress /
                        raceWaypoints.Length),
                    0,
                    RaceSimulationManager.Instance.maxLaps);

            if (car.isPlayer)
            {
                int previousLoop =
                    Mathf.FloorToInt(
                        previousProgress /
                        raceWaypoints.Length);

                int currentLoop =
                    Mathf.FloorToInt(
                        car.trackProgress /
                        raceWaypoints.Length);

                if (currentLoop >
                    previousLoop)
                {
                    if (RaceManager.Instance != null)
                    {
                        RaceManager.Instance
                            .CompleteLap();
                    }
                }
            }

            int currentWaypoint =
                Mathf.FloorToInt(
                    car.trackProgress)
                % raceWaypoints.Length;

            int nextWaypoint =
                (currentWaypoint + 1)
                % raceWaypoints.Length;

            float lerpValue =
                car.trackProgress -
                Mathf.Floor(car.trackProgress);

            Vector3 currentPos =
                raceWaypoints[currentWaypoint].position;

            Vector3 nextPos =
                raceWaypoints[nextWaypoint].position;

            car.transform.position =
                Vector3.Lerp(
                    currentPos,
                    nextPos,
                    lerpValue);

            Vector3 direction =
                nextPos - currentPos;

            if (direction.sqrMagnitude > 0.001f)
            {
                float targetAngle =
                    Mathf.Atan2(
                        direction.y,
                        direction.x)
                    * Mathf.Rad2Deg;

                float currentAngle =
                    car.transform.eulerAngles.z;

                float turnDifference =
                    Mathf.DeltaAngle(
                        currentAngle,
                        targetAngle +
                        movementRotationOffset +
                        180f);

                float driftOffset =
                    Mathf.Clamp(
                        turnDifference * 0.35f,
                        -maxDriftAngle,
                        maxDriftAngle);

                float finalAngle =
                    targetAngle +
                    movementRotationOffset +
                    180f +
                    driftOffset;

                Quaternion targetRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        finalAngle);

                car.transform.rotation =
                    Quaternion.Slerp(
                        car.transform.rotation,
                        targetRotation,
                        rotationSpeed *
                        Time.deltaTime);
            }
        }

        UpdatePositions();
    }

    private void UpdatePositions()
    {
        activeCars.Sort(
            (a, b) =>
                b.trackProgress.CompareTo(
                    a.trackProgress));

        for (int i = 0;
             i < activeCars.Count;
             i++)
        {
            activeCars[i].racePosition =
                i + 1;
        }
    }

    public int GetPlayerPosition()
    {
        foreach (RaceCarVisual car in activeCars)
        {
            if (car != null &&
                car.isPlayer)
            {
                return car.racePosition;
            }
        }

        return activeCars.Count;
    }

    public string GetStandingText(
        int index)
    {
        if (index < 0 ||
            index >= activeCars.Count)
        {
            return "-";
        }

        return $"{index + 1}. {activeCars[index].driverName}";
    }

    public string GetStandingLapText(
        int index)
    {
        if (index < 0 ||
            index >= activeCars.Count)
        {
            return "-";
        }

        return $"{activeCars[index].currentLap}/{RaceSimulationManager.Instance.maxLaps}";
    }

    private Sprite GetRandomAiCarSprite()
    {
        if (aiCarSprites == null ||
            aiCarSprites.Length == 0)
        {
            return null;
        }

        return aiCarSprites[
            Random.Range(
                0,
                aiCarSprites.Length)];
    }

    public List<RaceCarVisual> GetActiveCars()
    {
        return activeCars;
    }

    public void ClearGrid()
    {
        foreach (RaceCarVisual car
            in activeCars)
        {
            if (car != null)
            {
                Destroy(
                    car.gameObject);
            }
        }

        activeCars.Clear();
    }
}