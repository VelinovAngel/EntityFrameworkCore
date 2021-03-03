using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        //	PatientId
        //	FirstName(up to 50 characters, unicode)
        //	LastName(up to 50 characters, unicode)
        //	Address(up to 250 characters, unicode)
        //	Email(up to 80 characters, not unicode)
        //	HasInsurance
        public Patient()
        {
            Visitations = new HashSet<Visitation>();
            Diagnoses = new HashSet<Diagnose>();
            Prescriptions = new HashSet<PatientMedicament>();
        }


        public int PatientId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(80)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
