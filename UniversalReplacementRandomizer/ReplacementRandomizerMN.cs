using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReplacementRandomizer;

public class ReplacementRandomizerMN
{
    private readonly SeedManager SeedManager;
    private readonly Dictionary<string, RandomizationGroupMN> Groups; // SeedManager key => RandomizationGroupMN

    public int GetBaseSeed() { return SeedManager.GetBaseSeed(); } // necessary for deterministic randomization
    public SeedManager GetSeedManager() { return SeedManager; } // caveat: could influence randomization if used outside of the ReplacementRandomizerMN

    public ReplacementRandomizerMN(string? prefix = null, int? baseSeed = null)
    {
        Groups = new Dictionary<string, RandomizationGroupMN>();
        SeedManager = new SeedManager(prefix, baseSeed);
    }

    public Dictionary<string, int[]> RandomizeAllGroups()
    {
        // shuffle every group in the Groups dictionary by iterating through all keys present
        List<string> keys = Groups.Keys.ToList();

        // format results as a dictionary of dictionaries mapping key => replacementsDictionary
        Dictionary<string, int[]> result = new();

        foreach (string key in keys)
        {
            Random rng = SeedManager.GetRandomByKey(key);  // retrieve the namespaced randomness generator
            result[key] = Groups[key].Randomize(rng); // direct access by key, since it's generated from the dictionary
        }

        return result;
    }

    public int[] RandomizeGroup(string key)
    {
        Random rng = SeedManager.GetRandomByKey(key);
        if (!Groups.TryGetValue(key, out RandomizationGroupMN? group))
        {
            throw new Exception("Unrecognized key");
        }

        return group.Randomize(rng);
    }

    public int[] RetryingRandomizeGroup(string key, int? maxAttempts = null)
    {
        Random rng = SeedManager.GetRandomByKey(key);
        if (!Groups.TryGetValue(key, out RandomizationGroupMN? group))
        {
            throw new Exception("Unrecognized key");
        }

        return group.RetryingValidatedRandomize(rng, maxAttempts);
    }

    public void AddGroup(string key, RandomizationGroupMN group)
    {
        // add a new randomization group
        if (Groups.ContainsKey(key))
        {
            throw new Exception($"Dictionary {nameof(Groups)} already contains key: {key}");
        }

        Groups.Add(key, group);
    }
}