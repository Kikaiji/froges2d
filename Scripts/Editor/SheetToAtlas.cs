#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;

[Tool]
public partial class SheetToAtlas : EditorScript
{
	// Called when the script is executed (using File -> Run in Script Editor).
	private CompressedTexture2D spritesheet;
	private string dir;

	public override void _Run()
	{
		dir  = "res://Art/Sprites/AtlasSprites/";
		spritesheet = (CompressedTexture2D) GD.Load("res://Art/Sprites/spritesheetx64.png");
		List<AtlasTexture> textures = new List<AtlasTexture>();
		
		GD.Print(spritesheet.GetWidth());
		for (int x = 0; x < spritesheet.GetWidth() / 32; x++)
		{
			for (int y = 0; y < spritesheet.GetHeight() / 32; y++)
			{
				GD.Print(x + ", " + y);
				var tex = new AtlasTexture();
				tex.Atlas = spritesheet;
				tex.Region = new Rect2(x * 32, y * 32, 32, 32);
				tex.ResourceName = ("textures" + x + y);
				if(CheckTexture(tex)) textures.Add(tex);
			}
		}
		
		//var err = ResourceSaver.Save(textures[0], "res://Art/Sprites/AtlasSprites/");
		foreach (var tex in textures)
		{
			var err = ResourceSaver.Save(tex, dir + tex.ResourceName + ".tres");
			GD.Print(err);
		}
	}

	private bool CheckTexture(Texture2D tex)
	{
		var img = tex.GetImage();
		for (int x = 0; x < tex.GetWidth(); x++)
		{
			for (int y = 0; y < tex.GetHeight(); y++)
			{
				if (img.GetPixel(x, y).A != 0) return true;
			}
		}
		return false;
	}
}

#endif
