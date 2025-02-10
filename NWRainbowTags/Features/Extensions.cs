using System.Linq;
using LabApi.Features.Console;

namespace NWRainbowTags;

public static class Extensions
{
    private static bool EqualsTo(UserGroup thisGroup, UserGroup otherGroup)
    {
        return thisGroup.BadgeColor == otherGroup.BadgeColor && thisGroup.BadgeText == otherGroup.BadgeText &&
               thisGroup.Permissions == otherGroup.Permissions && thisGroup.Cover == otherGroup.Cover &&
               thisGroup.HiddenByDefault == otherGroup.HiddenByDefault && thisGroup.Shared == otherGroup.Shared &&
               thisGroup.KickPower == otherGroup.KickPower &&
               thisGroup.RequiredKickPower == otherGroup.RequiredKickPower;
    }
    
    public static string GetGroupKey(UserGroup group)
    {
        if (group == null)
            return string.Empty;

        return ServerStatic.PermissionsHandler.Groups.FirstOrDefault(g => EqualsTo(g.Value, group)).Key ?? string.Empty;
    }
    
    public static bool TryGetColors(string rank, out string[] availableColors)
    {
        availableColors = Main.Instance!.Config!.Sequences;
        return !string.IsNullOrEmpty(rank) && Main.Instance.Config.RanksWithRTags.Contains(rank);
    }
    
    public static bool TryGetCustomColors(string rank, out string[] availableColors)
    {
        if (string.IsNullOrEmpty(rank) || !Main.Instance.Config!.GroupSequences.TryGetValue(rank, out var sequence))
        {
            Logger.Error($"A player with the rank '{rank}' does not have a custom group sequence! They will be given the default sequence colors.");
            availableColors = Main.Instance.Config!.Sequences;
            return false;
        }

        availableColors = sequence.ToArray();
        return Main.Instance.Config.RanksWithRTags.Contains(rank);
    }
}