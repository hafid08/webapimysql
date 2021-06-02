using System;
using System.Collections.Generic;
using System.Text;

namespace Loyalto
{
    public partial class Toqen
    {
        //Toqen()
        //{
        //    OnCreated();
        //}
        public int pid { get; set; }
        public string token { get; set; }
        public DateTime ttime { get; set; }
        public DateTime texpired { get; set; }

        //#region Extensibility Method Definitions

        //partial void OnCreated();

        //#endregion
    }
}
