using UnityEngine.Networking;

/// <summary>
/// Player option change packet
/// </summary>
public class UpdatePlayerData : MessageBase {
	public LobbyPlayerData data;
}