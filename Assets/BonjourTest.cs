using UnityEngine;

public class BonjourTest : MonoBehaviour 
{

    bool querying = false;

    string label;
    string status;
    // Default service name. _http._tcp corresponds to http services (f.e. printers)
    string service = "_http._tcp.";

    string[] services = new System.String[0];

    int centerX = Screen.width / 2;

    GUIStyle labelStyle = new GUIStyle();

    void Start () 
    {
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = Color.white;
	}

	void OnGUI()
	{
        GUI.Label(new Rect(centerX - 50, 25, 100, 25), "Bonjour Client", labelStyle);
        service = GUI.TextField(new Rect(centerX - 125, 50, 175, 25), service);

        if (!querying && GUI.Button(new Rect(centerX + 50, 50, 75, 25), "Query"))
        {
            // Start lookup for specified service inside "local" domain
            Bonjour.StartLookup(service, "local.");
            querying = true;
            status = "";
        }
        if (querying)
        {
            // Query status only every 10th frame. Managed -> Native calls are quite expensive.
            // Similar coding pattern could be considered as good practice. 
            if (Time.frameCount % 10 == 0)
            {
                status = Bonjour.GetLookupStatus();
                services = Bonjour.GetServiceNames();
                label = status;
            }

            if (status == "Done")
                querying = false;

            //Stop lookup
            if (querying && GUI.Button(new Rect(centerX + 50, 50, 75, 25), "Stop"))
                Bonjour.StopLookup();

        }

        // Display status
        GUI.Label(new Rect(centerX - 50, 75, 100, 25), label, labelStyle);

        // List of looked up services
        for (int i = 0; i < services.Length; i++)
        {
            GUI.Button(new Rect(centerX - 75, 100 + i * 25, 150, 25), services[i]);
        }
    }
}
