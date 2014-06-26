using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldAnalyzerMap.Domain.Model;
using System.Data;

namespace WorldAnalyzerMap.Domain
{
    public class SqlDataMessageSource : DataMessageRepository
    {
        private List<IObserver<DataMessageRepository>> observers = new List<IObserver<DataMessageRepository>>();
        private DataTable dt = new DataTable();
        private string connectionString;

        public SqlDataMessageSource(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        private void RefreshDependency(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SELECT id, msg FROM dbo.TableToTrank", connection))
                {
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(OnMessage);

                    connection.Open();

                    using (var da = new SqlDataAdapter(command))
                    {
                        dt.Clear();
                        da.Fill(dt);
                    }
                }
            }
        }

        public override IEnumerable<DataMessage> GetDataMessages()
        {
            //return new DataMessage[] {
            //    new DataMessage() { Message = "hello", },
            //    new DataMessage() { Message = "from", },
            //    new DataMessage() { Message = "Mars", },
            //};
            var query =
                from r in dt.Rows.Cast<DataRow>()
                select new DataMessage() { Message = r["msg"].ToString(), };

            return query;
        }

        public override IDisposable Subscribe(IObserver<DataMessageRepository> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new DataMessageRepository.Unsubscriber(this.observers, observer);
        }

        public override void StartListening()
        {
            SqlDependency.Start(connectionString);
            RefreshDependency(connectionString);
        }

        public override void StopListening()
        {
            SqlDependency.Stop(connectionString);
        }

        public override void OnMessage(object sender, EventArgs e)
        {

            if ((e as SqlNotificationEventArgs).Type == SqlNotificationType.Change)
            {
                RefreshDependency(connectionString);

                foreach (var observer in this.observers)
                {
                    observer.OnNext(this);             
                }
            }
        }
    }
}
