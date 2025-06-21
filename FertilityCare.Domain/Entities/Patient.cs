using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class Patient
{
    public Guid Id { get; set; }

    public Guid UserProfileId { get; set; }

    public virtual UserProfile UserProfile { get; set; }

    public string? MedicalHistory { get; set; }
        
    public string? PartnerFullName { get; set; }    

    public string? PartnerEmail { get; set; }   

    public string? PartnerPhone { get; set; }
}
    