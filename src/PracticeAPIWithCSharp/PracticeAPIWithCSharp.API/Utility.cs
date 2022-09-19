using PracticeAPIWithCSharp.API.Models;
using System.Collections.Generic;
using System.Management;

namespace PracticeAPIWithCSharp.API
{
    public class Utility
    {
        public static List<HardDrive> GetHardDrives()
        {
            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            List<HardDrive> hdCollection = new List<HardDrive>();
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                var hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.InterfaceType = wmi_HD["InterfaceType"].ToString();
                hd.SerialNo = wmi_HD["SerialNumber"].ToString();
                hdCollection.Add(hd);
            }
            return hdCollection;
        }
        public static List<User> UsersRecords = new List<User>()
        {
            new User{ UserName="user1",Password="password1"},
            new User{ UserName="user2",Password="password2"},

            new User{ UserName="user3",Password="password3"}
        };
    }
    public class HardDrive
    {
        public string Model { get; set; }
        public string InterfaceType { get; set; }
        public string Caption { get; set; }
        public string SerialNo { get; set; }
    }
}
