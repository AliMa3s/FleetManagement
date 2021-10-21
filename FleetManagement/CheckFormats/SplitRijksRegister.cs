using System;

namespace FleetManagement.CheckFormats
{
    class SplitRijksRegister
    {
        public string Maand { get;}
        public string Dag { get; }
        public string ControleDatum { get; }
        public string Geslacht { get; }
        public long CheckGetal { get; set; }
        public string ControleSom { get; }

        public SplitRijksRegister(string rijksRegisterNummer)
        {
            Maand = rijksRegisterNummer.Substring(2, 2);
            Dag = rijksRegisterNummer.Substring(4, 2);
            ControleDatum = rijksRegisterNummer.Substring(0, 6);
            Geslacht = rijksRegisterNummer.Substring(6, 3);
            CheckGetal = (int)long.Parse(rijksRegisterNummer.Substring(0, 9));
            ControleSom = rijksRegisterNummer.Substring(9, 2);
        }
    }
}
