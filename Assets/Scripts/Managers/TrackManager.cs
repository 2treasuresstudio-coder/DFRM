using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance;

    [Header("Default Track")]
    public TrackData stockCarRaceTrack;

    [Header("Current Race")]
    public TrackData currentTrack;

    [Header("Queued Race")]
    public TrackData nextTrack;

    [Header("Available Tracks")]
    public TrackData[] availableTracks;

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
        if (stockCarRaceTrack != null)
        {
            currentTrack =
                stockCarRaceTrack;

            nextTrack =
                stockCarRaceTrack;
        }
    }

    public void SetCurrentTrack(
        TrackData track)
    {
        if (track == null)
            return;

        currentTrack =
            track;
    }

    public void QueueTrack(
        TrackData track)
    {
        if (track == null)
            return;

        nextTrack =
            track;
    }

    public void ConsumeQueuedTrack()
    {
        if (nextTrack != null)
        {
            currentTrack =
                nextTrack;
        }

        nextTrack =
            stockCarRaceTrack;
    }

    public void ResetToDefaultTrack()
    {
        currentTrack =
            stockCarRaceTrack;
    }
}