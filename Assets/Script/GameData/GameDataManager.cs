using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WakHead
{
    public class GameDataManager
    {
        public ActorGameDataArray ActorGameData { get; set; }

        private Object[] _loadedSkillIcon;
        private Object[] _loadedProfilelIcon;
        private Object[] _loadedBodyImage;

        public void Load()
        {
            GameDataLoad();
            SkillIconLoad();
            ProfileIconLoad();
            BodyImageLoad();
        }

        public void GameDataLoad()
        {
            string assetBundlePath;

#if UNITY_EDITOR
            assetBundlePath = "Assets/StreamingAssets/gamedata";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/gamedata";
#endif
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
            AssetBundle bundle = request.assetBundle;

            TextAsset data = bundle.LoadAsset("ActorData") as TextAsset;

            ActorGameData = JsonUtility.FromJson<ActorGameDataArray>(data.ToString());
        }

        public void SkillIconLoad()
        {
            string assetBundlePath;

#if UNITY_EDITOR
            assetBundlePath = "Assets/StreamingAssets/skillicon";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/skillicon";
#endif
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
            AssetBundle bundle = request.assetBundle;

            _loadedSkillIcon = bundle.LoadAllAssets();
        }

        public void ProfileIconLoad()
        {
            string assetBundlePath;

#if UNITY_EDITOR
            assetBundlePath = "Assets/StreamingAssets/profileicon";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/profileicon";
#endif
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
            AssetBundle bundle = request.assetBundle;

            _loadedProfilelIcon = bundle.LoadAllAssets();
        }
        
        public void BodyImageLoad()
        {
            string assetBundlePath;

#if UNITY_EDITOR
            assetBundlePath = "Assets/StreamingAssets/bodyimage";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/bodyimage";
#endif
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
            AssetBundle bundle = request.assetBundle;

            _loadedBodyImage = bundle.LoadAllAssets();
        }

        public Sprite FindSkillIcon(string name)
        {
            foreach (var skillIcon in _loadedSkillIcon)
            {
                if (skillIcon.name == name)
                {
                    var skillIconTex = skillIcon as Texture2D;

                    Sprite skillIconSprite = Sprite.Create(skillIconTex,
                        new Rect(0.0f, 0.0f, skillIconTex.width, skillIconTex.height), new Vector2(0.5f, 0.5f), 100.0f);

                    return skillIconSprite as Sprite;
                }
            }

            return null;
        }

        public Sprite FindProfileIcon(string name)
        {
            foreach (var profileIcon in _loadedProfilelIcon)
            {
                if (profileIcon.name == name)
                {
                    var profileIconTex = profileIcon as Texture2D;

                    Sprite profileIconSprite = Sprite.Create(profileIconTex,
                        new Rect(0.0f, 0.0f, profileIconTex.width, profileIconTex.height), new Vector2(0.5f, 0.5f),
                        100.0f);

                    return profileIconSprite as Sprite;
                }
            }

            return null;
        }
        
        public Sprite FindBodyImage(string name)
        {
            foreach (var bodyImage in _loadedBodyImage)
            {
                if (bodyImage.name == name)
                {
                    var bodyImageTex = bodyImage as Texture2D;

                    Sprite bodyImageSprite = Sprite.Create(bodyImageTex,
                        new Rect(0.0f, 0.0f, bodyImageTex.width, bodyImageTex.height), new Vector2(0.5f, 0.5f),
                        100.0f);

                    return bodyImageSprite as Sprite;
                }
            }

            return null;
        }
    }
}