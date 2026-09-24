using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DriverInfoPanelController : MonoBehaviour
{
    [Header("Driver Info")]
    public TextMeshProUGUI driverNameText;
    public TextMeshProUGUI driverAgeText;
    public TextMeshProUGUI driverNationalityText;
    public TextMeshProUGUI driverExperienceText;
    public TextMeshProUGUI driverContractTypeText;

    [Header("Portrait")]
    public Image portraitImage;
    public Sprite defaultPortrait;

    [Header("Stat Values")]
    public TextMeshProUGUI racecraftValue;
    public TextMeshProUGUI consistencyValue;
    public TextMeshProUGUI aggressionValue;
    public TextMeshProUGUI adaptabilityValue;
    public TextMeshProUGUI fitnessValue;

    [Header("Career Stats")]
    public TextMeshProUGUI racesText;
    public TextMeshProUGUI winsText;
    public TextMeshProUGUI podiumsText;
    public TextMeshProUGUI championshipsText;

    [Header("Trait Banner")]
    public Image traitIcon;

    public Sprite blankTraitSprite;

    [Header("Buttons")]
    public Button driver1Button;
    public Button driver2Button;

    public Button upgradeButton;
    public Button hireDriverButton;

    [Header("Upgrade System")]
    public DriverUpgradeManager upgradeManager;

    [Header("Settings")]
    public int hireSecondDriverCost = 500;

    private void Start()
    {
        if (driver1Button != null)
            driver1Button.onClick.AddListener(SelectDriver1);

        if (driver2Button != null)
            driver2Button.onClick.AddListener(SelectDriver2);

        if (hireDriverButton != null)
            hireDriverButton.onClick.AddListener(OnHireDriverClicked);

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClicked);

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    public void SelectDriver1()
    {
        DriverManager.Instance.SelectDriver(0);
        Refresh();
    }

    public void SelectDriver2()
    {
        DriverManager.Instance.SelectDriver(1);
        Refresh();
    }

    private void OnHireDriverClicked()
    {
        if (RaceManager.Instance.gold < hireSecondDriverCost)
        {
            Debug.Log("Not enough gold.");
            return;
        }

        RaceManager.Instance.gold -= hireSecondDriverCost;

        DriverManager.Instance.secondDriverUnlocked = true;

        RecruitmentManager.Instance.GenerateRecruitmentEvent();

        Refresh();
    }

    private void OnUpgradeClicked()
    {
        DriverData driver =
            DriverManager.Instance.GetSelectedDriver();

        if (driver == null)
            return;

        if (upgradeManager == null)
            return;

        if (!upgradeManager.CanAffordUpgrade(driver))
            return;

        upgradeManager.OpenUpgradePopup(driver);
    }

    public void Refresh()
    {
        if (DriverManager.Instance == null)
            return;

        int selectedIndex =
            DriverManager.Instance.GetSelectedDriverIndex();

        DriverData driver =
            DriverManager.Instance.GetSelectedDriver();

        bool driverExists =
            driver != null &&
            !string.IsNullOrEmpty(driver.DriverName);

        // DRIVER 2 EMPTY SLOT
        if (selectedIndex == 1 &&
            !driverExists)
        {
            ShowEmptyDriverSlot();
            return;
        }

        // DRIVER 1 EMPTY SLOT
        if (selectedIndex == 0 &&
            !driverExists)
        {
            ShowEmptyDriverSlot();
            return;
        }

        PopulateDriver(driver);

        // Driver exists
        upgradeButton.gameObject.SetActive(true);
        hireDriverButton.gameObject.SetActive(false);

        if (upgradeManager != null)
        {
            upgradeButton.interactable =
                upgradeManager.CanAffordUpgrade(driver);
        }
    }

    private void PopulateDriver(
        DriverData driver)
    {
        driverNameText.text =
            driver.DriverName;

        driverAgeText.text =
            driver.Age.ToString();

        driverNationalityText.text =
            driver.Nationality;

        driverExperienceText.text =
            driver.ExperienceLevel;

        driverContractTypeText.text =
            driver.ContractType;

        if (portraitImage != null)
        {
            portraitImage.sprite =
                driver.Portrait;
        }

        racecraftValue.text =
            driver.Racecraft.ToString("N0");

        consistencyValue.text =
            driver.Consistency.ToString("N0");

        aggressionValue.text =
            driver.Aggression.ToString("N0");

        adaptabilityValue.text =
            driver.Adaptability.ToString("N0");

        fitnessValue.text =
            driver.Fitness.ToString("N0");

        racesText.text =
            driver.Races.ToString("N0");

        winsText.text =
            driver.Wins.ToString("N0");

        podiumsText.text =
            driver.Podiums.ToString("N0");

        championshipsText.text =
            driver.Championships.ToString("N0");

        LoadTraitBanner(
            traitIcon,
            driver.Trait);
    }

    private void ShowEmptyDriverSlot()
    {
        driverNameText.text = "None";
        driverAgeText.text = "00";
        driverNationalityText.text = "NA";
        driverExperienceText.text = "NA";
        driverContractTypeText.text = "NA";

        if (portraitImage != null)
        {
            portraitImage.sprite =
                defaultPortrait;
        }

        racecraftValue.text = "0";
        consistencyValue.text = "0";
        aggressionValue.text = "0";
        adaptabilityValue.text = "0";
        fitnessValue.text = "0";

        racesText.text = "0";
        winsText.text = "0";
        podiumsText.text = "0";
        championshipsText.text = "0";

        traitIcon.sprite =
            blankTraitSprite;

        upgradeButton.gameObject.SetActive(false);

        hireDriverButton.gameObject.SetActive(
            DriverManager.Instance.GetSelectedDriverIndex() == 1);

        hireDriverButton.interactable =
            RaceManager.Instance.gold >=
            hireSecondDriverCost;
    }

    private void LoadTraitBanner(
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
                .GetTraitSprite(
                    traitName);
    }
}