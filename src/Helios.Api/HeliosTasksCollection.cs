using System.Collections.Concurrent;
using Helios.Modbus;

namespace Helios.Api;

public class HeliosTasksCollection : BlockingCollection<Func<HeliosClient, Task>>
{
    public HeliosTasksCollection() 
        : base(new ConcurrentQueue<Func<HeliosClient, Task>>())
    {
    }

    public HeliosTasksCollection(int maxSize) 
        : base(new ConcurrentQueue<Func<HeliosClient, Task>>(), maxSize)
    {
    }
}