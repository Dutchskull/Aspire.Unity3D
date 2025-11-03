using ConfigToJson;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private async void Start()
    {
        Debug.Log(ConfigProvider.Configuration.ToJsonString());

        string url = ConfigProvider.Configuration.GetValue<string>("services__api__https__0") ??
            ConfigProvider.Configuration.GetValue<string>("services__api__http__0");

        using (HttpClient httpClient = new())
        {
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync("/");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync();
            text.text = content ?? "";
        }
    }
}
