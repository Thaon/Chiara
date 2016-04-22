using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerUtilityScript : MonoBehaviour {

    string m_typeName = "Game Engines network scene";
    string m_gameName = "Game Engines network scene";
    HostData[] m_hostList;
    string m_ipAddress = "xxx.xxx.xxx.xxx";

    public InputField m_inputField;

    public GameObject m_voxelChunkPrefab;
    public GameObject m_canvas;
    public GameObject m_networkFPCPrefab;
    public GameObject m_mainCamera;

    void Start ()
    {
        MasterServer.ipAddress = m_ipAddress;
    }
	
	void Update ()
    {
	    
	}

    public void UpdateIP()
    {
        m_ipAddress = m_inputField.text;
    }

    public void StartServer()
    {
        if (!Network.isServer && !Network.isClient)
        {
            Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
            MasterServer.RegisterHost(m_typeName, m_gameName);
        }
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
        Network.Instantiate(m_voxelChunkPrefab, Vector3.zero, Quaternion.identity, 0);        m_canvas.SetActive(false);
    }
    public void RefreshHostList()
    {
        MasterServer.RequestHostList(m_typeName);
    }    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            m_hostList = MasterServer.PollHostList();
            foreach (HostData hd in m_hostList)
            {
                if (hd.gameName == m_gameName)
                {
                    Network.Connect(hd);
                }
            }
        }
    }    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        m_canvas.SetActive(false);
        m_mainCamera.SetActive(false);
        SpawnPlayer();
    }    void SpawnPlayer()
    {
        Network.Instantiate(m_networkFPCPrefab, new Vector3(8, 8, 8), Quaternion.identity, 0);
    }
}
