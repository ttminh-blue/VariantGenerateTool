using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Tool
{
    public partial class Form1 : Form
    {
        private DataSourceInfo datasource = new DataSourceInfo();
        public class DataSourceInfo
        {
            public string DatasourceName { get; set; }
            public string DatasourceGroupName { get; set; }
            public string CommandName { get; set; }
            public string DatasourceID { get; set; }
            public string Countries { get; set; }
            public List<VariantModel> ListVariants { get; set; }
            public List<DatasourceConfigModel> DatasourceConfigs { get; set; }
        }
        public class VariantModel
        {
            public string VariantName { get; set; }
            public string NameFormat { get; set; }
            public string AddressFormat { get; set; }
            public string Note { get; set; }
            public bool Priority { get; set; }
            public List<FieldInfo> Fields { get; set; }
        }
        public class FieldInfo
        {
            public string FieldName { get; set; }
            public bool RequiredInput { get; set; }
            public bool OptionalInput { get; set; }
            public bool Output { get; set; }
            public bool Appended { get; set; }
            public string Comments { get; set; }
        }
        public class DatasourceConfigModel
        {
            public string ConfigurationParameter { get; set; }
            public string ConfigurationValue { get; set; }
            public string Notes { get; set; }
        }

        public class ConfigModel
        {
            public string Email { get; set; }
            public string ApiToken { get; set; }
        }

        private bool convertStringToBool(string value)
        {
            if (value.ToLower().Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private string GenerateDatasourceClass(DataSourceInfo ds)
        {
            var sb = new StringBuilder();

            string className = ds.CommandName?.Replace(" ", "") ?? "GeneratedClass";

            sb.AppendLine($"public class {className} : CreateDatasourceRequest");
            sb.AppendLine("{");
            sb.AppendLine($"    private const int _datasourceID        = DatasourceIds.{className};");
            sb.AppendLine($"    private const int _commandID          = CommandTypeIds.{className};");
            sb.AppendLine($"    private const string _datasourceGroupName = \"{ds.DatasourceGroupName}\";");
            sb.AppendLine($"    private const string _datasourceName      = \"{ds.DatasourceName}\";");
            sb.AppendLine($"    private const string _commandName         = \"{ds.CommandName}\";");
            sb.AppendLine($"    private static readonly int[] _countryIDs = [(int)CountryEnum.{ds.Countries}];");
            sb.AppendLine();

            int variantIndex = 1;
            foreach (var variant in ds.ListVariants)
            {
                string noteComment = !string.IsNullOrWhiteSpace(variant.Note)
                    ? $"// Variant {variantIndex}: {variant.Note}"
                    : $"// Variant {variantIndex}";

                sb.AppendLine($"    {noteComment}");
                sb.AppendLine($"    private readonly List<(int fieldId, bool isRequired, bool isOptional, bool isOutput, bool isAppended)> _fieldsVariant{variantIndex} =");
                sb.AppendLine("    [");

                int maxFieldNameLen = variant.Fields.Max(f => f.FieldName.Length);

                foreach (var field in variant.Fields)
                {
                    string fieldName = field.FieldName.PadRight(maxFieldNameLen);
                    string required = field.RequiredInput ? "true " : "false";
                    string optional = field.OptionalInput ? "true " : "false";
                    string output = field.Output ? "true " : "false";
                    string appended = field.Appended ? "true " : "false";

                    string comment = !string.IsNullOrWhiteSpace(field.Comments) ? $" // {field.Comments}" : "";

                    sb.AppendLine(
                        $"        ((int)FieldEnum.{fieldName}, {required}, {optional}, {output}, {appended}), {comment}"
                    );
                }

                sb.AppendLine("    ];");
                sb.AppendLine();
                variantIndex++;
            }

            sb.AppendLine($"    public {className}()");
            sb.AppendLine("    {");
            sb.AppendLine("        IAppSettingsHelper appSettingsHelper = Gateway.Common.ObjectFactory.GetInstance<IAppSettingsHelper>();");
            sb.AppendLine("        Datasource = new DatasourceIdentificationData");
            sb.AppendLine("        {");
            sb.AppendLine($"            Credential = new CredentialData");
            sb.AppendLine("            {");
            sb.AppendLine($"                UserName        = appSettingsHelper.GetString(\"{className}.Username\"),");
            sb.AppendLine($"                TestUserName   = appSettingsHelper.GetString(\"{className}.TestUsername\"),");
            sb.AppendLine($"                EndpointUrl    = appSettingsHelper.GetString(\"{className}.EndpointUrl\"),");
            sb.AppendLine($"                TestEndpointUrl= appSettingsHelper.GetString(\"{className}.TestEndpointUrl\"),");
            sb.AppendLine($"                Password       = appSettingsHelper.GetString(\"{className}.Password\"),");
            sb.AppendLine($"                TestPassword   = appSettingsHelper.GetString(\"{className}.TestPassword\"),");
            sb.AppendLine($"                CredentialFormat = string.Empty");
            sb.AppendLine("            },");
            sb.AppendLine("            DatasourceId    = _datasourceID,");
            sb.AppendLine("            CommandId       = _commandID,");
            sb.AppendLine("            DatasourceName  = _datasourceName,");
            sb.AppendLine("            CommandName     = _commandName,");
            sb.AppendLine("            Industries      = Enum.GetValues(typeof(IndustryEnum)).Cast<IndustryEnum>().Select(x => (int)x).ToArray(),");
            sb.AppendLine("            ProductList     = [(int)ProductEnum.IdentityVerification],");
            sb.AppendLine("            IsTestable      = true,");
            sb.AppendLine("            AllowAppendData = true,");
            sb.AppendLine($"            CountryFields   = new Dictionary<int, int[]> {{ {{ (int)CountryEnum.{ds.Countries}, [] }} }}");
            sb.AppendLine("        };");
            sb.AppendLine();

            sb.AppendLine("        DatasourceGroupVariants =");
            sb.AppendLine("        [");

            for (int i = 0; i < ds.ListVariants.Count; i++)
            {
                var variant = ds.ListVariants[i];
                var fieldVar = $"_fieldsVariant{i + 1}";
                string addressFormat = variant.AddressFormat?.ToLower().Contains("address1") == true ? "Address1" : "Segmented";
                string nameFormat = variant.NameFormat?.ToLower().Contains("segmented") == true ? "Segmented" : "Segmented";
                string note = variant.Note?.Replace("\"", "\\\"") ?? "";

                sb.AppendLine("            new DynamicDatasourceGroupVariantData");
                sb.AppendLine("            {");
                sb.AppendLine("                DatasourceGroupName = _datasourceGroupName,");
                sb.AppendLine("                DatasourceId        = _datasourceID,");
                sb.AppendLine($"                Fields              = {fieldVar}.Select(x => x.fieldId).ToArray(),");
                sb.AppendLine("                CountryIds          = _countryIDs,");
                sb.AppendLine($"                RequiredFields      = [..{fieldVar}.Where(f => f.isRequired).Select(x => x.fieldId)],");
                sb.AppendLine($"                OptionalFields      = [..{fieldVar}.Where(f => f.isOptional).Select(x => x.fieldId)],");
                sb.AppendLine($"                OutputFields        = [..{fieldVar}.Where(f => f.isOutput).Select(x => x.fieldId)],");
                sb.AppendLine($"                AppendedFields      = [..{fieldVar}.Where(f => f.isAppended).Select(x => x.fieldId)],");
                sb.AppendLine("                SourceType          = _datasourceGroupName,");
                string parsedPriority = variant.Priority ? "true" : "false";
                sb.AppendLine($"                Priority            = {parsedPriority},");
                sb.AppendLine($"                AddressFormat       = (int)AddressFormatEnum.{addressFormat},");
                sb.AppendLine($"                NameFormat          = (int)NameFormatEnum.{nameFormat},");
                sb.AppendLine($"                Notes               = \"{note}\"");
                sb.Append(i == ds.ListVariants.Count - 1 ? "            }" : "            },");
            }

            sb.AppendLine("        ];");

            if (datasource.DatasourceConfigs?.Any() == true)
            {
                sb.AppendLine();
                sb.AppendLine("        DatasourceConfigurationParameters =");
                sb.AppendLine("        [");

                foreach (var config in datasource.DatasourceConfigs)
                {
                    sb.AppendLine("            new DatasourceConfigurationParameterData");
                    sb.AppendLine("            {");
                    sb.AppendLine($"                DatasourceId = _datasourceID,");
                    sb.AppendLine($"                Name = \"{config.ConfigurationParameter}\",");
                    sb.AppendLine($"                CountryId = (int)CountryEnum.{ds.Countries},");
                    sb.AppendLine($"                Value = \"{config.ConfigurationValue}\",");
                    sb.AppendLine($"                Note = \"{config.Notes.Replace("\"", "\\\"")}\"");
                    sb.AppendLine("            },");
                }

                sb.AppendLine("        ];");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private DataSourceInfo ParseDatasourceInfo(HtmlAgilityPack.HtmlDocument doc)
        {
            var info = new DataSourceInfo();

            var table = doc.DocumentNode.SelectSingleNode("//table[.//th[contains(text(), 'Vendor Name')]]");

            if (table == null)
                return info;

            var rows = table.SelectNodes(".//tr");
            if (rows == null)
                return info;

            foreach (var row in rows)
            {
                var th = row.SelectSingleNode(".//th");
                var td = row.SelectSingleNode(".//td");

                if (th == null || td == null) continue;

                string key = HtmlEntity.DeEntitize(th.InnerText.Trim());
                string value = HtmlEntity.DeEntitize(td.InnerText.Trim());

                switch (key.ToLowerInvariant())
                {
                    case var s when s.Contains("internal datasource name"):
                        info.DatasourceName = value;
                        break;

                    case var s when s.Contains("public datasource group name"):
                        info.DatasourceGroupName = value;
                        break;

                    case "command name":
                        info.CommandName = value;
                        break;

                    case "datasource id":
                        info.DatasourceID = value;
                        break;

                    case "countries":
                        info.Countries = value;
                        break;
                }
            }

            return info;
        }
        private List<DatasourceConfigModel> ParseDatasourceConfig(HtmlAgilityPack.HtmlDocument doc)
        {
            var result = new List<DatasourceConfigModel>();

            var tableNode = doc.DocumentNode.SelectSingleNode("//table[.//th[contains(text(), 'Configuration Parameter')]]");
            if (tableNode == null) return result;

            var rowNodes = tableNode.SelectNodes(".//tr");
            if (rowNodes == null) return result;

            foreach (var row in rowNodes.Skip(1))
            {
                var cells = row.SelectNodes(".//td");
                if (cells == null || cells.Count < 3)
                    continue;

                var config = new DatasourceConfigModel
                {
                    ConfigurationParameter = HtmlEntity.DeEntitize(cells[0].InnerText.Trim()),
                    ConfigurationValue = HtmlEntity.DeEntitize(cells[1].InnerText.Trim()),
                    Notes = HtmlEntity.DeEntitize(cells[2].InnerText.Trim())
                };

                result.Add(config);
            }

            return result;
        }

        private List<VariantModel> GetVariants(HtmlAgilityPack.HtmlDocument doc)
        {
            var variants = new List<VariantModel>();
            var variantNodes = doc.DocumentNode.SelectNodes("//*[contains(text(), 'Variant #')]");

            if (variantNodes == null) return variants;

            foreach (var variantNode in variantNodes)
            {
                var variant = new VariantModel();
                variant.VariantName = variantNode.InnerText.Trim();
                if (!variant.VariantName.StartsWith("Variant"))
                    continue;

                HtmlNode? metaTable = doc.DocumentNode
                    .SelectNodes("//table[.//th[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 'informationtype')]]")
                    ?.OrderBy(t => Math.Abs(t.StreamPosition - variantNode.StreamPosition))
                    .FirstOrDefault();

                ParseMetadata(metaTable, variant);

                var fieldTables = doc.DocumentNode
                                 .SelectNodes("//table[.//th[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 'fieldname') or contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 'view name') or contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 'gg field')]]");

                var table = fieldTables?
                    .Where(t => t.StreamPosition > variantNode.StreamPosition)
                    .OrderBy(t => t.StreamPosition)
                    .FirstOrDefault();

                variant.Fields = ParseFieldTable(table);

                variants.Add(variant);
            }
            return variants;
        }

        private void ParseMetadata(HtmlNode metaTable, VariantModel variant)
        {
            if (metaTable == null) return;

            var rows = metaTable.SelectNodes(".//tr");
            if (rows == null) return;

            foreach (var row in rows.Skip(1))
            {
                var cells = row.SelectNodes("td");
                if (cells == null || cells.Count < 2) continue;

                string key = HtmlEntity.DeEntitize(cells[0].InnerText.Trim()).ToLowerInvariant();
                string value = HtmlEntity.DeEntitize(cells[1].InnerText.Trim());

                switch (key)
                {
                    case "name format":
                        variant.NameFormat = value;
                        break;
                    case "address format":
                        variant.AddressFormat = value;
                        break;
                    case "product type":
                        break;
                    case "note":
                        variant.Note = value;
                        break;
                    case "priority":
                        variant.Priority = convertStringToBool(value);
                        break;
                }
            }
        }

        private List<FieldInfo> ParseFieldTable(HtmlNode? table)
        {
            var fields = new List<FieldInfo>();

            if (table == null)
                return fields;

            var rows = table.SelectNodes(".//tr");
            if (rows == null || rows.Count <= 1)
                return fields;

            foreach (var row in rows.Skip(1))
            {
                var cells = row.SelectNodes("td|th");
                if (cells == null || cells.Count < 2) continue;

                string GetCell(int index)
                {
                    return index < cells.Count ? HtmlEntity.DeEntitize(cells[index].InnerText.Trim()) : string.Empty;
                }

                var field = new FieldInfo
                {
                    FieldName = GetCell(1),
                    RequiredInput = convertStringToBool(GetCell(2)),
                    OptionalInput = convertStringToBool(GetCell(3)),
                    Output = convertStringToBool(GetCell(4)),
                    Appended = convertStringToBool(GetCell(5)),
                    Comments = GetCell(6)
                };

                fields.Add(field);
            }

            return fields;
        }
        private ConfigModel? LoadConfig(string path = "config.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Config file not found");

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ConfigModel>(json);
        }
        public Form1()
        {
            InitializeComponent();
            txtContent.Font = new Font("Consolas", 10);
        }

        private async void btnClick_Click(object sender, EventArgs e)
        {
            string confluenceUrl = txtUrl.Text;
            var config = LoadConfig();
            string? username = config?.Email;
            string? apiToken = config?.ApiToken;

            string basicAuth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{apiToken}"));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(confluenceUrl);
                var content = await response.Content.ReadAsStringAsync();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(content);
                datasource = ParseDatasourceInfo(doc);
                var listVariants = GetVariants(doc);
                datasource.ListVariants = listVariants;
                var configs = ParseDatasourceConfig(doc);
                datasource.DatasourceConfigs = configs;
                string text = GenerateDatasourceClass(datasource);
                txtContent.Text = text;
            }
        }

        private void txtCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtContent.Text);
            MessageBox.Show("Copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}