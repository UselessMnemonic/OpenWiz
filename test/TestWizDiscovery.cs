using OpenWiz;
using System;
using System.Text;

public class TestWizDiscovery {
    public static void Main(string[] args)
    {
        WizDiscoveryService wds = new WizDiscoveryService(390198, "192.128.0.100", "F01898091AD8");
        wds.Start();
        while (true) {}
    }
}