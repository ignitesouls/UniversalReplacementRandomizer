﻿// SPDX-License-Identifier: GPL-3.0-only
namespace UniversalReplacementRandomizer;

public static class ListRandomizationExtensions
{
    // FisherYates shuffle is a uniformly random shuffle algorithm: any permutation is equally likely
    public static void FisherYatesShuffle<T>(this IList<T> list, Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);                 // choose a random index to replace the value at the current position of the array
            (list[i], list[j]) = (list[j], list[i]); // swap the values
        }
    }

    // same thing, but start from the 0-index of the array instead of the other end.
    public static void ReverseFisherYatesShuffle<T>(this IList<T> list, Random rng)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int j = rng.Next(i, list.Count);         // choose a random index to replace the value at the current position of the array
            (list[i], list[j]) = (list[j], list[i]); // swap the values
        }
    }

    // a generator that performs the FisherYates shuffle one step at a time.
    public static IEnumerable<(int, T)> LazyFisherYatesShuffle<T>(this IList<T> list, Random rng)
    {
        // the last value is implicitly chosen (i > 0)
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);                 // choose a random index to replace the value at the current position of the array
            (list[i], list[j]) = (list[j], list[i]); // swap the values
            yield return (i, list[i]);               // yield the current index and its finalized replacement
        }
        yield return (0, list[0]);                   // yield the final index and its finalized replacement
    }

    // a generator that performs the FisherYates shuffle one step at a time, in reverse.
    public static IEnumerable<(int, T)> LazyReverseFisherYatesShuffle<T>(this IList<T> list, Random rng)
    {
        // the last value is implicitly chosen (i < list.Count - 1)
        for (int i = 0; i < list.Count - 1; i++)
        {
            int j = rng.Next(i, list.Count);         // choose a random index to replace the value at the current position of the array
            (list[i], list[j]) = (list[j], list[i]); // swap the values
            yield return (i, list[i]);               // yield the current index and its finalized replacement
        }
        yield return (list.Count - 1, list[list.Count - 1]); // yield the final index and its finalized replacement
    }
}
