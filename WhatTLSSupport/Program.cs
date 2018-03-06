using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WhatTLSSupport
{
    class Program
    {
        static void Main(string[] args)
        {
            //Based on code from https://stackoverflow.com/a/43534406
            Console.WriteLine(".NET Runtime: " + System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(int).Assembly.Location).ProductVersion);
            Console.WriteLine("Enabled protocols:   " + ServicePointManager.SecurityProtocol);

            Console.WriteLine("Available protocols: ");
            Boolean platformSupportsTls12 = false;
            foreach (SecurityProtocolType protocol in Enum.GetValues(typeof(SecurityProtocolType)))
            {
                Console.WriteLine("{0} ({1})", protocol.ToString(), protocol.GetHashCode());
                if (protocol.GetHashCode() == 3072)
                {
                    platformSupportsTls12 = true;
                }
            }

            Console.WriteLine("Is Tls12 enabled: " + ServicePointManager.SecurityProtocol.HasFlag((SecurityProtocolType)3072));

            // enable Tls12, if possible
            if (!ServicePointManager.SecurityProtocol.HasFlag((SecurityProtocolType)3072)){
                if (platformSupportsTls12)
                {
                    Console.WriteLine("Platform supports Tls12, but it is not enabled.");
                    Console.WriteLine("Add the following registry keys:");
                    Console.WriteLine();
                    Console.WriteLine(@"[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319]");
                    Console.WriteLine("\"SchUseStrongCrypto\" = dword:00000001");
                    Console.WriteLine(@"[HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319]");
                    Console.WriteLine("\"SchUseStrongCrypto\" = dword:00000001");
                    Console.WriteLine();
                    Console.WriteLine("Or enable in your code:");
                    Console.WriteLine(@"System.Net.ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;");
                }
                else
                {
                    Console.WriteLine("Platform does not supports Tls12.");
                }
            }
        }
    }
}
