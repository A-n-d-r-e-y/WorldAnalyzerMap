using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldAnalyzerMap.Domain;
using WorldAnalyzerMap.Domain.Model;
using System.Windows.Threading;

namespace WorldAnalyzerMap.ViewModel
{
    public class ShellViewModel : IObserver<LivingEntityPointRepository>, INotifyPropertyChanged
    {
        LivingEntityPointRepository entityRepository;

        public List<LivingEntityPoint> DataMessages
        {
            get { return entityRepository.GetDataMessages().ToList(); }
        }

        public int MessagesCount
        {
            get { return entityRepository.GetDataMessages().Count(); }
        }

        public ShellViewModel(LivingEntityPointRepository entityRepository)
        {
            if (entityRepository == null) throw new ArgumentNullException("entityRepository");

            this.entityRepository = entityRepository;

            entityRepository.StartListening();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(LivingEntityPointRepository value)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                RaisePropertyChanged("MessagesCount");
            });        
        }
    }
}
