namespace MMAR
{
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine.Networking;

    public static class WebHelper
    {
        public static async Task<string> GetDataFromUrl(string url, string body)
        {
            using var req = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            if (req.result != UnityWebRequest.Result.Success)
                throw new System.Exception($"Login failed: {req.error} {req.downloadHandler.text}");
            return req.downloadHandler.text;
        }
    }

}