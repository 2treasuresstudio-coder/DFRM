using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUpgradeManager : MonoBehaviour
{
    public static CarUpgradeManager Instance;

    [Header("Popup")]
    public GameObject popupRoot;

    [Header("Cost")]
    public TextMeshProUGUI costText;

    [Header("Option Texts")]
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;

    [Header("Buttons")]
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;

    public Button closeButton;

    private CarData selectedCar;

    private string option1;
    private string option2;
    private string option3;

    private int currentUpgradeCost;

    private readonly string[] upgradePool =
    {
        "TopSpeed",
        "Acceleration",
        "Handling",
        "FuelEfficiency",
        "Reliability",
        "TireManagement"
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
            return;
        }
    }

    private void Start()
    {
        if (popupRoot != null)
        {
            popupRoot.SetActive(false);
        }

        option1Button.onClick.AddListener(
            () => ApplyUpgrade(option1));

        option2Button.onClick.AddListener(
            () => ApplyUpgrade(option2));

        option3Button.onClick.AddListener(
            () => ApplyUpgrade(option3));

        closeButton.onClick.AddListener(
            ClosePopup);
    }

    public int GetUpgradeCost(
        CarData car)
    {
        return 100 +
               (car.UpgradeLevel * 50);
    }

    public bool CanAffordUpgrade(
        CarData car)
    {
        if (car == null)
            return false;

        return RaceManager.Instance.gold >=
               GetUpgradeCost(car);
    }

    public void OpenUpgradePopup(
        CarData car)
    {
        selectedCar = car;

        currentUpgradeCost =
            GetUpgradeCost(car);

        costText.text =
            $"Upgrade Cost: {currentUpgradeCost:N0} Gold";

        List<string> choices =
            new List<string>(upgradePool);

        option1 =
            GetRandomChoice(choices);

        option2 =
            GetRandomChoice(choices);

        option3 =
            GetRandomChoice(choices);

        option1Text.text =
            GetDisplayText(option1);

        option2Text.text =
            GetDisplayText(option2);

        option3Text.text =
            GetDisplayText(option3);

        popupRoot.SetActive(true);
    }

    private string GetRandomChoice(
        List<string> choices)
    {
        int index =
            Random.Range(
                0,
                choices.Count);

        string choice =
            choices[index];

        choices.RemoveAt(index);

        return choice;
    }

    private string GetDisplayText(
        string upgradeType)
    {
        return $"+25% {upgradeType}";
    }

    private void ApplyUpgrade(
        string upgradeType)
    {
        if (selectedCar == null)
            return;

        RaceManager.Instance.gold -=
            currentUpgradeCost;

        switch (upgradeType)
        {
            case "TopSpeed":

                selectedCar.TopSpeed +=
                    Mathf.RoundToInt(
                        selectedCar.TopSpeed * 0.5f); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Acceleration":

                selectedCar.Acceleration +=
                    Mathf.RoundToInt(
                        selectedCar.Acceleration * 0.5f); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Handling":

                selectedCar.Handling +=
                    Mathf.RoundToInt(
                        selectedCar.Handling * 0.5f); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "FuelEfficiency":

                selectedCar.FuelEfficiency +=
                    Mathf.RoundToInt(
                        selectedCar.FuelEfficiency * 0.5f); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Reliability":

                selectedCar.Reliability +=
                    Mathf.RoundToInt(
                        selectedCar.Reliability * 0.5f); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "TireManagement":

                selectedCar.TireManagement +=
                    Mathf.RoundToInt(
                        selectedCar.TireManagement * 0.5f); //===!THIS IS FOR DEMO ONLY !===//

                break;
        }

        selectedCar.UpgradeLevel++;

        popupRoot.SetActive(false);

        Debug.Log(
            $"Car Upgrade Applied: {upgradeType}");

        Debug.Log(
            $"Car Upgrade Level: {selectedCar.UpgradeLevel}");
    }

    private void ClosePopup()
    {
        if (popupRoot != null)
        {
            popupRoot.SetActive(false);
        }
    }
}