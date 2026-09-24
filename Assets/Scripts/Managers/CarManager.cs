using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager Instance;

    [Header("Player Car")]
    public CarData playerCar;

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

        CreateStarterCar();
    }

    private void CreateStarterCar()
    {
        playerCar = new CarData();

        playerCar.CarName = "Starter Chassis";

        playerCar.TopSpeed = 100;

        playerCar.Acceleration = 100;

        playerCar.Handling = 100;

        playerCar.Reliability = 100;

        playerCar.FuelEfficiency = 100;

        playerCar.TireManagement = 100;

        playerCar.UpgradeLevel = 0;
    }

    public CarData GetPlayerCar()
    {
        return playerCar;
    }
}