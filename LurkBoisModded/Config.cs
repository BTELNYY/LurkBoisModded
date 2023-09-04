using Interactables.Interobjects.DoorUtils;
using LurkBoisModded.Base;
using LurkBoisModded.EventHandlers.Scp914;
using LurkBoisModded.Managers;
using PlayerRoles;
using Scp914;
using System.Collections.Generic;
using CustomPlayerEffects;
using System.ComponentModel;

namespace LurkBoisModded
{
    public class Config
    {
        [Description("Should the plugin be enabled")]
        public bool PluginEnabled { get; set; } = true;

        [Description("Should the height of players be randomized when they spawn?")]
        public bool RandomizeHumanHeight { get; set; } = true;

        [Description("Height Max/Min, inclusive. Also its a scale multiplier, so setting this to 2 will double the height")]
        public float MaxHeight { get; set; } = 1.1f;
        public float MinHeight { get; set; } = 0.95f;

        [Description("SCP 330 (Candy) Config")]
        public Scp330Config Scp330Config { get; set; } = new Scp330Config();

        [Description("SCP 914 Config")]
        public Scp914Config Scp914Config { get; set; } = new Scp914Config();

        [Description("SCP 106 Pocket Dimension Config")]
        public Scp106PocketDimensionConfig Scp106PdConfig { get; set; } = new Scp106PocketDimensionConfig();

        [Description("SCP 079 Configuration")]
        public Scp079 Scp079Config { get; set; } = new Scp079();

        [Description("Proximity chat config")]
        public ProximityChatConfig ProximityChatConfig { get; set; } = new ProximityChatConfig();

        [Description("Controls Facility Options")]
        public FacilityConfig FacilityConfig { get; set; } = new FacilityConfig();

        [Description("SCP 939s Config")]
        public Scp939Config Scp939Config { get; set; } = new Scp939Config();

        [Description("SCP 049 Config")]
        public Scp049Config Scp049Config { get; set; } = new Scp049Config();

        [Description("Controls the OnFire effect")]
        public FireConfig FireConfig { get; set; } = new FireConfig();

        [Description("Controls Abilities")]
        public AbilityConfig AbilityConfig { get; set; } = new AbilityConfig();

         [Description("Death message for people who use .suicide")]
        public string SuicideDeathReason { get; set; } = "Suicide by gunshot to the head";

        [Description("Chance that a coin will explode when you flip it")]
        public float CoinExplodeChance { get; set; } = 0.01f;

        [Description("List of items with weight for General random loot")]
        public List<ItemWeightDefinition> ItemQualityConfig { get; set; } = new List<ItemWeightDefinition>()
        {
            new ItemWeightDefinition(ItemType.None, 0.1f),
            new ItemWeightDefinition(ItemType.Coin, 0.2f),
            new ItemWeightDefinition(ItemType.Flashlight, 0.35f),
            new ItemWeightDefinition(ItemType.KeycardJanitor, 0.4f),
            new ItemWeightDefinition(ItemType.Radio, 0.4f),
            new ItemWeightDefinition(ItemType.KeycardScientist, 0.5f),
            new ItemWeightDefinition(ItemType.GunCOM15, 0.6f),
            new ItemWeightDefinition(ItemType.KeycardResearchCoordinator, 0.65f),
            new ItemWeightDefinition(ItemType.KeycardZoneManager, 0.6f),
            new ItemWeightDefinition(ItemType.KeycardContainmentEngineer, 0.75f),
            new ItemWeightDefinition(ItemType.KeycardGuard, 0.8f),
            new ItemWeightDefinition(ItemType.ArmorLight, 0.85f),
            new ItemWeightDefinition(ItemType.GrenadeFlash, 1f),
            new ItemWeightDefinition(ItemType.GunCOM18, 1f),
            new ItemWeightDefinition(ItemType.GunFSP9, 1.1f),
            new ItemWeightDefinition(ItemType.Painkillers, 2f),
            new ItemWeightDefinition(ItemType.KeycardMTFPrivate, 2.1f),
            new ItemWeightDefinition(ItemType.Medkit, 2.1f),
            new ItemWeightDefinition(ItemType.KeycardMTFOperative, 2.5f),
            new ItemWeightDefinition(ItemType.GunCrossvec, 3f),
            new ItemWeightDefinition(ItemType.GunE11SR, 4f),
        };
    }

    public class Scp330Config
    {
        [Description("Should SCP 330 give pink candy?")]
        public bool EnablePinkCandy { get; set; } = true;

        [Description("Chance of a player getting pink candy from SCP 330?")]
        public float PinkCandyChance { get; set; } = 0.2f;
    }

    public class Scp914Config
    {
        [Description("Controls what can happen to a player based on the knob setting. You can control the odds of nothing happening by just adding the Nothing flag to a list.")]
        public Dictionary<Scp914KnobSetting, List<Scp914SettingEventHandler.Scp914Event>> Scp914Settings { get; set; } = new Dictionary<Scp914KnobSetting, List<Scp914SettingEventHandler.Scp914Event>>()
        {
            [Scp914KnobSetting.Rough] = new List<Scp914SettingEventHandler.Scp914Event>()
            {
                Scp914SettingEventHandler.Scp914Event.Death,
                Scp914SettingEventHandler.Scp914Event.Nothing,
            },
            [Scp914KnobSetting.Coarse] = new List<Scp914SettingEventHandler.Scp914Event>()
            {
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Zombie,
                Scp914SettingEventHandler.Scp914Event.Scp914DamageOverTimeEffect,
            },
            [Scp914KnobSetting.OneToOne] = new List<Scp914SettingEventHandler.Scp914Event>()
            {
                Scp914SettingEventHandler.Scp914Event.Nothing
            },
            [Scp914KnobSetting.Fine] = new List<Scp914SettingEventHandler.Scp914Event>()
            {
                Scp914SettingEventHandler.Scp914Event.Nothing
            },
            [Scp914KnobSetting.VeryFine] = new List<Scp914SettingEventHandler.Scp914Event>()
            {
                Scp914SettingEventHandler.Scp914Event.BlueAsh,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
                Scp914SettingEventHandler.Scp914Event.Nothing,
            },
        };

        [Description("Death message for SCP 914")]
        public string Scp914DeathReason { get; set; } = "Body is horribly disfigured and destroyed, signs of machining, crushing, laser cutting and blunt force as visible";

        [Description("How long should the cardiac arrest last from SCP 914?")]
        public float Scp914DamageOverTimeDuration { get; set; } = 19;

        [Description("How much damage per second should the effect do?")]
        public byte Scp914DamageOverTimeIntensity { get; set; } = 5;

        [Description("Damage over time death reason")]
        public string Scp914DamageOverTimeDeathReason { get; set; } = "Body shows signs of damage identical to recorded damage of SCP 914 test subjects";

        [Description("What should the death message be for victims of the blue ash event?")]
        public string Scp914BlueAshDeathReason { get; set; } = "Turned into a pile of blue ash";

        [Description("Reminder for blue ash death")]
        public string Scp914BlueAshReminder { get; set; } = "Reminder! You will die in {time} seconds!";

        [Description("How fast should the victim go?")]
        public byte Scp914BlueAshIntensity { get; set; } = 255;

        [Description("How long before the victim dies?")]
        public float Scp914BlueAshDuration { get; set; } = 120f;
    }

    public class Scp106PocketDimensionConfig
    {
        [Description("Drop bodies from pocket dimension?")]
        public bool Scp106DropBodiesFromPd { get; set; } = true;

        [Description("Randomized values for how long bodies should stay in PD before being dropped into the map.")]
        public float Scp106PdDropDelayMin { get; set; } = 0.5f;
        public float Scp106PdDropDelayMax { get; set; } = 1f;
    }

    public class Scp079
    {
        [Description("Enable SCP 079 Gas?")]
        public bool GasEnabled { get; set; } = true;

        [Description("Duration of the effect")]
        public float GasDuration { get; set; } = 20f;

        [Description("Cooldown of the command. Note that this is added to the duration.")]
        public float GasCooldown { get; set; } = 40f;

        [Description("Intensity")]
        public byte GasIntensity { get; set; } = 1;

        [Description("What tier should SCP 079 unlock the command?")]
        public int GasUnlockAt { get; set; } = 4;

        [Description("Cost of the ability in AP")]
        public float GasCost { get; set; } = 100f;

        [Description("Suffocation death message")]
        public string SuffocationDeathMessage { get; set; } = "Blue lips and pale skin puts the cause as suffocation";
    }

    public class Scp939Config
    {
        public float Duration { get; set; } = 10f;
        public byte Intensity { get; set; } = 1;
        public bool Stacks { get; set; } = true;
        public bool ModifyHeight { get; set; } = true;
        public float MinHeight { get; set; } = 0.9f;
        public float MaxHeight { get; set; } = 1.1f;
    }

    public class FacilityConfig
    {
        [Description("SCP 939 Cryo Door Config")]
        public bool SpawnScp939Door { get; set; } = true;

        [Description("Door Keycard permissions")]
        public KeycardPermissions[] Scp939DoorKeycardRequirements { get; set; } = new KeycardPermissions[]
        {
            KeycardPermissions.ContainmentLevelThree,
        };

        [Description("Should the door be open at the beginning of a round? (Note, setting to false will trap SCP 939 inside. (If there is any)")]
        public bool Scp939DoorDefaultOpen { get; set; } = true;

        [Description("Should the plugin modify GR 18 Inner?")]
        public bool ModifyGr18 { get; set; } = true;

        [Description("GR 18 Inner Keycard permissions. (Glass room)")]
        public KeycardPermissions[] Gr18KeycardPermissions { get; set; } = new KeycardPermissions[]
        {
            KeycardPermissions.ContainmentLevelTwo
        };
    }

    public class ProximityChatConfig
    {
        [Description("Enable Proximity chat?")]
        public bool EnableCustomChat { get; set; } = true;

        [Description("List of Roles who can use Proximity chat")]
        public List<RoleTypeId> AllowedRoles { get; set; } = new List<RoleTypeId>()
        {
            RoleTypeId.Scp939,
            RoleTypeId.Scp049,
        };

        [Description("Distance where proximity can be heard")]
        public float ProximityChatDistance { get; set; } = 10f;

        public string ProximityChatEnabled { get; set; } = "Proximity chat Enabled!";

        public string ProximityChatDisabled { get; set; } = "Proximity chat Disabled!";

        public string ProximityChatCanBeUsed { get; set; } = "Your class can use proximity chat! Press your noclip key to activate it! (left alt by default)";
    }

    public class Scp049Config
    {
        public float SprintSpeed { get; set; } = 5.7f;
    }

    public class Scp096Config
    {
        public float HumanTargetDamage { get; set; } = 70f;
        public float HumanNonTargetDamage { get; set; } = 45f;
        public float WindowDamage { get; set; } = 750f;
        public float DoorDamage { get; set; } = 750f;
    }

    public class FireConfig
    {
        public float Damage { get; set; } = 5f;

        public float[] FireColor { get; set; } = new float[3] { 215f, 53f, 2f };

        public float ColorIntensity { get; set; } = 0.25f;

        public float ColorRange { get; set; } = 1.25f;

        public string FireTip { get; set; } = "You are on fire!";

        public string DeathReason { get; set; } = "Severe burning and charring suggests death as fire";
    }

    public class AbilityConfig
    {
        public string CooldownMessage { get; set; } = "Ability is on cooldown for another {time} seconds!";

        public RemoteExplosiveAbilityConfig RemoteExplosiveAbilityConfig { get; set; } = new RemoteExplosiveAbilityConfig();

        public InspireAbilityConfig InspireAbilityConfig { get; set; } = new InspireAbilityConfig();

        public WarCryAbilityConfig WarCryAbilityConfig { get; set; } = new WarCryAbilityConfig();
    }

    public class RemoteExplosiveAbilityConfig
    {
        public bool IsSingleUse { get; set; } = true;

        public int MaxDetonations { get; set; } = int.MaxValue;

        public byte BatteryPercentPenaltyPerUse { get; set; } = 100;
    }

    public class InspireAbilityConfig
    {
        public string NoTargetsMessage { get; set; } = "Nobody to inspire.";

        public string Inspired { get; set; } = "You were inspired by {playername}";

        public string YouInspired { get; set; } = "You inspired {count} person(s)!";

        public float Cooldown { get; set; } = 60f;

        public float Range { get; set; } = 15f;

        public float AhpGranted { get; set; } = 25f;
    }

    public class WarCryAbilityConfig
    {
        public string NoTargetsMessage { get; set; } = "Nobody to hear your war cry.";

        public string WarCryHeard { get; set; } = "{count} person(s) heard your war cry!";

        public string WarCryEffectYou { get; set; } = "You heard the war cry of {playername}!";

        public float Cooldown { get; set; } = 60f;

        public float Range { get; set; } = 15f;

        public List<EffectDefinition> Effects { get; set; } = new List<EffectDefinition>()
        {
            new EffectDefinition(){Name = nameof(DamageReduction), Duration = 30f, Intensity = 8}
        };
    }
}