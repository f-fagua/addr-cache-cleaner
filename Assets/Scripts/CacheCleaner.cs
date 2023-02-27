using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class CacheCleaner : MonoBehaviour
{
    [SerializeField]
    private bool m_CleanOldCachedBundles;
    
    private void Start()
    {
        Addressables.InitializeAsync().Completed += handle =>
        {
            if(m_CleanOldCachedBundles)
                CleanCache();
        };
    }

    private void CleanCache()
    {
      var usedBundles = GetCurrentlyUsedBundles();
      var cachedBundles = GetCachedBundles();
    
       foreach (var cachedBundle in cachedBundles)
      {
          if (!usedBundles.Contains(cachedBundle))
          {
              Debug.Log($"{cachedBundle} no longer in use");
              
              var cachedVersions = new List<Hash128>();
              Caching.GetCachedVersions(cachedBundle, cachedVersions);
              
              if (cachedVersions.Count > 0)
                  Caching.ClearAllCachedVersions(cachedBundle);
          }
      }
    }
    
    private List<string> GetCurrentlyUsedBundles()
    {
        List<string> usedBundles = new List<string>();
        foreach (var catalog in Addressables.ResourceLocators) //Iterate over each catalog
        {
            foreach (var key in catalog.Keys) //Iterate over each catalog location
            {
                if (key.ToString().Contains(".bundle"))
                {
                    catalog.Locate(key, typeof(object), out var locations);
                    foreach (var location in locations)
                    {
                        var bundleOptions = (AssetBundleRequestOptions) location.Data;
                        usedBundles.Add(bundleOptions.BundleName);
                    }
                }
            }   
        }
        return usedBundles;
    }
    
    private List<string> GetCachedBundles()
    {
        var cachedBundles = new List<string>();
        foreach (var directory in Directory.GetDirectories(Caching.currentCacheForWriting.path))
        {
            var lastPathIndex = directory.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            cachedBundles.Add(directory.Substring(lastPathIndex));
        }
    
        return cachedBundles;
    }
}
