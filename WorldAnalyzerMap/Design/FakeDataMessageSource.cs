using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldAnalyzerMap.Domain;
using WorldAnalyzerMap.Domain.Model;

namespace WorldAnalyzerMap.Design
{
    public class FakeDataMessageSource : DataMessageRepository
    {
        private List<IObserver<DataMessageRepository>> observers = new List<IObserver<DataMessageRepository>>();

        public override IEnumerable<DataMessage> GetDataMessages()
        {
            return new DataMessage[] {
                new DataMessage() { Message = "hello", },
                new DataMessage() { Message = "from", },
                new DataMessage() { Message = "Mars", },
            };
        }

        public override IDisposable Subscribe(IObserver<DataMessageRepository> observer)
        {
            if (!this.observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new DataMessageRepository.Unsubscriber(this.observers, observer);
        }

        public override void StartListening()
        {
            OnMessage(null, new EventArgs());
        }

        public override void StopListening() { }

        public override void OnMessage(object sender, EventArgs e)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(this);
            }
        }
    }
}
