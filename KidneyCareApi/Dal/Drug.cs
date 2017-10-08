namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Drugs")]
    public partial class Drug
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string DrugGroup { get; set; }

        [StringLength(255)]
        public string DrugGroupTwo { get; set; }

        [StringLength(255)]
        public string DrugGroupTwoLogogram { get; set; }

        [StringLength(255)]
        public string DrugName { get; set; }

        [StringLength(255)]
        public string DrugCode { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }
    }
}
