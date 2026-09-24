using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceHUDController : MonoBehaviour
{
    [Header("Race HUD")]
    public TextMeshProUGUI raceNameText;

    public TextMeshProUGUI nextRaceText;

    public TextMeshProUGUI lapText;

    public TextMeshProUGUI positionText;

    public TextMeshProUGUI playerRatingText;

    [Header("Prize Pool")]
    public TextMeshProUGUI firstPrizeText;

    public TextMeshProUGUI secondPrizeText;

    public TextMeshProUGUI thirdPrizeText;

    [Header("Standings")]
    public TextMeshProUGUI position1Text;

    public TextMeshProUGUI position2Text;

    public TextMeshProUGUI position3Text;

    public TextMeshProUGUI position4Text;

    public TextMeshProUGUI position5Text;

    [Header("Boost")]
    public Image boostFillImage;

    public Button pushModeButton;

    [Header("Vehicle Condition")]
    public Image fuelFillImage;

    public Image tireFillImage;

    [Header("Colors")]
    public Color fullColor =
        new Color(0.4f, 0.8f, 1f);

    public Color fullGreenColor =
        new Color(0.4f, 0.8f, 1f);

    public Color midColor =
        Color.yellow;

    public Color lowColor =
        Color.red;

    private void Update()
    {
        if (RaceSimulationManager.Instance == null)
            return;

        UpdateTrackInfo();

        UpdateRaceInfo();

        UpdateStandings();

        UpdateBoostUI();

        UpdateVehicleConditionUI();
    }

    private void UpdateTrackInfo()
    {
        if (TrackManager.Instance == null)
            return;

        if (raceNameText != null)
        {
            if (TrackManager.Instance.currentTrack != null)
            {
                raceNameText.text =
                    TrackManager.Instance
                        .currentTrack
                        .trackName;
            }
            else
            {
                raceNameText.text =
                    RaceSimulationManager.Instance.raceName;
            }
        }

        if (nextRaceText != null)
        {
            if (TrackManager.Instance.nextTrack != null)
            {
                nextRaceText.text =
                    TrackManager.Instance
                        .nextTrack
                        .trackName;
            }
            else if (
                TrackManager.Instance.stockCarRaceTrack != null)
            {
                nextRaceText.text =
                    TrackManager.Instance
                        .stockCarRaceTrack
                        .trackName;
            }
        }
    }

    private void UpdateRaceInfo()
    {
        if (RaceManager.Instance != null)
        {
            int remainingLaps =
                Mathf.Max(
                    0,
                    RaceSimulationManager.Instance.maxLaps -
                    RaceManager.Instance.currentLap);

            lapText.text =
                $"Remaining Laps: {remainingLaps}";
        }

        if (RaceTrackVisualManager.Instance != null)
        {
            positionText.text =
                $"{RaceTrackVisualManager.Instance.GetPlayerPosition()} / 10";
        }

        if (TrackManager.Instance != null &&
            TrackManager.Instance.currentTrack != null)
        {
            TrackData track =
                TrackManager.Instance.currentTrack;

            if (firstPrizeText != null)
            {
                firstPrizeText.text =
                    $"${track.firstPrize:N0}";
            }

            if (secondPrizeText != null)
            {
                secondPrizeText.text =
                    $"${track.secondPrize:N0}";
            }

            if (thirdPrizeText != null)
            {
                thirdPrizeText.text =
                    $"${track.thirdPrize:N0}";
            }
        }

        if (playerRatingText != null)
        {
            playerRatingText.text =
                GetPlayerRating().ToString("N0");
        }
    }

    private void UpdateStandings()
    {
        if (RaceTrackVisualManager.Instance == null)
            return;

        position1Text.text =
            RaceTrackVisualManager.Instance.GetStandingText(0);

        position2Text.text =
            RaceTrackVisualManager.Instance.GetStandingText(1);

        position3Text.text =
            RaceTrackVisualManager.Instance.GetStandingText(2);

        position4Text.text =
            RaceTrackVisualManager.Instance.GetStandingText(3);

        position5Text.text =
            RaceTrackVisualManager.Instance.GetStandingText(4);
    }

    private void UpdateBoostUI()
    {
        if (RaceManager.Instance == null)
            return;

        float boostPercent =
            RaceManager.Instance.GetBoostPercent();

        if (boostFillImage != null)
        {
            boostFillImage.fillAmount =
                boostPercent;

            UpdateConditionColor(
                boostFillImage,
                boostPercent);
        }

        if (pushModeButton != null)
        {
            pushModeButton.interactable =
                RaceManager.Instance.boostEnergy >= 100f;
        }
    }

    private void UpdateVehicleConditionUI()
    {
        if (RaceManager.Instance == null)
            return;

        float fuelPercent =
            RaceManager.Instance.fuel / 100f;

        float tirePercent =
            RaceManager.Instance.tireHealth / 100f;

        if (fuelFillImage != null)
        {
            fuelFillImage.fillAmount =
                fuelPercent;

            UpdateConditionColorGreen(
                fuelFillImage,
                fuelPercent);
        }

        if (tireFillImage != null)
        {
            tireFillImage.fillAmount =
                tirePercent;

            UpdateConditionColorGreen(
                tireFillImage,
                tirePercent);
        }
    }

    private void UpdateConditionColor(
        Image image,
        float value)
    {
        if (image == null)
            return;

        if (value >= 0.50f)
        {
            image.color =
                fullColor;
        }
        else if (value >= 0.20f)
        {
            image.color =
                midColor;
        }
        else
        {
            image.color =
                lowColor;
        }
    }

    private void UpdateConditionColorGreen(
        Image image,
        float value)
    {
        if (image == null)
            return;

        if (value >= 0.50f)
        {
            image.color =
                fullGreenColor;
        }
        else if (value >= 0.20f)
        {
            image.color =
                midColor;
        }
        else
        {
            image.color =
                lowColor;
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
