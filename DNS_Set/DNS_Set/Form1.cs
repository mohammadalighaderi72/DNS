using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;
namespace DNS_Set
{
    public partial class FrmDNS : Form
    {
        public FrmDNS()
        {


            InitializeComponent();
        }

        private void FrmDNS_Load(object sender, EventArgs e)
        {



            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);


            if (!isAdmin)

            {
                MessageBox.Show("برنامه باید با سطح دسترسی ادمین اجرا شود.", "خطا در اجرای برنامه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();


            foreach (NetworkInterface adapter in adapters)
            {
                comboBox1.Items.Add(adapter.Name);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // دریافت نام آداپتر انتخاب شده
                string selectedAdapterName = comboBox1.SelectedItem.ToString();

                // پیدا کردن آداپتر انتخاب شده
                NetworkInterface adapter = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(a => a.Name == selectedAdapterName);

                if (adapter != null)
                {
                    // دریافت آدرس های IP و DNS آداپتر
                    IPInterfaceProperties ipProperties = adapter.GetIPProperties();
                    IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;

                    // نمایش آدرس DNS در Label
                    label1.Text = dnsAddresses.FirstOrDefault().ToString();
                }
                else
                {
                    label1.Text = "آداپتر انتخاب شده یافت نشد.";
                }


            }
            catch
            {
                MessageBox.Show("خطا در انجام عملیات مجدد تلاش کنید", "عملیات ناموفق", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedAdapterName = comboBox1.SelectedItem.ToString();
                
                
                string filetext =     "netsh interface ipv4 set dnsservers name=\""+ selectedAdapterName + "\" source=dhcp \n " + "netsh interface ipv4 add dnsserver \"" + selectedAdapterName + "\" " + textBox1.Text + "\n" +"netsh interface ipv4 add dnsserver \"" + selectedAdapterName + "\" " + textBox2.Text;
                File.WriteAllText("network.bat", filetext);

                // دریافت مسیر اجرای برنامه
                string currentDirectory = Directory.GetCurrentDirectory();

                // تشکیل مسیر کامل فایل bat
                string batchFilePath = Path.Combine(currentDirectory, "network.bat");

                // اجرای فایل bat با استفاده از کد قبلی
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c " + batchFilePath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
                File.Delete("network.bat");
                MessageBox.Show("آدرس DNS با موفقیت تغییر یافت.", "عملیات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("خطا در انجام عملیات مجدد تلاش کنید", "عملیات ناموفق", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedAdapterName = comboBox1.SelectedItem.ToString();


                string filetext = "netsh interface ipv4 set dnsservers name=\"" + selectedAdapterName + "\" source=dhcp \n " + "ipconfig /renew";  
                File.WriteAllText("network.bat", filetext);

                // دریافت مسیر اجرای برنامه
                string currentDirectory = Directory.GetCurrentDirectory();

                // تشکیل مسیر کامل فایل bat
                string batchFilePath = Path.Combine(currentDirectory, "network.bat");

                // اجرای فایل bat با استفاده از کد قبلی
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c " + batchFilePath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
                File.Delete("network.bat");
                MessageBox.Show("آدرس DNS با موفقیت تغییر یافت.", "عملیات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("خطا در انجام عملیات مجدد تلاش کنید", "عملیات ناموفق", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
    
}
