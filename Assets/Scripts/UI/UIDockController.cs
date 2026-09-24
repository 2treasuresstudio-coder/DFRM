using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDockController : MonoBehaviour
{
    [Header("Race UI")]
    public TextMeshProUGUI goldText;

    public TextMeshProUGUI lapText;

    public TextMeshProUGUI fuelText;

    public TextMeshProUGUI tireText;

    [Header("Team UI")]
    public TextMeshProUGUI companyNameText;

    [Header("Driver UI")]
    public TextMeshProUGUI driverNameText;

    public TextMeshProUGUI driverAgeText;

    public TextMeshProUGUI driverRacecraftText;

    [Header("Buttons")]
    public Button refuelButton;

    public Button tireButton;

    public Button pushModeButton;

    public Button recruitDriverButton;

    private void Start()
    {
        if (refuelButton != null)
        {
            refuelButton.onClick.AddListener(
                OnRefuelClicked);
        }

        if (tireButton != null)
        {
            tireButton.onClick.AddListener(
                OnTiresClicked);
        }

        if (pushModeButton != null)
        {
            pushModeButton.onClick.AddListener(
                OnPushModeClicked);
        }

        if (recruitDriverButton != null)
        {
            recruitDriverButton.onClick.AddListener(
                OnRecruitDriverClicked);
        }
    }

    private void Update()
    {
        UpdateRaceUI();

        UpdateDriverUI();

        UpdateTeamUI();
    }

    private void UpdateRaceUI()
    {
        if (RaceManager.Instance == null)
            return;

        goldText.text =
            $"Gold: {RaceManager.Instance.gold:N0}";

        lapText.text =
            $"Lap: {RaceManager.Instance.currentLap}";

        fuelText.text =
            $"Fuel: {RaceManager.Instance.fuel:F0}%";

        tireText.text =
            $"Tires: {RaceManager.Instance.tireHealth:F0}%";

        if (refuelButton != null)
        {
            refuelButton.interactable =
                RaceManager.Instance.CanRefuel();
        }

        if (tireButton != null)
        {
            tireButton.interactable =
                RaceManager.Instance.CanChangeTires();
        }
    }

    private void UpdateTeamUI()
    {
        if (companyNameText == null)
            return;

        if (TeamManager.Instance == null)
            return;

        companyNameText.text =
            TeamManager.Instance.companyName;
    }

    private void UpdateDriverUI()
    {
        if (DriverManager.Instance == null)
            return;

        DriverData driver =
            DriverManager.Instance.GetSelectedDriver();

        if (driver == null)
            return;

        if (driverNameText != null)
        {
            driverNameText.text =
                $"Driver: {driver.DriverName}";
        }

        if (driverAgeText != null)
        {
            driverAgeText.text =
                $"Age: {driver.Age}";
        }

        if (driverRacecraftText != null)
        {
            driverRacecraftText.text =
                $"Racecraft: {driver.Racecraft}";
        }
    }

    private void OnRefuelClicked()
    {
        if (RaceManager.Instance != null)
        {
            RaceManager.Instance.Refuel();
        }
    }

    private void OnTiresClicked()
    {
        if (RaceManager.Instance != null)
        {
            RaceManager.Instance.ChangeTires();
        }
    }

    private void OnPushModeClicked()
    {
        if (RaceManager.Instance != null)
        {
            RaceManager.Instance.TogglePushMode();
        }
    }

    private void OnRecruitDriverClicked()
    {
        if (RecruitmentManager.Instance != null)
        {
            RecruitmentManager.Instance
                .GenerateRecruitmentEvent();
        }
    }
}