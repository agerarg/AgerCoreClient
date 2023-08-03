using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
public class NetworkClient : MonoBehaviour
{
    public static NetworkClient instance;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];

    private Queue<string> receivedMessages = new Queue<string>();

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
        instance = this;

    }
    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient("localhost", 12345);
            stream = client.GetStream();
            Debug.Log("Connected to server.");
            StartCoroutine(ReceiveDataFromServer());
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect to the server: " + e.Message);
            SceneManager.LoadScene("Disconnected");
        }
    }

    public void SendDataToServer(string call,string[,] stringArray)
    {

        string message = call+"#{";
        bool moreThanOne = false;
        for (int row = 0; row < stringArray.GetLength(0); row++)
        {
            if (moreThanOne)
                message += ",";
            for (int col = 0; col < stringArray.GetLength(1); col++)
            {
                string value = stringArray[row, col];
                // Your logic to process each element (value) goes here
                //Debug.Log($"Value at row {row}, col {col}: {value}");
                if(col==0)
                    message += '"'+value+'"'+':';
                else
                    message += '"' + value + '"';
            }
            moreThanOne = true;
        }

        message += "}";

        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent data to server: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data to the server: " + e.Message);
        }
    }
     
    private void Update()
    {
        
        // Process received messages on the main Unity thread
        while (receivedMessages.Count > 0)
        {
            string message = receivedMessages.Dequeue();
            HandleResponse(message);
        }

    }

    private void HandleResponse(string data)
    {
        Debug.Log("data from server:" + data);
        string[] splitData = data.Split(char.Parse("#"));
        switch (splitData[0])
        {
            case "WORLD":
                switch (splitData[1])
                {
                    case "MOVE":

                        Json_Movement move = JsonUtility.FromJson<Json_Movement>(splitData[2]);
                        NetworkEvents.PlayerMovement?.Invoke(move.id, move.x, move.y);
                        
                   break;
                    case "PLAYER":
                        Json_Player player = JsonUtility.FromJson<Json_Player>(splitData[2]);

                        NetworkEvents.NewPlayer?.Invoke(player);
                      //  Debug.Log("load player from area: " + player.name);
                     break;
                    case "DATA":
                        Json_MapLoad loadMap = JsonUtility.FromJson<Json_MapLoad>(splitData[2]);
                        NetworkEvents.LoadMap?.Invoke(loadMap);
                    break;
                    case "DISCONNECTED":
                        Json_Disconnected disc = JsonUtility.FromJson<Json_Disconnected>(splitData[2]);
                        NetworkEvents.RemovePlayer?.Invoke(disc.id);
                    break;
                }
                break;
            case "MSG":
                switch (splitData[1])
                {
                    case "TXT":
                        Debug.Log("Received MSG from server: " + splitData[2]);
                        break;
                    case "LOGINOK":
                        Debug.Log("Loged success!");
                        NetworkEvents.login?.Invoke(true,"sucess");
                        break;
                    case "REGISTEROK":
                        Debug.Log("Register success!");
                        break;
                    case "CHARACTER":
                        switch (splitData[2])
                        {
                            case "ENTER":
                                Json_Character loadChar = JsonUtility.FromJson<Json_Character>(splitData[3]);
                                NetworkEvents.CharacterEnter?.Invoke(loadChar);
                           break;
                            case "LIST":
                                //Debug.Log("Show characters:" + splitData[3]);

                                Json_CharacterList newCharList = JsonUtility.FromJson<Json_CharacterList>(splitData[3]);
                                NetworkEvents.characterList?.Invoke(newCharList);
                                break;
                            case "SELECTED":
                                //Debug.Log("Show selected character previusly:" + splitData[3]);
                            break;
                        }
                   break;
                }
            break;
            case "ERROR":
                switch (splitData[1])
                {
                    case "DISCONNECTED":
                        SceneManager.LoadScene("Disconnected");
                        Destroy(gameObject);
                    break;
                    case "LOGIN":
                        Debug.Log("login fail!");
                        NetworkEvents.login?.Invoke(false, "fail");
                        break;
                    case "USERALREADYINUSE":
                        Debug.Log("username already in use!");
                    break;
                    case "REGISTER":
                        Debug.Log("Error: REGISTER CODE:" + splitData[2]);
                        break;
                    case "CHARACTER":
                        Debug.Log("Error: CHARACTER CODE:" + splitData[2]);
                        break;
                }
                break;
            default:
                Debug.LogError(" Network type not define: "+ data);
                break;
        }
    }

    private IEnumerator ReceiveDataFromServer()
    {
        while (true)
        {
            try
            {
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    string receivedData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

                    string[] jumperData = receivedData.Split("|");
                    foreach (string message in jumperData)
                    {
                        if(message.Length>1)
                            receivedMessages.Enqueue(message);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data from the server: " + e.Message);
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (stream != null)
            stream.Close();

        if (client != null)
            client.Close();
    }
}
