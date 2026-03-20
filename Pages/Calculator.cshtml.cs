using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SvsWebApp.Models;
using SvsWebApp.Services;

namespace SvsWebApp.Pages;

public class CalculatorModel : PageModel
{
    private readonly PlayerEntryStore _store;

    public CalculatorModel(PlayerEntryStore store)
    {
        _store = store;
    }

    [BindProperty]
    public CalculatorInput Input { get; set; } = new();

    [BindProperty]
    public string SubmittedBy { get; set; } = string.Empty;

    [BindProperty]
    public string Notes { get; set; } = string.Empty;

    [BindProperty]
    public bool SaveEntry { get; set; } = true;

    [BindProperty]
    public IFormFile? Screenshot { get; set; }

    public CalculatorResult? Result { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        try
        {
            Result = CalculatorService.Calculate(Input);

            if (SaveEntry)
            {
                var screenshotPath = await _store.SaveScreenshotAsync(Screenshot);
                var entry = new SavedPlayerEntry
                {
                    SubmittedBy = SubmittedBy?.Trim() ?? string.Empty,
                    Notes = Notes?.Trim() ?? string.Empty,
                    Input = Input,
                    Result = Result,
                    ScreenshotPath = screenshotPath
                };
                _store.Save(entry);
                SuccessMessage = "Player was calculated and saved.";
            }
            else
            {
                SuccessMessage = "Player was calculated.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
