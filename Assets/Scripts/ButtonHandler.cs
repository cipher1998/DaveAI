using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
	public void OnRequestButtonClicked(string customerState)
	{
		WebRequester.RequestData(customerState);
	}
}
