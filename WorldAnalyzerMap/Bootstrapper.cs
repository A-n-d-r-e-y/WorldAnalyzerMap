using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldAnalyzerMap.ViewModel;
using WorldAnalyzerMap.View;
//using WorldAnalyzerMap.Design;
using WorldAnalyzerMap.Domain;
using System.Data.SqlClient;

namespace WorldAnalyzerMap
{
    public class Bootstrapper
    {
        public void Run()
        {
            // composition root of the application

            var sb = new SqlConnectionStringBuilder();
            sb.DataSource = "localhost";
            sb.InitialCatalog = "test";
            sb.IntegratedSecurity = true;

            var shell = new Shell();
            var ms = new SqlLivingEntityPointRepository(sb.ConnectionString);
            var vm = new ShellViewModel(ms);

            IDisposable disposer = ms.Subscribe(vm);

            shell.DataContext = vm;

            App.Current.MainWindow = shell;
            App.Current.MainWindow.Show();

            //App.Current.Deactivated += new EventHandler(new Action<object, EventArgs>((o, e) => { disposer.Dispose(); }));
        }
    }
}
