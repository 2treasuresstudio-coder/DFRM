using UnityEngine;

public class DriverManager : MonoBehaviour
{
    public static DriverManager Instance;

    [Header("Portraits")]
    public Sprite[] malePortraits;
    public Sprite[] femalePortraits;

    [Header("Driver Roster")]
    public DriverData[] drivers = new DriverData[2];

    [SerializeField]
    private int selectedDriverIndex = 0;

    [Header("Roster Settings")]
    public bool secondDriverUnlocked = false;

    public int secondDriverUnlockCost = 500;

    private readonly string[] maleFirstNames =
    {
        "Alex",
        "Jake",
        "Noah",
        "Ryan"
    };

    private readonly string[] femaleFirstNames =
    {
        "Mia",
        "Sarah",
        "Ella",
        "Sophia"
    };

    private readonly string[] lastNames =
    {
        "Carter",
        "Walker",
        "Torres",
        "Smith",
        "Johnson",
        "Taylor",
        "Green",
        "Adams"
    };

    private readonly string[] nationalities =
    {
        "Canada",
        "USA",
        "Philippines",
        "Australia",
        "Japan",
        "Brazil",
        "United Kingdom"
    };

    private readonly string[] contractTypes =
    {
        "Short Term",
        "Season Contract",
        "Development Contract"
    };

    private readonly string[] experienceLevels =
    {
        "Rookie",
        "Junior",
        "Experienced",
        "Expert"
    };

    private readonly string[] availableTraits =
    {
        "Fan Favorite",
        "Fast Starter",
        "Fuel Saver",
        "Tire Whisperer",
        "Late Braker",
        "Overtake Specialist",
        "Risk Taker",
        "Ice Cold"
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

        drivers = new DriverData[2];

        drivers[0] = null;
        drivers[1] = null;
    }

    public bool HasAnyDriver()
    {
        if (drivers[0] == null)
            return false;

        return !string.IsNullOrEmpty(
            drivers[0].DriverName);
    }

    public DriverData GenerateRandomDriver(
        bool forceRookie = false)
    {
        DriverData driver = new DriverData();

        bool isFemale = Random.value > 0.5f;

        driver.IsFemale = isFemale;

        if (isFemale)
        {
            driver.DriverName =
                femaleFirstNames[
                    Random.Range(
                        0,
                        femaleFirstNames.Length)]
                + " " +
                lastNames[
                    Random.Range(
                        0,
                        lastNames.Length)];

            if (femalePortraits != null &&
                femalePortraits.Length > 0)
            {
                driver.Portrait =
                    femalePortraits[
                        Random.Range(
                            0,
                            femalePortraits.Length)];
            }
        }
        else
        {
            driver.DriverName =
                maleFirstNames[
                    Random.Range(
                        0,
                        maleFirstNames.Length)]
                + " " +
                lastNames[
                    Random.Range(
                        0,
                        lastNames.Length)];

            if (malePortraits != null &&
                malePortraits.Length > 0)
            {
                driver.Portrait =
                    malePortraits[
                        Random.Range(
                            0,
                            malePortraits.Length)];
            }
        }

        driver.Nationality =
            nationalities[
                Random.Range(
                    0,
                    nationalities.Length)];

        driver.ContractType =
            contractTypes[
                Random.Range(
                    0,
                    contractTypes.Length)];

        //if (forceRookie)
        //{
        //    driver.ExperienceLevel = "Rookie";
        //}
        //else
        //{
        //    driver.ExperienceLevel =
        //        experienceLevels[
        //            Random.Range(
        //                0,
        //                experienceLevels.Length)];
        //}

        driver.ExperienceLevel = "Rookie";

        switch (driver.ExperienceLevel)
        {
            case "Rookie":

                driver.Age =
                    Random.Range(1, 21);

                driver.Racecraft =
                    Random.Range(1, 10);

                driver.Consistency =
                    Random.Range(1, 10);

                driver.Aggression =
                    Random.Range(1, 10);

                driver.Adaptability =
                    Random.Range(1, 10);

                driver.Fitness =
                    Random.Range(1, 10);

                driver.Potential =
                    Random.Range(1, 10);

                break;

            case "Junior":

                driver.Age =
                    Random.Range(18, 25);

                driver.Racecraft =
                    Random.Range(500, 2001);

                driver.Consistency =
                    Random.Range(500, 2001);

                driver.Aggression =
                    Random.Range(500, 2001);

                driver.Adaptability =
                    Random.Range(500, 2001);

                driver.Fitness =
                    Random.Range(500, 2001);

                driver.Potential =
                    Random.Range(75, 96);

                break;

            case "Experienced":

                driver.Age =
                    Random.Range(22, 31);

                driver.Racecraft =
                    Random.Range(2000, 10001);

                driver.Consistency =
                    Random.Range(2000, 10001);

                driver.Aggression =
                    Random.Range(2000, 10001);

                driver.Adaptability =
                    Random.Range(2000, 10001);

                driver.Fitness =
                    Random.Range(2000, 10001);

                driver.Potential =
                    Random.Range(65, 86);

                break;

            case "Expert":

                driver.Age =
                    Random.Range(28, 41);

                driver.Racecraft =
                    Random.Range(10000, 50001);

                driver.Consistency =
                    Random.Range(10000, 50001);

                driver.Aggression =
                    Random.Range(10000, 50001);

                driver.Adaptability =
                    Random.Range(10000, 50001);

                driver.Fitness =
                    Random.Range(10000, 50001);

                driver.Potential =
                    Random.Range(50, 76);

                break;
        }

        driver.UpgradeLevel = 0;

        driver.Trait =
            availableTraits[
                Random.Range(
                    0,
                    availableTraits.Length)];


        driver.Wins = 0;
        driver.Podiums = 0;
        driver.Championships = 0;

        return driver;
    }

    public bool CanUnlockSecondDriver()
    {
        if (secondDriverUnlocked)
            return false;

        return RaceManager.Instance.gold >=
               secondDriverUnlockCost;
    }

    public bool UnlockSecondDriver()
    {
        if (secondDriverUnlocked)
            return false;

        if (RaceManager.Instance.gold <
            secondDriverUnlockCost)
            return false;

        RaceManager.Instance.gold -=
            secondDriverUnlockCost;

        secondDriverUnlocked = true;

        Debug.Log(
            "Second Driver Slot Unlocked");

        return true;
    }

    public bool AddDriver(DriverData driver)
    {
        bool slot1Empty =
            drivers[0] == null ||
            string.IsNullOrEmpty(
                drivers[0].DriverName);

        if (slot1Empty)
        {
            drivers[0] = driver;

            selectedDriverIndex = 0;

            Debug.Log(
                $"First Driver Signed: {driver.DriverName}");

            return true;
        }

        if (!secondDriverUnlocked)
        {
            Debug.Log(
                "Second Driver Slot Locked.");

            return false;
        }

        bool slot2Empty =
            drivers[1] == null ||
            string.IsNullOrEmpty(
                drivers[1].DriverName);

        if (slot2Empty)
        {
            drivers[1] = driver;

            Debug.Log(
                $"Second Driver Signed: {driver.DriverName}");

            return true;
        }

        Debug.Log(
            "Driver Slot 2 already occupied.");

        return false;
    }

    public DriverData GetDriver(int index)
    {
        if (index < 0 ||
            index >= drivers.Length)
            return null;

        return drivers[index];
    }

    public DriverData GetSelectedDriver()
    {
        return drivers[selectedDriverIndex];
    }

    public void SelectDriver(int index)
    {
        if (index < 0 ||
            index >= drivers.Length)
            return;

        selectedDriverIndex = index;
    }

    public bool IsSlotOccupied(int index)
    {
        if (index < 0 ||
            index >= drivers.Length)
            return false;

        if (drivers[index] == null)
            return false;

        return !string.IsNullOrEmpty(
            drivers[index].DriverName);
    }

    public int GetSelectedDriverIndex()
    {
        return selectedDriverIndex;
    }
}