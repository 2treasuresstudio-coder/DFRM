using System.Collections.Generic;
using UnityEngine;

public class TraitDatabase : MonoBehaviour
{
    public static TraitDatabase Instance;

    [System.Serializable]
    public class TraitEntry
    {
        public string traitName;
        public Sprite traitSprite;
    }

    [Header("Trait Library")]
    public List<TraitEntry> traits =
        new List<TraitEntry>();

    private Dictionary<string, Sprite> traitLookup;

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

        BuildLookup();
    }

    private void BuildLookup()
    {
        traitLookup =
            new Dictionary<string, Sprite>();

        foreach (TraitEntry entry in traits)
        {
            if (!traitLookup.ContainsKey(entry.traitName))
            {
                traitLookup.Add(
                    entry.traitName,
                    entry.traitSprite);
            }
        }
    }

    public Sprite GetTraitSprite(string traitName)
    {
        if (traitLookup.TryGetValue(
            traitName,
            out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning(
            $"Trait sprite not found: {traitName}");

        return null;
    }
}
