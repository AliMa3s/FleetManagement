﻿using FleetManagement.CheckFormats;
using FleetManagement.Exceptions;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Manager.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace FleetManagement.Bouwers
{
    public class VoertuigBouwer
    {
        private readonly IVoertuigManager _voertuigManager;

        public AutoModel AutoModel { get; set; }
        public string Chassisnummer { get; set;  }
        public string Nummerplaat { get; set; }
        public BrandstofVoertuig Brandstof { get; set; }
        public Kleur? VoertuigKleur { get; set; }
        public AantalDeuren? AantalDeuren { get; set; }
        public Bestuurder Bestuurder { get; set; }

        public VoertuigBouwer(IVoertuigManager voertuigManager)
        {
            _voertuigManager = voertuigManager;
        }

        public bool IsGeldig() 
        {
            return AutoModel != null
                && AutoModel.AutoModelId > 0
                && !string.IsNullOrWhiteSpace(Chassisnummer)
                && !string.IsNullOrWhiteSpace(Nummerplaat)
                && Bestuurder != null
                && Bestuurder.BestuurderId > 0
                && Brandstof != null
                && bestaatChassisOfNummerplaat();
        }

        private bool IsChassisNummerGeldig()
        {
            //if(_repo.BestaatChassisNummer(ChassisNummer)) //Nog IRepository implementatie aanmaken om chassis te checken
            //{
            //    return true;
            //}

            return false;
        }

        private bool IsNummerplaatGeldig()
        {
            //if (_repo.BestaatNummerplaat(NummerPlaat))  //Nog IRepository implementatie aanmaken om nummerplaat te checken
            //{
            //    return true;
            //}

            return false;
        }

        private bool bestaatChassisOfNummerplaat()
        {
            if (!_voertuigManager.bestaatChassisOfNummerplaat(Chassisnummer, Nummerplaat)) {
                return true;
            }
 
            return false;
        }

        public Voertuig BouwVoertuig()
        {
            if(!IsGeldig())
            {
                throw new VoertuigBouwerException("Voertuig kan niet worden gebouwd");
            }

            //check kleur & aantal deuren mss nog indien ingevuld (indien API ipv WPF Parsen van selectors)

            Voertuig voertuig = new(
                AutoModel,
                Chassisnummer,
                Nummerplaat,
                Brandstof
            ) { 
                VoertuigKleur = VoertuigKleur,
                AantalDeuren = AantalDeuren
            };

            voertuig.VoegBestuurderToe(Bestuurder);
            return voertuig;
        }

       public string Status()
        {
            StringBuilder message = new();
            if (AutoModel == null) { message.AppendLine("AutoModel mag niet leeg zijn"); }
            if (AutoModel.AutoModelId < 1) { message.AppendLine("AutoModel is niet gelecteerd uit de lijst"); }
            if (string.IsNullOrWhiteSpace(Chassisnummer)) { message.AppendLine("ChassisNummer moet ingevuld zijn"); }
            if (string.IsNullOrWhiteSpace(Nummerplaat)) { message.AppendLine("Nummerplaat moet ingevuld zijn"); }
            if (!CheckFormat.IsChassisNummerGeldig(Chassisnummer)) { message.AppendLine("Chassisnummer is niet het correcte formaat"); }
            if (!CheckFormat.IsNummerplaatGeldig(Nummerplaat)) { message.AppendLine("Nummerplaat is niet het correcte formaat"); }
            if (Bestuurder == null) { message.AppendLine("Bestuurder moet ingevuld zijn"); }
            if (Bestuurder.BestuurderId < 1) { message.AppendLine("Bestuurder moet geslecteerd zijn uit de lijst"); }
            if (Brandstof == null) { message.AppendLine("Brandstof mag niet leeg zijn"); }
            if (!IsChassisNummerGeldig()) { message.AppendLine("Chassisnummer bestaat al"); }
            if (!IsNummerplaatGeldig()) { message.AppendLine("Nummerplaat bestaat al"); }

            return message.ToString();
        }
    }
}
