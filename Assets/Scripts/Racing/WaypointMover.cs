using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [Header("Track")]
    public Transform[] waypoints;

    [Header("Speed")]
    public float normalSpeed = 3f;
    public float pushSpeed = 15f;

    private int currentWaypoint = 0;

    private void Update()
    {
        if (DriverManager.Instance == null)
            return;

        DriverData driver =
            DriverManager.Instance.GetDriver(0);

        if (driver == null)
            return;

        if (string.IsNullOrEmpty(driver.DriverName))
            return;

        if (RaceFlowManager.Instance != null)
        {
            if (!RaceFlowManager.Instance.IsRaceRunning())
                return;
        }

        if (PitStopManager.Instance != null)
        {
            if (PitStopManager.Instance.IsPitStopActive())
                return;
        }

        if (waypoints == null || waypoints.Length == 0)
            return;

        float currentSpeed = normalSpeed;

        if (RaceManager.Instance != null)
        {
            if (RaceManager.Instance.pushMode)
            {
                currentSpeed = pushSpeed;
            }
        }

        Transform targetWaypoint =
            waypoints[currentWaypoint];

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                targetWaypoint.position,
                currentSpeed * Time.deltaTime);

        float distanceToTarget =
            Vector2.Distance(
                transform.position,
                targetWaypoint.position);

        if (distanceToTarget < 0.05f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;

                if (RaceManager.Instance != null)
                {
                    RaceManager.Instance.CompleteLap();
                }
            }
        }
    }
}