using System;
using System.Collections.Generic;
using System.Text;

namespace Loyalto
{
    public partial class Valuta
    {
        Valuta()
        {
            OnCreated();
        }
        public virtual int vid { get; set; }
        public virtual string vcode { get; set; }
        public virtual string vname { get; set; }
        public virtual decimal vprice { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }
}
