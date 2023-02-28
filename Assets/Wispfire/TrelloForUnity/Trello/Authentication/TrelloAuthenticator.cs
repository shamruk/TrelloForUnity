using UnityEngine;

namespace Wispfire.TrelloForUnity
{
    public abstract class TrelloAuthenticator : ScriptableObject
    {        
        [SerializeField]
        public string DefaultListID;
        public abstract string GetAuthString();
    }
}

