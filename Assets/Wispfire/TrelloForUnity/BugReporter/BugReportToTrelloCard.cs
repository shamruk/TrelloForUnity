﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Wispfire.TrelloForUnity;

namespace Wispfire.BugReporting
{
    public class BugReportToTrelloCard : MonoBehaviour {
        public string cheatCodeTitle;
        public UnityEvent onCheatCode = new ();
        public Trello Client;

        [SerializeField]
        private string BugReportListID;

        public void HandleBugReport(BugReport report, Action OnDone)
        {
            if (report.Title == cheatCodeTitle) {
                onCheatCode.Invoke();
                return;
            }
            StartCoroutine(handleBugReport(report, OnDone, string.IsNullOrEmpty(BugReportListID) ? Client.authenticator.DefaultListID : BugReportListID));
        }

        IEnumerator handleBugReport(BugReport report, Action OnDone, string targetList)
        {
            // Turn bug report info into card
            var title = string.Empty;
            title += report.Title;
            if (!string.IsNullOrEmpty(report.Category)) {
                title += " #" + removeWhiteSpace(report.Category);    
            }
            if (report.Vip) {
                title += " #VIP";    
            }
            title += " #" + removeWhiteSpace(report.Version);
            title += " #" + removeWhiteSpace(report.Platform);
            title += " #" + removeWhiteSpace(report.SceneName);

            string description = string.Empty;
            description += "Username: " + report.Username + "\n";
            description += "E-Mail: " + report.Email + "\n";
            description += "\n";
            description += "Report: \n" + report.Description + "\n";

            //create card
            var CreateCard = Client.CreateTrelloCard(targetList, title, description);
            yield return CreateCard;

            if (!string.IsNullOrEmpty(CreateCard.error))
            {
                Debug.LogError("Creating trello card failed with error: " + CreateCard.error);
                yield break;
            }

            // Add attachments
            for (int i = 0; i < report.TextFiles.Count; i++)
            {
                var addText = Client.AddAttachmentToCard(CreateCard.Result.id, report.TextFiles[i].GetFilename(), report.TextFiles[i].GetBytes());
                yield return addText;
                if (!string.IsNullOrEmpty(addText.error))
                {
                    Debug.LogError("Adding text attachment to trello card failed with error: " + addText.error);
                }
            }
                        
            for (int i = 0; i < report.Screenshots.Count; i++)
            {
                var addScreenshot = Client.AddAttachmentToCard(CreateCard.Result.id, report.Screenshots[i].GetFilename(), report.Screenshots[i].GetBytes());
                yield return addScreenshot;
                if (!string.IsNullOrEmpty(addScreenshot.error))
                {
                    Debug.LogError("Adding image attachment to trello card failed with error: " + addScreenshot.error);
                }
            }
            if (OnDone != null) { OnDone(); }
        }

        string removeWhiteSpace(string source)
        {
            return source.Replace(" ", "");
        }
    }
}

