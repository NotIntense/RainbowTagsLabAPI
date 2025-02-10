using System;
using CommandSystem;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using NWRainbowTags;
using RemoteAdmin;

namespace RainbowTags.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ToggleRTag : ICommand
{
    public string Command => "togglerainbowtag";
    public string[] Aliases => ["trt"];
    public string Description => "Toggles your rainbow tag on or off";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is PlayerCommandSender playerCommandSender)
        {
            Player player = Player.Get(playerCommandSender);
            
            if (player == null)
            {
                response = "You must be in-game to use this command!";
                return false;
            }
            
            if (!Main.Instance.Config!.RanksWithRTags.Contains(player.GroupName))
            {
                response = "You must be a member of a rank with a rainbow tag to use this command!";
                return false;
            }

            if (player.GameObject.TryGetComponent(out TagController rainbowTag))
            {
                if (rainbowTag.enabled)
                {
                    rainbowTag.enabled = false;
                    
                    if (player.UserGroup != null) 
                        player.GroupColor = player.UserGroup.BadgeColor;
                    
                    Main.PlayersWithoutRTags.Add(player);
                    response = "Your rainbow tag has been disabled!";
                }
                else
                {
                    rainbowTag.enabled = true;
                    Main.PlayersWithoutRTags.Remove(player);
                    response = "Your rainbow tag has been enabled!";
                }

                return true;
            }

            Main.PlayersWithoutRTags.Remove(player);
                
            if (player.UserGroup != null)
                Main.Instance.OnChangingGroup(new PlayerGroupChangedEventArgs(player.ReferenceHub, player.UserGroup));
                
            response = "Your rainbow tag has been enabled!";
            return true;
        }

        response = "You must be in-game to use this command!";
        return false;
    }
}
