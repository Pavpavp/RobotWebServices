using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.SubscriptionServices
{

    public interface IEventArgs<T>
    {
        T ValueChanged { get; set; }
        void SetValueChanged(string s);
    }

    public class IOEventArgs : EventArgs, IEventArgs<int>
    {
        public int ValueChanged { get; set; }

        public void SetValueChanged(string s)
        {
            string valueChanged = s?.Split(new string[] { "lvalue\">" }, StringSplitOptions.None)[1].Split('<')[0].Trim();

            ValueChanged = int.Parse(valueChanged, CultureInfo.InvariantCulture);
        }

    }



    public class BackupEventArgs : EventArgs, IEventArgs<BackupState>
    {
        public BackupState ValueChanged { get; set; }

        public void SetValueChanged(string s)
        {
            string valueChanged = s?.Split(new string[] { "lvalue\">" }, StringSplitOptions.None)[1].Split('<')[0].Trim();

            // ValueChanged = int.Parse(valueChanged, CultureInfo.InvariantCulture);
        }

    }

    public enum BackupState
    {
        INIT,
        PROGRESS,
        COMPLETE
    }
}
