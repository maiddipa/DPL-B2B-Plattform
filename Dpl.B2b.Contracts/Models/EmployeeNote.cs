using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dpl.B2b.Contracts.Models
{
    public class EmployeeNote
    {
        public int Id { get; set; }
        public EmployeeNoteType Type { get; set; }
        public EmployeeNoteReason Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Contact { get; set; }
        public DateTime ContactedAt { get; set; }

        public string Text { get; set; }
    }

    public class EmployeeNoteCreateRequest
    {
        public EmployeeNoteType Type { get; set; }
        public EmployeeNoteReason Reason { get; set; }
        [JsonIgnore] // this field is not returned from web api
        public int UserId { get; set; }
        public string Contact { get; set; }
        public DateTime ContactedAt { get; set; }
        public string Text { get; set; }
    }
}
