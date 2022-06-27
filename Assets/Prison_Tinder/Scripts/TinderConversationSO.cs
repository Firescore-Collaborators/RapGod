using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TinderConversationSO", menuName = "Tinder/TinderConversationSO")]
public class TinderConversationSO : ScriptableObject
{
    public List<string> recipientNeutralMessage;
    public List<string> userPositiveMessage;
    public List<string> userNegativeMessage;
    public List<string> recipientPositiveReply;
    public List<string> recipientNegativeReply;

}
