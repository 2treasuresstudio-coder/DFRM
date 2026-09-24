using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DriverUpgradeManager : MonoBehaviour
{
    public static DriverUpgradeManager Instance;

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

    private DriverData selectedDriver;

    private string option1;
    private string option2;
    private string option3;

    private int currentUpgradeCost;

    private readonly string[] upgradePool =
    {
        "Racecraft",
        "Consistency",
        "Aggression",
        "Adaptability",
        "Fitness"
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
        popupRoot.SetActive(false);

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
        DriverData driver)
    {
        return 100 +
               (driver.UpgradeLevel * 50);
    }

    public bool CanAffordUpgrade(
        DriverData driver)
    {
        if (driver == null)
            return false;

        return RaceManager.Instance.gold >=
               GetUpgradeCost(driver);
    }

    public void OpenUpgradePopup(
        DriverData driver)
    {
        selectedDriver = driver;

        currentUpgradeCost =
            GetUpgradeCost(driver);

        costText.text =
            $"Upgrade Cost: {currentUpgradeCost:N0} Gold";

        List<string> choices =
            new List<string>(upgradePool);

        option1 = GetRandomUpgrade(choices);
        option2 = GetRandomUpgrade(choices);
        option3 = GetRandomUpgrade(choices);

        option1Text.text =
            GetUpgradeDescription(option1);

        option2Text.text =
            GetUpgradeDescription(option2);

        option3Text.text =
            GetUpgradeDescription(option3);

        popupRoot.SetActive(true);
    }

    private string GetRandomUpgrade(
        List<string> availableChoices)
    {
        int index =
            Random.Range(
                0,
                availableChoices.Count);

        string choice =
            availableChoices[index];

        availableChoices.RemoveAt(index);

        return choice;
    }

    private string GetUpgradeDescription(
        string upgradeType)
    {
        return $"+25% {upgradeType}";
    }

    private void ApplyUpgrade(
        string upgradeType)
    {
        if (selectedDriver == null)
            return;

        RaceManager.Instance.gold -=
            currentUpgradeCost;

        switch (upgradeType)
        {
            case "Racecraft":

                selectedDriver.Racecraft +=
                    Mathf.Max(
                        1,
                        Mathf.RoundToInt(
                            selectedDriver.Racecraft * 2f)); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Consistency":

                selectedDriver.Consistency +=
                    Mathf.Max(
                        1,
                        Mathf.RoundToInt(
                            selectedDriver.Consistency * 2f)); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Aggression":

                selectedDriver.Aggression +=
                    Mathf.Max(
                        1,
                        Mathf.RoundToInt(
                            selectedDriver.Aggression * 2f)); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Adaptability":

                selectedDriver.Adaptability +=
                    Mathf.Max(
                        1,
                        Mathf.RoundToInt(
                            selectedDriver.Adaptability * 2f)); //===!THIS IS FOR DEMO ONLY !===//

                break;

            case "Fitness":

                selectedDriver.Fitness +=
                    Mathf.Max(
                        1,
                        Mathf.RoundToInt(
                            selectedDriver.Fitness * 2f)); //===!THIS IS FOR DEMO ONLY !===//

                break;
        }

        selectedDriver.UpgradeLevel++;

        popupRoot.SetActive(false);

        Debug.Log(
            $"Upgrade Applied: {upgradeType}");

        Debug.Log(
            $"Upgrade Level: {selectedDriver.UpgradeLevel}");

        Debug.Log(
            $"Next Upgrade Cost: {GetUpgradeCost(selectedDriver):N0}");
    }

    private void ClosePopup()
    {
        popupRoot.SetActive(false);
    }
}