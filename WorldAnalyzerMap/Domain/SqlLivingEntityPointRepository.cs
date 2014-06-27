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
    public class SqlLivingEntityPointRepository : LivingEntityPointRepository
    {
        private List<IObserver<LivingEntityPointRepository>> observers = new List<IObserver<LivingEntityPointRepository>>();
        private DataTable dt = new DataTable();
        private string connectionString;

        public SqlLivingEntityPointRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        private void RefreshDependency(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SELECT [id], [entity_type], [x], [y], [z] FROM [dbo].[test__]", connection))
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

        public override IEnumerable<LivingEntityPoint> GetDataMessages()
        {
            var query =
                from r in dt.Rows.Cast<DataRow>()
                select new LivingEntityPoint()
                {
                    EntityType = r.Field<string>("entity_type"),
                    X = r.Field<double?>("x"),
                    Y = r.Field<double?>("y"),
                    Z = r.Field<double?>("z"),
                };

            return query;
        }

        public override IDisposable Subscribe(IObserver<LivingEntityPointRepository> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new LivingEntityPointRepository.Unsubscriber(this.observers, observer);
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
