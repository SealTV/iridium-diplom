using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class ServerConnector
{

    private Task connectTask;

    private Stream workStream;

	public void SeverConnector()
	{
	}

    private void Start()
    {
        connectTask = Task.Factory.StartNew(this.Connect);
    }

    private void Connect()
    {
        
    }
}
