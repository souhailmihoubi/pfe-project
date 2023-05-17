using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;

    public string userID;

    public string username;

    bool isConnected;

    [SerializeField] GameObject connectingPanel;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] TextMeshProUGUI chatDisplay;

    [SerializeField] TMP_InputField chatInput;

    [SerializeField] GameObject chatPanel;

    string currentChat ;


    private void Start()
    {
        userID = "2566"; // naatih l user id baad l authentification!!

        username = PhotonNetwork.NickName;

        isConnected = true;

        chatClient = new ChatClient(this);

        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));

        text.text = "Chat is connecting ...";

    }

    private void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

    }

    public void OnConnected()
    {
        text.text = "Chat connected!";

        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        connectingPanel.SetActive(false);

        chatPanel.SetActive(true);
    }

    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    public void SubmitPublicChatOnClick()
    {
        chatClient.PublishMessage("RegionChannel", currentChat);

        chatInput.text = "";

        currentChat = "";

    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(message);
    }

    public void OnChatStateChange(ChatState state)
    {

        switch (state)
        {
            case ChatState.ConnectingToNameServer:
                Debug.Log("Chat connecting!");
                break;
            case ChatState.ConnectedToNameServer:
                Debug.Log("Chat connected!");
                break;
            case ChatState.Disconnected:
                Debug.Log("Chat disconnected!");
                break;
            default:
                break;
        }
    }



    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";

        for(int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0}: {1}", senders[i], messages[i]);

            chatDisplay.text += "\n " + msgs;
            
            Debug.Log(msgs);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }


}
