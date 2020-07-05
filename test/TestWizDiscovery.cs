using OpenWiz;
using System;
using System.Threading;
using System.Text;
using System.Collections.Concurrent;

public class TestWizDiscovery : IWizDiscoveryListener {

    private ConcurrentDictionary<string, WizHandle> handles;

    public TestWizDiscovery()
    {
        handles = new ConcurrentDictionary<string, WizHandle>();
    }

    public void OnDiscover(WizHandle discovered)
    {
        if (!handles.ContainsKey(discovered.Mac))
        {
            Console.WriteLine($"[INFO] TestWizDiscovery::OnDiscover found light {discovered.Mac}");
            handles.TryAdd(discovered.Mac, discovered);
        }
    }

    public static void Main(string[] args)
    {
        TestWizDiscovery twd = new TestWizDiscovery();
        WizDiscoveryService wds = new WizDiscoveryService(390198, "192.168.0.100", new byte[]{ 0xF0, 0x18, 0x98, 0x09, 0x1A, 0xD8});

        wds.Start(twd);
        
        while (true) {}
    }
}