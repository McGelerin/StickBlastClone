using System.Collections.Generic;
using Runtime.PlaceHolderObject;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Runtime.Data.Persistent.LevelDataSO
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Level Data", order = 0)]
    public class LevelSo : SerializedScriptableObject
    {
        [Header("Level Data")]
        public int RequirementScore;
        public Color32 LevelColor;

        [Header("PlaceholderType")] 
        public List<PlaceHolderGridObjectType> PlaceHolderTypes = new List<PlaceHolderGridObjectType>();
        
        [Header("Grid Data")]
        
        [TableMatrix(HorizontalTitle = "Custom Cell Drawing", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 24)]
        public bool[,] CustomCellDrawing = new bool[5,5];
#if UNITY_EDITOR
        private static bool DrawColoredEnumElement(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
    
            return value;
        }
#endif
    }
}