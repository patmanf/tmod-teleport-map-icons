using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace SpawnIconTP;

[PublicAPI]
internal class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    public static Config Instance;

    [DefaultValue(true)]
    [BackgroundColor(161, 219, 255)]
    public bool Enabled;

    [DefaultValue(true)]
    [BackgroundColor(104, 159, 255)]
    public bool ConchEnabled;
    
    [Dropdown]
    [DefaultValue(ItemModes.InventoryOnly)]
    [BackgroundColor(123, 95, 212)]
    public ItemModes ItemMode;
    internal enum ItemModes { InventoryOnly, InventoryOrVoidBag, AnyInventory, DontRequireItems }

    [Header("ItemLists")]

    [BackgroundColor(115, 230, 238)]
    public List<ItemDefinition> MirrorItems = [];

    [BackgroundColor(211, 168, 127)]
    public List<ItemDefinition> MagicConchItems = [];

    [BackgroundColor(157, 70, 70)]
    public List<ItemDefinition> DemonConchItems = [];

    [BackgroundColor(90, 146, 90)]
    public List<ItemDefinition> ShellphoneItems = [];

    public Config()
    {
        MirrorItems.Add(new ItemDefinition(ItemID.MagicMirror));
        MirrorItems.Add(new ItemDefinition(ItemID.IceMirror));
        MirrorItems.Add(new ItemDefinition(ItemID.CellPhone));
        
        MagicConchItems.Add(new ItemDefinition(ItemID.MagicConch));
        
        DemonConchItems.Add(new ItemDefinition(ItemID.DemonConch));
        
        ShellphoneItems.Add(new ItemDefinition(ItemID.Shellphone));
        ShellphoneItems.Add(new ItemDefinition(ItemID.ShellphoneSpawn));
        ShellphoneItems.Add(new ItemDefinition(ItemID.ShellphoneOcean));
        ShellphoneItems.Add(new ItemDefinition(ItemID.ShellphoneHell));
    }
}