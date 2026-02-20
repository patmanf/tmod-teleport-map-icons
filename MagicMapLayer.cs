using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace SpawnIconTP;

public class MagicMapLayer : ModMapLayer
{
    public override Position GetDefaultPosition() => new Before(IMapLayer.Spawn);

    internal static Icon HomeIcon = new(TextureAssets.SpawnBed, "UI.SpawnBed", ClickHome);
    internal static Icon SpawnIcon = new(TextureAssets.SpawnPoint, "UI.SpawnPoint", ClickSpawn);
    internal static Icon OceanIconL = new("SpawnIconTP/Textures/ocean", "Bestiary_Biomes.Ocean", () => ClickOcean(false));
    internal static Icon OceanIconR = new("SpawnIconTP/Textures/ocean", "Bestiary_Biomes.Ocean", () => ClickOcean(true));
    internal static Icon HellIcon = new("SpawnIconTP/Textures/hell", "Bestiary_Biomes.TheUnderworld", ClickHell);

    public static void SetOceanPos(Vector2 pos, bool right)
    {
        if (right) OceanIconR.SetPosition(pos);
        else OceanIconL.SetPosition(pos);
    }

    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    {
        if (!Config.Instance.Enabled) return;

        bool hasShellphone = HasItems(Config.Instance.ShellphoneItems);

        if (hasShellphone || HasItems(Config.Instance.MirrorItems))
        {
            IMapLayer.Spawn.Hide();

            SpawnIcon.Draw(ref context, ref text, new Vector2(Main.spawnTileX, Main.spawnTileY));
            if (Main.LocalPlayer.SpawnX != -1)
                HomeIcon.Draw(ref context, ref text, new Vector2(Main.LocalPlayer.SpawnX, Main.LocalPlayer.SpawnY));
        }

        if (!Config.Instance.ConchEnabled) return;

        if (hasShellphone || HasItems(Config.Instance.MagicConchItems))
        {
            OceanIconL.Draw(ref context, ref text);
            OceanIconR.Draw(ref context, ref text);
        }

        if (hasShellphone || HasItems(Config.Instance.DemonConchItems))
            HellIcon.Draw(ref context, ref text);
    }

    private static bool HasItems(List<ItemDefinition> list)
    {
        Player player = Main.LocalPlayer;
        return (Config.Instance.ItemMode) switch
        {
            Config.ItemModes.InventoryOnly
                => list.Exists(item => player.HasItem(item.Type)),
            Config.ItemModes.InventoryOrVoidBag
                => list.Exists(item => player.HasItemInInventoryOrOpenVoidBag(item.Type)),
            Config.ItemModes.AnyInventory
                => list.Exists(item => player.HasItemInAnyInventory(item.Type)),
            Config.ItemModes.DontRequireItems => true,
            _ => false
        };
    }

    private static void ClickSpawn()
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
            Main.LocalPlayer.Shellphone_Spawn();
        else
            NetMessage.SendData(MessageID.RequestTeleportationByServer, -1, -1, null, 3);
    }

    private static void ClickHome()
    {
        MagicMapPlayer.Get().TeleportHome();
        if (Main.netMode == NetmodeID.MultiplayerClient)
            MagicMap.SendTeleportedHome();
    }

    private static void ClickOcean(bool right)
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
            MagicMapPlayer.Get().TeleportOcean(right);
        else
            MagicMap.RequestOceanTeleport(right);
    }

    private static void ClickHell()
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
            Main.LocalPlayer.DemonConch();
        else
            NetMessage.SendData(MessageID.RequestTeleportationByServer, -1, -1, null, 2);
    }

    public class Icon(Asset<Texture2D> texture, LocalizedText hoverText, Action clickAction)
    {
        public Icon(string textureName, string hoverKey, Action clickAction)
            : this(ModContent.Request<Texture2D>(textureName), hoverKey, clickAction) { }
        
        public Icon(Asset<Texture2D> texture, string hoverKey, Action clickAction)
            : this(texture, Language.GetText(hoverKey), clickAction) { }

        private static readonly SpriteFrame Frame = new(1, 1, 0, 0);
        private Vector2 _position;

        public void SetPosition(Vector2 position) => _position = position;

        public void Draw(ref MapOverlayDrawContext context, ref string text)
            => Draw(ref context, ref text, _position);

        public void Draw(ref MapOverlayDrawContext context, ref string text, Vector2 position)
        {
            var result = context.Draw(texture.Value, position, Color.White, Frame, 1f, 2f, Alignment.Center);
            if (!result.IsMouseOver) return;

            Main.cancelWormHole = true;
            text = hoverText.Value;

            if (!Main.mouseLeft || !Main.mouseLeftRelease)
                return;

            Main.mouseLeftRelease = false;
            Main.mapFullscreen = false;
            PlayerInput.LockGamepadButtons("MouseLeft");
            SoundEngine.PlaySound(SoundID.MenuClose);
            SoundEngine.PlaySound(SoundID.Item6);

            clickAction();
        }
    }
}
