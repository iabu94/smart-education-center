//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace smart_education_center.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GradeVsSubject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GradeVsSubject()
        {
            this.Test = new HashSet<Test>();
            this.Lesson = new HashSet<Lesson>();
        }
    
        public int Id { get; set; }
        public Nullable<int> GradeId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public string Description { get; set; }
    
        public virtual Grade Grade { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Test { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lesson> Lesson { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
