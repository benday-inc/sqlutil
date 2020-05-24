using Benday.Presentation;
using System;

namespace Benday.Presentation
{
    public abstract class MessageBoxRequestViewModelBase : GalaSoft.MvvmLight.ViewModelBase, IViewModelBase
    {

        public MessageBoxRequestViewModelBase()
        {

        }

        public event EventHandler RedrawRequested;

        public event MessageBoxEventHandler MessageBoxRequested;

        public virtual void RequestMessageBox(string message)
        {
            if (MessageBoxRequested != null)
            {
                if (MessageBoxRequested != null) MessageBoxRequested(
                    this, new MessageBoxEventArgs(message));
            }
        }

        public virtual void RequestMessageBox(string message, bool isUnexpectedException, Exception ex)
        {
            if (MessageBoxRequested != null)
            {
                if (MessageBoxRequested != null) MessageBoxRequested(
                    this, new MessageBoxEventArgs(message, isUnexpectedException, ex));
            }
        }

        protected virtual void RequestMessageBox(string message, Exception ex)
        {
            if (MessageBoxRequested != null)
            {
                if (MessageBoxRequested != null) MessageBoxRequested(
                    this, new MessageBoxEventArgs(message, ex));
            }
        }

        protected void RequestRedraw()
        {
            if (RedrawRequested != null)
            {
                RedrawRequested(this, new EventArgs());
            }
        }
    }
}
