using UnityEngine;
using System.Collections;
using UnityEditor;

public class ArtistFont : MonoBehaviour
{	public static void BatchCreateArtistFont()
	{
		string dirName = "";
		string fntname = EditorUtils.SelectObjectPathInfo(ref dirName).Split('.')[0];
		Debug.Log(fntname);
		Debug.Log(dirName);

		string fntFileName = dirName + fntname + ".fnt";
		
		Font CustomFont = new Font();
		{
			AssetDatabase.CreateAsset(CustomFont, dirName + fntname + ".fontsettings");
			AssetDatabase.SaveAssets();
		}

		TextAsset BMFontText = null;
		{
			BMFontText = AssetDatabase.LoadAssetAtPath(fntFileName, typeof(TextAsset)) as TextAsset;
		}

		BMFont mbFont = new BMFont();
		BMFontReader.Load(mbFont, BMFontText.name, BMFontText.bytes);  // 借用NGUI封装的读取类
		CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphs.Count];
		for (int i = 0; i < mbFont.glyphs.Count; i++)
		{
			BMGlyph bmInfo = mbFont.glyphs[i];
			CharacterInfo info = new CharacterInfo();
            float uvMinX = bmInfo.x * 1.0f / mbFont.texWidth;
            float uvMaxX = (bmInfo.x + bmInfo.width) * 1.0f / mbFont.texWidth;
            float uvMaxY = 1 - bmInfo.y * 1.0f / mbFont.texHeight;
            float uvMinY = (mbFont.texHeight - bmInfo.height - bmInfo.y) * 1.0f / mbFont.texHeight;
            info.index = bmInfo.index;
            info.uvBottomLeft = new Vector2(uvMinX, uvMinY);
            info.uvBottomRight = new Vector2(uvMaxX, uvMinY);
            info.uvTopLeft = new Vector2(uvMinX, uvMaxY);
            info.uvTopRight = new Vector2(uvMaxX, uvMaxY);
            info.minX = bmInfo.offsetX;
            info.minY = bmInfo.offsetY;
            info.glyphWidth = bmInfo.width;
            info.glyphHeight = bmInfo.height;
            info.advance = bmInfo.advance;

            //float uvx = bmInfo.x * 1.0f / mbFont.texWidth;
            //float uvy = 1 - (bmInfo.y * 1.0f / mbFont.texHeight);
            //float uvw = bmInfo.width * 1.0f / mbFont.texWidth;
            //float uvh = -1.0f * bmInfo.height / mbFont.texHeight;
            //info.index = bmInfo.index;
            //info.uvBottomLeft = new Vector2(uvx, uvy);
            //info.uvBottomRight = new Vector2(uvx + uvw, uvy);
            //info.uvTopLeft = new Vector2(uvx, uvy + uvh);
            //info.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);
            //info.minX = bmInfo.offsetX;
            ////info.minY = bmInfo.offsetY + bmInfo.height / 2;
            //info.minY = bmInfo.offsetY + bmInfo.height;
            //info.glyphWidth = bmInfo.width;
            //info.glyphHeight = -bmInfo.height;
            //info.advance = bmInfo.advance;

            //info.index = bmInfo.index;
            //info.uv.x = (float)bmInfo.x / (float)mbFont.texWidth;
            //info.uv.y = 1 - (float)bmInfo.y / (float)mbFont.texHeight;
            //info.uv.width = (float)bmInfo.width / (float)mbFont.texWidth;
            //info.uv.height = -1f * (float)bmInfo.height / (float)mbFont.texHeight;
            //info.vert.x = (float)bmInfo.offsetX;
            //info.vert.y = (float)bmInfo.offsetY;
            //info.vert.width = (float)bmInfo.width;
            //info.vert.height = (float)bmInfo.height;
            //info.width = (float)bmInfo.advance;
            characterInfo[i] = info;
		}
		CustomFont.characterInfo = characterInfo;


		string textureFilename = dirName + mbFont.spriteName + ".png";
		Material mat = null;
		{
			Shader shader = Shader.Find("Transparent/Diffuse");
			mat = new Material(shader);
			Texture tex = AssetDatabase.LoadAssetAtPath(textureFilename, typeof(Texture)) as Texture;
			mat.SetTexture("_MainTex", tex);
			AssetDatabase.CreateAsset(mat, dirName + fntname + ".mat");
			AssetDatabase.SaveAssets();
		}
		CustomFont.material = mat;
	}
}
