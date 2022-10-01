using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unitility.SelectionHistory;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Unitility.SelectionHistory
{
    public class SelectionHistoryWindow : EditorWindow, IHasCustomMenu
    {
        private static class Styles
        {
            public static GUIStyle LineStyle = (GUIStyle) "TV Line";
            public static GUIStyle LineBoldStyle = (GUIStyle) "TV LineBold";
            public static GUIStyle SelectionStyle = (GUIStyle) "TV Selection";
            public static GUIStyle CountBadge = (GUIStyle) "CN CountBadge";
        }
        
        private static float RowHeight => EditorGUIUtility.singleLineHeight + 2f;
        
        private SelectionSnapshot[] history;
        private int current;

        private Vector2 scrollPosition;
        private bool isDragging = false;

        
        [MenuItem("Window/Unitility/Selection History")]
        private static void Open()
        {
            SelectionHistoryWindow window = (SelectionHistoryWindow) GetWindow(typeof(SelectionHistoryWindow));
            window.titleContent = new GUIContent("Selection History");
            window.Show();
        }

        private void OnEnable()
        {
            Refresh();
            SelectionHistoryManager.HistoryChanged += Refresh;
        }

        private void OnDisable()
        {
            SelectionHistoryManager.HistoryChanged -= Refresh;
        }

        private void Refresh()
        {
            this.history = SelectionHistoryManager.History.ToArray().Reverse().ToArray();
            this.current = SelectionHistoryManager.History.Size - SelectionHistoryManager.History.GetCurrentArrayIndex() - 1;
            this.scrollPosition.y = GetTargetScrollPosition();
            Repaint();
        }

        void OnGUI()
        {
            //TODO: Possibly replace with GameObjectTreeViewGUI
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
            for (var i = 0; i < this.history.Length; i++)
            {
                SelectionSnapshot snapshot = this.history[i];
                bool isCurrent = i == current;
                if (!snapshot.IsEmpty) SelectionSnapshotGUI(snapshot, isCurrent, i);
            }
            EditorGUILayout.EndScrollView();
        }
        
        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(EditorGUIUtility.TrTextContent("Clear"), false, Clear);
        }
        
        public void Clear()
        {
            SelectionHistoryManager.Clear();
        }

        public void Select(int index)
        {
            SelectionHistoryManager.HideSelectionFromHistory();
            //Reverse index because auf reversed list
            SelectionHistoryManager.Select(SelectionHistoryManager.Size -index -1);
        }

        private void SelectionSnapshotGUI(SelectionSnapshot snapshot, bool selected, int index)
        {
            Rect rowRect = EditorGUILayout.GetControlRect(
                hasLabel: true,
                height: EditorGUIUtility.singleLineHeight, 
                style: EditorStyles.label);

            rowRect.x -= 2f;
            rowRect.width += 4f;

            var currentEvent = Event.current;

            bool hover = rowRect.Contains(currentEvent.mousePosition);
            
            switch (currentEvent.type)
            {
                case EventType.MouseEnterWindow:
                    Repaint();
                    break;
                case EventType.MouseDown:
                    if (!hover) break;
                    this.isDragging = false;
                    break;
                case EventType.MouseUp:
                    if (!hover) break;
                    this.isDragging = false;
                    Select(index);
                    break;
                case EventType.MouseDrag:
                    if (!hover) break;
                    if (!this.isDragging)
                    {
                        this.isDragging = true;
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.StartDrag(snapshot.ActiveObject.name);
                        DragAndDrop.objectReferences = snapshot.Objects;

                        List<string> paths = new List<string>(snapshot.Objects.Length);
                        foreach (Object obj in snapshot.Objects)
                        {
                            if (EditorUtility.IsPersistent(obj)) paths.Add(AssetDatabase.GetAssetPath(obj));
                        }
                        DragAndDrop.paths = paths.ToArray();
                    }

                    break;
                case EventType.Repaint:
                    DrawSelectionSnapShotGUI(rowRect, snapshot, selected, hover);
                    break;
            }
        }

        private void DrawSelectionSnapShotGUI(Rect rect, SelectionSnapshot snapshot, bool selected, bool hover)
        {
            float paddingLeft = 5f;
            float iconSize = 16f;
            
            //TODO: Draw hover highlight (see: https://github.com/Unity-Technologies/UnityCsReference/blob/61f92bd79ae862c4465d35270f9d1d57befd1761/Editor/Mono/GUI/TreeView/GameObjectTreeViewGUI.cs#L375)
            
            if (selected) Styles.SelectionStyle.Draw(rect, false, false, true, true);

            var contentRect = rect;
            contentRect.xMin += paddingLeft;

            GUIContent content = EditorGUIUtility.ObjectContent(snapshot.ActiveObject, snapshot.ActiveObject.GetType());
            var icon = GetMiniThumbnail(snapshot.ActiveObject);

            var iconRect = contentRect;
            iconRect.width = iconSize;
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);

            var labelRect = contentRect;
            labelRect.xMin += iconSize + 2f;
            labelRect.yMin += 1f;

            Styles.LineStyle.Draw(labelRect, snapshot.ActiveObject.name, false, false, selected, true);

            int count = snapshot.Objects.Length;
            if (count > 1) //Show count badge
            {
                var countRect = contentRect;
                countRect.xMin += countRect.width-27f;
                countRect.yMin += 1;
                Styles.CountBadge.Draw(countRect, new GUIContent($"+{count - 1}"), 0);
            }
        }

        private float GetTargetScrollPosition()
        {
            int rowOffset = 3; //TODO: Add setting for how many future items to show
            int target = this.current - rowOffset;
            return target * RowHeight;
        }

        private static Texture2D GetMiniThumbnail(Object obj)
        {
            if ((bool) obj) return AssetPreview.GetMiniThumbnail(obj);
            else return AssetPreview.GetMiniTypeThumbnail(obj.GetType());
        }
    }
}
