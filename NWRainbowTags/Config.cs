using System.Collections.Generic;
using System.ComponentModel;

namespace NWRainbowTags;

public class Config
{
    public bool GroupSpecificSequences { get; set; } = false;

    [Description("Tags Configuration")]
    public float ColorInterval { get; set; } = 0.5f;
    public List<string> RanksWithRTags { get; set; } =
    [
        "owner",
        "moderator",
        "admin"
    ];
    public string[] Sequences { get; set; } =
    [
        "red",
        "orange",
        "yellow",
        "green",
        "blue_green",
        "magenta",
        "silver",
        "crimson"
    ];

    public Dictionary<string, List<string>> GroupSequences { get; set; } = new()
    {
        { "owner", ["red", "orange", "yellow", "green", "blue_green", "magenta", "silver", "crimson"] },
        { "moderator", ["red", "orange", "yellow", "green", "blue_green", "magenta", "silver", "crimson"] },
        { "admin", ["red", "orange", "yellow", "green", "blue_green", "magenta", "silver", "crimson"] }
    };
}