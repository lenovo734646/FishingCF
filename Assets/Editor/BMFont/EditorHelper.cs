using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class EditorHelper : MonoBehaviour {

	[MenuItem("Assets/BatchCreateArtistFont")]
	static public void BatchCreateArtistFont()
	{
		ArtistFont.BatchCreateArtistFont();
	}

    [MenuItem("Tools/just do it")]
    static public void ChangeFish()
    {
        GameObject[] objs = Selection.gameObjects;

        foreach (var item in objs)
        {
            var idle = item.transform.Find("idle");
            idle.GetComponent<SpriteRenderer>().sprite = idle.GetComponent<AnimPlayer>().Sprites[0];
            var die = item.transform.Find("die");
            die.GetComponent<SpriteRenderer>().sprite = die.GetComponent<UnityEngine.UI.Image>().sprite;
        }
    }
}
