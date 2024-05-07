using System;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using IngameDebugConsole;

public class VoiceChat : MonoBehaviour
{
    bool joined = false;
    float posUpdateTime = 0.5f;
    float curTime;
    Channel3DProperties props;

    [SerializeField] int audibleDistance;
    [SerializeField] int conversationalDistance;
    [SerializeField] float audioFadeIntensityByDistance;
    [SerializeField] GameObject cam;

    private void Start() 
    {
        DebugLogConsole.AddCommand("VC", "Joins the Vc", Init);
        cam = GameObject.Find("Camera");
        curTime = posUpdateTime;
    }

    public void Init()
    {
        InitializeAsync();
    }

    void Update()
    {
        curTime -= Time.deltaTime;
        if (joined && curTime < 0) 
        {
            VivoxService.Instance.Set3DPosition(cam, "Lobby");
            curTime = posUpdateTime;
        }
        
        props = new Channel3DProperties(
            audibleDistance, conversationalDistance, audioFadeIntensityByDistance, AudioFadeModel.InverseByDistance);
    }

    async void InitializeAsync()
    {
        print("start INIT");
        await UnityServices.InitializeAsync();

        // if (!AuthenticationService.Instance.IsSignedIn) {
        //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //     // PlayerId = AuthenticationService.Instance.PlayerId;
        // }

        await VivoxService.Instance.InitializeAsync();
        print("end INIT");

        LoginToVivoxAsync();
    }

    public async void LoginToVivoxAsync()
    {
        print("start LOGIN");
        LoginOptions options = new LoginOptions();
        options.DisplayName = "player";
        options.EnableTTS = false;
        await VivoxService.Instance.LoginAsync(options);;
        print("end LOGIN");

        JoinChannelAsync("Lobby"); 
    }

    async void JoinChannelAsync(string channelName)
    {
        print("start JOIN");
        //Join channel with name channelName and capability for text and audio transmission
        await VivoxService.Instance.JoinPositionalChannelAsync("Lobby", ChatCapability.TextAndAudio, props);
        VivoxService.Instance.Set3DPosition(gameObject, "Lobby");
        print("end JOIN");

        joined = true;
    }
}
