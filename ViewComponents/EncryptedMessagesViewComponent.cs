using InfoProtection.Models;
using InfoProtection.Servises;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EncryptedMessagesViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public EncryptedMessagesViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        DbAccess dbAccess = new(_context);
        var encryptedMessages = await dbAccess.GetAllEnc();
        return View(encryptedMessages);
    }
}
