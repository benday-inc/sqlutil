using System;

namespace Benday.Presentation
{
    public abstract class MessagingViewModelBase : ViewModelBase
    {
        private IMessageManager _messageManager;

        public MessagingViewModelBase(IMessageManager messageManager)
        {
            _messageManager = messageManager ?? throw new ArgumentNullException(nameof(messageManager), $"{nameof(messageManager)} is null.");
        }

        public IMessageManager Messages { get => _messageManager; }
    }
}
