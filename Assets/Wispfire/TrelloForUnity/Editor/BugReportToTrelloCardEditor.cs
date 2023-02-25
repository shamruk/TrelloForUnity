using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Wispfire.BugReporting;
using Wispfire.TrelloForUnity;

namespace Wispfire.TrelloForUnity.Editor {
    [CustomEditor(typeof(BugReportToTrelloCard), true)]
    public class BugReportToTrelloCardEditor : UnityEditor.Editor {
        private BugReportToTrelloCard Target => (BugReportToTrelloCard)target;
        
        
        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();
            root.Add(new IMGUIContainer(base.OnInspectorGUI));
            root.Add(base.CreateInspectorGUI());
            root.Add(new Button(GetBoardListIds) { text = nameof(GetBoardListIds) });
            return root;
        }

        private void GetBoardListIds() {
            Application.OpenURL("https://trello.com/1/boards/XXXXXXXX/lists");
        }
    }
}