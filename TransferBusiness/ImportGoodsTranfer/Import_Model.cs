using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransferDataAccess.Models;
using TransferBusiness;

namespace Business.Models
{
    public class Import_Model
    {
        public string Import_Type { get; set; }

        public string Import_By { get; set; }

        public string Import_FileName { get; set; }

        public string Import_Case { get; set; }

        public List<ColumnDetailModel> Columns { get; set; }

        public List<_Prepare_Imports> Data { get; set; }

        public class ColumnDetailModel
        {
            public string Column { get; set; }

            public string Header { get; set; }
        }
    }

    public class ConfirmImport_Model
    {
        public string Confirm_By { get; set; }

        public Guid? Import_Index { get; set; }
    }

    public class Valid_ImportModel
    {
        public Guid Model_Index { get; set; }

        public _Prepare_Imports Data { get; set; }
    }

    public class Invalid_ImportModel
    {
        public Guid Model_Index { get; set; }

        public List<ColumnDetailModel> Errors { get; set; } = new List<ColumnDetailModel>();

        public class ColumnDetailModel
        {
            public string Column { get; set; }

            public string Header { get; set; }

            public string Message { get; set; }
        }
    }

    public class Validated_ImportModel : Result
    {
        public Guid Import_GuID { get; set; }

        public List<dynamic> Valid_Data { get; set; } = new List<dynamic>();

        public List<dynamic> Invalid_Data { get; set; } = new List<dynamic>();
    }

    public static class ImportExtensions
    {
        public static Import_Model GetImportModel(string jsonData)
        {
            Import_Model model;
            try
            {
                model = JsonConvert.DeserializeObject<Import_Model>(jsonData ?? string.Empty);
                if (model is null)
                {
                    throw new Exception("Invalid Import Model is NULL");
                }

                if (model.Columns is null || model.Columns.Count == 0)
                {
                    throw new Exception("Invalid Import Model : Column not found");
                }

                if (model.Data is null || model.Data.Count == 0)
                {
                    throw new Exception("Invalid Import Model : Data not found");
                }
                
                if (model.Import_By.IsNull())
                {
                    throw new Exception("Invalid Import Model : Imports By not found");
                }

                if (model.Import_FileName.IsNull())
                {
                    throw new Exception("Invalid Import Model : Imports FileName not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        public static ConfirmImport_Model GetConfirmImportModel(string jsonData)
        {
            ConfirmImport_Model model;
            try
            {
                model = JsonConvert.DeserializeObject<ConfirmImport_Model>(jsonData ?? string.Empty);
                if (model is null)
                {
                    throw new Exception("Invalid ConfrimImport Model is NULL");
                }

                if (!model.Import_Index.HasValue)
                {
                    throw new Exception("Invalid ConfirmImport Model : Import Index not found");
                }

                if (model.Confirm_By.IsNull())
                {
                    throw new Exception("Invalid ConfirmImport Model : Confirm by not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        public static Import_Model.ColumnDetailModel FindColumn(this Import_Model model, string key)
        {
            Import_Model.ColumnDetailModel result = model.Columns.FirstOrDefault(w => w.Column?.Trim().ToLower() == key?.Trim().ToLower());
            if (result is null) { throw new Exception($"Column : [ {key} ] not found"); }
            return result;
        }

        public static void AddError(this _Prepare_Imports model, string message, bool forceReplace = false)
        {
            if (model.Import_Status != -1 || forceReplace)
            {
                model.Import_Message = message;
                model.Import_Status = -1;
            }
        }

        public static bool RemoveError(this List<_Prepare_Imports> model)
        {
            model.RemoveAll(r => r.Import_Status == 1);
            return model.Count > 0;
        }

        public static void AddError(this List<Invalid_ImportModel> models, ref List<Valid_ImportModel> valid_model, Func<Valid_ImportModel, bool> predicate, Import_Model.ColumnDetailModel column, string message)
        {
            Invalid_ImportModel invalid_model;
            bool isNewModel;
            valid_model.Where(predicate).ToList().ForEach(e =>
            {
                e.Data.AddError(message);
                invalid_model = models.FirstOrDefault(w => w.Model_Index == e.Model_Index) ?? new Invalid_ImportModel() { Model_Index = e.Model_Index };
                isNewModel = invalid_model.Errors.Count == 0;
                invalid_model.Errors.Add(new Invalid_ImportModel.ColumnDetailModel() { Column = column.Column, Header = column.Header, Message = message });
                if (isNewModel) { models.Add(invalid_model); };
            });
        }

        public static string TrimWithNull(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        public static bool IsNull(this string value)
        {
            return string.IsNullOrEmpty((value ?? string.Empty).Trim());
        }

        public static bool IsNotNull(this string value)
        {
            return !string.IsNullOrEmpty((value ?? string.Empty).Trim());
        }

        public static DateTime? ConvertToDateTime(this string value)
        {
            return DateTime.TryParse((value ?? string.Empty).Trim(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"), System.Globalization.DateTimeStyles.None, out DateTime date) ? (DateTime?)date : null;
        }

        public static string ConvertToDateString(this string value, string formatDate)
        {
            DateTime? date = ConvertToDateTime(value);
            return date.HasValue ? date.Value.ToString(formatDate) : null;
        }

        public static bool IsDate(this string value)
        {
            return ConvertToDateTime(value).HasValue;
        }

        public static decimal? ConvertToDecimal(this string value, decimal? defaultValue = null)
        {
            return decimal.TryParse((value ?? string.Empty).Trim(), out decimal dec) ? (decimal?)dec : defaultValue;
        }

        public static string ConvertToString(this decimal? value, string defaultValue = null)
        {
            return value.HasValue ? value.Value.ToString() : defaultValue;
        }

        public static string ConvertToString(this Guid? value, string defaultValue = null)
        {
            return value.HasValue ? value.Value.ToString() : defaultValue;
        }

        public static Guid? ConvertToGuid(this string value)
        {
            return Guid.TryParse((value ?? string.Empty).Trim(), out Guid ret) ? (Guid?)ret : null;
        }

        public static bool IsNumber(this string value)
        {
            return ConvertToDecimal(value).HasValue;
        }
    }
}
