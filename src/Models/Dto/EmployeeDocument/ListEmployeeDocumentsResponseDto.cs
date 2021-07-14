using System;
using System.Collections.Generic;

namespace Jaxofy.Models.Dto.EmployeeDocument
{
    /// <summary>
    /// List of documents (their id + tuples) response DTO.
    /// </summary>
    public class ListEmployeeDocumentsResponseDto
    {
        /// <summary>
        /// The found documents, as a list of tuples containing the document ID + name.
        /// </summary>
        public List<Tuple<long, string>> DocumentIdNameTuples { get; set; }
    }
}