using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceSimulationManager : MonoBehaviour
{
    public static RaceSimulationManager Instance;

    [Header("Race")]
    public string raceName = "DAYTONA 500";

    public int maxLaps = 10;

    public int totalDrivers = 10;

    public int currentPosition = 10;

    public int championshipPoints;

    public int currentPrize;

    [Header("Race Progression")]
    public int raceNumber = 1;

    public float raceRewardGrowth = 1.03f;

    [System.Serializable]
    public class RaceEntry
    {
        public string driverName;

        public int driverRating;

        public int carRating;

        public int performanceRating;

        public float visualSpeed;

        public bool isPlayer;

        public int finishingPosition;
    }

    public List<RaceEntry> raceEntries =
        new List<RaceEntry>();

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

    public void SetupRace()
    {
        raceEntries.Clear();

        DriverData player =
            DriverManager.Instance.GetDriver(0);

        if (player == null)
            return;

        AddPlayerEntry(player);

        for (int i = 0;
             i < totalDrivers - 1;
             i++)
        {
            AddAiEntry(i);
        }

        ApplyVisualSpeeds();

        Debug.Log(
            $"========== RACE {raceNumber} SETUP ==========");

        if (TrackManager.Instance != null &&
            TrackManager.Instance.currentTrack != null)
        {
            Debug.Log(
                $"Track: {TrackManager.Instance.currentTrack.trackName}");
        }

        foreach (RaceEntry entry in raceEntries)
        {
            Debug.Log(
                $"{entry.driverName} | " +
                $"Driver:{entry.driverRating} | " +
                $"Car:{entry.carRating} | " +
                $"Performance:{entry.performanceRating} | " +
                $"Speed:{entry.visualSpeed:F2}");
        }

        Debug.Log(
            "================================");
    }

    private void AddPlayerEntry(
        DriverData player)
    {
        RaceEntry entry =
            new RaceEntry();

        entry.driverName =
            player.DriverName;

        entry.isPlayer = true;

        entry.driverRating =
            player.Racecraft +
            player.Consistency +
            player.Aggression +
            player.Adaptability +
            player.Fitness;

        entry.carRating = 0;

        CarData car = null;

        if (CarManager.Instance != null)
        {
            car =
                CarManager.Instance.GetPlayerCar();

            if (car != null)
            {
                entry.carRating =
                    car.TopSpeed +
                    car.Acceleration +
                    car.Handling +
                    car.Reliability +
                    car.FuelEfficiency +
                    car.TireManagement;
            }
        }

        if (TrackManager.Instance != null &&
            TrackManager.Instance.currentTrack != null &&
            car != null)
        {
            TrackData track =
                TrackManager.Instance.currentTrack;

            float straightScore =
                car.TopSpeed *
                (track.straightPercent / 100f);

            float cornerScore =
                car.Handling *
                (track.cornerPercent / 100f);

            float technicalScore =
                player.Consistency *
                (track.technicalPercent / 100f);

            float accelerationScore =
                car.Acceleration * 0.5f;

            float racecraftScore =
                player.Racecraft;

            entry.performanceRating =
                Mathf.RoundToInt(
                    straightScore +
                    cornerScore +
                    technicalScore +
                    accelerationScore +
                    racecraftScore);
        }
        else
        {
            entry.performanceRating =
                entry.driverRating +
                entry.carRating;
        }

        entry.visualSpeed =
            Mathf.Clamp(
                2f +
                (entry.performanceRating * 0.0001f),
                2f,
                2.5f);

        raceEntries.Add(entry);

        Debug.Log(
            $"PLAYER RATING\n" +
            $"Track: {(TrackManager.Instance != null && TrackManager.Instance.currentTrack != null ? TrackManager.Instance.currentTrack.trackName : "None")}\n" +
            $"Driver Rating: {entry.driverRating:N0}\n" +
            $"Car Rating: {entry.carRating:N0}\n" +
            $"Performance Rating: {entry.performanceRating:N0}\n" +
            $"Visual Speed: {entry.visualSpeed:F2}");
    }

    private void AddAiEntry(
        int aiIndex)
    {
        RaceEntry entry =
            new RaceEntry();

        entry.driverName =
            aiNames[
                aiIndex %
                aiNames.Length];

        entry.isPlayer = false;

        int[] aiLadder =
        {
            700,
            750,
            800,
            850,
            900,
            950,
            1000,
            1100,
            1200
        };

        entry.driverRating =
            aiLadder[
                Mathf.Clamp(
                    aiIndex,
                    0,
                    aiLadder.Length - 1)];

        entry.carRating = 0;

        entry.performanceRating =
            entry.driverRating;

        entry.visualSpeed =
            Mathf.Clamp(
                2f +
                (entry.performanceRating * 0.0001f),
                2f,
                2.5f);

        raceEntries.Add(entry);
    }

    private void ApplyVisualSpeeds()
    {
        if (RaceTrackVisualManager.Instance == null)
            return;

        List<RaceCarVisual> cars =
            RaceTrackVisualManager.Instance.GetActiveCars();

        foreach (RaceEntry entry in raceEntries)
        {
            RaceCarVisual visual =
                cars.Find(
                    x => x.driverName ==
                    entry.driverName);

            if (visual != null)
            {
                visual.SetPerformanceRating(
                    entry.performanceRating);

                visual.SetVisualSpeed(
                    entry.visualSpeed);
            }
        }
    }

    public void SimulateRace()
    {
        if (RaceTrackVisualManager.Instance == null)
            return;

        List<RaceCarVisual> cars =
            RaceTrackVisualManager.Instance.GetActiveCars();

        cars = cars
            .OrderByDescending(
                x => x.trackProgress)
            .ToList();

        Debug.Log(
            "========== FINAL STANDINGS ==========");

        for (int i = 0;
             i < cars.Count;
             i++)
        {
            Debug.Log(
                $"P{i + 1} | " +
                $"{cars[i].driverName} | " +
                $"Progress: {cars[i].trackProgress:F2} | " +
                $"Speed: {cars[i].visualSpeed:F2} | " +
                $"Rating: {cars[i].performanceRating}");
        }

        Debug.Log(
            "====================================");

        currentPosition =
            RaceTrackVisualManager.Instance
                .GetPlayerPosition();

        DriverData player =
            DriverManager.Instance.GetDriver(0);

        if (player != null)
        {
            AwardResult(player);
        }
    }

    private int GetScaledPrize(
        int basePrize)
    {
        return Mathf.RoundToInt(
            basePrize *
            Mathf.Pow(
                raceRewardGrowth,
                raceNumber - 1));
    }

    private void AwardResult(
        DriverData driver)
    {
        driver.Races++;

        switch (currentPosition)
        {
            case 1:

                driver.Wins++;
                driver.Podiums++;

                currentPrize =
                    GetScaledPrize(500);

                championshipPoints = 25;

                break;

            case 2:

                driver.Podiums++;

                currentPrize =
                    GetScaledPrize(350);

                championshipPoints = 18;

                break;

            case 3:

                driver.Podiums++;

                currentPrize =
                    GetScaledPrize(250);

                championshipPoints = 15;

                break;

            case 4:

                currentPrize =
                    GetScaledPrize(150);

                championshipPoints = 12;

                break;

            case 5:

                currentPrize =
                    GetScaledPrize(100);

                championshipPoints = 10;

                break;

            default:

                currentPrize =
                    GetScaledPrize(50);

                championshipPoints = 0;

                break;
        }

        driver.ChampionshipPoints +=
            championshipPoints;

        RaceManager.Instance.gold +=
            currentPrize;

        Debug.Log(
            $"Race {raceNumber} Complete | " +
            $"Position:{currentPosition} | " +
            $"Prize:${currentPrize:N0}");

        raceNumber++;

        Debug.Log(
            $"Career Updated | " +
            $"Races:{driver.Races} | " +
            $"Wins:{driver.Wins} | " +
            $"Podiums:{driver.Podiums}");
    }

    public string GetStandingText(
        int index)
    {
        if (RaceTrackVisualManager.Instance == null)
            return "-";

        return RaceTrackVisualManager.Instance
            .GetStandingText(index);
    }
}