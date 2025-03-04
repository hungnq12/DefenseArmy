using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PopupHolder : MonoBehaviour
{
    [SerializeField] List<Popup> popupList;

    public Popup GetPopup<T>(out Popup value)
    {
        value = GetFirstElementOfType<T>(popupList);
        return value;
    }

    private Popup GetFirstElementOfType<T>(IEnumerable<Popup> list)
    {
        foreach (var item in list)
        {
            if (item is T)
            {
                return item;
            }
        }

        return null;
    }

#if UNITY_EDITOR
    [SerializeField] Object popupFolder;
    //[Button("Load Popups")]
    private void LoadConfig()
    {
        string[] popups = Directory
            .GetFiles(AssetDatabase.GetAssetPath(popupFolder), "*Popup*.prefab", SearchOption.AllDirectories)
            .ToArray();

        popupList = new List<Popup>();
        for (int i = 0; i < popups.Length; i++)
        {
            var file = UnityEditor.AssetDatabase.LoadAssetAtPath<Popup>(popups[i]);
            if (file != null)
                popupList.Add(file);
            AssetDatabase.SaveAssets();
        }

        EditorUtility.SetDirty(this);
    }
#endif
}