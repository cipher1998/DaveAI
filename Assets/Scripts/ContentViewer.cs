using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContentViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] AudioSource audiosource;

	private static ContentViewer contentViewer = null;

	private void Awake()
	{
		if(contentViewer == null)
		{
			contentViewer = this;
		}
		else
		{
			Destroy(contentViewer);
		}
	}
	public static void ShowContent(string placeholderValue , AudioClip clip)
	{
		contentViewer.placeholder.text = placeholderValue;
		contentViewer.audiosource.clip = clip;
		contentViewer.audiosource.Play();
	}
}
