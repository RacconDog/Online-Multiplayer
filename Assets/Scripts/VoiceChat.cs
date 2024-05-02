using System;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;

public class VoiceChat : MonoBehaviour
{
    private void Awake() {
        InitializeAsync();
    }

    public void Init()
    {
        LoginToVivoxAsync();
        JoinEchoChannelAsync();
        JoinChannelAsync("Lobby");

    }

    async void InitializeAsync()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await VivoxService.Instance.InitializeAsync();
    }

    public async void JoinEchoChannelAsync()
    {
        string channelToJoin = "Lobby";
        await VivoxService.Instance.JoinEchoChannelAsync(channelToJoin, ChatCapability.TextAndAudio);
    }

    public async void LoginToVivoxAsync()
    {
        LoginOptions options = new LoginOptions();
        options.DisplayName = "asdf";
        options.EnableTTS = false;
        await VivoxService.Instance.LoginAsync(options);
    }

    public async void LeaveEchoChannelAsync()
    {
        string channelToLeave = "Lobby";
        await VivoxService.Instance.LeaveChannelAsync(channelToLeave);
    }

    async void JoinChannelAsync(string channelName)
    {
        //Join channel with name channelName and capability for text and audio transmission
        await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.TextAndAudio);
    }
}
