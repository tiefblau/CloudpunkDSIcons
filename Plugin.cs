using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CloudpunkDSIcons;

public class Plugin : MonoBehaviour
{
    private static bool done = false;

    private static readonly string resourceNamespace = typeof(Plugin).Namespace;

    private static readonly HashSet<string> textureNames = ["controller_layout_Xbox", "itematlas_volaris"];

    private UnityAction<Scene, LoadSceneMode> onSceneLoaded;

    public void OnEnable()
    {
        onSceneLoaded = (UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (done == false && scene.name == "Main")
        {
            ReplaceTextures();
        }
    }

    private static void ReplaceTextures()
    {
        Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();

        for (int i = 0; i < textures.Length; ++i)
        {
            Texture2D texture = textures[i];
            string textureName = texture.name;

            if (textureNames.Remove(textureName) == true)
            {
                byte[] data = GetResourceContent($"{resourceNamespace}.resources.{textureName}_modified.png");

                if (texture.LoadImage(data, true) == true)
                {
                    PluginLoader.Log.LogInfo($"Texture {textureName} replaced.");
                }
                else
                {
                    PluginLoader.Log.LogError("Failed to upload replacement texture into memory!");
                }

                if (textureNames.Count == 0)
                {
                    PluginLoader.Log.LogInfo("Done!");
                    done = true;
                    break;
                }
            }
        }
    }

    private static byte[] GetResourceContent(string resourceName)
    {
        using Stream resourceFilestream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (resourceFilestream == null)
        {
            PluginLoader.Log.LogError("Failed to load replacement texture from resource!");
            return null;
        }

        byte[] data = new byte[resourceFilestream.Length];
        resourceFilestream.Read(data, 0, data.Length);

        return data;
    }
}
