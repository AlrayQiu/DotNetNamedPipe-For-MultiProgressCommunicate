namespace SingleProcess;

using System.Diagnostics;
using Rosidl.Messages.Builtin;
using Tlarc;

class Client : Component
{
    Server server;
    public override void Start()
    {
    }
    public override void Update()
    {
        Console.WriteLine("time : {0:f4}", (DateTime.Now.Ticks - server.tick) / 1e7 * 1e3);
    }
}