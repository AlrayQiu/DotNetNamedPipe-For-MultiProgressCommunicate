namespace SingleProcess;

using Rosidl.Messages.Builtin;
using Tlarc;

class Server : Component
{
    public long tick;
    public override void Start()
    {
    }
    public override void Update()
    {
        tick = DateTime.Now.Ticks;
    }
}