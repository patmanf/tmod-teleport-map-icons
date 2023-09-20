using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace SpawnIconTP;

internal class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public static Config Instance;

    [DefaultValue(true)]
    [BackgroundColor(161, 219, 255)]
    public bool Enabled;

    [DefaultValue(true)]
    public bool RequireMirror;

    public List<ItemDefinition> MirrorItems;

    [DefaultValue(true)]
    [BackgroundColor(123, 95, 212)]
    public bool AllowVoidBag;

    public Config()
    {
        MirrorItems = new()
        {
            new ItemDefinition(ItemID.MagicMirror),
            new ItemDefinition(ItemID.IceMirror),
            new ItemDefinition(ItemID.CellPhone),
            new ItemDefinition(ItemID.Shellphone),
            new ItemDefinition(ItemID.ShellphoneSpawn),
            new ItemDefinition(ItemID.ShellphoneOcean),
            new ItemDefinition(ItemID.ShellphoneHell),
        };
    }
}
