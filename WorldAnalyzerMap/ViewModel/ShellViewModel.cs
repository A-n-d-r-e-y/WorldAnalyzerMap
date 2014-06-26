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
    public class ShellViewModel : IObserver<DataMessageRepository>, INotifyPropertyChanged
    {
        DataMessageRepository messageRepository;

        public List<DataMessage> DataMessages
        {
            get { return messageRepository.GetDataMessages().ToList(); }
        }

        public int MessagesCount
        {
            get { return messageRepository.GetDataMessages().Count(); }
        }

        public ShellViewModel(DataMessageRepository messageRepository)
        {
            if (messageRepository == null) throw new ArgumentNullException("messageRepository");

            this.messageRepository = messageRepository;

            messageRepository.StartListening();
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

        public void OnNext(DataMessageRepository value)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                RaisePropertyChanged("MessagesCount");
            });        
        }
    }
}
