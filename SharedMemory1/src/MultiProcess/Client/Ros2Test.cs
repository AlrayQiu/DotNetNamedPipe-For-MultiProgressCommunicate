namespace Client.Ros2;

using Tlarc;
using Tlarc.IO.ROS2Msgs;
using Tlarc.IO.ROS2Msgs.Std;

class Client : Component
{
    public FloatMultiArray msg;
    public override void Start()
    {
        msg = new FloatMultiArray(IOManager);

        msg.Subscript("tlarc/test", (data) =>
        {
            Console.WriteLine("{0:F4}\n", (DateTime.Now.Ticks & 0xffff) / 10000f - data[0]);
        });
    }
    public override void Update()
    {
    }
}