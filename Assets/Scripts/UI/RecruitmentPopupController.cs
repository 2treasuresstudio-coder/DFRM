using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentPopupController : MonoBehaviour
{
    [Header("Popup")]
    private bool freeRecruitment;

    public GameObject popupRoot;

    [Header("Driver Info")]
    public TextMeshProUGUI driverNameText;
    public TextMeshProUGUI driverAgeText;
    public TextMeshProUGUI driverNationalityText;
    public TextMeshProUGUI driverExperienceText;
    public TextMeshProUGUI driverContractTypeText;

    public Image portraitImage;

    [Header("Stat Values")]
    public TextMeshProUGUI racecraftValue;
    public TextMeshProUGUI consistencyValue;
    public TextMeshProUGUI aggressionValue;
    public TextMeshProUGUI adaptabilityValue;
    public TextMeshProUGUI fitnessValue;

    [Header("Trait Icon")]
    public Image traitIcon;

    public Sprite blankTraitSprite;

    [Header("Cost")]
    public TextMeshProUGUI costText;

    [Header("First Driver Setup")]
    public TextMeshProUGUI rejectInfoText;

    [Header("Buttons")]
    public Button signButton;
    public Button rejectButton;

    private DriverData currentCandidate;

    private bool mandatoryRecruitment;

    private int rejectCount;

    private const int maxRejects = 3;

    private void Start()
    {
        signButton.onClick.AddListener(OnSignClicked);
        rejectButton.onClick.AddListener(OnRejectClicked);
    }

    public void ShowCandidate(
        DriverData candidate)
    {
        mandatoryRecruitment = false;
        freeRecruitment = false;

        currentCandidate = candidate;

        PopulateDriver(candidate);

        rejectButton.gameObject.SetActive(true);
        rejectButton.interactable = true;

        if (rejectInfoText != null)
        {
            rejectInfoText.gameObject.SetActive(false);
        }

        popupRoot.SetActive(true);
    }

    public void ShowMandatoryCandidate(
        DriverData candidate)
    {
        mandatoryRecruitment = true;
        freeRecruitment = true;

        rejectCount = 0;

        currentCandidate = candidate;

        PopulateDriver(candidate);

        rejectButton.gameObject.SetActive(true);
        rejectButton.interactable = true;

        if (rejectInfoText != null)
        {
            rejectInfoText.gameObject.SetActive(true);

            rejectInfoText.text =
                "You can only reject 3 times.";
        }

        popupRoot.SetActive(true);
    }

    private void PopulateDriver(
        DriverData candidate)
    {
        driverNameText.text =
            candidate.DriverName;

        driverAgeText.text =
            candidate.Age.ToString();

        driverNationalityText.text =
            candidate.Nationality;

        driverExperienceText.text =
            candidate.ExperienceLevel;

        driverContractTypeText.text =
            candidate.ContractType;

        if (portraitImage != null)
        {
            portraitImage.sprite =
                candidate.Portrait;
        }

        racecraftValue.text =
            candidate.Racecraft.ToString("N0");

        consistencyValue.text =
            candidate.Consistency.ToString("N0");

        aggressionValue.text =
            candidate.Aggression.ToString("N0");

        adaptabilityValue.text =
            candidate.Adaptability.ToString("N0");

        fitnessValue.text =
            candidate.Fitness.ToString("N0");

        LoadTrait(
            traitIcon,
            candidate.Trait);

        if (freeRecruitment)
        {
            costText.text =
                "Hiring Cost: FREE";
        }
        else
        {
            costText.text =
                $"Hiring Cost: {CalculateHiringCost(candidate):N0} Gold";
        }
    }

    private int CalculateHiringCost(
        DriverData driver)
    {
        int overall =
            (driver.Racecraft +
             driver.Consistency +
             driver.Aggression +
             driver.Adaptability +
             driver.Fitness) / 5;

        return overall * 2;
    }

    private void LoadTrait(
        Image target,
        string traitName)
    {
        if (string.IsNullOrEmpty(traitName))
        {
            target.sprite =
                blankTraitSprite;

            return;
        }

        if (TraitDatabase.Instance == null)
            return;

        target.sprite =
            TraitDatabase.Instance
                .GetTraitSprite(traitName);
    }

    private void OnSignClicked()
    {
        bool success =
            DriverManager.Instance.AddDriver(
                currentCandidate);

        if (!success)
            return;

        popupRoot.SetActive(false);

        if (RaceTrackVisualManager.Instance != null)
        {
            RaceTrackVisualManager.Instance
                .SpawnRaceGrid();
        }

        Debug.Log(
            $"Signed Driver: {currentCandidate.DriverName}");

        Debug.Log(
            $"Racecraft: {currentCandidate.Racecraft}");

        Debug.Log(
            $"Trait: {currentCandidate.Trait}");
    }

    private void OnRejectClicked()
    {
        if (!mandatoryRecruitment)
        {
            popupRoot.SetActive(false);

            Debug.Log(
                "Driver Rejected");

            return;
        }

        rejectCount++;

        if (rejectCount >= maxRejects)
        {
            rejectButton.interactable = false;

            if (rejectInfoText != null)
            {
                rejectInfoText.text =
                    "You have rejected 3 times. Cannot reject anymore.";
            }

            return;
        }

        int remainingRejects =
            maxRejects - rejectCount;

        if (rejectInfoText != null)
        {
            if (remainingRejects == 1)
            {
                rejectInfoText.text =
                    "You can only reject 1 time.";
            }
            else
            {
                rejectInfoText.text =
                    $"You can only reject {remainingRejects} times.";
            }
        }

        currentCandidate =
            DriverManager.Instance.GenerateRandomDriver(true);

        PopulateDriver(currentCandidate);
    }
}