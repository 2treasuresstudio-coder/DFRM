using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceFlowManager : MonoBehaviour
{
    public static RaceFlowManager Instance;

    [Header("UI")]
    public GameObject raceFlowPanel;

    public TextMeshProUGUI eventTitleText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI resultText;

    public Image timerFillImage;

    [Header("Timing")]
    public float nextRaceDuration = 10f;

    public float countdownDuration = 3f;

    public float resultsDuration = 10f;

    private bool raceRunning;

    private bool raceLoopStarted;

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

    private void Start()
    {
        raceRunning = false;

        raceLoopStarted = false;

        if (raceFlowPanel != null)
        {
            raceFlowPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (raceLoopStarted)
            return;

        if (DriverManager.Instance == null)
            return;

        if (!DriverManager.Instance.HasAnyDriver())
            return;

        raceLoopStarted = true;

        StartCoroutine(
            RaceLoop());
    }

    public bool IsRaceRunning()
    {
        return raceRunning;
    }

    private IEnumerator RaceLoop()
    {
        while (true)
        {
            yield return StartCoroutine(
                ShowNextRacePanel());

            yield return StartCoroutine(
                ShowCountdown());

            StartRace();

            yield return StartCoroutine(
                WaitForRaceCompletion());

            yield return StartCoroutine(
                ShowResults());
        }
    }

    private IEnumerator ShowNextRacePanel()
    {
        raceFlowPanel.SetActive(true);

        if (TrackManager.Instance != null &&
            TrackManager.Instance.currentTrack != null)
        {
            eventTitleText.text =
                TrackManager.Instance.currentTrack.trackName;
        }
        else
        {
            eventTitleText.text =
                RaceSimulationManager.Instance.raceName;
        }

        messageText.text =
            "NEXT RACE";

        resultText.text = "";

        float timer =
            nextRaceDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            countdownText.text =
                Mathf.CeilToInt(timer).ToString();

            if (timerFillImage != null)
            {
                timerFillImage.fillAmount =
                    timer / nextRaceDuration;
            }

            yield return null;
        }
    }

    private IEnumerator ShowCountdown()
    {
        raceFlowPanel.SetActive(true);

        resultText.text = "";

        float timer =
            countdownDuration;

        while (timer > 0)
        {
            messageText.text =
                "STARTING IN";

            countdownText.text =
                Mathf.CeilToInt(timer).ToString();

            if (timerFillImage != null)
            {
                timerFillImage.fillAmount =
                    timer / countdownDuration;
            }

            timer -= Time.deltaTime;

            yield return null;
        }

        messageText.text = "";

        countdownText.text = "GO!";

        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = 0f;
        }

        yield return new WaitForSeconds(1f);

        raceFlowPanel.SetActive(false);
    }

    private void StartRace()
    {
        raceRunning = true;

        if (TrackManager.Instance != null)
        {
            TrackManager.Instance
                .ConsumeQueuedTrack();
        }

        if (RaceManager.Instance != null)
        {
            RaceManager.Instance.ResetRace();
        }

        if (RaceTrackVisualManager.Instance != null)
        {
            RaceTrackVisualManager.Instance
                .ResetCarsToGrid();
        }

        if (RaceSimulationManager.Instance != null)
        {
            RaceSimulationManager.Instance
                .SetupRace();
        }

        Debug.Log(
            $"Race Started: {TrackManager.Instance.currentTrack.trackName}");
    }

    private IEnumerator WaitForRaceCompletion()
    {
        while (RaceManager.Instance.currentLap <
               RaceSimulationManager.Instance.maxLaps)
        {
            yield return null;
        }

        RaceSimulationManager.Instance
            .SimulateRace();

        raceRunning = false;

        Debug.Log(
            "Race Finished");
    }

    private IEnumerator ShowResults()
    {
        raceFlowPanel.SetActive(true);

        if (TrackManager.Instance != null &&
            TrackManager.Instance.currentTrack != null)
        {
            eventTitleText.text =
                TrackManager.Instance.currentTrack.trackName;
        }
        else
        {
            eventTitleText.text =
                RaceSimulationManager.Instance.raceName;
        }

        messageText.text =
            "RACE FINISHED";

        countdownText.text = "";

        resultText.text =
            $"Position: {RaceSimulationManager.Instance.currentPosition}\n\n" +
            $"Prize: ${RaceSimulationManager.Instance.currentPrize:N0}\n\n" +
            $"Player Rating: {GetPlayerRating():N0}";

        float timer =
            resultsDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timerFillImage != null)
            {
                timerFillImage.fillAmount =
                    timer / resultsDuration;
            }

            yield return null;
        }
    }

    private int GetPlayerRating()
    {
        DriverData driver =
            DriverManager.Instance.GetDriver(0);

        if (driver == null)
            return 0;

        int driverRating =
            driver.Racecraft +
            driver.Consistency +
            driver.Aggression +
            driver.Adaptability +
            driver.Fitness;

        int carRating = 0;

        if (CarManager.Instance != null)
        {
            CarData car =
                CarManager.Instance.GetPlayerCar();

            if (car != null)
            {
                carRating =
                    car.TopSpeed +
                    car.Acceleration +
                    car.Handling +
                    car.FuelEfficiency +
                    car.Reliability +
                    car.TireManagement;
            }
        }

        return driverRating + carRating;
    }
}