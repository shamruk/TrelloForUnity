using System;
using System.Collections;
using UnityEngine;

namespace Wispfire.BugReporting {
    [RequireComponent(
        typeof(SessionLogger),
        typeof(BugReportToTrelloCard)
    )]
    public class BugReporter : MonoBehaviour {
        
        public BugReporterGUIController ReportInterface;

        private Texture2D cachedScreenshot;
        private SessionLogger sessionLog;
        private BugReportToTrelloCard client;
        
        public string category;
        public bool SkipScreenshot;
        public Func<string> StateGetter;
        public bool vip;
        public bool skipLogs;
        public string customLogs = string.Empty;

        void Start() {
            sessionLog = GetComponent<SessionLogger>();
            client = GetComponent<BugReportToTrelloCard>();

            if (ReportInterface != null) {
                ReportInterface.OnSubmit.AddListener(OnBugSubmit);
            }
        }

        void OnBugSubmit(BugReportUserData data) {
            StartCoroutine(generateBugReport(data));
        }

        IEnumerator generateBugReport(BugReportUserData data) {
            if (!SkipScreenshot) {
                yield return StartCoroutine(screenGrabAndCache());
            }

            Debug.Log("Submitting bug report.");

            string version = Application.version; // This gets the version number from Project Settings->Player under Other Settings.

            var report = BugReport.CreateInstance<BugReport>();
            report.SetData(
                data,
                "",
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                version,
                Application.platform);
            report.Category = category;
            report.Vip = vip;
            if (!SkipScreenshot) {
                report.AddScreenshot("screenshot_" + System.DateTime.UtcNow.ToShortTimeString(), cachedScreenshot);
            }
            if (!skipLogs) {
                report.AddTextAttachment("device", SystemInformation.GetDebugSystemInfo(), "txt");
                report.AddTextAttachment("logs", sessionLog.PrintLog() + "\nCustomLogs:\n" + customLogs, "txt");
            }
            if (StateGetter != null) {
                report.AddTextAttachment("state", StateGetter(), "txt");
            }

            Send(report);
        }

        public void Send(BugReport report) {
            client.HandleBugReport(report, OnBugReportSent);
        }

        void OnBugReportSent() {
            Debug.Log("Bug report sent!");
        }


        IEnumerator screenGrabAndCache() {
            yield return new WaitForEndOfFrame();
            // Create a texture the size of the screen, RGB24 format
            int width = Screen.width;
            int height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            cachedScreenshot = tex;
        }

    }

    public struct BugReportUserData {
        public string Title;
        public string Description;
        public string Email;

        public BugReportUserData(string title, string description, string email) {
            Title = title;
            Description = description;
            Email = email;
        }
    }
}