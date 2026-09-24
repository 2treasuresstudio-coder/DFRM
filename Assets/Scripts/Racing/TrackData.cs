using UnityEngine;

[CreateAssetMenu(
    fileName = "New Track",
    menuName = "Idle Racing/Track Data")]
public class TrackData : ScriptableObject
{
    public string trackName;
    public string trackDescription;

    public int requiredRating;

    public int entryFee;

    public int firstPrize;

    public int secondPrize;

    public int thirdPrize;

    public Sprite trackImage;

    [Range(0, 100)]
    public float straightPercent = 50f;

    [Range(0, 100)]
    public float cornerPercent = 30f;

    [Range(0, 100)]
    public float technicalPercent = 20f;
}