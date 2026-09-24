using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceSelectionPanelController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Track UI")]
    public Image trackImage;

    public TextMeshProUGUI trackNameText;

    public TextMeshProUGUI trackDescriptionText;

    public TextMeshProUGUI ratingRequirementText;

    public TextMeshProUGUI entryFeeText;

    public TextMeshProUGUI firstPrizeText;

    public TextMeshProUGUI secondPrizeText;

    public TextMeshProUGUI thirdPrizeText;

    [Header("Buttons")]
    public Button previousButton;

    public Button nextButton;

    public Button purchaseEntryButton;

    public Button closeButton;

    [Header("Button Text")]
    public TextMeshProUGUI purchaseButtonText;

    private int currentTrackIndex;

    private void Start()
    {
        currentTrackIndex = 0;

        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(
                ClosePanel);
        }
    }

    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }

        RefreshUI();
    }

    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void NextTrack()
    {
        if (TrackManager.Instance == null)
            return;

        if (TrackManager.Instance.availableTracks == null)
            return;

        if (TrackManager.Instance.availableTracks.Length == 0)
            return;

        currentTrackIndex++;

        if (currentTrackIndex >=
            TrackManager.Instance.availableTracks.Length)
        {
            currentTrackIndex = 0;
        }

        RefreshUI();
    }

    public void PreviousTrack()
    {
        if (TrackManager.Instance == null)
            return;

        if (TrackManager.Instance.availableTracks == null)
            return;

        if (TrackManager.Instance.availableTracks.Length == 0)
            return;

        currentTrackIndex--;

        if (currentTrackIndex < 0)
        {
            currentTrackIndex =
                TrackManager.Instance.availableTracks.Length - 1;
        }

        RefreshUI();
    }

    public void PurchaseEntry()
    {
        TrackData track =
            GetCurrentTrack();

        if (track == null)
            return;

        int playerRating =
            GetPlayerRating();

        if (playerRating <
            track.requiredRating)
        {
            Debug.Log(
                $"Rating too low. Required: {track.requiredRating}");

            return;
        }

        if (RaceManager.Instance.gold <
            track.entryFee)
        {
            Debug.Log(
                $"Not enough gold. Need ${track.entryFee:N0}");

            return;
        }

        RaceManager.Instance.gold -=
            track.entryFee;

        if (TrackManager.Instance != null)
        {
            TrackManager.Instance.QueueTrack(
                track);
        }

        Debug.Log(
            $"Queued Race: {track.trackName}");

        ClosePanel();
    }

    private void RefreshUI()
    {
        TrackData track =
            GetCurrentTrack();

        if (track == null)
            return;

        if (trackImage != null)
        {
            trackImage.sprite =
                track.trackImage;
        }

        trackNameText.text =
            track.trackName;

        trackDescriptionText.text =
            track.trackDescription;

        ratingRequirementText.text =
            $"Required Rating: {track.requiredRating:N0}";

        entryFeeText.text =
            $"Entry Fee: ${track.entryFee:N0}";

        firstPrizeText.text =
            $"1st: ${track.firstPrize:N0}";

        secondPrizeText.text =
            $"2nd: ${track.secondPrize:N0}";

        thirdPrizeText.text =
            $"3rd: ${track.thirdPrize:N0}";

        int playerRating =
            GetPlayerRating();

        bool canEnter =
            playerRating >=
            track.requiredRating;

        if (purchaseEntryButton != null)
        {
            purchaseEntryButton.interactable =
                canEnter;
        }

        if (purchaseButtonText != null)
        {
            if (!canEnter)
            {
                purchaseButtonText.text =
                    $"REQUIRES {track.requiredRating:N0} RATING";
            }
            else
            {
                purchaseButtonText.text =
                    $"PURCHASE ENTRY (${track.entryFee:N0})";
            }
        }
    }

    private TrackData GetCurrentTrack()
    {
        if (TrackManager.Instance == null)
            return null;

        if (TrackManager.Instance.availableTracks == null)
            return null;

        if (TrackManager.Instance.availableTracks.Length == 0)
            return null;

        return TrackManager.Instance.availableTracks[
            currentTrackIndex];
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