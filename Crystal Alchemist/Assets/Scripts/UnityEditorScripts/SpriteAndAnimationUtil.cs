﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public struct FileList
{
    public GameObject spriteRendererObject;
    public string path;
}

[System.Serializable]
public class AnimationKey
{
    public int startIndex;
    public int endIndex;
    public int rowStartIndex;
    public int rowEndIndex;
    public string animTyp;
    public int intervall;
}

public class SpriteAndAnimationUtil : MonoBehaviour
{
    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private string assetPath = "Assets/Resources/Art/Characters/Player Sprites/";

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private string animationPath = "Resources/Animations/Player Animations/";

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private List<AnimationKey> animationKeys = new List<AnimationKey>();

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private Character character;

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private List<string> rows = new List<string>();

#if UNITY_EDITOR
    [Button]
    public void UpdateSpritesAndAnimations()
    {
        List<PlayerAnimationPart> childObjects = new List<PlayerAnimationPart>();
        CustomUtilities.UnityFunctions.GetChildObjects(this.transform, childObjects); //get all children from GameObject

        List<AnimationClip> clips = getAnimationClips(); //get all AnimationClips from Asset Folder

        foreach (PlayerAnimationPart part in childObjects)
        {
            SliceAndNameSprites(part); //Slice and Name Sprites
        }

        foreach (AnimationClip clip in clips)
        {
            foreach (PlayerAnimationPart childObject in childObjects)
            {
                //set all animation clips foreach object
                SetAnimationClips(clip, childObject);
            }

            Debug.Log("<color=blue>Set Animation of: " + clip.name + " for " + childObjects.Count + " GameObjects.</color>");
        }
    }

    [Button]
    public void temp()
    {
        List<CharacterCreatorPart> childObjects = new List<CharacterCreatorPart>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorPart>(this.transform, childObjects); //get all children from GameObject

        foreach (CharacterCreatorPart part in childObjects)
        {
            part.properties.Add(part.property);
        }
    }

#endif

    private void Awake()
    {
        Destroy(this);
    }

    private string GetGameObjectPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null && transform.parent != this.transform)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

    private void SetAnimationClips(AnimationClip clip, PlayerAnimationPart childObject)
    {
        if (clip != null)
        {
            string path = this.assetPath + "/" + childObject.property.getFullPath();
            GameObject spriteObject = childObject.gameObject;

            List<Object> sprite = AssetDatabase.LoadAllAssetsAtPath(path).ToList();
            List<Sprite> sprites = new List<Sprite>();
                                 
            foreach (Object obj in sprite)
            {
                if (obj is Sprite && obj.name.Contains(clip.name)) sprites.Add((Sprite)obj);
            }

            //sprites = sprites.OrderBy(c => int.Parse(c.name.Split('_').Last())).ToList();
            sprites = sprites.OrderBy(c => c.name).ToList();

            if (childObject.GetComponent<SpriteRenderer>() != null 
                && clip.name.Contains("Idle Down")
                && sprites.Count > 0)
                childObject.GetComponent<SpriteRenderer>().sprite = sprites[0];

            EditorCurveBinding spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.path = GetGameObjectPath(spriteObject.transform);
            spriteBinding.propertyName = "m_Sprite";

            if (sprites.Count > 0)
            {
                int intervall = getAnimationClipIntervall(sprites[0].name);
                int arrayLength = sprites.Count + 1;
                if (intervall <= 0) arrayLength = 1;

                ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[arrayLength];
                float time = 0f;

                try
                {
                    for (int i = 0; i < spriteKeyFrames.Length; i++)
                    {
                        spriteKeyFrames[i] = new ObjectReferenceKeyframe();
                        spriteKeyFrames[i].time = time;

                        if (i == spriteKeyFrames.Length - 1) spriteKeyFrames[i].value = sprites[i - 1];
                        else spriteKeyFrames[i].value = sprites[i];

                        time += (float)(intervall / 60f);
                    }
                }
                catch
                {
                    Debug.Log("<color=red>Error: " + childObject.gameObject.name + " (" + childObject.property.getFullPath() + ")</color>");
                }

                AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyFrames);
                //if(file.setSortOrder) clip.SetCurve("", typeof(SpriteRenderer), "m_SortOrder", AnimationCurve.Linear(0, file.sortOrder, 0, file.sortOrder));

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();                
            }
        }
        else
        {
            Debug.Log("<color=red>Error: Clip is null</color>");
        }
    }

    private List<AnimationClip> getAnimationClips()
    {
        List<string> files = Directory.GetFiles(Application.dataPath + "/Resources/" + this.animationPath, "*.anim").ToList();
        List<AnimationClip> clips = new List<AnimationClip>();

        foreach (string file in files)
        {
            string path = "Assets/" + file.Replace(Application.dataPath, "");
            clips.Add(AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip);
        }

        return clips;
    }

    private void SliceAndNameSprites(PlayerAnimationPart file)
    {
        string assetpath = this.assetPath + "/" + file.property.getFullPath();
        Texture2D myTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetpath);

        File.Delete(Application.dataPath + "/" + this.assetPath.Replace("Assets", "") + "/" + file.property.getFullPath() + ".meta");

        string path = AssetDatabase.GetAssetPath(myTexture);
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
        ti.isReadable = true;

        ti.spritePixelsPerUnit = 16;
        ti.spriteImportMode = SpriteImportMode.Multiple;
        ti.filterMode = FilterMode.Point;
        ti.textureCompression = TextureImporterCompression.Uncompressed;

        List<SpriteMetaData> newData = new List<SpriteMetaData>();

        int SliceWidth = 32;
        int SliceHeight = 48;
        int columnIndex = 0;

        List<string> spriteNames = new List<string>();

        for (int i = 0; i < myTexture.width; i += SliceWidth)
        {
            int rowIndex = 0;

            for (int j = myTexture.height; j > 0; j -= SliceHeight)
            {
                AnimationKey column = getAnimationKey(columnIndex, rowIndex);
                string row = getAnimationRowKey(rowIndex);

                if (column != null)
                {
                    string direction = row;
                    string animationName = column.animTyp;

                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;

                    string spriteName = animationName + " " + direction;
                    if (column.endIndex - column.startIndex > 0) spriteName = animationName + " " + direction + " " + columnIndex;

                    smd.name = spriteName;
                    smd.rect = new Rect(i, j - SliceHeight, SliceWidth, SliceHeight);

                    newData.Add(smd);
                }

                rowIndex++;
            }

            columnIndex++;
        }

        ti.spritesheet = newData.ToArray();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        AssetDatabase.LoadAssetAtPath<Texture2D>(assetpath);

        Debug.Log("<color=blue>Sliced and Named: " + file.property.getFullPath() + " into " + newData.Count + " sprites</color>");
    }

    private AnimationKey getAnimationKey(int columnIndex, int rowIndex)
    {
        foreach(AnimationKey column in this.animationKeys)
        {
            if (columnIndex >= column.startIndex
               && columnIndex <= column.endIndex
               && rowIndex >= column.rowStartIndex
               && rowIndex <= column.rowEndIndex)
            {
                return column;
            }
        }

        return null;
    }

    private string getAnimationRowKey(int rowIndex)
    {
        if (rowIndex < this.rows.Count) return this.rows[rowIndex];
        return null;
    }

    private int getAnimationClipIntervall(string name)
    {
        int result = 10;

        for (int i = this.animationKeys.Count - 1; i >= 0; i--)
        {
            if (name.ToUpper().Contains(this.animationKeys[i].animTyp.ToUpper()))
            {
                result = this.animationKeys[i].intervall;
                break;
            }
        }

        return result;
    }
}
