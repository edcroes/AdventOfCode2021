using System.Text.RegularExpressions;

namespace AoC2021.Common;

public static class MatchExtensions
{
    public static int GetInt(this Match match, string groupName) =>
        int.Parse(match.Groups[groupName].Value);
}