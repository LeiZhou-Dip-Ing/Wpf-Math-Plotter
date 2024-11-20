using DryIoc;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Test.Commen.ViewModels
{
    public abstract class ValidationViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();
        public bool HasErrors =>_validationErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName != null && _validationErrors.ContainsKey(propertyName))
            {
                return _validationErrors[propertyName];
            }

            return null;
        }

        public abstract string ValidateProperty<T>(T value, string propertyName);

        // Raise the ErrorsChanged event
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            if (_validationErrors.ContainsKey(propertyName))
            {
                _validationErrors[propertyName].Add(errorMessage);
            }
            else
            {
                _validationErrors.Add(
                    propertyName,
                    new List<string>
                    {
                        errorMessage
                    });
            }

            OnErrorsChanged(propertyName);
        }

        protected void ClearErrors(string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName))
            {
                _validationErrors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ValidateProperty<T>(T oldValue, T newValue, string propertyName, bool forceValidation = false)
        {
            if (Equals(oldValue, newValue) && !forceValidation)
            {
                return;
            }

            ClearErrors(propertyName);
            var validationResult = ValidateProperty(newValue, propertyName);
            if (!string.IsNullOrEmpty(validationResult))
            {
                AddError(propertyName, validationResult);
            }
        }



    }
}
