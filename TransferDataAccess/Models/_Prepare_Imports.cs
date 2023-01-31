using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    [Table("_Prepare_Imports")]
    public partial class _Prepare_Imports
    {
        [Key]
        public int RowIndex { get; set; }

        public Guid Import_Index { get; set; }

        public DateTime Import_Date { get; set; }
        public DateTime? Run_Date { get; set; }

        [Required]
        [StringLength(4000)]
        public string Import_Type { get; set; }

        [StringLength(4000)]
        public string Import_Message { get; set; }

        [StringLength(4000)]
        public string Import_File_Name { get; set; }

        public int Import_Status { get; set; }

        [Required]
        [StringLength(4000)]
        public string Import_By { get; set; }

        [StringLength(4000)]
        public string Import_Case { get; set; }

        public bool? IsHeader { get; set; }

        public int? Seq { get; set; }

        [StringLength(4000)]
        public string C0 { get; set; }

        [StringLength(4000)]
        public string C1 { get; set; }

        [StringLength(4000)]
        public string C2 { get; set; }

        [StringLength(4000)]
        public string C3 { get; set; }

        [StringLength(4000)]
        public string C4 { get; set; }

        [StringLength(4000)]
        public string C5 { get; set; }

        [StringLength(4000)]
        public string C6 { get; set; }

        [StringLength(4000)]
        public string C7 { get; set; }

        [StringLength(4000)]
        public string C8 { get; set; }

        [StringLength(4000)]
        public string C9 { get; set; }

        [StringLength(4000)]
        public string C10 { get; set; }

        [StringLength(4000)]
        public string C11 { get; set; }

        [StringLength(4000)]
        public string C12 { get; set; }

        [StringLength(4000)]
        public string C13 { get; set; }

        [StringLength(4000)]
        public string C14 { get; set; }

        [StringLength(4000)]
        public string C15 { get; set; }

        [StringLength(4000)]
        public string C16 { get; set; }

        [StringLength(4000)]
        public string C17 { get; set; }

        [StringLength(4000)]
        public string C18 { get; set; }

        [StringLength(4000)]
        public string C19 { get; set; }

        [StringLength(4000)]
        public string C20 { get; set; }

        [StringLength(4000)]
        public string C21 { get; set; }

        [StringLength(4000)]
        public string C22 { get; set; }

        [StringLength(4000)]
        public string C23 { get; set; }

        [StringLength(4000)]
        public string C24 { get; set; }

        [StringLength(4000)]
        public string C25 { get; set; }

        [StringLength(4000)]
        public string C26 { get; set; }

        [StringLength(4000)]
        public string C27 { get; set; }

        [StringLength(4000)]
        public string C28 { get; set; }

        [StringLength(4000)]
        public string C29 { get; set; }

        [StringLength(4000)]
        public string C30 { get; set; }

        [StringLength(4000)]
        public string C31 { get; set; }

        [StringLength(4000)]
        public string C32 { get; set; }

        [StringLength(4000)]
        public string C33 { get; set; }

        [StringLength(4000)]
        public string C34 { get; set; }

        [StringLength(4000)]
        public string C35 { get; set; }

        [StringLength(4000)]
        public string C36 { get; set; }

        [StringLength(4000)]
        public string C37 { get; set; }

        [StringLength(4000)]
        public string C38 { get; set; }

        [StringLength(4000)]
        public string C39 { get; set; }

        [StringLength(4000)]
        public string C40 { get; set; }

        [StringLength(4000)]
        public string C41 { get; set; }

        [StringLength(4000)]
        public string C42 { get; set; }

        [StringLength(4000)]
        public string C43 { get; set; }

        [StringLength(4000)]
        public string C44 { get; set; }

        [StringLength(4000)]
        public string C45 { get; set; }

        [StringLength(4000)]
        public string C46 { get; set; }

        [StringLength(4000)]
        public string C47 { get; set; }

        [StringLength(4000)]
        public string C48 { get; set; }

        [StringLength(4000)]
        public string C49 { get; set; }

        [StringLength(4000)]
        public string C50 { get; set; }

        [StringLength(4000)]
        public string C51 { get; set; }

        [StringLength(4000)]
        public string C52 { get; set; }

        [StringLength(4000)]
        public string C53 { get; set; }

        [StringLength(4000)]
        public string C54 { get; set; }

        [StringLength(4000)]
        public string C55 { get; set; }

        [StringLength(4000)]
        public string C56 { get; set; }

        [StringLength(4000)]
        public string C57 { get; set; }

        [StringLength(4000)]
        public string C58 { get; set; }

        [StringLength(4000)]
        public string C59 { get; set; }

        [StringLength(4000)]
        public string C60 { get; set; }

        [StringLength(4000)]
        public string C61 { get; set; }

        [StringLength(4000)]
        public string C62 { get; set; }

        [StringLength(4000)]
        public string C63 { get; set; }

        [StringLength(4000)]
        public string C64 { get; set; }

        [StringLength(4000)]
        public string C65 { get; set; }

        [StringLength(4000)]
        public string C66 { get; set; }

        [StringLength(4000)]
        public string C67 { get; set; }

        [StringLength(4000)]
        public string C68 { get; set; }

        [StringLength(4000)]
        public string C69 { get; set; }

        [StringLength(4000)]
        public string C70 { get; set; }

        [StringLength(4000)]
        public string C71 { get; set; }

        [StringLength(4000)]
        public string C72 { get; set; }

        [StringLength(4000)]
        public string C73 { get; set; }

        [StringLength(4000)]
        public string C74 { get; set; }

        [StringLength(4000)]
        public string C75 { get; set; }

        [StringLength(4000)]
        public string C76 { get; set; }

        [StringLength(4000)]
        public string C77 { get; set; }

        [StringLength(4000)]
        public string C78 { get; set; }

        [StringLength(4000)]
        public string C79 { get; set; }

        [StringLength(4000)]
        public string C80 { get; set; }

        [StringLength(4000)]
        public string C81 { get; set; }

        [StringLength(4000)]
        public string C82 { get; set; }

        [StringLength(4000)]
        public string C83 { get; set; }

        [StringLength(4000)]
        public string C84 { get; set; }

        [StringLength(4000)]
        public string C85 { get; set; }

        [StringLength(4000)]
        public string C86 { get; set; }

        [StringLength(4000)]
        public string C87 { get; set; }

        [StringLength(4000)]
        public string C88 { get; set; }

        [StringLength(4000)]
        public string C89 { get; set; }

        [StringLength(4000)]
        public string C90 { get; set; }

        [StringLength(4000)]
        public string C91 { get; set; }

        [StringLength(4000)]
        public string C92 { get; set; }

        [StringLength(4000)]
        public string C93 { get; set; }

        [StringLength(4000)]
        public string C94 { get; set; }

        [StringLength(4000)]
        public string C95 { get; set; }

        [StringLength(4000)]
        public string C96 { get; set; }

        [StringLength(4000)]
        public string C97 { get; set; }

        [StringLength(4000)]
        public string C98 { get; set; }

        [StringLength(4000)]
        public string C99 { get; set; }

        [StringLength(4000)]
        public string C100 { get; set; }

        [StringLength(4000)]
        public string C101 { get; set; }

        [StringLength(4000)]
        public string C102 { get; set; }

        [StringLength(4000)]
        public string C103 { get; set; }

        [StringLength(4000)]
        public string C104 { get; set; }

        [StringLength(4000)]
        public string C105 { get; set; }

        [StringLength(4000)]
        public string C106 { get; set; }

        [StringLength(4000)]
        public string C107 { get; set; }

        [StringLength(4000)]
        public string C108 { get; set; }

        [StringLength(4000)]
        public string C109 { get; set; }

        [StringLength(4000)]
        public string C110 { get; set; }

        [StringLength(4000)]
        public string C111 { get; set; }

        [StringLength(4000)]
        public string C112 { get; set; }

        [StringLength(4000)]
        public string C113 { get; set; }

        [StringLength(4000)]
        public string C114 { get; set; }

        [StringLength(4000)]
        public string C115 { get; set; }

        [StringLength(4000)]
        public string C116 { get; set; }

        [StringLength(4000)]
        public string C117 { get; set; }

        [StringLength(4000)]
        public string C118 { get; set; }

        [StringLength(4000)]
        public string C119 { get; set; }

        [StringLength(4000)]
        public string C120 { get; set; }

        [StringLength(4000)]
        public string C121 { get; set; }

        [StringLength(4000)]
        public string C122 { get; set; }

        [StringLength(4000)]
        public string C123 { get; set; }

        [StringLength(4000)]
        public string C124 { get; set; }

        [StringLength(4000)]
        public string C125 { get; set; }

        [StringLength(4000)]
        public string C126 { get; set; }

        [StringLength(4000)]
        public string C127 { get; set; }

        [StringLength(4000)]
        public string C128 { get; set; }

        [StringLength(4000)]
        public string C129 { get; set; }

        [StringLength(4000)]
        public string C130 { get; set; }

        [StringLength(4000)]
        public string C131 { get; set; }

        [StringLength(4000)]
        public string C132 { get; set; }

        [StringLength(4000)]
        public string C133 { get; set; }

        [StringLength(4000)]
        public string C134 { get; set; }

        [StringLength(4000)]
        public string C135 { get; set; }

        [StringLength(4000)]
        public string C136 { get; set; }

        [StringLength(4000)]
        public string C137 { get; set; }

        [StringLength(4000)]
        public string C138 { get; set; }

        [StringLength(4000)]
        public string C139 { get; set; }

        [StringLength(4000)]
        public string C140 { get; set; }

        [StringLength(4000)]
        public string C141 { get; set; }

        [StringLength(4000)]
        public string C142 { get; set; }

        [StringLength(4000)]
        public string C143 { get; set; }

        [StringLength(4000)]
        public string C144 { get; set; }

        [StringLength(4000)]
        public string C145 { get; set; }

        [StringLength(4000)]
        public string C146 { get; set; }

        [StringLength(4000)]
        public string C147 { get; set; }

        [StringLength(4000)]
        public string C148 { get; set; }

        [StringLength(4000)]
        public string C149 { get; set; }

        [StringLength(4000)]
        public string C150 { get; set; }

        [StringLength(4000)]
        public string C151 { get; set; }

        [StringLength(4000)]
        public string C152 { get; set; }

        [StringLength(4000)]
        public string C153 { get; set; }

        [StringLength(4000)]
        public string C154 { get; set; }

        [StringLength(4000)]
        public string C155 { get; set; }

        [StringLength(4000)]
        public string C156 { get; set; }

        [StringLength(4000)]
        public string C157 { get; set; }

        [StringLength(4000)]
        public string C158 { get; set; }

        [StringLength(4000)]
        public string C159 { get; set; }

        [StringLength(4000)]
        public string C160 { get; set; }

        [StringLength(4000)]
        public string C161 { get; set; }

        [StringLength(4000)]
        public string C162 { get; set; }

        [StringLength(4000)]
        public string C163 { get; set; }

        [StringLength(4000)]
        public string C164 { get; set; }

        [StringLength(4000)]
        public string C165 { get; set; }

        [StringLength(4000)]
        public string C166 { get; set; }

        [StringLength(4000)]
        public string C167 { get; set; }

        [StringLength(4000)]
        public string C168 { get; set; }

        [StringLength(4000)]
        public string C169 { get; set; }

        [StringLength(4000)]
        public string C170 { get; set; }

        [StringLength(4000)]
        public string C171 { get; set; }

        [StringLength(4000)]
        public string C172 { get; set; }

        [StringLength(4000)]
        public string C173 { get; set; }

        [StringLength(4000)]
        public string C174 { get; set; }

        [StringLength(4000)]
        public string C175 { get; set; }

        [StringLength(4000)]
        public string C176 { get; set; }

        [StringLength(4000)]
        public string C177 { get; set; }

        [StringLength(4000)]
        public string C178 { get; set; }

        [StringLength(4000)]
        public string C179 { get; set; }

        [StringLength(4000)]
        public string C180 { get; set; }

        [StringLength(4000)]
        public string C181 { get; set; }

        [StringLength(4000)]
        public string C182 { get; set; }

        [StringLength(4000)]
        public string C183 { get; set; }

        [StringLength(4000)]
        public string C184 { get; set; }

        [StringLength(4000)]
        public string C185 { get; set; }

        [StringLength(4000)]
        public string C186 { get; set; }

        [StringLength(4000)]
        public string C187 { get; set; }

        [StringLength(4000)]
        public string C188 { get; set; }

        [StringLength(4000)]
        public string C189 { get; set; }

        [StringLength(4000)]
        public string C190 { get; set; }

        [StringLength(4000)]
        public string C191 { get; set; }

        [StringLength(4000)]
        public string C192 { get; set; }

        [StringLength(4000)]
        public string C193 { get; set; }

        [StringLength(4000)]
        public string C194 { get; set; }

        [StringLength(4000)]
        public string C195 { get; set; }

        [StringLength(4000)]
        public string C196 { get; set; }

        [StringLength(4000)]
        public string C197 { get; set; }

        [StringLength(4000)]
        public string C198 { get; set; }

        [StringLength(4000)]
        public string C199 { get; set; }

        [StringLength(4000)]
        public string C200 { get; set; }
    }
}
