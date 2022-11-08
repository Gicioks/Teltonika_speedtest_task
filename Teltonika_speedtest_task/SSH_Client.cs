using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SSH_client
{
	private SshClient client = null;

	private string IP = null;
	private int Port = -1;
	private string User = null;
	private string Password = null;

	/// <summary>
	/// SSH client
	/// </summary>
	/// <param name="IP">Device IP</param>
	/// <param name="User">Username</param>
	/// <param name="Password">Password</param>
	/// <param name="Port">SSH Port</param>
	public SSH_client(string IP = "192.168.1.1", int Port = 22, string User="root", string Password="admin01")
    {
		this.IP = IP;
		this.Port = Port;
		this.User = User;
		this.Password = Password;

		Connect();
    }

	private void Connect()
    {
		if (client == null)
		{
			client = new SshClient(IP, Port, User, Password);
			client.Connect();
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

	public string ExecuteCommand(string cmd)
    {
		string result = null;
		//string error = null;

		if (client != null && cmd != null && cmd.Length > 0)
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
		string response = null;
		response = ExecuteCommand("cat /etc/config/system | grep routername | awk -F\"'\" '{print $2}'").Trim();

		return response;
    }
}

