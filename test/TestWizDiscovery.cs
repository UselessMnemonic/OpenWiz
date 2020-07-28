using OpenWiz;
using System;
using System.Collections.Concurrent;
using System.Net;

public class TestWizDiscovery {

    private ConcurrentDictionary<string, WizHandle> handles;
    private WizSocket socket;

    public TestWizDiscovery()
    {
        handles = new ConcurrentDictionary<string, WizHandle>();
        socket = new WizSocket();
        
        socket.Bind();
    }

    public void OnDiscover(WizHandle discovered)
    {
        if (!handles.ContainsKey(discovered.Mac))
        {
            Console.WriteLine($"[INFO] TestWizDiscovery::OnDiscover found light {discovered.Mac}");
            handles.TryAdd(discovered.Mac, discovered);
            socket.BeginRecieveFrom(discovered, new AsyncCallback(UpdateCallback), discovered);
        }
    }

    private void UpdateCallback(IAsyncResult result)
    {
        WizState state = socket.EndReceiveFrom(result.AsyncState as WizHandle, result);
        Console.WriteLine($"[INFO] TestWizDiscovery::UpdateCallback:\n\t{state}");
        socket.SendToAsync(WizState.MakeGetUserConfig(), result.AsyncState as WizHandle);
        socket.BeginRecieveFrom(result.AsyncState as WizHandle, new AsyncCallback(UpdateCallback), result.AsyncState);
    }

    public static void Main(string[] args)
    {
        TestWizDiscovery twd = new TestWizDiscovery();
        WizDiscoveryService wds = new WizDiscoveryService(390198, "192.168.0.100", new byte[]{ 0xF0, 0x18, 0x98, 0x09, 0x1A, 0xD8});
        twd.OnDiscover(new WizHandle("a8bb5088cfc2", IPAddress.Parse("192.168.0.154")));
        wds.Start(twd.OnDiscover);
        while (true) {}
    }
}