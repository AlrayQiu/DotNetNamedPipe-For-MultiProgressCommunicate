namespace Server.Ros2;

using Tlarc;
using Tlarc.IO.ROS2Msgs;
using Tlarc.IO.ROS2Msgs.Std;

class Server : Component
{
    public Int32 int32;
    DateTime last;
    public override void Start()
    {
        int32 = new Int32(IOManager);

        int32.RegistryPublisher("tlarc/test");
        last = DateTime.Now;
    }
    public override void Update()
    {
        Console.WriteLine("FPS:{0}", 1 / (DateTime.Now - last).Duration().TotalSeconds);
        last = DateTime.Now;
        int32.Publish((int)DateTime.Now.Ticks);
    }
}