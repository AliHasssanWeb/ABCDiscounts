using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Route
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string InitialLocation { get; set; }
        public string DesignationLocation { get; set; }
    }
}
