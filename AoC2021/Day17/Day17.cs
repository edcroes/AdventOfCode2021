﻿using System.Text.RegularExpressions;

namespace AoC2021.Day17;

public class Day17 : IMDay
{
    private static readonly Regex _bucketRegex = new Regex(@"^target area: x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)\s*$"); 
    public string FilePath { private get; init; } = "Day17\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var bucket = await GetBucket();
        var velocitiesToTest = GetVelocityCandidates(bucket);

        var maxHeight = velocitiesToTest
            .Select(v => GetMaxHeightOnHit(bucket, v))
            .Where(v => v.HasValue)
            .Select(v => v.Value)
            .Max(h => h);

        return maxHeight.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var bucket = await GetBucket();
        var velocitiesToTest = GetVelocityCandidates(bucket);

        var numberOfOptions = velocitiesToTest
            .Select(v => GetMaxHeightOnHit(bucket, v))
            .Count(h => h.HasValue);

        return numberOfOptions.ToString();
    }

    private static List<Point> GetVelocityCandidates(Rectangle bucket)
    {
        List<Point> candidates = new();

        for (var x = 1; x <= bucket.Right; x++)
        {
            for (var y = bucket.Top; y <= Math.Abs(bucket.Top); y++)
            {
                candidates.Add(new Point(x, y));
            }
        }

        return candidates;
    }

    private static int? GetMaxHeightOnHit(Rectangle bucket, Point velocity)
    {
        int maxHeight = 0;
        var currentPoint = new Point(0, 0);

        while (currentPoint.X < bucket.Right && currentPoint.Y > bucket.Top)
        {
            currentPoint = new Point(currentPoint.X + velocity.X, currentPoint.Y + velocity.Y);
            maxHeight = Math.Max(maxHeight, currentPoint.Y);
            if (bucket.Contains(currentPoint))
            {
                return maxHeight;
            }

            velocity = new Point(Math.Max(0, velocity.X - 1), velocity.Y - 1);
        }

        return null;
    }

    private async Task<Rectangle> GetBucket()
    {
        var content = await File.ReadAllTextAsync(FilePath);
        var match = _bucketRegex.Match(content);
        var fromX = int.Parse(match.Groups[1].Value);
        var toX = int.Parse(match.Groups[2].Value);
        var fromY = int.Parse(match.Groups[3].Value);
        var toY = int.Parse(match.Groups[4].Value);

        // + 1 because 20..30 means from x = 20 to x = 30 which is 11 and not 10
        // Notice that this rectangles top is at the lower left bottom of the rectangle
        return new Rectangle(fromX, fromY, toX - fromX + 1, toY - fromY + 1);
    }
}
