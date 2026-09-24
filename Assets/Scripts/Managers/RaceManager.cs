using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("Race")]
    public int currentLap = 0;
    public int totalLaps = 9999;

    [Header("Resources")]
    public int gold = 0;

    [Header("Economy")]
    public int refuelCost = 50;
    public int tireChangeCost = 75;

    [Header("Telemetry")]
    [Range(0, 100)]
    public float fuel = 100f;

    [Range(0, 100)]
    public float tireHealth = 100f;

    [Header("Engine Mode")]
    public bool pushMode = false;

    [Header("Boost")]
    [Range(0f, 100f)]
    public float boostEnergy = 100f;

    public float boostDrainRate = 20f;

    public float boostRechargeRate = 10f;

    public bool boostReady = true;

    [Header("Fuel Usage")]
    public float normalFuelUsage = 0.4f;
    public float pushFuelUsage = 0.8f;

    [Header("Tire Usage")]
    public float normalTireUsage = 0.2f;
    public float pushTireUsage = 0.5f;

    private bool pitStopTriggered;

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

    private void Update()
    {
        if (!CanRunSimulation())
            return;

        if (RaceFlowManager.Instance != null)
        {
            if (!RaceFlowManager.Instance.IsRaceRunning())
                return;
        }

        if (PitStopManager.Instance != null &&
            PitStopManager.Instance.IsPitStopActive())
        {
            return;
        }

        ProcessBoost();

        ProcessFuel();

        ProcessTires();

        CheckPitStopCondition();
    }

    private bool CanRunSimulation()
    {
        if (DriverManager.Instance == null)
            return false;

        DriverData driver =
            DriverManager.Instance.GetDriver(0);

        if (driver == null)
            return false;

        if (string.IsNullOrEmpty(driver.DriverName))
            return false;

        return true;
    }

    private void ProcessBoost()
    {
        if (pushMode)
        {
            boostEnergy -=
                boostDrainRate *
                Time.deltaTime;

            if (boostEnergy <= 0f)
            {
                boostEnergy = 0f;

                pushMode = false;

                boostReady = false;
            }
        }
        else
        {
            boostEnergy +=
                boostRechargeRate *
                Time.deltaTime;

            if (boostEnergy >= 100f)
            {
                boostEnergy = 100f;

                boostReady = true;
            }
        }

        boostEnergy =
            Mathf.Clamp(
                boostEnergy,
                0f,
                100f);
    }

    private void ProcessFuel()
    {
        if (pushMode)
        {
            fuel -=
                pushFuelUsage *
                Time.deltaTime;
        }
        else
        {
            fuel -=
                normalFuelUsage *
                Time.deltaTime;
        }

        fuel =
            Mathf.Clamp(
                fuel,
                0f,
                100f);
    }

    private void ProcessTires()
    {
        if (pushMode)
        {
            tireHealth -=
                pushTireUsage *
                Time.deltaTime;
        }
        else
        {
            tireHealth -=
                normalTireUsage *
                Time.deltaTime;
        }

        tireHealth =
            Mathf.Clamp(
                tireHealth,
                0f,
                100f);
    }

    private void CheckPitStopCondition()
    {
        if (pitStopTriggered)
            return;

        if (fuel <= 1f ||
            tireHealth <= 1f)
        {
            pitStopTriggered = true;

            WaypointMover mover =
                FindFirstObjectByType<WaypointMover>();

            if (PitStopManager.Instance != null)
            {
                PitStopManager.Instance
                    .StartPitStop(mover);
            }

            Debug.Log(
                "Pit Stop Required");
        }
    }

    public void RestoreVehicle()
    {
        fuel = 100f;

        tireHealth = 100f;

        pitStopTriggered = false;

        Debug.Log(
            "Fuel and Tires Restored");
    }

    public void ResetRace()
    {
        currentLap = 0;

        fuel = 100f;

        tireHealth = 100f;

        boostEnergy = 100f;

        boostReady = true;

        pitStopTriggered = false;

        pushMode = false;

        Debug.Log(
            "Race Reset");
    }

    public void CompleteLap()
    {
        if (!CanRunSimulation())
            return;

        if (RaceFlowManager.Instance != null)
        {
            if (!RaceFlowManager.Instance.IsRaceRunning())
                return;
        }

        if (PitStopManager.Instance != null &&
            PitStopManager.Instance.IsPitStopActive())
        {
            return;
        }

        currentLap++;

        gold += 10;

        Debug.Log(
            $"Lap {currentLap} Complete | Gold: {gold}");
    }

    public bool Refuel()
    {
        if (gold < refuelCost)
        {
            Debug.Log(
                "Not enough Gold to Refuel");

            return false;
        }

        gold -= refuelCost;

        fuel = 100f;

        return true;
    }

    public bool ChangeTires()
    {
        if (gold < tireChangeCost)
        {
            Debug.Log(
                "Not enough Gold to Change Tires");

            return false;
        }

        gold -= tireChangeCost;

        tireHealth = 100f;

        return true;
    }

    public void TogglePushMode()
    {
        if (!boostReady)
            return;

        pushMode = !pushMode;
    }

    public bool CanRefuel()
    {
        return gold >= refuelCost;
    }

    public bool CanChangeTires()
    {
        return gold >= tireChangeCost;
    }

    public float GetBoostPercent()
    {
        return boostEnergy / 100f;
    }
}