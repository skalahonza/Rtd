using UnityEngine.Networking;

/// <summary>
/// Instantiate packet
/// </summary>
public class InstantiateData : MessageBase {
	public LobbyPlayerData[] players;
	public int mapIndex;
	public int ID;
}