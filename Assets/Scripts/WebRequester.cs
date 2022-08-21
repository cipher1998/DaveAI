using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequester : MonoBehaviour
{
	private static WebRequester webRequester = null;
	private void Awake()
	{
		if (webRequester == null)
		{
			webRequester = this;
		}
		else
		{
			Destroy(webRequester);
		}
	}
	private static string placeholderText;
	private static AudioClip clip;
	private static string URL = $"https://test.iamdave.ai/conversation/exhibit_aldo/74710c52-42a5-3e65-b1f0-2dc39ebe42c2"; 
	public static void RequestData(string state)
	{
		webRequester.StartCoroutine(RequestDataCoroutine(state));
	}

	private static IEnumerator RequestDataCoroutine(string state)
	{
		string json = $"{{\"data-raw\": {{\"system_response\":\"sr_init\",\"engagement_id\":\"NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyZXhoaWJpdF9hbGRv\" , \"customer_state\":\"{state}\"}}}}";
		byte[] jsonbytes = Encoding.UTF8.GetBytes(json);
		using (UnityWebRequest www =  new UnityWebRequest(URL , "POST"))
		{
			www.uploadHandler = new UploadHandlerRaw(jsonbytes);
			www.downloadHandler = new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application / json");
			www.SetRequestHeader("X-I2CE-ENTERPRISE-ID", "dave_expo");
			www.SetRequestHeader("X-I2CE-USER-ID", "74710c52-42a5-3e65-b1f0-2dc39ebe42c2");
			www.SetRequestHeader("X-I2CE-API-KEY", "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyMTYwNzIyMDY2NiAzNw__");
			yield return www.SendWebRequest();

			if(www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(www.result.ToString());
			}
			else
			{
				JObject jobject = JsonConvert.DeserializeObject<JObject>(www.downloadHandler.text);
				Debug.Log(jobject.ToString());
				string audioClipUrl = jobject["response_channels"]["voice"].ToString();
				placeholderText = jobject["placeholder"].ToString();
				webRequester.StartCoroutine(RequestAudioClip(audioClipUrl));
			}
		}
	}
	private static IEnumerator RequestAudioClip(string url)
	{
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				clip = DownloadHandlerAudioClip.GetContent(www);
				ContentViewer.ShowContent(placeholderText, clip);
			}
		}
	}
}
