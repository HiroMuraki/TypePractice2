using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TypePractice2.Model {
    [DataContract]
    public struct WordModel {
        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string Meaning { get; set; }
    }
}
