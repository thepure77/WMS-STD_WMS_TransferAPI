using System.Collections.Generic;

using Business.Models;
using TransferDataAccess.Models;

namespace InterfaceWMSBusiness.TranferExtensions
{
    public static class TranferExtensions
    {
        public static dynamic ReturnModel(this _Prepare_Imports model, List<Invalid_ImportModel.ColumnDetailModel> errors)
        {
            return new { Error = (errors ?? new List<Invalid_ImportModel.ColumnDetailModel>())
                        , model.C0,  model.C1, model.C2, model.C3, model.C4, model.C5, model.C6, model.C7, model.C8, model.C9
                        , model.C10, model.C11, model.C12, model.C13, model.C14, model.C15, model.C16, model.C17,model.C18,model.C19
                        , model.C20, model.C21, model.C22, model.C23, model.C24, model.C25, model.C26
            };
        }

        public static string pallet(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C0" : model.C0;
        }

        public static string location_new(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C1" : model.C1;
        }

        public static string Location_old(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C2" : model.C2;
        }

        public static string SKU(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C3" : model.C3;
        }

        public static string SKU_name(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C4" : model.C4;
        }

        public static string Qty(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C5" : model.C5;
        }

        public static string Unit(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C6" : model.C6;
        }

        public static string Lot(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C7" : model.C7;
        }

        public static string Sloc(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C8" : model.C8;
        }

        public static string ItemStatus(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C9" : model.C9;
        }

        public static string MFG_date(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C10" : model.C10;
        }

        public static string EXP_date(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C11" : model.C11;
        }

        public static string GR_No(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C12" : model.C12;
        }

        public static string Owner_Tel(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C13" : model.C13;
        }

        public static string Owner_Mobile(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C14" : model.C14;
        }
        public static string Owner_Barcode(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C15" : model.C15;
        }
        public static string Contact_Person(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C16" : model.C16;
        }
        public static string Contact_Person2(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C17" : model.C17;
        }
        public static string Contact_Person3(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C18" : model.C18;
        }
        public static string Contact_Tel(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C19" : model.C19;
        }
        public static string Contact_Tel2(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C20" : model.C20;
        }
        public static string Contact_Tel3(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C21" : model.C21;
        }
        public static string Contact_Email(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C22" : model.C22;
        }
        public static string Contact_Email2(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C23" : model.C23;
        }
        public static string Contact_Email3(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C24" : model.C24;
        }
        public static string Ref_No2(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C25" : model.C25;
        }
        public static string Status(this _Prepare_Imports model, bool getName = false)
        {
            return getName ? "C26" : model.C26;
        }

        public static string OwnerType_Index(this _Prepare_Imports model)
        {
            return model.C100;
        }

        public static void OwnerType_Index(this _Prepare_Imports model, string value)
        {
            model.C100 = value;
        }

        public static string PlanGoodsIssueItem_Index(this _Prepare_Imports model)
        {
            return model.C101;
        }

        public static void PlanGoodsIssueItem_Index(this _Prepare_Imports model, string value)
        {
            model.C101 = value;
        }

        public static string Owner_Index(this _Prepare_Imports model)
        {
            return model.C102;
        }

        public static void Owner_Index(this _Prepare_Imports model, string value)
        {
            model.C102 = value;
        }

        public static string WareHouse_Index(this _Prepare_Imports model)
        {
            return model.C103;
        }

        public static void WareHouse_Index(this _Prepare_Imports model, string value)
        {
            model.C103 = value;
        }

        public static string DocumentType_Index(this _Prepare_Imports model)
        {
            return model.C104;
        }

        public static void DocumentType_Index(this _Prepare_Imports model, string value)
        {
            model.C104 = value;
        }

        public static string ShipTo_Index(this _Prepare_Imports model)
        {
            return model.C105;
        }

        public static void ShipTo_Index(this _Prepare_Imports model, string value)
        {
            model.C105 = value;
        }

        public static string SoldTo_Index(this _Prepare_Imports model)
        {
            return model.C106;
        }

        public static void SoldTo_Index(this _Prepare_Imports model, string value)
        {
            model.C106 = value;
        }

        public static string Product_Index(this _Prepare_Imports model)
        {
            return model.C107;
        }

        public static void Product_Index(this _Prepare_Imports model, string value)
        {
            model.C107 = value;
        }

        public static string ProductConversion_Index(this _Prepare_Imports model)
        {
            return model.C108;
        }

        public static void ProductConversion_Index(this _Prepare_Imports model, string value)
        {
            model.C108 = value;
        }

        public static string Ratio(this _Prepare_Imports model)
        {
            return model.C109;
        }

        public static void Ratio(this _Prepare_Imports model, string value)
        {
            model.C109 = value;
        }

        public static string ProductOwner_Index(this _Prepare_Imports model)
        {
            return model.C110;
        }

        public static void ProductOwner_Index(this _Prepare_Imports model, string value)
        {
            model.C110 = value;
        }

        public static string Master_Owner_Id(this _Prepare_Imports model)
        {
            return model.C112;
        }

        public static void Master_Owner_Id(this _Prepare_Imports model, string value)
        {
            model.C112 = value;
        }

        public static string Master_Owner_Name(this _Prepare_Imports model)
        {
            return model.C113;
        }

        public static void Master_Owner_Name(this _Prepare_Imports model, string value)
        {
            model.C113 = value;
        }

        public static string Master_DocumentType_Id(this _Prepare_Imports model)
        {
            return model.C114;
        }

        public static void Master_DocumentType_Id(this _Prepare_Imports model, string value)
        {
            model.C114 = value;
        }

        public static string Master_DocumentType_Name(this _Prepare_Imports model)
        {
            return model.C115;
        }

        public static void Master_DocumentType_Name(this _Prepare_Imports model, string value)
        {
            model.C115 = value;
        }

        public static string Master_WareHouse_Id(this _Prepare_Imports model)
        {
            return model.C116;
        }

        public static void Master_WareHouse_Id(this _Prepare_Imports model, string value)
        {
            model.C116 = value;
        }

        public static string Master_WareHouse_Name(this _Prepare_Imports model)
        {
            return model.C117;
        }

        public static void Master_WareHouse_Name(this _Prepare_Imports model, string value)
        {
            model.C117 = value;
        }

        public static string Master_ShipTo_Id(this _Prepare_Imports model)
        {
            return model.C118;
        }

        public static void Master_ShipTo_Id(this _Prepare_Imports model, string value)
        {
            model.C118 = value;
        }

        public static string Master_ShipTo_Name(this _Prepare_Imports model)
        {
            return model.C119;
        }

        public static void Master_ShipTo_Name(this _Prepare_Imports model, string value)
        {
            model.C119 = value;
        }

        public static string Master_ShipTo_Address(this _Prepare_Imports model)
        {
            return model.C120;
        }

        public static void Master_ShipTo_Address(this _Prepare_Imports model, string value)
        {
            model.C120 = value;
        }

        public static string Master_ShipTo_Contact_Person(this _Prepare_Imports model)
        {
            return model.C121;
        }

        public static void Master_ShipTo_Contact_Person(this _Prepare_Imports model, string value)
        {
            model.C121 = value;
        }

        public static string Master_SoldTo_Id(this _Prepare_Imports model)
        {
            return model.C122;
        }

        public static void Master_SoldTo_Id(this _Prepare_Imports model, string value)
        {
            model.C122 = value;
        }

        public static string Master_SoldTo_Name(this _Prepare_Imports model)
        {
            return model.C123;
        }

        public static void Master_SoldTo_Name(this _Prepare_Imports model, string value)
        {
            model.C123 = value;
        }

        public static string Master_SoldTo_Address(this _Prepare_Imports model)
        {
            return model.C124;
        }

        public static void Master_SoldTo_Address(this _Prepare_Imports model, string value)
        {
            model.C124 = value;
        }

        public static string Master_SoldTo_Contact_Person(this _Prepare_Imports model)
        {
            return model.C125;
        }

        public static void Master_SoldTo_Contact_Person(this _Prepare_Imports model, string value)
        {
            model.C125 = value;
        }

        public static string Master_ItemStatus_Index(this _Prepare_Imports model)
        {
            return model.C126;
        }

        public static void Master_ItemStatus_Index(this _Prepare_Imports model, string value)
        {
            model.C126 = value;
        }

        public static string Master_ItemStatus_Id(this _Prepare_Imports model)
        {
            return model.C127;
        }

        public static void Master_ItemStatus_Id(this _Prepare_Imports model, string value)
        {
            model.C127 = value;
        }

        public static string Master_ItemStatus_Name(this _Prepare_Imports model)
        {
            return model.C128;
        }

        public static void Master_ItemStatus_Name(this _Prepare_Imports model, string value)
        {
            model.C128 = value;
        }



        public static string Master_Product_Id(this _Prepare_Imports model)
        {
            return model.C131;
        }

        public static void Master_Product_Id(this _Prepare_Imports model, string value)
        {
            model.C131 = value;
        }

        public static string Master_Product_Name(this _Prepare_Imports model)
        {
            return model.C132;
        }

        public static void Master_Product_Name(this _Prepare_Imports model, string value)
        {
            model.C132 = value;
        }

        public static string Master_Product_Name2(this _Prepare_Imports model)
        {
            return model.C133;
        }

        public static void Master_Product_Name2(this _Prepare_Imports model, string value)
        {
            model.C133 = value;
        }

        public static string Master_Product_Name3(this _Prepare_Imports model)
        {
            return model.C134;
        }

        public static void Master_Product_Name3(this _Prepare_Imports model, string value)
        {
            model.C134 = value;
        }

        public static string Master_ProductConversion_Id(this _Prepare_Imports model)
        {
            return model.C135;
        }

        public static void Master_ProductConversion_Id(this _Prepare_Imports model, string value)
        {
            model.C135 = value;
        }

        public static string Master_ProductConversion_Name(this _Prepare_Imports model)
        {
            return model.C136;
        }

        public static void Master_ProductConversion_Name(this _Prepare_Imports model, string value)
        {
            model.C136 = value;
        }


        public static string Master_ProductConversion_Weight(this _Prepare_Imports model)
        {
            return model.C137;
        }

        public static void Master_ProductConversion_Weight(this _Prepare_Imports model, string value)
        {
            model.C137 = value;
        }

        public static string Master_ProductConversion_Weight_Index(this _Prepare_Imports model)
        {
            return model.C138;
        }

        public static void Master_ProductConversion_Weight_Index(this _Prepare_Imports model, string value)
        {
            model.C138 = value;
        }

        public static string Master_ProductConversion_Weight_Id(this _Prepare_Imports model)
        {
            return model.C139;
        }

        public static void Master_ProductConversion_Weight_Id(this _Prepare_Imports model, string value)
        {
            model.C139 = value;
        }

        public static string Master_ProductConversion_Weight_Name(this _Prepare_Imports model)
        {
            return model.C140;
        }

        public static void Master_ProductConversion_Weight_Name(this _Prepare_Imports model, string value)
        {
            model.C140 = value;
        }

        public static string Master_ProductConversion_WeightRatio(this _Prepare_Imports model)
        {
            return model.C141;
        }

        public static void Master_ProductConversion_WeightRatio(this _Prepare_Imports model, string value)
        {
            model.C141 = value;
        }


        public static string Master_ProductConversion_GrsWeight(this _Prepare_Imports model)
        {
            return model.C142;
        }

        public static void Master_ProductConversion_GrsWeight(this _Prepare_Imports model, string value)
        {
            model.C142 = value;
        }

        public static string Master_ProductConversion_GrsWeight_Index(this _Prepare_Imports model)
        {
            return model.C143;
        }

        public static void Master_ProductConversion_GrsWeight_Index(this _Prepare_Imports model, string value)
        {
            model.C143 = value;
        }

        public static string Master_ProductConversion_GrsWeight_Id(this _Prepare_Imports model)
        {
            return model.C144;
        }

        public static void Master_ProductConversion_GrsWeight_Id(this _Prepare_Imports model, string value)
        {
            model.C144 = value;
        }

        public static string Master_ProductConversion_GrsWeight_Name(this _Prepare_Imports model)
        {
            return model.C145;
        }

        public static void Master_ProductConversion_GrsWeight_Name(this _Prepare_Imports model, string value)
        {
            model.C145 = value;
        }

        public static string Master_ProductConversion_GrsWeightRatio(this _Prepare_Imports model)
        {
            return model.C146;
        }

        public static void Master_ProductConversion_GrsWeightRatio(this _Prepare_Imports model, string value)
        {
            model.C146 = value;
        }


        public static string Master_ProductConversion_Width(this _Prepare_Imports model)
        {
            return model.C147;
        }

        public static void Master_ProductConversion_Width(this _Prepare_Imports model, string value)
        {
            model.C147 = value;
        }

        public static string Master_ProductConversion_Width_Index(this _Prepare_Imports model)
        {
            return model.C148;
        }

        public static void Master_ProductConversion_Width_Index(this _Prepare_Imports model, string value)
        {
            model.C148 = value;
        }

        public static string Master_ProductConversion_Width_Id(this _Prepare_Imports model)
        {
            return model.C149;
        }

        public static void Master_ProductConversion_Width_Id(this _Prepare_Imports model, string value)
        {
            model.C149 = value;
        }

        public static string Master_ProductConversion_Width_Name(this _Prepare_Imports model)
        {
            return model.C150;
        }

        public static void Master_ProductConversion_Width_Name(this _Prepare_Imports model, string value)
        {
            model.C150 = value;
        }

        public static string Master_ProductConversion_WidthRatio(this _Prepare_Imports model)
        {
            return model.C151;
        }

        public static void Master_ProductConversion_WidthRatio(this _Prepare_Imports model, string value)
        {
            model.C151 = value;
        }


        public static string Master_ProductConversion_Length(this _Prepare_Imports model)
        {
            return model.C152;
        }

        public static void Master_ProductConversion_Length(this _Prepare_Imports model, string value)
        {
            model.C152 = value;
        }

        public static string Master_ProductConversion_Length_Index(this _Prepare_Imports model)
        {
            return model.C153;
        }

        public static void Master_ProductConversion_Length_Index(this _Prepare_Imports model, string value)
        {
            model.C153 = value;
        }

        public static string Master_ProductConversion_Length_Id(this _Prepare_Imports model)
        {
            return model.C154;
        }

        public static void Master_ProductConversion_Length_Id(this _Prepare_Imports model, string value)
        {
            model.C154 = value;
        }

        public static string Master_ProductConversion_Length_Name(this _Prepare_Imports model)
        {
            return model.C155;
        }

        public static void Master_ProductConversion_Length_Name(this _Prepare_Imports model, string value)
        {
            model.C155 = value;
        }

        public static string Master_ProductConversion_LengthRatio(this _Prepare_Imports model)
        {
            return model.C156;
        }

        public static void Master_ProductConversion_LengthRatio(this _Prepare_Imports model, string value)
        {
            model.C156 = value;
        }


        public static string Master_ProductConversion_Height(this _Prepare_Imports model)
        {
            return model.C157;
        }

        public static void Master_ProductConversion_Height(this _Prepare_Imports model, string value)
        {
            model.C157 = value;
        }

        public static string Master_ProductConversion_Height_Index(this _Prepare_Imports model)
        {
            return model.C158;
        }

        public static void Master_ProductConversion_Height_Index(this _Prepare_Imports model, string value)
        {
            model.C158 = value;
        }

        public static string Master_ProductConversion_Height_Id(this _Prepare_Imports model)
        {
            return model.C159;
        }

        public static void Master_ProductConversion_Height_Id(this _Prepare_Imports model, string value)
        {
            model.C159 = value;
        }

        public static string Master_ProductConversion_Height_Name(this _Prepare_Imports model)
        {
            return model.C160;
        }

        public static void Master_ProductConversion_Height_Name(this _Prepare_Imports model, string value)
        {
            model.C160 = value;
        }

        public static string Master_ProductConversion_HeightRatio(this _Prepare_Imports model)
        {
            return model.C161;
        }

        public static void Master_ProductConversion_HeightRatio(this _Prepare_Imports model, string value)
        {
            model.C161 = value;
        }


        public static string Master_ProductConversion_Volume(this _Prepare_Imports model)
        {
            return model.C162;
        }

        public static void Master_ProductConversion_Volume(this _Prepare_Imports model, string value)
        {
            model.C162 = value;
        }

        public static string Master_ProductConversion_Volume_Index(this _Prepare_Imports model)
        {
            return model.C163;
        }

        public static void Master_ProductConversion_Volume_Index(this _Prepare_Imports model, string value)
        {
            model.C163 = value;
        }

        public static string Master_ProductConversion_Volume_Id(this _Prepare_Imports model)
        {
            return model.C164;
        }

        public static void Master_ProductConversion_Volume_Id(this _Prepare_Imports model, string value)
        {
            model.C164 = value;
        }

        public static string Master_ProductConversion_Volume_Name(this _Prepare_Imports model)
        {
            return model.C165;
        }

        public static void Master_ProductConversion_Volume_Name(this _Prepare_Imports model, string value)
        {
            model.C165 = value;
        }

        public static string Master_ProductConversion_VolumeRatio(this _Prepare_Imports model)
        {
            return model.C166;
        }

        public static void Master_ProductConversion_VolumeRatio(this _Prepare_Imports model, string value)
        {
            model.C166 = value;
        }

        public static string Format_PO_Date(this _Prepare_Imports model)
        {
            return model.C167;
        }

        public static void Format_PO_Date(this _Prepare_Imports model, string value)
        {
            model.C167 = value;
        }

        public static string Format_Due_Date(this _Prepare_Imports model)
        {
            return model.C168;
        }

        public static void Format_Due_Date(this _Prepare_Imports model, string value)
        {
            model.C168 = value;
        }

    }
}