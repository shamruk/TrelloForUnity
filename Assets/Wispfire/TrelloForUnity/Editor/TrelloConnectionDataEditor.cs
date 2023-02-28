using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Wispfire.TrelloForUnity;

namespace Wispfire.TrelloForUnity.Editor {
    [CustomEditor(typeof(TrelloConnectionData), true)]
    public class TrelloConnectionDataEditor : UnityEditor.Editor {
        private TrelloConnectionData Target => (TrelloConnectionData)target;
        
        private string appName = "UnityIntegration";
        private string keyUrl = "https://trello.com/app-key";
        private string tokenUrl = "https://trello.com/1/authorize?expiration=never&scope=read,write,account&response_type=token&name=UnityIntegration&key=";
        
        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();
            root.Add(new IMGUIContainer(base.OnInspectorGUI));
            root.Add(base.CreateInspectorGUI());
            root.Add(new Button(GetKey) { text = nameof(GetKey) });
            root.Add(new Button(GetToken) { text = nameof(GetToken) });
            root.Add(new Button(SetAppName) { text = nameof(SetAppName) });
            root.Add(new Button(GetBoardListIds) { text = nameof(GetBoardListIds) });
            return root;
        }
        
        private void GetKey() {
            Application.OpenURL(keyUrl);
        }

        private void GetToken() {
            if (string.IsNullOrEmpty(Target.APIKey)) {
                Debug.LogError("get key first");
                return;
            }
            Application.OpenURL(tokenUrl + Target.APIKey);
        }

        private void SetAppName() {
            Target.ApplicationName = appName;
        }
        
        private void GetBoardListIds() {
            Application.OpenURL("https://trello.com/1/boards/XXXXXXXX/lists");
        }
    }
}