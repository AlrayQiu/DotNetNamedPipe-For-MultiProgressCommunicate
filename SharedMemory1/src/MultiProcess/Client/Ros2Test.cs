namespace Client.Ros2;

using Tlarc;
using Tlarc.IO.ROS2Msgs;
using Tlarc.IO.ROS2Msgs.Std;

class Client : Component
{
    public Int32 int32;
    public override void Start()
    {
        int32 = new Int32(IOManager);

        int32.Subscript("tlarc/test", (data) => { Console.WriteLine("{0:D}\n", (int)DateTime.Now.Ticks - data); });
    }
    public override void Update()
    {
    }
}