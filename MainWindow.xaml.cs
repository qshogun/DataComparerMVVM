using Microsoft.PowerShell;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
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
using System.IO;
using System.Windows.Interop;
using System.Windows.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string Value = "blablabla";

        private InitialSessionState _initialSessionState;
        private Runspace _runspace;
        private PowerShell _powerShell;

        public MainWindow()
        {
            InitializeComponent();
            _initialSessionState = InitialSessionState.CreateDefault();
            _runspace = RunspaceFactory.CreateRunspace(_initialSessionState);
            _runspace.Open();
            _powerShell = PowerShell.Create(_runspace);
        }

        private static void SetExecutionPolicy(PowerShell powerShell, ExecutionPolicy policy, ExecutionPolicyScope scope)
        {
            powerShell.AddCommand("Set-ExecutionPolicy")
              .AddParameter("ExecutionPolicy", policy)
              .AddParameter("Scope", scope);
            powerShell.Invoke();
        }

        private void InitializePowerShellSetup()
        {
            _initialSessionState = InitialSessionState.CreateDefault();
            _runspace = RunspaceFactory.CreateRunspace(_initialSessionState);
            _runspace.Open();
            _powerShell = PowerShell.Create(_runspace);
            SetExecutionPolicy(_powerShell, ExecutionPolicy.Unrestricted, ExecutionPolicyScope.CurrentUser);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _powerShell.AddCommand(@"e:\test.ps1");
            _powerShell.Invoke();
            while (_powerShell.Streams.Error.Count > 0)
            {
                var error = _powerShell.Streams.Error.First();
                System.Windows.MessageBox.Show(error.Exception.Message);
                break;
            }
            if (_powerShell.Streams.Error.Count == 0)
            {
                System.Windows.MessageBox.Show("Success");
                System.Windows.MessageBox.Show(File.ReadAllLines(@"e:\test.txt").Length.ToString());
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _powerShell.AddCommand(@"e:\test.ps1").AddParameter("param1", "cosinnego");
            _powerShell.Invoke();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder...";
            folderDialog.UseDescriptionForTitle = true;
            folderDialog.InitialDirectory = "C:\\";

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFolder = folderDialog.SelectedPath;
                FolderPath.Text = selectedFolder;
            }
        }
    }
}