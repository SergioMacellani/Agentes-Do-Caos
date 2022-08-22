using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectStyle : MonoBehaviour
{
    private List<List<PresetsScript>> styleFiles = new List<List<PresetsScript>>(7);
    private List<PresetsScript> st;
    
    public GameObject StylePrefab;
    public List<Vector2> StyleImage;
    public List<Transform> StyleContainer;
    public CreatePlayerOrder playerOrder;
    void Start()
    {
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Face").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Mouth").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Hair").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Eye").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Eyebrow").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Ears").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Clothes").ToList());
        styleFiles.Add(st = Resources.LoadAll<PresetsScript>("PresetsIcon/Add").ToList());

        for (int f = 0; f < 7; f++)
        {
            StartCoroutine(CreateContainers(styleFiles[f], f));
        }
    }

    private IEnumerator CreateContainers(List<PresetsScript> styles, int i)
    {
        foreach (PresetsScript preset in styles)
        {
            GameObject styleContainer = (GameObject)Instantiate(StylePrefab, StyleContainer[i]);
            PresetsScript style = styleContainer.transform.GetChild(0).GetComponent<PresetsScript>();
            style.playerOrder = playerOrder;
            style.SpriteOrder = preset.SpriteOrder;
            style.StyleType = preset.StyleType;
            if (preset.HaveIcon)
            {
                style.HaveIcon = true;
                style.SpriteIcon = preset.SpriteIcon;
            }
            style.transform.localScale = Vector3.one*StyleImage[i].x;
            style.transform.position += Vector3.up*StyleImage[i].y;
        }
        yield break;
    }
}
