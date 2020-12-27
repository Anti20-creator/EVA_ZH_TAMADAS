using System;
using System.Collections.Generic;
using System.Text;
using ZH.ViewModel;

namespace ZH
{
    public class Field
    {
        private String _text;
        private String _color;

        public String Color
        {
            get { return _color; }
            set
            {
                _color = value;
            }
        }
        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                }
            }
        }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Int32 Number { get; set; }
        public DelegateCommand StepCommand { get; set; }

    }
}
