using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace FamilyHubs.OR.Umbraco;

public interface IUmbracoDataTypeLoader
{
    Task<IDataType> Textarea();
    Task<IDataType> Numeric();
    Task<IDataType> DatePickerWithTime();
    Task<IDataType> ContentPicker();
    Task<IDataType> DocumentPicker();
}

public class UmbracoDataTypeLoader(IDataTypeService dataTypeService) : IUmbracoDataTypeLoader
{
    public async Task<IDataType> Textarea() => (await dataTypeService.GetByEditorAliasAsync("Umbraco.TextArea")).First();
    public async Task<IDataType> Numeric() => (await dataTypeService.GetByEditorAliasAsync("Umbraco.Integer")).First();
    public async Task<IDataType> DatePickerWithTime() => (await dataTypeService.GetByEditorAliasAsync("Umbraco.DateTime")).First();
    public async Task<IDataType> ContentPicker() => (await dataTypeService.GetByEditorAliasAsync("Umbraco.ContentPicker")).First();
    public async Task<IDataType> DocumentPicker() => (await dataTypeService.GetByEditorAliasAsync("Umbraco.DocumentPicker")).First();
}

