using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AddToHOST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string morningBL = @"C:\Windows\System32\drivers\etc\daytime_blocklist.txt";
        string permanentBL = @"C:\Windows\System32\drivers\etc\permanent_blocklist.txt.txt";
        string HostBL = @"C:\Windows\System32\drivers\etc\hosts";

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("hello");
        }

        private void AddToCurrent(object sender, RoutedEventArgs e)
        {
            if (siteTB == null) return;
            using StreamWriter writer = File.AppendText(HostBL);
            writer.WriteLine(FormatForHosts());
            siteTB.Text = string.Empty;
        }

        private void AddToMorning(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("A2M is clicked");
            if (siteTB == null) return;
            using StreamWriter writer = File.AppendText(morningBL);
            writer.WriteLine(FormatForHosts());
            siteTB.Text = string.Empty;
        }

        private void AddToPermanent(object sender, RoutedEventArgs e)
        {
            if (siteTB == null) return;
            using (var pbl = new StreamWriter(permanentBL, true))
            using (var mbl = new StreamWriter(morningBL, true))
            using (var hbl = new StreamWriter(HostBL, true))
            {
                string text2add = FormatForHosts();
                pbl.WriteLine(text2add);
                mbl.WriteLine(text2add);
                hbl.WriteLine(text2add);
            }
            
            siteTB.Text = string.Empty;
        }

        private string FormatForHosts () 
        {

            string inputText = siteTB.Text ;
            string outputText = string.Empty;

            string[] urls2Add = inputText.Split("\r\n");
            foreach (string url in urls2Add)
            {
                
                bool isValid = Regex.IsMatch(url, @"^[a-zA-Z0-9_./]+$");
                if (isValid == true)
                    outputText += $"0.0.0.0    {url}\n\n";
                else
                    Debug.WriteLine("This is invalid: " + url);

            }
              string cleanedOutput = outputText.TrimEnd();
            return cleanedOutput;
        }

        private void GetStrict(object sender, RoutedEventArgs e)
        {
            string morning = File.ReadAllText(morningBL);
            File.WriteAllText(HostBL, morning);
        }
    }
    }

