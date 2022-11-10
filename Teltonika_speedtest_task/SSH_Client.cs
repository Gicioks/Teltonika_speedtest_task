using Renci.SshNet;
using System;

public class SSH_client
{
	private SshClient client = null;

	private string ip = null;
	private int port = -1;
	private string user = null;
	private string password = null;

	/// <summary>
	/// SSH client
	/// </summary>
	/// <param name="ip">Device IP</param>
	/// <param name="user">Username</param>
	/// <param name="password">Password</param>
	/// <param name="port">SSH Port</param>
	public SSH_client(string ip = "192.168.1.1", int port = 22, string user="root", string password="admin01")
    {
		this.ip = ip;
		this.port = port;
		this.user = user;
		this.password = password;

		Connect();
    }

	private void Connect()
    {
		if (client == null)
		{
			client = new SshClient(ip, port, user, password);
			try
			{
				client.Connect();
			}
			catch (Exception e) { }
		}
    }

	/// <summary>
	/// Might be uneccessary/obsolete
	/// </summary>
	public void Disconnect()
    {
		if (client != null)
        {
			client.Disconnect();
			client = null;
        }
    }

	public bool IsConnected()
	{
		if (client == null)
		{
			return false;
		}

		return client.IsConnected;
	}

	public string ExecuteCommand(string cmd)
    {
		string result = null;
		//string error = null;

		if (IsConnected() && cmd != null && cmd.Length > 0)
        {
			var command = client.CreateCommand(cmd);
			command.Execute();
			result = command.Result;
			//error = command.Error;

			return result;
        }

		return null;
    }

	public string GetDevice()
    {
		return ExecuteCommand("cat /etc/config/system | grep routername | awk -F\"'\" '{print $2}'").Trim();
    }
}

