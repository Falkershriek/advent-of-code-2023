using AdventOfCode2023.AdventDays.Day4Old;
using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day5
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @"D:\Home\Projects\Software\AdventOfCode2023\AdventDays\Day5\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Types
        private record MapEntry(long Destination, long Source, long Span);
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private List<long> GetSeeds(List<string> puzzleLines)
            => puzzleLines[0].Split(' ')
                             .Skip(1)
                             .Select(x => long.Parse(x))
                             .ToList();
        private List<List<MapEntry>> GetMaps(List<string> puzzleLines)
        {
            List<List<MapEntry>> maps = new List<List<MapEntry>>();

            for (int i = 2; i < puzzleLines.Count; i++)
            {
                List<MapEntry> map;
                if (puzzleLines[i].Contains("map"))
                {
                    i++;
                    map = new List<MapEntry>();

                    while (i < puzzleLines.Count && puzzleLines[i] != "")
                    {
                        long[] currentValues = puzzleLines[i].Split(' ').Select(x => long.Parse(x)).ToArray();
                        map.Add(new MapEntry(currentValues[0], currentValues[1], currentValues[2]));
                        i++;
                    }

                    maps.Add(map);
                }
            }

            return maps;
        }

        /// <summary>
        /// Get the minimum source value of the given map entry.
        /// </summary>
        private long MinSource(MapEntry mapEntry)
            => mapEntry.Source;

        /// <summary>
        /// Get the maximum source value of the given map entry.
        /// </summary>
        private long MaxSource(MapEntry mapEntry)
            => mapEntry.Source + mapEntry.Span - 1;

        /// <summary>
        /// Get the minimum destination value of the given map entry.
        /// </summary>
        private long MinDestination(MapEntry mapEntry)
            => mapEntry.Destination;

        /// <summary>
        /// Get the maximum destination of the given map entry.
        /// </summary>
        private long MaxDestination(MapEntry mapEntry)
            => mapEntry.Destination + mapEntry.Span - 1;

        /// <summary>
        /// Return the span of the map entry.
        /// </summary>
        private long Span(MapEntry mapEntry)
            => mapEntry.Span;

        /// <summary>
        /// Get the span of the two values.
        /// </summary>
        private long GetSpan(long minValue, long maxValue)
            => maxValue - minValue + 1;

        /// <summary>
        /// Make a new seed entry using the provided values.
        /// </summary>
        private MapEntry GetSeedEntry(long value, long span)
            => new MapEntry(value, value, span);

        /// <summary>
        /// Return true if the given value lies within the source span of the given map entry.
        /// </summary>
        private bool SourceSpanContainsValue(MapEntry mapEntry, long value)
            => value >= MinSource(mapEntry) && value <= MaxSource(mapEntry);

        /// <summary>
        /// Return true if the given value lies within the destination span of the given map entry.
        /// </summary>
        private bool DestinationSpanContainsValue(MapEntry mapEntry, long value)
            => value >= MinDestination(mapEntry) && value <= MaxDestination(mapEntry);

        /// <summary>
        /// Return true if destination span of the source entry intersects the source span of the destination entry at any point.
        /// </summary>
        private bool IntersectionExists(MapEntry sourceEntry, MapEntry destinationEntry)
            => SourceSpanContainsValue(destinationEntry, MinDestination(sourceEntry))
            || SourceSpanContainsValue(destinationEntry, MaxDestination(sourceEntry))
            || DestinationSpanContainsValue(sourceEntry, MinSource(destinationEntry))
            || DestinationSpanContainsValue(sourceEntry, MaxSource(destinationEntry));

        /// <summary>
        /// Return true if the source's entry destination span starts before the destination entry's source span.
        /// </summary>
        private bool SourceEntryStartsBeforeDestinationEntry(MapEntry sourceEntry, MapEntry destinationEntry)
            => MinDestination(sourceEntry) < MinSource(destinationEntry);

        /// <summary>
        /// Return true if the source's entry destination span ends after the destination entry's source span.
        /// </summary>
        private bool SourceEntryEndsAfterDestinationEntry(MapEntry sourceEntry, MapEntry destinationEntry)
            => MaxDestination(sourceEntry) > MaxSource(destinationEntry);

        /// <summary>
        /// Return a part of the source entry that intersects with the destination entry.
        /// </summary>
        private MapEntry GetSourceIntersection(MapEntry sourceEntry, MapEntry destinationEntry)
        {
            if (!IntersectionExists(sourceEntry, destinationEntry))
                throw new Exception("ble");

            long sourceValue;
            long destinationValue;
            long spanValue;

            if (SourceEntryStartsBeforeDestinationEntry(sourceEntry, destinationEntry))
            {
                sourceValue = UnmappedValue(sourceEntry, MinSource(destinationEntry));
                destinationValue = MinSource(destinationEntry);
            }
            else
            {
                sourceValue = MinSource(sourceEntry);
                destinationValue = MinDestination(sourceEntry);
            }

            if (SourceEntryEndsAfterDestinationEntry(sourceEntry, destinationEntry))
                spanValue = GetSpan(destinationValue, MaxSource(destinationEntry));
            else
                spanValue = GetSpan(destinationValue, MaxDestination(sourceEntry));

            return new MapEntry(destinationValue, sourceValue, spanValue);
        }

        /// <summary>
        /// Return a part of the destination entry that intersects with the destination entry.
        /// </summary>
        private MapEntry GetDestinationIntersection(MapEntry sourceEntry, MapEntry destinationEntry)
        {
            if (!IntersectionExists(sourceEntry, destinationEntry))
                throw new Exception("ble");

            long sourceValue;
            long destinationValue;
            long spanValue;

            if (SourceEntryStartsBeforeDestinationEntry(sourceEntry, destinationEntry))
            {
                sourceValue = MinSource(destinationEntry);
                destinationValue = MinDestination(destinationEntry);
            }
            else
            {
                sourceValue = MappedValue(sourceEntry, MinSource(sourceEntry));
                destinationValue = MappedValue(destinationEntry, sourceValue);
            }

            if (SourceEntryEndsAfterDestinationEntry(sourceEntry, destinationEntry))
                spanValue = GetSpan(destinationValue, MaxDestination(destinationEntry));
            else
                spanValue = GetSpan(sourceValue, MaxDestination(sourceEntry));

            return new MapEntry(destinationValue, sourceValue, spanValue);
        }

        /// <summary>
        /// Return the parts of the source and destination entries that fully intersect with one another.
        /// </summary>
        private (MapEntry, MapEntry) GetIntersections(MapEntry sourceEntry, MapEntry destinationEntry)
        {
            if (!IntersectionExists(sourceEntry, destinationEntry))
                throw new Exception("ble");

            MapEntry sourceIntersection = GetSourceIntersection(sourceEntry, destinationEntry);
            MapEntry destinationIntersection = GetDestinationIntersection(sourceEntry, destinationEntry);

            return (sourceIntersection, destinationIntersection);
        }

        /// <summary>
        /// Return true if the two entries map perfectly onto one another.
        /// </summary>
        private bool EntriesAreEqual(MapEntry sourceEntry, MapEntry destinationEntry)
            => MinDestination(sourceEntry) == MinSource(destinationEntry)
            && MaxDestination(sourceEntry) == MaxSource(destinationEntry)
            && Span(sourceEntry) == Span(destinationEntry);

        /// <summary>
        /// Find the intersecting parts of both the entries, merge them and return the result.
        /// </summary>
        private MapEntry MergedIntersections(MapEntry sourceEntry, MapEntry destinationEntry)
        {
            if (!IntersectionExists(sourceEntry, destinationEntry))
                throw new Exception("fuck");

            (MapEntry sourceIntersection, MapEntry destinationIntersection) = GetIntersections(sourceEntry, destinationEntry);
            return MergedEntry(sourceIntersection, destinationIntersection);
        }

        /// <summary>
        /// Merge two equal map entries that map onto one another and return the result.
        /// </summary>
        private MapEntry MergedEntry(MapEntry sourceEntry, MapEntry destinationEntry)
        {
            if (!EntriesAreEqual(sourceEntry, destinationEntry))
                throw new Exception("blep");

            MapEntry mergedEntry = new MapEntry
            (
                MinDestination(destinationEntry),
                MinSource(sourceEntry),
                Span(sourceEntry)
            );

            return mergedEntry;
        }

        /// <summary>
        /// Map the given value using the given map and return the result.
        /// </summary>
        private long MappedValue(List<MapEntry> map, long value)
        {
            MapEntry? compatibleMapEntry = map.Find(x => SourceSpanContainsValue(x, value));

            if (compatibleMapEntry == null)
                throw new Exception("fug");

            return MappedValue(compatibleMapEntry, value);
        }

        /// <summary>
        /// Map the given value using the given map entry and return the result.
        /// </summary>
        private long MappedValue(MapEntry mapEntry, long value)
            => value - mapEntry.Source + mapEntry.Destination;

        /// <summary>
        /// Reverse the mapping of the given value and return the result.
        /// </summary>
        private long UnmappedValue(MapEntry mapEntry, long value)
            => value + mapEntry.Source - mapEntry.Destination;

        /// <summary>
        /// Get the map entry with the smallest source value.
        /// </summary>
        private MapEntry GetEntryWithMinSource(List<MapEntry> map)
        {
            long minSource = map.Min(x => x.Source);
            MapEntry? foundEntry = map.Find(x => x.Source == minSource);

            if (foundEntry == null) throw new Exception("ble");

            return foundEntry;
        }

        /// <summary>
        /// Get the map entry with the largest source value.
        /// </summary>
        private MapEntry GetEntryWithMaxSource(List<MapEntry> map)
        {
            long maxSource = map.Max(x => x.Source);
            MapEntry? foundEntry = map.Find(x => x.Source == maxSource);

            if (foundEntry == null) throw new Exception("ble");

            return foundEntry;
        }

        /// <summary>
        /// Complete the given map by adding the previously unmapped spans.
        /// </summary>
        private List<MapEntry> GetCompleteMap(List<MapEntry> map)
        {
            List<MapEntry> completeMap = new List<MapEntry>();
            long lastMappedSource = -1;

            while (lastMappedSource != long.MaxValue)
            {
                List<MapEntry> remainingEntries = map.FindAll(x => x.Source > lastMappedSource);

                if (remainingEntries.Count == 0)
                {
                    MapEntry mapEntry = new MapEntry
                    (
                        lastMappedSource + 1,
                        lastMappedSource + 1,
                        GetSpan(lastMappedSource + 1, long.MaxValue)
                    );

                    completeMap.Add(mapEntry);
                    break;
                }

                MapEntry lowestRemainingSourceEntry = GetEntryWithMinSource(remainingEntries);

                if (lowestRemainingSourceEntry.Source == lastMappedSource + 1)
                {
                    completeMap.Add(lowestRemainingSourceEntry);
                }
                else
                {
                    MapEntry mapEntry = new MapEntry
                    (
                        lastMappedSource + 1,
                        lastMappedSource + 1,
                        GetSpan(lastMappedSource + 1, GetEntryWithMinSource(remainingEntries).Source - 1)
                    );
                    completeMap.Add(mapEntry);
                }

                lastMappedSource = MaxSource(GetEntryWithMaxSource(completeMap));
            }

            return completeMap;
        }

        private List<MapEntry> MergedMap(List<MapEntry> sourceMap, List<MapEntry> destinationMap)
        {
            List<MapEntry> mergedMap = new List<MapEntry>();

            foreach (MapEntry sourceEntry in sourceMap)
            {
                List<MapEntry> remainingEntries = destinationMap.FindAll(x => IntersectionExists(sourceEntry, x));

                if (remainingEntries.Count == 0)
                    break;

                foreach (MapEntry destinationEntry in remainingEntries)
                    mergedMap.Add(MergedIntersections(sourceEntry, destinationEntry));
            }

            return mergedMap;
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            List<long> seeds = GetSeeds(PuzzleLines);

            List<List<MapEntry>> maps = GetMaps(PuzzleLines);
            List<List<MapEntry>> completeMaps = new List<List<MapEntry>>();

            foreach (List<MapEntry> map in maps)
                completeMaps.Add(GetCompleteMap(map));

            List<MapEntry> finalMap = completeMaps[0];

            for (int i = 1; i < completeMaps.Count; i++)
                finalMap = MergedMap(finalMap, completeMaps[i]);

            long lowestDestinationFound = long.MaxValue;

            foreach (long seed in seeds)
            {
                long currentLocation = MappedValue(finalMap, seed);
                lowestDestinationFound = (currentLocation < lowestDestinationFound) ? currentLocation : lowestDestinationFound;
            }

            return lowestDestinationFound;
        }

        public override long Part2()
        {
            List<long> seeds = GetSeeds(PuzzleLines);

            List<MapEntry> seedMap = new List<MapEntry>();

            for (int i = 0; i < seeds.Count; i += 2)
                seedMap.Add(GetSeedEntry(seeds[i], seeds[i + 1]));

            List<List<MapEntry>> maps = GetMaps(PuzzleLines);
            List<List<MapEntry>> completeMaps = new List<List<MapEntry>>();

            bool seedBoop = true;
            foreach (List<MapEntry> map in maps)
            {
                if (seedBoop)
                {
                    completeMaps.Add(seedMap);
                    seedBoop = false;
                }
                completeMaps.Add(GetCompleteMap(map));
            }

            List<MapEntry> finalMap = completeMaps[0];

            for (int i = 1; i < completeMaps.Count; i++)
                finalMap = MergedMap(finalMap, completeMaps[i]);

            return finalMap.Min(x => MinDestination(x));
        }
        #endregion
    }
}
