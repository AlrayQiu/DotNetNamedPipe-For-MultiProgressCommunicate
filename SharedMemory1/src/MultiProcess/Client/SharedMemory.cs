namespace Client.SharedMemoryTest;

using SharedMemory;
using Tlarc;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;

unsafe class SpmcRingBuffer : SharedBuffer
{
    int _bufferSize;
    int _position = 0;
    byte[] buffer;
    private SpmcRingBuffer(string name, int bufferSize, bool ownsSharedMemory) : base(name, bufferSize, ownsSharedMemory)
    {
        Open();
        if (base.IsOwnerOfSharedMemory)
        {
            _bufferSize = bufferSize + sizeof(int);
            Write(ref _bufferSize, 0);
            Write(-1, 4);
            _position = 8;
            return;
        }
        Read(out _bufferSize, 0);
        Read(out _position, 4);
        byte[] buffer = new byte[_bufferSize];
    }
    public static SpmcRingBuffer CreateNew(string name, int bufferSize) => new(name, bufferSize, true);
    public static SpmcRingBuffer OpenExist(string name) => new(name, 0, false);
    private void Write(int source, bool position = false)
    {
        var arr = BitConverter.GetBytes(source);
        if (_position + 4 > _bufferSize)
        {
            WriteArray(arr, 0, _bufferSize - _position, _position);
            WriteArray(arr, 0, 4 - _bufferSize + _position, 4);
            _position = 4 - _bufferSize + _position + 4;
        }
        else
        {
            WriteArray(arr, 0, 4, _position);
            _position = 4 + _position;
        }
    }
    private void WritePosition(int position)
    {
        var arr = BitConverter.GetBytes(position);
        WriteArray(arr, 0, 4, 4);
    }
    public void Write<T>(T[] values) where T : struct
    {
        Span<byte> buffer = MemoryMarshal.Cast<T, byte>(values);
        var p = _position;
        Write(buffer.Length);
        foreach (var b in buffer)
            Write(b);
        WritePosition(p);
    }
    private void ReadPosition()
    {
        var buffer = new byte[4];
        ReadArray(buffer, 0, 4, 4);
        _position = BitConverter.ToInt32(buffer, 0);
    }
    private int ReadSize()
    {
        var arr = new byte[4];
        if (_position + 4 > _bufferSize)
        {
            ReadArray(arr, 0, _bufferSize - _position, _position);
            ReadArray(arr, 0, 4 - _bufferSize + _position, 4);
            _position = 4 - _bufferSize + _position + 4;
            return BitConverter.ToInt32(arr, 0);
        }
        else
        {
            ReadArray(arr, 0, 4, _position);
            _position = 4 + _position;
            return BitConverter.ToInt32(arr, 0);
        }
    }
    public void Read<T>(out T[] values) where T : struct
    {
        ReadPosition();
        var len = ReadSize();
        ReadArray(buffer, 0, len, _position);
        values = MemoryMarshal.Cast<byte, T>(buffer).ToArray();
    }
}

class Client : Component
{
    //申请共享内存
    // SpmcRingBuffer buffer;
    MemoryMappedFile mmf;
    int head = 0;
    float[] data = new float[1024];
    public override void Start()
    {
        mmf = MemoryMappedFile.CreateFromFile("/dev/shm/test", System.IO.FileMode.OpenOrCreate, null, 100000);
    }
    public override void Update()
    {
        // buffer.Read(out data);
        using var acc = mmf.CreateViewAccessor();
        long data_ = 0;
        acc.Read(0, out data_);
        Console.WriteLine("{0:f4}", ((long)DateTime.Now.Ticks - (long)data_) / 1E7 * 1e3);
    }
}