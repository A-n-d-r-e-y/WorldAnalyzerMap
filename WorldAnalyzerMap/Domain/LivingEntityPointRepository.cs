using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldAnalyzerMap.Domain.Model;

namespace WorldAnalyzerMap.Domain
{
    public abstract class LivingEntityPointRepository : IObservable<LivingEntityPointRepository>, IDisposable
    {
        public abstract IEnumerable<LivingEntityPoint> GetDataMessages();

        public abstract IDisposable Subscribe(IObserver<LivingEntityPointRepository> observer);

        public abstract void StartListening();

        public abstract void StopListening();

        public abstract void OnMessage(object sender, EventArgs e);

        public class Unsubscriber : IDisposable
        {
            private List<IObserver<LivingEntityPointRepository>> _observers;
            private IObserver<LivingEntityPointRepository> _observer;

            public Unsubscriber(List<IObserver<LivingEntityPointRepository>> observers, IObserver<LivingEntityPointRepository> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (this._observer != null
                    && this._observers.Contains(this._observer))
                {
                    this._observers.Remove(_observer);
                }
            }
        }

        public virtual void Dispose()
        {
            StopListening();
        }
    }
}
