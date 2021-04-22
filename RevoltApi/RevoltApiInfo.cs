﻿using Newtonsoft.Json;

namespace RevoltApi
{
    public class RevoltApiInfo
    {
        [JsonProperty("revolt")] public string Version;
        [JsonProperty("features")] public RevoltApiInfoFeatures Features;
        [JsonProperty("ws")] public string WebsocketUrl;
        [JsonProperty("vapid")] public string Vapid;
    }

    public class RevoltApiInfoFeatures
    {
        [JsonProperty("registration")] public bool Registration;
        [JsonProperty("captcha")] public RevoltApiCaptchaFeature Captcha;
        [JsonProperty("email")] public bool Email;
        [JsonProperty("invite_only")] public bool InviteOnly;
        [JsonProperty("autumn")] public RevoltApiAutumnFeature Autumn;
        [JsonProperty("voso")] public RevoltApiVosoFeature Voso;
    }

    public class RevoltApiCaptchaFeature
    {
        [JsonProperty("enabled")] public bool Enabled;
        [JsonProperty("key")] public string Key;
    }

    public class RevoltApiAutumnFeature
    {
        [JsonProperty("enabled")] public bool Enabled;
        [JsonProperty("url")] public string Url;
    }

    public class RevoltApiVosoFeature
    {
        [JsonProperty("enabled")] public bool Enabled;
        [JsonProperty("url")] public string Url;
        [JsonProperty("ws")] public string WebsocketUrl;
    }

    public class VosoInformation
    {
        [JsonProperty("voso")] public string Version;
        [JsonProperty("ws")] public string WebsocketUrl;
        [JsonProperty("features")] public VosoFeatures Features;
    }

    public class VosoFeatures
    {
        [JsonProperty("rtp")] public bool Rtp;
    }
}