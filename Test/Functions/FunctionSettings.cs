using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Commen.Enum;

namespace Test.Functions
{
    public class FunctionSettings : INotifyPropertyChanged
    {
        private double _amplitude;
        private double _frequency;
        private double _phase;
        private double _step;
        private double _xMin;
        private double _xMax;
        private double _yMin;
        private double _yMax;
        private FunctionType _functionType;
        public event Action PropertyChangedCallback;

        public double Amplitude
        {
            get => _amplitude;
            set
            {                
                _amplitude = value;              
                OnPropertyChangedCallback();
                OnPropertyChanged(nameof(Amplitude));
            }
        }

        public double Frequency
        {
            get => _frequency;
            set
            {                
                _frequency = value;
                OnPropertyChanged(nameof(Frequency));              
            }
        }

        public double Phase
        {
            get => _phase;
            set
            {               
                _phase = value;
                OnPropertyChanged(nameof(Phase));                
            }
        }

        private double _shift;
        public double Shift
        {
            get => _shift;
            set
            {
                _shift = value;
                OnPropertyChanged(nameof(Shift));
            }
        }

        public double Step
        {
            get => _step;
            set
            {              
                _step = value;
                OnPropertyChanged(nameof(Step));               
            }
        }

        public double XMin
        {
            get => _xMin;
            set
            {              
                _xMin = value;
                OnPropertyChanged(nameof(XMin));              
            }
        }

        public double XMax
        {
            get => _xMax;
            set
            {               
                _xMax = value;
                OnPropertyChanged(nameof(XMax));              
            }
        }

        public double YMin
        {
            get => _yMin;
            set
            {              
                _yMin = value;
                OnPropertyChanged(nameof(YMin));               
            }
        }

        public double YMax
        {
            get => _yMax;
            set
            {             
                _yMax = value;
                OnPropertyChanged(nameof(YMax));               
            }
        }

        public FunctionType FunctionType
        {
            get => _functionType;
            set
            {
                if (_functionType != value)
                {
                    _functionType = value;
                    OnPropertyChanged(nameof(FunctionType));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChangedCallback()
        {
            PropertyChangedCallback?.Invoke();
        }
    }
}
