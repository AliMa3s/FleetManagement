using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Interfaces;
using FleetManagement.Exceptions;
public class Voertuig : IVoertuig , Bestuurder
{
    int VoertuigId { get; }
    string ChassisNummer { get; private set; }
    string NummerPlaat { get; private set; }
    Kleur? kleur { get; set; }
    BrandstofType Brandstof { get; set; }
    DateTime InBoekDatum { get; set; }
    int? AantalDeuren { get; set; } //int?=> nullable waarde



    public Voertuig()
    {

    }
    public Voertuig(int voertuigId,string chassisnummer,string nummerplaat,Kleur kleur , BrandstofType brandstof,DateTime inboekdatum,int aantaldeuren)
    {
        this.VoertuigId = voertuigId;
        this.ChassisNummer = chassisnummer;
        this.NummerPlaat = nummerplaat;
        this.kleur = kleur;
        this.Brandstof = brandstof;
        this.InBoekDatum = inboekdatum;
        this.AantalDeuren = aantaldeuren;
    }

    public void GetVoertuigID(Voertuig voertuigID)
    {
        if (Voertuigid < 0) throw new VoertuigException("voertuig - VoertuigId niet  gevonden.");
        return voertuigID;
        
    }
    public void UpdateNummerplaat(string nummerplaat)
    {
        if (nummerplaat.Length < 0) throw new VoertuigException("voertuig - nummerplaat niet gevonden.");
        NummerPlaat = nummerplaat;
    }

    public void GetChassisNummer (string chassisnummer)
    {
       return ChassisNummer = chassisnummer;
    }

    public void GetBestuurder(Bestuurder bestuurder)
    {
        if (!bestuurder.BestuurderId) throw new VoertuigException("voertuig - geen bestuurder gevonden.");
    }
}
