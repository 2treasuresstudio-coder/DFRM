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

        playerCar.TopSpeed = 10;

        playerCar.Acceleration = 10;

        playerCar.Handling = 10;

        playerCar.Reliability = 10;

        playerCar.FuelEfficiency = 10;

        playerCar.TireManagement = 10;

        playerCar.UpgradeLevel = 0;
    }

    public CarData GetPlayerCar()
    {
        return playerCar;
    }
}