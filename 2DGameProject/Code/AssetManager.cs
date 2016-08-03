using System.Collections.Generic;
using SFML.Graphics;

public class AssetManager
{
    //------------------------------------------------//
    //--------------------TEXTURES--------------------//
    //------------------------------------------------//
    static Dictionary<TextureName, Texture> textures = new Dictionary<TextureName, Texture>();

    public static Texture GetTexture(TextureName textureName)
    {
        if (textures.Count == 0)
        {
            LoadTextures(); 
        }
        return textures[textureName];
    }

    static void LoadTextures()
    {
        textures.Add(TextureName.Player, new Texture("Textures/Player.png"));

        textures.Add(TextureName.WhitePixel, new Texture("Textures/pixel.png"));
        textures.Add(TextureName.MainMenuBackground, new Texture("Textures/MainMenu_Background.jpg"));

    }

    public enum TextureName
    {
        Player,

        WhitePixel,
        MainMenuBackground,
    }

    //------------------------------------------------//
    //---------------------FONTS----------------------//
    //------------------------------------------------//
    static Dictionary<FontName, Font> fonts = new Dictionary<FontName, Font>();

    public static Font GetFont(FontName fontName)
    {
        if (fonts.Count == 0)
        {
            LoadFonts();
        }
        return fonts[fontName];
    }

    static void LoadFonts()
    {
        fonts.Add(FontName.Calibri, new Font("Fonts/calibri.ttf"));

    }

    public enum FontName
    {
        Calibri,
    }
}
