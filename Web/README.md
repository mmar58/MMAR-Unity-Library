# Web

## WebHelper.cs

A static helper for async POST requests using UnityWebRequest.

* Handles JSON body, error checking, and async/await pattern.
* Simplifies backend communication from Unity.
* Throws exceptions on request failure for robust error handling.

### Usage Example

```csharp
using System.Threading.Tasks;
using MMAR;

// Example async method in a MonoBehaviour
public async Task SendData()
{
	string url = "https://your.api/endpoint";
	string jsonBody = "{\"key\":\"value\"}";
	try
	{
		string response = await WebHelper.GetDataFromUrl(url, jsonBody);
		Debug.Log($"Server response: {response}");
	}
	catch (System.Exception ex)
	{
		Debug.LogError($"Request failed: {ex.Message}");
	}
}
```


