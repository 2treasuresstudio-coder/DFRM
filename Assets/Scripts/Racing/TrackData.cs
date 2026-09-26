using UnityEngine;

[CreateAssetMenu(
    fileName = "TrackData",
    menuName = "Racing/Track Data")]
public class TrackData : ScriptableObject
{
    [Header("Track Info")]
    public string trackName;

    [TextArea]
    public string trackDescription;

    public Sprite trackImage;

    [Header("Requirements")]
    public int requiredRating;

    public int entryFee;

    [Header("Prizes")]
    public int firstPrize;

    public int secondPrize;

    public int thirdPrize;

    [Header("Track Layout")]
    [Range(0, 100)]
    public int straightPercent;

    [Range(0, 100)]
    public int cornerPercent;

    [Range(0, 100)]
    public int technicalPercent;

    [Header("League")]
    public bool isLeagueEvent;
}