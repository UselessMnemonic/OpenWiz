using OpenWiz;
using System;
using System.Text;

public class TestWizDiscovery {
    public static void Main(string[] args)
    {
        WizDiscoveryService wds = new WizDiscoveryService(390198, "192.128.0.100", "F0::18:98:09:1A:D8");
        wds.Start();
        while (true) {}
    }
}