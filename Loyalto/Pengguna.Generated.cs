using System;
using System.Collections.Generic;
using System.Text;

namespace Loyalto
{
    public partial class Pengguna
    {
        Pengguna()
        {
            OnCreated();
        }
        public virtual int pid { get; set; }
        public virtual string puser { get; set; }
        public virtual string ppass { get; set; }
        public virtual int pstatus { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }
}
