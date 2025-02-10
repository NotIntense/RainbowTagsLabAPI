using System.Collections.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using RainbowTags;
using Version = System.Version;
using static NWRainbowTags.Extensions;

namespace NWRainbowTags
{
    public class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }
        
        private bool _invalidConfig;
        
        public override string Name => "NWRainbowTags";
        public override string Description => "A simple plugin to give players a rainbow tag in-game.";
        public override string Author => "NotIntense";
        public override Version Version => new(1, 0, 0);
        public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
        public static List<Player> PlayersWithoutRTags { get; } = [];

        public override void LoadConfigs()
        {
            base.LoadConfigs();
            _invalidConfig = !this.TryLoadConfig("config.yml", out Config config);
            Config = config ?? new Config();
        }

        public override void Enable()
        {
            Instance = this;
            
            PlayerEvents.GroupChanged += OnChangingGroup;
            
            if (!_invalidConfig) return;
            Logger.Error("NWRainbowTags config is invalid! Please check your config file. Using default config.");
            Config = new Config();
        }

        public override void Disable()
        {
            PlayerEvents.GroupChanged -= OnChangingGroup;
            
            Instance = null;
            _invalidConfig = false;
        }
        
        public void OnChangingGroup(PlayerGroupChangedEventArgs ev)
        {
            if (PlayersWithoutRTags.Contains(ev.Player)) 
                return;
            
            string[] colors;
            
            if (Config!.GroupSpecificSequences)
                TryGetCustomColors(GetGroupKey(ev.Group), out colors);
            else
                TryGetColors(GetGroupKey(ev.Group), out colors);

            if (colors == null)
            {
                //Something went wrong
                Logger.Error($"Failed to assign RainbowTag to {ev.Player}!\n NewGroup Null? : {false}\n Could get colors? : {(Config.GroupSpecificSequences ? TryGetCustomColors(GetGroupKey(ev.Group), out _) : TryGetColors(GetGroupKey(ev.Group), out _))}");
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<TagController>());
                return;
            }

            Logger.Debug("RainbowTags: Added to " + ev.Player.Nickname);

            if (ev.Player.GameObject.TryGetComponent(out TagController controller))
                UnityEngine.Object.Destroy(controller);

            var rtController = ev.Player.GameObject.AddComponent<TagController>();
            rtController.Colors = colors;
            rtController.interval = Config.ColorInterval;
        }

    }
}