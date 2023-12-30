using System.Collections.Generic;
using UnityEngine;
using Wispfire.TrelloForUnity;

namespace Wispfire.BugReporting
{
    public class BugReport : ScriptableObject
    {

        public string Title;
        public string Username;
        public string Email;
        [TextArea(5, 10)]
        public string Description;
        public string SceneName;
        public string Version;
        public string Platform;
        public bool Vip;
        public string Category;

        public List<ImageAttachment> Screenshots = new List<ImageAttachment>();
        public List<TextAttachment> TextFiles = new List<TextAttachment>();


        public void SetData(BugReportUserData userdata, string username, string sceneName, string version, RuntimePlatform platform)
        {
            Description = userdata.Description;
            Email = userdata.Email;
            Title = userdata.Title;
            SceneName = sceneName;
            Username = username;
            Version = version;
            Platform = platform.ToString();
        }

        public void AddTextAttachment(string name, string content, string extension)
        {
            TextFiles.Add(new TextAttachment(name, content, extension));
        }

        public void AddScreenshot(string name, Texture2D screenshot)
        {
            Screenshots.Add(new ImageAttachment(name, screenshot, ImageAttachment.TextureEncoding.jpg));
        }

        void OnDestroy()
        {
            for (int i = 0; i < Screenshots.Count; i++)
            {
                Screenshots[i].OnDestroy();
            }
        }

    }
}
