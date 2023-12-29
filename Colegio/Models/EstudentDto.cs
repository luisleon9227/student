﻿using Colegio.Enums;

namespace Colegio.Models
{
    public class EstudentDto
    {
        //[Key]
        public Guid Id { get; set; }

        //[Required]
        public TypeIdentificationEnum TypeIdentification { get; set; }

        //[Required]
        public int Identification { get; set; }

        //[Required]
        //[MaxLength(50)]
        public string Name { get; set; }

        //[Required]
        //[MaxLength(50)]
        public string Email { get; set; }

        public Guid GradeId { get; set; }

        //[Required]
        public virtual GradeDto Grade { get; set; }
    }
}
