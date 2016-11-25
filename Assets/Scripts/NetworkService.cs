using UnityEngine;
using System.Collections;
using System;

public class NetworkService {
    //private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml&APPID=48e0ee8f49f90feae2ee221bd0de3c3a";
    //private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&APPID=48e0ee8f49f90feae2ee221bd0de3c3a";
    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Shanghai,CN&APPID=48e0ee8f49f90feae2ee221bd0de3c3a";
    private const string webImage = "http://b3-q.mafengwo.net/s7/M00/F5/1A/wKgB6lPMqkWAOiz8AAFvh9XPX5Y92.jpeg?imageView2%2F2%2Fw%2F300%2Fh%2F300%2Fq%2F90";
    //private const string jsonApi = "http://localhost:8080/Weather/index.do";

    private bool IsResponseValid(WWW www) {
        if (www.error != null) {
            Debug.Log("bad connection");
            return false;
        } else if (string.IsNullOrEmpty(www.text)) {
            Debug.Log("bad data");
            return false;
        } else {
            return true;
        }
    }

    private IEnumerator CallAPI(string url, Action<string> callback) {
        WWW www = new WWW(url);
        yield return www;
        if (!IsResponseValid(www)) {
            yield break;
        }
        callback(www.text);
    }

    public IEnumerator GetWeatherJSON(Action<string> callback) {
        return CallAPI(jsonApi, callback);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback) {
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }
}
