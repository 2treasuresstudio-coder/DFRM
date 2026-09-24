using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarInfoPanelController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Car Image")]
    public Image carImage;

    public Sprite defaultCarSprite;

    [Header("Car Info")]
    public TextMeshProUGUI carNameText;

    public TextMeshProUGUI topSpeedText;
    public TextMeshProUGUI accelerationText;
    public TextMeshProUGUI handlingText;
    public TextMeshProUGUI fuelEfficiencyText;
    public TextMeshProUGUI reliabilityText;
    public TextMeshProUGUI tireManagementText;

    [Header("Rating")]
    public TextMeshProUGUI overallRatingText;

    [Header("Buttons")]
    public Button upgradeButton;

    public Button closeButton;

    [Header("Managers")]
    public CarUpgradeManager upgradeManager;

    private void Start()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(
                OnUpgradeClicked);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(
                ClosePanel);
        }

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    public void OpenPanel()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void OnUpgradeClicked()
    {
        if (CarManager.Instance == null)
            return;

        if (upgradeManager == null)
            return;

        CarData car =
            CarManager.Instance.GetPlayerCar();

        if (car == null)
            return;

        if (!upgradeManager.CanAffordUpgrade(car))
            return;

        upgradeManager.OpenUpgradePopup(car);
    }

    public void Refresh()
    {
        if (CarManager.Instance == null)
            return;

        CarData car =
            CarManager.Instance.GetPlayerCar();

        if (car == null)
        {
            ShowEmptyCar();
            return;
        }

        if (carImage != null)
        {
            if (defaultCarSprite != null)
            {
                carImage.sprite =
                    defaultCarSprite;
            }
        }

        carNameText.text =
            $"{car.CarName} Lv.{car.UpgradeLevel}";

        topSpeedText.text =
            car.TopSpeed.ToString("N0");

        accelerationText.text =
            car.Acceleration.ToString("N0");

        handlingText.text =
            car.Handling.ToString("N0");

        fuelEfficiencyText.text =
            car.FuelEfficiency.ToString("N0");

        reliabilityText.text =
            car.Reliability.ToString("N0");

        tireManagementText.text =
            car.TireManagement.ToString("N0");

        if (overallRatingText != null)
        {
            int overallRating =
                car.TopSpeed +
                car.Acceleration +
                car.Handling +
                car.FuelEfficiency +
                car.Reliability +
                car.TireManagement;

            overallRatingText.text =
                overallRating.ToString("N0");
        }

        if (upgradeManager != null)
        {
            upgradeButton.interactable =
                upgradeManager.CanAffordUpgrade(car);
        }
    }

    private void ShowEmptyCar()
    {
        if (carImage != null)
        {
            carImage.sprite =
                defaultCarSprite;
        }

        carNameText.text =
            "No Car";

        topSpeedText.text = "0";
        accelerationText.text = "0";
        handlingText.text = "0";
        fuelEfficiencyText.text = "0";
        reliabilityText.text = "0";
        tireManagementText.text = "0";

        if (overallRatingText != null)
        {
            overallRatingText.text = "0";
        }

        if (upgradeButton != null)
        {
            upgradeButton.interactable = false;
        }
    }
}