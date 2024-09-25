namespace Server.Ros2;

using Tlarc;
using Tlarc.IO.ROS2Msgs;
using Tlarc.IO.ROS2Msgs.Std;

class Server : Component
{
    public FloatMultiArray msg;
    float[] data = new float[1];
    public override void Start()
    {
        msg = new FloatMultiArray(IOManager);

        msg.RegistryPublisher("tlarc/test");
    }
    public override void Update()
    {
        data[0] = (DateTime.Now.Ticks & 0xffff) / 10000f;
        msg.Publish(data);
    }
}